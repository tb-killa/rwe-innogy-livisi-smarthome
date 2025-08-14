using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Messages;

public class DeleteMessageRequest : BaseRequest
{
	[XmlAttribute]
	public Guid MessageId { get; set; }
}
