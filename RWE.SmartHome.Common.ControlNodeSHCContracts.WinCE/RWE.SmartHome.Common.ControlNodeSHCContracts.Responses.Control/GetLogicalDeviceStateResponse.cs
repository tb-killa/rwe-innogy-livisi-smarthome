using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;

public class GetLogicalDeviceStateResponse : BaseResponse
{
	[XmlAttribute]
	public ControlResult Result { get; set; }

	public LogicalDeviceState State { get; set; }
}
