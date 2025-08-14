using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API.Interfaces;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public interface IRepository : IEntityCache
{
	int RepositoryVersion { get; set; }

	bool IsDirty { get; set; }

	void LoadAllEntities();

	void DeleteAllEntities();

	void SetLocation(Location location);

	void SetLogicalDevice(LogicalDevice logicalDevice);

	void SetBaseDevice(BaseDevice baseDevice);

	void SetInteraction(Interaction interaction);

	void SetHomeSetup(HomeSetup homeSetup);

	void DeleteLocation(Guid id);

	void DeleteLogicalDevice(Guid id);

	void DeleteBaseDevice(Guid id);

	void DeleteInteraction(Guid id);

	void DeleteHomeSetup(Guid id);

	List<BaseDevice> GetBaseDevices();

	List<LogicalDevice> GetLogicalDevices();

	List<Location> GetLocations();

	List<Interaction> GetInteractions();

	List<HomeSetup> GetHomeSetups();

	List<BaseDevice> GetOriginalBaseDevices();

	List<LogicalDevice> GetOriginalLogicalDevices();

	List<Location> GetOriginalLocations();

	List<Interaction> GetOriginalInteractions();

	List<HomeSetup> GetOriginalHomeSetups();

	List<BaseDevice> GetOriginalBaseDevices(int sinceVersion);

	List<LogicalDevice> GetOriginalLogicalDevices(int sinceVersion);

	List<Location> GetOriginalLocations(int sinceVersion);

	List<Interaction> GetOriginalInteractions(int sinceVersion);

	List<HomeSetup> GetOriginalHomeSetups(int sinceVersion);

	List<BaseDevice> GetModifiedBaseDevices();

	List<BaseDevice> GetDeletedBaseDevices();

	List<LogicalDevice> GetModifiedLogicalDevices();

	List<Location> GetModifiedLocations();

	List<Interaction> GetModifiedInteractions();

	List<HomeSetup> GetModifiedHomeSetups();

	List<BaseDevice> GetAffectedBaseDevices(ProtocolIdentifier protocolId);

	List<BaseDevice> GetAffectedBaseDevices(string appId);

	Location GetOriginalLocation(Guid id);

	BaseDevice GetOriginalBaseDevice(Guid id);

	LogicalDevice GetOriginalLogicalDevice(Guid id);

	Interaction GetOriginalInteraction(Guid id);

	HomeSetup GetOriginalHomeSetup(Guid id);

	List<Interaction> GetMarkedForDeleteInteractions();

	ConfigurationUpdateReport GetUpdateReport();

	List<EntityMetadata> GenerateRepositoryCommittedDeletionReport(int sinceVersion);

	void Commit(CommitType commitType);

	void Rollback();

	void PurgeChangeHistory();

	bool IsCalibrationPending();
}
