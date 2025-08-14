using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class ApplicationTokenClientLocalOnly : IApplicationTokenClient
{
	public ApplicationsToken GetApplicationToken(string certificateThumbprint)
	{
		return new ApplicationsToken();
	}

	public string GetApplicationTokenHash(string certificateThumbprint)
	{
		return string.Empty;
	}
}
