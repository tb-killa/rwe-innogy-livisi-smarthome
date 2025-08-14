using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class NumberValue
{
	public uint ValueId { get; private set; }

	public DateTime? TimeStamp { get; private set; }

	public decimal Value { get; private set; }

	public NumberValue(uint valueId, decimal value, DateTime? timeStamp)
	{
		ValueId = valueId;
		Value = value;
		TimeStamp = timeStamp;
	}

	public NumberValue(uint valueId, decimal value)
	{
		ValueId = valueId;
		Value = value;
	}
}
