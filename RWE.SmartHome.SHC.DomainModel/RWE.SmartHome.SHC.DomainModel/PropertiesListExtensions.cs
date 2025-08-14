using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.DomainModel;

public static class PropertiesListExtensions
{
	private static T GetOrCreatePropertyByName<T>(this List<Property> list, string propertyName) where T : Property, new()
	{
		Property property = list.FirstOrDefault((Property x) => x.Name.Equals(propertyName));
		T val = null;
		if (property != null)
		{
			val = property as T;
			if (val == null)
			{
				list.Remove(property);
			}
		}
		if (val == null)
		{
			val = new T();
			string name = propertyName;
			val.Name = name;
			list.Add(val);
		}
		return val;
	}

	public static void UpdateStringProperty(this List<Property> list, string propertyName, string newValue, Action onNewValue, bool supportTimestamp)
	{
		StringProperty orCreatePropertyByName = list.GetOrCreatePropertyByName<StringProperty>(propertyName);
		if (orCreatePropertyByName.Value != newValue)
		{
			orCreatePropertyByName.Value = newValue;
			if (supportTimestamp)
			{
				orCreatePropertyByName.UpdateTimestamp = ShcDateTime.UtcNow;
			}
			onNewValue?.Invoke();
		}
	}

	public static void UpdateNumericProperty(this List<Property> list, string propertyName, decimal? newValue, Action onNewValue, bool supportTimestamp)
	{
		NumericProperty orCreatePropertyByName = list.GetOrCreatePropertyByName<NumericProperty>(propertyName);
		if (orCreatePropertyByName.Value != newValue)
		{
			orCreatePropertyByName.Value = newValue;
			if (supportTimestamp)
			{
				orCreatePropertyByName.UpdateTimestamp = ShcDateTime.UtcNow;
			}
			onNewValue?.Invoke();
		}
	}

	public static void UpdateBooleanProperty(this List<Property> list, string propertyName, bool? newValue, Action onNewValue, bool supportTimestamp)
	{
		BooleanProperty orCreatePropertyByName = list.GetOrCreatePropertyByName<BooleanProperty>(propertyName);
		if (orCreatePropertyByName.Value != newValue)
		{
			orCreatePropertyByName.Value = newValue;
			if (supportTimestamp)
			{
				orCreatePropertyByName.UpdateTimestamp = ShcDateTime.UtcNow;
			}
			onNewValue?.Invoke();
		}
	}

	public static void UpdateDateTimeProperty(this List<Property> list, string propertyName, DateTime? newValue, Action onNewValue, bool supportTimestamp)
	{
		DateTimeProperty orCreatePropertyByName = list.GetOrCreatePropertyByName<DateTimeProperty>(propertyName);
		if (orCreatePropertyByName.Value != newValue)
		{
			orCreatePropertyByName.Value = newValue;
			if (supportTimestamp)
			{
				orCreatePropertyByName.UpdateTimestamp = ShcDateTime.UtcNow;
			}
			onNewValue?.Invoke();
		}
	}
}
