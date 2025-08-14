using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.FamilyAndFriends;

public class GetHomeStateRequest : BaseRequest
{
	public Guid HomeId { get; set; }
}
