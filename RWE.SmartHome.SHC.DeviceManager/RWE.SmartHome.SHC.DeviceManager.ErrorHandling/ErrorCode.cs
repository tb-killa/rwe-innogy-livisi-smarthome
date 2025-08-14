namespace RWE.SmartHome.SHC.DeviceManager.ErrorHandling;

public enum ErrorCode
{
	Unknown,
	DeviceInclusionError,
	DeviceDoesNotExist,
	DeviceUnknownInBackend,
	DeviceKeyExchangeUnexpectedException,
	DeviceKeyExchangeNotReachedYet,
	InvalidConfigurationAction,
	Busy,
	Timeout,
	SerialTimeout,
	MediumBusy,
	NoReply,
	Error,
	Incoming,
	CrcError,
	ModeError,
	DutyCycle,
	BidcosInclusionFailed,
	BidcosGroupAddressFailed
}
