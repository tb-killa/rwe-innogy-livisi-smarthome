using System;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;

public class CalculationOperand : IEquatable<CalculationOperand>
{
	public uint? ValueId { get; set; }

	public uint? CalculationId { get; set; }

	public uint? PartnerId { get; set; }

	public uint? StateMachineId { get; set; }

	public uint? TimerId { get; set; }

	public uint? CalendarId { get; set; }

	public double? ConstantNumber { get; set; }

	public string ConstantString { get; set; }

	public byte[] ConstantBinary { get; set; }

	public bool? IsUpdated { get; set; }

	public bool Equals(CalculationOperand otherOperand)
	{
		if (otherOperand != null)
		{
			uint? valueId = ValueId;
			uint? valueId2 = otherOperand.ValueId;
			if (valueId.GetValueOrDefault() == valueId2.GetValueOrDefault() && valueId.HasValue == valueId2.HasValue && CalculationId == otherOperand.CalculationId)
			{
				uint? partnerId = PartnerId;
				uint? partnerId2 = otherOperand.PartnerId;
				if (partnerId.GetValueOrDefault() == partnerId2.GetValueOrDefault() && partnerId.HasValue == partnerId2.HasValue && StateMachineId == otherOperand.StateMachineId)
				{
					uint? timerId = TimerId;
					uint? timerId2 = otherOperand.TimerId;
					if (timerId.GetValueOrDefault() == timerId2.GetValueOrDefault() && timerId.HasValue == timerId2.HasValue && CalendarId == otherOperand.CalendarId)
					{
						double? constantNumber = ConstantNumber;
						double? constantNumber2 = otherOperand.ConstantNumber;
						if (constantNumber.GetValueOrDefault() == constantNumber2.GetValueOrDefault() && constantNumber.HasValue == constantNumber2.HasValue && ConstantString == otherOperand.ConstantString && ((ConstantBinary != null) ? ConstantBinary.Compare(otherOperand.ConstantBinary) : (otherOperand.ConstantBinary == null)))
						{
							return IsUpdated == otherOperand.IsUpdated;
						}
					}
				}
			}
		}
		return false;
	}

	public override bool Equals(object other)
	{
		return Equals(other as CalculationOperand);
	}

	public override int GetHashCode()
	{
		int num = (ValueId.HasValue ? ValueId.Value.GetHashCode() : 0);
		num = (937 * num) ^ (CalculationId.HasValue ? CalculationId.Value.GetHashCode() : 0);
		num = (937 * num) ^ (PartnerId.HasValue ? PartnerId.Value.GetHashCode() : 0);
		num = (937 * num) ^ (StateMachineId.HasValue ? StateMachineId.Value.GetHashCode() : 0);
		num = (937 * num) ^ (TimerId.HasValue ? TimerId.Value.GetHashCode() : 0);
		num = (937 * num) ^ (CalendarId.HasValue ? CalendarId.Value.GetHashCode() : 0);
		num = (937 * num) ^ (ConstantNumber.HasValue ? ConstantNumber.Value.GetHashCode() : 0);
		return (937 * num) ^ ((ConstantString != null) ? ConstantString.GetHashCode() : 0);
	}
}
