using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using RWE.SmartHome.SHC.DeviceActivity.DeviceActivityInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SmartHome.SHC.API.DeviceActivityLogging;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity;

internal class DeviceActivityLogger : IDeviceActivityLogger, IService, IShcBaseDeviceWatcher
{
	private const int MaxLogEntriesCount = 1000;

	private readonly int batchSize;

	private SubscriptionToken deviceStateChangedSubscription;

	private SubscriptionToken shcStartupSubscription;

	private SubscriptionToken shcStartupSubscriptionRound2;

	private SubscriptionToken backendConnectivitySubscription;

	private SubscriptionToken networkAccessAllowedSubscription;

	private SubscriptionToken sensorTriggeredSubscription;

	private SubscriptionToken softwareUpdateEventSubscription;

	private SubscriptionToken memoryShortageEventTracking;

	private SubscriptionToken reachabilityEventSubscription;

	private SubscriptionToken deviceActivitySubscriptionToken;

	private SubscriptionToken localCommunicationStatusSubscriptionToken;

	private readonly IEventManager eventManager;

	private readonly IDeviceActivityPersistence logDataPersistence;

	private readonly ITrackDataPersistence trackDataPersistence;

	private readonly IRepository configRepository;

	private readonly INetworkingMonitor networkMonitor;

	private readonly IBusinessLogic businessLogic;

	private readonly IDataTrackingClient storageClient;

	private readonly DeviceActivityLoggerScheduler activityLoggerScheduler;

	private volatile int logEntriesCount;

	private readonly Dispatcher dispatcher = new Dispatcher();

	private object logAccessSync = new object();

	private volatile bool dalEnabled = true;

	private readonly IDalTypeResolver dalTypeResolver;

	private readonly DeviceActivityReporter deviceActivityReporter;

	private readonly DalDataConverter dalDataConverter;

	private readonly IRegistrationService registrationService;

	private Dictionary<Guid, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> lastRecordedStates = new Dictionary<Guid, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>();

	private Guid shcId = Guid.Empty;

