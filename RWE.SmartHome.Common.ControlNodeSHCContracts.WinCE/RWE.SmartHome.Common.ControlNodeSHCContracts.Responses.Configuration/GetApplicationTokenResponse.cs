using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;

public class GetApplicationTokenResponse : BaseResponse
{
	public ApplicationsToken Token { get; set; }
}
