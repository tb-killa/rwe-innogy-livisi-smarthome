using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public static class VrccExtensions
{
	public static bool IsVrccCompatibleDevice(this LogicalDevice device)
	{
		if (device == null || device.Properties == null)
		{
			return false;
		}
		return device.Properties.Any((Property prop) => prop.Name == "VRCCTemperature" || prop.Name == "VRCCHumidity" || prop.Name == "VRCCSetPoint");
	}

	public static decimal? FormatPropertyValue(this Property vrccProperty)
	{
		string name = vrccProperty.Name;
		decimal? result = ((vrccProperty is NumericProperty numericProperty) ? numericProperty.Value : ((decimal?)null));
		if (!result.HasValue)
		{
			return null;
		}
		switch (name)
		{
		case "Temperature":
		case "PointTemperature":
			return Convert.ToDecimal(Math.Floor(10.0 * Convert.ToDouble(result.Value)) / 10.0);
		case "Humidity":
			return decimal.Truncate(result.Value);
		default:
			return result;
		}
	}
}
