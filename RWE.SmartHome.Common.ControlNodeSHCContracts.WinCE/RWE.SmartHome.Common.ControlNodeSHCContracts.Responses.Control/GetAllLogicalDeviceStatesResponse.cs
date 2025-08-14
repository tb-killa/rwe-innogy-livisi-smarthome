using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;

public class GetAllLogicalDeviceStatesResponse : BaseResponse
{
	[XmlAttribute]
	public ControlResult Result { get; set; }

	public List<LogicalDeviceState> States { get; set; }
}
