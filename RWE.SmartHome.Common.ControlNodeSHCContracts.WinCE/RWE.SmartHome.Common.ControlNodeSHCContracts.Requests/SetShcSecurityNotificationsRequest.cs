using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSecurityNotifications;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class SetShcSecurityNotificationsRequest : BaseRequest
{
	public ShcSecurityNotificationsSettings SecurityNotificationsSettings { get; set; }
}