	internal DeviceActivityLogger(IEventManager eventManager, IDeviceActivityPersistence logDataPersistence, ITrackDataPersistence trackDataPersistence, IRepository configRepository, INetworkingMonitor networkMonitor, IBusinessLogic businessLogic, IDataTrackingClient storageClient, IScheduler scheduler, int batchSize, IDalTypeResolver dalTypeResolver, IRegistrationService registrationService)
	{
		this.eventManager = eventManager;
		this.logDataPersistence = logDataPersistence;
		this.trackDataPersistence = trackDataPersistence;
		this.configRepository = configRepository;
		this.networkMonitor = networkMonitor;
		this.businessLogic = businessLogic;
		this.storageClient = storageClient;
		this.batchSize = batchSize;
		this.dalTypeResolver = dalTypeResolver;
		this.registrationService = registrationService;
		deviceActivityReporter = new DeviceActivityReporter(eventManager, logDataPersistence, scheduler, configRepository, FlushPendingData);
		dalDataConverter = new DalDataConverter(configRepository);
		activityLoggerScheduler = new DeviceActivityLoggerScheduler(SendPendingDataToBackend);
		shcStartupSubscription = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStarted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.SubscriberThread, dispatcher);
		shcStartupSubscriptionRound2 = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartedRound2, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, dispatcher);
		logEntriesCount = logDataPersistence.GetLogCount();
	}

	public void DeleteDeviceActivityData()
	{
		Log.Debug(Module.DeviceActivity, "Removing device activity data.");
		dispatcher.Dispatch(new Executable<object>
		{
			Action = DeleteActivityData,
			Argument = null
		});
	}

	public void FlushPendingData()
	{
		activityLoggerScheduler.ForceFlush();
	}

	public void Initialize()
	{
		if (deviceStateChangedSubscription == null)
		{
			deviceStateChangedSubscription = eventManager.GetEvent<LogicalDeviceStateChangedEvent>().Subscribe(OnDeviceStateChanged, null, ThreadOption.SubscriberThread, dispatcher);
		}
		if (shcStartupSubscription == null)
		{
			shcStartupSubscription = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStarted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.SubscriberThread, dispatcher);
		}
		if (shcStartupSubscriptionRound2 == null)
		{
			shcStartupSubscriptionRound2 = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartedRound2, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, dispatcher);
		}
		if (backendConnectivitySubscription == null)
		{
			backendConnectivitySubscription = eventManager.GetEvent<ChannelConnectivityChangedEvent>().Subscribe(OnConnectivityChanged, null, ThreadOption.SubscriberThread, dispatcher);
		}
		if (networkAccessAllowedSubscription == null)
		{
			networkAccessAllowedSubscription = eventManager.GetEvent<InternetAccessAllowedChangedEvent>().Subscribe(delegate
			{
				MonitorLogSize(recordAdded: false);
			}, (InternetAccessAllowedChangedEventArgs args) => args.InternetAccessAllowed, ThreadOption.SubscriberThread, dispatcher);
		}
		if (sensorTriggeredSubscription == null)
		{
			sensorTriggeredSubscription = eventManager.GetEvent<DeviceEventDetectedEvent>().Subscribe(OnSensorTriggered, null, ThreadOption.SubscriberThread, dispatcher);
		}
		if (softwareUpdateEventSubscription == null)
		{
			softwareUpdateEventSubscription = eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(OnSoftwareUpdate, null, ThreadOption.PublisherThread, null);
		}
		if (memoryShortageEventTracking == null)
		{
			memoryShortageEventTracking = eventManager.GetEvent<MemoryShortageEvent>().Subscribe(OnMemoryTrackingEvent, null, ThreadOption.PublisherThread, null);
		}
		if (reachabilityEventSubscription == null)
		{
			reachabilityEventSubscription = eventManager.GetEvent<DeviceUnreachableChangedEvent>().Subscribe(OnReachabilityChanged, null, ThreadOption.SubscriberThread, dispatcher);
		}
		if (deviceActivitySubscriptionToken == null)
		{
			deviceActivitySubscriptionToken = eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, null, ThreadOption.SubscriberThread, dispatcher);
		}
		if (localCommunicationStatusSubscriptionToken == null)
		{
			localCommunicationStatusSubscriptionToken = eventManager.GetEvent<LocalCommunicationStatusEvent>().Subscribe(OnLocalCommunicationStatusChanged, null, ThreadOption.SubscriberThread, dispatcher);
		}
		dispatcher.Start();
	}

	public void Uninitialize()
	{
		dispatcher.Stop();
		if (deviceStateChangedSubscription != null)
		{
			eventManager.GetEvent<LogicalDeviceStateChangedEvent>().Unsubscribe(deviceStateChangedSubscription);
			deviceStateChangedSubscription = null;
		}
		if (shcStartupSubscription != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(shcStartupSubscription);
			shcStartupSubscription = null;
		}
		if (shcStartupSubscriptionRound2 != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(shcStartupSubscriptionRound2);
			shcStartupSubscriptionRound2 = null;
		}
		if (backendConnectivitySubscription != null)
		{
			eventManager.GetEvent<ChannelConnectivityChangedEvent>().Unsubscribe(backendConnectivitySubscription);
			backendConnectivitySubscription = null;
		}
		if (networkAccessAllowedSubscription != null)
		{
			eventManager.GetEvent<InternetAccessAllowedChangedEvent>().Unsubscribe(networkAccessAllowedSubscription);
			networkAccessAllowedSubscription = null;
		}
		if (sensorTriggeredSubscription != null)
		{
			eventManager.GetEvent<DeviceEventDetectedEvent>().Unsubscribe(sensorTriggeredSubscription);
			sensorTriggeredSubscription = null;
		}
		if (softwareUpdateEventSubscription != null)
		{
			eventManager.GetEvent<SoftwareUpdateProgressEvent>().Unsubscribe(softwareUpdateEventSubscription);
			softwareUpdateEventSubscription = null;
		}
		if (memoryShortageEventTracking != null)
		{
			eventManager.GetEvent<MemoryShortageEvent>().Unsubscribe(memoryShortageEventTracking);
			memoryShortageEventTracking = null;
		}
		if (reachabilityEventSubscription != null)
		{
			eventManager.GetEvent<DeviceUnreachableChangedEvent>().Unsubscribe(reachabilityEventSubscription);
			reachabilityEventSubscription = null;
		}
		if (deviceActivitySubscriptionToken != null)
		{
			eventManager.GetEvent<ConfigurationProcessedEvent>().Unsubscribe(deviceActivitySubscriptionToken);
			deviceActivitySubscriptionToken = null;
		}
		if (localCommunicationStatusSubscriptionToken != null)
		{
			eventManager.GetEvent<LocalCommunicationStatusEvent>().Unsubscribe(localCommunicationStatusSubscriptionToken);
			localCommunicationStatusSubscriptionToken = null;
		}
		if (activityLoggerScheduler != null)
		{
			activityLoggerScheduler.StopScheduler();
		}
	}

	public void ProcessUpdate(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property[] oldShcBaseDeviceProperties, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property[] newShcBaseDeviceProperties)
	{
		bool flag = dalEnabled;
		dalEnabled = ReadDalState(newShcBaseDeviceProperties);
		if (flag != dalEnabled)
		{
			eventManager.GetEvent<DeviceActivityLoggingChangedEvent>().Publish(new DeviceActivityLoggingChangedEventArgs
			{
				DALState = (dalEnabled ? DeviceActivityLoggingState.DALEnabled : DeviceActivityLoggingState.DALDisabled)
			});
		}
	}

	private void DeleteActivityData(object obj)
	{
		logDataPersistence.RemoveEntries();
	}

	private void OnDeviceStateChanged(LogicalDeviceStateChangedEventArgs args)
	{
		try
		{
			if (args == null || args.NewLogicalDeviceState == null || args.NewLogicalDeviceState.GetProperties() == null || args.OldLogicalDeviceState == null || args.OldLogicalDeviceState.GetProperties() == null)
			{
				Log.Debug(Module.DeviceActivity, "Invalid logical device state data received. Discarding event. Device might have become unreachable or recovered from unreachable state");
				return;
			}
			LogicalDevice logicalDevice = configRepository.GetLogicalDevice(args.LogicalDeviceId);
			if (logicalDevice == null)
			{
				Log.Warning(Module.DeviceActivity, "Device not found in configuration. Will not store data: " + args.LogicalDeviceId);
			}
			else
			{
				if (!dalEnabled || logicalDevice.ActivityLogActive != true || dalTypeResolver.GetDeviceLoggingType(logicalDevice) != DeviceActivityLoggingType.Core)
				{
					return;
				}
				RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property property = GetProperty(args.NewLogicalDeviceState, logicalDevice.PrimaryPropertyName);
				if (property == null || string.IsNullOrEmpty(property.GetValueAsString()))
				{
					Log.Debug(Module.DeviceActivity, $"Logical device state property [{logicalDevice.PrimaryPropertyName}] not found or value is empty");
					return;
				}
				RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property property2 = GetProperty(args.OldLogicalDeviceState, logicalDevice.PrimaryPropertyName);
				if (property2 != null && property2.GetValueAsString() == property.GetValueAsString())
				{
					Log.Debug(Module.DeviceActivity, $"Received identical information on the Display property, discarding: {property.Name} {property.GetValueAsString()}={property2.GetValueAsString()}");
					return;
				}
				if (IsNullOrEmpty(property2) && lastRecordedStates.TryGetValue(args.LogicalDeviceId, out var value) && value.GetValueAsString() == property.GetValueAsString())
				{
					Log.Debug(Module.DeviceActivity, $"Received identical information as the last recorded information, discarding: {property.Name} {property.GetValueAsString()}={value}");
					return;
				}
				RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property deviceSpecificStateChange = GetDeviceSpecificStateChange(logicalDevice, property);
				if (deviceSpecificStateChange != null)
				{
					lock (logAccessSync)
					{
						DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
						deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
						deviceActivityLogEntry.DeviceId = args.LogicalDeviceId.ToString();
						deviceActivityLogEntry.ActivityType = ActivityType.LogicalDeviceStateChanged;
						deviceActivityLogEntry.NewState = deviceSpecificStateChange.GetValueAsString();
						deviceActivityLogEntry.EventType = EventType.DalEntry;
						DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
						trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
						logDataPersistence.AddEntry(deviceActivityLogEntry2);
						MonitorLogSize(recordAdded: true);
						lastRecordedStates[args.LogicalDeviceId] = deviceSpecificStateChange;
					}
					Log.Debug(Module.DeviceActivity, $"Device {args.LogicalDeviceId.ToString()} event was stored.");
				}
				else
				{
					Log.Debug(Module.DeviceActivity, $"Device {args.LogicalDeviceId.ToString()} state change event discarded.");
				}
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.DeviceActivity, $"OnDeviceStateChanged failed, the event may be lost: {arg} ");
		}
	}

	private bool IsNullOrEmpty(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property previousPropertyValue)
	{
		if (previousPropertyValue == null)
		{
			return true;
		}
		return string.IsNullOrEmpty(previousPropertyValue.GetValueAsString());
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property GetDeviceSpecificStateChange(LogicalDevice device, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property newStateToBeLogged)
	{
		lastRecordedStates.TryGetValue(device.Id, out var _);
		if (device is SmokeDetectorSensor)
		{
			BooleanProperty booleanProperty = newStateToBeLogged as BooleanProperty;
			if (booleanProperty.Value == false)
			{
				Log.Debug(Module.DeviceActivity, $"Received smoke sensor deactivation event [{booleanProperty.Name}={booleanProperty.Value}]. The state change will not be recorded.");
				return null;
			}
		}
		return newStateToBeLogged;
	}

	private void OnShcStarted(ShcStartupCompletedEventArgs args)
	{
		Guid guid = GetShcId();
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property[] shcBdProperties = ((configRepository.GetShcBaseDevice() == null) ? null : configRepository.GetShcBaseDevice().Properties.ToArray());
		dalEnabled = ReadDalState(shcBdProperties);
		DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
		deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
		deviceActivityLogEntry.DeviceId = guid.ToString();
		deviceActivityLogEntry.ActivityType = ActivityType.ShcStarted;
		deviceActivityLogEntry.NewState = string.Empty;
		deviceActivityLogEntry.EventType = ((!dalEnabled) ? EventType.ShcTrackingEvent : EventType.Both);
		DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
		lock (logAccessSync)
		{
			TrackData trackData = dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2);
			if (trackData != null)
			{
				trackDataPersistence.Add(trackData);
			}
			logDataPersistence.AddEntry(deviceActivityLogEntry2);
			MonitorLogSize(recordAdded: true);
		}
	}

	private void OnShcStartedRound2(ShcStartupCompletedEventArgs args)
	{
		activityLoggerScheduler.StartScheduler();
	}

	private void OnConnectivityChanged(ChannelConnectivityChangedEventArgs args)
	{
		Guid guid = GetShcId();
		DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
		deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
		deviceActivityLogEntry.DeviceId = guid.ToString();
		deviceActivityLogEntry.ActivityType = ActivityType.ShcConnectivityStateChanged;
		deviceActivityLogEntry.NewState = args.Connected.ToString();
		deviceActivityLogEntry.EventType = ((!dalEnabled) ? EventType.ShcTrackingEvent : EventType.Both);
		DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
		lock (logAccessSync)
		{
			trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
			logDataPersistence.AddEntry(deviceActivityLogEntry2);
			MonitorLogSize(recordAdded: true);
		}
	}

	private void OnSensorTriggered(DeviceEventDetectedEventArgs args)
	{
		if (!dalEnabled)
		{
			return;
		}
		LogicalDevice logicalDevice = configRepository.GetLogicalDevice(args.LogicalDeviceId);
		if (logicalDevice != null && logicalDevice.ActivityLogActive == true)
		{
			lock (logAccessSync)
			{
				DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
				deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
				deviceActivityLogEntry.DeviceId = args.LogicalDeviceId.ToString();
				deviceActivityLogEntry.ActivityType = ActivityType.LogicalDeviceStateChanged;
				deviceActivityLogEntry.NewState = ((args.EventDetails.Count > 0) ? args.EventDetails[0].GetValueAsComparable().ToString() : string.Empty);
				deviceActivityLogEntry.EventType = EventType.DalEntry;
				DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
				trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
				logDataPersistence.AddEntry(deviceActivityLogEntry2);
				MonitorLogSize(recordAdded: true);
			}
		}
	}

	private bool IsLoggingSupportedForLogicalDeviceType(LogicalDevice logicalDevice)
	{
		if (!(logicalDevice is PushButtonSensor))
		{
			return logicalDevice is MotionDetectionSensor;
		}
		return true;
	}

	private void MonitorLogSize(bool recordAdded)
	{
		if (recordAdded)
		{
			logEntriesCount++;
		}
		if (logEntriesCount > 0 && logEntriesCount % batchSize == 0 && networkMonitor.InternetAccessAllowed)
		{
			FlushPendingData();
		}
		if (logEntriesCount > 1000)
		{
			lock (logAccessSync)
			{
				Log.Warning(Module.DeviceActivity, "MaxLogEntriesCount reached. Values will be rolled over");
				logDataPersistence.PurgeOldestEntries(batchSize);
				logEntriesCount = logDataPersistence.GetLogCount();
			}
		}
	}

	private bool SameBatch(List<DeviceActivityLogEntry> newBatch, List<DeviceActivityLogEntry> oldBatch)
	{
		if (oldBatch == null || newBatch == null)
		{
			return false;
		}
		if (oldBatch.Last().EntryId < newBatch.First().EntryId)
		{
			return false;
		}
		return newBatch.Select((DeviceActivityLogEntry e) => e.EntryId).Intersect(oldBatch.Select((DeviceActivityLogEntry e) => e.EntryId)).Any();
	}

	private bool SendPendingDataToBackend()
	{
		if (registrationService.IsShcLocalOnly)
		{
			Log.Information(Module.DeviceActivity, "SHC is not registered in backend, do not send DAL data to backend");
			return false;
		}
		bool flag = true;
		lock (logAccessSync)
		{
			List<DeviceActivityLogEntry> oldBatch = null;
			int num = 0;
			int num2 = -1;
			bool flag2 = true;
			while (++num <= 11)
			{
				try
				{
					List<DeviceActivityLogEntry> list = logDataPersistence.GetOldestEvents(batchSize);
					if (list == null || list.Count == 0)
					{
						list = logDataPersistence.GetPendingEvents();
						flag2 = false;
					}
					if (list == null || list.Count == 0)
					{
						break;
					}
					if (SameBatch(list, oldBatch))
					{
						Log.Warning(Module.DeviceActivity, "Found duplicate batch. Dropping DAL data.");
						logDataPersistence.PurgeAllEntries();
						break;
					}
					Log.Debug(Module.DeviceActivity, $"Storing {list.Count} entries to backend.");
					flag = storageClient.StoreData(list.ConvertAll((DeviceActivityLogEntry e) => dalDataConverter.ConvertToTrackData(e)));
					if (!flag)
					{
						Log.Error(Module.DeviceActivity, $"Failed to upload device activity data - service error.");
						break;
					}
					Log.Debug(Module.DeviceActivity, "Entries were stored successfully.");
					oldBatch = list;
					try
					{
						if (flag2)
						{
							logDataPersistence.RemoveEntriesById(list.First().EntryId, list.Last().EntryId);
							num2 = list[0].EntryId;
						}
						else
						{
							logDataPersistence.RemoveFromLocalCache(list);
						}
					}
					catch (Exception arg)
					{
						Log.Error(Module.DeviceActivity, $"Could not delete data: {arg}");
						logDataPersistence.PurgeAllEntries();
						goto end_IL_003c;
					}
					continue;
					end_IL_003c:;
				}
				catch (Exception arg2)
				{
					Log.Error(Module.DeviceActivity, $"Failed to upload device activity data: {arg2}");
					flag = false;
				}
				break;
			}
			logEntriesCount = logDataPersistence.GetLogCount();
			if ((double)num2 > 1000000000.0 && logEntriesCount == 0)
			{
				logDataPersistence.PurgeAllEntries();
			}
		}
		return flag;
	}

	private void OnSoftwareUpdate(SoftwareUpdateProgressEventArgs args)
	{
		if (args.State == SoftwareUpdateState.Started)
		{
			activityLoggerScheduler.ForceFlush();
		}
		else if (args.State == SoftwareUpdateState.Success || args.State == SoftwareUpdateState.Failed)
		{
			Guid guid = GetShcId();
			DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
			deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
			deviceActivityLogEntry.DeviceId = guid.ToString();
			deviceActivityLogEntry.ActivityType = ActivityType.ShcSoftwareUpdate;
			deviceActivityLogEntry.EventType = ((!dalEnabled) ? EventType.ShcTrackingEvent : EventType.Both);
			DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
			if (args.State == SoftwareUpdateState.Success)
			{
				deviceActivityLogEntry2.NewState = true.ToString();
			}
			else if (args.State == SoftwareUpdateState.Failed)
			{
				deviceActivityLogEntry2.NewState = false.ToString();
			}
			lock (logAccessSync)
			{
				trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
				logDataPersistence.AddEntry(deviceActivityLogEntry2);
				MonitorLogSize(recordAdded: true);
			}
		}
	}

	private void OnMemoryTrackingEvent(MemoryShortageEventArgs args)
	{
		if (args != null && !args.IsStartup)
		{
			DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
			deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
			deviceActivityLogEntry.DeviceId = GetShcId().ToString();
			deviceActivityLogEntry.ActivityType = (args.IsShortage ? ActivityType.ShcMemoryLoadExceeded : ActivityType.ShcMemoryLoadOK);
			deviceActivityLogEntry.EventType = EventType.ShcTrackingEvent;
			deviceActivityLogEntry.NewState = args.MemoryLoad.ToString();
			DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
			lock (logAccessSync)
			{
				trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
				logDataPersistence.AddEntry(deviceActivityLogEntry2);
				MonitorLogSize(recordAdded: true);
			}
		}
	}

	private void OnReachabilityChanged(DeviceUnreachableChangedEventArgs args)
	{
		if (args == null)
		{
			return;
		}
		BaseDevice baseDevice = configRepository.GetBaseDevice(args.DeviceId);
		if (baseDevice == null)
		{
			return;
		}
		DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
		deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
		deviceActivityLogEntry.DeviceId = args.DeviceId.ToString();
		deviceActivityLogEntry.ActivityType = (args.Unreachable ? ActivityType.ShcDeviceUnReachable : ActivityType.ShcDeviceReachable);
		deviceActivityLogEntry.EventType = EventType.ShcTrackingEvent;
		deviceActivityLogEntry.NewState = baseDevice.DeviceType;
		DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
		if (string.IsNullOrEmpty(deviceActivityLogEntry2.NewState))
		{
			Log.Debug(Module.DeviceActivity, $"SHC tracking reachability for unsupported device: {baseDevice.Id} [{baseDevice.AppId}]");
			return;
		}
		lock (logAccessSync)
		{
			trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
			logDataPersistence.AddEntry(deviceActivityLogEntry2);
			MonitorLogSize(recordAdded: true);
		}
	}

	private Guid GetShcId()
	{
		if (shcId == Guid.Empty)
		{
			BaseDevice shcBaseDevice = configRepository.GetShcBaseDevice();
			if (shcBaseDevice != null)
			{
				shcId = shcBaseDevice.Id;
			}
		}
		return shcId;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property GetProperty(LogicalDeviceState logicalDeviceState, string propertyName)
	{
		return (from p in logicalDeviceState.GetProperties()
			where p != null && p.Name == propertyName
			select p).FirstOrDefault();
	}

	private bool ReadDalState(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property[] shcBdProperties)
	{
		if (shcBdProperties == null)
		{
			Log.Warning(Module.DeviceActivity, "SHC base device is NULL. DAL is globally enabled.");
			return true;
		}
		if (shcBdProperties.FirstOrDefault((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property x) => x.Name.Equals("ActivityLogEnabled")) is BooleanProperty { Value: var value })
		{
			return value == true;
		}
		return false;
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs obj)
	{
		bool flag = false;
		if (obj.ConfigurationPhase != ConfigurationProcessedPhase.ValidatedInternally)
		{
			return;
		}
		Log.Debug(Module.DeviceActivity, $"#IN OnConfigurationChanged with {obj.ModifiedBaseDevices.Count} modified devices and {obj.DeletedBaseDevices.Count} deleted devices");
		BaseDevice modifiedBaseDevice;
		foreach (BaseDevice modifiedBaseDevice2 in obj.ModifiedBaseDevices)
		{
			modifiedBaseDevice = modifiedBaseDevice2;
			if (obj.RepositoryInclusionReport != null && obj.RepositoryInclusionReport.Any((EntityMetadata dr) => dr.Id == modifiedBaseDevice.Id && dr.EntityType == EntityType.BaseDevice))
			{
				flag = true;
				Log.Debug(Module.DeviceActivity, $"Device with ID:{modifiedBaseDevice.Id} has been included");
				lock (logAccessSync)
				{
					string text = modifiedBaseDevice.DeviceType ?? "null";
					DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
					deviceActivityLogEntry.ActivityType = ActivityType.DeviceIncluded;
					deviceActivityLogEntry.DeviceId = modifiedBaseDevice.Id.ToString();
					deviceActivityLogEntry.EventType = EventType.ShcTrackingEvent;
					deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
					deviceActivityLogEntry.NewState = $"{modifiedBaseDevice.AppId};{text};{modifiedBaseDevice.Id};{modifiedBaseDevice.SerialNumber};{string.Empty}";
					DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
					trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
					logDataPersistence.AddEntry(deviceActivityLogEntry2);
				}
			}
		}
		BaseDevice deletedBaseDevice;
		foreach (BaseDevice deletedBaseDevice2 in obj.DeletedBaseDevices)
		{
			deletedBaseDevice = deletedBaseDevice2;
			if (obj.RepositoryDeletionReport != null && obj.RepositoryDeletionReport.Any((EntityMetadata dr) => dr.Id == deletedBaseDevice.Id && dr.EntityType == EntityType.BaseDevice))
			{
				flag = true;
				Log.Debug(Module.DeviceActivity, $"Device with ID:{deletedBaseDevice.Id} has been excluded");
				lock (logAccessSync)
				{
					DeviceActivityLogEntry deviceActivityLogEntry3 = new DeviceActivityLogEntry();
					deviceActivityLogEntry3.ActivityType = ActivityType.DeviceExcluded;
					deviceActivityLogEntry3.DeviceId = deletedBaseDevice.Id.ToString();
					deviceActivityLogEntry3.EventType = EventType.ShcTrackingEvent;
					deviceActivityLogEntry3.Timestamp = DateTime.UtcNow;
					deviceActivityLogEntry3.NewState = $"{deletedBaseDevice.AppId};{deletedBaseDevice.DeviceType};{deletedBaseDevice.Id};{deletedBaseDevice.SerialNumber};{deletedBaseDevice.Manufacturer}";
					DeviceActivityLogEntry deviceActivityLogEntry4 = deviceActivityLogEntry3;
					trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry4));
					logDataPersistence.AddEntry(deviceActivityLogEntry4);
				}
			}
		}
		if (flag)
		{
			FlushPendingData();
		}
	}

	private void OnLocalCommunicationStatusChanged(LocalCommunicationStatusEventArgs eventArgs)
	{
		lock (logAccessSync)
		{
			DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
			deviceActivityLogEntry.ActivityType = ActivityType.LocalCommunicationStatus;
			deviceActivityLogEntry.DeviceId = eventArgs.DeviceId.ToString();
			deviceActivityLogEntry.EventType = EventType.Both;
			deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
			deviceActivityLogEntry.NewState = eventArgs.Status.ToString();
			DeviceActivityLogEntry deviceActivityLogEntry2 = deviceActivityLogEntry;
			trackDataPersistence.Add(dalDataConverter.ConvertToTrackData(deviceActivityLogEntry2));
			logDataPersistence.AddEntry(deviceActivityLogEntry2);
		}
	}
}
