namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public enum ModifyEntitiesResult
{
	Unknown = 0,
	NotInTransactionScope = 1,
	TransactionCommitBusy = 2,
	TransactionScopeTimedOut = 4
}
