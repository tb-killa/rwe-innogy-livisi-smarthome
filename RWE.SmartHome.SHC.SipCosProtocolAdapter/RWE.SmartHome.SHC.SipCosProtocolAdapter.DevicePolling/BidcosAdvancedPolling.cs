using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.DevicePolling.AdvancedPolling;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.DevicePolling;

internal class BidcosAdvancedPolling
{
	private readonly Dispatcher dispatcher = new Dispatcher();

	private readonly IDeviceManager deviceManager;

	private IScheduler taskScheduler;

	private IEventManager eventManager;

	private object syncLock = new object();

	private ILogicalDeviceHandlerCollection ldHandlerCollection;

	private Dictionary<DeviceTypesEq3, IEnumerable<byte>> specialPollingDevices = new Dictionary<DeviceTypesEq3, IEnumerable<byte>>();

	private Dictionary<Guid, PollingInfo> advancedPollingTasks = new Dictionary<Guid, PollingInfo>();

	internal BidcosAdvancedPolling(IEventManager eventManager, IDeviceManager deviceManager, IScheduler taskScheduler, ILogicalDeviceHandlerCollection ldHandlerCollection)
	{
		this.deviceManager = deviceManager;
		this.taskScheduler = taskScheduler;
		this.ldHandlerCollection = ldHandlerCollection;
		this.eventManager = eventManager;
		RegisterDevicesForSpecialPolling();
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartupCompleted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound2, ThreadOption.BackgroundThread, null);
		dispatcher.Start();
	}

	private void RegisterDevicesForSpecialPolling()
	{
		specialPollingDevices.Add(DeviceTypesEq3.Wsd, GetStatusInfoChannels(DeviceTypesEq3.Wsd));
		specialPollingDevices.Add(DeviceTypesEq3.Sir, GetStatusInfoChannels(DeviceTypesEq3.Sir));
	}

	private void OnShcStartupCompleted(ShcStartupCompletedEventArgs args)
	{
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnDevicesDeleted, (ConfigurationProcessedEventArgs a) => a.RepositoryDeletionReport.Any((EntityMetadata em) => em.EntityType == EntityType.BaseDevice), ThreadOption.SubscriberThread, dispatcher);
		eventManager.GetEvent<DeviceUnreachableChangedEvent>().Subscribe(OnDeviceReachableChanged, null, ThreadOption.SubscriberThread, dispatcher);
	}

	private void OnDevicesDeleted(ConfigurationProcessedEventArgs args)
	{
		if (args != null)
		{
			List<Guid> list = (from em in args.RepositoryDeletionReport
				where em.EntityType == EntityType.BaseDevice
				select em.Id).ToList();
			list.ForEach(RemoveSpecialPolling);
		}
	}

	private void OnDeviceReachableChanged(DeviceUnreachableChangedEventArgs args)
	{
		if (args.Unreachable)
		{
			StartAdvancedPolling(args.DeviceId);
		}
		else
		{
			EndAdvancedPolling(args.DeviceId);
		}
	}

	private void StartAdvancedPolling(Guid deviceId)
	{
		lock (syncLock)
		{
			if (!advancedPollingTasks.ContainsKey(deviceId))
			{
				ScheduleNextPollingTrial(deviceId);
			}
		}
	}

	private void EndAdvancedPolling(Guid deviceId)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[deviceId];
		if (deviceInformation != null)
		{
			Log.Debug(Module.SipCosProtocolAdapter, $"Device {deviceInformation} became reachable again. Ending special polling...");
		}
		RemoveSpecialPolling(deviceId);
	}

	private void RemoveSpecialPolling(Guid deviceId)
	{
		lock (syncLock)
		{
			if (advancedPollingTasks.TryGetValue(deviceId, out var value))
			{
				taskScheduler.RemoveSchedulerTask(value.PollingTaskId);
				advancedPollingTasks.Remove(deviceId);
			}
		}
	}

	private void ScheduleNextPollingTrial(Guid deviceId)
	{
		lock (syncLock)
		{
			IDeviceInformation deviceInformation = deviceManager.DeviceList[deviceId];
			if (deviceInformation != null && specialPollingDevices.ContainsKey((DeviceTypesEq3)deviceInformation.ManufacturerDeviceType) && deviceInformation.DeviceInclusionState == DeviceInclusionState.Included)
			{
				if (!advancedPollingTasks.TryGetValue(deviceId, out var value))
				{
					value = new PollingInfo(deviceInformation, specialPollingDevices[(DeviceTypesEq3)deviceInformation.ManufacturerDeviceType]);
					advancedPollingTasks[deviceId] = value;
					Log.Debug(Module.SipCosProtocolAdapter, $"Device {deviceInformation} became unreachable. Starting special polling...");
				}
				AddPollingTask(value);
			}
		}
	}

	private void AddPollingTask(PollingInfo pollingInfo)
	{
		Guid guid = Guid.NewGuid();
		FixedTimeAndDateSchedulerTask schedulerTask = new FixedTimeAndDateSchedulerTask(guid, delegate
		{
			CheckReachability(pollingInfo);
		}, DateTime.Now.AddSeconds((double)pollingInfo.NextPollingInterval));
		taskScheduler.AddSchedulerTask(schedulerTask);
		pollingInfo.PollingScheduled(guid);
	}

	private void CheckReachability(PollingInfo pollInfo)
	{
		TryPollDevice(pollInfo);
	}

	private void TryPollDevice(PollingInfo pollInfo)
	{
		RequestStatusInfo(pollInfo);
		if (pollInfo.NextPollingInterval != PollingIntervals.PollEnd)
		{
			ScheduleNextPollingTrial(pollInfo.DeviceInfo.DeviceId);
			return;
		}
		Log.Debug(Module.SipCosProtocolAdapter, $"Device {pollInfo.DeviceInfo} still unreachable. Ending special polling.");
		RemoveSpecialPolling(pollInfo.DeviceInfo.DeviceId);
	}

	private void RequestStatusInfo(PollingInfo pollInfo)
	{
		IDeviceController dc = deviceManager[pollInfo.DeviceInfo.DeviceId];
		if (dc != null)
		{
			pollInfo.StatusInfoChannels.ToList().ForEach(delegate(byte si)
			{
				Log.Debug(Module.SipCosProtocolAdapter, $"Requesting StatusInfo from device {pollInfo.DeviceInfo.ToString()}...");
				dc.RequestStatusInfo(si);
			});
		}
		else
		{
			Log.Error(Module.SipCosProtocolAdapter, $"There is no device controller registered for {pollInfo.DeviceInfo}");
		}
	}

	private IEnumerable<byte> GetStatusInfoChannels(DeviceTypesEq3 deviceType)
	{
		List<byte> result = new List<byte>();
		IActuatorHandler actuatorHandler = null;
		switch (deviceType)
		{
		case DeviceTypesEq3.Wsd:
			actuatorHandler = ldHandlerCollection.GetAlarActuatorHandler() as IActuatorHandler;
			break;
		case DeviceTypesEq3.Sir:
			actuatorHandler = ldHandlerCollection.GetSirenAlarmActuator() as IActuatorHandler;
			break;
		}
		if (actuatorHandler != null)
		{
			return actuatorHandler.StatusInfoChannels.ToList();
		}
		return result;
	}
}
