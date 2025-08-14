using System.Collections.Generic;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class LemonbeatPhysicalDevice
{
	public PhysicalDeviceDescription DeviceDescription { get; set; }

	public List<MemoryInformation> MemoryInformation { get; set; }

	public List<ValueDescription> ValueDescription { get; set; }
}
