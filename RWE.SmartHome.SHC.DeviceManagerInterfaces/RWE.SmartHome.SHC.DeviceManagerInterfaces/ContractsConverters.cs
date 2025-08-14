using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public static class ContractsConverters
{
	public static UpdateState ToContractsUpdateState(this CosIPDeviceUpdateState state)
	{
		switch (state)
		{
		case CosIPDeviceUpdateState.UpdateTransferred:
			return UpdateState.UpdateAvailable;
		case CosIPDeviceUpdateState.UpdatePending:
		case CosIPDeviceUpdateState.Updating:
			return UpdateState.Updating;
		case CosIPDeviceUpdateState.UpToDate:
		case CosIPDeviceUpdateState.UpdateAvailable:
		case CosIPDeviceUpdateState.TransferInProgress:
		case CosIPDeviceUpdateState.TransferConfirmationPending:
		case CosIPDeviceUpdateState.ReactivateDutyCycle:
			return UpdateState.UpToDate;
		default:
			throw new ArgumentOutOfRangeException("Invalid configuration state");
		}
	}
}
