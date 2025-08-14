using System;
using System.IO;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHost.Exceptions;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class MainAssemblyDataFactory
{
	private readonly string basePath;

	private readonly AppHostMessageFactory messageFactory;

	private readonly INetworkingMonitor networkMonitor;

	private readonly IAddinRepoManager addinRepo;

	private readonly IRegistrationService registrationService;

	public MainAssemblyDataFactory(string basePath, AppHostMessageFactory messageFactory, INetworkingMonitor networkMonitor, IAddinRepoManager addinRepo, IRegistrationService registrationService)
	{
		this.basePath = basePath;
		this.messageFactory = messageFactory;
		this.networkMonitor = networkMonitor;
		this.addinRepo = addinRepo;
		this.registrationService = registrationService;
	}

	public MainAssemblyData GetMainAssemblyData(ApplicationTokenEntry tokenEntry, bool lastAttempt)
	{
		MainAssemblyData mainAssemblyData = CheckTokenDownloadPath(tokenEntry.AppManifest);
		if (mainAssemblyData != null)
		{
			return mainAssemblyData;
		}
		if (string.IsNullOrEmpty(tokenEntry.AppId))
		{
			return null;
		}
		string text = Path.Combine("\\NandFlash\\Apps", AppModuleHandler.GetLocalFolderName(tokenEntry));
		string text2 = Path.Combine(text, tokenEntry.AppManifest.Substring(tokenEntry.AppManifest.LastIndexOf("/") + 1));
		bool flag = File.Exists(text2);
		if (!flag && !AppModuleHandler.DownloadManifestFile(tokenEntry, text2, text, networkMonitor, addinRepo))
		{
			Log.Error(Module.ApplicationsHost, "Could not download manifest " + tokenEntry.AppManifest);
			return null;
		}
		ManifestReader manifestReader = new ManifestReader(text2);
		string mainAssemblyName = manifestReader.MainAssemblyName;
		if (string.IsNullOrEmpty(mainAssemblyName))
		{
			Log.Error(Module.ApplicationsHost, "Invalid assembly name in manifest.");
			return null;
		}
		if (string.IsNullOrEmpty(tokenEntry.SHCPackageChecksum) || string.Compare(manifestReader.PackageCheckSumString, tokenEntry.SHCPackageChecksum.Trim(), StringComparison.OrdinalIgnoreCase) != 0)
		{
			Log.Warning(Module.ApplicationsHost, string.Format("Checksums do not match. Token: {0}, manifest: {1}", tokenEntry.SHCPackageChecksum ?? "null", (manifestReader.PackageCheckSum == null) ? "null" : manifestReader.PackageCheckSum.ToReadable()));
			messageFactory.CreateInvalidApplicationMessage(tokenEntry);
			File.Delete(text2);
			throw new AppDownloadException(AppDownloadResult.InvalidManifest);
		}
		string packageUrl = $"{tokenEntry.ApplicationUrl}/{tokenEntry.Version}/{manifestReader.PackageFileName}";
		string[] array = mainAssemblyName.Split(',');
		array[0] = Path.Combine(text, array[0]);
		if (!registrationService.IsShcLocalOnly)
		{
			AppDownloadResult appDownloadResult = AppModuleHandler.DownloadApplicationPackage(packageUrl, text, array[0], manifestReader.PackageCheckSum, tokenEntry, !flag, networkMonitor, addinRepo);
			if (appDownloadResult != AppDownloadResult.Success)
			{
				if (lastAttempt && appDownloadResult == AppDownloadResult.NetworkFailure)
				{
					messageFactory.CreateDownloadFailureMessage(tokenEntry);
				}
				else if (appDownloadResult == AppDownloadResult.InvalidData)
				{
					messageFactory.CreateInvalidApplicationMessage(tokenEntry);
				}
				throw new AppDownloadException(appDownloadResult);
			}
		}
		MainAssemblyData mainAssemblyData2 = ExtractAssemblyNameAndTypes(array);
		if (mainAssemblyData2 != null)
		{
			return mainAssemblyData2;
		}
		return null;
	}

	public MainAssemblyData GetMainAssemblyDataFromLocalAddin(ApplicationTokenEntry tokenEntry)
	{
		string path = Path.Combine("\\NandFlash\\Apps", AppModuleHandler.GetLocalFolderName(tokenEntry));
		string fileName = Path.Combine(path, tokenEntry.AppManifest);
		ManifestReader manifestReader = new ManifestReader(fileName);
		string mainAssemblyName = manifestReader.MainAssemblyName;
		string[] array = mainAssemblyName.Split(',');
		array[0] = Path.Combine(path, array[0]);
		return ExtractAssemblyNameAndTypes(array);
	}

	private MainAssemblyData CheckTokenDownloadPath(string tokenDownloadPath)
	{
		if (string.IsNullOrEmpty(tokenDownloadPath))
		{
			return null;
		}
		MainAssemblyData result = null;
		string[] array = tokenDownloadPath.Split(',');
		MainAssemblyData mainAssemblyData = ExtractAssemblyNameAndTypes(array);
		if (mainAssemblyData != null)
		{
			result = mainAssemblyData;
		}
		array[0] = Path.Combine(basePath, array[0]);
		MainAssemblyData mainAssemblyData2 = ExtractAssemblyNameAndTypes(array);
		if (mainAssemblyData2 != null)
		{
			result = mainAssemblyData2;
		}
		return result;
	}

	private MainAssemblyData ExtractAssemblyNameAndTypes(string[] configLines)
	{
		MainAssemblyData result = null;
		if (configLines.Length > 1 && File.Exists(configLines[0]))
		{
			result = new MainAssemblyData(configLines[0], configLines.Skip(1).ToArray());
		}
		return result;
	}
}
