using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class CoreNumberValue
{
	public uint Id { get; set; }

	public DateTime? TimeStamp { get; set; }

	public decimal Value { get; set; }

	public CoreNumberValue(uint valueId, decimal value, DateTime? timeStamp)
	{
		Id = valueId;
		Value = value;
		TimeStamp = timeStamp;
	}

	public CoreNumberValue(uint valueId, decimal value)
	{
		Id = valueId;
		Value = value;
	}

	public CoreNumberValue()
	{
	}

	public bool Equals(CoreNumberValue otherNumber)
	{
		if (otherNumber != null && otherNumber.Id == Id && otherNumber.TimeStamp == TimeStamp)
		{
			return otherNumber.Value == Value;
		}
		return false;
	}
}
