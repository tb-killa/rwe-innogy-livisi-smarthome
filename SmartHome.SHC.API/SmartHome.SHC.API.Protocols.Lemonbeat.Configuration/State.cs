using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class State
{
	public uint StateId { get; set; }

	public List<Transaction> Transactions { get; private set; }

	public State()
	{
		Transactions = new List<Transaction>();
	}

	public State(uint stateId, List<Transaction> trnsactions)
	{
		StateId = stateId;
		Transactions = trnsactions;
	}
}
