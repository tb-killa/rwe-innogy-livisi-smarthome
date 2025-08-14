namespace SmartHome.SHC.API.Protocols.wMBus;

public class BinaryValue : VariableDataEntry
{
	public byte[] Value { get; set; }

	public BinaryValue()
		: base(ValueType.Binary)
	{
	}
}
