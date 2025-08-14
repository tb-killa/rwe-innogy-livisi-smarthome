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

public class HumiditySensorHandler : ISensorHandlerEntityTypes, ISensorHandler, ILogicalDeviceHandler
{
	private byte channel = 201;

	private IDeviceList deviceList;

	public bool IsStatusRequestAllowed => false;

	public IEnumerable<byte> StatusInfoChannels => null;

	public IEnumerable<Type> SupportedSensorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(HumiditySensor));
			return list;
		}
	}

	public HumiditySensorHandler(IDeviceList deviceList)
	{
		this.deviceList = deviceList;
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		if (logicalDevice != null && channelStates.ContainsKey(channel))
		{
			IDeviceInformation deviceInformation = deviceList[logicalDevice.BaseDeviceId];
			if (deviceInformation == null)
			{
				throw new InvalidOperationException("Unable to find device information for device: " + logicalDevice.BaseDeviceId);
			}
			GenericDeviceState genericDeviceState = new GenericDeviceState();
			genericDeviceState.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState.Properties = new List<Property>
			{
				new NumericProperty
				{
					Name = "Humidity",
					Value = (decimal)channelStates[channel].Value / 2.0m,
					UpdateTimestamp = ShcDateTime.UtcNow
				},
				new BooleanProperty
				{
					Name = "MoldWarning",
					Value = ((deviceInformation.StatusInfo != null) ? deviceInformation.StatusInfo.Mold : ((bool?)null)),
					UpdateTimestamp = ShcDateTime.UtcNow
				}
			};
			return genericDeviceState;
		}
		return null;
	}
}
