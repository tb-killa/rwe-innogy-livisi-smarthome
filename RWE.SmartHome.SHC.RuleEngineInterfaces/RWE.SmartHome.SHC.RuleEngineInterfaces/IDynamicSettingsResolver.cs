using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.RuleEngineInterfaces;

public interface IDynamicSettingsResolver
{
	IEnumerable<Property> GetTargetState(IEnumerable<Parameter> action);

	IEnumerable<Property> GetTargetTypes(IEnumerable<Parameter> action);
}
