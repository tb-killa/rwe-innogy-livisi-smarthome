using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.Configuration;

namespace RWE.SmartHome.SHC.DataAccess.Configuration;

public sealed class ConfigurationPersistence : IConfigurationPersistence, IEntityPersistence, IConfigurationSettingsPersistence, IService
{
	private const string DATA_COLUMN = "Data";

	private const string PKEY = "PKey";

	private const string ID_COLUMN = "Id";

	private const string CONFIGURATION_VARIABLES_TABLE = "ConfigurationVariables";

	private const string CONFIGURATION_VARIABLES_KEY = "PKey";

	private const string CONFIGURATION_VERSION_NAME = "Version";

	private const string CONFIGURATION_NAME_FIELD = "Name";

	private const string CONFIGURATION_VALUE_FIELD = "Value";

	private const string CONFIGURATION_DIRTY_NAME = "ConfigDirty";

	private const string DAL_ENABLED_NAME = "DAL";

	private readonly DatabaseConnectionsPool persistence;

	private Dictionary<int, DatabaseConnection> currentConnections = new Dictionary<int, DatabaseConnection>();

	public ConfigurationPersistence(Container container)
	{
		persistence = container.Resolve<DatabaseConnectionsPool>();
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.ConfigurationVariables);
		list.Add(Resources.ConfigurationVariables_PKey);
		list.Add(Resources.LogicalDevicesXml);
		list.Add(Resources.LogicalDevicesXml_PKey);
		list.Add(Resources.HomeSetupsXml);
		list.Add(Resources.HomeSetupsXml_PKey);
		list.Add(Resources.LocationsXml);
		list.Add(Resources.LocationsXml_PKey);
		list.Add(Resources.BaseDevicesXml);
		list.Add(Resources.BaseDevicesXml_PKey);
		list.Add(Resources.InteractionsXml);
		list.Add(Resources.InteractionsXml_PKey);
		List<string> commandTexts = list;
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		databaseConnection.ExecuteNonQueries(commandTexts);
	}

	public void Uninitialize()
	{
	}

	public IEnumerable<T> Load<T>() where T : Entity
	{
		IEntitySerialization serialization = Serializations.GetSerialization(typeof(T));
		string commandText = $"SELECT [Id],[Data] FROM [{serialization.GetTableName()}]";
		XmlSerializer serializer = serialization.GetSerializer();
		List<T> list = new List<T>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			foreach (IDataRecord item in (DbDataReader)(object)val)
			{
				string s = item.GetString(item.GetOrdinal("Data"));
				using StringReader textReader = new StringReader(s);
				list.Add(serializer.Deserialize(textReader) as T);
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StringCollection LoadSerialized<T>() where T : Entity
	{
		StringCollection stringCollection = new StringCollection();
		IEntitySerialization serialization = Serializations.GetSerialization(typeof(T));
		serialization.GetSerializer();
		string commandText = $"SELECT [Id],[Data] FROM [{serialization.GetTableName()}]";
		string collectionTagName = serialization.GetCollectionTagName();
		Regex regex = new Regex("\\sxmlns:.*?\".*?\"");
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(commandText);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			stringCollection.Add($"<{collectionTagName}>");
			foreach (IDataRecord item in (DbDataReader)(object)val)
			{
				string input = item.GetString(item.GetOrdinal("Data"));
				stringCollection.Add(regex.Replace(input, string.Empty));
			}
			stringCollection.Add($"</{collectionTagName}>");
			return stringCollection;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StringCollection GetEmptyList<T>() where T : Entity
	{
		StringCollection stringCollection = new StringCollection();
		IEntitySerialization serialization = Serializations.GetSerialization(typeof(T));
		string collectionTagName = serialization.GetCollectionTagName();
		stringCollection.Add($"<{collectionTagName} />");
		return stringCollection;
	}

	public void Save<T>(IEnumerable<T> entities) where T : Entity
	{
		IEntitySerialization serialization = Serializations.GetSerialization(typeof(T));
		string tableName = serialization.GetTableName();
		XmlSerializer serializer = serialization.GetSerializer();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		try
		{
			databaseConnection.BeginTransaction();
			foreach (T entity in entities)
			{
				T current = entity;
				using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(tableName);
				databaseCommand.CommandType = CommandType.TableDirect;
				databaseCommand.IndexName = "PKey";
				databaseCommand.SetRange((DbRangeOptions)16, new object[1] { current.Id }, null);
				SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
				try
				{
					using TextWriter textWriter = new StringWriter();
					using XmlWriter xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings
					{
						OmitXmlDeclaration = true,
						Indent = false
					});
					serializer.Serialize(xmlWriter, current);
					if (((DbDataReader)(object)val).Read())
					{
						val.SetString(((DbDataReader)(object)val).GetOrdinal("Data"), textWriter.ToString());
						val.Update();
					}
					else
					{
						SqlCeUpdatableRecord val2 = val.CreateRecord();
						val2.SetGuid(val2.GetOrdinal("Id"), current.Id);
						val2.SetString(val2.GetOrdinal("Data"), textWriter.ToString());
						val.Insert(val2);
					}
					xmlWriter.Close();
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			databaseConnection.CommitTransaction();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Caught exception in DB: " + ex.Message + Environment.NewLine + ex.StackTrace);
			databaseConnection.RollbackTransaction();
			throw;
		}
	}

	public void Delete<T>(IEnumerable<T> entities) where T : Entity
	{
		string tableName = GetTableName(typeof(T));
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		try
		{
			databaseConnection.BeginTransaction();
			foreach (T entity in entities)
			{
				T current = entity;
				using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(tableName);
				databaseCommand.CommandType = CommandType.TableDirect;
				databaseCommand.IndexName = "PKey";
				databaseCommand.SetRange((DbRangeOptions)16, new object[1] { current.Id }, null);
				SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
				try
				{
					if (((DbDataReader)(object)val).Read())
					{
						val.Delete();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			databaseConnection.CommitTransaction();
		}
		catch
		{
			databaseConnection.RollbackTransaction();
		}
	}

	public void SaveConfigurationVersion(int version)
	{
		SaveConfigurationVariable("Version", version.ToString());
	}

	public int LoadConfigurationVersion()
	{
		string value = LoadConfigurationVariable("Version");
		try
		{
			return Convert.ToInt32(value);
		}
		catch
		{
			return 0;
		}
	}

	public void SaveConfigurationDirtyFlag(bool isDirty)
	{
		SaveConfigurationVariable("ConfigDirty", isDirty.ToString());
	}

	public bool LoadConfigurationDirtyFlag()
	{
		string value = LoadConfigurationVariable("ConfigDirty");
		try
		{
			return Convert.ToBoolean(value);
		}
		catch
		{
			return true;
		}
	}

	private static string GetTableName(Type type)
	{
		if ((object)typeof(LogicalDevice) == type)
		{
			return "LogicalDevicesXml";
		}
		if ((object)typeof(Location) == type)
		{
			return "LocationsXml";
		}
		if ((object)typeof(BaseDevice) == type)
		{
			return "BaseDevicesXml";
		}
		if ((object)typeof(Interaction) == type)
		{
			return "InteractionsXml";
		}
		if ((object)typeof(HomeSetup) == type)
		{
			return "HomeSetupsXml";
		}
		throw new ArgumentException($"Unsupported generic type {type.FullName}");
	}

	private void SaveConfigurationVariable(string variableName, string variableValue)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ConfigurationVariables");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { variableName }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				val.SetString(((DbDataReader)(object)val).GetOrdinal("Value"), variableValue);
				val.Update();
				return;
			}
			SqlCeUpdatableRecord val2 = val.CreateRecord();
			val2.SetString(val2.GetOrdinal("Name"), variableName);
			val2.SetString(val2.GetOrdinal("Value"), variableValue);
			val.Insert(val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private string LoadConfigurationVariable(string variableName)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ConfigurationVariables");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { variableName }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				try
				{
					return ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Value"));
				}
				catch
				{
					return null;
				}
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void Begin()
	{
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		DatabaseConnection connection;
		lock (currentConnections)
		{
			if (currentConnections.ContainsKey(managedThreadId))
			{
				throw new InvalidOperationException("Configuration persistence doesn't support nested transactions YET");
			}
			connection = persistence.GetConnection();
			currentConnections[managedThreadId] = connection;
		}
		connection.BeginTransaction();
	}

	public void Commit()
	{
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		DatabaseConnection databaseConnection;
		lock (currentConnections)
		{
			databaseConnection = currentConnections[managedThreadId];
			currentConnections.Remove(managedThreadId);
		}
		databaseConnection.CommitTransaction();
		databaseConnection.Dispose();
	}

	public void Rollback()
	{
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		DatabaseConnection databaseConnection;
		lock (currentConnections)
		{
			databaseConnection = currentConnections[managedThreadId];
			currentConnections.Remove(managedThreadId);
		}
		try
		{
			databaseConnection.RollbackTransaction();
		}
		catch (NullReferenceException)
		{
		}
		databaseConnection.Dispose();
	}
}
