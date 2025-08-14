using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSettings;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.ShcSettings;

public class SetShcInformationRequest : BaseRequest
{
	public ShcInformation Information { get; set; }
}
