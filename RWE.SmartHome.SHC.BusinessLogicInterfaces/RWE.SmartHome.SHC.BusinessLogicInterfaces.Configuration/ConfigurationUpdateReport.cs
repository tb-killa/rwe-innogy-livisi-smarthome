using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public class ConfigurationUpdateReport
{
	public List<EntityMetadata> NewEntities { get; private set; }

	public List<EntityMetadata> ModifiedEntities { get; private set; }

	public List<EntityMetadata> DeletedEntities { get; private set; }

	public List<Location> NewLocations { get; private set; }

	public List<Location> ModifiedLocations { get; private set; }

	public List<Location> DeletedLocations { get; private set; }

	public List<BaseDevice> NewBaseDevices { get; private set; }

	public List<BaseDevice> ModifiedBaseDevices { get; private set; }

	public List<BaseDevice> DeletedBaseDevices { get; private set; }

	public List<LogicalDevice> NewLogicalDevices { get; private set; }

	public List<LogicalDevice> ModifiedLogicalDevices { get; private set; }

	public List<LogicalDevice> DeletedLogicalDevices { get; private set; }

	public List<Interaction> NewInteractions { get; private set; }

	public List<Interaction> ModifiedInteractions { get; private set; }

	public List<Interaction> DeletedInteractions { get; private set; }

	public List<HomeSetup> NewHomeSetups { get; private set; }

	public List<HomeSetup> ModifiedHomeSetups { get; private set; }

	public List<HomeSetup> DeletedHomeSetups { get; private set; }

	public ConfigurationUpdateReport(List<EntityMetadata> newEntities, List<EntityMetadata> modifiedEntities, List<EntityMetadata> deletedEntities, List<Location> newLocations, List<Location> modifiedLocations, List<Location> deletedLocations, List<BaseDevice> newBaseDevices, List<BaseDevice> modifiedBaseDevices, List<BaseDevice> deletedBaseDevices, List<LogicalDevice> newLogicalDevices, List<LogicalDevice> modifiedLogicalDevices, List<LogicalDevice> deletedLogicalDevices, List<Interaction> newInteractions, List<Interaction> modifiedInteractions, List<Interaction> deletedInteractions, List<HomeSetup> newHomeSetups, List<HomeSetup> modifiedHomeSetups, List<HomeSetup> deletedHomeSetups)
	{
		NewEntities = newEntities;
		ModifiedEntities = modifiedEntities;
		DeletedEntities = deletedEntities;
		NewLocations = newLocations;
		ModifiedLocations = modifiedLocations;
		DeletedLocations = deletedLocations;
		NewBaseDevices = newBaseDevices;
		ModifiedBaseDevices = modifiedBaseDevices;
		DeletedBaseDevices = deletedBaseDevices;
		NewLogicalDevices = newLogicalDevices;
		ModifiedLogicalDevices = modifiedLogicalDevices;
		DeletedLogicalDevices = deletedLogicalDevices;
		NewInteractions = newInteractions;
		ModifiedInteractions = modifiedInteractions;
		DeletedInteractions = deletedInteractions;
		NewHomeSetups = newHomeSetups;
		ModifiedHomeSetups = modifiedHomeSetups;
		DeletedHomeSetups = deletedHomeSetups;
	}
}
