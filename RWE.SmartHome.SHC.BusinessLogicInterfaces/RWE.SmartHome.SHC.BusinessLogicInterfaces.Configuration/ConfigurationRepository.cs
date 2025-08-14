using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API.Interfaces;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public class ConfigurationRepository : IRepository, IEntityCache
{
	private enum EntityRepositoryState
	{
		Unchanged,
		New,
		Changed,
		Deleted
	}

	private class RepositoryEntry<T> where T : Entity
	{
		public T ActualValue { get; set; }

		public T OriginalValue { get; set; }

		public EntityRepositoryState State { get; set; }

		public RepositoryEntry(T value, EntityRepositoryState state)
		{
			State = state;
			switch (state)
			{
			case EntityRepositoryState.Unchanged:
				OriginalValue = value;
				break;
			case EntityRepositoryState.New:
			case EntityRepositoryState.Changed:
				ActualValue = value;
				break;
			default:
				throw new ArgumentOutOfRangeException("state");
			}
		}
	}

	private readonly Dictionary<Guid, RepositoryEntry<Location>> locations = new Dictionary<Guid, RepositoryEntry<Location>>();

	private readonly Dictionary<Guid, RepositoryEntry<BaseDevice>> baseDevices = new Dictionary<Guid, RepositoryEntry<BaseDevice>>();

	private readonly Dictionary<Guid, RepositoryEntry<LogicalDevice>> logicalDevices = new Dictionary<Guid, RepositoryEntry<LogicalDevice>>();

	private readonly List<KeyValuePair<int, EntityMetadata>> deletionHistory = new List<KeyValuePair<int, EntityMetadata>>();

	private readonly IEntityPersistence entityPersistence;

	private readonly IConfigurationSettingsPersistence configurationSettingsPersistence;

	private int repositoryVersion;

	private readonly Dictionary<Guid, RepositoryEntry<Interaction>> interactions = new Dictionary<Guid, RepositoryEntry<Interaction>>();

	private readonly Dictionary<Guid, RepositoryEntry<HomeSetup>> homeSetups = new Dictionary<Guid, RepositoryEntry<HomeSetup>>();

	public int RepositoryVersion
	{
		get
		{
			return repositoryVersion;
		}
		set
		{
			repositoryVersion = value;
		}
	}

	public bool IsDirty
	{
		get
		{
			return configurationSettingsPersistence.LoadConfigurationDirtyFlag();
		}
		set
		{
			configurationSettingsPersistence.SaveConfigurationDirtyFlag(value);
		}
	}

	public ConfigurationRepository(IEntityPersistence entityPersistence, IConfigurationSettingsPersistence configVersionPersistence)
	{
		this.entityPersistence = entityPersistence;
		configurationSettingsPersistence = configVersionPersistence;
		LoadAllEntities();
	}

	public Location GetLocation(Guid id)
	{
		return GetEntity(locations, id);
	}

	public LogicalDevice GetLogicalDevice(Guid id)
	{
		return GetEntity(logicalDevices, id);
	}

	public BaseDevice GetBaseDevice(Guid id)
	{
		return GetEntity(baseDevices, id);
	}

	public Interaction GetInteraction(Guid id)
	{
		return GetEntity(interactions, id);
	}

	public void LoadAllEntities()
	{
		locations.Clear();
		baseDevices.Clear();
		logicalDevices.Clear();
		deletionHistory.Clear();
		interactions.Clear();
		homeSetups.Clear();
		LoadDictionary(locations);
		LoadDictionary(baseDevices);
		LoadDictionary(logicalDevices);
		LoadDictionary(interactions);
		LoadDictionary(homeSetups);
		repositoryVersion = configurationSettingsPersistence.LoadConfigurationVersion();
	}

	public void DeleteAllEntities()
	{
		DeleteEntities(interactions);
		DeleteEntities(logicalDevices);
		DeleteEntities(baseDevices);
		DeleteEntities(locations);
		DeleteEntities(homeSetups);
	}

	public void SetLocation(Location location)
	{
		SetEntity(locations, location);
	}

	public void SetLogicalDevice(LogicalDevice logicalDevice)
	{
		SetEntity(logicalDevices, logicalDevice);
	}

	public void SetBaseDevice(BaseDevice baseDevice)
	{
		SetEntity(baseDevices, baseDevice);
	}

	public void SetInteraction(Interaction interaction)
	{
		SetEntity(interactions, interaction);
	}

	public void DeleteSensor(Guid id)
	{
		DeleteEntity(logicalDevices, id);
	}

	public void DeleteLocation(Guid id)
	{
		DeleteEntity(locations, id);
	}

	public void DeleteLogicalDevice(Guid id)
	{
		DeleteEntity(logicalDevices, id);
	}

	public void DeleteBaseDevice(Guid id)
	{
		DeleteEntity(baseDevices, id);
	}

	public void DeleteInteraction(Guid id)
	{
		DeleteEntity(interactions, id);
	}

	public List<BaseDevice> GetBaseDevices()
	{
		return GetEntities(baseDevices);
	}

	public List<LogicalDevice> GetLogicalDevices()
	{
		return GetEntities(logicalDevices);
	}

	public List<Location> GetLocations()
	{
		return GetEntities(locations);
	}

	public List<Interaction> GetInteractions()
	{
		return GetEntities(interactions);
	}

	public List<BaseDevice> GetOriginalBaseDevices()
	{
		return GetOriginalEntities(baseDevices);
	}

	public List<LogicalDevice> GetOriginalLogicalDevices()
	{
		return GetOriginalEntities(logicalDevices);
	}

	public List<Location> GetOriginalLocations()
	{
		return GetOriginalEntities(locations);
	}

	public List<Interaction> GetOriginalInteractions()
	{
		return GetOriginalEntities(interactions);
	}

	public List<BaseDevice> GetOriginalBaseDevices(int sinceVersion)
	{
		return GetOriginalEntities(baseDevices, sinceVersion);
	}

	public List<LogicalDevice> GetOriginalLogicalDevices(int sinceVersion)
	{
		return GetOriginalEntities(logicalDevices, sinceVersion);
	}

	public List<Location> GetOriginalLocations(int sinceVersion)
	{
		return GetOriginalEntities(locations, sinceVersion);
	}

	public List<Interaction> GetOriginalInteractions(int sinceVersion)
	{
		return GetOriginalEntities(interactions, sinceVersion);
	}

	public List<BaseDevice> GetModifiedBaseDevices()
	{
		return GetModifiedEntities(baseDevices);
	}

	public List<BaseDevice> GetDeletedBaseDevices()
	{
		return GetDeletedEntities(baseDevices);
	}

	public List<LogicalDevice> GetModifiedLogicalDevices()
	{
		return GetModifiedEntities(logicalDevices);
	}

	public List<Location> GetModifiedLocations()
	{
		return GetModifiedEntities(locations);
	}

	public List<Interaction> GetModifiedInteractions()
	{
		return GetModifiedEntities(interactions);
	}

	public List<HomeSetup> GetModifiedHomeSetups()
	{
		return GetModifiedEntities(homeSetups);
	}

	public List<BaseDevice> GetAffectedBaseDevices(ProtocolIdentifier protocolId)
	{
		return GetAffectedBaseDevices((BaseDevice bd) => bd.ProtocolId == protocolId);
	}

	public List<BaseDevice> GetAffectedBaseDevices(string appId)
	{
		return GetAffectedBaseDevices((BaseDevice bd) => bd.AppId == appId);
	}

	public Location GetOriginalLocation(Guid id)
	{
		return GetOriginalEntity(locations, id);
	}

	public BaseDevice GetOriginalBaseDevice(Guid id)
	{
		return GetOriginalEntity(baseDevices, id);
	}

	public LogicalDevice GetOriginalLogicalDevice(Guid id)
	{
		return GetOriginalEntity(logicalDevices, id);
	}

	public Interaction GetOriginalInteraction(Guid id)
	{
		return GetOriginalEntity(interactions, id);
	}

	public ConfigurationUpdateReport GetUpdateReport()
	{
		List<RepositoryEntry<Location>> newEntitiesE = GetNewEntitiesE(locations);
		List<RepositoryEntry<Location>> modifiedEntitiesE = GetModifiedEntitiesE(locations);
		List<RepositoryEntry<Location>> deletedEntitiesE = GetDeletedEntitiesE(locations);
		List<RepositoryEntry<BaseDevice>> newEntitiesE2 = GetNewEntitiesE(baseDevices);
		List<RepositoryEntry<BaseDevice>> modifiedEntitiesE2 = GetModifiedEntitiesE(baseDevices);
		List<RepositoryEntry<BaseDevice>> deletedEntitiesE2 = GetDeletedEntitiesE(baseDevices);
		List<RepositoryEntry<LogicalDevice>> newEntitiesE3 = GetNewEntitiesE(logicalDevices);
		List<RepositoryEntry<LogicalDevice>> modifiedEntitiesE3 = GetModifiedEntitiesE(logicalDevices);
		List<RepositoryEntry<LogicalDevice>> deletedEntitiesE3 = GetDeletedEntitiesE(logicalDevices);
		List<RepositoryEntry<Interaction>> newEntitiesE4 = GetNewEntitiesE(interactions);
		List<RepositoryEntry<Interaction>> modifiedEntitiesE4 = GetModifiedEntitiesE(interactions);
		List<RepositoryEntry<Interaction>> deletedEntitiesE4 = GetDeletedEntitiesE(interactions);
		List<RepositoryEntry<HomeSetup>> newEntitiesE5 = GetNewEntitiesE(homeSetups);
		List<RepositoryEntry<HomeSetup>> modifiedEntitiesE5 = GetModifiedEntitiesE(homeSetups);
		List<RepositoryEntry<HomeSetup>> deletedEntitiesE5 = GetDeletedEntitiesE(homeSetups);
		List<EntityMetadata> list = new List<EntityMetadata>();
		list.AddRange(newEntitiesE.Select((RepositoryEntry<Location> x) => new EntityMetadata
		{
			EntityType = EntityType.Location,
			Id = x.ActualValue.Id
		}));
		list.AddRange(newEntitiesE2.Select((RepositoryEntry<BaseDevice> x) => new EntityMetadata
		{
			EntityType = EntityType.BaseDevice,
			Id = x.ActualValue.Id
		}));
		list.AddRange(newEntitiesE3.Select((RepositoryEntry<LogicalDevice> x) => new EntityMetadata
		{
			EntityType = EntityType.LogicalDevice,
			Id = x.ActualValue.Id
		}));
		list.AddRange(newEntitiesE4.Select((RepositoryEntry<Interaction> x) => new EntityMetadata
		{
			EntityType = EntityType.Interaction,
			Id = x.ActualValue.Id
		}));
		list.AddRange(newEntitiesE5.Select((RepositoryEntry<HomeSetup> x) => new EntityMetadata
		{
			EntityType = EntityType.HomeSetup,
			Id = x.ActualValue.Id
		}));
		List<Location> newLocations = newEntitiesE.Select((RepositoryEntry<Location> x) => x.ActualValue).ToList();
		List<BaseDevice> newBaseDevices = newEntitiesE2.Select((RepositoryEntry<BaseDevice> x) => x.ActualValue).ToList();
		List<LogicalDevice> newLogicalDevices = newEntitiesE3.Select((RepositoryEntry<LogicalDevice> x) => x.ActualValue).ToList();
		List<Interaction> newInteractions = newEntitiesE4.Select((RepositoryEntry<Interaction> x) => x.ActualValue).ToList();
		List<HomeSetup> newHomeSetups = newEntitiesE5.Select((RepositoryEntry<HomeSetup> x) => x.ActualValue).ToList();
		List<EntityMetadata> list2 = new List<EntityMetadata>();
		list2.AddRange(modifiedEntitiesE.Select((RepositoryEntry<Location> x) => new EntityMetadata
		{
			EntityType = EntityType.Location,
			Id = x.ActualValue.Id
		}));
		list2.AddRange(modifiedEntitiesE2.Select((RepositoryEntry<BaseDevice> x) => new EntityMetadata
		{
			EntityType = EntityType.BaseDevice,
			Id = x.ActualValue.Id
		}));
		list2.AddRange(modifiedEntitiesE3.Select((RepositoryEntry<LogicalDevice> x) => new EntityMetadata
		{
			EntityType = EntityType.LogicalDevice,
			Id = x.ActualValue.Id
		}));
		list2.AddRange(modifiedEntitiesE4.Select((RepositoryEntry<Interaction> x) => new EntityMetadata
		{
			EntityType = EntityType.Interaction,
			Id = x.ActualValue.Id
		}));
		list2.AddRange(modifiedEntitiesE5.Select((RepositoryEntry<HomeSetup> x) => new EntityMetadata
		{
			EntityType = EntityType.HomeSetup,
			Id = x.ActualValue.Id
		}));
		List<Location> modifiedLocations = modifiedEntitiesE.Select((RepositoryEntry<Location> x) => x.ActualValue).ToList();
		List<BaseDevice> modifiedBaseDevices = modifiedEntitiesE2.Select((RepositoryEntry<BaseDevice> x) => x.ActualValue).ToList();
		List<LogicalDevice> modifiedLogicalDevices = modifiedEntitiesE3.Select((RepositoryEntry<LogicalDevice> x) => x.ActualValue).ToList();
		List<Interaction> modifiedInteractions = modifiedEntitiesE4.Select((RepositoryEntry<Interaction> x) => x.ActualValue).ToList();
		List<HomeSetup> modifiedHomeSetups = modifiedEntitiesE5.Select((RepositoryEntry<HomeSetup> x) => x.ActualValue).ToList();
		List<EntityMetadata> list3 = new List<EntityMetadata>();
		list3.AddRange(deletedEntitiesE.Select((RepositoryEntry<Location> x) => new EntityMetadata
		{
			EntityType = EntityType.Location,
			Id = x.OriginalValue.Id
		}));
		list3.AddRange(deletedEntitiesE2.Select((RepositoryEntry<BaseDevice> x) => new EntityMetadata
		{
			EntityType = EntityType.BaseDevice,
			Id = x.OriginalValue.Id
		}));
		list3.AddRange(deletedEntitiesE3.Select((RepositoryEntry<LogicalDevice> x) => new EntityMetadata
		{
			EntityType = EntityType.LogicalDevice,
			Id = x.OriginalValue.Id
		}));
		list3.AddRange(deletedEntitiesE4.Select((RepositoryEntry<Interaction> x) => new EntityMetadata
		{
			EntityType = EntityType.Interaction,
			Id = x.OriginalValue.Id
		}));
		list3.AddRange(deletedEntitiesE5.Select((RepositoryEntry<HomeSetup> x) => new EntityMetadata
		{
			EntityType = EntityType.HomeSetup,
			Id = x.OriginalValue.Id
		}));
		List<Location> deletedLocations = deletedEntitiesE.Select((RepositoryEntry<Location> x) => x.OriginalValue).ToList();
		List<BaseDevice> deletedBaseDevices = deletedEntitiesE2.Select((RepositoryEntry<BaseDevice> x) => x.OriginalValue).ToList();
		List<LogicalDevice> deletedLogicalDevices = deletedEntitiesE3.Select((RepositoryEntry<LogicalDevice> x) => x.OriginalValue).ToList();
		List<Interaction> deletedInteractions = deletedEntitiesE4.Select((RepositoryEntry<Interaction> x) => x.OriginalValue).ToList();
		List<HomeSetup> deletedHomeSetups = deletedEntitiesE5.Select((RepositoryEntry<HomeSetup> x) => x.OriginalValue).ToList();
		return new ConfigurationUpdateReport(list, list2, list3, newLocations, modifiedLocations, deletedLocations, newBaseDevices, modifiedBaseDevices, deletedBaseDevices, newLogicalDevices, modifiedLogicalDevices, deletedLogicalDevices, newInteractions, modifiedInteractions, deletedInteractions, newHomeSetups, modifiedHomeSetups, deletedHomeSetups);
	}

	public List<EntityMetadata> GenerateRepositoryCommittedDeletionReport(int sinceVersion)
	{
		return (from p in deletionHistory
			where p.Key > sinceVersion
			select p.Value).ToList();
	}

	public void Commit(CommitType commitType)
	{
		entityPersistence.Begin();
		int num = 0;
		try
		{
			num += PersistEntityChanges(locations);
			foreach (RepositoryEntry<BaseDevice> value in baseDevices.Values)
			{
				if (value.ActualValue != null)
				{
					value.ActualValue.VolatilePropertiesSpecified = false;
				}
				if (value.OriginalValue != null)
				{
					value.OriginalValue.VolatilePropertiesSpecified = false;
				}
			}
			num += PersistEntityChanges(baseDevices);
			num += PersistEntityChanges(logicalDevices);
			num += PersistEntityChanges(interactions);
			num += PersistEntityChanges(homeSetups);
			num += PersistEntityDeletions(logicalDevices);
			num += PersistEntityDeletions(baseDevices);
			num += PersistEntityDeletions(locations);
			num += PersistEntityDeletions(interactions);
			num += PersistEntityDeletions(homeSetups);
			entityPersistence.Commit();
		}
		catch (Exception)
		{
			entityPersistence.Rollback();
			throw;
		}
		CommitLocalCache(locations, EntityType.Location);
		CommitLocalCache(baseDevices, EntityType.BaseDevice);
		CommitLocalCache(logicalDevices, EntityType.LogicalDevice);
		CommitLocalCache(interactions, EntityType.Interaction);
		CommitLocalCache(homeSetups, EntityType.HomeSetup);
		if (num > 0 && commitType == CommitType.DirtyRepository)
		{
			long num2 = repositoryVersion + 1;
			repositoryVersion = (int)(num2 % int.MaxValue);
		}
		configurationSettingsPersistence.SaveConfigurationVersion(repositoryVersion);
	}

	public void Rollback()
	{
		RollbackLocalCache(locations);
		RollbackLocalCache(baseDevices);
		RollbackLocalCache(logicalDevices);
		RollbackLocalCache(interactions);
		RollbackLocalCache(homeSetups);
	}

	public void PurgeChangeHistory()
	{
		Rollback();
		ResetAllEntityVersions(locations);
		ResetAllEntityVersions(baseDevices);
		ResetAllEntityVersions(logicalDevices);
		ResetAllEntityVersions(interactions);
		ResetAllEntityVersions(homeSetups);
		deletionHistory.Clear();
	}

	public bool IsCalibrationPending()
	{
		foreach (RollerShutterActuator item in GetLogicalDevices().OfType<RollerShutterActuator>())
		{
			if (item.IsCalibrating)
			{
				return true;
			}
		}
		return false;
	}

	private static void ResetAllEntityVersions<T>(IDictionary<Guid, RepositoryEntry<T>> entityCollection) where T : Entity
	{
		lock (entityCollection)
		{
			foreach (KeyValuePair<Guid, RepositoryEntry<T>> item in entityCollection)
			{
				T originalValue = item.Value.OriginalValue;
				originalValue.Version = 0;
			}
		}
	}

	private T GetEntity<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, Guid id) where T : Entity
	{
		lock (entityDictionary)
		{
			T result = null;
			if (entityDictionary.ContainsKey(id))
			{
				RepositoryEntry<T> repositoryEntry = entityDictionary[id];
				switch (repositoryEntry.State)
				{
				case EntityRepositoryState.Unchanged:
					return repositoryEntry.OriginalValue;
				case EntityRepositoryState.New:
				case EntityRepositoryState.Changed:
					return repositoryEntry.ActualValue;
				default:
					throw new ArgumentOutOfRangeException();
				case EntityRepositoryState.Deleted:
					break;
				}
			}
			return result;
		}
	}

	private T GetOriginalEntity<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, Guid id) where T : Entity
	{
		lock (entityDictionary)
		{
			T result = null;
			if (entityDictionary.ContainsKey(id))
			{
				return entityDictionary[id].OriginalValue;
			}
			return result;
		}
	}

	private void SetEntity<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, T entity) where T : Entity
	{
		lock (entityDictionary)
		{
			if (entityDictionary.TryGetValue(entity.Id, out var value))
			{
				if (value.State != EntityRepositoryState.New)
				{
					value.State = EntityRepositoryState.Changed;
				}
				value.ActualValue = entity;
			}
			else
			{
				value = new RepositoryEntry<T>(entity, EntityRepositoryState.New);
				entityDictionary[entity.Id] = value;
			}
			T actualValue = value.ActualValue;
			actualValue.Version = repositoryVersion + 1;
		}
	}

	private void DeleteEntity<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, Guid id) where T : Entity
	{
		lock (entityDictionary)
		{
			if (entityDictionary.TryGetValue(id, out var value))
			{
				if (value.State == EntityRepositoryState.New)
				{
					entityDictionary.Remove(id);
				}
				else
				{
					value.State = EntityRepositoryState.Deleted;
				}
			}
		}
	}

	private void DeleteEntities<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		lock (entityDictionary)
		{
			List<Guid> list = new List<Guid>(entityDictionary.Keys);
			foreach (Guid item in list)
			{
				DeleteEntity(entityDictionary, item);
			}
		}
	}

	private static List<T> GetEntities<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		lock (entityDictionary)
		{
			List<T> list = new List<T>(entityDictionary.Count);
			foreach (RepositoryEntry<T> value in entityDictionary.Values)
			{
				switch (value.State)
				{
				case EntityRepositoryState.Unchanged:
					list.Add(value.OriginalValue);
					break;
				case EntityRepositoryState.New:
				case EntityRepositoryState.Changed:
					list.Add(value.ActualValue);
					break;
				}
			}
			return list;
		}
	}

	private static List<T> GetOriginalEntities<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		lock (entityDictionary)
		{
			return (from entity in entityDictionary.Values
				where entity.State != EntityRepositoryState.New
				select entity.OriginalValue).ToList();
		}
	}

	private static List<T> GetOriginalEntities<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, int sinceVersion) where T : Entity
	{
		lock (entityDictionary)
		{
			return (from entity in entityDictionary.Values.Where(delegate(RepositoryEntry<T> entity)
				{
					if (entity.State != EntityRepositoryState.New)
					{
						T originalValue = entity.OriginalValue;
						return originalValue.Version > sinceVersion;
					}
					return false;
				})
				select entity.OriginalValue).ToList();
		}
	}

	private static List<RepositoryEntry<T>> GetNewEntitiesE<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		return entityDictionary.Values.Where((RepositoryEntry<T> entity) => entity.State == EntityRepositoryState.New).ToList();
	}

	private static List<RepositoryEntry<T>> GetModifiedEntitiesE<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		return entityDictionary.Values.Where((RepositoryEntry<T> entity) => entity.State == EntityRepositoryState.New || entity.State == EntityRepositoryState.Changed).ToList();
	}

	private static List<RepositoryEntry<T>> GetDeletedEntitiesE<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		return entityDictionary.Values.Where((RepositoryEntry<T> entity) => entity.State == EntityRepositoryState.Deleted).ToList();
	}

	private static List<T> GetModifiedEntities<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		lock (entityDictionary)
		{
			return (from entity in entityDictionary.Values
				where entity.State == EntityRepositoryState.New || entity.State == EntityRepositoryState.Changed
				select entity.ActualValue).ToList();
		}
	}

	private static List<T> GetDeletedEntities<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		lock (entityDictionary)
		{
			return (from entity in entityDictionary.Values
				where entity.State == EntityRepositoryState.Deleted
				select entity.OriginalValue).ToList();
		}
	}

	private static IEnumerable<EntityMetadata> GenerateModificationReport<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, EntityType type) where T : Entity
	{
		lock (entityDictionary)
		{
			return from entity in entityDictionary
				where entity.Value.State != EntityRepositoryState.Unchanged && entity.Value.State != EntityRepositoryState.Deleted
				select new EntityMetadata
				{
					Id = entity.Key,
					EntityType = type
				};
		}
	}

	private static IEnumerable<EntityMetadata> GenerateDeletionReport<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, EntityType type) where T : Entity
	{
		lock (entityDictionary)
		{
			return from entity in entityDictionary
				where entity.Value.State == EntityRepositoryState.Deleted
				select new EntityMetadata
				{
					Id = entity.Key,
					EntityType = type
				};
		}
	}

	private static IEnumerable<EntityMetadata> GenerateInclusionReport<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary, EntityType type) where T : Entity
	{
		lock (entityDictionary)
		{
			return from entity in entityDictionary
				where entity.Value.State == EntityRepositoryState.New
				select new EntityMetadata
				{
					Id = entity.Key,
					EntityType = type
				};
		}
	}

	private void CommitLocalCache<T>(IDictionary<Guid, RepositoryEntry<T>> entityRepository, EntityType entityType) where T : Entity
	{
		lock (entityRepository)
		{
			List<Guid> list = new List<Guid>();
			foreach (KeyValuePair<Guid, RepositoryEntry<T>> item in entityRepository)
			{
				switch (item.Value.State)
				{
				case EntityRepositoryState.New:
				case EntityRepositoryState.Changed:
					item.Value.OriginalValue = item.Value.ActualValue;
					item.Value.ActualValue = null;
					item.Value.State = EntityRepositoryState.Unchanged;
					break;
				case EntityRepositoryState.Deleted:
				{
					EntityMetadata entityMetadata = new EntityMetadata();
					entityMetadata.EntityType = entityType;
					entityMetadata.Id = item.Key;
					EntityMetadata value = entityMetadata;
					deletionHistory.Add(new KeyValuePair<int, EntityMetadata>(RepositoryVersion + 1, value));
					list.Add(item.Key);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				case EntityRepositoryState.Unchanged:
					break;
				}
			}
			list.ForEach(delegate(Guid e)
			{
				entityRepository.Remove(e);
			});
		}
	}

	private static void RollbackLocalCache<T>(IDictionary<Guid, RepositoryEntry<T>> entityRepository) where T : Entity
	{
		lock (entityRepository)
		{
			List<Guid> list = new List<Guid>();
			foreach (KeyValuePair<Guid, RepositoryEntry<T>> item in entityRepository)
			{
				switch (item.Value.State)
				{
				case EntityRepositoryState.New:
					list.Add(item.Key);
					break;
				case EntityRepositoryState.Changed:
				case EntityRepositoryState.Deleted:
					item.Value.ActualValue = null;
					item.Value.State = EntityRepositoryState.Unchanged;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				case EntityRepositoryState.Unchanged:
					break;
				}
			}
			list.ForEach(delegate(Guid e)
			{
				entityRepository.Remove(e);
			});
		}
	}

	private void LoadDictionary<T>(IDictionary<Guid, RepositoryEntry<T>> entityDictionary) where T : Entity
	{
		IEnumerable<T> enumerable = entityPersistence.Load<T>();
		lock (entityDictionary)
		{
			foreach (T item in enumerable)
			{
				T current = item;
				entityDictionary[current.Id] = new RepositoryEntry<T>(current, EntityRepositoryState.Unchanged);
			}
		}
	}

	private int PersistEntityDeletions<T>(IDictionary<Guid, RepositoryEntry<T>> entityRepository) where T : Entity
	{
		IEnumerable<T> enumerable;
		lock (entityRepository)
		{
			enumerable = (from e in entityRepository.Values
				where e.State == EntityRepositoryState.Deleted
				select e.OriginalValue).ToList();
		}
		entityPersistence.Delete(enumerable);
		return enumerable.Count();
	}

	private int PersistEntityChanges<T>(IDictionary<Guid, RepositoryEntry<T>> entityRepository) where T : Entity
	{
		IEnumerable<T> enumerable;
		lock (entityRepository)
		{
			enumerable = (from e in entityRepository.Values
				where e.State == EntityRepositoryState.New || e.State == EntityRepositoryState.Changed
				select e.ActualValue).ToList();
		}
		entityPersistence.Save(enumerable);
		return enumerable.Count();
	}

	private List<BaseDevice> GetAffectedBaseDevices(Func<BaseDevice, bool> filter)
	{
		List<BaseDevice> result = new List<BaseDevice>();
		result.AddRange(from bd in GetModifiedBaseDevices()
			where bd != null && filter(bd)
			select bd);
		result.AddRange(from ld in GetModifiedLogicalDevices()
			select ld.BaseDevice into bd
			where bd != null && filter(bd)
			select bd);
		IEnumerable<Interaction> source = GetModifiedInteractions().Union(GetDeletedInteractions());
		foreach (Trigger item in source.SelectMany((Interaction i) => i.Rules).SelectMany((Rule r) => r.Triggers))
		{
			LogicalDevice logicalDevice = GetLogicalDevice(item.Entity.EntityIdAsGuid());
			if (logicalDevice != null && logicalDevice.BaseDevice != null && filter(logicalDevice.BaseDevice))
			{
				result.Add(logicalDevice.BaseDevice);
			}
			item.TriggerConditions.ForEach(delegate(Condition c)
			{
				result.AddRange(c.GetReferencedDevices(this));
			});
		}
		foreach (ActionDescription item2 in source.SelectMany((Interaction i) => i.Rules).SelectMany((Rule r) => r.Actions))
		{
			BaseDevice baseDevice = null;
			if (item2.Target.LinkType == EntityType.LogicalDevice)
			{
				LogicalDevice logicalDevice2 = GetLogicalDevice(item2.Target.EntityIdAsGuid());
				if (logicalDevice2 != null)
				{
					baseDevice = logicalDevice2.BaseDevice;
				}
			}
			else if (item2.Target.LinkType == EntityType.BaseDevice)
			{
				baseDevice = GetBaseDevice(item2.Target.EntityIdAsGuid());
			}
			if (baseDevice != null && filter(baseDevice))
			{
				result.Add(baseDevice);
			}
			result.AddRange(item2.GetReferencedDevices(this));
		}
		return result;
	}

	private List<Interaction> GetDeletedInteractions()
	{
		lock (interactions)
		{
			return (from entity in interactions.Values
				where entity.State == EntityRepositoryState.Deleted
				select entity.OriginalValue).ToList();
		}
	}

	public List<Interaction> GetMarkedForDeleteInteractions()
	{
		return GetDeletedInteractions();
	}

	public HomeSetup GetHomeSetup(Guid id)
	{
		return GetEntity(homeSetups, id);
	}

	public void SetHomeSetup(HomeSetup homeSetup)
	{
		SetEntity(homeSetups, homeSetup);
	}

	public void DeleteHomeSetup(Guid id)
	{
		DeleteEntity(homeSetups, id);
	}

	public List<HomeSetup> GetHomeSetups()
	{
		return GetEntities(homeSetups);
	}

	public List<HomeSetup> GetOriginalHomeSetups()
	{
		return GetOriginalEntities(homeSetups);
	}

	public List<HomeSetup> GetOriginalHomeSetups(int sinceVersion)
	{
		return GetOriginalEntities(homeSetups, sinceVersion);
	}

	public HomeSetup GetOriginalHomeSetup(Guid id)
	{
		return GetOriginalEntity(homeSetups, id);
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
