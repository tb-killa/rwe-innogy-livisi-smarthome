using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

internal interface ICustomTriggerConverter
{
	SmartHome.Common.API.Entities.Entities.CustomTrigger FromSmartHomeCustomTrigger(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger shTrigger);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger ToSmartHomeCustomTrigger(SmartHome.Common.API.Entities.Entities.CustomTrigger apiTrigger);
}
