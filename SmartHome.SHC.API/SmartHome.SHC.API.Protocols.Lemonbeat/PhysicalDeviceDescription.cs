namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class PhysicalDeviceDescription
{
	public string Name { get; set; }

	public uint ManufacturerId { get; set; }

	public uint ManufacturerProductId { get; set; }

	public byte[] SGTIN { get; set; }

	public uint? DeviceType { get; set; }

	public string HardwareVersion { get; set; }

	public byte[] MACAddress { get; set; }

	public string BootLoaderVersion { get; set; }

	public string StackVersion { get; set; }

	public string ApplicationVersion { get; set; }

	public RadioMode RadioMode { get; set; }

	public uint? WakeupInterval { get; set; }

	public uint? WakeupOffset { get; set; }

	public uint? WakeupChannel { get; set; }

	public uint? ChannelMap { get; set; }

	public uint? ChannelScanTime { get; set; }
}
