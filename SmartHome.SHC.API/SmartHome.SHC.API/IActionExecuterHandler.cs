using SmartHome.SHC.API.Configuration;

namespace SmartHome.SHC.API;

public interface IActionExecuterHandler
{
	ExecutionResult ExecuteAction(ActionDescription action, ExecutionContext context);
}
