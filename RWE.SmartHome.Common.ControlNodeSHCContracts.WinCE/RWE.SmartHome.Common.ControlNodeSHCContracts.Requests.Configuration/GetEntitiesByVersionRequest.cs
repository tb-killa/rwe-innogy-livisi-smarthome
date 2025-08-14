using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class GetEntitiesByVersionRequest : BaseRequest
{
	[XmlAttribute]
	public int ChangesSinceVersion { get; set; }
}
