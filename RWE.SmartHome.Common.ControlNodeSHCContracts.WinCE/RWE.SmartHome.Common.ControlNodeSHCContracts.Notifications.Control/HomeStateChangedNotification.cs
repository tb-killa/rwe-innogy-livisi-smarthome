using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;

public class HomeStateChangedNotification : BaseNotification
{
	public Guid HomeId { get; set; }
}
