using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;

public class Required : ValueConstraint<Property>
{
	protected override ValidationResult IsValid(Property property, Property repositoryProperty)
	{
		ValidationResult validationResult = new ValidationResult();
		if (property == null)
		{
			validationResult.Add("Required property is missing.");
		}
		return validationResult;
	}
}
