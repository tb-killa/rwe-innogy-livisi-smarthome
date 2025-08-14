using System;
using System.IO;
using System.Threading;
using RWE.SmartHome.SHC.CommonFunctionality;
using SHCWrapper.FirmwareUpdater;
using SHCWrapper.Misc;

namespace RWE.SmartHome.SHC.FirmwareUpdater;

public static class FirmwareUpdater
{
	private const string FLASH_SHC_IMAGE_FILE = "/NandFlash/update.bin";

	private const string FLASH_SHC_IMAGE_TARGET = "/NandFlash/target.bin";

	public const string FIRMWARE_UPDATE_FAILED = "F B002";

	public static bool MustUpdateFirmwareFromFlash()
	{
		return File.Exists("/NandFlash/update.bin");
	}

	public static bool UpdateFirmwareFromFlash()
	{
		try
		{
			Lcd.Text = "UPDATING";
			Lcd.Update();
			Console.WriteLine("Writing firmware update into update partition");
			File.Delete("/NandFlash/target.bin");
			File.Move("/NandFlash/update.bin", "/NandFlash/target.bin");
			if (WinCEFirmwareManager.WriteRawPartition("/NandFlash/target.bin"))
			{
				File.Delete("/NandFlash/target.bin");
				UpdatePerformedHandling.SetUpdatePerformedState(UpdatePerformedStatus.Controlled, flush: true);
				Console.WriteLine("Flash firmware update finished, resetting now");
				Lcd.Text = "REBOOT";
				Lcd.Update();
				Thread.Sleep(500);
				ResetManager.Reset();
				Thread.Sleep(int.MaxValue);
				return true;
			}
		}
		catch (Exception ex)
		{
			DumpException(ex);
		}
		Console.WriteLine("Flash firmware update failed");
		return false;
	}

	private static void DumpException(Exception ex)
	{
		Console.WriteLine("Error: {0}\r\n{1}", ex.Message, ex);
	}
}
