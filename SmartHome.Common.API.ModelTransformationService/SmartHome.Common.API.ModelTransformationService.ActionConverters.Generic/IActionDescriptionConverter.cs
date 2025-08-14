using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.Generic;

internal interface IActionDescriptionConverter
{
	Action FromSmartHomeActionDescription(ActionDescription shActionDescription);

	ActionDescription ToSmartHomeActionDescription(Action apiActionDescription);
}
