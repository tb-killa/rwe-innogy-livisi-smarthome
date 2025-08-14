using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Ionic.Zip;
using JsonLite;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

namespace RWE.SmartHome.SHC.ApplicationsHost;

public class AddinsRepoManager : IAddinRepoManager
{
	private const string InstalledAddinsRepositoryPath = "\\NandFlash\\Apps";

	private const string CoreDeliveredAddinsPath = "\\NandFlash\\SHC\\addins";

	private const string ManifestFileEnding = ".manifest.xml";

	private const string AddinZipFilePattern = "[a-zA-Z.]+-V?\\d+.\\d+.\\d+.\\d+.zip";

	private const string PersistenceFile = "\\NandFlash\\Apps\\addins.xml";

	private const string addinUsbPath = "\\Hard Disk\\update\\binding";

	private const string addinTempPath = "\\NandFlash\\Temp\\Apps\\";

	private const string SunriseSunsetSensor = "SunriseSunsetSensor";

	private const string ApplicationVersion = "2.0";

	private const string RWE = ".RWE";

	private const string VariableActuator = "VariableActuator";

	private readonly XmlSerializer serializer = new XmlSerializer(typeof(List<ApplicationTokenEntry>));

	private JsonSerializer serilizer = new JsonSerializer();

	private JsonDeserializer deserializer = new JsonDeserializer();

	private List<ApplicationTokenEntry> addins;

	private readonly IApplicationsTokenPersistence applicationsTokenPersistence;

	private readonly ITokenCache tokenCache;

	private readonly IRegistrationService registrationService;

	private readonly IEventManager eventManager;

	public string AppIdPrefix { get; private set; }

	public AddinsRepoManager(IApplicationsTokenPersistence applicationsTokenPersistence, ITokenCache tokenCache, IRegistrationService registrationService, IEventManager eventManager)
	{
		AppIdPrefix = "sh://";
		this.applicationsTokenPersistence = applicationsTokenPersistence;
		this.tokenCache = tokenCache;
		this.registrationService = registrationService;
		this.eventManager = eventManager;
		eventManager.GetEvent<DeleteAddinsFromPersistenceFileEvent>().Subscribe(OnDeleteAddinsFromPersistenceFile, null, ThreadOption.PublisherThread, null);
		SaveApplicationsTokenHash();
		if (!Directory.Exists("\\NandFlash\\Apps"))
		{
			Directory.CreateDirectory("\\NandFlash\\Apps");
		}
		if (!File.Exists("\\NandFlash\\Apps\\addins.xml"))
		{
			addins = new List<ApplicationTokenEntry>();
			SaveChanges();
		}
		else
		{
			LoadAddinsFromPersistence();
		}
		AddVariableActuatorAndSunriseSunsetInPersistence();
	}

	public List<AddinUpdateData> GetAddinsToRemove(ApplicationsToken appsToken)
	{
		List<AddinUpdateData> list = new List<AddinUpdateData>();
		foreach (ApplicationTokenEntry item in addins.ToList())
		{
			string text = item.AppId.Remove(0, AppIdPrefix.Length);
			string text2 = Path.Combine("\\Hard Disk\\update\\binding", text);
			if (!Directory.Exists(text2))
			{
				continue;
			}
			List<string> source = Directory.GetFiles(text2).Where(IsManifestFile).ToList();
			string text3 = source.FirstOrDefault();
			ManifestReader manifestReader = new ManifestReader(text3);
			Version version = new Version(manifestReader.Version);
			Version value = new Version(item.Version);
			int num = version.CompareTo(value);
			if (num <= 0)
			{
				Log.Debug(Module.ApplicationsHost, $"There was no update for the add-in: {item.Name}");
				continue;
			}
			ApplicationTokenEntry applicationTokenEntry = RemoveAddin(item.AppId, appsToken);
			if (applicationTokenEntry == null)
			{
				Log.Information(Module.ApplicationsHost, $"The add-in was not installed: {item.Name}");
				continue;
			}
			AddinUpdateData addinUpdateData = new AddinUpdateData();
			addinUpdateData.Type = text;
			addinUpdateData.ManifestFile = text3;
			addinUpdateData.AddinFilePath = text2;
			addinUpdateData.AppsToken = appsToken;
			list.Add(addinUpdateData);
		}
		return list;
	}

