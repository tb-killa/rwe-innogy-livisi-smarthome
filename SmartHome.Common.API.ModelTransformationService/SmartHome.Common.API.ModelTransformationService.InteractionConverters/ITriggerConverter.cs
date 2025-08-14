using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

internal interface ITriggerConverter
{
	SmartHome.Common.API.Entities.Entities.Trigger FromSmartHomeTrigger(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger shTrigger);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger ToSmartHomeTrigger(SmartHome.Common.API.Entities.Entities.Trigger apiTrigger);
}
