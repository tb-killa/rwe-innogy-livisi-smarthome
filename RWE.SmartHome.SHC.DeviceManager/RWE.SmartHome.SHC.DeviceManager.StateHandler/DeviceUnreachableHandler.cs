using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DeviceManager.Events;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.StateHandler;

internal class DeviceUnreachableHandler
{
	private const string LoggingSource = "DeviceUnreachableHandler";

	private const int DeviceReachabilityCheckInterval = 30;

	private readonly IEventManager eventManager;

	private readonly IDeviceList deviceList;

	private readonly byte[] shcAddress;

	private readonly ISipCosPersistence persistence;

	private readonly ICommunicationWrapper communicationWrapper;

	private readonly Dictionary<Guid, DateTime> deviceHeartbeats;

	private static readonly List<uint> devicesExpectedToPublishState = new List<uint>(4) { 5u, 6u, 1u, 21u, 15u };

	public DeviceUnreachableHandler(IDeviceList deviceList, ISipCosPersistence persistence, ICommunicationWrapper communicationWrapper, IEventManager eventManager, byte[] shcAddress, IScheduler taskScheduler)
	{
		this.communicationWrapper = communicationWrapper;
		this.persistence = persistence;
		this.shcAddress = shcAddress;
		this.deviceList = deviceList;
		this.eventManager = eventManager;
		deviceHeartbeats = new Dictionary<Guid, DateTime>();
		communicationWrapper.SendScheduler.ReachableChanged += SendSchedulerReachableChanged;
		eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(OnDeviceInclusionStateChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartupCompletedRound2, (ShcStartupCompletedEventArgs m) => m.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, null);
		if (deviceList.Any(UnreachableDevice))
		{
			AtLeastOneUnreachableDevice(devicesUnreachable: true);
		}
		foreach (IDeviceInformation device in deviceList)
		{
			if (NeedsIcmpRequest(device) && (device.DeviceInclusionState == DeviceInclusionState.InclusionPending || (device.DeviceInclusionState == DeviceInclusionState.Included && device.DeviceUnreachable)))
			{
				EnqueueIcmpPackage(device);
			}
		}
		taskScheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), CheckDevicesHeartbeat, TimeSpan.FromMinutes(5.0)));
	}

	private void OnShcStartupCompletedRound2(ShcStartupCompletedEventArgs args)
	{
		foreach (IDeviceInformation item in deviceList.Where(UnreachableDevice))
		{
			eventManager.GetEvent<DeviceUnreachableChangedEvent>().Publish(new DeviceUnreachableChangedEventArgs(item.DeviceId, item.DeviceUnreachable));
		}
	}

	private void OnDeviceInclusionStateChanged(DeviceInclusionStateChangedEventArgs args)
	{
		if (args.DeviceInclusionState == DeviceInclusionState.ExclusionPending && !deviceList.Any(UnreachableDevice))
		{
			AtLeastOneUnreachableDevice(devicesUnreachable: false);
		}
	}

	private static bool UnreachableDevice(IDeviceInformation deviceInformation)
	{
		if (deviceInformation.DeviceUnreachable && deviceInformation.DeviceInclusionState != DeviceInclusionState.ExclusionPending && deviceInformation.DeviceInclusionState != DeviceInclusionState.Excluded && deviceInformation.DeviceInclusionState != DeviceInclusionState.Found)
		{
			return deviceInformation.DeviceInclusionState != DeviceInclusionState.FoundWithAddressCollision;
		}
		return false;
	}

	private static bool NeedsIcmpRequest(IDeviceInformation device)
	{
		if (device.ProtocolType == ProtocolType.SipCos && device.BestOperationMode != DeviceInfoOperationModes.EventListener)
		{
			return !ShouldReceiveState(device);
		}
		return false;
	}

	private void SendSchedulerReachableChanged(object sender, DeviceReachableChangedEventArgs e)
	{
		if (e.DeviceInformation is DeviceInformation deviceInformation)
		{
			if (e.Reachable)
			{
				DeviceReachable(deviceInformation);
			}
			else
			{
				DeviceUnreachable(deviceInformation);
			}
		}
	}

	private void DeviceUnreachable(IDeviceInformation device)
	{
		if (!device.DeviceUnreachable)
		{
			Log.Warning(Module.DeviceManager, "DeviceUnreachableHandler", $"Device with address {deviceList.LogInfoByAddress(device.Address)} marked as unreachable.");
			device.DeviceUnreachable = true;
			persistence.SaveInTransaction(device, suppressEvent: true);
			if (NeedsIcmpRequest(device) && device.ManufacturerDeviceType != 10)
			{
				EnqueueIcmpPackage(device);
			}
			if (device.ManufacturerDeviceType == 10)
			{
				OnRouterUnreachable();
			}
			TryPingRouters();
			if (device.DeviceInclusionState == DeviceInclusionState.Included)
			{
				eventManager.GetEvent<DeviceUnreachableChangedEvent>().Publish(new DeviceUnreachableChangedEventArgs(device.DeviceId, device.DeviceUnreachable));
				AtLeastOneUnreachableDevice(devicesUnreachable: true);
			}
		}
	}

	private void EnqueueIcmpPackage(IBasicDeviceInformation deviceInformation)
	{
		Predicate<PacketSequence> predicate = (PacketSequence sequence) => sequence.Parent != null && sequence.Parent.QueueType == QueueType.Icmp && sequence.SequenceType == SequenceType.Icmp && sequence.Current != null && sequence.Current.Message != null && sequence.Current.Message.Length >= 3 && sequence.Current.Message[2] == 129;
		communicationWrapper.SendScheduler.RemoveSequencesConditionally(deviceInformation.DeviceId, predicate, SequenceState.Aborted);
		CORESTACKHeader cORESTACKHeader = new CORESTACKHeader();
		cORESTACKHeader.MacSource = shcAddress;
		cORESTACKHeader.MacDestination = deviceInformation.Address;
		CORESTACKHeader header = cORESTACKHeader;
		CORESTACKMessage packet = communicationWrapper.IcmpHandler.GenerateICMPMessage(header, (byte)deviceInformation.BestOperationMode, deviceInformation.Rssi, ICMP_type.ECHO_REQUEST);
		PacketSequence packetSequence = new PacketSequence(SequenceType.Icmp);
		packetSequence.Add(packet);
		communicationWrapper.SendScheduler.Enqueue(packetSequence);
	}

	private void DeviceReachable(IDeviceInformation deviceInformation)
	{
		deviceHeartbeats[deviceInformation.DeviceId] = DateTime.UtcNow;
		if (deviceInformation.DeviceUnreachable)
		{
			Log.Information(Module.DeviceManager, "DeviceUnreachableHandler", $"Device with address {deviceList.LogInfoByAddress(deviceInformation.Address)} marked as reachable.");
			deviceInformation.DeviceUnreachable = false;
			persistence.SaveInTransaction(deviceInformation, suppressEvent: true);
			eventManager.GetEvent<DeviceUnreachableChangedEvent>().Publish(new DeviceUnreachableChangedEventArgs(deviceInformation.DeviceId, deviceInformation.DeviceUnreachable));
			if (!deviceList.Any(UnreachableDevice))
			{
				AtLeastOneUnreachableDevice(devicesUnreachable: false);
			}
			if (deviceInformation.ManufacturerDeviceType == 10)
			{
				OnRouterReachable();
			}
		}
	}

	private void AtLeastOneUnreachableDevice(bool devicesUnreachable)
	{
		AtLeastOneDeviceUnreachableChangedEvent atLeastOneDeviceUnreachableChangedEvent = eventManager.GetEvent<AtLeastOneDeviceUnreachableChangedEvent>();
		atLeastOneDeviceUnreachableChangedEvent.Publish(new AtLeastOneDeviceUnreachableChangedEventArgs
		{
			DevicesUnreachable = devicesUnreachable
		});
	}

	private void OnRouterReachable()
	{
		deviceList.ContainsRouter = true;
		communicationWrapper.SendScheduler.ForceEchoRequestForUnreachableDevices();
	}

	private void OnRouterUnreachable()
	{
		deviceList.ForceDetectionOfRouters = true;
	}

	private void TryPingRouters()
	{
		if (!deviceList.ContainsRouter)
		{
			return;
		}
		foreach (IDeviceInformation device in deviceList)
		{
			if (device.ManufacturerDeviceType == 10 && device.DeviceInclusionState == DeviceInclusionState.Included && !device.DeviceUnreachable)
			{
				EnqueueIcmpPackage(device);
			}
		}
	}

	private void CheckDevicesHeartbeat()
	{
		DateTime utcNow = DateTime.UtcNow;
		TryPingRouters();
		foreach (IDeviceInformation item in deviceList.Where((IDeviceInformation di) => di.DeviceInclusionState == DeviceInclusionState.Included))
		{
			if (!ShouldReceiveState(item))
			{
				continue;
			}
			if (deviceHeartbeats.ContainsKey(item.DeviceId))
			{
				if (deviceHeartbeats[item.DeviceId].AddMinutes(30.0) < utcNow)
				{
					Log.Warning(Module.DeviceManager, $"Not heard from device {deviceList.LogInfoByDeviceInfo(item)} since {deviceHeartbeats[item.DeviceId]}.");
					DeviceUnreachable(item);
				}
			}
			else
			{
				deviceHeartbeats[item.DeviceId] = utcNow;
				Log.Debug(Module.DeviceManager, $"Device {deviceList.LogInfoByDeviceInfo(item)} added to heartbeat check list.");
			}
		}
	}

	private static bool ShouldReceiveState(IDeviceInformation deviceInfo)
	{
		if (deviceInfo.ManufacturerCode == 1)
		{
			return devicesExpectedToPublishState.Contains(deviceInfo.ManufacturerDeviceType);
		}
		return false;
	}
}
