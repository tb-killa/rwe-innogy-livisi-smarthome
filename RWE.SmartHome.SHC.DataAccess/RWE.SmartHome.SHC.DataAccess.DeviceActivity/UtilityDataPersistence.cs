using System;
using System.Collections.Generic;
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

internal class UtilityDataPersistence : IUtilityDataPersistence, IService
{
	private const string TableName = "UtilityData";

	private const string IdColumn = "Id";

	private const string EntityIdColumn = "EntityId";

	private const string MeterIdCoulmn = "MeterId";

	private const string TimestampColumn = "Timestamp";

	private const string UtilityTypeColumn = "UtilityType";

	private const string ValueColumn = "Value";

	private const string DataColumn = "Data";

	private const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

	private const string InsertCommandFormat = "INSERT INTO [{0}] ({1}, {2}, {3}, {4}, {5}, {6}) VALUES (@EntityId, @MeterId, @Timestamp, @UtilityType, @Value, @Data);";

	private const string SelectOldestCommandFormat = "SELECT TOP({0}) * FROM [{1}] ORDER BY [{2}] ASC";

	private const int EntriesToDeleteCount = 500;

	private DatabaseConnectionsPool persistence;

	private SerializerJson serializer;

	private int entriesCount;

	private object locker = new object();

	private readonly int MaxDataEntryCount;

	private readonly IDalUsbStorage dalUsbStorage;

	private readonly IEventManager eventManager;

	public UtilityDataPersistence(IConfigurationManager configManager, IDalUsbStorage dalUsbStorage, IEventManager eventManager)
	{
		this.dalUsbStorage = dalUsbStorage;
		this.eventManager = eventManager;
		int? num = configManager["RWE.SmartHome.SHC.DataAccess"].GetInt("MaxUtilityEntries");
		if (num.HasValue)
		{
			MaxDataEntryCount = num.Value;
		}
		else
		{
			MaxDataEntryCount = 2000;
		}
		this.dalUsbStorage.ExportDalToUsb();
		persistence = DalDbConnectionsPool.Pool;
		serializer = new SerializerJson();
		this.eventManager.GetEvent<USBDriveNotificationEvent>().Subscribe(OnUsbStateChanged, null, ThreadOption.PublisherThread, null);
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.UtilityData);
		list.Add(Resources.UtilityData_PKey);
		list.Add(Resources.UtilityData_Index);
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

