using System.Collections.Generic;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;

public class DeviceDiscoveryStatusChangedNotification : BaseNotification
{
	[XmlAttribute]
	public bool Active { get; set; }

	public List<string> AppIds { get; set; }
}
