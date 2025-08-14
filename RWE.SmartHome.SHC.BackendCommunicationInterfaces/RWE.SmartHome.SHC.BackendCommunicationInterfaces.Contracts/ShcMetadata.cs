using System.Globalization;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public class ShcMetadata
{
	public string Name { get; set; }

	public string Ip { get; set; }

	public string HardwareVersion { get; set; }

	public string FirmwareVersion { get; set; }

	public string ApplicationVersion { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Name: '{0}', IP: [{1}], HardwareVersion: '{2}', FirmwareVersion: '{3}', ApplicationVersion: '{4}'.", Name, Ip, HardwareVersion, FirmwareVersion, ApplicationVersion);
	}
}
