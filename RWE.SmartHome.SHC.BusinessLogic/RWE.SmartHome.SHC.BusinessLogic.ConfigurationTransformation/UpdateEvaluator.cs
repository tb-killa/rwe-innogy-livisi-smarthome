using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

internal class UpdateEvaluator
{
	private readonly IRepository repository;

	private UpdateReport updateReport;

	private readonly EntityRelationsGraph originalsEntitiesRelationsGraph;

	private readonly EntityRelationsGraph modifiedOriginalsEntitiesRelationsGraph;

	private readonly EntityRelationsGraph modifiedEntitiesRelationsGraph;

	public UpdateReport UpdateReport
	{
		get
		{
			if (updateReport == null)
			{
				Init();
			}
			return updateReport;
		}
	}

	public UpdateEvaluator(IRepository repository)
	{
		this.repository = repository;
		originalsEntitiesRelationsGraph = new EntityRelationsGraph(new OriginalsRelationsGraphConfigurationRepository(repository));
		modifiedOriginalsEntitiesRelationsGraph = new EntityRelationsGraph(new ModifiedOriginalsRelationsGraphConfigurationRepository(repository));
		modifiedEntitiesRelationsGraph = new EntityRelationsGraph(new ModifiedRelationsGraphConfigurationRepository(repository));
	}

	private UpdateReport Init()
	{
		updateReport = new UpdateReport();
		ConfigurationUpdateReport configurationUpdateReport = repository.GetUpdateReport();
		updateReport.RepositoryDeletionReport = configurationUpdateReport.DeletedEntities;
		List<Location> modifiedLocations = configurationUpdateReport.ModifiedLocations;
		foreach (Location item in modifiedLocations)
		{
			_ = item;
		}
		if (configurationUpdateReport.DeletedLocations.Count > 0)
		{
			updateReport.AddValidateAll();
		}
		bool flag = configurationUpdateReport.ModifiedHomeSetups.Count > 0 && repository.GetOriginalHomeSetups().Count > 0;
		if (configurationUpdateReport.DeletedHomeSetups.Count > 0 || flag)
		{
			updateReport.AddValidateAll();
		}
		foreach (BaseDevice modifiedBaseDevice in configurationUpdateReport.ModifiedBaseDevices)
		{
			CheckBaseDevice(modifiedBaseDevice);
		}
		if (flag)
		{
			HandleModifiedHomeSetup();
		}
		foreach (BaseDevice deletedBaseDevice in configurationUpdateReport.DeletedBaseDevices)
		{
			HandleModifiedDevice(deletedBaseDevice);
		}
		foreach (LogicalDevice modifiedLogicalDevice in configurationUpdateReport.ModifiedLogicalDevices)
		{
			CheckLogicalDevice(modifiedLogicalDevice);
		}
		foreach (Interaction modifiedInteraction in configurationUpdateReport.ModifiedInteractions)
		{
			CheckModifiedInteraction(modifiedInteraction);
		}
		foreach (Interaction deletedInteraction in configurationUpdateReport.DeletedInteractions)
		{
			HandleModifiedInteraction(deletedInteraction);
		}
		return updateReport;
	}

	private void CheckModifiedInteraction(Interaction modifiedInteraction)
	{
		HandleModifiedInteraction(modifiedInteraction);
	}

	private void HandleModifiedInteraction(Interaction modifiedInteraction)
	{
		List<BaseDevice> list = new List<BaseDevice>();
		list.AddRange(originalsEntitiesRelationsGraph.GetRelatedBaseDevices(modifiedInteraction));
		list.AddRange(modifiedOriginalsEntitiesRelationsGraph.GetRelatedBaseDevices(modifiedInteraction));
		list.AddRange(modifiedEntitiesRelationsGraph.GetRelatedBaseDevices(modifiedInteraction));
		foreach (BaseDevice item in list)
		{
			updateReport.AddAppUpdate(item.AppId);
			updateReport.AddAppValidation(CoreConstants.CoreAppId);
			updateReport.AddAppValidation(item.AppId);
			updateReport.AddProtocolUpdate(item.ProtocolId);
		}
	}

