using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class StringValue
{
	public uint ValueId { get; private set; }

	public DateTime? TimeStamp { get; private set; }

	public string Value { get; private set; }

	public StringValue(uint valueId, string value, DateTime? timeStamp)
	{
		ValueId = valueId;
		Value = value;
		TimeStamp = timeStamp;
	}

	public StringValue(uint valueId, string value)
	{
		ValueId = valueId;
		Value = value;
	}
}
