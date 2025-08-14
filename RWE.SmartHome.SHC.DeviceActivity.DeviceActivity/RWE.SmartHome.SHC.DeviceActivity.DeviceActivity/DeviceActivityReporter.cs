using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity;

public class DeviceActivityReporter
{
	private readonly IDeviceActivityPersistence logDataPersistence;

	private readonly Dispatcher dispatcher = new Dispatcher();

	private readonly IScheduler scheduler;

	private readonly IRepository repository;

	private readonly IEventManager eventManager;

	private readonly object syncRoot = new object();

	internal List<BaseDevice> activeDevices = new List<BaseDevice>();

	private readonly Action postClearEvent;

	private bool flushRestricted;

	private readonly Random randomizer;

	private List<string> deviceTypesToIgnore = new List<string>
	{
		BuiltinPhysicalDeviceType.SHC.ToString(),
		BuiltinPhysicalDeviceType.VRCC.ToString(),
		BuiltinPhysicalDeviceType.NotificationSender.ToString(),
		"VariableActuator",
		"SunriseSunsetSensor"
	};

	internal event Action ProcessingDone;

	public DeviceActivityReporter(IEventManager eventManager, IDeviceActivityPersistence logDataPersistence, IScheduler scheduler, IRepository repository, Action postClearEvent)
	{
		this.logDataPersistence = logDataPersistence;
		this.scheduler = scheduler;
		this.repository = repository;
		this.postClearEvent = postClearEvent;
		this.eventManager = eventManager;
		flushRestricted = false;
		randomizer = new Random();
		SubscribeEvents();
		AddMonthlyResetTask();
		dispatcher.Start();
	}

	private void AddMonthlyResetTask()
	{
		scheduler.AddSchedulerTask(new DayOfMonthSchedulerTask(Guid.NewGuid(), ClearActiveDevices, ShcDateTime.UtcNow.Date.ToLocalTime(), 1u, 4095));
	}

