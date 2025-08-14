using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSecurityNotifications;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class ShcSecurityNotificationsStateResponse : BaseResponse
{
	public ShcSecurityNotificationsSettings SecurityNotificationsSettings { get; set; }
}
