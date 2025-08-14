using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogic.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.DomainModel.Constants;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using RWE.SmartHome.SHC.SHCRelayDriver;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class SHCBaseDeviceHandler : IShcBaseDeviceHandler, IVirtualCurrentPhysicalStateHandler
{
	private const string loggingSource = "SHCBaseDevice";

	private readonly IRepository configRepo;

	private readonly IEventManager eventManager;

	private readonly ITokenCache tokenCache;

	private EventHandler timeManagerEvent;

	private readonly IDeviceMonitor deviceMonitor;

	private readonly ISoftwareUpdateProcessor softwareUpdateProcessor;

	private readonly SHCBaseDevicePersistence persistence;

	private readonly List<Property> stateProperties = new List<Property>();

	private readonly Dispatcher dispatcher = new Dispatcher();

	private readonly VirtualResidentHandler virtualResidentHandler;

	private readonly IScheduler scheduler;

	private readonly Random randomizer;

	private int osStateResetTimeoutSeconds = 3600;

	private Timer osstateUpdaterTimer;

	public Guid? DeviceId => configRepo.GetShcBaseDevice()?.Id;

	public event EventHandler<VirtualDeviceAvailableArgs> StateChanged;

	public SHCBaseDeviceHandler(IRepository repository, IRepositorySync repositorySync, IEventManager eventManager, IScheduler scheduler, INotificationHandler notificationHandler, ITokenCache tokenCache, IDeviceMonitor deviceMonitor, ISoftwareUpdateProcessor softwareUpdateProcessor)
	{
		randomizer = new Random();
		configRepo = repository;
		this.eventManager = eventManager;
		this.tokenCache = tokenCache;
		this.deviceMonitor = deviceMonitor;
		this.softwareUpdateProcessor = softwareUpdateProcessor;
		persistence = new SHCBaseDevicePersistence(notificationHandler, repository, repositorySync);
		virtualResidentHandler = new VirtualResidentHandler(configRepo, scheduler, eventManager);
		this.scheduler = scheduler;
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartupCompletedRound2, (ShcStartupCompletedEventArgs x) => x.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, null, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<OSStateChangedEvent>().Subscribe(OnOSStateChanges, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DSTChangedEvent>().Subscribe(OnDaylightSavingTimeChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<PerformResetEvent>().Subscribe(OnShcReset, null, ThreadOption.PublisherThread, null);
		dispatcher.Start();
	}

	public PhysicalDeviceState GetState(Guid id)
	{
		BaseDevice shcBaseDevice = configRepo.GetShcBaseDevice();
		if (shcBaseDevice == null || shcBaseDevice.Id != id)
		{
			return null;
		}
		return GetPhysicalDeviceState(shcBaseDevice);
	}

	public void UpdateDiscoveryActiveProperty(bool value)
	{
		stateProperties.UpdateBooleanProperty(ShcBaseDeviceConstants.StateProperties.DiscoveryActive, value, OnStatePropertyUpdated, supportTimestamp: true);
	}

	public bool EnsureDevice()
	{
		BaseDevice baseDevice = new BaseDevice();
		AddBaseDeviceDefaultProperties(baseDevice);
		return persistence.AddShcBaseDevice(baseDevice);
	}

	private PhysicalDeviceState GetPhysicalDeviceState(BaseDevice baseDevice)
	{
		if (baseDevice == null)
		{
			return null;
		}
		UpdateOnRequestProperties();
		List<Property> collection = stateProperties.Select((Property x) => x.Clone()).ToList();
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.PhysicalDeviceId = baseDevice.Id;
		PhysicalDeviceState physicalDeviceState2 = physicalDeviceState;
		physicalDeviceState2.DeviceProperties.Properties.AddRange(collection);
		return physicalDeviceState2;
	}

	private void OnShcStartupCompletedRound2(ShcStartupCompletedEventArgs args)
	{
		Start();
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		if (args.ModifiedInteractions.Any())
		{
			virtualResidentHandler.RefreshVirtualResidentTasks();
		}
	}

	private void Start()
	{
		UpdateStartupProperties();
		HookEvents();
		AnnounceDevice();
		virtualResidentHandler.RefreshVirtualResidentTasks();
		Log.Information(Module.BusinessLogic, "SHCBaseDevice", "Started");
	}

	private void AddBaseDeviceDefaultProperties(BaseDevice baseDevice)
	{
		Log.Information(Module.BusinessLogic, "SHCBaseDevice", "Default properties set");
		baseDevice.ProtocolId = ProtocolIdentifier.Virtual;
		baseDevice.SerialNumber = SHCSerialNumber.SerialNumber();
		baseDevice.Manufacturer = "RWE";
		baseDevice.DeviceType = BuiltinPhysicalDeviceType.SHC.ToString();
		baseDevice.AppId = CoreConstants.CoreAppId;
		baseDevice.DeviceVersion = "1.1";
		baseDevice.TimeOfAcceptance = ShcDateTime.UtcNow;
		baseDevice.Name = "Smart Home Controller";
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.AttrHardwareVersion, "00.02", null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.HostName, "SMARTHOME01", null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.TimeZone, string.Empty, null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.MACAddress, string.Empty, null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.GeoLocation, null, null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.ShcType, string.Empty, null, supportTimestamp: false);
		baseDevice.Properties.UpdateBooleanProperty("ActivityLogEnabled", true, null, supportTimestamp: false);
		baseDevice.Properties.UpdateDateTimeProperty(ShcBaseDeviceConstants.ConfigurationProperties.RegistrationTime, ShcDateTime.UtcNow, null, supportTimestamp: false);
		baseDevice.Properties.UpdateBooleanProperty(ShcBaseDeviceConstants.ConfigurationProperties.BackendConnectionMonitored, false, null, supportTimestamp: false);
		baseDevice.Properties.UpdateBooleanProperty(ShcBaseDeviceConstants.ConfigurationProperties.RFCommFailureNotification, false, null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(PhysicalDeviceBasicProperties.FirmwareVersion, string.Empty, null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.SoftwareVersion, string.Empty, null, supportTimestamp: false);
		baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.UIConfigState, "New", null, supportTimestamp: false);
	}

	private void HookEvents()
	{
		Log.Information(Module.BusinessLogic, "SHCBaseDevice", "Listening for events");
		timeManagerEvent = TimeZoneManager_TimeZoneChanged;
		TimeZoneManager.TimeZoneChanged += timeManagerEvent;
		eventManager.GetEvent<NetworkCableAttachedEvent>().Subscribe(OnNetworkCableAttached, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<TokenCacheUpdateEvent>().Subscribe(OnTokenChacheUpdated, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SoftwareUpdateAvailableEvent>().Subscribe(OnSoftwareUpdateAvailable, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SoftwareUpdateNotAvailableEvent>().Subscribe(OnSoftwareUpdateNotAvailable, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<UsbDeviceConnectionChangedEvent>().Subscribe(OnUsbDeviceConnectionChanged, null, ThreadOption.PublisherThread, null);
	}

	private void UnhookEvents()
	{
		TimeZoneManager.TimeZoneChanged -= timeManagerEvent;
		eventManager.GetEvent<TokenCacheUpdateEvent>().Unsubscribe(OnTokenChacheUpdated);
		eventManager.GetEvent<SoftwareUpdateAvailableEvent>().Unsubscribe(OnSoftwareUpdateAvailable);
		eventManager.GetEvent<SoftwareUpdateNotAvailableEvent>().Unsubscribe(OnSoftwareUpdateNotAvailable);
		eventManager.GetEvent<NetworkCableAttachedEvent>().Unsubscribe(OnNetworkCableAttached);
		eventManager.GetEvent<UsbDeviceConnectionChangedEvent>().Unsubscribe(OnUsbDeviceConnectionChanged);
	}

	private void TimeZoneManager_TimeZoneChanged(object sender, EventArgs e)
	{
		persistence.Update(delegate(BaseDevice baseDevice, IRepository repository)
		{
			bool hasBdChanged = false;
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.TimeZone, TimeZoneManager.GetShcTimeZoneName(), delegate
			{
				hasBdChanged = true;
			}, supportTimestamp: false);
			return hasBdChanged;
		});
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.CurrentUTCOffset, (decimal)TimeZoneManager.GetShcUtcOffset().TotalMinutes, OnStatePropertyUpdated, supportTimestamp: true);
	}

	private void OnNetworkCableAttached(NetworkCableAttachedEventArgs args)
	{
		persistence.Update(delegate(BaseDevice baseDevice, IRepository repository)
		{
			bool hasBdChanged = false;
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.MACAddress, NetworkTools.GetDeviceMacAddress(), delegate
			{
				hasBdChanged = true;
			}, supportTimestamp: false);
			return hasBdChanged;
		});
		stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.IPAddress, NetworkTools.GetDeviceIp(), OnStatePropertyUpdated, supportTimestamp: true);
	}

	private void OnTokenChacheUpdated(TokenCacheUpdateEventArgs args)
	{
		persistence.Update(delegate(BaseDevice baseDevice, IRepository repository)
		{
			bool hasBdChanged = false;
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.ShcType, GetShcType(), delegate
			{
				hasBdChanged = true;
			}, supportTimestamp: false);
			return hasBdChanged;
		});
		if (!string.IsNullOrEmpty(args.Hash))
		{
			stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.ProductsHash, args.Hash, OnStatePropertyUpdated, supportTimestamp: true);
		}
	}

	private void OnSoftwareUpdateAvailable(SoftwareUpdateAvailableEventArgs args)
	{
		stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.UpdateAvailable, args.Version, OnStatePropertyUpdated, supportTimestamp: true);
	}

	private void OnSoftwareUpdateNotAvailable(SoftwareUpdateNotAvailableEventArgs args)
	{
		stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.UpdateAvailable, string.Empty, OnStatePropertyUpdated, supportTimestamp: true);
	}

	private void OnOSStateChanges(OSStateChangedEventArgs args)
	{
		UpdateOSState(args.NewOSState, ShouldNotifyOSStateChange(args.NewOSState));
	}

	private void OnShcReset(PerformResetEventArgs args)
	{
		UpdateOSState(OSState.Rebooting, notifyChange: true);
	}

	private void OnDaylightSavingTimeChanged(DSTChangedEventArgs args)
	{
		int num = randomizer.Next(1, 180);
		Log.Information(Module.BusinessLogic, $"Applying DST config update in {num} minutes");
		scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), ApplyDstConfigUpdates, TimeSpan.FromMinutes(num), runOnce: true));
	}

	private void ApplyDstConfigUpdates()
	{
		decimal value = (decimal)TimeZoneManager.GetShcUtcOffset().TotalMinutes;
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.CurrentUTCOffset, value, OnStatePropertyUpdated, supportTimestamp: true);
	}

	private void UpdateOSState(OSState newOSState, bool notifyChange)
	{
		if (notifyChange)
		{
			stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.OSState, newOSState.ToString(), OnStatePropertyUpdated, supportTimestamp: true);
		}
		else
		{
			stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.OSState, newOSState.ToString(), null, supportTimestamp: true);
		}
	}

	private void OnUsbDeviceConnectionChanged(UsbDeviceConnectionChangedEventArgs args)
	{
		string propertyName = ((args.ProtocolIdentifier == ProtocolIdentifier.wMBus) ? ShcBaseDeviceConstants.StateProperties.MBusDongleAttached : ShcBaseDeviceConstants.StateProperties.LBDongleAttached);
		stateProperties.UpdateBooleanProperty(propertyName, args.Connected, OnStatePropertyUpdated, supportTimestamp: true);
	}

	private void UpdateStartupProperties()
	{
		Log.Information(Module.BusinessLogic, "SHCBaseDevice", "Startup properties set");
		persistence.Update(delegate(BaseDevice baseDevice, IRepository repository)
		{
			bool propertiesUpdated = false;
			Action onNewValue = delegate
			{
				propertiesUpdated = true;
			};
			if (baseDevice.SerialNumber != SHCSerialNumber.SerialNumber())
			{
				baseDevice.SerialNumber = SHCSerialNumber.SerialNumber();
				propertiesUpdated = true;
			}
			propertiesUpdated = RemoveUnusedProperties(baseDevice) || propertiesUpdated;
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.HostName, NetworkTools.GetHostName(), onNewValue, supportTimestamp: false);
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.TimeZone, TimeZoneManager.GetShcTimeZoneName(), onNewValue, supportTimestamp: false);
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.MACAddress, NetworkTools.GetDeviceMacAddress(), onNewValue, supportTimestamp: false);
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.ShcType, GetShcType(), onNewValue, supportTimestamp: false);
			baseDevice.Properties.UpdateStringProperty(PhysicalDeviceBasicProperties.FirmwareVersion, SHCVersion.OsVersion, onNewValue, supportTimestamp: false);
			baseDevice.Properties.UpdateStringProperty(ShcBaseDeviceConstants.ConfigurationProperties.SoftwareVersion, SHCVersion.ApplicationVersion, onNewValue, supportTimestamp: false);
			if (!baseDevice.TimeOfAcceptance.HasValue)
			{
				DateTime? dateTimeValue = baseDevice.Properties.GetDateTimeValue(ShcBaseDeviceConstants.ConfigurationProperties.RegistrationTime);
				if (dateTimeValue.HasValue)
				{
					baseDevice.TimeOfAcceptance = dateTimeValue;
					propertiesUpdated = true;
				}
			}
			bool flag = FixFunctionParamValues(repository);
			bool flag2 = FixCorruptedInteractions(repository);
			return (flag || flag2 || propertiesUpdated) ? true : false;
		});
		stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.UpdateAvailable, softwareUpdateProcessor.AvailableUpdateVersion, null, supportTimestamp: true);
		stateProperties.UpdateDateTimeProperty(ShcBaseDeviceConstants.StateProperties.LastReboot, PerformanceMonitoring.StartTime(), null, supportTimestamp: true);
		stateProperties.UpdateBooleanProperty(ShcBaseDeviceConstants.StateProperties.MBusDongleAttached, false, null, supportTimestamp: true);
		stateProperties.UpdateBooleanProperty(ShcBaseDeviceConstants.StateProperties.LBDongleAttached, false, null, supportTimestamp: true);
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.ConfigurationVersion, configRepo.RepositoryVersion, null, supportTimestamp: true);
		stateProperties.UpdateBooleanProperty(ShcBaseDeviceConstants.StateProperties.DiscoveryActive, false, null, supportTimestamp: true);
		stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.IPAddress, NetworkTools.GetDeviceIp(), null, supportTimestamp: true);
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.CurrentUTCOffset, (decimal)TimeZoneManager.GetShcUtcOffset().TotalMinutes, null, supportTimestamp: true);
		using (TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("SHCBaseDevice/UpdateStartupProperties"))
		{
			if (tokenCacheContext.AppsToken != null)
			{
				stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.ProductsHash, tokenCacheContext.AppsToken.GetHash(), null, supportTimestamp: true);
			}
		}
		UpdateInitialOSState();
	}

	private bool FixFunctionParamValues(IRepository repository)
	{
		bool result = false;
		try
		{
			if (!FilePersistence.InteractionFunctionValuesFixed)
			{
				Log.Information(Module.BusinessLogic, "Starting function name fixing...");
				foreach (Interaction item in from m in repository.GetInteractions()
					select m.Clone())
				{
					if (InteractionChecker.FixFunctionValuesIfNecessary(item))
					{
						repository.SetInteraction(item);
						result = true;
						Log.Information(Module.BusinessLogic, $"Function values were fixed in interaction (id:{item.Id})");
					}
				}
				FilePersistence.InteractionFunctionValuesFixed = true;
				Log.Information(Module.BusinessLogic, "Ending function name fixing...");
			}
			else
			{
				Log.Information(Module.BusinessLogic, "No need to fix function value casing anymore.");
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"General error occurred when trying to update function parameter value casing: {ex.Message}");
		}
		return result;
	}

	private bool FixCorruptedInteractions(IRepository repository)
	{
		bool result = false;
		try
		{
			if (!FilePersistence.InteractionsVerified)
			{
				result = RemoveInteractionParameterIfNeeded(repository);
				FilePersistence.InteractionsVerified = true;
				Log.Information(Module.BusinessLogic, "Ending fixing the corrupted interactions...");
			}
			else
			{
				Log.Information(Module.BusinessLogic, "No need to fix the interactions.");
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"There is a problem verifying the interactions configuration {ex.Message} {ex.StackTrace}");
		}
		return result;
	}

	private static bool RemoveInteractionParameterIfNeeded(IRepository repository)
	{
		bool result = false;
		Log.Information(Module.BusinessLogic, "Starting verifying interaction configuration...");
		foreach (Interaction item in repository.GetInteractions().ToList())
		{
			foreach (Rule rule in item.Rules)
			{
				List<ActionDescription> list = rule.Actions.Where((ActionDescription action) => action.ActionType == "Toggle").ToList();
				foreach (ActionDescription item2 in list)
				{
					if (item2.Data.FirstOrDefault((Parameter parameter) => parameter.Name == "DelayTime") != null)
					{
						item2.Data.RemoveAll((Parameter parameter) => string.IsNullOrEmpty(parameter.Name));
						repository.SetInteraction(item);
						result = true;
						Log.Information(Module.BusinessLogic, $"Removed the unnecessary parameter in interaction (id:{item.Id})");
					}
				}
			}
		}
		return result;
	}

	private bool RemoveUnusedProperties(BaseDevice baseDevice)
	{
		bool flag = baseDevice.Properties.RemoveAll((Property m) => m.Name == ShcBaseDeviceConstants.StateProperties.CurrentUTCOffset) > 0;
		if (flag)
		{
			Log.Information(Module.BusinessLogic, "Removed CurrentUTCOffset config property from SHC BaseDevice");
		}
		return flag;
	}

	private void OnStatePropertyUpdated()
	{
		Log.Information(Module.BusinessLogic, "SHCBaseDevice", "State property updated");
		BaseDevice shcBaseDevice = configRepo.GetShcBaseDevice();
		PhysicalDeviceState physicalDeviceState = GetPhysicalDeviceState(shcBaseDevice);
		eventManager.GetEvent<PhysicalDeviceStateChangedEvent>().Publish(new PhysicalDeviceStateChangedEventArgs(shcBaseDevice.Id, physicalDeviceState));
	}

	private void UpdateOnRequestProperties()
	{
		Log.Information(Module.BusinessLogic, "SHCBaseDevice", "Updating on request properties");
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.ConfigurationVersion, configRepo.RepositoryVersion, null, supportTimestamp: true);
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.MemoryLoad, PerformanceMonitoring.GetGlobalMemoryStatus().LoadPercentage, null, supportTimestamp: true);
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.CPULoad, (decimal)Math.Round(PerformanceMonitoring.GetCPULoad()), null, supportTimestamp: true);
		stateProperties.UpdateNumericProperty(ShcBaseDeviceConstants.StateProperties.DiskUsage, (decimal)Math.Round(PerformanceMonitoring.GetDiskUsage()), null, supportTimestamp: true);
		string currentAppTokenHash = tokenCache.GetCurrentAppTokenHash();
		if (currentAppTokenHash != null)
		{
			stateProperties.UpdateStringProperty(ShcBaseDeviceConstants.StateProperties.ProductsHash, currentAppTokenHash, OnStatePropertyUpdated, supportTimestamp: true);
		}
	}

	private string GetShcType()
	{
		using TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("SHCBaseDevice/GetSHCType");
		return (tokenCacheContext.AppsToken != null) ? tokenCacheContext.AppsToken.ShcType.ToString(CultureInfo.InvariantCulture) : string.Empty;
	}

	private void AnnounceDevice()
	{
		EventHandler<VirtualDeviceAvailableArgs> stateChanged = this.StateChanged;
		BaseDevice shcBaseDevice = configRepo.GetShcBaseDevice();
		if (shcBaseDevice != null)
		{
			stateChanged?.Invoke(this, new VirtualDeviceAvailableArgs(shcBaseDevice.Id));
		}
	}

	private bool ShouldNotifyOSStateChange(OSState newState)
	{
		string stringValue = stateProperties.GetStringValue(ShcBaseDeviceConstants.StateProperties.OSState);
		if (newState == OSState.Normal)
		{
			if (!string.IsNullOrEmpty(stringValue))
			{
				return !(stringValue == OSState.Normal.ToString());
			}
			return false;
		}
		return true;
	}

	private void UpdateInitialOSState()
	{
		OSState oSState = (WasUpdatePerformed() ? OSState.Updated : OSState.Rebooted);
		UpdateOSState(oSState, notifyChange: true);
		if (oSState == OSState.Rebooted)
		{
			osstateUpdaterTimer = new Timer(DoOSStateTransition, null, osStateResetTimeoutSeconds * 1000, -1);
		}
	}

	internal void SetOsStateResetTimeout(int timeout)
	{
		osStateResetTimeoutSeconds = timeout;
	}

	private void DoOSStateTransition(object param)
	{
		string stringValue = stateProperties.GetStringValue(ShcBaseDeviceConstants.StateProperties.OSState);
		if (stringValue == OSState.Rebooted.ToString())
		{
			OnOSStateChanges(new OSStateChangedEventArgs(OSState.Normal));
			if (osstateUpdaterTimer != null)
			{
				osstateUpdaterTimer.Dispose();
			}
		}
	}

	private bool WasUpdatePerformed()
	{
		return UpdatePerformedHandling.WasUpdatePerformed() == UpdatePerformedStatus.Successful;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			UnhookEvents();
		}
	}
}
