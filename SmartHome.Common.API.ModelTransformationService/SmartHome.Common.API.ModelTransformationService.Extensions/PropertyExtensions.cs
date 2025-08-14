using System;
using System.Collections.Generic;
using System.Linq;
using ModelTransformations.Helpers;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.Helpers;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.Extensions;

public static class PropertyExtensions
{
	private static readonly ILogger logger = LogManager.Instance.GetLogger(typeof(PropertyExtensions));

	public static T GetPropertyValue<T>(this List<Property> properties, string propertyName)
	{
		return properties.GetPropertyValue<T>(propertyName, isMandatory: true);
	}

	public static T GetPropertyValue<T>(this List<Property> properties, string propertyName, bool isMandatory)
	{
		logger.DebugEnterMethod($"GetPropertyValue<{typeof(T)}>");
		T result = default(T);
		if (properties == null || properties.Count == 0)
		{
			if (isMandatory)
			{
				logger.LogAndThrow<ArgumentException>("Properties list is empty - mandatory properties missing");
			}
			return result;
		}
		Property property = properties.FirstOrDefault((Property p) => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
		if (property == null)
		{
			if (isMandatory)
			{
				logger.LogAndThrow<ArgumentException>($"Property <{propertyName}> with <{typeof(T).Name}> type mandatory for this action.");
			}
			return result;
		}
		result = ((!typeof(T).IsEnum) ? GetValue<T>(property) : GetEnumValue<T>(property));
		logger.DebugExitMethod($"GetPropertyValue<{typeof(T)}>");
		return result;
	}

	private static T GetEnumValue<T>(Property property)
	{
		T result = default(T);
		if (property.Value == null || !GenericParser.Parse<T>(property.Value.ToString(), out result))
		{
			string arg = property.Value.ToString();
			logger.LogAndThrow<ArgumentException>($"Wrong value for parameter <{property.Name}> with type <{typeof(T).Name}>. Provided value <{arg}>");
		}
		return result;
	}

	private static T GetValue<T>(Property property)
	{
		T result = default(T);
		try
		{
			if ((object)typeof(T) == typeof(Guid))
			{
				Guid guid = property.Value.ToString().ToGuid();
				result = (T)(object)guid;
				return result;
			}
			result = TypeConverter.To<T>((IConvertible)property.Value);
			return result;
		}
		catch (Exception exception)
		{
			string text = property.Value.ToString();
			Type type = property.Value.GetType();
			logger.Error("Property value cast error.", exception);
			logger.LogAndThrow<ArgumentException>($"Wrong value for property <{property.Name}> with type <{typeof(T).Name}>. Provided value <{text}> with type <{type.Name}>");
		}
		return result;
	}
}