	public ApplicationTokenEntry GetUpdatedAddin(AddinUpdateData addinToUpdate)
	{
		string temporaryAddinPath = CopyUsbFilesToTemporaryFolder(addinToUpdate.Type, "\\NandFlash\\Temp\\Apps\\", addinToUpdate.AddinFilePath);
		LocalAddin localAddin = AddAddin(temporaryAddinPath, addinToUpdate.ManifestFile);
		InstallLocalAddin(localAddin, deleteZipFromTemporarySpace: true);
		ApplicationTokenEntry product = GetAddinTokenEntry(localAddin);
		product.IsEnabledByUser = true;
		product.IsEnabledByWebshop = true;
		if (!addinToUpdate.AppsToken.Entries.Any((ApplicationTokenEntry e) => e.AppId == product.AppId))
		{
			addinToUpdate.AppsToken.Entries.Add(product);
			applicationsTokenPersistence.SaveApplicationsToken(addinToUpdate.AppsToken);
		}
		return product;
	}

	private ApplicationTokenEntry RemoveAddin(string appId, ApplicationsToken token)
	{
		ApplicationTokenEntry application = token.Entries.FirstOrDefault((ApplicationTokenEntry e) => e.AppId == appId);
		if (application == null)
		{
			return null;
		}
		token.Entries.RemoveAll((ApplicationTokenEntry e) => e.AppId == application.AppId);
		RemoveAddinFromPersistenceFile(application);
		token.Hash = token.GetHash();
		applicationsTokenPersistence.SaveApplicationsToken(token);
		return application;
	}

	private void SaveApplicationsTokenHash()
	{
		ApplicationsToken applicationsToken = applicationsTokenPersistence.LoadApplicationsToken();
		if (applicationsToken != null && !string.IsNullOrEmpty(applicationsToken.Hash))
		{
			FilePersistence.ApplicationsTokenHash = applicationsToken.Hash;
		}
	}

	private void AddVariableActuatorAndSunriseSunsetInPersistence()
	{
		if (applicationsTokenPersistence == null)
		{
			Log.Information(Module.ApplicationsHost, "The applicationsTokenPersistence was null when adding the Variable Actuator and SunriseSunset add-in information to add-ins data");
			return;
		}
		ApplicationsToken applicationsToken = applicationsTokenPersistence.LoadApplicationsToken();
		if (applicationsToken != null)
		{
			if (addins.FirstOrDefault((ApplicationTokenEntry addin) => addin.Name == "VariableActuator") == null)
			{
				string appId = AppIdPrefix + "VariableActuator.RWE";
				AddOrUpdatePersistenceFile("VariableActuator", appId, "2.0", isService: false);
			}
			if (addins.FirstOrDefault((ApplicationTokenEntry addin) => addin.Name == "SunriseSunsetSensor") == null)
			{
				string appId2 = AppIdPrefix + "SunriseSunsetSensor.RWE";
				AddOrUpdatePersistenceFile("SunriseSunsetSensor", appId2, "2.0", isService: true);
			}
		}
	}

	public void InstallCoreDeliveredAddinsIfExists(ApplicationsToken installedAddins)
	{
		if (!Directory.Exists("\\NandFlash\\SHC\\addins"))
		{
			return;
		}
		List<string> list = Directory.GetFiles("\\NandFlash\\SHC\\addins").Where(IsManifestFile).ToList();
		List<ApplicationTokenEntry> source = addins.ToList();
		foreach (string item in list)
		{
			ManifestReader manifestReader = new ManifestReader(item);
			ApplicationTokenEntry applicationTokenEntry = installedAddins.Entries.FirstOrDefault((ApplicationTokenEntry addin) => addin.AppId == manifestReader.AppId);
			if (applicationTokenEntry == null)
			{
				LocalAddin localAddin = AddAddin("\\NandFlash\\SHC\\addins", item);
				InstallLocalAddin(localAddin, deleteZipFromTemporarySpace: false);
				ApplicationTokenEntry applicationTokenEntry2 = source.FirstOrDefault((ApplicationTokenEntry addin) => addin.AppId == manifestReader.AppId);
				applicationTokenEntry2.AppManifest = localAddin.DestinationFolderManifestFileName;
				applicationTokenEntry2.IsEnabledByUser = true;
				applicationTokenEntry2.IsEnabledByWebshop = true;
				installedAddins.Entries.Add(applicationTokenEntry2);
				applicationsTokenPersistence.SaveApplicationsToken(installedAddins);
			}
		}
	}

	private void OnDeleteAddinsFromPersistenceFile(DeleteAddinsFromPersistenceFileEventArgs args)
	{
		if (registrationService.IsShcLocalOnly)
		{
			RemoveAllAddinsFromPersistenceFiles();
		}
	}

