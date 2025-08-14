using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class SmokeDetectorSensorHandler : ISensorHandlerEntityTypes, ISensorHandler, ILogicalDeviceHandler
{
	private const int smokeChannel = 1;

	private const int alarmValue = 200;

	public IEnumerable<Type> SupportedSensorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(SmokeDetectorSensor));
			return list;
		}
	}

	public bool IsStatusRequestAllowed => false;

	public IEnumerable<byte> StatusInfoChannels => null;

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		GenericDeviceState result = null;
		if (channelStates.ContainsKey(1))
		{
			GenericDeviceState genericDeviceState = new GenericDeviceState();
			genericDeviceState.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState.Properties = new List<Property>
			{
				new BooleanProperty
				{
					Name = "IsSmokeAlarm",
					Value = (channelStates[1].Value == 200),
					UpdateTimestamp = ShcDateTime.UtcNow
				}
			};
			result = genericDeviceState;
		}
		return result;
	}
}
