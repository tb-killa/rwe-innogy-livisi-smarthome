namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

public enum DeviceUpdateState : byte
{
	UpToDate,
	TransferInProgress,
	ImageTransferred,
	UpdatePending,
	Updating
}
