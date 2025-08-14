using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class CoreStringValue
{
	public uint Id { get; set; }

	public DateTime? TimeStamp { get; set; }

	public string Value { get; set; }

	public CoreStringValue(uint valueId, string value, DateTime? timeStamp)
	{
		Id = valueId;
		Value = value;
		TimeStamp = timeStamp;
	}

	public CoreStringValue(uint valueId, string value)
	{
		Id = valueId;
		Value = value;
	}

	public CoreStringValue()
	{
	}

	public bool Equals(CoreStringValue otherString)
	{
		if (otherString != null && otherString.Id == Id && otherString.TimeStamp == TimeStamp)
		{
			return otherString.Value == Value;
		}
		return false;
	}
}
