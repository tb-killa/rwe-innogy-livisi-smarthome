using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class StateMachine
{
	public uint StateMachineId { get; set; }

	public List<State> States { get; private set; }

	public StateMachine()
	{
		States = new List<State>();
	}

	public StateMachine(uint id, List<State> states)
	{
		StateMachineId = id;
		States = states;
	}
}
