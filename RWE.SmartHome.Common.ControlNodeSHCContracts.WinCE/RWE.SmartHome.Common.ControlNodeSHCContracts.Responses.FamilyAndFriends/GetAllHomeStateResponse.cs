using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.FamilyAndFriends;

public class GetAllHomeStateResponse : BaseResponse
{
	[XmlAttribute]
	public ControlResult Result { get; set; }

	public List<HomeState> HomeStates { get; set; }
}
