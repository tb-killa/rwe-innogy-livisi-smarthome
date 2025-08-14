using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using SmartHome.Common.API.Entities.Enumerations;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IConstantsConverter
{
	ConnectionTerminationReason TerminationReasonFromNotification(LogoutNotificationReason reason);
}
