namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

public enum MessageType : short
{
	ShcDeferrableUpdate = 1,
	ShcMandatoryUpdate = 2,
	ShcRealTimeClockLost = 3,
	UserEmailAddressNotValidated = 4,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceLowBattery = 7,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceLowRfQuality = 8,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceUnreachable = 9,
	[RequiredParameter(MessageParameterKey.FriendlyName)]
	UserInvitiationAccepted = 10,
	ShcOnlineSwitchIsOff = 11,
	ShcNoConnectionToBackend = 12,
	ShcOptionalUpdate = 13,
	ShcUpdateCompleted = 14,
	ShcUpdateCanceled = 15,
	[RequiredParameter(MessageParameterKey.FriendlyName)]
	UserForeignDeletion = 17,
	UserSelfDeletion = 18,
	AppAddedToShc = 19,
	GUIMandatoryUpdate = 20,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceFactoryReset = 23,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceFreeze = 24,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceMold = 25,
	[RequiredParameter(MessageParameterKey.DeviceGroup)]
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceUpdateAvailable = 27,
	[RequiredParameter(MessageParameterKey.DeviceGroup)]
	[RequiredParameter(MessageParameterKey.DeviceId)]
	DeviceUpdateFailed = 28,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	SmokeDetected = 29,
	[RequiredParameter(MessageParameterKey.DeviceId)]
	BidCosInclusionTimeout = 30,
	UiMessage = 31,
	AddressCollision = 32,
	[RequiredParameter(MessageParameterKey.CustomApplicationMessage)]
	[RequiredParameter(MessageParameterKey.AppId)]
	CustomApplication = 33,
	ApplicationExpirationApproaching = 36,
	ApplicationExpired = 37,
	ApplicationDisabled = 38,
	BackendConfigOutOfSync = 39,
	[RequiredParameter(MessageParameterKey.ShcLogLevelExpirationInfo)]
	LogLevelChanged = 41,
	[RequiredParameter(MessageParameterKey.ShcRemoteRebootReason)]
	[RequiredParameter(MessageParameterKey.RequesterInfo)]
	ShcRemoteRebooted = 48,
	ConsumerInformationMessage = 49,
	[RequiredParameter(MessageParameterKey.AppVersion)]
	[RequiredParameter(MessageParameterKey.AppId)]
	ApplicationLoadingError = 50,
	AppUpdatedOnShc = 51,
	AppTokenSyncFailure = 52,
	[RequiredParameter(MessageParameterKey.AppId)]
	InvalidCustomApp = 53,
	[RequiredParameter(MessageParameterKey.AppId)]
	AppDownloadFailed = 54,
	[RequiredParameter(MessageParameterKey.AppId)]
	[RequiredParameter(MessageParameterKey.AppVersion)]
	CustomAppWasUpgraded = 55,
	[RequiredParameter(MessageParameterKey.AppVersion)]
	[RequiredParameter(MessageParameterKey.AppId)]
	CustomAppUpgradeFailed = 56,
	[RequiredParameter(MessageParameterKey.ProtocolId)]
	USBDeviceUnplugged = 57,
	InvalidAesKey = 64,
	DeviceActivityLoggingEnabled = 65,
	[RequiredParameter(MessageParameterKey.MemoryLoad)]
	MemoryShortage = 66,
	LemonBeatDongleInitializationFailed = 67,
	RfCommFailureDetected = 68,
	RuleExecutionFailed = 69,
	[RequiredParameter(MessageParameterKey.AppId)]
	ProductRemoved = 70,
	[RequiredParameter(MessageParameterKey.AppId)]
	ProductActivated = 71,
	[RequiredParameter(MessageParameterKey.AppId)]
	ProductDeactivated = 72,
	[RequiredParameter(MessageParameterKey.AppId)]
	ProductUpdateAvailable = 73,
	ConfigFixEntityDeleted = 80,
	QuotaReachedSoon = 81,
	QuotaExceededLimit = 82
}
