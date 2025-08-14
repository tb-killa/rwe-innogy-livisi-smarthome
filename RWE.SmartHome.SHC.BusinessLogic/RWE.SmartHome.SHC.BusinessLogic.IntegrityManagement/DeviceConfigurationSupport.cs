using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;

internal class DeviceConfigurationSupport
{
	private readonly IRepository repository;

	private readonly IRepositorySync repositoryLock;

	public DeviceConfigurationSupport(IApplicationsHost appHost, IRepository repository, IRepositorySync repositoryLock)
	{
		this.repository = repository;
		this.repositoryLock = repositoryLock;
		appHost.ApplicationStateChanged += AppHost_ApplicationStateChanged;
	}

	private void AppHost_ApplicationStateChanged(ApplicationLoadStateChangedEventArgs args)
	{
		if (args.ApplicationState == ApplicationStates.ApplicationsUninstalled)
		{
			RemoveAppDevices(args.Application.ApplicationId);
		}
	}

	private void RemoveAppDevices(string appId)
	{
		using RepositoryLockContext repositoryLockContext = repositoryLock.GetLock("DeviceConfigurationSupport RemoveAppDevices", new RepositoryUpdateContextData(CoreConstants.CoreAppId));
		List<BaseDevice> list = (from baseDevice in repository.GetBaseDevices()
			where baseDevice.AppId == appId
			select baseDevice).ToList();
		list.ForEach(delegate(BaseDevice baseDevice)
		{
			repository.DeleteBaseDevice(baseDevice.Id);
		});
		repositoryLockContext.Commit = true;
	}
}
