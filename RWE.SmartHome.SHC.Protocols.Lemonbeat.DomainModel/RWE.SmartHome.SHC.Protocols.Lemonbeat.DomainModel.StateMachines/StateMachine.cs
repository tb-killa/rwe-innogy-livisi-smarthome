using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

public class StateMachine : ConfigurationItem
{
	public List<State> States { get; set; }

	public override bool Equals(ConfigurationItem other)
	{
		if (other is StateMachine stateMachine && base.Id == stateMachine.Id && !States.Except(stateMachine.States).Any())
		{
			return !stateMachine.States.Except(States).Any();
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (base.Id.GetHashCode() * 937) ^ States.Count.GetHashCode();
	}
}
