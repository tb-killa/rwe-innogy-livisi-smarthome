using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;

namespace RWE.SmartHome.SHC.DataAccess.DeviceActivity;

internal class TrackDataPersistence : ITrackDataPersistence, IService
{
	private const string TableName = "TrackData";

	private const string IdComumn = "Id";

	private const string DeviceIdColumn = "DeviceId";

	private const string PropertiesColumn = "Properties";

	private const string TimestampColumn = "Timestamp";

	private const string EntityIdColumn = "EntityId";

	private const string EventTypeColumn = "EventType";

	private const string EntityTypeColumn = "EntityType";

	private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

	private const string InsertCommandFormat = "INSERT INTO [{0}] ({1}, {2}, {3}, {4}, {5}, {6}) VALUES (@DeviceId, @EntityId, @EntityType, @Timestamp, @Properties, @EventType);";

	private const string SelectByEntityCommandFormat = "SELECT * FROM [{0}] WHERE [EntityType] = '{1}' AND [EntityId] = '{2}' ORDER BY [Timestamp] DESC";

	private const string SelectOldestCommandFormat = "SELECT TOP({0}) * FROM [{1}] ORDER BY [{2}] ASC";

	private const string DeleteAllCommandFormat = "DELETE FROM [{0}]";

	private const int RemoveOldestBatchSize = 500;

	private DatabaseConnectionsPool persistence;

	private SerializerJson serializer;

	private volatile int entriesCount;

	private object locker = new object();

	private readonly int MaxLogEntriesCount;

	private readonly IDalUsbStorage dalUsbStorage;

	private readonly IEventManager eventManager;

