namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class SHCRestartRequest : AuthenticationRequest
{
	public string Reason { get; set; }

	public string Requester { get; set; }
}
