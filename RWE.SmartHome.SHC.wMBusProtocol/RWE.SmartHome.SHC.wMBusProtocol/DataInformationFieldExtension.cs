namespace RWE.SmartHome.SHC.wMBusProtocol;

public class DataInformationFieldExtension
{
	public bool ExtensionBit { get; set; }

	public bool DeviceUnit { get; set; }

	public byte Tariff { get; set; }

	public byte StorageNumber { get; set; }

	public DataInformationFieldExtension()
	{
	}

	public DataInformationFieldExtension(bool extensionBit, bool deviceUnit, byte tariff, byte storageNumber)
	{
		ExtensionBit = extensionBit;
		DeviceUnit = deviceUnit;
		Tariff = tariff;
		StorageNumber = storageNumber;
	}

	public DataInformationFieldExtension(byte field)
	{
		ExtensionBit = (field & 0x80) == 128;
		DeviceUnit = (field & 0x40) == 64;
		Tariff = (byte)((byte)(field << 2) >> 6);
		StorageNumber = (byte)((byte)(field << 4) >> 4);
	}

	public byte GetValue()
	{
		byte b = (byte)(Tariff << 4);
		if (ExtensionBit)
		{
			b |= 0x80;
		}
		if (DeviceUnit)
		{
			b |= 0x40;
		}
		return (byte)(b | StorageNumber);
	}

	public override string ToString()
	{
		return $"Extension: {(ExtensionBit ? 1 : 0)}, Unit: {DeviceUnit}, Tariff: {Tariff}, Storage: {StorageNumber}";
	}
}
