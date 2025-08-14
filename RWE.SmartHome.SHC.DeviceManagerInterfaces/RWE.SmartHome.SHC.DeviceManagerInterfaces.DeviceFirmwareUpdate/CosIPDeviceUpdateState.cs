namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;

public enum CosIPDeviceUpdateState : byte
{
	UpToDate,
	UpdateAvailable,
	TransferInProgress,
	TransferConfirmationPending,
	ReactivateDutyCycle,
	UpdateTransferred,
	UpdatePending,
	Updating
}
