using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.ApplicationsHost.Configuration;
using RWE.SmartHome.SHC.ApplicationsHost.Exceptions;
using RWE.SmartHome.SHC.ApplicationsHost.Storage;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using RWE.SmartHome.SHC.RuleEngineInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using RWE.Smarthome.SHC.CompatibilityShim;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Protocols.CustomProtocol;
using SmartHome.SHC.API.Protocols.Lemonbeat;
using SmartHome.SHC.API.Protocols.wMBus;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class ApplicationsHost : IApplicationsHost
{
	private const int MinSecondsReconnectTokenCacheRefresh = 300;

	private const int MaxSecondsReconnectTokenCacheRefresh = 3600;

	private IDynamicSettingsResolver dynamicStateResolver;

	private readonly IRepository configurationRepository;

	private readonly ITokenCache tokenCache;

	private IApplicationsSettings settingsPersistence;

	private readonly IEventManager eventManager;

	private readonly Dictionary<string, ApplicationStateInfo> loadedApplications;

	private readonly Dictionary<string, ApplicationEnvironment> loadedApplicationsEnvironments;

	private readonly string basePath;

	private IExternalCommandDispatcher externalCommandDispatcher;

	private readonly Container container;

	private readonly INetworkingMonitor networkMonitor;

	private readonly IAddinRepoManager addinRepo;

	private readonly ICertificateManager certManager;

	private IConfigurationProcessor configProcessor;

	private readonly AddInLoader addInLoader;

	private AppHostMessageFactory messageFactory;

	private readonly IAppHostSysData sysData;

	private MainAssemblyDataFactory mainAssemblyDataFactory;

	private readonly IScheduler scheduler;

	private readonly ApplicationVersionStorage applicationVersionStorage = new ApplicationVersionStorage();

	private static XmlRootAttribute xmlRoot = new XmlRootAttribute
	{
		ElementName = "AppToken",
		IsNullable = true
	};

	private readonly XmlSerializer appTokenSerializer = new XmlSerializer(typeof(ApplicationsToken), xmlRoot);

	private readonly IRegistrationService registrationService;

	private AddinsConfigurationRepository addinsConfigurationRepository;

	private AddinsConfigurationValidator addinsConfigurationValidator;

	private bool wasConnected = true;

	private bool mismatchLogged;

	private int silencePeriod = 24;

	private bool isShcStarting = true;

	public event Action<ApplicationLoadStateChangedEventArgs> ApplicationStateChanged;

	public event Action ApplicationsLoaded;

	internal ApplicationsHost(ITokenCache tokenCache, Container container, IAppHostSysData sysData)
	{
		this.sysData = sysData;
		basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
		loadedApplications = new Dictionary<string, ApplicationStateInfo>();
		loadedApplicationsEnvironments = new Dictionary<string, ApplicationEnvironment>();
		this.tokenCache = tokenCache;
		this.container = container;
		eventManager = container.Resolve<IEventManager>();
		configurationRepository = container.Resolve<IRepository>();
		networkMonitor = container.Resolve<INetworkingMonitor>();
		addinRepo = container.Resolve<IAddinRepoManager>();
		registrationService = container.Resolve<IRegistrationService>();
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcInitialized, (ShcStartupCompletedEventArgs a) => a.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcInitializationFinished, (ShcStartupCompletedEventArgs a) => a.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, null);
		certManager = container.Resolve<ICertificateManager>();
		scheduler = container.Resolve<IScheduler>();
		scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), CheckApplicationsToken, TimeSpan.FromDays(1.0)));
		eventManager.GetEvent<ChannelConnectivityChangedEvent>().Subscribe(OnConnectivityChanged, null, ThreadOption.BackgroundThread, null);
		addInLoader = new AddInLoader();
	}

	public IWMBusDeviceHandler GetWMBusDeviceHandler(WMBusDeviceTypeIdentifier deviceType)
	{
		IWMBusDeviceHandler iWMBusDeviceHandler = (from la in loadedApplications
			where la.Value.IsActive
			select la.Value.Application as IWMBusDeviceHandler into ap
			where ap?.HandledDeviceTypes.Contains(deviceType) ?? false
			select ap).FirstOrDefault();
		if (iWMBusDeviceHandler != null)
		{
			return iWMBusDeviceHandler;
		}
		if (deviceType != null)
		{
			iWMBusDeviceHandler = (from la in loadedApplications
				where la.Value.IsActive
				select la.Value.Application as IWMBusDeviceHandler into ap
				where ap?.HandledDeviceTypes.Any((WMBusDeviceTypeIdentifier dt) => dt != null && dt.Manufacturer == deviceType.Manufacturer && dt.Medium == deviceType.Medium && !dt.Version.HasValue) ?? false
				select ap).FirstOrDefault();
			if (iWMBusDeviceHandler != null && !mismatchLogged)
			{
				Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Found addin without version: {iWMBusDeviceHandler.ApplicationId}");
				mismatchLogged = true;
			}
		}
		return iWMBusDeviceHandler;
	}

	public IDeviceHandler GetLemonbeatDeviceHandler(LemonbeatDeviceTypeIdentifier deviceType)
	{
		return (from la in loadedApplications
			where la.Value.IsActive
			select la.Value.Application as IDeviceHandler into ap
			where ap?.HandledDeviceTypes.Contains(deviceType) ?? false
			select ap).FirstOrDefault();
	}

	public T GetCustomDevice<T>(string applicationId) where T : class
	{
		lock (loadedApplications)
		{
			if (!string.IsNullOrEmpty(applicationId) && loadedApplications.ContainsKey(applicationId) && loadedApplications[applicationId].IsActive)
			{
				return loadedApplications[applicationId].Application as T;
			}
			return null;
		}
	}

	public IEnumerable<T> GetCustomDevices<T>() where T : class
	{
		lock (loadedApplications)
		{
			return (from y in loadedApplications
				where y.Value.IsActive
				select y.Value.Application as T into z
				where z != null
				select z).ToList();
		}
	}

	public RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult ExecuteAction(string appId, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription action)
	{
		IAddIn customDevice = GetCustomDevice<IAddIn>(appId);
		if (customDevice is global::SmartHome.SHC.API.IActionExecuterHandler actionExecuterHandler)
		{
			try
			{
				global::SmartHome.SHC.API.Configuration.ExecutionContext executionContext = new global::SmartHome.SHC.API.Configuration.ExecutionContext();
				executionContext.Source = ExecutionSource.DirectExecution;
				global::SmartHome.SHC.API.Configuration.ExecutionContext context = executionContext;
				global::SmartHome.SHC.API.ExecutionResult dsdkResult = actionExecuterHandler.ExecuteAction(action.ToApi(), context);
				return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.FromDeviceSDK(dsdkResult);
			}
			catch (Exception ex)
			{
				return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error(ex.Message);
			}
		}
		return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("Invalid configuration");
	}

	public string GetAddinVersion(string appId)
	{
		string result = string.Empty;
		using (TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("GetAddinVersion"))
		{
			try
			{
				ApplicationTokenEntry applicationTokenEntry = tokenCacheContext.AppsToken.Entries.Find((ApplicationTokenEntry a) => a.AppId == appId);
				if (applicationTokenEntry != null)
				{
					result = applicationTokenEntry.Version;
				}
			}
			catch (Exception ex)
			{
				Log.Error(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "An unexpected error occured while getting version of addin [" + appId + "]: " + ex.Message);
			}
		}
		return result;
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		foreach (BaseDevice baseDevice in devices)
		{
			ICustomProtocolDeviceHandler customDevice = GetCustomDevice<ICustomProtocolDeviceHandler>(baseDevice.AppId);
			if (customDevice != null)
			{
				try
				{
					customDevice.DropDiscoveredDevice(baseDevice.Id);
				}
				catch (Exception ex)
				{
					Log.Exception(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, ex, "Could not remove discovered device.");
				}
			}
		}
	}

	private bool FetchApplicationsToken(bool lastAttempt, out bool tokenUpdated)
	{
		bool flag = true;
		tokenUpdated = false;
		Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "Checking if application token is up to date");
		if (registrationService.IsShcLocalOnly)
		{
			tokenUpdated = tokenCache.UpdateToken("FetchApplicationsToken");
		}
		else if (!tokenCache.IsTokenUpToDate("FetchApplicationsToken"))
		{
			Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "Application token outdated. Updating.");
			flag = (tokenUpdated = tokenCache.UpdateToken("FetchApplicationsToken"));
		}
		else
		{
			Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "Application token up to date.");
		}
		if (messageFactory != null && (flag || lastAttempt))
		{
			messageFactory.UpdateApplicationsTokenLoadFailureAlert(flag);
		}
		return flag;
	}

	private void OnShcInitialized(ShcStartupCompletedEventArgs args)
	{
		messageFactory = new AppHostMessageFactory(container.Resolve<IMessagesAndAlertsManager>());
		mainAssemblyDataFactory = new MainAssemblyDataFactory(basePath, messageFactory, networkMonitor, addinRepo, registrationService);
		if (registrationService.IsShcLocalOnly)
		{
			FetchApplicationsToken(lastAttempt: true, out var _);
		}
		else
		{
			container.Resolve<IBusinessLogic>().PerformBackendCommunicationWithRetries("Could not check token at startup.", (bool lastAttempt) => FetchApplicationsToken(lastAttempt, out var _));
		}
		settingsPersistence = container.Resolve<IApplicationsSettings>();
		externalCommandDispatcher = container.Resolve<IExternalCommandDispatcher>();
		configProcessor = container.Resolve<IConfigurationProcessor>();
		IConfigurationManager configurationManager = container.Resolve<IConfigurationManager>();
		dynamicStateResolver = container.Resolve<IDynamicSettingsResolver>();
		silencePeriod = configurationManager["RWE.SmartHome.SHC.Core"].GetInt("SilencePeriod") ?? 24;
		addinsConfigurationRepository = new AddinsConfigurationRepository(configurationRepository, eventManager, dynamicStateResolver);
		addinsConfigurationValidator = new AddinsConfigurationValidator(addinsConfigurationRepository);
		configProcessor.RegisterConfigurationValidator(addinsConfigurationValidator);
		LoadAllActiveApplicationsWithRetries();
		tokenCache.TokenUpdated += LoadAllActiveApplicationsWithRetries;
	}

	private List<AddinUpdateData> TryToUpdateAddinsFromUsb(ApplicationsToken appsToken)
	{
		List<AddinUpdateData> addinsToRemove = addinRepo.GetAddinsToRemove(appsToken);
		foreach (AddinUpdateData item in addinsToRemove)
		{
			CleanupAppRepository(appsToken);
			ApplicationTokenEntry updatedAddin = addinRepo.GetUpdatedAddin(item);
			LoadApplication(updatedAddin, lastAttempt: true, isNewApplication: true);
			UpdateApplication(lastAttempt: true, updatedAddin);
		}
		if (addinsToRemove.Any())
		{
			RefreshAllVersions(appsToken.Entries);
			tokenCache.UpdateToken("TryToUpdateAddinsFromUsb");
		}
		return addinsToRemove;
	}

	private void OnShcInitializationFinished(ShcStartupCompletedEventArgs args)
	{
		isShcStarting = false;
	}

	private void LoadAllActiveApplicationsWithRetries()
	{
		IBusinessLogic businessLogic = container.Resolve<IBusinessLogic>();
		businessLogic.PerformBackendCommunicationWithRetries("Could not download all apps.", LoadAllActiveApplications);
	}

	private bool LoadAllActiveApplications(bool lastAttempt)
	{
		bool flag = false;
		List<AddinUpdateData> list = null;
		using (TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("LoadAllActiveApplications"))
		{
			LogToken(tokenCacheContext.AppsToken);
			if (tokenCacheContext.AppsToken == null || tokenCacheContext.AppsToken.Entries == null)
			{
				return true;
			}
			lock (loadedApplications)
			{
				applicationVersionStorage.LoadAllVersions();
				if (registrationService.IsShcLocalOnly)
				{
					list = TryToUpdateAddinsFromUsb(tokenCacheContext.AppsToken);
					addinRepo.InstallCoreDeliveredAddinsIfExists(tokenCacheContext.AppsToken);
				}
				ApplicationTokenEntry entry;
				foreach (ApplicationTokenEntry entry2 in tokenCacheContext.AppsToken.Entries)
				{
					entry = entry2;
					if (!ShouldReloadApplicationTokenEntry(entry))
					{
						continue;
					}
					entry.ActiveOnShc = false;
					try
					{
						Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Checking application {entry.AppId}");
						if (entry.IsEnabled && (!entry.ExpirationDate.HasValue || entry.ExpirationDate >= DateTime.UtcNow.Date))
						{
							if (list == null || list.FirstOrDefault((AddinUpdateData addin) => string.Equals(addin.Type, entry.AppId.Remove(0, addinRepo.AppIdPrefix.Length))) == null)
							{
								if (loadedApplications.ContainsKey(entry.AppId))
								{
									CheckAndUpdateApplication(entry, lastAttempt);
								}
								else
								{
									LoadApplication(entry, lastAttempt, isNewApplication: true);
								}
							}
						}
						else
						{
							Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {entry.AppId} is not enabled or has expired");
							if (loadedApplications.ContainsKey(entry.AppId) && loadedApplications[entry.AppId].State == ApplicationState.Active)
							{
								DeactivateApplication(entry);
								loadedApplications[entry.AppId].State = ApplicationState.Inactive;
							}
						}
					}
					catch (AppDownloadException ex)
					{
						flag = ex.Result != AppDownloadResult.InvalidData;
						if (ex.Result == AppDownloadResult.InvalidData || ex.Result == AppDownloadResult.InvalidManifest)
						{
							loadedApplications[entry.AppId] = new ApplicationStateInfo(null, ApplicationState.LoadFailed);
						}
					}
					catch (Exception arg)
					{
						Log.Error(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Failed to load application {entry.AppId}: {arg}");
						loadedApplications[entry.AppId] = new ApplicationStateInfo(null, ApplicationState.LoadFailed);
					}
				}
				CleanupAppRepository(tokenCacheContext.AppsToken);
				RefreshAllVersions(tokenCacheContext.AppsToken.Entries);
			}
		}
		if (!flag)
		{
			this.ApplicationsLoaded?.Invoke();
		}
		return !flag;
	}

	private void RefreshAllVersions(List<ApplicationTokenEntry> entries)
	{
		applicationVersionStorage.ClearVersions();
		entries.ForEach(delegate(ApplicationTokenEntry e)
		{
			applicationVersionStorage.SetAppVersion(e.AppId, e.Version);
		});
		applicationVersionStorage.SaveAllVersions();
	}

	private void LogToken(ApplicationsToken token)
	{
		Log.Debug(RWE.SmartHome.SHC.Core.Module.BackendCommunication, appTokenSerializer.Serialize(token));
	}

	private bool CheckAndUpdateApplication(ApplicationTokenEntry entry, bool lastAttempt)
	{
		ApplicationStateInfo applicationStateInfo = loadedApplications[entry.AppId];
		bool flag = 0 != string.Compare(applicationStateInfo.Version, entry.Version, StringComparison.OrdinalIgnoreCase);
		try
		{
			if (flag)
			{
				UpdateApplication(lastAttempt, entry);
			}
			else
			{
				if (!RestoreLoadedApplication(entry, applicationStateInfo) && !(DateTime.UtcNow.Subtract(applicationStateInfo.LastDownloadFailure).TotalHours > (double)silencePeriod))
				{
					return false;
				}
				if (entry.UpdateAvailable && entry.UpdateAvailable != applicationStateInfo.UpdateAvailable)
				{
					applicationStateInfo.UpdateAvailable = true;
					FireApplicationUpdateAvailable(entry);
					messageFactory.CreateUpdateAvailableMessage(entry);
				}
				else if (DifferAppParameters(applicationStateInfo.Parameters, entry.Parameters))
				{
					FireProductStateUpdatedEvent(entry, this.ApplicationStateChanged, ApplicationStates.ApplicationUpdated);
				}
			}
			return true;
		}
		catch (Exception arg)
		{
			Log.Error(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Failed to load application {entry.AppId}: {arg}");
			loadedApplications[entry.AppId] = new ApplicationStateInfo(null, ApplicationState.LoadFailed);
			FireApplicationUpdatedEvent(entry);
			messageFactory.CreateApplicationUpdatedMessage(entry, success: false);
		}
		return false;
	}

	private bool DifferAppParameters(List<ApplicationParameter> loadedParams, List<ApplicationParameter> appTokenParams)
	{
		if (loadedParams == null && appTokenParams == null)
		{
			return false;
		}
		if ((loadedParams == null) ^ (appTokenParams == null))
		{
			return true;
		}
		if (loadedParams.Count != appTokenParams.Count)
		{
			return true;
		}
		foreach (ApplicationParameter appTokenParam in appTokenParams)
		{
			string key = appTokenParam.Key;
			ApplicationParameter applicationParameter = loadedParams.FirstOrDefault((ApplicationParameter p) => p.Key == key);
			if (applicationParameter == null || applicationParameter.Value != appTokenParam.Value)
			{
				return true;
			}
		}
		return false;
	}

	private void FireProductStateUpdatedEvent(ApplicationTokenEntry entry, Action<ApplicationLoadStateChangedEventArgs> handler, ApplicationStates appNewState)
	{
		ApplicationStateInfo value;
		bool flag = loadedApplications.TryGetValue(entry.AppId, out value) && value.State != ApplicationState.LoadFailed;
		if (entry.Parameters == null)
		{
			entry.Parameters = new List<ApplicationParameter>();
		}
		handler?.Invoke(new ApplicationLoadStateChangedEventArgs
		{
			Application = (flag ? value.Application : new InvalidApplication(entry.AppId)),
			ApplicationVersion = entry.Version,
			ApplicationState = appNewState,
			Parameters = entry.GetApplicationStateFromToken()
		});
	}

	private void FireApplicationUpdatedEvent(ApplicationTokenEntry entry)
	{
		entry.UpdateAvailable = false;
		FireProductStateUpdatedEvent(entry, this.ApplicationStateChanged, ApplicationStates.ApplicationUpdated);
		Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {entry.AppId} was updated to version {entry.Version}");
	}

	private void FireApplicationUpdateAvailable(ApplicationTokenEntry entry)
	{
		FireProductStateUpdatedEvent(entry, this.ApplicationStateChanged, ApplicationStates.ApplicationUpdateAvailable);
		Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {entry.AppId} has an update available.");
	}

	private void UninstallForUpgrade(ApplicationStateInfo appStateInfo)
	{
		string text = "sh://InvalidInstance";
		try
		{
			text = appStateInfo.Application.ApplicationId;
			appStateInfo.Application.Uninitialize(CleanupMode.Upgrade);
		}
		catch (Exception ex)
		{
			Log.Exception(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, ex, "Exception caught while uninitializing add-in {0} v.{1}", text, appStateInfo.Version);
		}
		loadedApplicationsEnvironments[appStateInfo.Application.ApplicationId].Uninitialize();
	}

	private void CleanupMissingApps(List<string> existingAppIds)
	{
		existingAppIds.Add(CoreConstants.CoreAppId);
		Predicate<Message> predicate = (Message msg) => !string.IsNullOrEmpty(msg.AppId) && !existingAppIds.Contains(msg.AppId);
		messageFactory.DeleteAllMessages(predicate);
		List<string> list = (from setting in settingsPersistence.GetAllSettings()
			select setting.ApplicationId).Distinct().Except(existingAppIds).ToList();
		list.ForEach(delegate(string id)
		{
			Log.Debug(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Removing left-over settings for appID: [{id}]");
			settingsPersistence.RemoveAllApplicationSettings(id);
		});
		FilesystemStorage.KeepOnlyFoldersFor(existingAppIds);
		RemoveOrphanBaseDevices(existingAppIds);
	}

	private void RemoveOrphanBaseDevices(List<string> existingAppIds)
	{
		foreach (BaseDevice item in (from bd in configurationRepository.GetBaseDevices()
			where !existingAppIds.Contains(bd.AppId)
			select bd).ToList())
		{
			configurationRepository.DeleteBaseDevice(item.Id);
		}
	}

	private void ReactivateApp(ApplicationTokenEntry entry)
	{
		AddInConfiguration addInConfiguration = entry.GetAddInConfiguration();
		loadedApplications[entry.AppId].Application.Activate(addInConfiguration);
		loadedApplications[entry.AppId].State = ApplicationState.Active;
		entry.ActiveOnShc = true;
		FireProductStateUpdatedEvent(entry, this.ApplicationStateChanged, ApplicationStates.ApplicationActivated);
		messageFactory.CreateApplicationActivatedMessage(entry, GetInitiator(entry));
		loadedApplications[entry.AppId].EnabledByWebshop = entry.IsEnabledByWebshop;
		loadedApplications[entry.AppId].EnabledByUser = entry.IsEnabledByUser;
	}

	public void LoadApplication(ApplicationTokenEntry appEntry, bool lastAttempt, bool isNewApplication)
	{
		try
		{
			if (appEntry.ApplicationKind == ApplicationKind.SilverlightOnly)
			{
				appEntry.ActiveOnShc = true;
				return;
			}
			MainAssemblyData mainAssemblyData = null;
			mainAssemblyData = ((!registrationService.IsShcLocalOnly) ? mainAssemblyDataFactory.GetMainAssemblyData(appEntry, lastAttempt) : mainAssemblyDataFactory.GetMainAssemblyDataFromLocalAddin(appEntry));
			if (mainAssemblyData != null)
			{
				if (!CanLoadAssembly(mainAssemblyData, appEntry))
				{
					FireProductStateUpdatedEvent(appEntry, this.ApplicationStateChanged, ApplicationStates.ApplicationAdded);
					return;
				}
				Assembly assembly = Assembly.LoadFrom(mainAssemblyData.FileName);
				string text = assembly.GetName().Version.ToString();
				if (!string.IsNullOrEmpty(appEntry.Version) && string.Compare(text, appEntry.Version, StringComparison.OrdinalIgnoreCase) != 0)
				{
					Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"File {mainAssemblyData.FileName} has version {text} and in token is {appEntry.Version}.");
				}
				string[] typesToLoad = mainAssemblyData.TypesToLoad;
				foreach (string text2 in typesToLoad)
				{
					Log.Debug(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Found app type {text2} in file {mainAssemblyData.FileName}");
					IAddIn addIn = addInLoader.LoadApplication(assembly, text2);
					if (addIn == null || addIn.ApplicationId != appEntry.AppId || string.IsNullOrEmpty(addIn.ApplicationId))
					{
						continue;
					}
					if (!IsEnoughMemoryLeft())
					{
						int num = 0;
						bool flag = false;
						do
						{
							num++;
							Thread.Sleep(10000);
							flag = IsEnoughMemoryLeft();
						}
						while (!flag && num < 3);
						if (!flag)
						{
							break;
						}
					}
					loadedApplicationsEnvironments[addIn.ApplicationId] = new ApplicationEnvironment(addIn.ApplicationId, appEntry.Version, settingsPersistence, externalCommandDispatcher, networkMonitor, certManager, configurationRepository, container.Resolve<IRepositorySync>(), eventManager, mainAssemblyData.FileName, addinsConfigurationValidator, messageFactory.GetMessagesManager(), addIn as IConfigurationValidation, addinsConfigurationRepository, scheduler);
					addIn.Initialize(appEntry.GetAddInConfiguration(), loadedApplicationsEnvironments[addIn.ApplicationId]);
					loadedApplications[addIn.ApplicationId] = new ApplicationStateInfo(addIn, ApplicationState.Active)
					{
						Version = text,
						UpdateAvailable = appEntry.UpdateAvailable,
						Parameters = appEntry.Parameters,
						EnabledByUser = appEntry.IsEnabledByUser,
						EnabledByWebshop = appEntry.IsEnabledByWebshop
					};
					appEntry.ActiveOnShc = true;
					appEntry.AppId = addIn.ApplicationId;
				}
				if (!appEntry.ActiveOnShc)
				{
					Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {appEntry.AppId} could not be loaded from {mainAssemblyData.FileName}");
					FireProductStateUpdatedEvent(appEntry, this.ApplicationStateChanged, ApplicationStates.ApplicationAdded);
					throw new InvalidDataException("Could not initialize application");
				}
				Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {appEntry.AppId} was loaded from {mainAssemblyData.FileName}");
				if (isNewApplication)
				{
					FireProductStateUpdatedEvent(appEntry, this.ApplicationStateChanged, ApplicationStates.ApplicationActivated);
					Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {appEntry.AppId} was added from {mainAssemblyData.FileName}");
				}
				CheckApplicationUpdated(appEntry);
			}
			else
			{
				loadedApplications[appEntry.AppId].State = ApplicationState.LoadFailed;
			}
		}
		catch (AppDownloadException ex)
		{
			FireProductStateUpdatedEvent(appEntry, this.ApplicationStateChanged, ApplicationStates.ApplicationAdded);
			throw ex;
		}
		finally
		{
			messageFactory.UpdateApplicationLoadFailureAlert(appEntry);
		}
	}

	private void CheckApplicationUpdated(ApplicationTokenEntry appEntry)
	{
		string appVersion = applicationVersionStorage.GetAppVersion(appEntry.AppId);
		string version = appEntry.Version;
		if (!string.IsNullOrEmpty(appVersion) && !string.IsNullOrEmpty(version) && string.Compare(appVersion, version, StringComparison.OrdinalIgnoreCase) != 0)
		{
			PublishStateAfterUpdate(appEntry);
			string message = $"Application {appEntry.AppId} was updated from {appVersion} version to {version} version";
			Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, message);
		}
	}

	private bool CanLoadAssembly(MainAssemblyData mad, ApplicationTokenEntry appEntry)
	{
		if (!File.Exists(mad.FileName))
		{
			Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Could not find file for URI {appEntry.AppManifest}.");
			return false;
		}
		if (mad.TypesToLoad == null || mad.TypesToLoad.Length == 0)
		{
			Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"No types found to load for app {appEntry.AppId}");
			return false;
		}
		return IsEnoughMemoryLeft();
	}

	private void UninstallApplication(IAddIn app)
	{
		if (app == null)
		{
			Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "UninstallApplication(): null parameter provided");
			return;
		}
		string applicationId = app.ApplicationId;
		UninitializeApplicationForUninstall(app);
		try
		{
			loadedApplicationsEnvironments[applicationId].Uninstall();
			certManager.CleanupCertificates(applicationId);
			this.ApplicationStateChanged?.Invoke(new ApplicationLoadStateChangedEventArgs
			{
				Application = app,
				ApplicationState = ApplicationStates.ApplicationsUninstalled
			});
			Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {app.ApplicationId} was uninstalled.");
		}
		catch (Exception ex)
		{
			Log.Exception(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, ex, "Error cleaning up the environment {0}", app.ApplicationId);
		}
	}

	private static void UninitializeApplicationForUninstall(IAddIn app)
	{
		try
		{
			app.Uninitialize(CleanupMode.Uninstall);
		}
		catch (Exception ex)
		{
			Log.Exception(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, ex, "Error while uninstalling {0}. Proceeding with environment unloading", app.ApplicationId);
		}
	}

	private void DeactivateApplication(ApplicationTokenEntry entry)
	{
		IAddIn application = loadedApplications[entry.AppId].Application;
		if (application == null)
		{
			Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "DeactivateApplication(): null parameter provided");
			return;
		}
		try
		{
			application.Deactivate();
			FireProductStateUpdatedEvent(entry, this.ApplicationStateChanged, ApplicationStates.ApplicationDeactivated);
			Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {application.ApplicationId} was deactivated.");
			messageFactory.CreateApplicationDeactivatedMessage(entry, GetInitiator(entry));
			loadedApplications[entry.AppId].EnabledByWebshop = entry.IsEnabledByWebshop;
			loadedApplications[entry.AppId].EnabledByUser = entry.IsEnabledByUser;
		}
		catch (Exception ex)
		{
			Log.Error(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Error while deactivating {application.ApplicationId}: {ex.Message}");
		}
	}

	private void OnConnectivityChanged(ChannelConnectivityChangedEventArgs args)
	{
		if (!wasConnected && args.Connected)
		{
			int num = new RandomNumberGenerator().Next(300, 3600);
			Log.Information(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "Network connection has been restored. Checking app token in " + num + " seconds.");
			container.Resolve<IScheduler>().AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), CheckApplicationsToken, TimeSpan.FromSeconds(num), runOnce: true));
		}
		wasConnected = args.Connected;
	}

	private void CheckApplicationsToken()
	{
		if (registrationService.IsShcLocalOnly)
		{
			return;
		}
		container.Resolve<IBusinessLogic>().PerformBackendCommunicationWithRetries("Could not check token.", delegate(bool lastAttempt)
		{
			bool tokenUpdated;
			bool flag = FetchApplicationsToken(lastAttempt, out tokenUpdated);
			if (flag && tokenUpdated)
			{
				flag = LoadAllActiveApplications(lastAttempt);
			}
			return flag;
		});
	}

	private bool ShouldReloadApplicationTokenEntry(ApplicationTokenEntry entry)
	{
		if (entry == null || string.IsNullOrEmpty(entry.AppId))
		{
			return false;
		}
		return true;
	}

	private bool RestoreLoadedApplication(ApplicationTokenEntry entry, ApplicationStateInfo appState)
	{
		switch (appState.State)
		{
		case ApplicationState.LoadFailed:
			return false;
		case ApplicationState.Active:
		case ApplicationState.ActiveByDefault:
			entry.ActiveOnShc = true;
			try
			{
				appState.Application.Refresh(entry.GetAddInConfiguration());
			}
			catch (Exception arg2)
			{
				Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {entry.AppId} could not be refreshed: {arg2}");
				return false;
			}
			return true;
		case ApplicationState.Inactive:
			try
			{
				ReactivateApp(entry);
			}
			catch (Exception arg)
			{
				Log.Warning(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Application {entry.AppId} could not be activated: {arg}");
				return false;
			}
			return true;
		default:
			return false;
		}
	}

	public void CleanupAppRepository(ApplicationsToken token)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, ApplicationStateInfo> loadedApplication in loadedApplications)
		{
			if (loadedApplication.Value.State == ApplicationState.ActiveByDefault)
			{
				continue;
			}
			string key = loadedApplication.Key;
			if (token.Entries.All((ApplicationTokenEntry entry) => entry.AppId != key))
			{
				try
				{
					UninstallApplication(loadedApplication.Value.Application);
					messageFactory.CreateApplicationRemovedMessage(loadedApplication.Key);
					list.Add(loadedApplication.Key);
				}
				catch (Exception ex)
				{
					Log.Error(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, $"Failed to uninstall app {loadedApplication.Key}: {ex.Message}");
				}
			}
		}
		list.ForEach(delegate(string appId)
		{
			loadedApplications.Remove(appId);
		});
		AppModuleHandler.CleanupUnusedAppFolders(token);
		CleanupMissingApps(token.Entries.Select((ApplicationTokenEntry entry) => entry.AppId).ToList());
	}

	private void UpdateApplication(bool lastAttempt, ApplicationTokenEntry entry)
	{
		if (loadedApplications[entry.AppId].Application != null)
		{
			if (loadedApplications[entry.AppId].IsActive || loadedApplications[entry.AppId].State == ApplicationState.Inactive)
			{
				UninstallForUpgrade(loadedApplications[entry.AppId]);
				loadedApplications[entry.AppId].Application = null;
			}
			if (loadedApplications[entry.AppId].State != ApplicationState.Unknown)
			{
				LoadApplication(entry, lastAttempt, isNewApplication: false);
			}
		}
		PublishStateAfterUpdate(entry);
	}

	private void PublishStateAfterUpdate(ApplicationTokenEntry entry)
	{
		if (loadedApplications[entry.AppId].Application != null)
		{
			if (loadedApplications[entry.AppId].IsActive || loadedApplications[entry.AppId].State == ApplicationState.Inactive)
			{
				FireApplicationUpdatedEvent(entry);
				messageFactory.CreateApplicationUpdatedMessage(entry, success: true);
			}
		}
		else if (loadedApplications[entry.AppId].State == ApplicationState.LoadFailed)
		{
			messageFactory.CreateApplicationUpdatedMessage(entry, success: false);
		}
	}

	private string GetInitiator(ApplicationTokenEntry entry)
	{
		if (loadedApplications[entry.AppId].EnabledByUser != entry.IsEnabledByUser)
		{
			return "User";
		}
		if (loadedApplications[entry.AppId].EnabledByWebshop != entry.IsEnabledByWebshop)
		{
			return "Webshop";
		}
		return "Unknown";
	}

	private bool IsEnoughMemoryLeft()
	{
		int num = (isShcStarting ? sysData.MemoryLimitStartup : sysData.MemoryLimit);
		int memoryLoad = sysData.MemoryLoad;
		bool flag = num == 0 || memoryLoad < num;
		if (!flag)
		{
			Log.WarningFormat(RWE.SmartHome.SHC.Core.Module.ApplicationsHost, "AppHost", true, "Memory threshold reached, unable to load addin(s). Memory load: {0}, current limit: {1}", memoryLoad, num);
		}
		return flag;
	}
}
