using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API.Interfaces;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public class ConfigurationRepositoryProxy : IProxyRepository, IRepository, IEntityCache, IIntegrityHandlerAggregator, IIntegrityManagementControl
{
	private IRepository repository;

	private Dictionary<ConfigEventType, List<ICoreIntegrityHandler>> intHandlers;

	private IntegrityManagementHooksState hookingState;

	public int RepositoryVersion
	{
		get
		{
			return repository.RepositoryVersion;
		}
		set
		{
			repository.RepositoryVersion = value;
		}
	}

	public bool IsDirty
	{
		get
		{
			return repository.IsDirty;
		}
		set
		{
			repository.IsDirty = value;
		}
	}

	public ConfigurationRepositoryProxy(IEntityPersistence entityPersistence, IConfigurationSettingsPersistence configVersionPersistence)
	{
		repository = new ConfigurationRepository(entityPersistence, configVersionPersistence);
		intHandlers = new Dictionary<ConfigEventType, List<ICoreIntegrityHandler>>();
		hookingState = IntegrityManagementHooksState.HooksEnabled;
	}

	public BaseDevice GetBaseDevice(Guid id)
	{
		return repository.GetBaseDevice(id);
	}

	public Location GetLocation(Guid id)
	{
		return repository.GetLocation(id);
	}

	public LogicalDevice GetLogicalDevice(Guid id)
	{
		return repository.GetLogicalDevice(id);
	}

	public Interaction GetInteraction(Guid id)
	{
		return repository.GetInteraction(id);
	}

	public List<BaseDevice> GetBaseDevices()
	{
		return repository.GetBaseDevices();
	}

	public List<LogicalDevice> GetLogicalDevices()
	{
		return repository.GetLogicalDevices();
	}

	public List<Location> GetLocations()
	{
		return repository.GetLocations();
	}

	public List<Interaction> GetInteractions()
	{
		return repository.GetInteractions();
	}

	public List<Interaction> GetMarkedForDeleteInteractions()
	{
		return repository.GetMarkedForDeleteInteractions();
	}

	public List<BaseDevice> GetOriginalBaseDevices()
	{
		return repository.GetOriginalBaseDevices();
	}

	public List<LogicalDevice> GetOriginalLogicalDevices()
	{
		return repository.GetOriginalLogicalDevices();
	}

	public List<Location> GetOriginalLocations()
	{
		return repository.GetOriginalLocations();
	}

	public List<Interaction> GetOriginalInteractions()
	{
		return repository.GetOriginalInteractions();
	}

	public List<BaseDevice> GetOriginalBaseDevices(int sinceVersion)
	{
		return repository.GetOriginalBaseDevices(sinceVersion);
	}

	public List<LogicalDevice> GetOriginalLogicalDevices(int sinceVersion)
	{
		return repository.GetOriginalLogicalDevices(sinceVersion);
	}

	public List<Location> GetOriginalLocations(int sinceVersion)
	{
		return repository.GetOriginalLocations(sinceVersion);
	}

	public List<Interaction> GetOriginalInteractions(int sinceVersion)
	{
		return repository.GetOriginalInteractions(sinceVersion);
	}

	public List<BaseDevice> GetModifiedBaseDevices()
	{
		return repository.GetModifiedBaseDevices();
	}

	public List<BaseDevice> GetDeletedBaseDevices()
	{
		return repository.GetDeletedBaseDevices();
	}

	public List<HomeSetup> GetModifiedHomeSetups()
	{
		return repository.GetModifiedHomeSetups();
	}

	public List<LogicalDevice> GetModifiedLogicalDevices()
	{
		return repository.GetModifiedLogicalDevices();
	}

	public List<Location> GetModifiedLocations()
	{
		return repository.GetModifiedLocations();
	}

	public List<Interaction> GetModifiedInteractions()
	{
		return repository.GetModifiedInteractions();
	}

	public List<BaseDevice> GetAffectedBaseDevices(ProtocolIdentifier protocolId)
	{
		return repository.GetAffectedBaseDevices(protocolId);
	}

	public List<BaseDevice> GetAffectedBaseDevices(string appId)
	{
		return repository.GetAffectedBaseDevices(appId);
	}

	public Location GetOriginalLocation(Guid id)
	{
		return repository.GetOriginalLocation(id);
	}

	public BaseDevice GetOriginalBaseDevice(Guid id)
	{
		return repository.GetOriginalBaseDevice(id);
	}

	public LogicalDevice GetOriginalLogicalDevice(Guid id)
	{
		return repository.GetOriginalLogicalDevice(id);
	}

	public Interaction GetOriginalInteraction(Guid id)
	{
		return repository.GetOriginalInteraction(id);
	}

	public void SetLocation(Location location)
	{
		EnsureInitializationDone();
		repository.SetLocation(location);
		NotifyLocationSet(location.Id);
	}

	public void SetLogicalDevice(LogicalDevice logicalDevice)
	{
		EnsureInitializationDone();
		repository.SetLogicalDevice(logicalDevice);
		NotifyLogicalDeviceSet(logicalDevice.Id);
	}

	public void SetBaseDevice(BaseDevice baseDevice)
	{
		EnsureInitializationDone();
		repository.SetBaseDevice(baseDevice);
		NotifyBaseDeviceSet(baseDevice.Id);
	}

	public void SetInteraction(Interaction interaction)
	{
		EnsureInitializationDone();
		SetCreationDate(interaction);
		SetLastChangedDate(interaction);
		repository.SetInteraction(interaction);
		NotifyInteractionSet(interaction.Id);
	}

	public void DeleteLocation(Guid id)
	{
		EnsureInitializationDone();
		repository.DeleteLocation(id);
		NotifyHandlers(ConfigEventType.LocationDeleted, id);
	}

	public void DeleteLogicalDevice(Guid id)
	{
		EnsureInitializationDone();
		repository.DeleteLogicalDevice(id);
		NotifyHandlers(ConfigEventType.DeviceDeleted, id);
	}

	public void DeleteBaseDevice(Guid id)
	{
		EnsureInitializationDone();
		repository.DeleteBaseDevice(id);
		NotifyHandlers(ConfigEventType.BaseDeviceDeleted, id);
	}

	public void DeleteInteraction(Guid id)
	{
		EnsureInitializationDone();
		repository.DeleteInteraction(id);
		NotifyHandlers(ConfigEventType.InteractionDeleted, id);
	}

	public void Commit(CommitType commitType)
	{
		repository.Commit(commitType);
	}

	public void Rollback()
	{
		repository.Rollback();
	}

	public List<EntityMetadata> GenerateRepositoryCommittedDeletionReport(int sinceVersion)
	{
		return repository.GenerateRepositoryCommittedDeletionReport(sinceVersion);
	}

	public void LoadAllEntities()
	{
		repository.LoadAllEntities();
	}

	public void DeleteAllEntities()
	{
		repository.DeleteAllEntities();
	}

	public void PurgeChangeHistory()
	{
		repository.PurgeChangeHistory();
	}

	public bool IsCalibrationPending()
	{
		return repository.IsCalibrationPending();
	}

	public void SubscribeHandler(ICoreIntegrityHandler integrityHandler)
	{
		foreach (ConfigEventType handledEvent in integrityHandler.GetHandledEvents())
		{
			List<ICoreIntegrityHandler> list;
			if (intHandlers.ContainsKey(handledEvent))
			{
				list = intHandlers[handledEvent];
			}
			else
			{
				list = new List<ICoreIntegrityHandler>();
				intHandlers.Add(handledEvent, list);
			}
			if (!list.Contains(integrityHandler))
			{
				list.Add(integrityHandler);
			}
		}
	}

	public void ClearSubscribers()
	{
		intHandlers.Clear();
	}

	private void NotifyLogicalDeviceSet(Guid logicalDeviceId)
	{
		LogicalDevice originalLogicalDevice = repository.GetOriginalLogicalDevice(logicalDeviceId);
		NotifyHandlers((originalLogicalDevice != null) ? ConfigEventType.DeviceUpdated : ConfigEventType.DeviceIncluded, logicalDeviceId);
	}

	private void NotifyBaseDeviceSet(Guid baseDeviceId)
	{
		BaseDevice originalBaseDevice = repository.GetOriginalBaseDevice(baseDeviceId);
		NotifyHandlers((originalBaseDevice != null) ? ConfigEventType.BaseDeviceUpdated : ConfigEventType.BaseDeviceIncluded, baseDeviceId);
	}

	private void NotifyInteractionSet(Guid interactionId)
	{
		Interaction originalInteraction = repository.GetOriginalInteraction(interactionId);
		NotifyHandlers((originalInteraction != null) ? ConfigEventType.InteractionUpdated : ConfigEventType.InteractionCreated, interactionId);
	}

	private void NotifyLocationSet(Guid locationId)
	{
		Location originalLocation = repository.GetOriginalLocation(locationId);
		NotifyHandlers((originalLocation != null) ? ConfigEventType.LocationUpdated : ConfigEventType.LocationCreated, locationId);
	}

	private void NotifyHomeSetupSet(Guid homeSetupId)
	{
		HomeSetup originalHomeSetup = repository.GetOriginalHomeSetup(homeSetupId);
		NotifyHandlers((originalHomeSetup != null) ? ConfigEventType.HomeSetupUpdated : ConfigEventType.HomeSetupIncluded, homeSetupId);
	}

	private void NotifyHandlers(ConfigEventType eventType, Guid entityId)
	{
		if (hookingState == IntegrityManagementHooksState.HooksDisabled || !intHandlers.ContainsKey(eventType))
		{
			return;
		}
		foreach (ICoreIntegrityHandler item in intHandlers[eventType])
		{
			item.Handle(eventType, entityId);
		}
	}

	private void EnsureInitializationDone()
	{
		if (intHandlers == null)
		{
			throw new ApplicationException("Unsafe call made to uninitialized repository.");
		}
	}

	public void UpdateLocation(Location location)
	{
		repository.SetLocation(location);
		NotifyHandlers(ConfigEventType.LocationUpdated, location.Id);
	}

	public void UpdateLogicalDevice(LogicalDevice logicalDevice)
	{
		repository.SetLogicalDevice(logicalDevice);
		NotifyHandlers(ConfigEventType.DeviceUpdated, logicalDevice.Id);
	}

	public void UpdateBaseDevice(BaseDevice baseDevice)
	{
		repository.SetBaseDevice(baseDevice);
		NotifyHandlers(ConfigEventType.BaseDeviceUpdated, baseDevice.Id);
	}

	private void SetLastChangedDate(Interaction interaction)
	{
		if (repository.GetInteraction(interaction.Id) != null || interaction.LastChangeDate.Equals(DateTime.MinValue))
		{
			interaction.LastChangeDate = ShcDateTime.UtcNow;
		}
	}

	private void SetCreationDate(Interaction interaction)
	{
		if (repository.GetInteraction(interaction.Id) == null && interaction.CreationDate < new DateTime(2010, 1, 1))
		{
			interaction.CreationDate = ShcDateTime.UtcNow;
		}
	}

	public void SetIntegrityManagementHooksState(IntegrityManagementHooksState hookingState)
	{
		this.hookingState = hookingState;
	}

	public ConfigurationUpdateReport GetUpdateReport()
	{
		return repository.GetUpdateReport();
	}

	public HomeSetup GetHomeSetup(Guid id)
	{
		return repository.GetHomeSetup(id);
	}

	public void SetHomeSetup(HomeSetup homeSetup)
	{
		EnsureInitializationDone();
		repository.SetHomeSetup(homeSetup);
		NotifyHomeSetupSet(homeSetup.Id);
	}

	public void DeleteHomeSetup(Guid id)
	{
		EnsureInitializationDone();
		repository.DeleteHomeSetup(id);
		NotifyHandlers(ConfigEventType.HomeSetupDeleted, id);
	}

	public List<HomeSetup> GetHomeSetups()
	{
		return repository.GetHomeSetups();
	}

	public List<HomeSetup> GetOriginalHomeSetups()
	{
		return repository.GetOriginalHomeSetups();
	}

	public List<HomeSetup> GetOriginalHomeSetups(int sinceVersion)
	{
		return repository.GetOriginalHomeSetups(sinceVersion);
	}

	public HomeSetup GetOriginalHomeSetup(Guid id)
	{
		return repository.GetOriginalHomeSetup(id);
	}

	public Home GetHome(Guid id)
	{
		throw new NotImplementedException();
	}

	public Member GetMember(Guid id)
	{
		throw new NotImplementedException();
	}
}
