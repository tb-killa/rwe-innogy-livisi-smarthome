namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports.Enums;

public enum StatusStateMachine
{
	StatemachineGetWrongId = 1,
	StatemachineSetWrongId = 2,
	StatemachineDeleteWrongId = 3,
	StatemachineGetWrongStateId = 4,
	StatemachineSetWrongStateId = 5,
	StatemachineDeleteWrongStateId = 6,
	StatemachineCheckWrongId = 11,
	StatemachineRunningTooLong = 12
}
