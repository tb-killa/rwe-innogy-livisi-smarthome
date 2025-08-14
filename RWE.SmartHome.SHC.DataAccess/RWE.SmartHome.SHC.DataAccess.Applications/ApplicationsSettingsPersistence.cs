using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

namespace RWE.SmartHome.SHC.DataAccess.Applications;

internal class ApplicationsSettingsPersistence : IApplicationsSettings, IService
{
	private const string TABLE = "ApplicationsSettings";

	private const string APP_ID_COLUMN = "ApplicationId";

	private const string NAME_COLUMN = "Name";

	private const string VALUE_COLUMN = "Value";

	private const string PRIMARY_KEY = "PK_ApplicationsSettings";

	private readonly DatabaseConnectionsPool persistence;

	internal ApplicationsSettingsPersistence(DatabaseConnectionsPool persistence)
	{
		this.persistence = persistence;
	}

	void IService.Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.ApplicationsSettings);
		list.Add(Resources.ApplicationsSettings_PKey);
		List<string> commandTexts = list;
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		databaseConnection.ExecuteNonQueries(commandTexts);
	}

	void IService.Uninitialize()
	{
	}

	ConfigurationItem IApplicationsSettings.GetSetting(string appId, string name)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ApplicationsSettings");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "PK_ApplicationsSettings";
		databaseCommand.SetRange((DbRangeOptions)16, new object[2] { appId, name }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				ConfigurationItem configurationItem = new ConfigurationItem();
				configurationItem.Name = name;
				configurationItem.Value = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Value"));
				return configurationItem;
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	void IApplicationsSettings.SetValue(ConfigurationItem setting)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ApplicationsSettings");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "PK_ApplicationsSettings";
		databaseCommand.SetRange((DbRangeOptions)16, new object[2] { setting.ApplicationId, setting.Name }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				val.SetString(((DbDataReader)(object)val).GetOrdinal("Value"), setting.Value);
				val.Update();
				return;
			}
			SqlCeUpdatableRecord val2 = val.CreateRecord();
			val2.SetString(val2.GetOrdinal("ApplicationId"), setting.ApplicationId);
			val2.SetString(val2.GetOrdinal("Name"), setting.Name);
			val2.SetString(val2.GetOrdinal("Value"), setting.Value);
			val.Insert(val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	List<ConfigurationItem> IApplicationsSettings.GetAllSettings(string appId)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.GetSettingsForApplication);
		databaseCommand.Parameters.Add("appId", (object)appId);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			IEnumerable<ConfigurationItem> source = from SqlCeUpdatableRecord record in (IEnumerable)val
				select new ConfigurationItem
				{
					Name = record.GetString(record.GetOrdinal("Name")),
					Value = record.GetString(record.GetOrdinal("Value"))
				};
			return source.ToList();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	bool IApplicationsSettings.Delete(string appId, string name)
	{
		int num = 0;
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ApplicationsSettings");
			databaseCommand.CommandType = CommandType.TableDirect;
			databaseCommand.IndexName = "PK_ApplicationsSettings";
			databaseCommand.SetRange((DbRangeOptions)16, new object[2] { appId, name }, null);
			SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
			try
			{
				if (((DbDataReader)(object)val).Read())
				{
					val.Delete();
					num++;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return num > 0;
	}

	void IApplicationsSettings.RemoveAllApplicationSettings(string appId)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.DeleteAllApplicationSettings);
		databaseCommand.Parameters.Add("appId", (object)appId);
		databaseCommand.ExecuteNonQuery();
	}

	List<ConfigurationItem> IApplicationsSettings.GetAllSettings()
	{
		return GetAllSettings(includeValues: true);
	}

	public List<string> GetAllNames(string appId)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.GetSettingsNamesForApplication);
		databaseCommand.Parameters.Add("appId", (object)appId);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			IEnumerable<string> source = from SqlCeUpdatableRecord record in (IEnumerable)val
				select record.GetString(record.GetOrdinal("Name"));
			return source.ToList();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<ConfigurationItem> GetAllSettingsMetadata(out int totalSize)
	{
		totalSize = GetAllSettingsSize();
		return GetAllSettings(includeValues: false);
	}

	private List<ConfigurationItem> GetAllSettings(bool includeValues)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		string commandText = (includeValues ? string.Format("SELECT [{0}], [{1}], [{2}] FROM [{3}]", "ApplicationId", "Name", "Value", "ApplicationsSettings") : string.Format("SELECT [{0}], [{1}] FROM [{2}]", "ApplicationId", "Name", "ApplicationsSettings"));
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		databaseCommand.CommandType = CommandType.Text;
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			while (((DbDataReader)(object)val).Read())
			{
				ConfigurationItem configurationItem = new ConfigurationItem();
				configurationItem.ApplicationId = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("ApplicationId"));
				configurationItem.Name = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Name"));
				configurationItem.Value = (includeValues ? ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Value")) : null);
				ConfigurationItem item = configurationItem;
				list.Add(item);
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private int GetAllSettingsSize()
	{
		try
		{
			using DatabaseConnection databaseConnection = persistence.GetConnection();
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(string.Format("SELECT SUM(DATALENGTH([{0}]) + DATALENGTH([{1}]) + \r\nDATALENGTH(IsNull([{2}], ''))) FROM [{3}]", "ApplicationId", "Name", "Value", "ApplicationsSettings"));
			databaseCommand.CommandType = CommandType.Text;
			SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
			try
			{
				if (!((DbDataReader)(object)val).Read())
				{
					return 0;
				}
				if (((DbDataReader)(object)val).IsDBNull(0))
				{
					return 0;
				}
				return ((DbDataReader)(object)val).GetInt32(0);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.DataAccess, "Failed to compute app settings size: " + ex);
			return 0;
		}
	}
}
