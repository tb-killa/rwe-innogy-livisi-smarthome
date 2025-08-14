namespace RWE.SmartHome.SHC.wMBusProtocol;

public class ValueInformationFieldFD : ValueInformationFieldBase
{
	public ValueInformationFieldCodeExtensionFD ValueInformation
	{
		get
		{
			return (ValueInformationFieldCodeExtensionFD)base.Value;
		}
		set
		{
			base.Value = (byte)value;
		}
	}

	public ValueInformationFieldFD()
	{
	}

	public ValueInformationFieldFD(bool extensionBit, ValueInformationFieldCodeExtensionFD valueInformation)
		: base(extensionBit, (byte)valueInformation)
	{
	}

	public ValueInformationFieldFD(byte field)
		: base(field)
	{
	}

	public override string ToString()
	{
		return $"Extension: {(base.ExtensionBit ? 1 : 0)}, Value type: {ValueInformation}";
	}
}
