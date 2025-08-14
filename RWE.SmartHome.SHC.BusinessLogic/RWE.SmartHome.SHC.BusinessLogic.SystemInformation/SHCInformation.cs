namespace RWE.SmartHome.SHC.BusinessLogic.SystemInformation;

public struct SHCInformation
{
	public string SerialNumber { get; set; }

	public string SoftwareVersion { get; set; }

	public string HardwareVersion { get; set; }

	public string OSVersion { get; set; }

	public string IPAddress { get; set; }

	public string Hostname { get; set; }
}
