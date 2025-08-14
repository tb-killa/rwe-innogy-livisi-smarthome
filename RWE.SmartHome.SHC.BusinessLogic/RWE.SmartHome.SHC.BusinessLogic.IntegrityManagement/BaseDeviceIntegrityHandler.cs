using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;

internal class BaseDeviceIntegrityHandler : ICoreIntegrityHandler
{
	private IRepository repositoryProxy;

	public BaseDeviceIntegrityHandler(IRepository repositoryProxy)
	{
		this.repositoryProxy = repositoryProxy;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		if (eventType != ConfigEventType.DeviceDeleted)
		{
			return;
		}
		List<BaseDevice> list = (from bd in repositoryProxy.GetBaseDevices()
			where bd.LogicalDeviceIds.Any((Guid ldid) => ldid == entityId)
			select bd).ToList();
		foreach (BaseDevice item in list)
		{
			repositoryProxy.DeleteBaseDevice(item.Id);
		}
	}

	public List<ConfigEventType> GetHandledEvents()
	{
		List<ConfigEventType> list = new List<ConfigEventType>();
		list.Add(ConfigEventType.DeviceDeleted);
		return list;
	}
}
