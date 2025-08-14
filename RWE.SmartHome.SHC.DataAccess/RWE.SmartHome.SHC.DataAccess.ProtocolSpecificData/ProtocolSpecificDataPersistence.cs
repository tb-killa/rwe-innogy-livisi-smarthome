using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.Events;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;

namespace RWE.SmartHome.SHC.DataAccess.ProtocolSpecificData;

public class ProtocolSpecificDataPersistence : IProtocolSpecificDataPersistence, IService
{
	private const string TABLE = "ProtocolSpecificEntities";

	private const string PRIMARY_KEY = "ProtocolSpecificEntities_PKey";

	private const string DATA_ID_COLUMN = "DataId";

	private const string SUB_ID_COLUMN = "SubId";

	private const string PROTOCOL_ID_COLUMN = "ProtocolId";

	private const string DATA_COLUMN = "Data";

	private readonly DatabaseConnectionsPool persistence;

	private readonly IEventManager eventManager;

	public ProtocolSpecificDataPersistence(DatabaseConnectionsPool persistence, IEventManager eventManager)
	{
		this.eventManager = eventManager;
		this.persistence = persistence;
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.ProtocolSpecificEntities);
		list.Add(Resources.ProtocolSpecificEntities_PKey);
		List<string> commandTexts = list;
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		databaseConnection.ExecuteNonQueries(commandTexts);
	}

	public void Uninitialize()
	{
	}

	public void SaveInTransaction(ProtocolIdentifier protocolId, string dataId, string subId, string data, bool suppressEvent)
	{
		DoInTransaction(delegate
		{
			Save(protocolId, dataId, subId, data, suppressEvent);
		});
	}

	public void Save(ProtocolIdentifier protocolId, string dataId, string subId, string data, bool suppressEvent)
	{
		//IL_0123: Expected O, but got Unknown
		ProtocolSpecificDataModifiedEventArgs e = new ProtocolSpecificDataModifiedEventArgs();
		e.ProtocolId = protocolId;
		e.DataId = dataId;
		e.SubId = subId;
		ProtocolSpecificDataModifiedEventArgs e2 = e;
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ProtocolSpecificEntities");
			databaseCommand.CommandType = CommandType.TableDirect;
			databaseCommand.IndexName = "ProtocolSpecificEntities_PKey";
			databaseCommand.SetRange((DbRangeOptions)16, new object[3] { protocolId, dataId, subId }, null);
			int num = 0;
			while (num++ < 5)
			{
				SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
				try
				{
					if (((DbDataReader)(object)val).Read())
					{
						val.SetString(((DbDataReader)(object)val).GetOrdinal("Data"), data);
						val.Update();
						e2.Modification = ModificationType.Modify;
						break;
					}
					SqlCeUpdatableRecord val2 = val.CreateRecord();
					val2.SetInt16(((DbDataReader)(object)val).GetOrdinal("ProtocolId"), (short)protocolId);
					val2.SetString(((DbDataReader)(object)val).GetOrdinal("DataId"), dataId);
					val2.SetString(((DbDataReader)(object)val).GetOrdinal("SubId"), subId);
					val2.SetString(((DbDataReader)(object)val).GetOrdinal("Data"), data);
					val.Insert(val2);
					e2.Modification = ModificationType.Add;
				}
				catch (SqlCeException ex)
				{
					SqlCeException ex2 = ex;
					if (-2147217873 == ex2.HResult)
					{
						continue;
					}
					throw;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				break;
			}
		}
		PublishProtocolSpecificDataModifiedEvent(suppressEvent, e2);
	}

	public string Load(ProtocolIdentifier protocolId, string dataId, string subId)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ProtocolSpecificEntities");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "ProtocolSpecificEntities_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[3] { protocolId, dataId, subId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			return ((DbDataReader)(object)val).Read() ? ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Data")) : null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<ProtocolSpecificDataEntity> LoadAll(ProtocolIdentifier protocolId)
	{
		List<ProtocolSpecificDataEntity> list = new List<ProtocolSpecificDataEntity>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ProtocolSpecificEntities");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "ProtocolSpecificEntities_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { protocolId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			while (((DbDataReader)(object)val).Read())
			{
				ProtocolSpecificDataEntity protocolSpecificDataEntityFromResultSet = GetProtocolSpecificDataEntityFromResultSet(val);
				list.Add(protocolSpecificDataEntityFromResultSet);
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<ProtocolSpecificDataEntity> LoadAll(ProtocolIdentifier protocolId, string dataId)
	{
		List<ProtocolSpecificDataEntity> list = new List<ProtocolSpecificDataEntity>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ProtocolSpecificEntities");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "ProtocolSpecificEntities_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[2] { protocolId, dataId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			while (((DbDataReader)(object)val).Read())
			{
				ProtocolSpecificDataEntity protocolSpecificDataEntityFromResultSet = GetProtocolSpecificDataEntityFromResultSet(val);
				list.Add(protocolSpecificDataEntityFromResultSet);
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<ProtocolSpecificDataEntity> LoadAll()
	{
		List<ProtocolSpecificDataEntity> list = new List<ProtocolSpecificDataEntity>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ProtocolSpecificEntities");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "ProtocolSpecificEntities_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			while (((DbDataReader)(object)val).Read())
			{
				ProtocolSpecificDataEntity protocolSpecificDataEntityFromResultSet = GetProtocolSpecificDataEntityFromResultSet(val);
				list.Add(protocolSpecificDataEntityFromResultSet);
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void Delete(ProtocolIdentifier protocolId, string dataId, string subId, bool suppressEvent)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ProtocolSpecificEntities");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "ProtocolSpecificEntities_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[3] { protocolId, dataId, subId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				val.Delete();
				PublishProtocolSpecificDataModifiedEvent(suppressEvent, new ProtocolSpecificDataModifiedEventArgs
				{
					Modification = ModificationType.Delete
				});
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void DeleteInTransaction(ProtocolIdentifier protocolId, string dataId, string subId, bool suppressEvent)
	{
		DoInTransaction(delegate
		{
			Delete(protocolId, dataId, subId, suppressEvent);
		});
	}

	private ProtocolSpecificDataEntity GetProtocolSpecificDataEntityFromResultSet(SqlCeResultSet resultSet)
	{
		ProtocolSpecificDataEntity protocolSpecificDataEntity = new ProtocolSpecificDataEntity();
		protocolSpecificDataEntity.DataId = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("DataId"));
		protocolSpecificDataEntity.ProtocolId = (ProtocolIdentifier)((DbDataReader)(object)resultSet).GetInt16(((DbDataReader)(object)resultSet).GetOrdinal("ProtocolId"));
		protocolSpecificDataEntity.SubId = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("SubId"));
		protocolSpecificDataEntity.Data = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("Data"));
		return protocolSpecificDataEntity;
	}

	public DatabaseConnection OpenDatabaseTransaction()
	{
		DatabaseConnection connection = persistence.GetConnection();
		connection.BeginTransaction();
		return connection;
	}

	public void SaveAll(IEnumerable<ProtocolSpecificDataEntity> entities, bool suppressEvent)
	{
		DoInTransaction(delegate
		{
			foreach (ProtocolSpecificDataEntity entity in entities)
			{
				Save(entity.ProtocolId, entity.DataId, entity.SubId, entity.Data, suppressEvent);
			}
		});
	}

	private void PublishProtocolSpecificDataModifiedEvent(bool suppressEvent, ProtocolSpecificDataModifiedEventArgs modifiedEventArgs)
	{
		if (!suppressEvent)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				Thread.Sleep(10);
				eventManager.GetEvent<ProtocolSpecificDataModifiedEvent>().Publish(modifiedEventArgs);
			});
		}
	}

	private void DoInTransaction(Action action)
	{
		using DatabaseConnection databaseConnection = OpenDatabaseTransaction();
		try
		{
			action();
			databaseConnection.CommitTransaction();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DataAccess, string.Concat("Error accessing the DB: ", ex, " stack: ", ex.StackTrace));
			databaseConnection.RollbackTransaction();
			throw;
		}
	}
}
