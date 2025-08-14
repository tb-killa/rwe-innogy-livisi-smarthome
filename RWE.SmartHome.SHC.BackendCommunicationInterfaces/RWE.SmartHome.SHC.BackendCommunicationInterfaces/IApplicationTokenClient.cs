using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IApplicationTokenClient
{
	ApplicationsToken GetApplicationToken(string certificateThumbprint);

	string GetApplicationTokenHash(string certificateThumbprint);
}
