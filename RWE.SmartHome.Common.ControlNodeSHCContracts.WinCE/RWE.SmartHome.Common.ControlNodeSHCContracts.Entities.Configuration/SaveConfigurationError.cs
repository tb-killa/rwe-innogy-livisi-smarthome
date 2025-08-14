namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public enum SaveConfigurationError : byte
{
	Unknown,
	ValidationError,
	BackendPersistenceError,
	LocalPersistenceError,
	BackendRestorePointRetrievalError,
	AddInError
}
