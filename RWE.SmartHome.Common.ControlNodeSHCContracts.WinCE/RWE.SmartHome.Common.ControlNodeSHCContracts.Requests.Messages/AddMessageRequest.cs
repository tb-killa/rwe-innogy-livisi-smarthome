using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Messages;

public class AddMessageRequest : AuthenticationRequest
{
	[XmlAttribute]
	public RecipientType RecipientType { get; set; }

	public List<Guid> Recipients { get; set; }

	public Message Message { get; set; }
}
