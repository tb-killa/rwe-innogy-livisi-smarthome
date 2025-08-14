using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

internal interface IRuleConverter
{
	SmartHome.Common.API.Entities.Entities.Rule FromSmartHomeRule(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule shRule);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule ToSmartHomeRule(SmartHome.Common.API.Entities.Entities.Rule apiRule, Guid interactionId);
}
