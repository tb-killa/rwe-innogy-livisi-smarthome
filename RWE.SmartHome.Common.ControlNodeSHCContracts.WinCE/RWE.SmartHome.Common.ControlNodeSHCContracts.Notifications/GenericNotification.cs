using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

public class GenericNotification : BaseNotification
{
	[Obsolete("This property is obsolete. Please use the BaseNotification.Namespace property instead.")]
	[XmlAttribute]
	public string ApplicationId { get; set; }

	[XmlAttribute]
	public string ApplicationVersion { get; set; }

	[XmlAttribute]
	public string EventType { get; set; }

	[XmlAttribute]
	public string EntityId { get; set; }

	[XmlAttribute]
	public EntityType EntityType { get; set; }

	public PropertyBag Data { get; set; }
}
