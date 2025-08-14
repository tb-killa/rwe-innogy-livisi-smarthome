using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class ReconnectRequest : BaseRequest
{
	[XmlAttribute]
	public string OldUri { get; set; }

	[XmlAttribute]
	public string NewUri { get; set; }
}
