using System.Collections.Generic;
using SmartHome.SHC.API.Configuration.Services;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Configuration.Validation;

public static class PropertyExtensions
{
	public static bool PropertyIsNotChanged(this IConfigurationProvider configurationProvider, Device deviceUpdate, string propertyName)
	{
		Device existingDevice = configurationProvider.GetExistingDevice(deviceUpdate);
		return existingDevice != null && PropertyIsNotChanged(existingDevice.Properties, deviceUpdate.Properties, propertyName);
	}

	public static bool PropertyIsNotChanged(this IConfigurationProvider configurationProvider, Capability capabilityUpdate, string propertyName)
	{
		Capability existingCapability = configurationProvider.GetExistingCapability(capabilityUpdate);
		return existingCapability != null && PropertyIsNotChanged(existingCapability.Properties, capabilityUpdate.Properties, propertyName);
	}

	private static bool PropertyIsNotChanged(IEnumerable<Property> existingProperties, IEnumerable<Property> newProperties, string propertyName)
	{
		Property property = existingProperties.Get(propertyName);
		Property property2 = newProperties.Get(propertyName);
		return (property == null && property2 == null) || (property != null && property2 != null && property.Equals(property2));
	}

	private static Device GetExistingDevice(this IConfigurationProvider configurationProvider, Device deviceUpdate)
	{
		if (deviceUpdate == null)
		{
			return null;
		}
		return configurationProvider.GetDevice(deviceUpdate.Id);
	}

	private static Capability GetExistingCapability(this IConfigurationProvider configurationProvider, Capability capabilityUpdate)
	{
		if (capabilityUpdate == null)
		{
			return null;
		}
		return configurationProvider.GetCapability(capabilityUpdate.Id);
	}
}
