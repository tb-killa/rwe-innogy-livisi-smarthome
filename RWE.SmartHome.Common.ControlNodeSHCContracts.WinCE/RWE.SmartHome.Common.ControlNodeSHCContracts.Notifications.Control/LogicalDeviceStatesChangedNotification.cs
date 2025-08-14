using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;

public class LogicalDeviceStatesChangedNotification : BaseNotification
{
	public List<LogicalDeviceState> LogicalDeviceStates { get; set; }
}
