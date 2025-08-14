using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;

public class ControlResultResponse : BaseResponse
{
	[XmlAttribute]
	public ControlResult Result { get; set; }
}
