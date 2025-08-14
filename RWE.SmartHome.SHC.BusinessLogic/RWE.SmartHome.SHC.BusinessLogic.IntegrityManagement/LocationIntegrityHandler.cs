using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;

internal class LocationIntegrityHandler : ICoreIntegrityHandler
{
	private readonly IRepository repositoryProxy;

	public LocationIntegrityHandler(IRepository repositoryProxy)
	{
		this.repositoryProxy = repositoryProxy;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		if (eventType != ConfigEventType.LocationDeleted)
		{
			return;
		}
		foreach (LogicalDevice item in from ld in repositoryProxy.GetLogicalDevices()
			where ld.LocationId.HasValue && ld.LocationId.Value == entityId
			select ld.Clone())
		{
			item.LocationId = null;
			item.Location = null;
			repositoryProxy.SetLogicalDevice(item);
		}
	}

	public List<ConfigEventType> GetHandledEvents()
	{
		List<ConfigEventType> list = new List<ConfigEventType>();
		list.Add(ConfigEventType.LocationDeleted);
		return list;
	}
}
