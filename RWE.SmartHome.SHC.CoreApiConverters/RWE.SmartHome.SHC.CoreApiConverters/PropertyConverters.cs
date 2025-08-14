using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class PropertyConverters
{
	public static global::SmartHome.SHC.API.PropertyDefinition.Property ToApiProperty(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property coreProperty)
	{
		if (coreProperty is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty numericProperty)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.NumericProperty(numericProperty.Name, numericProperty.Value);
		}
		if (coreProperty is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty stringProperty)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.StringProperty(stringProperty.Name, stringProperty.Value);
		}
		if (coreProperty is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty booleanProperty)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty(booleanProperty.Name, booleanProperty.Value);
		}
		if (coreProperty is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty dateTimeProperty)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty(dateTimeProperty.Name, dateTimeProperty.Value);
		}
		throw new ArgumentException("Input does not have an expected type. Input type is: " + coreProperty.GetType().FullName);
	}

	public static global::SmartHome.SHC.API.PropertyDefinition.Property ToApiProperty(this Parameter coreParameter)
	{
		if (coreParameter.Value == null)
		{
			return null;
		}
		if (coreParameter.Value is ConstantNumericBinding constantNumericBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.NumericProperty(coreParameter.Name, constantNumericBinding.Value);
		}
		if (coreParameter.Value is ConstantStringBinding constantStringBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.StringProperty(coreParameter.Name, constantStringBinding.Value);
		}
		if (coreParameter.Value is ConstantBooleanBinding constantBooleanBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty(coreParameter.Name, constantBooleanBinding.Value);
		}
		if (coreParameter.Value is ConstantDateTimeBinding constantDateTimeBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty(coreParameter.Name, constantDateTimeBinding.Value);
		}
		throw new ArgumentException("Input core parameter does not have an expected type. Input type is: " + coreParameter.GetType().FullName);
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToCoreProperty(this Parameter coreParameter)
	{
		if (coreParameter.Value == null)
		{
			return null;
		}
		if (coreParameter.Value is ConstantNumericBinding constantNumericBinding)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty numericProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty();
			numericProperty.Name = coreParameter.Name;
			numericProperty.Value = constantNumericBinding.Value;
			numericProperty.UpdateTimestamp = DateTime.UtcNow;
			return numericProperty;
		}
		if (coreParameter.Value is ConstantStringBinding constantStringBinding)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty stringProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty(coreParameter.Name, constantStringBinding.Value);
			stringProperty.UpdateTimestamp = DateTime.UtcNow;
			return stringProperty;
		}
		if (coreParameter.Value is ConstantBooleanBinding constantBooleanBinding)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty booleanProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty();
			booleanProperty.Name = coreParameter.Name;
			booleanProperty.Value = constantBooleanBinding.Value;
			booleanProperty.UpdateTimestamp = DateTime.UtcNow;
			return booleanProperty;
		}
		if (coreParameter.Value is ConstantDateTimeBinding constantDateTimeBinding)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty dateTimeProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty();
			dateTimeProperty.Name = coreParameter.Name;
			dateTimeProperty.Value = constantDateTimeBinding.Value;
			dateTimeProperty.UpdateTimestamp = DateTime.UtcNow;
			return dateTimeProperty;
		}
		throw new ArgumentException("Input core parameter does not have an expected type. Input type is: " + coreParameter.GetType().FullName);
	}

	public static Parameter ToCoreParameter(this global::SmartHome.SHC.API.PropertyDefinition.Property apiProperty)
	{
		global::SmartHome.SHC.API.PropertyDefinition.NumericProperty numericProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.NumericProperty;
		if (numericProperty != null)
		{
			Parameter parameter = new Parameter();
			parameter.Name = numericProperty.Name;
			parameter.Value = new ConstantNumericBinding
			{
				Value = numericProperty.Value.GetValueOrDefault()
			};
			return parameter;
		}
		global::SmartHome.SHC.API.PropertyDefinition.StringProperty stringProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.StringProperty;
		if (stringProperty != null)
		{
			Parameter parameter2 = new Parameter();
			parameter2.Name = stringProperty.Name;
			parameter2.Value = new ConstantStringBinding
			{
				Value = stringProperty.Value
			};
			return parameter2;
		}
		global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty booleanProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty;
		if (booleanProperty != null)
		{
			Parameter parameter3 = new Parameter();
			parameter3.Name = booleanProperty.Name;
			parameter3.Value = new ConstantBooleanBinding
			{
				Value = (booleanProperty.Value == true)
			};
			return parameter3;
		}
		global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty dateTimeProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty;
		if (dateTimeProperty != null)
		{
			Parameter parameter4 = new Parameter();
			parameter4.Name = dateTimeProperty.Name;
			parameter4.Value = new ConstantDateTimeBinding
			{
				Value = dateTimeProperty.Value.GetValueOrDefault()
			};
			return parameter4;
		}
		throw new ArgumentException("Input API property does not have an expected type. Input type is: " + apiProperty.GetType().FullName);
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToCoreProperty(this global::SmartHome.SHC.API.PropertyDefinition.Property apiProperty, bool includeTimestamp)
	{
		global::SmartHome.SHC.API.PropertyDefinition.NumericProperty numericProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.NumericProperty;
		if (numericProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty numericProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty();
			numericProperty2.Name = numericProperty.Name;
			numericProperty2.Value = numericProperty.Value;
			numericProperty2.UpdateTimestamp = (includeTimestamp ? new DateTime?(DateTime.UtcNow) : ((DateTime?)null));
			return numericProperty2;
		}
		global::SmartHome.SHC.API.PropertyDefinition.StringProperty stringProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.StringProperty;
		if (stringProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty stringProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty();
			stringProperty2.Name = stringProperty.Name;
			stringProperty2.Value = stringProperty.Value;
			stringProperty2.UpdateTimestamp = (includeTimestamp ? new DateTime?(DateTime.UtcNow) : ((DateTime?)null));
			return stringProperty2;
		}
		global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty booleanProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty;
		if (booleanProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty booleanProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty();
			booleanProperty2.Name = booleanProperty.Name;
			booleanProperty2.Value = booleanProperty.Value;
			booleanProperty2.UpdateTimestamp = (includeTimestamp ? new DateTime?(DateTime.UtcNow) : ((DateTime?)null));
			return booleanProperty2;
		}
		global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty dateTimeProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty;
		if (dateTimeProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty dateTimeProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty();
			dateTimeProperty2.Name = dateTimeProperty.Name;
			dateTimeProperty2.Value = dateTimeProperty.Value;
			dateTimeProperty2.UpdateTimestamp = (includeTimestamp ? new DateTime?(DateTime.UtcNow) : ((DateTime?)null));
			return dateTimeProperty2;
		}
		throw new ArgumentException("Input does not have an expected type. Input type is: " + apiProperty.GetType().FullName);
	}

	public static PropertyBag ToCorePropertyBag(this global::SmartHome.SHC.API.PropertyDefinition.Property[] apiPropertyBag, bool includeTimestamp)
	{
		PropertyBag propertyBag = new PropertyBag();
		propertyBag.Properties = apiPropertyBag.Select((global::SmartHome.SHC.API.PropertyDefinition.Property x) => x.ToCoreProperty(includeTimestamp)).ToList();
		return propertyBag;
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty ToCoreProperty(this global::SmartHome.SHC.API.PropertyDefinition.StringProperty apiProperty)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty stringProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty();
		stringProperty.Name = apiProperty.Name;
		stringProperty.Value = apiProperty.Value;
		stringProperty.UpdateTimestamp = DateTime.UtcNow;
		return stringProperty;
	}

	public static global::SmartHome.SHC.API.PropertyDefinition.StringProperty ToApiProperty(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty coreProperty)
	{
		return new global::SmartHome.SHC.API.PropertyDefinition.StringProperty(coreProperty.Name, coreProperty.Value);
	}
}