	private int GetCount()
	{
		int num = 0;
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("SELECT COUNT (*) FROM [{0}]", "UtilityData"));
			num = (int)databaseCommand.ExecuteScalar();
		}
		try
		{
			long num2 = -1L;
			num2 = new FileInfo(persistence.DatabaseFileName).Length;
			Log.Debug(Module.DataAccess, $"Utility entry Count: {num}, db size: {num2}");
		}
		catch
		{
			Log.Debug(Module.DataAccess, $"Utility entry Count: {num}");
		}
		return num;
	}

	public void Uninitialize()
	{
		DalDbConnectionsPool.Dispose();
	}

	public void Add(UtilityData data)
	{
		string commandText = string.Format("INSERT INTO [{0}] ({1}, {2}, {3}, {4}, {5}, {6}) VALUES (@EntityId, @MeterId, @Timestamp, @UtilityType, @Value, @Data);", "UtilityData", "EntityId", "MeterId", "Timestamp", "UtilityType", "Value", "Data");
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

	private void SetParameters(UtilityData data, DatabaseCommand insertCommand)
	{
		SqlCeParameter val = insertCommand.CreateParameter();
		((DbParameter)(object)val).ParameterName = "@EntityId";
		((DbParameter)(object)val).Value = data.EntityId;
		insertCommand.Parameters.Add(val);
		SqlCeParameter val2 = insertCommand.CreateParameter();
		((DbParameter)(object)val2).ParameterName = "@MeterId";
		((DbParameter)(object)val2).Value = data.MeterId;
		insertCommand.Parameters.Add(val2);
		SqlCeParameter val3 = insertCommand.CreateParameter();
		((DbParameter)(object)val3).ParameterName = "@Timestamp";
		((DbParameter)(object)val3).Value = data.Timestamp;
		insertCommand.Parameters.Add(val3);
		SqlCeParameter val4 = insertCommand.CreateParameter();
		((DbParameter)(object)val4).ParameterName = "@UtilityType";
		((DbParameter)(object)val4).Value = (byte)data.Utility;
		insertCommand.Parameters.Add(val4);
		SqlCeParameter val5 = insertCommand.CreateParameter();
		((DbParameter)(object)val5).ParameterName = "@Value";
		((DbParameter)(object)val5).Value = data.Value;
		insertCommand.Parameters.Add(val5);
		SqlCeParameter val6 = insertCommand.CreateParameter();
		((DbParameter)(object)val6).ParameterName = "@Data";
		((DbParameter)(object)val6).Value = serializer.Serialize(data.Data);
		insertCommand.Parameters.Add(val6);
	}

	private void OnUsbStateChanged(USBDriveNotificationEventArgs args)
	{
		persistence = new DatabaseConnectionsPool("RWE.SmartHome.SHC.Dal.sdf");
	}

	private void EnsureMaxEntriesLimit()
	{
		if (entriesCount > MaxDataEntryCount)
		{
			Log.Warning(Module.DeviceActivity, "MaxDataEntriesCount reached. Values will be rolled over");
			DeleteOldest(500);
			lock (locker)
			{
				entriesCount = GetCount();
			}
		}
	}

	private void DeleteOldest(int count)
	{
		string commandText = string.Format("SELECT TOP({0}) * FROM [{1}] ORDER BY [{2}] ASC", count, "UtilityData", "Timestamp");
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)1);
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

	public List<UtilityData> Get(UtilityType utilityType, string meterId, DateTime start, DateTime end, int offset, int limit, string dataPattern)
	{
		List<UtilityData> list = new List<UtilityData>();
		string commandText = string.Format("SELECT * FROM [UtilityData] WHERE [UtilityType] = {0} AND [MeterId] = '{1}' " + "AND ([Timestamp] BETWEEN '{2}' AND '{3}') " + ((dataPattern != null) ? "AND [Data] LIKE '%{4}%' " : string.Empty) + "ORDER BY [Timestamp] ASC ", (byte)utilityType, meterId, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"), dataPattern);
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
			SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
			try
			{
				for (int i = 0; i < offset; i++)
				{
					if (!((DbDataReader)(object)val).Read())
					{
						break;
					}
				}
				for (int j = 0; j < limit; j++)
				{
					if (!((DbDataReader)(object)val).Read())
					{
						break;
					}
					UtilityData utilityData = new UtilityData();
					utilityData.Id = ((DbDataReader)(object)val).GetInt32(((DbDataReader)(object)val).GetOrdinal("Id"));
					utilityData.EntityId = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("EntityId"));
					utilityData.MeterId = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("MeterId"));
					utilityData.Timestamp = DateTime.SpecifyKind(((DbDataReader)(object)val).GetDateTime(((DbDataReader)(object)val).GetOrdinal("Timestamp")), DateTimeKind.Utc);
					utilityData.Utility = (UtilityType)((DbDataReader)(object)val).GetByte(((DbDataReader)(object)val).GetOrdinal("UtilityType"));
					utilityData.Value = ((DbDataReader)(object)val).GetInt32(((DbDataReader)(object)val).GetOrdinal("Value"));
					utilityData.Data = new List<Property>();
					UtilityData utilityData2 = utilityData;
					string text = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Data"));
					if (!string.IsNullOrEmpty(text))
					{
						utilityData2.Data = serializer.Deserialize<List<Property>>(text);
					}
					list.Add(utilityData2);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return list;
	}

	public Survey GetSurvey(UtilityType utilityType, string meterId, string dataPattern)
	{
		string commandText = string.Format("SELECT COUNT(*) AS Count, MAX([Timestamp]) AS max, MIN([Timestamp]) AS min FROM [UtilityData] " + "WHERE [UtilityType] = {0} AND [MeterId] = '{1}' " + ((dataPattern != null) ? "AND [Data] LIKE '%{2}%'" : string.Empty), (byte)utilityType, meterId, dataPattern);
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
			SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
			try
			{
				if (((DbDataReader)(object)val).Read())
				{
					try
					{
						Survey survey = new Survey();
						survey.Count = ((DbDataReader)(object)val).GetInt32(((DbDataReader)(object)val).GetOrdinal("Count"));
						survey.Max = DateTime.SpecifyKind(((DbDataReader)(object)val).GetDateTime(((DbDataReader)(object)val).GetOrdinal("MAX")), DateTimeKind.Utc);
						survey.Min = DateTime.SpecifyKind(((DbDataReader)(object)val).GetDateTime(((DbDataReader)(object)val).GetOrdinal("MIN")), DateTimeKind.Utc);
						return survey;
					}
					catch
					{
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return null;
	}

	public void Delete()
	{
		string commandText = string.Format("DELETE FROM [{0}]", "UtilityData");
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
			int num = databaseCommand.ExecuteNonQuery();
			Log.Debug(Module.DataAccess, string.Format("Removed {0} entries from {1}", num, "UtilityData"));
		}
		lock (locker)
		{
			entriesCount = 0;
		}
	}

	public void Delete(UtilityType utilityType, string meterId)
	{
		string commandText = string.Format("DELETE FROM [{0}] WHERE [{1}] = {2} AND [{3}] = '{4}'", "UtilityData", "UtilityType", (byte)utilityType, "MeterId", meterId);
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
			int num = databaseCommand.ExecuteNonQuery();
			Log.Debug(Module.DataAccess, string.Format("Removed {0} entries from {1}", num, "UtilityData"));
		}
		lock (locker)
		{
			entriesCount = GetCount();
		}
	}
}
