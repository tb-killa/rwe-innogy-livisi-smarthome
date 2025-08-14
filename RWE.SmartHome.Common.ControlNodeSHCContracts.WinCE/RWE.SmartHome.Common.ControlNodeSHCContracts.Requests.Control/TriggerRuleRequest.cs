using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Control;

public class TriggerRuleRequest : BaseRequest
{
	[XmlAttribute]
	public Guid InteractionId { get; set; }
}
