using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.RuleEngineInterfaces;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

public class AddinsConfigurationRepository : IAddinsConfigurationRepository
{
	private readonly IRepository configurationRepository;

	private readonly SubscriptionToken configurationChangedSubscription;

	private readonly IDynamicSettingsResolver dynamicSettingsResolver;

	private readonly object syncRoot = new object();

	private AddinsConfigurationCache addinsValidationCache;

	private AddinsConfigurationCache addinsCache;

	public event EventHandler<AddinConfigurationUpdatedEventArgs> ConfigurationChanged;

	public AddinsConfigurationRepository(IRepository configurationRepository, IEventManager eventManager, IDynamicSettingsResolver dynamicSettingsResolver)
	{
		this.configurationRepository = configurationRepository;
		this.dynamicSettingsResolver = dynamicSettingsResolver;
		configurationChangedSubscription = eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, (ConfigurationProcessedEventArgs args) => args.ConfigurationPhase == ConfigurationProcessedPhase.UINotified, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChangeStarted, (ConfigurationProcessedEventArgs args) => args.ConfigurationPhase == ConfigurationProcessedPhase.Starting, ThreadOption.PublisherThread, null);
	}

	private void OnConfigurationChangeStarted(ConfigurationProcessedEventArgs args)
	{
		lock (syncRoot)
		{
			addinsValidationCache = RefreshConfigDataExt(GetAddinsCache(), args.ModifiedBaseDevices, args.DeletedBaseDevices, args.ModifiedLogicalDevices, (from y in args.RepositoryDeletionReport
				where y.EntityType == EntityType.LogicalDevice
				select configurationRepository.GetOriginalLogicalDevice(y.Id)).ToList(), configurationRepository.GetInteractions());
		}
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		lock (syncRoot)
		{
			List<string> allUpdatedApps = args.UpdateReport.AllUpdatedApps;
			allUpdatedApps = allUpdatedApps.Where((string x) => x != CoreConstants.CoreAppId).ToList();
			addinsCache = addinsValidationCache;
			addinsValidationCache = null;
			if (this.ConfigurationChanged != null)
			{
				AddinConfigurationUpdatedEventArgs e = new AddinConfigurationUpdatedEventArgs();
				e.AffectedAppIds = allUpdatedApps.ToArray();
				e.AddinsCache = addinsCache;
				AddinConfigurationUpdatedEventArgs e2 = e;
				this.ConfigurationChanged(this, e2);
				ClearVolatilePropertiesFromCache(addinsCache);
			}
		}
	}

	private void ClearVolatilePropertiesFromCache(AddinsConfigurationCache addinsConfigurationCache)
	{
		foreach (string item in addinsConfigurationCache.ListAddinsIds())
		{
			foreach (Device device in addinsConfigurationCache.GetAddinConfiguration(item).Devices)
			{
				device.VolatileProperties = new List<global::SmartHome.SHC.API.PropertyDefinition.Property>();
			}
		}
	}

	internal void UpdateValidationCache(UpdateReport updateReport)
	{
		lock (syncRoot)
		{
			addinsValidationCache = RefreshConfigDataExt(addinsCache, configurationRepository.GetModifiedBaseDevices(), (from y in updateReport.RepositoryDeletionReport
				where y.EntityType == EntityType.BaseDevice
				select configurationRepository.GetOriginalBaseDevice(y.Id)).ToList(), configurationRepository.GetModifiedLogicalDevices(), (from y in updateReport.RepositoryDeletionReport
				where y.EntityType == EntityType.LogicalDevice
				select configurationRepository.GetOriginalLogicalDevice(y.Id)).ToList(), configurationRepository.GetInteractions());
		}
	}

	private AddinsConfigurationCache RefreshConfigData(IEnumerable<LogicalDevice> coreLogicalDevices, IEnumerable<BaseDevice> coreBaseDevices, IEnumerable<Interaction> coreInteractions)
	{
		AddinsConfigurationCache addinsConfigurationCache = new AddinsConfigurationCache();
		RefreshConfigCache refreshConfigCache = new RefreshConfigCache();
		foreach (BaseDevice coreBaseDevice in coreBaseDevices)
		{
			string appId = coreBaseDevice.AppId;
			refreshConfigCache.AddBaseDevice(coreBaseDevice.Id, coreBaseDevice.AppId);
			if (appId != CoreConstants.CoreAppId)
			{
				addinsConfigurationCache.AddDevice(appId, coreBaseDevice.ToApiBaseDevice());
			}
		}
		foreach (LogicalDevice coreLogicalDevice in coreLogicalDevices)
		{
			string baseDeviceAppId = refreshConfigCache.GetBaseDeviceAppId(coreLogicalDevice.BaseDeviceId);
			refreshConfigCache.AddLogicalDevice(coreLogicalDevice.Id, baseDeviceAppId);
			if (baseDeviceAppId != CoreConstants.CoreAppId)
			{
				addinsConfigurationCache.AddCapability(baseDeviceAppId, coreLogicalDevice.ToApiCapability());
			}
		}
		foreach (Interaction coreInteraction in coreInteractions)
		{
			foreach (Rule rule in coreInteraction.Rules)
			{
				foreach (RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription action in rule.Actions)
				{
					string logicalDeviceAppId = refreshConfigCache.GetLogicalDeviceAppId(action.Target.EntityIdAsGuid());
					if (!string.IsNullOrEmpty(logicalDeviceAppId) && !(logicalDeviceAppId == CoreConstants.CoreAppId))
					{
						addinsConfigurationCache.AddActionDescription(logicalDeviceAppId, new global::SmartHome.SHC.API.Configuration.ActionDescription(action.Id, action.Target.ToApi(), action.ActionType, from p in dynamicSettingsResolver.GetTargetTypes(action.Data)
							select p.ToApiProperty()));
					}
				}
				string appId2;
				foreach (string item in addinsConfigurationCache.ListAddinsIds())
				{
					appId2 = item;
					Guid intId = coreInteraction.Id;
					string intName = coreInteraction.Name;
					IEnumerable<global::SmartHome.SHC.API.Configuration.Trigger> enumerable = rule.Triggers.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger x) => x.ToApiInteractionTrigger(new InteractionDetails(intId, intName)));
					foreach (global::SmartHome.SHC.API.Configuration.Trigger item2 in enumerable)
					{
						addinsConfigurationCache.AddTrigger(appId2, item2);
					}
					if (rule.CustomTriggers == null)
					{
						continue;
					}
					List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger> source = rule.CustomTriggers.Where((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger x) => x != null).ToList();
					foreach (RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger item3 in source.Where((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger ct) => IsAddinCustomTrigger(refreshConfigCache, ct, appId2)))
					{
						addinsConfigurationCache.AddCustomTrigger(appId2, item3.ToApiCustomTrigger());
					}
				}
			}
		}
		return addinsConfigurationCache;
	}

	private AddinsConfigurationCache RefreshConfigDataExt(AddinsConfigurationCache currentCache, IEnumerable<BaseDevice> modifiedBaseDevices, IEnumerable<BaseDevice> deletedBaseDevices, IEnumerable<LogicalDevice> modifiedLogicalDevices, IEnumerable<LogicalDevice> deletedLogicalDevices, IEnumerable<Interaction> coreInteractions)
	{
		AddinsConfigurationCache ret = currentCache.Clone();
		foreach (BaseDevice item in modifiedBaseDevices.Where((BaseDevice modifiedBaseDevice) => modifiedBaseDevice.AppId != CoreConstants.CoreAppId))
		{
			ret.UpdateBaseDevice(item.AppId, item.ToApiBaseDevice());
		}
		foreach (BaseDevice deletedBaseDevice in deletedBaseDevices)
		{
			ret.DeleteBaseDevice(deletedBaseDevice.AppId, deletedBaseDevice.Id);
		}
		foreach (LogicalDevice modifiedLogicalDevice in modifiedLogicalDevices)
		{
			ret.UpdateCapability(modifiedLogicalDevice.BaseDevice.AppId, modifiedLogicalDevice.ToApiCapability());
		}
		foreach (LogicalDevice deletedLogicalDevice in deletedLogicalDevices)
		{
			BaseDevice originalBaseDevice = configurationRepository.GetOriginalBaseDevice(deletedLogicalDevice.BaseDeviceId);
			ret.DeleteLogicalDevice(originalBaseDevice.AppId, deletedLogicalDevice.Id);
		}
		ret.ClearInteractionsInfo();
		foreach (Interaction coreInteraction in coreInteractions)
		{
			foreach (Rule rule in coreInteraction.Rules)
			{
				foreach (RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription action in rule.Actions)
				{
					string logicalDeviceAppId = ret.GetLogicalDeviceAppId(action.Target.EntityIdAsGuid());
					if (!string.IsNullOrEmpty(logicalDeviceAppId) && !(logicalDeviceAppId == CoreConstants.CoreAppId))
					{
						ret.AddActionDescription(logicalDeviceAppId, new global::SmartHome.SHC.API.Configuration.ActionDescription(action.Id, action.Target.ToApi(), action.ActionType, from p in dynamicSettingsResolver.GetTargetState(action.Data)
							select p.ToApiProperty()));
					}
				}
				foreach (string item2 in ret.ListAddinsIds())
				{
					Guid intId = coreInteraction.Id;
					string intName = coreInteraction.Name;
					IEnumerable<global::SmartHome.SHC.API.Configuration.Trigger> enumerable = rule.Triggers.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger x) => x.ToApiInteractionTrigger(new InteractionDetails(intId, intName)));
					foreach (global::SmartHome.SHC.API.Configuration.Trigger item3 in enumerable)
					{
						ret.AddTrigger(item2, item3);
					}
					if (rule.CustomTriggers == null)
					{
						continue;
					}
					string id = item2;
					List<global::SmartHome.SHC.API.Configuration.CustomTrigger> list = (from y in rule.CustomTriggers
						where IsAddinCustomTrigger(ret, y, id)
						select y.ToApiCustomTrigger()).ToList();
					foreach (global::SmartHome.SHC.API.Configuration.CustomTrigger item4 in list)
					{
						ret.AddCustomTrigger(item2, item4);
					}
				}
			}
		}
		return ret;
	}

	private bool IsAddinCustomTrigger(RefreshConfigCache refreshConfigCache, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger ct, string appId)
	{
		return ct.Entity.LinkType switch
		{
			EntityType.LogicalDevice => appId == refreshConfigCache.GetLogicalDeviceAppId(ct.Entity.EntityIdAsGuid()), 
			EntityType.BaseDevice => appId == refreshConfigCache.GetBaseDeviceAppId(ct.Entity.EntityIdAsGuid()), 
			_ => false, 
		};
	}

	private bool IsAddinCustomTrigger(AddinsConfigurationCache cache, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger ct, string appId)
	{
		if (ct == null)
		{
			return false;
		}
		return ct.Entity.LinkType switch
		{
			EntityType.LogicalDevice => appId == cache.GetLogicalDeviceAppId(ct.Entity.EntityIdAsGuid()), 
			EntityType.BaseDevice => appId == cache.GetBaseDeviceAppId(ct.Entity.EntityIdAsGuid()), 
			_ => false, 
		};
	}

	private AddinsConfigurationCache GetAddinsCache()
	{
		if (addinsCache == null)
		{
			addinsCache = RefreshConfigData(configurationRepository.GetOriginalLogicalDevices(), configurationRepository.GetOriginalBaseDevices(), configurationRepository.GetOriginalInteractions());
		}
		return addinsCache;
	}

	public AddinConfiguration GetConfiguration(string appId)
	{
		lock (syncRoot)
		{
			return GetAddinsCache().GetAddinConfiguration(appId);
		}
	}

	public AddinConfiguration GetValidationConfiguration(string appId)
	{
		lock (syncRoot)
		{
			return (addinsValidationCache != null) ? addinsValidationCache.GetAddinConfiguration(appId) : new AddinConfiguration(appId);
		}
	}
}
