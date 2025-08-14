using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.FamilyAndFriends;

public class GetMemberStateRequest : BaseRequest
{
	public Guid MemberId { get; set; }
}
