using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceFirmwareUpdate;

internal class DeviceFirmwareImagesService : IDeviceFirmwareImagesService, IDeviceFirmwareRepository, IDeviceUpdateClient
{
	private const string LoggingSource = "DeviceFirmwareImagesService";

	private readonly IDeviceFirmwareRepository repository;

	private readonly IDeviceUpdateClient deviceUpdateClient;

	private readonly IBusinessLogic businessLogic;

	public event EventHandler<FirmwareDownloadFinishedEventArgs> FirmwareDownloadFinished
	{
		add
		{
			repository.FirmwareDownloadFinished += value;
		}
		remove
		{
			repository.FirmwareDownloadFinished -= value;
		}
	}

	public DeviceFirmwareImagesService(IDeviceFirmwareRepository firmwareRepository, IDeviceUpdateClient deviceUpdateClient, IBusinessLogic businessLogic)
	{
		repository = firmwareRepository;
		this.deviceUpdateClient = deviceUpdateClient;
		this.businessLogic = businessLogic;
	}

	public void DownloadFirmware(DeviceDescriptor deviceDescriptor, string url, string md5Hash, string targetVersion)
	{
		repository.DownloadFirmware(deviceDescriptor, url, md5Hash, targetVersion);
	}

	public DeviceFirmwareDescriptor GetFirmware(DeviceDescriptor deviceDescriptor)
	{
		return repository.GetFirmware(deviceDescriptor);
	}

	public List<DeviceDescriptor> GetDownloadedFirmwareDescriptors()
	{
		return repository.GetDownloadedFirmwareDescriptors();
	}

	public void DeleteFirmware(DeviceDescriptor deviceDescriptor)
	{
		repository.DeleteFirmware(deviceDescriptor);
	}

	public DeviceUpdateResultCode CheckForDeviceUpdate(DeviceDescriptor deviceDescriptor, out RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateInfo updateInfo)
	{
		return deviceUpdateClient.CheckForDeviceUpdate(deviceDescriptor, out updateInfo);
	}

	public void CheckDeviceUpdate(DeviceDescriptor deviceDescriptor)
	{
		string devGrp = $"[manuf={deviceDescriptor.Manufacturer}], [prod={deviceDescriptor.ProductId}], [hw={deviceDescriptor.HardwareVersion}], [fw={deviceDescriptor.FirmwareVersion}], [addin={deviceDescriptor.AddInVersion}]";
		businessLogic.PerformBackendCommunicationWithRetries("Update check for device " + devGrp + " failed.", delegate
		{
			try
			{
				RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateInfo updateInfo;
				DeviceUpdateResultCode deviceUpdateResultCode = CheckForDeviceUpdate(deviceDescriptor, out updateInfo);
				switch (deviceUpdateResultCode)
				{
				case DeviceUpdateResultCode.NewerVersionAvailable:
				{
					Log.Debug(Module.BusinessLogic, "DeviceFirmwareImagesService", $"New firmware version v{updateInfo.Version} found for the device group:{devGrp}");
					DeviceFirmwareDescriptor firmware = GetFirmware(deviceDescriptor);
					if (firmware != null)
					{
						Log.Debug(Module.BusinessLogic, "DeviceFirmwareImagesService", "New FW version is already downloaded.");
					}
					else
					{
						DownloadFirmware(deviceDescriptor, updateInfo.ImageUrl, updateInfo.MD5Hash, updateInfo.Version);
					}
					break;
				}
				default:
					Log.Error(Module.BusinessLogic, "DeviceFirmwareImagesService", $"Error code [{deviceUpdateResultCode}] received while checking for FW update for device group:{devGrp}");
					break;
				case DeviceUpdateResultCode.AlreadyLatestVersion:
					break;
				}
			}
			catch (Exception ex)
			{
				Log.Exception(Module.BusinessLogic, "DeviceFirmwareImagesService", ex, "Unexpected error while checking for device update");
				return false;
			}
			return true;
		});
	}

	public void RemoveUnneededImages(List<DeviceDescriptor> neededFirmwareImages)
	{
		List<DeviceDescriptor> downloadedFirmwareDescriptors = GetDownloadedFirmwareDescriptors();
		List<DeviceDescriptor> list = downloadedFirmwareDescriptors.Except(neededFirmwareImages, new DeviceDescriptorComparer()).ToList();
		foreach (DeviceDescriptor item in list)
		{
			Log.Debug(Module.BusinessLogic, "DeviceFirmwareImagesService", $"Firmware for {item.FriendlyTrace()} will be removed");
			DeleteFirmware(item);
		}
	}
}
