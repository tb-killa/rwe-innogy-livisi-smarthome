using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class GetShcTypeRequest : BaseRequest
{
	public ShcRestriction Restriction { get; set; }
}
