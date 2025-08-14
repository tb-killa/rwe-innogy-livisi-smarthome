using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class ProbeShcResponse : BaseResponse
{
	[XmlElement]
	public ShcInfo ShcInformation { get; set; }
}
