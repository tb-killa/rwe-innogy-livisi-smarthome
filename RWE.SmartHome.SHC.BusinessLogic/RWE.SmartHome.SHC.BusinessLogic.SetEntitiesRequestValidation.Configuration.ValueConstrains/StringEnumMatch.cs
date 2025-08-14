using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;

public class StringEnumMatch : ValueConstraint<StringProperty>
{
	public string[] Values { get; set; }

	protected override ValidationResult IsValid(StringProperty property, StringProperty repositoryProperty)
	{
		ValidationResult validationResult = new ValidationResult();
		if (property == null)
		{
			validationResult.Add("property is null");
		}
		else
		{
			if (Values == null && property.Value != null)
			{
				validationResult.Add("value should be null");
			}
			if (Values != null && property.Value != null && !Values.Contains(property.Value))
			{
				validationResult.Add("value not in list");
			}
		}
		return validationResult;
	}
}
