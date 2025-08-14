using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DomainModel.Constants;

public static class ShcBaseDeviceConstants
{
	public static class Capabilities
	{
		public const string VirtualResidentCapabilityName = "VirtualResident";

		public const string CalendarCapabilityName = "Calendar";
	}

	public static class StateProperties
	{
		public static readonly string UpdateAvailable = "UpdateAvailable";

		public static readonly string LastReboot = "LastReboot";

		public static readonly string MemoryLoad = "MemoryLoad";

		public static readonly string CPULoad = "CPULoad";

		public static readonly string LBDongleAttached = "LBDongleAttached";

		public static readonly string MBusDongleAttached = "MBusDongleAttached";

		public static readonly string ConfigurationVersion = "ConfigVersion";

		public static readonly string OSState = "OSState";

		public static readonly string DiscoveryActive = "DiscoveryActive";

		public static readonly string CurrentUTCOffset = "CurrentUTCOffset";

		public static readonly string IPAddress = "IPAddress";

		public static readonly string ProductsHash = "ProductsHash";

		public static readonly string DiskUsage = "DiskUsage";

		public static readonly IEnumerable<string> PropertiesList = new string[9] { UpdateAvailable, LastReboot, MemoryLoad, CPULoad, LBDongleAttached, MBusDongleAttached, OSState, DiscoveryActive, DiskUsage };
	}

	public static class ConfigurationProperties
	{
		public static readonly string AttrHardwareVersion = "HardwareVersion";

		public static readonly string HostName = "HostName";

		public static readonly string TimeZone = "TimeZone";

		public static readonly string MACAddress = "MACAddress";

		public static readonly string ShcType = "ShcType";

		public static readonly string RegistrationTime = "RegistrationTime";

		public static readonly string UIConfigState = "ConfigurationState";

		public static readonly string BackendConnectionMonitored = "BackendConnectionMonitored";

		public static readonly string RFCommFailureNotification = "RFCommFailureNotification";

		public static readonly string GeoLocation = "GeoLocation";

		public static readonly string SoftwareVersion = "SoftwareVersion";
	}
}
