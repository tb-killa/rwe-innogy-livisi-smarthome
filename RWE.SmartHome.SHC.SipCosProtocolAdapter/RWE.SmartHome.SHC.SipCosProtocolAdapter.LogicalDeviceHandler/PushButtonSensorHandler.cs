using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class PushButtonSensorHandler : ISensorHandlerEntityTypes, ISensorHandler, ILogicalDeviceHandler
{
	public bool IsStatusRequestAllowed => false;

	public IEnumerable<byte> StatusInfoChannels => null;

	public IEnumerable<Type> SupportedSensorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(PushButtonSensor));
			return list;
		}
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		BuiltinPhysicalDeviceType builtinDeviceDeviceType = logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType();
		IEnumerable<byte> allowedChannels = GetChannels(builtinDeviceDeviceType);
		IEnumerable<KeyValuePair<byte, ChannelState>> source = channelStates.Where((KeyValuePair<byte, ChannelState> x) => allowedChannels.Contains(x.Key));
		if (source.Any())
		{
			if (source.Count() == 1)
			{
				KeyValuePair<byte, ChannelState> keyValuePair = source.First();
				GenericDeviceState genericDeviceState = new GenericDeviceState();
				genericDeviceState.LogicalDevice = logicalDevice;
				genericDeviceState.LogicalDeviceId = logicalDevice.Id;
				genericDeviceState.Properties = new List<Property>
				{
					new NumericProperty
					{
						Name = "LastKeyPressCounter",
						Value = keyValuePair.Value.KeystrokeCounter,
						UpdateTimestamp = ShcDateTime.UtcNow
					},
					new NumericProperty
					{
						Name = "LastPressedButtonIndex",
						Value = PushButtonIndexProvider.GetPushButtonIndex(builtinDeviceDeviceType, keyValuePair.Value.KeyChannelNumber),
						UpdateTimestamp = ShcDateTime.UtcNow
					}
				};
				return genericDeviceState;
			}
			Log.Warning(Module.SipCosProtocolAdapter, "Received more than one switch event on the deviceType: " + logicalDevice.DeviceType);
			return null;
		}
		return null;
	}

	private IEnumerable<byte> GetChannels(BuiltinPhysicalDeviceType builtinPhysicalDeviceType)
	{
		int num = 0;
		int start = 1;
		switch (builtinPhysicalDeviceType)
		{
		case BuiltinPhysicalDeviceType.BRC8:
			num = 8;
			break;
		case BuiltinPhysicalDeviceType.WSC2:
		case BuiltinPhysicalDeviceType.ISC2:
			num = 2;
			break;
		case BuiltinPhysicalDeviceType.ISS2:
		case BuiltinPhysicalDeviceType.ISD2:
		case BuiltinPhysicalDeviceType.ISR2:
			num = 2;
			start = 2;
			break;
		default:
			num = 0;
			break;
		}
		return from x in Enumerable.Range(start, num)
			select (byte)x;
	}
}
