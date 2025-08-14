using System;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.RepositoryOperations;

public class RepositorySync : IRepositorySync
{
	private const string LogSource = "RepositoryCommitWrapper";

	private readonly IRepository repository;

	private readonly IConfigurationProcessor configurationProcessor;

	private readonly IEventManager eventManager;

	private readonly object syncRoot = new object();

	private bool repositoryLocked;

	private readonly ManualResetEvent resetEvent = new ManualResetEvent(initialState: false);

	private readonly SubscriptionToken configurationProcessingFailedSubscriptionToken;

	private readonly SubscriptionToken configurationProcessedSubscriptionToken;

	private RepositoryLockContext currentLock;

	public event ConfigurationProcessingStatusUpdateEvent OnConfigurationStatusUpdate;

	public RepositorySync(IRepository repository, IConfigurationProcessor configurationProcessor, IEventManager eventManager)
	{
		this.repository = repository;
		this.configurationProcessor = configurationProcessor;
		this.eventManager = eventManager;
		configurationProcessingFailedSubscriptionToken = eventManager.GetEvent<ConfigurationProcessingFailedEvent>().Subscribe(ConfigurationProcessingFailed, null, ThreadOption.PublisherThread, null);
		configurationProcessedSubscriptionToken = eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(ConfigurationProcessed, null, ThreadOption.PublisherThread, null);
	}

	public RepositoryLockContext GetLock(string reason, RepositoryUpdateContextData updateContextData)
	{
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Getting lock context for {0}.", reason);
		RepositoryLockContext repositoryLockContext = TryGetLockContextInternal(reason, async: false, updateContextData);
		if (repositoryLockContext == null)
		{
			Log.Error(Module.BusinessLogic, "RepositoryCommitWrapper", "Getting lock failed.");
			throw new RepositoryLockedException();
		}
		return repositoryLockContext;
	}

	public RepositoryLockContext GetLockAsyncRelease(string reason, RepositoryUpdateContextData updateContextData)
	{
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Getting lock context for {0}.", reason);
		RepositoryLockContext repositoryLockContext = TryGetLockContextInternal(reason, async: true, updateContextData);
		if (repositoryLockContext == null)
		{
			Log.Error(Module.BusinessLogic, "RepositoryCommitWrapper", "Getting lock failed.");
			throw new RepositoryLockedException();
		}
		return repositoryLockContext;
	}

	public RepositoryLockContext WaitForLock(string reason, RepositoryUpdateContextData updateContextData)
	{
		return WaitForLock(reason, updateContextData, -1);
	}

	public RepositoryLockContext WaitForLock(string reason, RepositoryUpdateContextData updateContextData, int waitMili)
	{
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "WaitForLockContext {0}ms for {1}.", waitMili, reason);
		RepositoryLockContext repositoryLockContext = TryGetLockContextInternal(reason, async: false, updateContextData);
		if (repositoryLockContext != null)
		{
			return repositoryLockContext;
		}
		if (resetEvent.WaitOne(waitMili, exitContext: false))
		{
			Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Waiting for lock context {0}ms {1}.", waitMili, reason);
			return TryGetLockContextInternal(reason, async: false, updateContextData);
		}
		Log.ErrorFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Waiting for {0} lock failed.", reason);
		throw new RepositoryLockedException();
	}

	public void Release(RepositoryLockContext lockContext)
	{
		if (lockContext == null)
		{
			Log.Error(Module.BusinessLogic, "RepositoryCommitWrapper", "Null lockContext");
			return;
		}
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Lock release requested. Reason: {0}, Commit: {1}", lockContext.Reason, lockContext.Commit);
		if (lockContext != currentLock)
		{
			Log.ErrorFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Not releasing current lock.");
			Log.ErrorFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Current lock is {0}.", currentLock.Reason);
			throw new RepositoryLockInvalidException();
		}
		if (lockContext.Commit)
		{
			if (lockContext.AsyncCommit)
			{
				ThreadPool.QueueUserWorkItem(Commit, lockContext.UpdateContextData);
			}
			else
			{
				Commit(lockContext.UpdateContextData);
			}
			return;
		}
		lock (syncRoot)
		{
			repository.Rollback();
			currentLock = null;
			repositoryLocked = false;
		}
		resetEvent.Set();
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Update rolled back, lock released.");
	}

	protected virtual void FireOnConfigurationStatusUpdate(ConfigurationProcessingStatusEventArgs args)
	{
		this.OnConfigurationStatusUpdate?.Invoke(args);
	}

	private RepositoryLockContext TryGetLockContextInternal(string reason, bool async, RepositoryUpdateContextData updateContextData)
	{
		RepositoryLockContext repositoryLockContext = null;
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", false, "TryGetLockContextInternal lock");
		lock (syncRoot)
		{
			if (!repositoryLocked)
			{
				repositoryLockContext = new RepositoryLockContext(this, reason, updateContextData);
				repositoryLockContext.AsyncCommit = async;
				repositoryLocked = true;
				resetEvent.Reset();
				currentLock = repositoryLockContext;
			}
		}
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", false, "TryGetLockContextInternal lock exit");
		return repositoryLockContext;
	}

	private void Commit(object state)
	{
		RepositoryUpdateContextData updateContextData = state as RepositoryUpdateContextData;
		try
		{
			Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "FireConfigurationProcessEvent.");
		}
		catch (Exception)
		{
		}
		try
		{
			configurationProcessor.ProcessNewConfiguration(updateContextData);
		}
		finally
		{
			Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", false, "CommitLockRelease");
			lock (syncRoot)
			{
				currentLock = null;
				repositoryLocked = false;
			}
			Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", false, "CommitLockRelease done.");
			resetEvent.Set();
		}
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Configuration processed, lock released.");
	}

	private void ConfigurationProcessingFailed(ConfigurationProcessingFailedEventArgs args)
	{
		Log.InformationFormat(Module.BusinessLogic, "RepositoryCommitWrapper", true, "Rolling back repository due to configuration processing failure.");
		repository.Rollback();
		FireOnConfigurationStatusUpdate(new ConfigurationProcessingStatusEventArgs(ConfigurationProcessingStatus.Failed, args));
	}

	private void ConfigurationProcessed(ConfigurationProcessedEventArgs args)
	{
		if (args.ConfigurationPhase == ConfigurationProcessedPhase.UINotified)
		{
			FireOnConfigurationStatusUpdate(new ConfigurationProcessingStatusEventArgs(ConfigurationProcessingStatus.Processed, args));
		}
	}
}
