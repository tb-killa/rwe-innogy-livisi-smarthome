using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces.Events;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.EventForwarding;

internal sealed class EventForwarder
{
	private readonly IEventManager eventManager;

	private readonly ILogicalDeviceHandlerCollection logicalDeviceHandlerCollection;

	private readonly IRepository configurationRepository;

	private readonly IDevicePolling devicePolling;

	private readonly IDeviceManager deviceManager;

	private readonly List<Guid> smokeActuatorIds;

	public EventForwarder(IEventManager eventManager, IRepository configurationRepository, IDevicePolling devicePolling, ILogicalDeviceHandlerCollection logicalDeviceHandlerCollection, IDeviceManager deviceManager)
	{
		this.deviceManager = deviceManager;
		this.devicePolling = devicePolling;
		this.configurationRepository = configurationRepository;
		this.logicalDeviceHandlerCollection = logicalDeviceHandlerCollection;
		this.eventManager = eventManager;
		smokeActuatorIds = new List<Guid>();
		eventManager.GetEvent<SipCosValueChangedEvent>().Subscribe(SipCosValueChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SipCosSwitchCommandEvent>().Subscribe(SipCosSwitchCommand, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SmokeDetectorStateChangeTriggeredEvent>().Subscribe(SipCosActuatorStateChanged, null, ThreadOption.PublisherThread, null);
	}

	private void SipCosSwitchCommand(SipCosSwitchCommandEventArgs args)
	{
		BaseDevice device = GetDevice(args.DeviceAddress);
		if (device != null)
		{
			SortedList<byte, ChannelState> sortedList = new SortedList<byte, ChannelState>();
			sortedList.Add(args.KeyChannelNumber, new ChannelState
			{
				KeyChannelNumber = args.KeyChannelNumber,
				KeystrokeCounter = args.KeyStrokeCounter,
				Value = args.DecisionValue.GetValueOrDefault(),
				IsLongPress = args.IsLongPress
			});
			PublishLogicalDeviceStateChanged(device.Id, sortedList);
		}
	}

	private void SipCosValueChanged(SipCosValueChangedEventArgs eventArgs)
	{
		if (HasDeviceStateOnStatusInfo(eventArgs.DeviceId))
		{
			PublishLogicalDeviceStateChanged(eventArgs.DeviceId, eventArgs.ChannelStates);
		}
	}

	private bool HasDeviceStateOnStatusInfo(Guid baseDeviceId)
	{
		BaseDevice baseDevice = configurationRepository.GetBaseDevice(baseDeviceId);
		bool result = false;
		if (baseDevice != null)
		{
			result = baseDevice.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.WDS;
		}
		return result;
	}

	private void PublishLogicalDeviceStateChanged(Guid deviceId, SortedList<byte, ChannelState> channelStates)
	{
		IEnumerable<LogicalDeviceState> valueChangedLogicalDeviceStates = CreateValueChangedLogicalDeviceState(deviceId, channelStates);
		PublishLogicalDeviceStateChanged(valueChangedLogicalDeviceStates);
	}

	private void PublishLogicalDeviceStateChanged(IEnumerable<LogicalDeviceState> valueChangedLogicalDeviceStates)
	{
		RawLogicalDeviceStateChangedEvent rawLogicalDeviceStateChangedEvent = eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>();
		foreach (LogicalDeviceState valueChangedLogicalDeviceState in valueChangedLogicalDeviceStates)
		{
			RawLogicalDeviceStateChangedEventArgs payload = new RawLogicalDeviceStateChangedEventArgs(valueChangedLogicalDeviceState.LogicalDeviceId, valueChangedLogicalDeviceState);
			rawLogicalDeviceStateChangedEvent.Publish(payload);
		}
	}

	private void SipCosActuatorStateChanged(SmokeDetectorStateChangeTriggeredEventArgs eventArgs)
	{
		smokeActuatorIds.Add(eventArgs.ActuatorId);
	}

	private bool IsSmokeDetector(BaseDevice physicalDevice)
	{
		BuiltinPhysicalDeviceType builtinDeviceDeviceType = physicalDevice.GetBuiltinDeviceDeviceType();
		if (builtinDeviceDeviceType != BuiltinPhysicalDeviceType.WSD)
		{
			return builtinDeviceDeviceType == BuiltinPhysicalDeviceType.WSD2;
		}
		return true;
	}

	public IEnumerable<LogicalDeviceState> CreateValueChangedLogicalDeviceState(Guid deviceId, SortedList<byte, ChannelState> channelStates)
	{
		List<LogicalDeviceState> list = new List<LogicalDeviceState>();
		bool flag = false;
		BaseDevice originalBaseDevice = configurationRepository.GetOriginalBaseDevice(deviceId);
		IEnumerable<LogicalDevice> enumerable = (IsSmokeDetector(originalBaseDevice) ? GetAllSmokeDetectionDevices(deviceId) : GetAssociatedLogicalDevices(deviceId));
		foreach (LogicalDevice item in enumerable)
		{
			ILogicalDeviceHandler logicalDeviceHandler = logicalDeviceHandlerCollection.GetLogicalDeviceHandler(item);
			if (logicalDeviceHandler != null)
			{
				LogicalDeviceState logicalDeviceState = logicalDeviceHandler.CreateLogicalDeviceState(item, channelStates);
				if (logicalDeviceState != null)
				{
					list.Add(logicalDeviceState);
				}
				flag = logicalDeviceHandler is IActuatorHandlerEntityTypes actuatorHandlerEntityTypes && actuatorHandlerEntityTypes.GetIsPeriodicStatusPollingActive(item);
			}
		}
		if (flag && devicePolling != null)
		{
			devicePolling.HandleDeviceChanges();
		}
		return list;
	}

	private BaseDevice GetDevice(byte[] deviceAddress)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[deviceAddress];
		if (deviceInformation != null)
		{
			return configurationRepository.GetBaseDevice(deviceInformation.DeviceId);
		}
		return null;
	}

	private IEnumerable<LogicalDevice> GetAssociatedLogicalDevices(Guid physicalDeviceId)
	{
		return configurationRepository.GetOriginalLogicalDevices().FindAll(delegate(LogicalDevice d)
		{
			_ = d.BaseDeviceId;
			return d.BaseDeviceId == physicalDeviceId;
		});
	}

	private IEnumerable<LogicalDevice> GetAllSmokeDetectionDevices(Guid physicalDeviceId)
	{
		IDeviceList deviceList = deviceManager.DeviceList;
		IEnumerable<LogicalDevice> first = configurationRepository.GetOriginalLogicalDevices().OfType<AlarmActuator>().Cast<LogicalDevice>();
		List<LogicalDevice> second = new List<LogicalDevice>();
		if (!smokeActuatorIds.Contains(physicalDeviceId))
		{
			second = (from d in configurationRepository.GetOriginalLogicalDevices().OfType<SmokeDetectorSensor>()
				where d.BaseDeviceId == physicalDeviceId
				select d).Cast<LogicalDevice>().ToList();
		}
		else
		{
			smokeActuatorIds.Remove(physicalDeviceId);
		}
		return from d in first.Union(second)
			where !deviceList[d.BaseDeviceId].DeviceUnreachable
			select d;
	}
}
