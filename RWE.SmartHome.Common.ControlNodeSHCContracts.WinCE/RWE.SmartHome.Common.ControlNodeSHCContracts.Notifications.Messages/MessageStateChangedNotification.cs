using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;

public class MessageStateChangedNotification : BaseNotification
{
	[XmlAttribute]
	public MessageState State { get; set; }

	[XmlAttribute]
	public Guid MessageId { get; set; }
}
