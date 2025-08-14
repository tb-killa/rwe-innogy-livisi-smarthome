namespace RWE.SmartHome.SHC.wMBusProtocol;

public class ValueInformationField : ValueInformationFieldBase
{
	public ValueInformationFieldCode ValueInformation
	{
		get
		{
			return (ValueInformationFieldCode)base.Value;
		}
		set
		{
			base.Value = (byte)value;
		}
	}

	public ValueInformationField()
	{
	}

	public ValueInformationField(bool extensionBit, ValueInformationFieldCode valueInformation)
		: base(extensionBit, (byte)valueInformation)
	{
	}

	public ValueInformationField(byte field)
		: base(field)
	{
	}

	public override string ToString()
	{
		return $"Extension: {(base.ExtensionBit ? 1 : 0)}, Value type: {ValueInformation}";
	}
}
