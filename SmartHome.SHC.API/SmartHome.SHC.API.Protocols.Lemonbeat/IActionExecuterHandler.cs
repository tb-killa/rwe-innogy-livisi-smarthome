using SmartHome.SHC.API.Configuration;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface IActionExecuterHandler
{
	ControlMessage TranslateAction(ActionDescription action, ExecutionContext context);
}
