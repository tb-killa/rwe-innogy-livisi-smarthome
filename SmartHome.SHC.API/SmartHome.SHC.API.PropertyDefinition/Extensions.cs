using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.SHC.API.PropertyDefinition;

public static class Extensions
{
	public static T Get<T>(this IEnumerable<Property> propertyList, string propertyName) where T : class, Property
	{
		return propertyList.Get(propertyName) as T;
	}

	public static Property Get(this IEnumerable<Property> propertyList, string propertyName)
	{
		return propertyList?.FirstOrDefault((Property p) => p.Name == propertyName);
	}

	public static void Delete(this List<Property> properties, string propertyName)
	{
		properties.RemoveAll((Property p) => p.Name == propertyName);
	}

	public static string GetStringValue(this IEnumerable<Property> properties, string propertyName)
	{
		StringProperty stringProperty = properties.Get<StringProperty>(propertyName);
		return (stringProperty != null) ? stringProperty.Value : null;
	}

	public static decimal? GetDecimalValue(this IEnumerable<Property> properties, string propertyName)
	{
		NumericProperty numericProperty = properties.Get<NumericProperty>(propertyName);
		return (numericProperty != null) ? numericProperty.Value : ((decimal?)null);
	}

	public static DateTime? GetDateTimeValue(this IEnumerable<Property> properties, string propertyName)
	{
		DateTimeProperty dateTimeProperty = properties.Get<DateTimeProperty>(propertyName);
		return (dateTimeProperty != null) ? dateTimeProperty.Value : ((DateTime?)null);
	}

	public static bool? GetBooleanValue(this IEnumerable<Property> properties, string propertyName)
	{
		BooleanProperty booleanProperty = properties.Get<BooleanProperty>(propertyName);
		return (booleanProperty != null) ? booleanProperty.Value : ((bool?)null);
	}
}
