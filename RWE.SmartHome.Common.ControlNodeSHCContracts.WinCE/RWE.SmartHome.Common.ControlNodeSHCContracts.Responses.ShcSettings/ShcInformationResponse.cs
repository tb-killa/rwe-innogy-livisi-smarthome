using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSettings;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.ShcSettings;

public class ShcInformationResponse : BaseResponse
{
	public ShcInformation Information { get; set; }
}
