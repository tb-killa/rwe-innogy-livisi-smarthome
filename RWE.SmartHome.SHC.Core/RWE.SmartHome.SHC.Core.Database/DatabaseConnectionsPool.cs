using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Database;

public sealed class DatabaseConnectionsPool : IService
{
	private const int InitialPoolSize = 5;

	public const string ConfigDatabaseName = "RWE.SmartHome.SHC.Database.sdf";

	public const string DalDatabaseName = "RWE.SmartHome.SHC.Dal.sdf";

	public const string UsbDataPath = "Hard Disk\\data";

	private readonly object syncObject = new object();

	private readonly List<DatabaseConnectionEntry> usedConnections = new List<DatabaseConnectionEntry>();

	private readonly Queue<DatabaseConnectionEntry> freeConnections = new Queue<DatabaseConnectionEntry>();

	public static readonly string ConfigDatabaseFile = Path.Combine("NandFlash", "RWE.SmartHome.SHC.Database.sdf");

	public static readonly string DalDatabaseFile = Path.Combine("NandFlash", "RWE.SmartHome.SHC.Dal.sdf");

	private string databaseFileName;

	public string DatabaseFileName
	{
		get
		{
			return databaseFileName;
		}
		set
		{
			databaseFileName = value;
		}
	}

	public bool Disposed { get; private set; }

	private string DatabaseConnectionString
	{
		get
		{
			if (DatabaseFileName == "RWE.SmartHome.SHC.Dal.sdf")
			{
				return $"Default Lock Timeout=30000; Data Source={DatabaseFileName};Max Database Size=41";
			}
			return $"Default Lock Timeout=30000; Data Source={DatabaseFileName}";
		}
	}

	public DatabaseConnectionsPool(string dbName)
	{
		if (string.IsNullOrEmpty(dbName))
		{
			databaseFileName = ConfigDatabaseFile;
		}
		else if (dbName == "RWE.SmartHome.SHC.Dal.sdf")
		{
			if (!File.Exists(Path.Combine("Hard Disk\\data", "RWE.SmartHome.SHC.Dal.sdf")))
			{
				databaseFileName = Path.Combine("NandFlash", dbName);
			}
			else
			{
				databaseFileName = Path.Combine("Hard Disk\\data", "RWE.SmartHome.SHC.Dal.sdf");
			}
		}
		else
		{
			databaseFileName = Path.Combine("NandFlash", dbName);
		}
	}

	public DatabaseConnection GetConnection()
	{
		if (Disposed)
		{
			return null;
		}
		DatabaseConnectionEntry databaseConnectionEntry = null;
		lock (syncObject)
		{
			foreach (DatabaseConnectionEntry usedConnection in usedConnections)
			{
				if (usedConnection.OwnerThreadId == Thread.CurrentThread.ManagedThreadId)
				{
					databaseConnectionEntry = usedConnection;
					break;
				}
			}
			if (databaseConnectionEntry == null)
			{
				if (freeConnections.Count == 0)
				{
					freeConnections.Enqueue(NewConnection());
				}
				databaseConnectionEntry = freeConnections.Peek();
				usedConnections.Add(freeConnections.Dequeue());
			}
		}
		return new DatabaseConnection(this, databaseConnectionEntry);
	}

	private DatabaseConnectionEntry NewConnection()
	{
		DatabaseConnectionEntry databaseConnectionEntry = new DatabaseConnectionEntry(this, DatabaseConnectionString);
		databaseConnectionEntry.OwnerReleased = OnEntryOwnerReleased;
		return databaseConnectionEntry;
	}

	private void OnEntryOwnerReleased(DatabaseConnectionEntry entry)
	{
		lock (syncObject)
		{
			if (!usedConnections.Contains(entry))
			{
				throw new InvalidOperationException("Releasing an unused connection?");
			}
			usedConnections.Remove(entry);
			if (usedConnections.Count >= 5)
			{
				entry.Dispose();
			}
			else
			{
				freeConnections.Enqueue(entry);
			}
		}
	}

	public void Initialize()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		DeleteDatabaseFileIfShcIsAfterUpdateAndIsNotAccountless(DatabaseFileName);
		lock (syncObject)
		{
			Disposed = false;
			if (!File.Exists(DatabaseFileName))
			{
				Log.Information(Module.Core, $"Creating new database file: {DatabaseFileName}");
				SqlCeEngine val = new SqlCeEngine(DatabaseConnectionString);
				try
				{
					val.CreateDatabase();
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			for (int i = 0; i < 5; i++)
			{
				freeConnections.Enqueue(NewConnection());
			}
		}
	}

	private void DeleteDatabaseFileIfShcIsAfterUpdateAndIsNotAccountless(string database)
	{
		UpdatePerformedStatus item = UpdatePerformedHandling.WasUpdatePerformed();
		if (FlagsEnumExtension.HasFlag(item, UpdatePerformedStatus.Successful) && !File.Exists(LocalCommunicationConstants.IsShcLocalOnlyFlagPath) && File.Exists(database))
		{
			Log.Information(Module.Core, $"Deleting the database file: {database}");
			File.Delete(database);
		}
	}

	public void Uninitialize()
	{
		try
		{
			Disposed = true;
			for (int i = 0; i < 50; i++)
			{
				lock (syncObject)
				{
					if (usedConnections.Count == 0)
					{
						break;
					}
				}
				Thread.Sleep(100);
			}
			lock (syncObject)
			{
				while (freeConnections.Any())
				{
					freeConnections.Dequeue().Dispose();
				}
				if (usedConnections.Count > 0)
				{
					Console.WriteLine("WARNING: Forcibly shutting down pending used {0} connections", usedConnections.Count);
					usedConnections.ForEach(delegate(DatabaseConnectionEntry conn)
					{
						conn.Dispose();
					});
					usedConnections.Clear();
				}
			}
		}
		catch
		{
			Console.WriteLine("Invalid operation on closing database manager.");
		}
	}
}