	private void SubscribeEvents()
	{
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnDbAvailable, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.DatabaseAvailable, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStarted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound2, ThreadOption.SubscriberThread, dispatcher);
		eventManager.GetEvent<LogicalDeviceStateChangedEvent>().Subscribe(OnDeviceStateChanged, null, ThreadOption.SubscriberThread, dispatcher);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, null, ThreadOption.SubscriberThread, dispatcher);
		eventManager.GetEvent<ShcRebootScheduledEvent>().Subscribe(OnRebootScheduled, null, ThreadOption.PublisherThread, null);
	}

	private void OnRebootScheduled(ShcRebootScheduledEventArgs args)
	{
		PersistActiveDevices();
	}

	private void OnDbAvailable(ShcStartupCompletedEventArgs args)
	{
		LoadActiveDevices();
		StartDalFlushRestriction();
	}

	private void OnShcStarted(ShcStartupCompletedEventArgs args)
	{
		RaiseProcessingDoneEvent();
	}

	private void LoadActiveDevices()
	{
		Log.Information(Module.DeviceActivity, "Loading active device list from persistence...");
		try
		{
			List<Guid> list = FilePersistence.ActiveDevices;
			if (list.Any())
			{
				list.ForEach(delegate(Guid id)
				{
					BaseDevice baseDevice = repository.GetBaseDevice(id);
					if (baseDevice != null)
					{
						activeDevices.Add(baseDevice);
					}
				});
			}
			Log.Information(Module.DeviceActivity, $"Active device list loaded. Found {activeDevices.Count} devices.");
			LogActiveDevices();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DeviceActivity, $"An error occurred while loading the active device list: {ex.Message}");
		}
	}

	private void OnDeviceStateChanged(LogicalDeviceStateChangedEventArgs args)
	{
		if (args == null || args.NewLogicalDeviceState == null || args.NewLogicalDeviceState.GetProperties() == null)
		{
			Log.Debug(Module.DeviceActivity, "Invalid logical device state data received. Discarding event. Device might have become unreachable.");
			return;
		}
		LogicalDevice logicalDevice = repository.GetLogicalDevice(args.LogicalDeviceId);
		BaseDevice baseDevice = repository.GetBaseDevices().FirstOrDefault((BaseDevice d) => d.Id == logicalDevice.BaseDevice.Id);
		if (!IgnoreDeviceActivity(baseDevice))
		{
			if (activeDevices.All((BaseDevice d) => d.Id != baseDevice.Id))
			{
				Log.Debug(Module.DeviceActivity, $"Device with ID: [{baseDevice.Id}] marked as active for this month");
				MarkDeviceActive(baseDevice, ActivityReason.StateChanged, PersistenceType.FlushToBackend);
			}
			RaiseProcessingDoneEvent();
		}
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		BaseDevice modifiedBaseDevice;
		foreach (BaseDevice modifiedBaseDevice2 in args.ModifiedBaseDevices)
		{
			modifiedBaseDevice = modifiedBaseDevice2;
			if (args.RepositoryInclusionReport != null && args.RepositoryInclusionReport.Any((EntityMetadata dr) => dr.Id == modifiedBaseDevice.Id && dr.EntityType == EntityType.BaseDevice))
			{
				Log.Debug(Module.DeviceActivity, $"DeviceActivityReporter: Device with ID: [{modifiedBaseDevice.Id}] has been included");
				if (activeDevices.All((BaseDevice bd) => bd.Id != modifiedBaseDevice.Id))
				{
					MarkDeviceActive(modifiedBaseDevice, ActivityReason.Ignore, PersistenceType.LocalCache);
				}
			}
			else if (activeDevices.All((BaseDevice bd) => bd.Id != modifiedBaseDevice.Id))
			{
				MarkDeviceActive(modifiedBaseDevice, ActivityReason.ConfigChanged, PersistenceType.FlushToBackend);
			}
		}
		HandleModifiedCapabilities(args.ModifiedLogicalDevices);
		HandleDeviceExclusion(args.DeletedBaseDevices);
		RaiseProcessingDoneEvent();
	}

	private void HandleModifiedCapabilities(IEnumerable<LogicalDevice> modifiedLogicalDevices)
	{
		LogicalDevice logicalDevice;
		foreach (LogicalDevice modifiedLogicalDevice in modifiedLogicalDevices)
		{
			logicalDevice = modifiedLogicalDevice;
			BaseDevice baseDevice = repository.GetBaseDevices().FirstOrDefault((BaseDevice d) => d.Id == logicalDevice.BaseDeviceId);
			if (baseDevice != null && activeDevices.All((BaseDevice bd) => bd.Id != baseDevice.Id))
			{
				MarkDeviceActive(baseDevice, ActivityReason.ConfigChanged, PersistenceType.FlushToBackend);
			}
		}
	}

	private void HandleDeviceExclusion(IEnumerable<BaseDevice> deletedBaseDevices)
	{
		BaseDevice deletedBaseDevice;
		foreach (BaseDevice deletedBaseDevice2 in deletedBaseDevices)
		{
			deletedBaseDevice = deletedBaseDevice2;
			Log.Debug(Module.DeviceActivity, $"Device with ID: [{deletedBaseDevice.Id}] has been excluded");
			if (activeDevices.Any((BaseDevice bd) => bd.Id == deletedBaseDevice.Id))
			{
				activeDevices.Remove(deletedBaseDevice);
				PersistActiveDevices();
				LogActiveDevices();
			}
		}
	}

	private void ClearActiveDevices()
	{
		if (activeDevices.Any())
		{
			Log.Debug(Module.DeviceActivity, "Device activity list was cleared");
			activeDevices.Clear();
			PersistActiveDevices();
			StartDalFlushRestriction();
			LogActiveDevices();
			RaiseProcessingDoneEvent();
		}
	}

	private void StartDalFlushRestriction()
	{
		if (!flushRestricted)
		{
			flushRestricted = true;
			int num = randomizer.Next(30, 1440);
			Log.Information(Module.DeviceActivity, $"Restricting flush active device list for the next {num} minutes.");
			scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), StopDalFlushRestriction, TimeSpan.FromMinutes(num), runOnce: true));
		}
		else
		{
			Log.Debug(Module.DeviceActivity, "Flushing active device list is already restricted.");
		}
	}

	private void StopDalFlushRestriction()
	{
		if (flushRestricted)
		{
			flushRestricted = false;
			Log.Information(Module.DeviceActivity, "Clear restriction for flushing active device list.");
			FlushPendingData();
		}
	}

	private void AddDeviceActiveEntry(BaseDevice baseDevice, ActivityReason eventReason)
	{
		if (eventReason == ActivityReason.Ignore)
		{
			return;
		}
		lock (syncRoot)
		{
			_ = baseDevice.DeviceType;
			DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
			deviceActivityLogEntry.Timestamp = DateTime.UtcNow;
			deviceActivityLogEntry.DeviceId = baseDevice.Id.ToString();
			deviceActivityLogEntry.ActivityType = ActivityType.DeviceActive;
			deviceActivityLogEntry.NewState = eventReason.ToString();
			deviceActivityLogEntry.EventType = EventType.ShcTrackingEvent;
			DeviceActivityLogEntry newEntry = deviceActivityLogEntry;
			logDataPersistence.AddEntry(newEntry);
		}
	}

	private void MarkDeviceActive(BaseDevice baseDevice, ActivityReason reason, PersistenceType persistenceType)
	{
		if (!IgnoreDeviceActivity(baseDevice))
		{
			Log.Debug(Module.DeviceActivity, $"Device [{baseDevice.DeviceType} with ID: {baseDevice.Id} AppID: {baseDevice.AppId}] marked as active for this month");
			activeDevices.Add(baseDevice);
			PersistActiveDevices();
			LogActiveDevices();
			AddDeviceActiveEntry(baseDevice, reason);
			if (persistenceType == PersistenceType.FlushToBackend)
			{
				FlushPendingData();
			}
		}
	}

	private void PersistActiveDevices()
	{
		FilePersistence.ActiveDevices = activeDevices.Select((BaseDevice bd) => bd.Id).ToList();
	}

	private void FlushPendingData()
	{
		if (!flushRestricted)
		{
			Log.Debug(Module.DeviceActivity, "Flushing device activity data to BE.");
			postClearEvent();
		}
		else
		{
			Log.Warning(Module.DeviceActivity, "Device activity data flushing restricted");
		}
	}

	private void LogActiveDevices()
	{
		if (!activeDevices.Any())
		{
			Log.Debug(Module.DeviceActivity, "There is no device marked as active");
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Devices marked active: ");
		foreach (BaseDevice activeDevice in activeDevices)
		{
			stringBuilder.AppendFormat("[{0}]", new object[1] { activeDevice.Id });
		}
		Log.Debug(Module.DeviceActivity, stringBuilder.ToString());
	}

	private void RaiseProcessingDoneEvent()
	{
		this.ProcessingDone?.Invoke();
	}

	private bool IgnoreDeviceActivity(BaseDevice baseDevice)
	{
		if (baseDevice != null)
		{
			return deviceTypesToIgnore.Contains(baseDevice.DeviceType);
		}
		return false;
	}
}
