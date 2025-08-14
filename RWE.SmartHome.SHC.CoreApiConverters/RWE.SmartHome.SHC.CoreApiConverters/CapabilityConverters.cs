using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class CapabilityConverters
{
	private const string ActivityLogActivePropName = "ActivityLogActive";

	public static Capability ToApiCapability(this LogicalDevice coreLogicalDevice)
	{
		List<global::SmartHome.SHC.API.PropertyDefinition.Property> properties = new List<global::SmartHome.SHC.API.PropertyDefinition.Property>();
		if (coreLogicalDevice.Properties != null)
		{
			coreLogicalDevice.Properties.ForEach(delegate(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property p)
			{
				properties.Add(p.ToApiProperty());
			});
		}
		properties.RemoveAll((global::SmartHome.SHC.API.PropertyDefinition.Property m) => m.Name.Equals("ActivityLogActive"));
		properties.Add(new global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty("ActivityLogActive", coreLogicalDevice.ActivityLogActive));
		Guid physicalDeviceId = Guid.Empty;
		BaseDevice baseDevice = coreLogicalDevice.BaseDevice;
		if (baseDevice != null)
		{
			physicalDeviceId = baseDevice.Id;
		}
		return new Capability(coreLogicalDevice.Name, coreLogicalDevice.Id, properties, physicalDeviceId, coreLogicalDevice.DeviceType, coreLogicalDevice.PrimaryPropertyName);
	}

	public static LogicalDevice ToCoreLogicalDevice(this Capability apiLogicalDevice)
	{
		LogicalDevice logicalDevice = new LogicalDevice();
		logicalDevice.Id = apiLogicalDevice.Id;
		logicalDevice.DeviceType = apiLogicalDevice.DeviceType;
		logicalDevice.Name = apiLogicalDevice.Name;
		logicalDevice.BaseDeviceId = apiLogicalDevice.DeviceId;
		logicalDevice.Properties = (from p in apiLogicalDevice.Properties
			where !p.Name.Equals("ActivityLogActive")
			select p.ToCoreProperty(includeTimestamp: false)).ToList();
		logicalDevice.PrimaryPropertyName = apiLogicalDevice.PrimaryPropertyName;
		logicalDevice.ActivityLogActive = (from m in apiLogicalDevice.Properties.OfType<global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty>()
			where m.Name == "ActivityLogActive"
			select m.Value).FirstOrDefault() ?? false;
		return logicalDevice;
	}
}