	private void CheckBaseDevice(BaseDevice modifiedBaseDevice)
	{
		BaseDevice originalBaseDevice = repository.GetOriginalBaseDevice(modifiedBaseDevice.Id);
		if (originalBaseDevice != null)
		{
			Guid? locationId = originalBaseDevice.LocationId;
			Guid? locationId2 = modifiedBaseDevice.LocationId;
			if (locationId.HasValue == locationId2.HasValue && (!locationId.HasValue || !(locationId.GetValueOrDefault() != locationId2.GetValueOrDefault())) && (modifiedBaseDevice.VolatileProperties == null || modifiedBaseDevice.VolatileProperties.Count <= 0) && !IsPropListChanged(originalBaseDevice.Properties, modifiedBaseDevice.Properties))
			{
				return;
			}
		}
		HandleModifiedDevice(modifiedBaseDevice);
	}

	private void HandleModifiedHomeSetup()
	{
		updateReport.AddAppUpdate("sh://SunriseSunsetSensor.RWE");
	}

	private void HandleModifiedDevice(BaseDevice baseDevice)
	{
		if (ShcBaseDeviceIRepositoryExtensions.ShcBaseDevicesPredicate(baseDevice))
		{
			updateReport.AddAppUpdate("sh://SunriseSunsetSensor.RWE");
		}
		updateReport.AddAppUpdate(baseDevice.AppId);
		updateReport.AddAppValidation(CoreConstants.CoreAppId);
		updateReport.AddAppValidation(baseDevice.AppId);
		updateReport.AddProtocolUpdate(baseDevice.ProtocolId);
	}

	private void CheckLogicalDevice(LogicalDevice modifiedLogicalDevice)
	{
		LogicalDevice originalLogicalDevice = repository.GetOriginalLogicalDevice(modifiedLogicalDevice.Id);
		if (originalLogicalDevice != null)
		{
			Guid? locationId = originalLogicalDevice.LocationId;
			Guid? locationId2 = modifiedLogicalDevice.LocationId;
			if (locationId.HasValue == locationId2.HasValue && (!locationId.HasValue || !(locationId.GetValueOrDefault() != locationId2.GetValueOrDefault())) && !(originalLogicalDevice.PrimaryPropertyName != modifiedLogicalDevice.PrimaryPropertyName))
			{
				bool? activityLogActive = originalLogicalDevice.ActivityLogActive;
				bool? activityLogActive2 = modifiedLogicalDevice.ActivityLogActive;
				if (activityLogActive == true == (activityLogActive2 == true) && activityLogActive.HasValue == activityLogActive2.HasValue && !IsPropListChanged(originalLogicalDevice.GetAllProperties(), modifiedLogicalDevice.GetAllProperties()))
				{
					return;
				}
			}
		}
		HandleModifiedLogicalDevice(modifiedLogicalDevice);
	}

	private void HandleModifiedLogicalDevice(LogicalDevice modifiedLogicalDevice)
	{
		HandleModifiedDevice(modifiedLogicalDevice.BaseDevice);
	}

	private bool IsPropListChanged(List<Property> oldProperties, List<Property> newProperties)
	{
		if (oldProperties.Count != newProperties.Count)
		{
			return true;
		}
		foreach (Property newProperty in newProperties)
		{
			string propertyName = newProperty.Name;
			Property property = oldProperties.FirstOrDefault((Property x) => x.Name == propertyName);
			try
			{
				if (property == null)
				{
					return true;
				}
				IComparable valueAsComparable = property.GetValueAsComparable();
				IComparable valueAsComparable2 = newProperty.GetValueAsComparable();
				if ((valueAsComparable == null && valueAsComparable2 != null) || (valueAsComparable != null && valueAsComparable.CompareTo(valueAsComparable2) != 0))
				{
					return true;
				}
			}
			catch (Exception)
			{
				return true;
			}
		}
		return false;
	}
}
