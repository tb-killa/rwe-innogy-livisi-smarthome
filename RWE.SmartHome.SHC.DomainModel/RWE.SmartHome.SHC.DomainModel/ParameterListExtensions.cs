using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.DomainModel;

public static class ParameterListExtensions
{
	public static decimal? GetNumericValue(this List<Parameter> list, string propertyName)
	{
		Parameter parameter = list.FirstOrDefault((Parameter p) => p.Name == propertyName);
		if (parameter != null && parameter.Value is ConstantNumericBinding constantNumericBinding)
		{
			return constantNumericBinding.Value;
		}
		return null;
	}

	public static int? GetIntegerValue(this List<Parameter> list, string propertyName)
	{
		Parameter parameter = list.FirstOrDefault((Parameter p) => p.Name == propertyName);
		if (parameter != null && parameter.Value is ConstantNumericBinding constantNumericBinding)
		{
			return Convert.ToInt32(constantNumericBinding.Value);
		}
		return null;
	}

	public static bool? GetBooleanValue(this List<Parameter> list, string propertyName)
	{
		Parameter parameter = list.FirstOrDefault((Parameter p) => p.Name == propertyName);
		if (parameter != null && parameter.Value is ConstantBooleanBinding constantBooleanBinding)
		{
			return constantBooleanBinding.Value;
		}
		return null;
	}

	public static DateTime? GetDateTimeValue(this List<Parameter> list, string propertyName)
	{
		Parameter parameter = list.FirstOrDefault((Parameter p) => p.Name == propertyName);
		if (parameter != null && parameter.Value is ConstantDateTimeBinding constantDateTimeBinding)
		{
			return constantDateTimeBinding.Value;
		}
		return null;
	}

	public static string GetStringValue(this List<Parameter> list, string propertyName)
	{
		Parameter parameter = list.FirstOrDefault((Parameter p) => p.Name == propertyName);
		if (parameter != null && parameter.Value is ConstantStringBinding constantStringBinding)
		{
			return constantStringBinding.Value;
		}
		return null;
	}
}
