using System.Collections.Generic;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class ActivateDeviceDiscoveryRequest : BaseRequest
{
	[XmlElement("AppIds")]
	public List<string> AppIds { get; set; }
}
