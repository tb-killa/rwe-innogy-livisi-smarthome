using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Control;

public class GetLogicalDeviceStateRequest : BaseRequest
{
	[XmlAttribute]
	public Guid LogicalDeviceId { get; set; }
}
