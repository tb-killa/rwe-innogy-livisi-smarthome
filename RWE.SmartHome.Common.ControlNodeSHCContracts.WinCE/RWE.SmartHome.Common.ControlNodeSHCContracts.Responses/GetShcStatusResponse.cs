using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class GetShcStatusResponse : BaseResponse
{
	public ShcStatus Status { get; set; }
}
