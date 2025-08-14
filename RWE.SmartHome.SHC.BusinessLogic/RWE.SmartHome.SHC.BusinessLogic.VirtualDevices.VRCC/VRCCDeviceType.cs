using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

public class VRCCDeviceType
{
	private readonly Dictionary<string, string> capabilityProperties = new Dictionary<string, string>
	{
		{ "RoomSetpoint", "PointTemperature" },
		{ "RoomTemperature", "Temperature" },
		{ "RoomHumidity", "Humidity" }
	};

	public bool PropertiesAreValidForDeviceType(List<Property> properties, string deviceType)
	{
		if (DeviceTypeIsNotDefined(deviceType) || properties == null || properties.Count == 0)
		{
			return true;
		}
		string capabilityProperty = capabilityProperties[deviceType];
		return properties.Any((Property x) => x.Name == capabilityProperty);
	}

	private bool DeviceTypeIsNotDefined(string deviceType)
	{
		if (!string.IsNullOrEmpty(deviceType))
		{
			return !capabilityProperties.ContainsKey(deviceType);
		}
		return true;
	}
}
