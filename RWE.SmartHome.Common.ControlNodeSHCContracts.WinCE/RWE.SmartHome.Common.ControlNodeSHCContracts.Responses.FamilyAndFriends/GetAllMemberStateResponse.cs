using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.FamilyAndFriends;

public class GetAllMemberStateResponse : BaseResponse
{
	[XmlAttribute]
	public ControlResult Result { get; set; }

	public List<MemberState> MemberStates { get; set; }
}
