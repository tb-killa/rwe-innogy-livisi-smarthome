using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;

namespace RWE.SmartHome.SHC.Core.Database;

public class DatabaseCommand : IDisposable
{
	private bool disposed;

	private readonly SqlCeCommand command;

	private readonly DatabaseConnectionsPool dbPool;

	private readonly DatabaseConnectionEntry realConnection;

	public string CommandText
	{
		get
		{
			CheckDisposed();
			return ((DbCommand)(object)command).CommandText;
		}
		set
		{
			CheckDisposed();
			((DbCommand)(object)command).CommandText = value;
		}
	}

	public int CommandTimeout
	{
		get
		{
			CheckDisposed();
			return ((DbCommand)(object)command).CommandTimeout;
		}
		set
		{
			CheckDisposed();
			((DbCommand)(object)command).CommandTimeout = value;
		}
	}

	public CommandType CommandType
	{
		get
		{
			CheckDisposed();
			return ((DbCommand)(object)command).CommandType;
		}
		set
		{
			CheckDisposed();
			((DbCommand)(object)command).CommandType = value;
		}
	}

	public SqlCeParameterCollection Parameters
	{
		get
		{
			CheckDisposed();
			return command.Parameters;
		}
	}

	public string IndexName
	{
		get
		{
			CheckDisposed();
			return command.IndexName;
		}
		set
		{
			CheckDisposed();
			command.IndexName = value;
		}
	}

	internal DatabaseCommand(DatabaseConnectionsPool dbPool, DatabaseConnectionEntry conn)
	{
		this.dbPool = dbPool;
		command = conn.Connection.CreateCommand();
		realConnection = conn;
	}

	public void Dispose()
	{
		if (!disposed)
		{
			disposed = true;
			((Component)(object)command).Dispose();
		}
	}

	public SqlCeParameter CreateParameter()
	{
		CheckDisposed();
		return command.CreateParameter();
	}

	public int ExecuteNonQuery()
	{
		CheckDisposed();
		if (realConnection.Transaction != null)
		{
			command.Transaction = realConnection.Transaction;
		}
		return ((DbCommand)(object)command).ExecuteNonQuery();
	}

	public object ExecuteScalar()
	{
		CheckDisposed();
		if (realConnection.Transaction != null)
		{
			command.Transaction = realConnection.Transaction;
		}
		return ((DbCommand)(object)command).ExecuteScalar();
	}

	public SqlCeResultSet ExecuteResultSet(ResultSetOptions options)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		CheckDisposed();
		if (realConnection.Transaction != null)
		{
			command.Transaction = realConnection.Transaction;
		}
		return command.ExecuteResultSet(options);
	}

	public void SetRange(DbRangeOptions dbRangeOptions, object[] startData, object[] endData)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		CheckDisposed();
		command.SetRange(dbRangeOptions, startData, endData);
	}

	private void CheckDisposed()
	{
		if (disposed || dbPool.Disposed)
		{
			throw new ObjectDisposedException("Using properties or methods of a database command object already disposed");
		}
	}
}
