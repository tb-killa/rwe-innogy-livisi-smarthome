namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public enum UpdateCheckResultCode
{
	AlreadyLatest = 1,
	UpdateAvailable,
	ErrorServiceNotAccessible,
	ErrorUnknown
}
