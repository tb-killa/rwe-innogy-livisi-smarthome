using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

internal class ModifiedRelationsGraphConfigurationRepository : IEntityRelationsGraphConfigurationProxy
{
	private readonly IRepository configurationRepository;

	public ModifiedRelationsGraphConfigurationRepository(IRepository configurationRepository)
	{
		this.configurationRepository = configurationRepository;
	}

	public List<Interaction> GetInteractions()
	{
		return configurationRepository.GetModifiedInteractions();
	}

	public BaseDevice GetBaseDevice(Guid id)
	{
		return configurationRepository.GetBaseDevice(id);
	}

	public LogicalDevice GetLogicalDevice(Guid id)
	{
		return configurationRepository.GetLogicalDevice(id);
	}
}
