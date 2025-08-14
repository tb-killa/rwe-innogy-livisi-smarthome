using RWE.SmartHome.SHC.Core.Database;

namespace RWE.SmartHome.SHC.DataAccess.DeviceActivity;

public class DalDbConnectionsPool
{
	private static DatabaseConnectionsPool pool;

	public static DatabaseConnectionsPool Pool
	{
		get
		{
			if (pool == null)
			{
				pool = new DatabaseConnectionsPool("RWE.SmartHome.SHC.Dal.sdf");
				pool.Initialize();
			}
			return pool;
		}
	}

	private DalDbConnectionsPool()
	{
	}

	public static void Dispose()
	{
		if (pool != null)
		{
			pool.Uninitialize();
			pool = null;
		}
	}
}
