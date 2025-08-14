using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using RWE.SmartHome.SHC.DataAccessInterfaces.Messages;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;

namespace RWE.SmartHome.SHC.BusinessLogic.Persistence;

public class BackendPersistence : IBackendPersistence
{
	private const string ListShell = "<ArrayOf{0} xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" >{1}</ArrayOf{0}>";

	private const string ConfigurationItemShell = "<ConfigurationItem>{0}</ConfigurationItem>";

	private readonly Container container;

	private readonly ICertificateManager certificateManager;

	private readonly IMessagePersistence messagePersistence;

	private readonly IProtocolSpecificDataPersistence protocolSpecificDataPersistence;

	private readonly IRepository configurationRepository;

	private readonly ITechnicalConfigurationPersistence technicalConfiguration;

	private readonly IConfigurationClient configurationClient;

	private readonly IApplicationsSettings applicationsSettings;

	private readonly IIntegrityManagementControl integrityManagementControl;

	private XmlSerializer uiConfigSerializer;

	private XmlSerializer messageSerializer;

	private XmlSerializer technicalConfigSerializer;

	private XmlSerializer customAppsDataSerializer;

	private XmlSerializer protocolSpecificDataSerializer;

	private XmlSerializer configurationItemSerializer;

	private object dalConfigSerializerSync = new object();

	private XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

	private readonly string serialNo;

	private readonly object businessLogicMutex;

	private XmlSerializer ConfigurationItemSerializer
	{
		get
		{
			if (configurationItemSerializer == null)
			{
				configurationItemSerializer = new XmlSerializer(typeof(ConfigurationItem));
			}
			return configurationItemSerializer;
		}
	}

	private XmlSerializer UiConfigSerializer
	{
		get
		{
			if (uiConfigSerializer == null)
			{
				uiConfigSerializer = new XmlSerializer(typeof(RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration));
			}
			return uiConfigSerializer;
		}
	}

	private XmlSerializer MessageSerializer
	{
		get
		{
			if (messageSerializer == null)
			{
				messageSerializer = new XmlSerializer(typeof(MessagesAndAlertsContainer));
			}
			return messageSerializer;
		}
	}

	private XmlSerializer TechnicalConfigSerializer
	{
		get
		{
			if (technicalConfigSerializer == null)
			{
				technicalConfigSerializer = new XmlSerializer(typeof(TechnicalConfigurationContainer));
			}
			return technicalConfigSerializer;
		}
	}

	private XmlSerializer ProtocolSpecificDataSerializer
	{
		get
		{
			if (protocolSpecificDataSerializer == null)
			{
				protocolSpecificDataSerializer = new XmlSerializer(typeof(ProtocolSpecificDataContainer));
			}
			return protocolSpecificDataSerializer;
		}
	}

	private XmlSerializer CustomAppsDataSerializer
	{
		get
		{
			if (customAppsDataSerializer == null)
			{
				customAppsDataSerializer = new XmlSerializer(typeof(AddInConfigurationContainer));
			}
			return customAppsDataSerializer;
		}
	}

	public BackendPersistence(Container dependencyContainer, object businessLogicMutex)
	{
		container = dependencyContainer;
		certificateManager = container.Resolve<ICertificateManager>();
		messagePersistence = container.Resolve<IMessagePersistence>();
		configurationRepository = container.Resolve<IRepository>();
		integrityManagementControl = container.Resolve<IIntegrityManagementControl>();
		technicalConfiguration = container.Resolve<ITechnicalConfigurationPersistence>();
		protocolSpecificDataPersistence = container.Resolve<IProtocolSpecificDataPersistence>();
		configurationClient = container.Resolve<IConfigurationClient>();
		applicationsSettings = container.Resolve<IApplicationsSettings>();
		serialNo = SHCSerialNumber.SerialNumber();
		this.businessLogicMutex = businessLogicMutex;
		namespaces.Add(string.Empty, string.Empty);
	}

