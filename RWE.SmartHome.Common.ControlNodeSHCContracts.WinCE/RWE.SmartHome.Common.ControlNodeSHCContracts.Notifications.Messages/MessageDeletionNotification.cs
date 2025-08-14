using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;

public class MessageDeletionNotification : BaseNotification
{
	[XmlAttribute]
	public Guid MessageId { get; set; }
}
