using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;

public class InteractionRule : EntityRule<Interaction>
{
	public override ValidationResult Check(Interaction newInteraction, Interaction oldInteraction)
	{
		ValidationResult validationResult = new ValidationResult();
		if (oldInteraction != null && oldInteraction.IsInternal)
		{
			validationResult.Add("Attempting to alter an internal interaction with id: " + oldInteraction.Id);
		}
		if (newInteraction != null && newInteraction.IsInternal)
		{
			validationResult.Add("Attempting to create an internal interaction with id: " + newInteraction.Id);
		}
		return validationResult;
	}
}
