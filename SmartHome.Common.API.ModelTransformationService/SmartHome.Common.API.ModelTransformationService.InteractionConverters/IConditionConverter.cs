using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

internal interface IConditionConverter
{
	SmartHome.Common.API.Entities.Entities.Condition FromSmartHomeCondition(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition shCondition);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition ToSmartHomeCondition(SmartHome.Common.API.Entities.Entities.Condition apiCondition);
}
