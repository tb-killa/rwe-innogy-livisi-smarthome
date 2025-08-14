using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public abstract class AuthenticationRequest : BaseRequest
{
	[XmlAttribute]
	public string UserName { get; set; }

	[XmlAttribute]
	public string Password { get; set; }
}
