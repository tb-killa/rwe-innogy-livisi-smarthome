namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

public enum LogoutNotificationReason
{
	CommunicationChannelTerminated,
	LogoutRequest,
	AutoLogout,
	SessionExpired,
	SecondLoginViaSameChannelContext,
	PerfomSoftwareUpdate,
	RebootShc,
	ForcedLogout,
	FactoryReset
}
