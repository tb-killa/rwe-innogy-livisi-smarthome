using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;

namespace RWE.SmartHome.SHC.DataAccess.DeviceActivity;

internal class DeviceActivityPersistence : IDeviceActivityPersistence, IService
{
	private const string TABLE = "DeviceActivityLogs";

	private const string ID_COLUMN = "Id";

	private const string DEVICE_ID_COLUMN = "DeviceId";

	private const string ACTIVITY_TYPE_COLUMN = "ActivityType";

	private const string TIMESTAMP_COLUMN = "Timestamp";

	private const string NEW_STATE_COLUMN = "NewState";

	private const string EVENT_TYPE_COLUMN = "EventType";

	private const int TimeoutPeriod = 1200;

	private const int MaxCacheSize = 200;

	private readonly DatabaseConnectionsPool Persistence;

	private List<DeviceActivityLogEntry> LocalCache;

	private DateTime LastFlushTime;

	public DeviceActivityPersistence(DatabaseConnectionsPool persistence)
	{
		Persistence = persistence;
		LocalCache = new List<DeviceActivityLogEntry>();
		LastFlushTime = DateTime.UtcNow;
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.DeviceActivityLogs);
		list.Add(Resources.DeviceActivityLogs_PKey);
		List<string> commandTexts = list;
		using DatabaseConnection databaseConnection = Persistence.GetConnection();
		databaseConnection.ExecuteNonQueries(commandTexts);
	}

	public void Uninitialize()
	{
	}

	public bool AddEntry(DeviceActivityLogEntry newEntry)
	{
		LocalCache.Add(new DeviceActivityLogEntry
		{
			ActivityType = newEntry.ActivityType,
			DeviceId = newEntry.DeviceId,
			EntryId = newEntry.EntryId,
			NewState = newEntry.NewState,
			Timestamp = newEntry.Timestamp,
			EventType = newEntry.EventType
		});
		if (LocalCache.Count > 200 || DateTime.UtcNow - LastFlushTime > TimeSpan.FromSeconds(1200.0))
		{
			return FlushCache();
		}
		return true;
	}

	private bool FlushCache()
	{
		if (LocalCache.Count == 0)
		{
			return true;
		}
		bool result = false;
		string commandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}, {4}, {5}) VALUES (@DeviceId, @ActivityType, @Timestamp, @NewState, @EventType);", "DeviceActivityLogs", "DeviceId", "ActivityType", "Timestamp", "NewState", "EventType");
		LastFlushTime = DateTime.UtcNow;
		using (DatabaseConnection databaseConnection = Persistence.GetConnection())
		{
			try
			{
				databaseConnection.BeginTransaction();
				using (DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText))
				{
					SqlCeParameter val = databaseCommand.CreateParameter();
					((DbParameter)(object)val).ParameterName = "@DeviceId";
					databaseCommand.Parameters.Add(val);
					SqlCeParameter val2 = databaseCommand.CreateParameter();
					((DbParameter)(object)val2).ParameterName = "@DeviceFriendlyName";
					databaseCommand.Parameters.Add(val2);
					SqlCeParameter val3 = databaseCommand.CreateParameter();
					((DbParameter)(object)val3).ParameterName = "@ActivityType";
					databaseCommand.Parameters.Add(val3);
					SqlCeParameter val4 = databaseCommand.CreateParameter();
					((DbParameter)(object)val4).ParameterName = "@Timestamp";
					databaseCommand.Parameters.Add(val4);
					SqlCeParameter val5 = databaseCommand.CreateParameter();
					((DbParameter)(object)val5).ParameterName = "@NewState";
					databaseCommand.Parameters.Add(val5);
					SqlCeParameter val6 = databaseCommand.CreateParameter();
					((DbParameter)(object)val6).ParameterName = "@EventType";
					databaseCommand.Parameters.Add(val6);
					foreach (DeviceActivityLogEntry item in LocalCache)
					{
						if (string.IsNullOrEmpty(item.DeviceId))
						{
							((DbParameter)(object)val).Value = DBNull.Value;
						}
						else
						{
							((DbParameter)(object)val).Value = item.DeviceId;
						}
						((DbParameter)(object)val3).Value = item.ActivityType;
						((DbParameter)(object)val4).Value = item.Timestamp;
						((DbParameter)(object)val5).Value = item.NewState;
						((DbParameter)(object)val6).Value = item.EventType;
						try
						{
							databaseCommand.ExecuteNonQuery();
							result = true;
						}
						catch (Exception ex)
						{
							Log.Error(Module.DataAccess, "Exception: " + ex.ToString());
						}
					}
				}
				databaseConnection.CommitTransaction();
			}
			catch
			{
				databaseConnection.RollbackTransaction();
				throw;
			}
		}
		LocalCache.Clear();
		return result;
	}

	public List<DeviceActivityLogEntry> GetAllEvents()
	{
		FlushCache();
		List<DeviceActivityLogEntry> list = new List<DeviceActivityLogEntry>();
		using DatabaseConnection databaseConnection = Persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("DeviceActivityLogs");
		databaseCommand.CommandType = CommandType.TableDirect;
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			int ordinal = ((DbDataReader)(object)val).GetOrdinal("Id");
			int ordinal2 = ((DbDataReader)(object)val).GetOrdinal("ActivityType");
			int ordinal3 = ((DbDataReader)(object)val).GetOrdinal("DeviceId");
			int ordinal4 = ((DbDataReader)(object)val).GetOrdinal("NewState");
			int ordinal5 = ((DbDataReader)(object)val).GetOrdinal("Timestamp");
			int ordinal6 = ((DbDataReader)(object)val).GetOrdinal("EventType");
			while (((DbDataReader)(object)val).Read())
			{
				DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
				deviceActivityLogEntry.EntryId = ((DbDataReader)(object)val).GetInt32(ordinal);
				deviceActivityLogEntry.ActivityType = (ActivityType)((DbDataReader)(object)val).GetInt32(ordinal2);
				deviceActivityLogEntry.DeviceId = ((DbDataReader)(object)val).GetString(ordinal3);
				deviceActivityLogEntry.NewState = ((DbDataReader)(object)val).GetString(ordinal4);
				deviceActivityLogEntry.Timestamp = ((DbDataReader)(object)val).GetDateTime(ordinal5);
				deviceActivityLogEntry.EventType = (EventType)((DbDataReader)(object)val).GetInt32(ordinal6);
				DeviceActivityLogEntry item = deviceActivityLogEntry;
				list.Add(item);
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<DeviceActivityLogEntry> GetOldestEvents(int n)
	{
		if (n < 1)
		{
			throw new ArgumentException("Invalid number of entries requested");
		}
		List<DeviceActivityLogEntry> list = new List<DeviceActivityLogEntry>();
		using DatabaseConnection databaseConnection = Persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("SELECT TOP({0}) * FROM [{1}] ORDER BY [{2}] ASC", n, "DeviceActivityLogs", "Id"));
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			int ordinal = ((DbDataReader)(object)val).GetOrdinal("Id");
			int ordinal2 = ((DbDataReader)(object)val).GetOrdinal("ActivityType");
			int ordinal3 = ((DbDataReader)(object)val).GetOrdinal("DeviceId");
			int ordinal4 = ((DbDataReader)(object)val).GetOrdinal("NewState");
			int ordinal5 = ((DbDataReader)(object)val).GetOrdinal("Timestamp");
			int ordinal6 = ((DbDataReader)(object)val).GetOrdinal("EventType");
			while (((DbDataReader)(object)val).Read())
			{
				DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
				deviceActivityLogEntry.EntryId = ((DbDataReader)(object)val).GetInt32(ordinal);
				deviceActivityLogEntry.ActivityType = (ActivityType)((DbDataReader)(object)val).GetInt32(ordinal2);
				deviceActivityLogEntry.DeviceId = (((DbDataReader)(object)val).IsDBNull(ordinal3) ? null : ((DbDataReader)(object)val).GetString(ordinal3));
				deviceActivityLogEntry.NewState = ((DbDataReader)(object)val).GetString(ordinal4);
				deviceActivityLogEntry.Timestamp = ((DbDataReader)(object)val).GetDateTime(ordinal5);
				deviceActivityLogEntry.EventType = (EventType)((DbDataReader)(object)val).GetInt32(ordinal6);
				DeviceActivityLogEntry item = deviceActivityLogEntry;
				list.Add(item);
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<DeviceActivityLogEntry> GetPendingEvents()
	{
		LastFlushTime = DateTime.UtcNow;
		return LocalCache.ToList();
	}

	public void RemoveFromLocalCache(IEnumerable<DeviceActivityLogEntry> entries)
	{
		if (entries != null)
		{
			LocalCache.RemoveAll(entries.Contains<DeviceActivityLogEntry>);
		}
	}

	public void RemoveEntriesById(int fromId, int toId)
	{
		string commandText = string.Format("DELETE FROM {0} WHERE {1} <= {2} AND {2} <= {3}", "DeviceActivityLogs", fromId, "Id", toId);
		using DatabaseConnection databaseConnection = Persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		try
		{
			databaseCommand.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DataAccess, "Exception: " + ex);
		}
	}

	public void PurgeOldestEntries(int n)
	{
		FlushCache();
		using DatabaseConnection databaseConnection = Persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("SELECT TOP({0}) * FROM [{1}] ORDER BY [{2}] ASC", n, "DeviceActivityLogs", "Timestamp"));
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			((DbDataReader)(object)val).GetOrdinal("Id");
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

	public void PurgeAllEntries()
	{
		using DatabaseConnection databaseConnection = Persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("TRUNCATE TABLE [{0}]", "DeviceActivityLogs"));
		try
		{
			databaseCommand.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DataAccess, "Could not purge DAL entries: " + ex);
		}
	}

	public int GetLogCount()
	{
		int num = 0;
		using (DatabaseConnection databaseConnection = Persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("SELECT COUNT (*) FROM [{0}]", "DeviceActivityLogs"));
			num = (int)databaseCommand.ExecuteScalar();
		}
		return num + LocalCache.Count;
	}

	public void RemoveEntries()
	{
		LocalCache.Clear();
		string commandText = string.Format("DELETE FROM [{0}]", "DeviceActivityLogs");
		using DatabaseConnection databaseConnection = Persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		try
		{
			databaseCommand.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DataAccess, "Exception: " + ex.ToString());
		}
	}
}
