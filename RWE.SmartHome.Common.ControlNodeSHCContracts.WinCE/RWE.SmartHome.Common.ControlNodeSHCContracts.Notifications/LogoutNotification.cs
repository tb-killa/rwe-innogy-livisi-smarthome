using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

public class LogoutNotification : BaseNotification
{
	[XmlAttribute]
	public LogoutNotificationReason Reason { get; set; }
}
