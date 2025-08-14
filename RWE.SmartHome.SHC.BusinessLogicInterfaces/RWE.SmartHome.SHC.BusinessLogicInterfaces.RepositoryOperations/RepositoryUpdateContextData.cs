using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations.Enums;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

public class RepositoryUpdateContextData
{
	public readonly string Id = Guid.NewGuid().ToString();

	public string AppId { get; private set; }

	public RestorePointCreationOptions CreateRestorePoint { get; private set; }

	public ForcePushDeviceConfiguration ForcePushDeviceConfiguration { get; set; }

	public SkipConfigurationValidation SkipConfigurationValidation { get; set; }

	public UpdateReport UpdateReport { get; set; }

	public RepositoryUpdateContextData()
	{
	}

	public RepositoryUpdateContextData(string appId)
		: this(appId, RestorePointCreationOptions.No)
	{
	}

	public RepositoryUpdateContextData(string appId, RestorePointCreationOptions createRestorePoint)
		: this(appId, createRestorePoint, ForcePushDeviceConfiguration.No)
	{
		AppId = appId;
		CreateRestorePoint = createRestorePoint;
	}

	public RepositoryUpdateContextData(string appId, ForcePushDeviceConfiguration forcePushDeviceConfiguration)
		: this(appId)
	{
		AppId = appId;
		ForcePushDeviceConfiguration = forcePushDeviceConfiguration;
	}

	public RepositoryUpdateContextData(string appId, RestorePointCreationOptions createRestorePoint, ForcePushDeviceConfiguration forcePushDeviceConfiguration)
	{
		AppId = appId;
		CreateRestorePoint = createRestorePoint;
		ForcePushDeviceConfiguration = forcePushDeviceConfiguration;
	}

	public RepositoryUpdateContextData(string appId, RestorePointCreationOptions createRestorePoint, ForcePushDeviceConfiguration forcePushDeviceConfiguration, SkipConfigurationValidation skipConfigurationValidation)
	{
		AppId = appId;
		CreateRestorePoint = createRestorePoint;
		ForcePushDeviceConfiguration = forcePushDeviceConfiguration;
		SkipConfigurationValidation = skipConfigurationValidation;
	}

	public RepositoryUpdateContextData(ForcePushDeviceConfiguration forcePushDeviceConfiguration)
		: this()
	{
		ForcePushDeviceConfiguration = forcePushDeviceConfiguration;
	}
}
