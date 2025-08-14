using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;

public class NewMessageNotification : BaseNotification
{
	public Message Message { get; set; }
}
