using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;

public class ReadOnly : ValueConstraint<Property>
{
	private const string ErrorMessage = "Attempted to modify readonly value setting value ";

	protected override ValidationResult IsValid(Property property, Property repositoryProperty)
	{
		ValidationResult validationResult = new ValidationResult();
		if (property != null || repositoryProperty != null)
		{
			if (property == null || repositoryProperty == null)
			{
				validationResult.Add("Attempted to modify readonly value setting value null");
				Log.DebugFormat(Module.BusinessLogic, "ReadOnly", false, "Null property checked requested:{0}, existing:{1}", (property ?? new StringProperty
				{
					Value = "null"
				}).GetValueAsString(), (repositoryProperty ?? new StringProperty
				{
					Value = "null"
				}).GetValueAsString());
			}
			else
			{
				validationResult.Add(property.Equals(repositoryProperty), "Attempted to modify readonly value setting value " + property.GetValueAsString());
			}
		}
		return validationResult;
	}
}
