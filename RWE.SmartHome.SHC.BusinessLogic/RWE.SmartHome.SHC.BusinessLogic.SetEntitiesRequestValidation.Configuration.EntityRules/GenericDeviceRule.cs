using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityMatches;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;

public abstract class GenericDeviceRule<T> : EntityRule<T> where T : Entity
{
	public List<PropertyRule> AttributesRules { get; set; }

	public List<PropertyRule> PropertiesRules { get; set; }

	public EntityMatch<T> Condition { get; set; }

	public override ValidationResult Check(T entity, T repositoryEntity)
	{
		return Check(entity, repositoryEntity, checkRanges: false);
	}

	public ValidationResult CheckValuesRange(T entity, T repositoryEntity)
	{
		return Check(entity, repositoryEntity, checkRanges: true);
	}

	private ValidationResult Check(T entity, T repositoryEntity, bool checkRanges)
	{
		ValidationResult validationResult = new ValidationResult();
		if (repositoryEntity == null)
		{
			return validationResult;
		}
		if (entity == null)
		{
			validationResult.Add("Null entity");
		}
		else if (Condition.Match(repositoryEntity))
		{
			validationResult.Add(SpecificCheck(entity, repositoryEntity, checkRanges), "error checking " + entity.Id);
		}
		return validationResult;
	}

	protected abstract IEnumerable<Property> GetAttributesWrapper(T entity);

	protected abstract IEnumerable<Property> GetPropertiesWrapper(T entity);

	private ValidationResult SpecificCheck(T entity, T repositoryEntity, bool checkRanges)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(CheckPropertiesList(AttributesRules, GetAttributesWrapper(entity), GetAttributesWrapper(repositoryEntity), checkRanges));
		validationResult.Add(CheckPropertiesList(PropertiesRules, GetPropertiesWrapper(entity), GetPropertiesWrapper(repositoryEntity), checkRanges));
		return validationResult;
	}

	private ValidationResult CheckPropertiesList(IEnumerable<PropertyRule> propertiesRules, IEnumerable<Property> props, IEnumerable<Property> repoProps, bool checkRanges)
	{
		ValidationResult validationResult = new ValidationResult();
		if (propertiesRules == null)
		{
			return validationResult;
		}
		foreach (PropertyRule propertiesRule in propertiesRules)
		{
			if (propertiesRule.Type == PropertyTypes.StringProperty)
			{
				validationResult.Add(ValidateProperty(propertiesRule, (string name) => GetProperty<StringProperty>(props, name), (string name) => GetProperty<StringProperty>(repoProps, name), checkRanges));
			}
			if (propertiesRule.Type == PropertyTypes.NumericProperty)
			{
				validationResult.Add(ValidateProperty(propertiesRule, (string name) => GetProperty<NumericProperty>(props, name), (string name) => GetProperty<NumericProperty>(repoProps, name), checkRanges));
			}
			if (propertiesRule.Type == PropertyTypes.DateTimeProperty)
			{
				validationResult.Add(ValidateProperty(propertiesRule, (string name) => GetProperty<DateTimeProperty>(props, name), (string name) => GetProperty<DateTimeProperty>(repoProps, name), checkRanges));
			}
			if (propertiesRule.Type == PropertyTypes.BooleanProperty)
			{
				validationResult.Add(ValidateProperty(propertiesRule, (string name) => GetProperty<BooleanProperty>(props, name), (string name) => GetProperty<BooleanProperty>(repoProps, name), checkRanges));
			}
			if (propertiesRule.Type == PropertyTypes.GuidList)
			{
				validationResult.Add(ValidateProperty(propertiesRule, (string name) => GetProperty<GuidListProperty>(props, name), (string name) => GetProperty<GuidListProperty>(repoProps, name), checkRanges));
			}
		}
		return validationResult;
	}

	private ValidationResult ValidateProperty<propertyT>(PropertyRule propertyRule, Func<string, propertyT> propFn, Func<string, propertyT> repoPropFn, bool checkRanges) where propertyT : Property
	{
		ValidationResult validationResult = new ValidationResult();
		propertyT property = propFn(propertyRule.Name);
		propertyT repositoryProperty = repoPropFn(propertyRule.Name);
		foreach (ValueConstraint valueConstraint in propertyRule.ValueConstraints)
		{
			if (!checkRanges || (!(valueConstraint is ReadOnly) && !(valueConstraint is Required)))
			{
				validationResult.Add(valueConstraint.IsValid(property, repositoryProperty), propertyRule.Name + " not validated");
			}
		}
		return validationResult;
	}

	private propertyT GetProperty<propertyT>(IEnumerable<Property> properties, string name) where propertyT : Property, new()
	{
		Property property = properties.FirstOrDefault((Property p) => p.Name == name);
		return property as propertyT;
	}
}
