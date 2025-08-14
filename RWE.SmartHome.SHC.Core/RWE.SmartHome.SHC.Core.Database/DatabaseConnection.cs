using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Core.Database;

public sealed class DatabaseConnection : IDisposable
{
	private readonly DatabaseConnectionsPool dbPool;

	private readonly DatabaseConnectionEntry realConnection;

	private bool disposed;

	internal DatabaseConnection(DatabaseConnectionsPool dbPool, DatabaseConnectionEntry connectionEntry)
	{
		this.dbPool = dbPool;
		realConnection = connectionEntry;
		realConnection.LockForCurrentThread();
	}

	~DatabaseConnection()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			disposed = true;
			if (disposing)
			{
				realConnection.UnlockFromCurrentThread();
			}
			else
			{
				realConnection.UnlockUnchecked();
			}
		}
	}

	public DatabaseCommand CreateDatabaseCommand()
	{
		CheckDisposed();
		return new DatabaseCommand(dbPool, realConnection);
	}

	public DatabaseCommand CreateDatabaseCommand(string commandText)
	{
		CheckDisposed();
		DatabaseCommand databaseCommand = new DatabaseCommand(dbPool, realConnection);
		databaseCommand.CommandText = commandText;
		return databaseCommand;
	}

	public void BeginTransaction()
	{
		CheckDisposed();
		realConnection.BeginTransaction();
	}

	public void CommitTransaction()
	{
		CheckDisposed();
		realConnection.CommitTransaction();
	}

	public void RollbackTransaction()
	{
		CheckDisposed();
		realConnection.RollbackTransaction();
	}

	public bool ExecuteNonQueries(List<string> commandTexts)
	{
		CheckDisposed();
		BeginTransaction();
		bool result = false;
		try
		{
			foreach (string commandText in commandTexts)
			{
				using DatabaseCommand databaseCommand = CreateDatabaseCommand(commandText);
				databaseCommand.ExecuteNonQuery();
			}
			CommitTransaction();
			result = true;
		}
		catch (Exception)
		{
			RollbackTransaction();
		}
		return result;
	}

	public bool ExecuteNonQuery(string commandText)
	{
		CheckDisposed();
		BeginTransaction();
		bool result = false;
		try
		{
			using (DatabaseCommand databaseCommand = CreateDatabaseCommand(commandText))
			{
				databaseCommand.ExecuteNonQuery();
			}
			CommitTransaction();
			result = true;
		}
		catch
		{
			RollbackTransaction();
		}
		return result;
	}

	private void CheckDisposed()
	{
		if (disposed || dbPool.Disposed)
		{
			throw new ObjectDisposedException("Using properties or methods of a database connection object already disposed");
		}
	}
}
