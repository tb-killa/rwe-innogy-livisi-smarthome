using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

public class State : IEquatable<State>
{
	public uint StateId { get; set; }

	public List<Transaction> Transactions { get; set; }

	public bool Equals(State otherState)
	{
		if (otherState != null && StateId == otherState.StateId && !Transactions.Except(otherState.Transactions).Any())
		{
			return !otherState.Transactions.Except(Transactions).Any();
		}
		return false;
	}

	public override bool Equals(object other)
	{
		return Equals(other as State);
	}

	public override int GetHashCode()
	{
		return (StateId.GetHashCode() * 937) ^ Transactions.Count.GetHashCode();
	}
}
