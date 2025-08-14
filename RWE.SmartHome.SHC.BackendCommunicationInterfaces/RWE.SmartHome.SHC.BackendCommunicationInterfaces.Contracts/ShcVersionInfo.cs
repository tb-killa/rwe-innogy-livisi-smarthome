using System.Globalization;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public class ShcVersionInfo
{
	public string HardwareVersion { get; set; }

	public string FirmwareVersion { get; set; }

	public string ApplicationVersion { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "[shc versions hw: {0}, fw: {1}, app: {2}]", new object[3] { HardwareVersion, FirmwareVersion, ApplicationVersion });
	}
}
