using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;

public static class DeviceUpdateExtensions
{
	private struct AddinDetails
	{
		public string AppId { get; private set; }

		public string AddinVersion { get; private set; }

		public AddinDetails(string appId, string addinVersion)
		{
			this = default(AddinDetails);
			AppId = appId;
			AddinVersion = addinVersion;
		}
	}

	private static AddinDetails GetAddinDetails(LemonbeatDeviceTypeIdentifier deviceTypeId, IApplicationsHost appHost)
	{
		string appId = string.Empty;
		string addinVersion = string.Empty;
		IDeviceHandler lemonbeatDeviceHandler = appHost.GetLemonbeatDeviceHandler(deviceTypeId);
		if (lemonbeatDeviceHandler != null)
		{
			appId = (lemonbeatDeviceHandler as IAddIn).ApplicationId;
			addinVersion = appHost.GetAddinVersion(appId);
		}
		return new AddinDetails(appId, addinVersion);
	}

	public static DeviceUpdateInfo GetDeviceUpdateInfo(this DeviceInformation device, IApplicationsHost appHost)
	{
		if (device == null)
		{
			return null;
		}
		AddinDetails addinDetails = GetAddinDetails(device.DeviceTypeIdentifier, appHost);
		DeviceUpdateInfo deviceUpdateInfo = new DeviceUpdateInfo();
		deviceUpdateInfo.AppID = addinDetails.AppId;
		deviceUpdateInfo.AddInVersion = addinDetails.AddinVersion;
		deviceUpdateInfo.CurrentFirmwareVersion = device.DeviceDescription.BootloaderVersion + "-" + device.DeviceDescription.StackVersion + "-" + device.DeviceDescription.ApplicationVersion;
		deviceUpdateInfo.DeviceId = device.DeviceId;
		deviceUpdateInfo.HardwareVersion = device.DeviceDescription.HardwareVersion;
		deviceUpdateInfo.IsEventListener = device.DeviceDescription.RadioMode == RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.RadioMode.WakeOnEvent;
		deviceUpdateInfo.IsReachable = device.IsReachable;
		deviceUpdateInfo.Manufacturer = Convert.ToInt16(device.DeviceTypeIdentifier.ManufacturerId);
		deviceUpdateInfo.ProductId = device.DeviceTypeIdentifier.ProductId;
		deviceUpdateInfo.UpdateState = device.DeviceUpdateState.ToFirmwareManagerUpdateState();
		return deviceUpdateInfo;
	}

	public static DeviceUpdateState ToFirmwareManagerUpdateState(this LemonbeatDeviceUpdateState lemonbeatUpdateState)
	{
		switch (lemonbeatUpdateState)
		{
		case LemonbeatDeviceUpdateState.UpToDate:
			return DeviceUpdateState.UpToDate;
		case LemonbeatDeviceUpdateState.UpdateAvailable:
		case LemonbeatDeviceUpdateState.TransferInProgress:
			return DeviceUpdateState.TransferInProgress;
		case LemonbeatDeviceUpdateState.ImageTransferred:
			return DeviceUpdateState.ImageTransferred;
		case LemonbeatDeviceUpdateState.UpdatePending:
			return DeviceUpdateState.UpdatePending;
		case LemonbeatDeviceUpdateState.Updating:
			return DeviceUpdateState.Updating;
		default:
			return DeviceUpdateState.UpToDate;
		}
	}

	public static UpdateState ToContractsUpdateState(this LemonbeatDeviceUpdateState lemonbeatUpdateState)
	{
		switch (lemonbeatUpdateState)
		{
		case LemonbeatDeviceUpdateState.UpToDate:
		case LemonbeatDeviceUpdateState.UpdateAvailable:
		case LemonbeatDeviceUpdateState.TransferInProgress:
			return UpdateState.UpToDate;
		case LemonbeatDeviceUpdateState.ImageTransferred:
			return UpdateState.UpdateAvailable;
		case LemonbeatDeviceUpdateState.UpdatePending:
		case LemonbeatDeviceUpdateState.Updating:
			return UpdateState.Updating;
		default:
			throw new ArgumentOutOfRangeException("Invalid configuration state");
		}
	}
}
