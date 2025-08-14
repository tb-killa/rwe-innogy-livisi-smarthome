using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

namespace RWE.SmartHome.SHC.DataAccess.Applications;

internal class ApplicationsTokenPersistence : IApplicationsTokenPersistence, IService
{
	private const string TABLE = "ApplicationsToken";

	private const string TOKEN_COLUMN = "Token";

	private readonly DatabaseConnectionsPool persistence;

	private static readonly XmlSerializer serializer = new XmlSerializer(typeof(ApplicationsToken));

	internal ApplicationsTokenPersistence(DatabaseConnectionsPool persistence)
	{
		this.persistence = persistence;
	}

	void IApplicationsTokenPersistence.SaveApplicationsToken(ApplicationsToken token)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ApplicationsToken");
		databaseCommand.CommandType = CommandType.TableDirect;
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			using StringWriter stringWriter = new StringWriter();
			serializer.Serialize(stringWriter, token);
			if (((DbDataReader)(object)val).Read())
			{
				val.SetString(((DbDataReader)(object)val).GetOrdinal("Token"), stringWriter.ToString());
				val.Update();
			}
			else
			{
				SqlCeUpdatableRecord val2 = val.CreateRecord();
				val2.SetString(val2.GetOrdinal("Token"), stringWriter.ToString());
				val.Insert(val2);
			}
			stringWriter.Close();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	ApplicationsToken IApplicationsTokenPersistence.LoadApplicationsToken()
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("ApplicationsToken");
		databaseCommand.CommandType = CommandType.TableDirect;
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				string text = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Token"));
				if (string.IsNullOrEmpty(text))
				{
					return null;
				}
				using StringReader textReader = new StringReader(text);
				return (ApplicationsToken)serializer.Deserialize(textReader);
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	void IService.Initialize()
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		try
		{
			databaseConnection.BeginTransaction();
			using (DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.ApplicationsToken))
			{
				databaseCommand.ExecuteNonQuery();
			}
			databaseConnection.CommitTransaction();
		}
		catch (Exception)
		{
			databaseConnection.RollbackTransaction();
		}
	}

	void IService.Uninitialize()
	{
	}
}
