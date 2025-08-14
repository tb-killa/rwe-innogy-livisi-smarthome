using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class DeviceDescription
{
	private const uint DONGLE_MANUFACTURER = 2u;

	private const uint DONGLE_PRODUCT_ID = 1u;

	public bool Included { get; set; }

	public byte[] MacAddress { get; set; }

	public uint ManufacturerId { get; set; }

	public string Name { get; set; }

	public uint ManufacturerProductId { get; set; }

	public RadioMode RadioMode { get; set; }

	public SGTIN96 SGTIN { get; set; }

	public uint? DeviceType { get; set; }

	public string HardwareVersion { get; set; }

	public uint? WakeupChannel { get; set; }

	public uint? WakeupInterval { get; set; }

	public uint? WakeupOffset { get; set; }

	public string BootloaderVersion { get; set; }

	public string StackVersion { get; set; }

	public string ApplicationVersion { get; set; }

	public uint? ChannelMap { get; set; }

	public uint? ChannelScanTime { get; set; }

	public bool IsDongle
	{
		get
		{
			if (ManufacturerId == 2)
			{
				return ManufacturerProductId == 1;
			}
			return false;
		}
	}
}
