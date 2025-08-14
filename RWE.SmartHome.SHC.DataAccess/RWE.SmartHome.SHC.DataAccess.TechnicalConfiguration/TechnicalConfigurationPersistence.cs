using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;

namespace RWE.SmartHome.SHC.DataAccess.TechnicalConfiguration;

public class TechnicalConfigurationPersistence : ITechnicalConfigurationPersistence, IService
{
	private const string TABLE = "TechnicalConfigurations";

	private const string PRIMARY_KEY = "TechnicalConfigurations_PKey";

	private const string ID_COLUMN = "Id";

	private const string SIZE_COLUMN = "Size";

	private const string DATA_COLUMN = "Data";

	private readonly DatabaseConnectionsPool persistence;

	public TechnicalConfigurationPersistence(Container container)
	{
		persistence = container.Resolve<DatabaseConnectionsPool>();
	}

	public IEnumerable<TechnicalConfigurationEntity> LoadAll()
	{
		List<TechnicalConfigurationEntity> list = new List<TechnicalConfigurationEntity>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("TechnicalConfigurations");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "TechnicalConfigurations_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			while (((DbDataReader)(object)val).Read())
			{
				Guid guid = ((DbDataReader)(object)val).GetGuid(((DbDataReader)(object)val).GetOrdinal("Id"));
				int @int = ((DbDataReader)(object)val).GetInt32(((DbDataReader)(object)val).GetOrdinal("Size"));
				byte[] array = new byte[@int];
				((DbDataReader)(object)val).GetBytes(((DbDataReader)(object)val).GetOrdinal("Data"), 0L, array, 0, @int);
				list.Add(new TechnicalConfigurationEntity
				{
					Id = guid,
					Data = array
				});
			}
			return list;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void SaveAll(IEnumerable<TechnicalConfigurationEntity> technicalConfigurations)
	{
		using DatabaseConnection databaseConnection = OpenDatabaseTransaction();
		try
		{
			foreach (TechnicalConfigurationEntity technicalConfiguration in technicalConfigurations)
			{
				Save(technicalConfiguration);
			}
			databaseConnection.CommitTransaction();
		}
		catch
		{
			databaseConnection.RollbackTransaction();
			throw;
		}
	}

	public void Save(TechnicalConfigurationEntity technicalConfiguration)
	{
		//IL_0116: Expected O, but got Unknown
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("TechnicalConfigurations");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "TechnicalConfigurations_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { technicalConfiguration.Id }, null);
		int num = 0;
		while (num++ < 5)
		{
			SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
			try
			{
				if (((DbDataReader)(object)val).Read())
				{
					val.SetInt32(((DbDataReader)(object)val).GetOrdinal("Size"), technicalConfiguration.Data.Length);
					val.SetBytes(((DbDataReader)(object)val).GetOrdinal("Data"), 0L, technicalConfiguration.Data, 0, technicalConfiguration.Data.Length);
					val.Update();
				}
				else
				{
					SqlCeUpdatableRecord val2 = val.CreateRecord();
					val2.SetGuid(val2.GetOrdinal("Id"), technicalConfiguration.Id);
					val2.SetInt32(val2.GetOrdinal("Size"), technicalConfiguration.Data.Length);
					val2.SetBytes(val2.GetOrdinal("Data"), 0L, technicalConfiguration.Data, 0, technicalConfiguration.Data.Length);
					val.Insert(val2);
				}
				break;
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
		}
	}

	public TechnicalConfigurationEntity Load(Guid id)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("TechnicalConfigurations");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "TechnicalConfigurations_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { id }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				int @int = ((DbDataReader)(object)val).GetInt32(((DbDataReader)(object)val).GetOrdinal("Size"));
				byte[] array = new byte[@int];
				((DbDataReader)(object)val).GetBytes(((DbDataReader)(object)val).GetOrdinal("Data"), 0L, array, 0, @int);
				TechnicalConfigurationEntity technicalConfigurationEntity = new TechnicalConfigurationEntity();
				technicalConfigurationEntity.Id = id;
				technicalConfigurationEntity.Data = array;
				return technicalConfigurationEntity;
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void Delete(Guid id)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("TechnicalConfigurations");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "TechnicalConfigurations_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { id }, null);
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

	public DatabaseConnection OpenDatabaseTransaction()
	{
		DatabaseConnection connection = persistence.GetConnection();
		connection.BeginTransaction();
		return connection;
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.TechnicalConfigurations);
		list.Add(Resources.TechnicalConfigurations_PKey);
		List<string> commandTexts = list;
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		databaseConnection.ExecuteNonQueries(commandTexts);
	}

	public void Uninitialize()
	{
	}
}
