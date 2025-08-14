namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public enum BackendPersistenceResult
{
	Success,
	ServiceAccessError,
	ServiceFailure,
	OperationCancelled,
	OperationPostponed
}
