using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;

public class ConfigurationProcessedEventArgs : EventArgs
{
	public int ConfigurationVersion { get; private set; }

	public List<Location> ModifiedLocations { get; private set; }

	public List<LogicalDevice> ModifiedLogicalDevices { get; private set; }

	public List<BaseDevice> ModifiedBaseDevices { get; private set; }

	public List<Interaction> ModifiedInteractions { get; private set; }

	public List<HomeSetup> ModifiedHomeSetups { get; private set; }

	public List<BaseDevice> DeletedBaseDevices { get; private set; }

	public List<EntityMetadata> RepositoryDeletionReport { get; private set; }

	public List<EntityMetadata> RepositoryModificationReport { get; private set; }

	public List<EntityMetadata> RepositoryInclusionReport { get; private set; }

	public ConfigurationProcessedPhase ConfigurationPhase { get; set; }

	public UpdateReport UpdateReport { get; private set; }

	public ConfigurationProcessedEventArgs(int configurationVersion, List<Location> modifiedLocations, List<LogicalDevice> modifiedLogicalDevices, List<BaseDevice> modifiedBaseDevices, List<Interaction> modifiedInteractions, List<HomeSetup> modifiedHomeSetups, List<BaseDevice> deletedBaseDevices, List<EntityMetadata> repositoryInclusionReport, List<EntityMetadata> repositoryDeletionReport, List<EntityMetadata> repositoryModificationReport, ConfigurationProcessedPhase phase, UpdateReport updateReport)
	{
		ConfigurationVersion = configurationVersion;
		ModifiedLocations = modifiedLocations;
		ModifiedLogicalDevices = modifiedLogicalDevices;
		ModifiedBaseDevices = modifiedBaseDevices;
		ModifiedInteractions = modifiedInteractions;
		ModifiedHomeSetups = modifiedHomeSetups;
		DeletedBaseDevices = deletedBaseDevices;
		RepositoryInclusionReport = repositoryInclusionReport;
		RepositoryDeletionReport = repositoryDeletionReport;
		RepositoryModificationReport = repositoryModificationReport;
		ConfigurationPhase = phase;
		UpdateReport = updateReport;
	}
}