	public TrackDataPersistence(IConfigurationManager configManager, IDalUsbStorage dalUsbStorage, IEventManager eventManager)
	{
		this.dalUsbStorage = dalUsbStorage;
		this.eventManager = eventManager;
		int? num = configManager["RWE.SmartHome.SHC.DataAccess"].GetInt("MaxDALEntries");
		if (num.HasValue)
		{
			MaxLogEntriesCount = num.Value;
		}
		else
		{
			MaxLogEntriesCount = 4000;
		}
		this.dalUsbStorage.ExportDalToUsb();
		persistence = DalDbConnectionsPool.Pool;
		serializer = new SerializerJson();
		this.eventManager.GetEvent<USBDriveNotificationEvent>().Subscribe(OnUsbStateChanged, null, ThreadOption.PublisherThread, null);
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.TrackData);
		list.Add(Resources.TrackData_PKey);
		list.Add(Resources.TrackData_Index);
		list.Add(Resources.TrackData_DropIndex);
		list.Add(Resources.TrackData_AlterEntityId);
		list.Add(Resources.TrackData_Index);
		List<string> list2 = list;
		foreach (string item in list2)
		{
			using DatabaseConnection databaseConnection = persistence.GetConnection();
			databaseConnection.ExecuteNonQuery(item);
		}
		lock (locker)
		{
			entriesCount = GetCount();
		}
	}

	private void OnUsbStateChanged(USBDriveNotificationEventArgs args)
	{
		if (args.Attached)
		{
			dalUsbStorage.ExportDalToUsb();
		}
		persistence = new DatabaseConnectionsPool("RWE.SmartHome.SHC.Dal.sdf");
	}

	private int GetCount()
	{
		int num = 0;
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("SELECT COUNT (*) FROM [{0}]", "TrackData"));
			num = (int)databaseCommand.ExecuteScalar();
		}
		try
		{
			long num2 = -1L;
			num2 = new FileInfo("\\NandFlash\\SHC\\RWE.SmartHOme.SHC.DAL.sdf").Length;
			Log.Debug(Module.DataAccess, $"DAL entry Count: {num}, db size: {num2}");
		}
		catch
		{
			Log.Debug(Module.DataAccess, $"DAL entry Count: {num}");
		}
		return num;
	}

	public void Uninitialize()
	{
		DalDbConnectionsPool.Dispose();
	}

	public void Add(TrackData data)
	{
		if (IsInvalid(data))
		{
			return;
		}
		string commandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}, {4}, {5}, {6}) VALUES (@DeviceId, @EntityId, @EntityType, @Timestamp, @Properties, @EventType);", "TrackData", "DeviceId", "EntityId", "EntityType", "Timestamp", "Properties", "EventType");
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
			SetParameters(data, databaseCommand);
			try
			{
				databaseCommand.ExecuteNonQuery();
				lock (locker)
				{
					entriesCount++;
				}
			}
			catch (Exception ex)
			{
				Log.Error(Module.DataAccess, ex.ToString());
			}
		}
		EnsureMaxEntriesLimit();
	}

	private static bool IsInvalid(TrackData data)
	{
		if (!string.IsNullOrEmpty(data.EntityId) && !string.IsNullOrEmpty(data.EntityType) && !string.IsNullOrEmpty(data.EventType) && !string.IsNullOrEmpty(data.EntityType))
		{
			return data.Properties == null;
		}
		return true;
	}

	private void SetParameters(TrackData data, DatabaseCommand command)
	{
		SqlCeParameter val = command.CreateParameter();
		((DbParameter)(object)val).ParameterName = "@DeviceId";
		object value;
		if (!string.IsNullOrEmpty(data.DeviceId))
		{
			object obj = (((DbParameter)(object)val).Value = data.DeviceId);
			value = obj;
		}
		else
		{
			value = DBNull.Value;
		}
		((DbParameter)(object)val).Value = value;
		command.Parameters.Add(val);
		SqlCeParameter val2 = command.CreateParameter();
		((DbParameter)(object)val2).ParameterName = "@EntityId";
		((DbParameter)(object)val2).Value = data.EntityId;
		command.Parameters.Add(val2);
		SqlCeParameter val3 = command.CreateParameter();
		((DbParameter)(object)val3).ParameterName = "@EntityType";
		((DbParameter)(object)val3).Value = data.EntityType;
		command.Parameters.Add(val3);
		SqlCeParameter val4 = command.CreateParameter();
		((DbParameter)(object)val4).ParameterName = "@Timestamp";
		((DbParameter)(object)val4).Value = data.Timestamp;
		command.Parameters.Add(val4);
		SqlCeParameter val5 = command.CreateParameter();
		((DbParameter)(object)val5).ParameterName = "@Properties";
		((DbParameter)(object)val5).Value = serializer.Serialize(data.Properties);
		command.Parameters.Add(val5);
		SqlCeParameter val6 = command.CreateParameter();
		((DbParameter)(object)val6).ParameterName = "@EventType";
		((DbParameter)(object)val6).Value = data.EventType;
		command.Parameters.Add(val6);
	}

	private void EnsureMaxEntriesLimit()
	{
		if (entriesCount > MaxLogEntriesCount)
		{
			Log.Warning(Module.DeviceActivity, "MaxLogEntriesCount reached. Values will be rolled over");
			DeleteOldest(500);
			lock (locker)
			{
				entriesCount = GetCount();
			}
		}
	}

	private void DeleteOldest(int count)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("SELECT TOP({0}) * FROM [{1}] ORDER BY [{2}] ASC", count, "TrackData", "Timestamp"));
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			while (((DbDataReader)(object)val).Read())
			{
				val.Delete();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<TrackData> GetAllEvents()
	{
		List<TrackData> list = new List<TrackData>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("TrackData");
		databaseCommand.CommandType = CommandType.TableDirect;
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			CollectResult(list, val);
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void CollectResult(List<TrackData> events, SqlCeResultSet result)
	{
		while (((DbDataReader)(object)result).Read())
		{
			events.Add(new TrackData
			{
				DeviceId = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("DeviceId")),
				EntityId = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("EntityId")),
				EntityType = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("EntityType")),
				Properties = serializer.Deserialize<List<Property>>(((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("Properties"))),
				Timestamp = DateTime.SpecifyKind(((DbDataReader)(object)result).GetDateTime(((DbDataReader)(object)result).GetOrdinal("Timestamp")), DateTimeKind.Utc),
				EventType = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("EventType"))
			});
		}
	}

	public List<TrackData> GetEventsForEntity(string entityType, string entityId)
	{
		List<TrackData> list = new List<TrackData>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		string commandText = string.Format("SELECT * FROM [{0}] WHERE [EntityType] = '{1}' AND [EntityId] = '{2}' ORDER BY [Timestamp] DESC", "TrackData", entityType, entityId);
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			CollectResult(list, val);
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<TrackData> GetEventsForEntity(string entityType, DateTime start, DateTime end, int offset, int limit, string dataPattern)
	{
		List<TrackData> list = new List<TrackData>();
		string commandText = string.Format("SELECT * FROM [TrackData] WHERE [EntityType] = '{0}' AND ([Timestamp] BETWEEN '{1}' AND '{2}')" + (string.IsNullOrEmpty(dataPattern) ? string.Empty : " AND [Properties] LIKE '%{3}%'") + " ORDER BY [Timestamp] DESC", entityType, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"), dataPattern);
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			CollectQueryResults(offset, limit, list, val);
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<TrackData> GetEventsForEntity(string entityType, string entityId, DateTime start, DateTime end, int offset, int limit, string dataPattern)
	{
		List<TrackData> list = new List<TrackData>();
		string commandText = string.Format("SELECT * FROM [TrackData] WHERE [EntityType] = '{0}' AND [EntityId] = '{1}' AND ([Timestamp] BETWEEN '{2}' AND '{3}')" + (string.IsNullOrEmpty(dataPattern) ? string.Empty : " AND [Properties] LIKE '%{4}%'") + " ORDER BY [Timestamp] DESC", entityType, entityId, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"), dataPattern);
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			CollectQueryResults(offset, limit, list, val);
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<TrackData> GetEvents(string entityType, string eventType, DateTime start, DateTime end, int offset, int limit, string dataPattern)
	{
		List<TrackData> list = new List<TrackData>();
		string commandText = string.Format("SELECT * FROM [TrackData] WHERE [EntityType] = '{0}' AND [EventType] = '{1}' AND ([Timestamp] BETWEEN '{2}' AND '{3}')" + (string.IsNullOrEmpty(dataPattern) ? string.Empty : " AND [Properties] LIKE '%{4}%'") + " ORDER BY [Timestamp] DESC", entityType, eventType, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"), dataPattern);
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			CollectQueryResults(offset, limit, list, val);
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<TrackData> GetEvents(string entityType, string entityId, string eventType, DateTime start, DateTime end, int offset, int limit, string dataPattern)
	{
		List<TrackData> list = new List<TrackData>();
		string commandText = string.Format("SELECT * FROM [TrackData] WHERE [EntityType] = '{0}' AND [EntityId] = '{1}' AND [EventType] = '{2}' AND ([Timestamp] BETWEEN '{3}' AND '{4}')" + (string.IsNullOrEmpty(dataPattern) ? string.Empty : " AND [Properties] LIKE '%{5}%'") + " ORDER BY [Timestamp] DESC", entityType, entityId, eventType, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"), dataPattern);
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			CollectQueryResults(offset, limit, list, val);
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void CollectQueryResults(int offset, int limit, List<TrackData> data, SqlCeResultSet result)
	{
		for (int i = 0; i < offset; i++)
		{
			if (!((DbDataReader)(object)result).Read())
			{
				break;
			}
		}
		for (int j = 0; j < limit; j++)
		{
			if (!((DbDataReader)(object)result).Read())
			{
				break;
			}
			TrackData trackData = new TrackData();
			trackData.DeviceId = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("DeviceId"));
			trackData.EntityId = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("EntityId"));
			trackData.EntityType = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("EntityType"));
			trackData.Properties = serializer.Deserialize<List<Property>>(((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("Properties")));
			trackData.Timestamp = DateTime.SpecifyKind(((DbDataReader)(object)result).GetDateTime(((DbDataReader)(object)result).GetOrdinal("Timestamp")), DateTimeKind.Utc);
			trackData.EventType = ((DbDataReader)(object)result).GetString(((DbDataReader)(object)result).GetOrdinal("EventType"));
			TrackData item = trackData;
			data.Add(item);
		}
	}

	public void DeleteAll()
	{
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("DELETE FROM [{0}]", "TrackData"));
			databaseCommand.ExecuteNonQuery();
		}
		lock (locker)
		{
			entriesCount = 0;
		}
	}
}
