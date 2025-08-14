namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ErrorHandling;

public enum ErrorCode
{
	FailedToWriteToFlash,
	FailedToBackupDeviceList,
	FailedToBackupTechnicalConfiguration,
	FailedToBackupUiConfiguration,
	FailedToBackupMessagesAndAlerts,
	FailedToBackupCustomApplicationsData,
	DeviceUnknown,
	ActuatorTypeNotSupported,
	InconsistentConfiguration,
	FailedToBackupDeviceActivityloggingConfiguration,
	FailedToBackupShcSettings
}
