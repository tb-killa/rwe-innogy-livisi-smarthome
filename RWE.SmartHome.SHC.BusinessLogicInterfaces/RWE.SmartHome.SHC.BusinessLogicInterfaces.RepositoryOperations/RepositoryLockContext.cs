using System;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

public sealed class RepositoryLockContext : IDisposable
{
	private readonly IRepositorySync repositorySync;

	public string Reason { get; private set; }

	public RepositoryUpdateContextData UpdateContextData { get; private set; }

	public bool Commit { get; set; }

	public bool AsyncCommit { get; set; }

	public RepositoryLockContext(IRepositorySync repositorySync, string reason, RepositoryUpdateContextData updateContextData)
	{
		if (repositorySync == null)
		{
			throw new ArgumentNullException("repositorySync");
		}
		if (updateContextData == null)
		{
			throw new ArgumentNullException("updateContextData");
		}
		this.repositorySync = repositorySync;
		UpdateContextData = updateContextData;
		Reason = reason;
	}

	public void Dispose()
	{
		Log.InformationFormat(Module.BusinessLogic, "RepositoryLockContext", true, "Releasing lock with {0}.", Commit ? "commit" : "no commit");
		repositorySync.Release(this);
		GC.SuppressFinalize(this);
	}

	~RepositoryLockContext()
	{
		Log.InformationFormat(Module.BusinessLogic, "RepositoryLockContext", true, "Releasing lock with {0}.", Commit ? "commit" : "no commit");
		repositorySync.Release(this);
	}
}
