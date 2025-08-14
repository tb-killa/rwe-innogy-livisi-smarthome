namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;

public enum ErrorCode
{
	Unknown,
	SequenceStateNotSuccess,
	DeviceDoesNotExist,
	DeviceDoesNotExistForConfig,
	DeviceFactoryResetPersistenceFailed,
	PersistConfigurationFailed,
	DeletePersistedConfigurationFailed,
	ReferenceConfigurationNull,
	DefaultConfigurationNull
}