	public BackendPersistenceResult BackupUIConfiguration(bool createRestorePoint, ManualResetEvent cancellationEvent)
	{
		while (!Monitor.TryEnter(businessLogicMutex))
		{
			if (cancellationEvent != null && cancellationEvent.WaitOne(1000, exitContext: false))
			{
				return BackendPersistenceResult.OperationCancelled;
			}
		}
		try
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration configuration = new RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration();
			configuration.Locations = configurationRepository.GetLocations();
			configuration.LogicalDevices = configurationRepository.GetLogicalDevices();
			configuration.BaseDevices = configurationRepository.GetBaseDevices();
			configuration.Interactions = configurationRepository.GetInteractions();
			configuration.HomeSetups = configurationRepository.GetHomeSetups();
			configuration.RepositoryVersion = configurationRepository.RepositoryVersion;
			RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration o = configuration;
			string xmlData;
			using (StringWriter stringWriter = new StringWriter())
			{
				UiConfigSerializer.Serialize(stringWriter, o);
				xmlData = stringWriter.ToString();
				stringWriter.Close();
			}
			return BackupManagedConfiguration(ConfigurationType.UIConfiguration, "/", xmlData, createRestorePoint, cancellationEvent);
		}
		finally
		{
			Monitor.Exit(businessLogicMutex);
		}
	}

	public bool RestoreUIConfiguration()
	{
		bool flag = false;
		lock (businessLogicMutex)
		{
			try
			{
				DisableCfgIntegrityManagement();
				GetList<Interaction>("INT").ForEach(configurationRepository.SetInteraction);
				GetList<Location>("LC").ForEach(configurationRepository.SetLocation);
				GetList<LogicalDevice>("LD").ForEach(configurationRepository.SetLogicalDevice);
				GetList<BaseDevice>("BD").ForEach(configurationRepository.SetBaseDevice);
				GetList<HomeSetup>("HMS").ForEach(configurationRepository.SetHomeSetup);
				flag = RestoreManagedConfiguration(ConfigurationType.UIConfiguration, "/UIConfigurationContainer/RepositoryVersion", out var xmlData);
				if (flag && xmlData != string.Empty)
				{
					configurationRepository.RepositoryVersion = int.Parse(xmlData);
				}
				configurationRepository.Commit(CommitType.InitialVersion);
				EnableCfgIntegrityManagement();
			}
			catch (Exception ex)
			{
				Log.Error(Module.BusinessLogic, $"Failed restoring the UI Configuration with error: {ex.ToString()}");
				EnableCfgIntegrityManagement();
				flag = false;
			}
			return flag;
		}
	}

	private void EnableCfgIntegrityManagement()
	{
		integrityManagementControl.SetIntegrityManagementHooksState(IntegrityManagementHooksState.HooksEnabled);
	}

	private void DisableCfgIntegrityManagement()
	{
		integrityManagementControl.SetIntegrityManagementHooksState(IntegrityManagementHooksState.HooksDisabled);
	}

	public bool RestoreUIConfigurationFromRestorePoint(string restorePointId)
	{
		bool flag = false;
		lock (businessLogicMutex)
		{
			try
			{
				DisableCfgIntegrityManagement();
				flag = RestoreManagedConfigurationFromRestorePoint(restorePointId, out var xmlData);
				if (flag)
				{
					if (xmlData != string.Empty)
					{
						using StringReader textReader = new StringReader(xmlData);
						configurationRepository.DeleteAllEntities();
						RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration configuration = (RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration)UiConfigSerializer.Deserialize(textReader);
						configuration.Locations.ForEach(configurationRepository.SetLocation);
						configuration.LogicalDevices.ForEach(configurationRepository.SetLogicalDevice);
						configuration.BaseDevices.ForEach(configurationRepository.SetBaseDevice);
						configuration.HomeSetups.ForEach(configurationRepository.SetHomeSetup);
					}
					else
					{
						flag = false;
					}
				}
				EnableCfgIntegrityManagement();
			}
			catch (Exception ex)
			{
				Log.Error(Module.BusinessLogic, $"Failed restoring the UI Configuration from restore point with error: {ex.ToString()}");
				EnableCfgIntegrityManagement();
				flag = false;
			}
			return flag;
		}
	}

	public bool DeleteUIConfiguration()
	{
		lock (businessLogicMutex)
		{
			return DeleteManagedConfiguration(ConfigurationType.UIConfiguration, null);
		}
	}

	public void ReleaseServiceClient()
	{
		configurationClient.ReleaseServiceClient();
	}

	public BackendPersistenceResult BackupTechnicalConfiguration(ManualResetEvent cancellationEvent)
	{
		while (!Monitor.TryEnter(businessLogicMutex))
		{
			if (cancellationEvent != null && cancellationEvent.WaitOne(1000, exitContext: false))
			{
				return BackendPersistenceResult.OperationCancelled;
			}
		}
		try
		{
			TechnicalConfigurationContainer technicalConfigurationContainer = new TechnicalConfigurationContainer();
			technicalConfigurationContainer.Entities = technicalConfiguration.LoadAll().ToList();
			TechnicalConfigurationContainer o = technicalConfigurationContainer;
			string xmlData;
			using (StringWriter stringWriter = new StringWriter())
			{
				TechnicalConfigSerializer.Serialize(stringWriter, o);
				xmlData = stringWriter.ToString();
				stringWriter.Close();
			}
			return BackupManagedConfiguration(ConfigurationType.TechnicalConfiguration, "/", xmlData, createRestorePoint: false, cancellationEvent);
		}
		finally
		{
			Monitor.Exit(businessLogicMutex);
		}
	}

	public bool RestoreTechnicalConfiguration()
	{
		lock (businessLogicMutex)
		{
			string xmlData;
			bool flag = RestoreManagedConfiguration(ConfigurationType.TechnicalConfiguration, "/", out xmlData);
			if (flag && xmlData != string.Empty)
			{
				using StringReader textReader = new StringReader(xmlData);
				TechnicalConfigurationContainer technicalConfigurationContainer = (TechnicalConfigurationContainer)TechnicalConfigSerializer.Deserialize(textReader);
				technicalConfiguration.SaveAll(technicalConfigurationContainer.Entities);
			}
			return flag;
		}
	}

	public bool DeleteTechnicalConfiguration()
	{
		lock (businessLogicMutex)
		{
			return DeleteManagedConfiguration(ConfigurationType.TechnicalConfiguration, null);
		}
	}

	public BackendPersistenceResult BackupDeviceList(ManualResetEvent cancellationEvent)
	{
		while (!Monitor.TryEnter(businessLogicMutex))
		{
			if (cancellationEvent != null && cancellationEvent.WaitOne(1000, exitContext: false))
			{
				return BackendPersistenceResult.OperationCancelled;
			}
		}
		try
		{
			ProtocolSpecificDataContainer o = new ProtocolSpecificDataContainer(protocolSpecificDataPersistence.LoadAll());
			string xmlData;
			using (StringWriter stringWriter = new StringWriter())
			{
				ProtocolSpecificDataSerializer.Serialize(stringWriter, o);
				xmlData = stringWriter.ToString();
				stringWriter.Close();
			}
			return BackupManagedConfiguration(ConfigurationType.DeviceList, "/", xmlData, createRestorePoint: false, cancellationEvent);
		}
		finally
		{
			Monitor.Exit(businessLogicMutex);
		}
	}

	public bool RestoreDeviceList()
	{
		lock (businessLogicMutex)
		{
			string xmlData;
			bool flag = RestoreManagedConfiguration(ConfigurationType.DeviceList, "/", out xmlData);
			if (flag && xmlData != string.Empty)
			{
				try
				{
					SaveProtocolSpecificDataLocally(xmlData);
				}
				catch (Exception)
				{
					Log.Error(Module.BusinessLogic, "Error Restoring the device list");
				}
			}
			return flag;
		}
	}

	private void SaveProtocolSpecificDataLocally(string xmlData)
	{
		ProtocolSpecificDataContainer entities = (ProtocolSpecificDataContainer)ProtocolSpecificDataSerializer.Deserialize(new StringReader(xmlData));
		protocolSpecificDataPersistence.SaveAll(entities, suppressEvent: true);
	}

	public bool DeleteDeviceList()
	{
		lock (businessLogicMutex)
		{
			return DeleteManagedConfiguration(ConfigurationType.DeviceList, null);
		}
	}

	public BackendPersistenceResult BackupMessagesAndAlerts(ManualResetEvent cancellationEvent)
	{
		while (!Monitor.TryEnter(businessLogicMutex))
		{
			if (cancellationEvent != null && cancellationEvent.WaitOne(1000, exitContext: false))
			{
				return BackendPersistenceResult.OperationCancelled;
			}
		}
		try
		{
			string xmlData;
			using (StringWriter stringWriter = new StringWriter())
			{
				MessagesAndAlertsContainer messagesAndAlertsContainer = new MessagesAndAlertsContainer();
				messagesAndAlertsContainer.MessageInfos = messagePersistence.CreateBackup().ToList();
				MessagesAndAlertsContainer o = messagesAndAlertsContainer;
				MessageSerializer.Serialize(stringWriter, o);
				xmlData = stringWriter.ToString();
				stringWriter.Close();
			}
			return BackupManagedConfiguration(ConfigurationType.MessagesAndAlerts, "/", xmlData, createRestorePoint: false, cancellationEvent);
		}
		finally
		{
			Monitor.Exit(businessLogicMutex);
		}
	}

	public bool RestoreMessagesAndAlerts()
	{
		lock (businessLogicMutex)
		{
			string xmlData;
			bool flag = RestoreManagedConfiguration(ConfigurationType.MessagesAndAlerts, "/", out xmlData);
			if (flag && xmlData != string.Empty)
			{
				using StringReader textReader = new StringReader(xmlData);
				MessagesAndAlertsContainer messagesAndAlertsContainer = (MessagesAndAlertsContainer)MessageSerializer.Deserialize(textReader);
				messagePersistence.RestoreFromBackup(messagesAndAlertsContainer.MessageInfos);
			}
			return flag;
		}
	}

	public bool DeleteMessagesAndAlerts()
	{
		lock (businessLogicMutex)
		{
			return DeleteManagedConfiguration(ConfigurationType.MessagesAndAlerts, null);
		}
	}

	public BackendPersistenceResult BackupCustomApplicationsSettings(ManualResetEvent cancellationEvent)
	{
		while (!Monitor.TryEnter(businessLogicMutex))
		{
			if (cancellationEvent != null && cancellationEvent.WaitOne(1000, exitContext: false))
			{
				return BackendPersistenceResult.OperationCancelled;
			}
		}
		try
		{
			int totalSize;
			List<ConfigurationItem> allSettingsMetadata = applicationsSettings.GetAllSettingsMetadata(out totalSize);
			return (totalSize < 30000) ? UploadCustomappsDataAllInOne(cancellationEvent) : UploadCustomappsDataOneByOne(allSettingsMetadata, cancellationEvent);
		}
		finally
		{
			Monitor.Exit(businessLogicMutex);
		}
	}

	private BackendPersistenceResult UploadCustomappsDataAllInOne(ManualResetEvent cancellationEvent)
	{
		AddInConfigurationContainer o = new AddInConfigurationContainer(applicationsSettings.GetAllSettings());
		string xmlData;
		using (StringWriter stringWriter = new StringWriter())
		{
			using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				Indent = false,
				OmitXmlDeclaration = true
			});
			CustomAppsDataSerializer.Serialize(xmlWriter, o, namespaces);
			xmlData = stringWriter.ToString();
			xmlWriter.Close();
		}
		return BackupManagedConfiguration(ConfigurationType.CustomApplication, "/", xmlData, createRestorePoint: false, cancellationEvent);
	}

	private BackendPersistenceResult UploadCustomappsDataOneByOne(List<ConfigurationItem> items, ManualResetEvent cancellationEvent)
	{
		bool flag = true;
		foreach (ConfigurationItem item in items)
		{
			ConfigurationItem setting = applicationsSettings.GetSetting(item.ApplicationId, item.Name);
			setting.ApplicationId = item.ApplicationId;
			ConfigurationResultCode configurationResultCode;
			using (StringWriter stringWriter = new StringWriter())
			{
				using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
				{
					Indent = false,
					OmitXmlDeclaration = true
				});
				if (flag)
				{
					flag = false;
					AddInConfigurationContainer addInConfigurationContainer = new AddInConfigurationContainer();
					addInConfigurationContainer.Add(setting);
					CustomAppsDataSerializer.Serialize(xmlWriter, addInConfigurationContainer, namespaces);
					configurationResultCode = configurationClient.SetManagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", ConfigurationType.CustomApplication, "/", stringWriter.ToString(), 1, -1, string.Empty, createRestorePoint: false);
				}
				else
				{
					ConfigurationItemSerializer.Serialize(xmlWriter, setting, namespaces);
					configurationResultCode = configurationClient.AddManagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", ConfigurationType.CustomApplication, "/ArrayOfConfigurationItem", stringWriter.ToString(), 1, -1, string.Empty, createRestorePoint: false);
				}
				xmlWriter.Close();
			}
			if (configurationResultCode != ConfigurationResultCode.Success)
			{
				return BackendPersistenceResult.ServiceFailure;
			}
		}
		return BackendPersistenceResult.Success;
	}

	public bool RestoreCustomApplicationsSettings()
	{
		try
		{
			return RestoreCustomApplicationsSettingsAllInOne();
		}
		catch (Exception ex)
		{
			if (ex is OutOfMemoryException || (ex.InnerException != null && ex.InnerException is QuotaExceededException))
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				Log.Information(Module.BusinessLogic, "Custom apps data too large. Will retry restoring one-by-one.");
				return RestoreCustomApplicationsSettingsOneByOne();
			}
			Log.Error(Module.BusinessLogic, "Failed to restore custom apps data: " + ex);
			return false;
		}
	}

	private bool RestoreCustomApplicationsSettingsAllInOne()
	{
		lock (businessLogicMutex)
		{
			string xmlData;
			bool flag = RestoreManagedConfiguration(ConfigurationType.CustomApplication, "/", out xmlData);
			if (flag && !string.IsNullOrEmpty(xmlData))
			{
				using StringReader textReader = new StringReader(xmlData);
				List<ConfigurationItem> list = (AddInConfigurationContainer)CustomAppsDataSerializer.Deserialize(textReader);
				list.ForEach(delegate(ConfigurationItem cfgItem)
				{
					applicationsSettings.SetValue(cfgItem);
				});
			}
			return flag;
		}
	}

	private bool RestoreCustomApplicationsSettingsOneByOne()
	{
		lock (businessLogicMutex)
		{
			bool result = false;
			int num = 1;
			string xmlData;
			do
			{
				bool flag = RestoreManagedConfiguration(ConfigurationType.CustomApplication, $"/ArrayOfConfigurationItem/ConfigurationItem[{num}]", out xmlData);
				if (1 == num)
				{
					result = flag;
				}
				if (flag && !string.IsNullOrEmpty(xmlData))
				{
					using (StringReader textReader = new StringReader($"<ConfigurationItem>{xmlData}</ConfigurationItem>"))
					{
						ConfigurationItem value = (ConfigurationItem)ConfigurationItemSerializer.Deserialize(textReader);
						applicationsSettings.SetValue(value);
					}
					num++;
					continue;
				}
				return result;
			}
			while (!string.IsNullOrEmpty(xmlData));
			return true;
		}
	}

	private BackendPersistenceResult BackupManagedConfiguration(ConfigurationType configurationType, string xPath, string xmlData, bool createRestorePoint, ManualResetEvent cancellationEvent)
	{
		ConfigurationResultCode result = ConfigurationResultCode.Success;
		Guid correlationId = Guid.NewGuid();
		ChunkWriter chunkWriter = new ChunkWriter(32768, delegate(byte[] chunk, int currentPackage, int nextPackage)
		{
			if (result == ConfigurationResultCode.Success)
			{
				string bulkXMLData = Encoding.UTF8.GetString(chunk, 0, chunk.Length);
				result = configurationClient.SetManagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", configurationType, xPath, bulkXMLData, currentPackage, nextPackage, correlationId.ToString(), createRestorePoint);
				if (result != ConfigurationResultCode.Success)
				{
					return BackendPersistenceResult.ServiceFailure;
				}
				return BackendPersistenceResult.Success;
			}
			return BackendPersistenceResult.ServiceFailure;
		}, cancellationEvent);
		BackendPersistenceResult backendPersistenceResult = chunkWriter.AddUtf8String(xmlData);
		if (backendPersistenceResult != BackendPersistenceResult.Success)
		{
			return backendPersistenceResult;
		}
		return chunkWriter.Flush();
	}

	private bool RestoreManagedConfiguration(ConfigurationType configurationType, string xPath, out string xmlData)
	{
		ConfigurationResultCode managedSHCConfiguration = configurationClient.GetManagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", configurationType, xPath, out xmlData);
		return managedSHCConfiguration == ConfigurationResultCode.Success;
	}

	private bool RestoreManagedConfigurationFromRestorePoint(string restorePointId, out string xmlData)
	{
		ConfigurationResultCode restorePointShcConfiguration = configurationClient.GetRestorePointShcConfiguration(certificateManager.PersonalCertificateThumbprint, restorePointId, "3.00", out xmlData);
		return restorePointShcConfiguration == ConfigurationResultCode.Success;
	}

	private bool DeleteManagedConfiguration(ConfigurationType configurationType, string xPath)
	{
		ConfigurationResultCode configurationResultCode = configurationClient.DeleteManagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", configurationType, xPath, createRestorePoint: false);
		return configurationResultCode == ConfigurationResultCode.Success;
	}

	private bool BackupUnmanagedConfiguration(ConfigurationType configurationType, byte[] data)
	{
		ConfigurationResultCode result = ConfigurationResultCode.Success;
		Guid correlationId = Guid.NewGuid();
		ChunkWriter chunkWriter = new ChunkWriter(32768, delegate(byte[] chunk, int currentPackage, int nextPackage)
		{
			if (result == ConfigurationResultCode.Success)
			{
				result = configurationClient.SetUnmanagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", configurationType, chunk, currentPackage, nextPackage, correlationId.ToString(), createRestorePoint: false);
				if (result != ConfigurationResultCode.Success)
				{
					return BackendPersistenceResult.ServiceFailure;
				}
				return BackendPersistenceResult.Success;
			}
			return BackendPersistenceResult.ServiceFailure;
		});
		if (chunkWriter.AddByteArray(data) == BackendPersistenceResult.Success)
		{
			return chunkWriter.Flush() == BackendPersistenceResult.Success;
		}
		return false;
	}

	private bool RestoreUnmanagedConfiguration(ConfigurationType configurationType, out byte[] data)
	{
		ConfigurationResultCode unmanagedSHCConfiguration = configurationClient.GetUnmanagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", configurationType, out data);
		return unmanagedSHCConfiguration == ConfigurationResultCode.Success;
	}

	private bool DeleteUnmanagedConfiguration(ConfigurationType configurationType)
	{
		ConfigurationResultCode configurationResultCode = configurationClient.DeleteUnmanagedSHCConfiguration(certificateManager.PersonalCertificateThumbprint, serialNo, "3.00", configurationType, createRestorePoint: false);
		return configurationResultCode == ConfigurationResultCode.Success;
	}

	private List<T> GetList<T>(string elementName)
	{
		string text = $"/UIConfigurationContainer/{elementName}s";
		if (!RestoreManagedConfiguration(ConfigurationType.UIConfiguration, text, out var xmlData))
		{
			throw new InvalidDataException($"Failed to obtain {text} from the server.");
		}
		if (xmlData != string.Empty)
		{
			xmlData = string.Format("<ArrayOf{0} xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" >{1}</ArrayOf{0}>", elementName, xmlData);
			using StringReader textReader = new StringReader(xmlData);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
			Log.Information(Module.BusinessLogic, $"Created serializer: GetList<T> {elementName}");
			return (List<T>)xmlSerializer.Deserialize(textReader);
		}
		return new List<T>();
	}
}
