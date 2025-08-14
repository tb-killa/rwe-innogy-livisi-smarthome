using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.RequestValidation;

public class EntityValidations
{
	private class PropertyValidation
	{
		public string Name { get; set; }

		public Type PropertyType { get; set; }

		public PropertyRequired IsRequired { get; set; }
	}

	private const string PropertyNameDoesntExist = "PROPERTY_NAME_DOESN'T_EXIST={0}";

	private const string EnumValueIsNotCorrect = "ENUM_VALUE_NOT_CORRECT={0}_VALUES={1}";

	private List<PropertyValidation> propertyValidations = new List<PropertyValidation>();

	protected void AddValidation(string propertyName, Type propertyType, PropertyRequired required)
	{
		propertyValidations.Add(new PropertyValidation
		{
			IsRequired = required,
			Name = propertyName,
			PropertyType = propertyType
		});
	}

	protected ValidationResult Validate<T>(T param, params Func<T, ValidationResult>[] validationCallbacks)
	{
		ValidationResult validationResult = new ValidationResult();
		for (int i = 0; i < validationCallbacks.Length; i++)
		{
			if (!validationResult.Valid)
			{
				break;
			}
			validationResult.Add(validationCallbacks[i](param));
		}
		return validationResult;
	}

	protected ValidationResult ValidateTypeProperties(IEnumerable<Property> properties)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (PropertyValidation propertyValidation in propertyValidations)
		{
			if (!ExistProperty(properties, propertyValidation))
			{
				string error = $"PROPERTY_NAME_DOESN'T_EXIST={propertyValidation.Name}";
				validationResult.Add(error);
			}
		}
		return validationResult;
	}

	protected ValidationResult ValidateEnumValue(string[] enumValues, string value)
	{
		ValidationResult validationResult = new ValidationResult();
		if (Array.IndexOf(enumValues, value) < 0)
		{
			string error = string.Format("ENUM_VALUE_NOT_CORRECT={0}_VALUES={1}", value, string.Join(";", enumValues));
			validationResult.Add(error);
		}
		return validationResult;
	}

	private bool ExistProperty(IEnumerable<Property> properties, PropertyValidation propertyValidation)
	{
		bool result = false;
		if (properties != null)
		{
			Property property = properties.FirstOrDefault((Property m) => propertyValidation.Name.Equals(m.Name));
			result = ((property != null) ? ((object)property.GetType() == propertyValidation.PropertyType) : (propertyValidation.IsRequired == PropertyRequired.Optional));
		}
		return result;
	}
}
