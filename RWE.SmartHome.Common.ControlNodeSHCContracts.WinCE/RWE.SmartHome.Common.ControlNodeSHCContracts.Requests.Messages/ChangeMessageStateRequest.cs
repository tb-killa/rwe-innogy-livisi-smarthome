using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Messages;

public class ChangeMessageStateRequest : BaseRequest
{
	[XmlAttribute]
	public MessageState NewState { get; set; }

	[XmlAttribute]
	public Guid MessageId { get; set; }
}
