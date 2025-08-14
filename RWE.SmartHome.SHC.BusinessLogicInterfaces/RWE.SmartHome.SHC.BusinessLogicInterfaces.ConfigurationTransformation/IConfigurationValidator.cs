using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;

public interface IConfigurationValidator
{
	IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData);
}
