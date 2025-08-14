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

internal class MotionDetectionSensorHandler : ISensorHandlerEntityTypes, ISensorHandler, ILogicalDeviceHandler
{
	private byte channel = 1;

	private int keystrokeCounterCache;

	public bool IsStatusRequestAllowed => false;

	public IEnumerable<byte> StatusInfoChannels => null;

	public IEnumerable<Type> SupportedSensorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(MotionDetectionSensor));
			return list;
		}
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		if (channelStates.ContainsKey(channel))
		{
			int keystrokeCounter = channelStates[channel].KeystrokeCounter;
			if (IsValidKeystrokeCounter(keystrokeCounter))
			{
				keystrokeCounterCache = keystrokeCounter;
				GenericDeviceState genericDeviceState = new GenericDeviceState();
				genericDeviceState.LogicalDevice = logicalDevice;
				genericDeviceState.LogicalDeviceId = logicalDevice.Id;
				genericDeviceState.Properties = new List<Property>
				{
					new NumericProperty
					{
						Name = "MotionDetectedCount",
						Value = keystrokeCounter,
						UpdateTimestamp = ShcDateTime.UtcNow
					}
				};
				return genericDeviceState;
			}
		}
		return null;
	}

	private bool IsValidKeystrokeCounter(int keystrokeCounter)
	{
		if (keystrokeCounter == 0)
		{
			if (keystrokeCounter == 0)
			{
				return keystrokeCounterCache == 255;
			}
			return false;
		}
		return true;
	}
}
