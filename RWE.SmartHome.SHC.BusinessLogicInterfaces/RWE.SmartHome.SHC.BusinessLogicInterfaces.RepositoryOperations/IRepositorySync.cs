namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

public interface IRepositorySync
{
	event ConfigurationProcessingStatusUpdateEvent OnConfigurationStatusUpdate;

	RepositoryLockContext GetLock(string reason, RepositoryUpdateContextData updateContextData);

	RepositoryLockContext GetLockAsyncRelease(string reason, RepositoryUpdateContextData updateContextData);

	RepositoryLockContext WaitForLock(string reason, RepositoryUpdateContextData updateContextData);

	RepositoryLockContext WaitForLock(string reason, RepositoryUpdateContextData updateContextData, int waitMili);

	void Release(RepositoryLockContext lockContext);
}
