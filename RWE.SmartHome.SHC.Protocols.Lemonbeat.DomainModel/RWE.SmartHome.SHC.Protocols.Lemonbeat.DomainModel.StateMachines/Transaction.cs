using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

public class Transaction : IEquatable<Transaction>
{
	public uint? CalculationId { get; set; }

	public uint? ActionId { get; set; }

	public uint? NextStateId { get; set; }

	public bool Equals(Transaction otherTransaction)
	{
		if (otherTransaction != null)
		{
			uint? calculationId = CalculationId;
			uint? calculationId2 = otherTransaction.CalculationId;
			if (calculationId.GetValueOrDefault() == calculationId2.GetValueOrDefault() && calculationId.HasValue == calculationId2.HasValue && ActionId == otherTransaction.ActionId)
			{
				return NextStateId == otherTransaction.NextStateId;
			}
		}
		return false;
	}

	public override bool Equals(object other)
	{
		return Equals(other as Transaction);
	}

	public override int GetHashCode()
	{
		int num = (CalculationId.HasValue ? CalculationId.Value.GetHashCode() : 0);
		num = (num * 937) ^ (ActionId.HasValue ? ActionId.Value.GetHashCode() : 0);
		return (num * 937) ^ (NextStateId.HasValue ? NextStateId.Value.GetHashCode() : 0);
	}
}
