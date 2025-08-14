namespace RWE.SmartHome.SHC.wMBusProtocol;

public class ValueInformationFieldBase
{
	public bool ExtensionBit { get; set; }

	protected byte Value { get; set; }

	public ValueInformationFieldBase()
	{
	}

	public ValueInformationFieldBase(bool extensionBit, byte valueInformation)
	{
		ExtensionBit = extensionBit;
		Value = valueInformation;
	}

	public ValueInformationFieldBase(byte field)
	{
		ExtensionBit = (field & 0x80) == 128;
		Value = (byte)(field & 0x7F);
	}

	public byte GetValue()
	{
		byte b = Value;
		if (ExtensionBit)
		{
			b |= 0x80;
		}
		return b;
	}
}
