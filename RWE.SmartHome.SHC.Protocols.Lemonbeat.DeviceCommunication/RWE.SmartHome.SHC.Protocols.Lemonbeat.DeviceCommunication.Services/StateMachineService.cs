using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class StateMachineService : SenderService<network>, IStateMachineService
{
	private const string DefaultNamespace = "urn:statemachinexsd";

	public StateMachineService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Statemachine, "urn:statemachinexsd")
	{
	}

	public IEnumerable<StateMachine> GetAllStateMachines(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier, isSet: false);
		network.device[0].Items = new object[1]
		{
			new statemachineGetType
			{
				state_idSpecified = false,
				statemachine_idSpecified = false
			}
		};
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.statemachine_get };
		network network2 = SendRequest(identifier, network);
		if (network2 != null && network2.device != null)
		{
			networkDevice networkDevice = network2.device.Where((networkDevice d) => d.device_id == identifier.SubDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Items != null)
			{
				statemachineReportType statemachineReportType = networkDevice.Items.OfType<statemachineReportType>().FirstOrDefault();
				if (statemachineReportType != null && statemachineReportType.statemachine != null)
				{
					return statemachineReportType.statemachine.Select((statemachineType sm) => ToDomainModelStateMachine(sm));
				}
			}
		}
		return new List<StateMachine>();
	}

	public StateMachine GetStateMachine(DeviceIdentifier identifier, uint stateMachineId)
	{
		network network = CreateNetworkMessage(identifier, isSet: false);
		network.device[0].Items = new object[1]
		{
			new statemachineGetType
			{
				statemachine_idSpecified = true,
				statemachine_id = stateMachineId,
				state_idSpecified = false
			}
		};
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.statemachine_get };
		network network2 = SendRequest(identifier, network);
		if (network2 == null || network2.device == null || network2.device.Length == 0)
		{
			return null;
		}
		IEnumerable<statemachineReportType> enumerable = network2.device.SelectMany((networkDevice device) => device.Items).OfType<statemachineReportType>();
		if (enumerable == null)
		{
			return null;
		}
		return ToDomainModelStateMachine(enumerable.First().statemachine.First());
	}

	public State GetStateDefinition(DeviceIdentifier identifier, uint stateMachineId, uint stateId)
	{
		network network = CreateNetworkMessage(identifier, isSet: false);
		network.device[0].Items = new object[1]
		{
			new statemachineGetType
			{
				statemachine_idSpecified = true,
				statemachine_id = stateMachineId,
				state_idSpecified = true,
				state_id = stateId
			}
		};
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.statemachine_get };
		network network2 = SendRequest(identifier, network);
		if (network2 == null || network2.device == null || network2.device.Length == 0)
		{
			return null;
		}
		IEnumerable<statemachineReportType> enumerable = network2.device.SelectMany((networkDevice device) => device.Items).OfType<statemachineReportType>();
		if (enumerable == null)
		{
			return null;
		}
		return ToDomainModelState(enumerable.First().statemachine.First().Items.First());
	}

	public uint GetCurrentState(DeviceIdentifier identifier, uint stateMachineId)
	{
		network network = CreateNetworkMessage(identifier, isSet: false);
		network.device[0].Items = new object[1]
		{
			new statemachineGetStateType
			{
				statemachine_idSpecified = true,
				statemachine_id = stateMachineId
			}
		};
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.statemachine_get };
		network network2 = SendRequest(identifier, network);
		if (network2 == null || network2.device == null || network2.device.Length == 0)
		{
			return 0u;
		}
		return network2.device.SelectMany((networkDevice device) => device.Items).OfType<statemachineReportStateType>()?.First().statemachine_state.First().Value ?? 0;
	}

	public Dictionary<uint, uint> GetAllCurrentStates(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier, isSet: false);
		network.device[0].Items = new object[1]
		{
			new statemachineGetStateType
			{
				statemachine_idSpecified = false
			}
		};
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.statemachine_get };
		network network2 = SendRequest(identifier, network);
		if (network2 == null || network2.device == null || network2.device.Length == 0)
		{
			return null;
		}
		IEnumerable<statemachineReportStateType> enumerable = network2.device.SelectMany((networkDevice device) => device.Items).OfType<statemachineReportStateType>();
		if (enumerable == null)
		{
			return null;
		}
		Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
		statemachineStateType[] statemachine_state = enumerable.First().statemachine_state;
		foreach (statemachineStateType statemachineStateType in statemachine_state)
		{
			dictionary[statemachineStateType.statemachine_id] = statemachineStateType.Value;
		}
		return dictionary;
	}

	public void SetAndDeleteStateMachines(DeviceIdentifier identifier, IEnumerable<StateMachine> toSetStateMachines, IEnumerable<uint> toDeleteStateMachines)
	{
		if (toDeleteStateMachines.Count() == 0 && toSetStateMachines.Count() == 0)
		{
			return;
		}
		List<ItemsChoiceType> list = new List<ItemsChoiceType>();
		network network = CreateNetworkMessage(identifier, isSet: true);
		List<object> list2 = new List<object>();
		if (toDeleteStateMachines.Count() > 0)
		{
			list2 = toDeleteStateMachines.Select((Func<uint, object>)((uint state) => new statemachineDeleteType
			{
				statemachine_id = state,
				statemachine_idSpecified = true,
				state_idSpecified = false
			})).ToList();
			for (int num = 0; num < toDeleteStateMachines.Count(); num++)
			{
				list.Add(ItemsChoiceType.statemachine_delete);
			}
		}
		if (toSetStateMachines.Count() > 0)
		{
			statemachineSetType statemachineSetType = new statemachineSetType();
			statemachineSetType.Items = toSetStateMachines.Select((StateMachine sm) => ToWireStateMachine(sm)).ToArray();
			statemachineSetType item = statemachineSetType;
			list2.Add(item);
			list.Add(ItemsChoiceType.statemachine_set);
		}
		network.device[0].ItemsElementName = list.ToArray();
		network.device[0].Items = list2.ToArray();
		SendMessage(identifier, network, TransportType.Connection);
	}

	private statemachineType ToWireStateMachine(StateMachine toSet)
	{
		statemachineType statemachineType = new statemachineType();
		statemachineType.statemachine_id = toSet.Id;
		statemachineType.Items = toSet.States.Select((State state) => ToWireState(state)).ToArray();
		return statemachineType;
	}

	private stateType ToWireState(State state)
	{
		stateType stateType = new stateType();
		stateType.state_id = state.StateId;
		stateType.transaction = state.Transactions.Select((Transaction transaction) => ToWireTransaction(transaction)).ToArray();
		return stateType;
	}

	private transactionType ToWireTransaction(Transaction transaction)
	{
		transactionType transactionType = new transactionType();
		transactionType.action_id = transaction.ActionId ?? 0;
		transactionType.action_idSpecified = transaction.ActionId.HasValue;
		transactionType.calculation_id = transaction.CalculationId ?? 0;
		transactionType.calculation_idSpecified = transaction.CalculationId.HasValue;
		transactionType.goto_state_id = transaction.NextStateId ?? 0;
		transactionType.goto_state_idSpecified = transaction.NextStateId.HasValue;
		return transactionType;
	}

	private StateMachine ToDomainModelStateMachine(statemachineType stateMachine)
	{
		StateMachine stateMachine2 = new StateMachine();
		stateMachine2.Id = stateMachine.statemachine_id;
		stateMachine2.States = (from item in stateMachine.Items.ToList()
			select ToDomainModelState(item)).ToList();
		return stateMachine2;
	}

	private State ToDomainModelState(stateType state)
	{
		State state2 = new State();
		state2.StateId = state.state_id;
		state2.Transactions = (from item in state.transaction.ToList()
			select ToDomainModelTransaction(item)).ToList();
		return state2;
	}

	private Transaction ToDomainModelTransaction(transactionType transaction)
	{
		Transaction transaction2 = new Transaction();
		transaction2.ActionId = (transaction.action_idSpecified ? new uint?(transaction.action_id) : ((uint?)null));
		transaction2.CalculationId = (transaction.calculation_idSpecified ? new uint?(transaction.calculation_id) : ((uint?)null));
		transaction2.NextStateId = (transaction.goto_state_idSpecified ? new uint?(transaction.goto_state_id) : ((uint?)null));
		return transaction2;
	}

	private network CreateNetworkMessage(DeviceIdentifier target, bool isSet)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				device_idSpecified = target.SubDeviceId.HasValue,
				device_id = (target.SubDeviceId ?? 0),
				Items = new statemachineType[1]
				{
					new statemachineType()
				}
			}
		};
		return network;
	}
}
