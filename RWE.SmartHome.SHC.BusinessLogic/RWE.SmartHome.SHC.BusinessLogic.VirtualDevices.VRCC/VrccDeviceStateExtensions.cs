using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

public static class VrccDeviceStateExtensions
{
	public static decimal? GetStateValue(this LogicalDeviceState state)
	{
		if (state == null)
		{
			return null;
		}
		string primaryPropertyName = state.LogicalDevice.PrimaryPropertyName;
		IEnumerable<NumericProperty> source = state.GetProperties().OfType<NumericProperty>();
		return source.FirstOrDefault((NumericProperty p) => p.Name == primaryPropertyName)?.Value;
	}
}
