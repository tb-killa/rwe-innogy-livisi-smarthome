using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;

public class NumericRangeStep : ValueConstraint<NumericProperty>
{
	public decimal Min { get; set; }

	public decimal Max { get; set; }

	public decimal Step { get; set; }

	protected override ValidationResult IsValid(NumericProperty property, NumericProperty repositoryProperty)
	{
		ValidationResult validationResult = new ValidationResult();
		if (property == null)
		{
			validationResult.Add("property is null");
		}
		else
		{
			decimal? value = property.Value;
			decimal min = Min;
			if (!(value.GetValueOrDefault() < min) || !value.HasValue)
			{
				decimal? value2 = property.Value;
				decimal max = Max;
				if (!(value2.GetValueOrDefault() > max) || !value2.HasValue)
				{
					decimal? num = (property.Value - (decimal?)Min) % (decimal?)Step;
					if (!(num.GetValueOrDefault() != 0m) && num.HasValue)
					{
						goto IL_010d;
					}
				}
			}
			validationResult.Add("value out of range");
		}
		goto IL_010d;
		IL_010d:
		return validationResult;
	}
}
