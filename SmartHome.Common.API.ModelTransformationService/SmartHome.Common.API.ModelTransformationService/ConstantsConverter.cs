using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using SmartHome.Common.API.Entities.Enumerations;

namespace SmartHome.Common.API.ModelTransformationService;

internal class ConstantsConverter : IConstantsConverter
{
	public ConnectionTerminationReason TerminationReasonFromNotification(LogoutNotificationReason reason)
	{
		return reason switch
		{
			LogoutNotificationReason.LogoutRequest => ConnectionTerminationReason.UserLoggedOut, 
			LogoutNotificationReason.PerfomSoftwareUpdate => ConnectionTerminationReason.SoftwareUpdate, 
			LogoutNotificationReason.SessionExpired => ConnectionTerminationReason.SessionExpired, 
			LogoutNotificationReason.RebootShc => ConnectionTerminationReason.DeviceReboot, 
			LogoutNotificationReason.ForcedLogout => ConnectionTerminationReason.TooManyConnections, 
			_ => ConnectionTerminationReason.ConnectionLost, 
		};
	}
}
