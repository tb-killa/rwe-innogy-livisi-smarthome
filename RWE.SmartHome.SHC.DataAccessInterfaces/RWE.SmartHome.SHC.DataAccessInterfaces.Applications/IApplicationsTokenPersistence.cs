using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

public interface IApplicationsTokenPersistence : IService
{
	void SaveApplicationsToken(ApplicationsToken token);

	ApplicationsToken LoadApplicationsToken();
}
