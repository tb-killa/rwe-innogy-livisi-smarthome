using System;
using System.IO;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;

namespace RWE.SmartHome.SHC.DataAccess.DeviceActivity;

public class DalUsbStorage : IDalUsbStorage
{
	private string usbPath = "Hard Disk";

	public void ExportDalToUsb()
	{
		try
		{
			if (!Directory.Exists(usbPath))
			{
				Log.Information(Module.DataAccess, "Cannot export the DAL, the USB is not connected");
				return;
			}
			ExportDatabaseFileOnUsb();
			SetDalDatabaseConnectionString(Path.Combine("Hard Disk\\data", "RWE.SmartHome.SHC.Dal.sdf"));
		}
		catch (Exception ex)
		{
			Log.Information(Module.DataAccess, $"There was a problem exporting the DAL database to USB {ex.Message} {ex.StackTrace}");
		}
	}

	private void SetDalDatabaseConnectionString(string databaseFilePath)
	{
		DalDbConnectionsPool.Pool.DatabaseFileName = databaseFilePath;
	}

	private void ExportDatabaseFileOnUsb()
	{
		CreateDirectoryOnUsb();
		if (!File.Exists(Path.Combine("Hard Disk\\data", "RWE.SmartHome.SHC.Dal.sdf")))
		{
			Log.Information(Module.DataAccess, "Exporting the DAL file to USB");
			File.Copy(DatabaseConnectionsPool.DalDatabaseFile, Path.Combine("Hard Disk\\data", "RWE.SmartHome.SHC.Dal.sdf"));
		}
	}

	private void CreateDirectoryOnUsb()
	{
		if (!Directory.Exists("Hard Disk\\data"))
		{
			Directory.CreateDirectory("Hard Disk\\data");
		}
	}
}
