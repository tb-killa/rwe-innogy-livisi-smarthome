namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;

public enum LemonbeatDeviceUpdateState
{
	UpToDate,
	UpdateAvailable,
	TransferInProgress,
	ImageTransferred,
	UpdatePending,
	Updating
}
