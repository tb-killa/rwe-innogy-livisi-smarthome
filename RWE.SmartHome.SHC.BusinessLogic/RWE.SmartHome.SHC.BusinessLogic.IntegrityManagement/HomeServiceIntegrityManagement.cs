using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;

public class HomeServiceIntegrityManagement : ICoreIntegrityHandler
{
	private readonly IProxyRepository repository;

	public HomeServiceIntegrityManagement(IProxyRepository repository)
	{
		this.repository = repository;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		if (eventType == ConfigEventType.HomeSetupUpdated)
		{
			HomeSetupUpdated(entityId);
		}
	}

	public List<ConfigEventType> GetHandledEvents()
	{
		List<ConfigEventType> list = new List<ConfigEventType>();
		list.Add(ConfigEventType.HomeSetupUpdated);
		return list;
	}

	private void HomeSetupUpdated(Guid homeSetupId)
	{
		HomeSetup homeSetup = repository.GetHomeSetup(homeSetupId);
		if (homeSetup == null)
		{
			return;
		}
		if (homeSetup.Properties.All((Property m) => !m.Name.Equals("GeoLocation")))
		{
			HomeSetup originalHomeSetup = repository.GetOriginalHomeSetup(homeSetup.Id);
			Property property = originalHomeSetup.Properties.FirstOrDefault((Property m) => m.Name.Equals("GeoLocation"));
			if (property != null)
			{
				homeSetup.Properties.Add(property);
			}
		}
		repository.SetHomeSetup(homeSetup);
	}
}
