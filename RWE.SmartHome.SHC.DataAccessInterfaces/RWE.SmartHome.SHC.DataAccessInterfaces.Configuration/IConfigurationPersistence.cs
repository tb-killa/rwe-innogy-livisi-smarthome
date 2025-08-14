using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.Configuration;

public interface IConfigurationPersistence : IEntityPersistence, IConfigurationSettingsPersistence, IService
{
}
