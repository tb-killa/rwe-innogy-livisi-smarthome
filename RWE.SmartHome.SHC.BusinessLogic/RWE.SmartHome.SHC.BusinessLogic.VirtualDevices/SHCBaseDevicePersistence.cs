using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class SHCBaseDevicePersistence
{
	private const string LoggingSource = "SHCBaseDevice Persistence";

	private const int MaxUpdateTimeout = 120000;

	private readonly IRepository repository;

	private readonly IRepositorySync repositorySync;

	private readonly object syncObject = new object();

	private readonly INotificationHandler notificationHandler;

	public SHCBaseDevicePersistence(INotificationHandler notificationHandler, IRepository repository, IRepositorySync repositorySync)
	{
		this.notificationHandler = notificationHandler;
		this.repository = repository;
		this.repositorySync = repositorySync;
	}

	public bool AddShcBaseDevice(BaseDevice baseDevice)
	{
		BaseDevice shcBaseDevice = repository.GetShcBaseDevice();
		if (shcBaseDevice == null)
		{
			Log.Information(Module.BusinessLogic, "SHCBaseDevice Persistence", "SHCBaseDevice device not found, adding.");
			repository.SetBaseDevice(baseDevice);
			return true;
		}
		Log.Information(Module.BusinessLogic, "SHCBaseDevice Persistence", "SHCBaseDevice found, nothing to do.");
		return false;
	}

	public bool Update(Func<BaseDevice, IRepository, bool> baseDeviceUpdate)
	{
		lock (syncObject)
		{
			bool flag = false;
			RepositoryLockContext repositoryLockContext = null;
			try
			{
				repositoryLockContext = repositorySync.WaitForLock("SHCBaseDevicePersistence::Update", new RepositoryUpdateContextData(CoreConstants.CoreAppId), 120000);
				if (repositoryLockContext == null)
				{
					Log.Error(Module.BusinessLogic, "SHCBaseDevice Persistence", "Could not aquire lock for SHCBD update.");
					return false;
				}
				BaseDevice baseDevice = repository.GetShcBaseDevice();
				if (baseDevice != null)
				{
					baseDevice = baseDevice.Clone();
				}
				flag = baseDeviceUpdate(baseDevice, repository);
				if (baseDevice != null)
				{
					repository.SetBaseDevice(baseDevice);
				}
				repositoryLockContext.Commit = flag;
				repositoryLockContext.Dispose();
				repositoryLockContext = null;
			}
			catch (Exception)
			{
				flag = false;
			}
			finally
			{
				if (repositoryLockContext != null)
				{
					repositoryLockContext.Dispose();
					repositoryLockContext = null;
				}
			}
			if (notificationHandler != null && flag)
			{
				Log.Information(Module.BusinessLogic, "SHCBaseDevice Persistence", "Configuration property updated");
			}
			return flag;
		}
	}
}
