using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;

public class DeviceDiscoveryFailureNotification : BaseNotification
{
	public List<string> AppIds { get; set; }
}
