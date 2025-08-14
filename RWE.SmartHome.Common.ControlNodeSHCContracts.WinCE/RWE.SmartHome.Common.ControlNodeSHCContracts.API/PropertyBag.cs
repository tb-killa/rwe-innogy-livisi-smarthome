using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.API;

public class PropertyBag
{
	public List<Property> Properties { get; set; }

	public PropertyBag()
	{
		Properties = new List<Property>();
	}

	public void SetValue<T>(string propertyName, T value)
	{
		SetValue(propertyName, value, DateTime.UtcNow);
	}

	public void SetValue<T>(string propertyName, T value, DateTime timestamp)
	{
		Property property = Properties.SingleOrDefault((Property p) => p.Name == propertyName);
		if ((object)typeof(T) == typeof(string) || typeof(T).IsEnum)
		{
			if (!(property is StringProperty stringProperty))
			{
				Properties.Add(new StringProperty(propertyName, value.ToString())
				{
					UpdateTimestamp = timestamp
				});
			}
			else
			{
				stringProperty.Value = value.ToString();
				stringProperty.UpdateTimestamp = timestamp;
			}
			return;
		}
		if ((object)typeof(T) == typeof(decimal))
		{
			decimal value2 = (decimal)Convert.ChangeType(value, typeof(decimal), CultureInfo.InvariantCulture);
			if (!(property is NumericProperty numericProperty))
			{
				Properties.Add(new NumericProperty
				{
					Name = propertyName,
					Value = value2,
					UpdateTimestamp = timestamp
				});
			}
			else
			{
				numericProperty.Value = value2;
				numericProperty.UpdateTimestamp = timestamp;
			}
			return;
		}
		if ((object)typeof(T) == typeof(DateTime))
		{
			DateTime value3 = (DateTime)Convert.ChangeType(value, typeof(DateTime), CultureInfo.InvariantCulture);
			if (!(property is DateTimeProperty dateTimeProperty))
			{
				Properties.Add(new DateTimeProperty
				{
					Name = propertyName,
					Value = value3,
					UpdateTimestamp = timestamp
				});
			}
			else
			{
				dateTimeProperty.Value = value3;
				dateTimeProperty.UpdateTimestamp = timestamp;
			}
			return;
		}
		if ((object)typeof(T) == typeof(bool))
		{
			bool value4 = (bool)Convert.ChangeType(value, typeof(bool), CultureInfo.InvariantCulture);
			if (!(property is BooleanProperty booleanProperty))
			{
				Properties.Add(new BooleanProperty
				{
					Name = propertyName,
					Value = value4,
					UpdateTimestamp = timestamp
				});
			}
			else
			{
				booleanProperty.Value = value4;
				booleanProperty.UpdateTimestamp = timestamp;
			}
			return;
		}
		throw new NotSupportedException("The required property type is not supported: " + typeof(T).Name);
	}

	public T? GetValue<T>(string propertyName) where T : struct
	{
		Property property = Properties.SingleOrDefault((Property p) => p.Name == propertyName);
		if (property == null)
		{
			return null;
		}
		if (typeof(T).IsEnum)
		{
			try
			{
				return (T)Enum.Parse(typeof(T), property.GetValueAsString(), ignoreCase: true);
			}
			catch (ArgumentException)
			{
				return null;
			}
		}
		if ((object)typeof(T) == typeof(decimal))
		{
			if (property is NumericProperty numericProperty)
			{
				return (T)Convert.ChangeType(numericProperty.Value, typeof(decimal), CultureInfo.InvariantCulture);
			}
		}
		else if ((object)typeof(T) == typeof(DateTime))
		{
			if (property is DateTimeProperty dateTimeProperty)
			{
				return (T)Convert.ChangeType(dateTimeProperty.Value, typeof(DateTime), CultureInfo.InvariantCulture);
			}
		}
		else if ((object)typeof(T) == typeof(bool) && property is BooleanProperty booleanProperty)
		{
			return (T)Convert.ChangeType(booleanProperty.Value, typeof(bool), CultureInfo.InvariantCulture);
		}
		return null;
	}

	public string GetValueAsString(string propertyName)
	{
		return Properties.SingleOrDefault((Property p) => p.Name == propertyName)?.GetValueAsString();
	}
}
