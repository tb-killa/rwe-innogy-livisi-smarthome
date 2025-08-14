using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.CommonFunctionality;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class DeviceConverters
{
	public static Device ToApiBaseDevice(this BaseDevice coreBaseDevice)
	{
		return coreBaseDevice.ToApiBaseDevice(null);
	}

	public static BaseDevice ToCoreBaseDevice(this Device apiBaseDevice)
	{
		List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> list = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>();
		if (apiBaseDevice.Properties != null)
		{
			foreach (global::SmartHome.SHC.API.PropertyDefinition.Property property in apiBaseDevice.Properties)
			{
				list.Add(property.ToCoreProperty(includeTimestamp: false));
			}
		}
		BaseDevice baseDevice = new BaseDevice();
		baseDevice.Id = apiBaseDevice.Id;
		baseDevice.Properties = list;
		baseDevice.SerialNumber = apiBaseDevice.SerialNumber;
		baseDevice.Manufacturer = apiBaseDevice.Manufacturer;
		baseDevice.DeviceType = apiBaseDevice.DeviceType;
		baseDevice.DeviceVersion = apiBaseDevice.DeviceVersion;
		baseDevice.TimeOfDiscovery = apiBaseDevice.TimeOfDiscovery.ToUniversalTime();
		baseDevice.Name = apiBaseDevice.Name;
		return baseDevice;
	}

	private static List<global::SmartHome.SHC.API.PropertyDefinition.Property> ConvertProperties(List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> coreProperties, List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> propertiesToAdd)
	{
		List<global::SmartHome.SHC.API.PropertyDefinition.Property> properties = new List<global::SmartHome.SHC.API.PropertyDefinition.Property>();
		if (coreProperties != null)
		{
			coreProperties.ForEach(delegate(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property p)
			{
				properties.Add(p.ToApiProperty());
			});
			if (propertiesToAdd != null && propertiesToAdd.Count > 0)
			{
				propertiesToAdd.ForEach(delegate(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property p)
				{
					global::SmartHome.SHC.API.PropertyDefinition.Property property = properties.FirstOrDefault((global::SmartHome.SHC.API.PropertyDefinition.Property pp) => pp.Name == p.Name);
					if (property != null)
					{
						properties.Remove(property);
					}
					properties.Add(p.ToApiProperty());
				});
			}
		}
		return properties;
	}

	public static Device ToApiBaseDevice(this BaseDevice coreBaseDevice, List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> propertiesToAdd)
	{
		List<global::SmartHome.SHC.API.PropertyDefinition.Property> properties = ConvertProperties(coreBaseDevice.Properties, propertiesToAdd);
		List<global::SmartHome.SHC.API.PropertyDefinition.Property> volatileProperties = new List<global::SmartHome.SHC.API.PropertyDefinition.Property>();
		if (coreBaseDevice.VolatileProperties != null && coreBaseDevice.VolatileProperties.Any())
		{
			coreBaseDevice.VolatileProperties.ForEach(delegate(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property vp)
			{
				volatileProperties.Add(vp.ToApiProperty());
			});
		}
		Device device = new Device(coreBaseDevice.Id, properties, coreBaseDevice.SerialNumber, coreBaseDevice.TimeOfDiscovery.GetValueOrDefault(ShcDateTime.UtcNow), coreBaseDevice.TimeOfAcceptance.GetValueOrDefault(ShcDateTime.UtcNow), coreBaseDevice.Manufacturer, coreBaseDevice.DeviceType, coreBaseDevice.DeviceVersion, coreBaseDevice.Name);
		device.VolatileProperties = volatileProperties;
		return device;
	}
}
