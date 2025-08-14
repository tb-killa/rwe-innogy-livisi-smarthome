using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using SmartHome.SHC.API.Control;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class DeviceStateConverters
{
	public static CapabilityState ToApiDeviceState(this GenericDeviceState coreState)
	{
		List<global::SmartHome.SHC.API.PropertyDefinition.Property> properties = new List<global::SmartHome.SHC.API.PropertyDefinition.Property>();
		if (coreState.Properties != null)
		{
			coreState.Properties.ForEach(delegate(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property p)
			{
				properties.Add(p.ToApiProperty());
			});
		}
		return new CapabilityState(properties);
	}

	public static GenericDeviceState ToCoreDeviceState(this CapabilityState apiState, Guid logicalDeviceId)
	{
		GenericDeviceState genericDeviceState = new GenericDeviceState();
		genericDeviceState.LogicalDeviceId = logicalDeviceId;
		genericDeviceState.Properties = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>();
		if (apiState.Properties != null)
		{
			List<global::SmartHome.SHC.API.PropertyDefinition.Property> list = new List<global::SmartHome.SHC.API.PropertyDefinition.Property>(apiState.Properties);
			for (int i = 0; i < list.Count; i++)
			{
				genericDeviceState.Properties.Add(list[i].ToCoreProperty(includeTimestamp: true));
			}
		}
		return genericDeviceState;
	}

	public static PhysicalDeviceState ToCorePhysicalDeviceState(this DeviceState apiState, Guid physicalDeviceId)
	{
		if (apiState == null)
		{
			return null;
		}
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.DeviceProperties = apiState.DeviceProperties.ToCorePropertyBag(includeTimestamp: true);
		PhysicalDeviceState physicalDeviceState2 = physicalDeviceState;
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.IsReachable, ConvertAsBoolReachabilityState(apiState.DeviceReachability));
		if (!string.IsNullOrEmpty(apiState.FirmwareVersion))
		{
			physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.FirmwareVersion, apiState.FirmwareVersion);
		}
		physicalDeviceState2.PhysicalDeviceId = physicalDeviceId;
		return physicalDeviceState2;
	}

	private static bool ConvertAsBoolReachabilityState(Reachability reachabilityStatus)
	{
		return reachabilityStatus == Reachability.Reachable;
	}
}
