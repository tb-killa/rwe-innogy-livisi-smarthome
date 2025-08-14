using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;

public class StringNotEmpty : ValueConstraint<StringProperty>
{
	protected override ValidationResult IsValid(StringProperty property, StringProperty repositoryProperty)
	{
		ValidationResult validationResult = new ValidationResult();
		if (property == null)
		{
			validationResult.Add("property is null");
		}
		else if (string.IsNullOrEmpty(property.Value))
		{
			validationResult.Add("value is null or empty");
		}
		return validationResult;
	}
}
