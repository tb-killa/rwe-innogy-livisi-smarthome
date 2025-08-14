using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public static class PropertyListExtensions
{
	public static T Get<T>(this List<Property> properties, string propertyName) where T : Property
	{
		return properties.FirstOrDefault((Property p) => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)) as T;
	}

	public static void Delete(this List<Property> properties, string propertyName)
	{
		properties.RemoveAll((Property p) => p.Name == propertyName);
	}

	public static string GetStringValue(this List<Property> properties, string propertyName)
	{
		return properties.Get<StringProperty>(propertyName)?.Value;
	}

	public static void SetString(this List<Property> properties, string propertyName, string value)
	{
		StringProperty stringProperty = properties.Get<StringProperty>(propertyName);
		if (stringProperty == null)
		{
			StringProperty stringProperty2 = new StringProperty();
			stringProperty2.Name = propertyName;
			stringProperty = stringProperty2;
			properties.Add(stringProperty);
		}
		stringProperty.Value = value;
	}

	public static decimal? GetDecimalValue(this List<Property> properties, string propertyName)
	{
		return properties.Get<NumericProperty>(propertyName)?.Value;
	}

	public static void SetDecimal(this List<Property> properties, string propertyName, decimal? value)
	{
		NumericProperty numericProperty = properties.Get<NumericProperty>(propertyName);
		if (numericProperty == null)
		{
			NumericProperty numericProperty2 = new NumericProperty();
			numericProperty2.Name = propertyName;
			numericProperty = numericProperty2;
			properties.Add(numericProperty);
		}
		numericProperty.Value = value;
	}

	public static DateTime? GetDateTimeValue(this List<Property> properties, string propertyName)
	{
		return properties.Get<DateTimeProperty>(propertyName)?.Value;
	}

	public static void SetDateTime(this List<Property> properties, string propertyName, DateTime? value)
	{
		DateTimeProperty dateTimeProperty = properties.Get<DateTimeProperty>(propertyName);
		if (dateTimeProperty == null)
		{
			DateTimeProperty dateTimeProperty2 = new DateTimeProperty();
			dateTimeProperty2.Name = propertyName;
			dateTimeProperty = dateTimeProperty2;
			properties.Add(dateTimeProperty);
		}
		dateTimeProperty.Value = value;
	}

	public static bool? GetBooleanValue(this List<Property> properties, string propertyName)
	{
		return properties.Get<BooleanProperty>(propertyName)?.Value;
	}
}
