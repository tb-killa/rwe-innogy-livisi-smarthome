namespace RWE.SmartHome.SHC.CommonFunctionality;

public enum NetworkProblem
{
	NetworkAdapterNotOperational,
	NoDhcpIpAddress,
	NoDhcpDefaultGateway,
	NameResolutionFailed,
	NameResolutionFailedNetworkDown,
	Other
}
