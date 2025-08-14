using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class SabotageSensorHandler : ISensorHandlerStringTypes, ISensorHandler, ILogicalDeviceHandler
{
	private const byte sabotageChannel = 4;

	private readonly IDeviceManager deviceManager;

	public bool IsStatusRequestAllowed => true;

	public IEnumerable<byte> StatusInfoChannels
	{
		get
		{
			List<byte> list = new List<byte>();
			list.Add(4);
			return list;
		}
	}

	public IEnumerable<string> SupportedSensorTypes
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("SabotageSensor");
			return list;
		}
	}

	public SabotageSensorHandler(IDeviceManager deviceManager)
	{
		this.deviceManager = deviceManager;
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		GenericDeviceState genericDeviceState = null;
		if (channelStates.ContainsKey(4))
		{
			GenericDeviceState genericDeviceState2 = new GenericDeviceState();
			genericDeviceState2.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState = genericDeviceState2;
			bool value = channelStates[4].ChannelError == 2;
			genericDeviceState.Properties = new List<Property>
			{
				new BooleanProperty
				{
					Name = "IsOn",
					Value = value,
					UpdateTimestamp = ShcDateTime.UtcNow
				}
			};
			int value2 = channelStates[4].Value;
			if (value2 != 200)
			{
				EnableAllChannels(logicalDevice.BaseDeviceId);
				DisableConfirmationSound(logicalDevice.BaseDeviceId);
			}
		}
		return genericDeviceState;
	}

	private void EnableAllChannels(Guid deviceId)
	{
		IDeviceController deviceController = deviceManager[deviceId];
		deviceController.ChangeDeviceState(RampMode.RampStart, 0, 200, 4, 0);
	}

	private void DisableConfirmationSound(Guid deviceId)
	{
		IDeviceController deviceController = deviceManager[deviceId];
		deviceController.SendVirtualConfigCommand(deviceManager.DefaultShcAddress, 4, new byte[2] { 169, 0 });
	}
}
