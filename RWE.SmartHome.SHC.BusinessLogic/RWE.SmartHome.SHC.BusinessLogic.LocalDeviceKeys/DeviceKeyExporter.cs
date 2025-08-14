using System.Collections.Generic;
using System.IO;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;

public class DeviceKeyExporter : IDeviceKeyExporter
{
	private const string ExportCsvFileName = "DevicesKeysStorage.csv";

	private const string ExportCsvDirectoryPath = "Hard Disk\\devices\\";

	private readonly IEventManager eventManager;

	private readonly IDeviceKeyRepository deviceKeyRepository;

	private readonly IExportDevicesKeysEmailSenderScheduler exportDevicesKeysEmailSenderScheduler;

	public DeviceKeyExporter(IEventManager eventManager, IDeviceKeyRepository deviceKeyRepository, IExportDevicesKeysEmailSenderScheduler exportDevicesKeysEmailSenderScheduler)
	{
		this.eventManager = eventManager;
		this.deviceKeyRepository = deviceKeyRepository;
		this.exportDevicesKeysEmailSenderScheduler = exportDevicesKeysEmailSenderScheduler;
		this.exportDevicesKeysEmailSenderScheduler.ScheduleEmailSending();
		eventManager.GetEvent<USBDriveNotificationEvent>().Subscribe(ExportDeviceKeyCsv, null, ThreadOption.PublisherThread, null);
	}

	private void ExportDeviceKeyCsv(USBDriveNotificationEventArgs args)
	{
		if (!args.Attached)
		{
			return;
		}
		if (!File.Exists("\\NandFlash\\DevicesKeysStorage.csv"))
		{
			Log.Information(Module.BusinessLogic, "The Devices Keys storage CSV does not exist");
		}
		try
		{
			ExportFile();
		}
		catch (IOException ex)
		{
			Log.Error(Module.BusinessLogic, $"There was an error exporting the device keys csv file, {ex.Message} {ex.StackTrace}");
		}
	}

	private void ExportFile()
	{
		if (File.Exists(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv")))
		{
			ImportKeysFromCsv();
		}
		CreateDirectoryOnUsb();
		if (File.Exists(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv")))
		{
			File.Delete(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv"));
		}
		File.Copy("\\NandFlash\\DevicesKeysStorage.csv", Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv"));
		FilePersistence.DevicesKeysExported = true;
	}

	private void ImportKeysFromCsv()
	{
		List<StoredDevice> allDevicesKeysFromFile = deviceKeyRepository.GetAllDevicesKeysFromFile(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv"));
		List<StoredDevice> allDevicesKeysFromStorage = deviceKeyRepository.GetAllDevicesKeysFromStorage();
		List<StoredDevice> list = new List<StoredDevice>();
		StoredDevice usbStoredDevice;
		foreach (StoredDevice item in allDevicesKeysFromFile)
		{
			usbStoredDevice = item;
			if (allDevicesKeysFromStorage.FirstOrDefault((StoredDevice device) => device.Sgtin.SequenceEqual(usbStoredDevice.Sgtin)) == null)
			{
				list.Add(usbStoredDevice);
			}
		}
		if (list.Any())
		{
			deviceKeyRepository.ImportDevicesKeys(list);
			File.Delete(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv"));
		}
	}

	private void CreateDirectoryOnUsb()
	{
		if (!Directory.Exists("Hard Disk\\devices\\"))
		{
			Directory.CreateDirectory("Hard Disk\\devices\\");
		}
	}
}
