using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IStateMachineService
{
	IEnumerable<StateMachine> GetAllStateMachines(DeviceIdentifier identifier);

	StateMachine GetStateMachine(DeviceIdentifier identifier, uint stateMachineId);

	State GetStateDefinition(DeviceIdentifier identifier, uint stateMachineId, uint stateId);

	uint GetCurrentState(DeviceIdentifier identifier, uint stateMachineId);

	Dictionary<uint, uint> GetAllCurrentStates(DeviceIdentifier identifier);

	void SetAndDeleteStateMachines(DeviceIdentifier identifier, IEnumerable<StateMachine> toSetStateMachines, IEnumerable<uint> toDeleteStateMachines);
}
