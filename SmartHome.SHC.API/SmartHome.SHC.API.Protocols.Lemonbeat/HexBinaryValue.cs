using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class HexBinaryValue
{
	public uint ValueId { get; private set; }

	public DateTime? TimeStamp { get; private set; }

	public byte[] Value { get; private set; }

	public HexBinaryValue(uint valueId, byte[] value, DateTime? timeStamp)
	{
		ValueId = valueId;
		Value = value;
		TimeStamp = timeStamp;
	}

	public HexBinaryValue(uint valueId, byte[] value)
	{
		ValueId = valueId;
		Value = value;
	}
}
