using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;

public class GetApplicationTokenHashResponse : BaseResponse
{
	[XmlAttribute]
	public string Hash { get; set; }
}
