namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class RasConnectionStatus
{
	public ConnectionState State { get; set; }

	public RasError Error { get; set; }

	public string DeviceType { get; set; }

	public string DeviceName { get; set; }
}
