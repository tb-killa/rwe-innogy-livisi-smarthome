using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class SoftwareUpdateRequest : AuthenticationRequest
{
	[XmlAttribute]
	public ShcUpdateAction Action { get; set; }
}
