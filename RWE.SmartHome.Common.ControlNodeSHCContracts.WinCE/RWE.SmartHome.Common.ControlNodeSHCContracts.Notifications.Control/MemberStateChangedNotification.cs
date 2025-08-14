using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;

public class MemberStateChangedNotification : BaseNotification
{
	public Guid MemberId { get; set; }
}
