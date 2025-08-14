using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;

public static class PhysicalDeviceBasicProperties
{
	public static readonly string IsReachable = "IsReachable";

	public static readonly string UpdateState = "UpdateState";

	public static readonly string DeviceInclusionState = "DeviceInclusionState";

	public static readonly string DeviceConfigurationState = "DeviceConfigurationState";

	public static readonly string FirmwareVersion = "FirmwareVersion";

	public static readonly string ProductsHash = "ProductsHash";

	public static readonly IEnumerable<string> PropertiesList = new string[5] { IsReachable, UpdateState, DeviceInclusionState, DeviceConfigurationState, FirmwareVersion };
}
