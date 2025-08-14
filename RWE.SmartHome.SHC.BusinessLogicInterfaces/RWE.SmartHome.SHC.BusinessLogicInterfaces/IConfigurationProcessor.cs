using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public interface IConfigurationProcessor : IService
{
	IRulesRepository RulesRepository { set; }

	void RegisterConfigurationValidator(IConfigurationValidator validator);

	void UnregisterConfigurationValidator(IConfigurationValidator validator);

	void ProcessNewConfiguration(RepositoryUpdateContextData updateContextData);
}
