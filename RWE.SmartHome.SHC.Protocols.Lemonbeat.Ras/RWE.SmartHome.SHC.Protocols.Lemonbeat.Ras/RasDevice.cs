namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class RasDevice
{
	private const int MaxDeviceName = 128;

	private const int MaxDeviceType = 16;

	private const int DeviceTypeOffset = 4;

	private const int DeviceNameOffset = 38;

	public string DeviceType;

	public string DeviceName;

	public RasDevice(byte[] buffer, int offset)
	{
		DeviceType = buffer.GetString(offset + 4, 16);
		DeviceName = buffer.GetString(offset + 38, 128);
	}
}
