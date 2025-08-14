using System.Collections.Generic;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;

namespace RWE.SmartHome.SHC.BusinessLogic.Persistence;

public class TechnicalConfigurationContainer
{
	public List<TechnicalConfigurationEntity> Entities { get; set; }
}
