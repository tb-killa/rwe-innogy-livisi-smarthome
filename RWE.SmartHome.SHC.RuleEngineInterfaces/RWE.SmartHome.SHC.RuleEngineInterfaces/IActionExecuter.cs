using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.RuleEngineInterfaces;

public interface IActionExecuter
{
	ExecutionResult Execute(ActionContext executionSource, ActionDescription action);
}
