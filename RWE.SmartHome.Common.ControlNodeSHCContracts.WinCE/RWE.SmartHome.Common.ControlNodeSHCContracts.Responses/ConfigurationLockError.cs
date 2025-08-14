namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public enum ConfigurationLockError
{
	Unknown,
	BlockedByOtherUser,
	ConfigurationOutOfDate,
	TransactionScopeBusy,
	ShcOfflineSwitchSet,
	NoNetworkAvailable
}
