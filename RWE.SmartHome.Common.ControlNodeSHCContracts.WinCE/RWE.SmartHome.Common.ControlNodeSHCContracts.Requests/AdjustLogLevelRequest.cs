namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class AdjustLogLevelRequest : AuthenticationRequest
{
	public int ExpireAfterMinutes { get; set; }

	public string Reason { get; set; }

	public string Requester { get; set; }
}
