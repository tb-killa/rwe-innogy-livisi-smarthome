using System;
using System.Collections.Generic;
using System.Linq;
using ModelTransformations.Helpers;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.Helpers;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.Extensions;

public static class ParameterExtensions
{
	private static readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ParameterExtensions));

	public static T GetConstantParameterValue<T>(this List<Parameter> parameters, string parameterName)
	{
		logger.DebugEnterMethod($"GetConstantParameterValue<{typeof(T)}>");
		T result = default(T);
		if (parameters == null || parameters.Count == 0)
		{
			logger.LogAndThrow<ArgumentException>("Parameters list is empty - mandatory parameters missing");
			return result;
		}
		Parameter parameter = parameters.FirstOrDefault((Parameter p) => p.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));
		if (parameter == null || parameter.Constant == null || parameter.Type != "Constant")
		{
			logger.LogAndThrow<ArgumentException>(string.Format("{0} parameter with {1} type and {2} value mandatory for this action.", parameterName, "Constant", typeof(T).Name));
			return result;
		}
		result = ((!typeof(T).IsEnum) ? GetValue<T>(parameter) : GetEnumValue<T>(parameter));
		logger.DebugExitMethod($"GetConstantParameterValue<{typeof(T)}>");
		return result;
	}

	private static T GetEnumValue<T>(Parameter parameter)
	{
		T result = default(T);
		if (parameter.Constant.Value == null || !GenericParser.Parse<T>(parameter.Constant.Value.ToString(), out result))
		{
			string arg = parameter.Constant.Value.ToString();
			logger.LogAndThrow<ArgumentException>($"Wrong value for parameter <{parameter.Name}> with type <{typeof(T).Name}>. Provided value <{arg}>");
		}
		return result;
	}

	private static T GetValue<T>(Parameter parameter)
	{
		T result = default(T);
		try
		{
			if ((object)typeof(T) == typeof(Guid))
			{
				Guid guid = parameter.Constant.Value.ToString().ToGuid();
				result = (T)(object)guid;
				return result;
			}
			result = TypeConverter.To<T>((IConvertible)parameter.Constant.Value);
			return result;
		}
		catch (Exception exception)
		{
			string text = parameter.Constant.Value.ToString();
			Type type = parameter.Constant.Value.GetType();
			logger.Error("Parameter value cast error.", exception);
			logger.LogAndThrow<ArgumentException>($"Wrong value for parameter <{parameter.Name}> with type <{typeof(T).Name}>. Provided value <{text}> with type <{type.Name}>");
		}
		return result;
	}
}
