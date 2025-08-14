using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Control;

public class DeviceState
{
	public Reachability DeviceReachability { get; set; }

	public Property[] DeviceProperties { get; set; }

	public string FirmwareVersion { get; set; }
}
