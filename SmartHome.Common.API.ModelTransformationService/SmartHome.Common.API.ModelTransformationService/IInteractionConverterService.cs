using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IInteractionConverterService
{
	SmartHome.Common.API.Entities.Entities.Interaction FromSmartHomeInteraction(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction shInteraction);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction ToSmartHomeInteraction(SmartHome.Common.API.Entities.Entities.Interaction apiInteraction);
}
