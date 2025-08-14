using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class GetLoggedInUsersResponse : BaseResponse
{
	public List<UserInfo> Users { get; set; }
}
