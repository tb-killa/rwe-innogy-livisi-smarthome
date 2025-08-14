using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class ActionResponse : BaseResponse
{
	[XmlAttribute]
	public string ApplicationId { get; set; }

	public List<Property> Payload { get; set; }
}
