using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.FamilyAndFriends;

public class GetHomeStateResponse : BaseResponse
{
	[XmlAttribute]
	public ControlResult Result { get; set; }

	public HomeState HomeState { get; set; }
}
