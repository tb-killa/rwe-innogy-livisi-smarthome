namespace SmartHome.SHC.API.Protocols.wMBus;

public class StringValue : VariableDataEntry
{
	public string Value { get; set; }

	public StringValue()
		: base(ValueType.String)
	{
	}

	public StringValue(string stringValue)
		: base(ValueType.String)
	{
		Value = stringValue;
	}
}
