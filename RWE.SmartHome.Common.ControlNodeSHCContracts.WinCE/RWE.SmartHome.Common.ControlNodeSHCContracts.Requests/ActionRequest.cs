using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class ActionRequest : AuthenticationRequest
{
	public ActionDescription ActionDescription { get; set; }
}
