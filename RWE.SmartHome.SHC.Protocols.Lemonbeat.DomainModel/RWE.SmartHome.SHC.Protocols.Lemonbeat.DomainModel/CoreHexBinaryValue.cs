using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class CoreHexBinaryValue
{
	public uint Id { get; set; }

	public DateTime? TimeStamp { get; set; }

	public byte[] Value { get; set; }

	public CoreHexBinaryValue(uint valueId, byte[] value, DateTime? timeStamp)
	{
		Id = valueId;
		Value = value;
		TimeStamp = timeStamp;
	}

	public CoreHexBinaryValue(uint valueId, byte[] value)
	{
		Id = valueId;
		Value = value;
	}

	public CoreHexBinaryValue()
	{
	}

	public bool Equals(CoreHexBinaryValue otherValue)
	{
		if (otherValue != null && otherValue.Id == Id && otherValue.TimeStamp == TimeStamp)
		{
			return otherValue.Value == Value;
		}
		return false;
	}
}
