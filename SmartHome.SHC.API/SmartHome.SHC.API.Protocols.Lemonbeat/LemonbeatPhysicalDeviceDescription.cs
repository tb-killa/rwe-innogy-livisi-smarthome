using System.Collections.Generic;
using System.Linq;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class LemonbeatPhysicalDeviceDescription
{
	public Property[] DeviceProperties { get; private set; }

	public PhysicalDescriptionStatus Status { get; private set; }

	public string Manufacturer { get; private set; }

	public string PhysicalDeviceType { get; private set; }

	public string Version { get; private set; }

	public string DeviceName { get; private set; }

	public LemonbeatPhysicalDeviceDescription(IEnumerable<Property> properties, PhysicalDescriptionStatus status, string manufacturer, string physicalDeviceType, string version, string deviceName)
	{
		if (properties == null)
		{
			DeviceProperties = new Property[0];
		}
		else
		{
			DeviceProperties = properties.ToArray();
		}
		Status = status;
		Manufacturer = manufacturer;
		PhysicalDeviceType = physicalDeviceType;
		Version = version;
		DeviceName = deviceName;
	}
}