	private void RemoveAllAddinsFromPersistenceFiles()
	{
		if (FactoryResetHandling.WasFactoryResetRequested(checkFactoryResetButton: false) == FactoryResetRequestedStatus.NotRequested)
		{
			return;
		}
		string text = AppIdPrefix + "VariableActuator.RWE";
		string text2 = AppIdPrefix + "SunriseSunsetSensor.RWE";
		foreach (ApplicationTokenEntry item in addins.ToList())
		{
			if (!(item.AppId == text) && !(item.AppId == text2))
			{
				addins.Remove(item);
			}
		}
		SaveChanges();
	}

	private void LoadAddinsFromPersistence()
	{
		try
		{
			using FileStream stream = new FileStream("\\NandFlash\\Apps\\addins.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
			addins = (List<ApplicationTokenEntry>)serializer.Deserialize(stream);
		}
		catch (Exception ex)
		{
			Log.Error(Module.ApplicationsHost, ex.Message);
			addins = new List<ApplicationTokenEntry>();
			SaveChanges();
		}
	}

	private static bool IsManifestFile(string filename)
	{
		return filename.EndsWith(".manifest.xml");
	}

	private void Remove(ApplicationTokenEntry addinToken)
	{
		addins.RemoveAll((ApplicationTokenEntry a) => a.Name == addinToken.Name && a.AppId == addinToken.AppId);
		SaveChanges();
	}

	private void SaveChanges()
	{
		try
		{
			using FileStream stream = new FileStream("\\NandFlash\\Apps\\addins.xml", FileMode.Create, FileAccess.Write, FileShare.Write);
			serializer.Serialize(stream, addins);
		}
		catch (Exception ex)
		{
			Log.Error(Module.ApplicationsHost, $"{ex.Message} {ex.StackTrace}");
		}
	}

	public void AddOrUpdatePersistenceFile(string name, string appId, string version, bool isService)
	{
		ApplicationTokenEntry applicationTokenEntry = addins.FirstOrDefault((ApplicationTokenEntry a) => a.Name == name && a.AppId == appId);
		if (applicationTokenEntry == null)
		{
			ApplicationTokenEntry applicationTokenEntry2 = new ApplicationTokenEntry();
			applicationTokenEntry2.Name = name;
			applicationTokenEntry2.AppId = appId;
			applicationTokenEntry2.Version = version;
			applicationTokenEntry2.IsService = isService;
			applicationTokenEntry2.ApplicationKind = ApplicationKind.SHCOnly;
			ApplicationTokenEntry applicationTokenEntry3 = applicationTokenEntry2;
			addins.Add(applicationTokenEntry3);
			tokenCache.UpdateApplicationVersionCache(applicationTokenEntry3);
		}
		else
		{
			tokenCache.UpdateApplicationVersionCache(applicationTokenEntry);
			applicationTokenEntry.Version = version;
		}
		SaveChanges();
	}

	public List<ApplicationTokenEntry> GetAllAddinsFromPersistenceFile()
	{
		return addins.ToList();
	}

	public void RemoveAddinFromPersistenceFile(ApplicationTokenEntry addin)
	{
		ApplicationTokenEntry item = addins.FirstOrDefault((ApplicationTokenEntry a) => a.Name == addin.Name && a.AppId == addin.AppId);
		addins.Remove(item);
		SaveChanges();
	}

	public ApplicationTokenEntry GetAddinTokenEntry(LocalAddin localAddin)
	{
		string destinationFolderByAddinInfo = GetDestinationFolderByAddinInfo(localAddin);
		ApplicationTokenEntry applicationTokenEntry = GetAllAddinsFromPersistenceFile().FirstOrDefault((ApplicationTokenEntry addin) => addin.AppId.Equals(localAddin.AppId, StringComparison.InvariantCultureIgnoreCase));
		if (applicationTokenEntry != null)
		{
			string text = applicationTokenEntry.Name + ".manifest.xml";
			string text2 = Path.Combine(destinationFolderByAddinInfo, text);
			if (!File.Exists(text2))
			{
				return null;
			}
			ManifestReader manifestReader = new ManifestReader(text2);
			applicationTokenEntry.AppManifest = text;
			applicationTokenEntry.SHCPackageChecksum = manifestReader.PackageCheckSumString;
		}
		return applicationTokenEntry;
	}

	public LocalAddin GetAddinFormUsbToTemporatyLocation(string type)
	{
		LocalAddin localAddin = null;
		try
		{
			string text = Path.Combine("\\Hard Disk\\update\\binding", type);
			if (Directory.Exists(text))
			{
				string temporaryAddinPath = CopyUsbFilesToTemporaryFolder(type, "\\NandFlash\\Temp\\Apps\\", text);
				string manifestFileFromLocation = GetManifestFileFromLocation(temporaryAddinPath);
				if (manifestFileFromLocation == null)
				{
					Log.Error(Module.ApplicationsHost, $"No manifest files for the add-in: {type}");
					return null;
				}
				return AddAddin(temporaryAddinPath, manifestFileFromLocation);
			}
			return null;
		}
		catch (Exception ex)
		{
			Log.Error(Module.ApplicationsHost, ex.Message);
			return null;
		}
	}

	private string GetManifestFileFromLocation(string temporaryAddinPath)
	{
		List<string> list = Directory.GetFiles(temporaryAddinPath).Where(IsManifestFile).ToList();
		if (list.Count > 1)
		{
			Log.Error(Module.ApplicationsHost, $"There should be only one manifest file, but there are : {list.Count}");
			return null;
		}
		return list.FirstOrDefault();
	}

	private LocalAddin AddAddin(string temporaryAddinPath, string manifest)
	{
		LocalAddin localAddin = new LocalAddin();
		ManifestReader manifestReader = new ManifestReader(manifest);
		string fileName = Path.GetFileName(manifest);
		string name = fileName.Substring(0, fileName.Length - ".manifest.xml".Length);
		string text = Path.Combine(temporaryAddinPath, manifestReader.PackageFileName);
		localAddin.Name = name;
		localAddin.AppId = manifestReader.AppId;
		localAddin.Path = text;
		localAddin.ZipTemporaryPathFolder = temporaryAddinPath;
		localAddin.ManifestTemporatyFilePath = manifest;
		localAddin.ManifestChecksum = manifestReader.PackageCheckSum;
		if (File.Exists(text))
		{
			AddOrUpdatePersistenceFile(name, manifestReader.AppId, manifestReader.Version, isService: false);
		}
		else
		{
			Log.Error(Module.ApplicationsHost, $"Cannot find any zip archive in this path: {text}");
		}
		return localAddin;
	}

	private string CopyUsbFilesToTemporaryFolder(string type, string addinTempPath, string addinFilePath)
	{
		string text = Path.Combine(addinTempPath, type);
		Directory.CreateDirectory(text);
		string[] files = Directory.GetFiles(addinFilePath);
		string[] array = files;
		foreach (string text2 in array)
		{
			string fileName = Path.GetFileName(text2);
			string destFileName = Path.Combine(text, fileName);
			File.Copy(text2, destFileName, overwrite: true);
		}
		return text;
	}

	public void InstallLocalAddin(LocalAddin localAddin, bool deleteZipFromTemporarySpace)
	{
		string destinationFolderByAddinInfo = GetDestinationFolderByAddinInfo(localAddin);
		try
		{
			using (FileStream inputStream = new FileStream(localAddin.Path, FileMode.Open, FileAccess.Read))
			{
				using MD5 mD = MD5.Create();
				byte[] first = mD.ComputeHash(inputStream);
				if (!first.SequenceEqual(localAddin.ManifestChecksum))
				{
					Log.Error(Module.ApplicationsHost, $"The add-in: {localAddin.Name} is not signed");
					throw new AddinNotSignedException($"The add-in:{localAddin.Name} was not signed.");
				}
			}
			Directory.CreateDirectory(destinationFolderByAddinInfo);
			string fileName = Path.GetFileName(localAddin.ManifestTemporatyFilePath);
			File.Copy(destFileName: localAddin.DestinationFolderManifestFileName = Path.Combine(destinationFolderByAddinInfo, fileName), sourceFileName: localAddin.ManifestTemporatyFilePath, overwrite: true);
			ExtractAddinZipArchive(localAddin, destinationFolderByAddinInfo);
		}
		finally
		{
			if (deleteZipFromTemporarySpace)
			{
				File.Delete(localAddin.Path);
			}
		}
	}

	private static void ExtractAddinZipArchive(LocalAddin localAddin, string destinationFolder)
	{
		using ZipFile zipFile = ZipFile.Read(localAddin.Path);
		foreach (ZipEntry item in zipFile)
		{
			string path = Path.Combine(destinationFolder, item.FileName);
			if (File.Exists(path))
			{
				try
				{
					File.Delete(path);
				}
				catch (IOException)
				{
					continue;
				}
			}
			item.Extract(destinationFolder);
		}
	}

	private string GetDestinationFolderByAddinInfo(LocalAddin localAddin)
	{
		ApplicationTokenEntry entry = addins.FirstOrDefault((ApplicationTokenEntry a) => a.Name == localAddin.Name && a.AppId == localAddin.AppId);
		return Path.Combine("\\NandFlash\\Apps", AppModuleHandler.GetLocalFolderName(entry));
	}
}
