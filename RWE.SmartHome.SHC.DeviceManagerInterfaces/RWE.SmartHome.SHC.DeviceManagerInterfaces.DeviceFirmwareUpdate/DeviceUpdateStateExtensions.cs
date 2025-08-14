using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.CommonFunctionality;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;

public static class DeviceUpdateStateExtensions
{
	public static DeviceUpdateState ToFirmwareManagerUpdateState(this CosIPDeviceUpdateState cosIPUpdateState)
	{
		switch (cosIPUpdateState)
		{
		case CosIPDeviceUpdateState.UpToDate:
			return DeviceUpdateState.UpToDate;
		case CosIPDeviceUpdateState.UpdateAvailable:
		case CosIPDeviceUpdateState.TransferInProgress:
		case CosIPDeviceUpdateState.TransferConfirmationPending:
		case CosIPDeviceUpdateState.ReactivateDutyCycle:
			return DeviceUpdateState.TransferInProgress;
		case CosIPDeviceUpdateState.UpdateTransferred:
			return DeviceUpdateState.ImageTransferred;
		case CosIPDeviceUpdateState.UpdatePending:
			return DeviceUpdateState.UpdatePending;
		case CosIPDeviceUpdateState.Updating:
			return DeviceUpdateState.Updating;
		default:
			return DeviceUpdateState.UpToDate;
		}
	}

	public static DeviceUpdateInfo GetDeviceUpdateInfo(this IDeviceInformation device)
	{
		if (device != null)
		{
			DeviceUpdateInfo deviceUpdateInfo = new DeviceUpdateInfo();
			deviceUpdateInfo.AddInVersion = SHCVersion.ApplicationVersion;
			deviceUpdateInfo.AppID = CoreConstants.CoreAppId;
			deviceUpdateInfo.DeviceType = PhysicalDeviceFactory.GetDeviceType(device).ToString();
			deviceUpdateInfo.CurrentFirmwareVersion = ToReadableVersion(device.ManufacturerDeviceAndFirmware);
			deviceUpdateInfo.DeviceId = device.DeviceId;
			deviceUpdateInfo.HardwareVersion = null;
			deviceUpdateInfo.IsEventListener = device.BestOperationMode == DeviceInfoOperationModes.EventListener;
			deviceUpdateInfo.IsReachable = !device.DeviceUnreachable;
			deviceUpdateInfo.Manufacturer = device.ManufacturerCode;
			deviceUpdateInfo.ProductId = device.ManufacturerDeviceType;
			deviceUpdateInfo.UpdateState = device.UpdateState.ToFirmwareManagerUpdateState();
			return deviceUpdateInfo;
		}
		return null;
	}

	private static string ToReadableVersion(byte firmware)
	{
		byte b = (byte)(firmware & 0xF);
		byte b2 = (byte)((firmware & 0xF0) >> 4);
		return $"{b2:X}.{b:X}";
	}
}
