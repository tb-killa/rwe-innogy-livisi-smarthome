using System;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Text;
using System.Threading;

namespace RWE.SmartHome.SHC.Core.Database;

internal sealed class DatabaseConnectionEntry : IDisposable
{
	private const int InvalidThreadId = 0;

	private bool disposed;

	private int referenceCounter;

	private int referenceCounterTransaction;

	private readonly DatabaseConnectionsPool dbPool;

	internal SqlCeConnection Connection { get; private set; }

	public int OwnerThreadId { get; private set; }

	internal Action<DatabaseConnectionEntry> OwnerReleased { get; set; }

	internal SqlCeTransaction Transaction { get; private set; }

	public DatabaseConnectionEntry(DatabaseConnectionsPool dbPool, string connectionString)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		this.dbPool = dbPool;
		Connection = new SqlCeConnection(connectionString);
		((DbConnection)(object)Connection).Open();
	}

	public bool LockForCurrentThread()
	{
		CheckDisposed();
		if (OwnerThreadId != 0 && OwnerThreadId != Thread.CurrentThread.ManagedThreadId)
		{
			return false;
		}
		OwnerThreadId = Thread.CurrentThread.ManagedThreadId;
		referenceCounter++;
		return true;
	}

	public void UnlockFromCurrentThread()
	{
		if (referenceCounter <= 0)
		{
			Console.WriteLine("Reference counter decrement - a negative value: invalid");
		}
		else if (OwnerThreadId == 0 || OwnerThreadId != Thread.CurrentThread.ManagedThreadId)
		{
			Console.WriteLine("Trying to unlock a connection from invalid thread: {0} (current={1})", Thread.CurrentThread.ManagedThreadId, OwnerThreadId);
		}
		else
		{
			UnlockUnchecked();
		}
	}

	internal void UnlockUnchecked()
	{
		referenceCounter--;
		if (referenceCounter == 0)
		{
			OwnerThreadId = 0;
			OwnerReleased?.Invoke(this);
		}
	}

	public override string ToString()
	{
		CheckDisposed();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Connection held by thread: " + OwnerThreadId);
		return stringBuilder.ToString();
	}

	public void BeginTransaction()
	{
		CheckDisposed();
		if (referenceCounterTransaction == 0)
		{
			Transaction = Connection.BeginTransaction();
		}
		else if (Transaction == null)
		{
			throw new InvalidOperationException("Unable to start a new transaction on a canceled transaction");
		}
		referenceCounterTransaction++;
	}

	public void CommitTransaction()
	{
		CheckDisposed();
		referenceCounterTransaction--;
		if (referenceCounterTransaction < 0)
		{
			throw new InvalidOperationException("Commit on inexistant transaction");
		}
		if (Transaction == null)
		{
			throw new InvalidOperationException("Commit on a canceled transaction");
		}
		if (referenceCounterTransaction == 0)
		{
			((DbTransaction)(object)Transaction).Commit();
			CloseTransaction();
		}
	}

	public void RollbackTransaction()
	{
		CheckDisposed();
		if (Thread.CurrentThread.ManagedThreadId != OwnerThreadId)
		{
			throw new InvalidOperationException("Used connection from the wrong thread: " + ToString());
		}
		referenceCounterTransaction--;
		if (referenceCounterTransaction < 0)
		{
			throw new InvalidOperationException("Rollback on inexistant transaction");
		}
		((DbTransaction)(object)Transaction).Rollback();
		CloseTransaction();
	}

	public void Dispose()
	{
		if (!disposed)
		{
			disposed = true;
			if (Transaction != null)
			{
				((DbTransaction)(object)Transaction).Rollback();
				CloseTransaction();
			}
			CloseConnection();
			OwnerReleased = null;
			GC.SuppressFinalize(this);
		}
	}

	private void CloseConnection()
	{
		((DbConnection)(object)Connection).Close();
		Connection = null;
	}

	private void CloseTransaction()
	{
		Transaction.Dispose();
		Transaction = null;
	}

	private void CheckDisposed()
	{
		if (disposed || dbPool.Disposed)
		{
			throw new ObjectDisposedException("Using properties or methods of an internal database connection entry object already disposed");
		}
	}
}
