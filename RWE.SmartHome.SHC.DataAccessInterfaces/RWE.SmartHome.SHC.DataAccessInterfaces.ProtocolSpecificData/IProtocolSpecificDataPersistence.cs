using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;

public interface IProtocolSpecificDataPersistence : IService
{
	void SaveInTransaction(ProtocolIdentifier protocolId, string dataId, string subId, string data, bool suppressEvent);

	void Save(ProtocolIdentifier protocolId, string dataId, string subId, string data, bool suppressEvent);

	string Load(ProtocolIdentifier protocolId, string dataId, string subId);

	List<ProtocolSpecificDataEntity> LoadAll(ProtocolIdentifier protocolId, string dataId);

	List<ProtocolSpecificDataEntity> LoadAll(ProtocolIdentifier protocolId);

	List<ProtocolSpecificDataEntity> LoadAll();

	void DeleteInTransaction(ProtocolIdentifier protocolId, string dataId, string subId, bool suppressEvent);

	DatabaseConnection OpenDatabaseTransaction();

	void SaveAll(IEnumerable<ProtocolSpecificDataEntity> entities, bool suppressEvent);
}
