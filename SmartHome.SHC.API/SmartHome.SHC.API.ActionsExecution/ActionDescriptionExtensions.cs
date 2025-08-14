using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.ActionsExecution;

public static class ActionDescriptionExtensions
{
	public static Guid? GetParamGuid(this ActionDescription action, string paramName)
	{
		StringProperty param = action.GetParam<StringProperty>(paramName);
		return (param != null) ? new Guid?(new Guid(param.Value)) : ((Guid?)null);
	}

	public static List<Guid> GetParamsGuid(this ActionDescription action, string paramName)
	{
		List<StringProperty> source = action.GetParams<StringProperty>(paramName);
		return source.Select(delegate(StringProperty x)
		{
			try
			{
				return new Guid(x.Value);
			}
			catch (Exception)
			{
				throw new ActionParamsException(paramName);
			}
		}).ToList();
	}

	public static Guid GetRequiredParamGuid(this ActionDescription action, string paramName)
	{
		try
		{
			return new Guid(action.GetRequiredParamString(paramName));
		}
		catch (Exception)
		{
			throw new ActionParamsException(paramName);
		}
	}

	public static DateTime? GetParamDateTime(this ActionDescription action, string paramName)
	{
		DateTimeProperty param = action.GetParam<DateTimeProperty>(paramName);
		return (param != null) ? param.Value : ((DateTime?)null);
	}

	public static List<DateTime> GetParamsDateTime(this ActionDescription action, string paramName)
	{
		List<DateTimeProperty> source = action.GetParams<DateTimeProperty>(paramName);
		return source.Select((DateTimeProperty x) => x.Value.GetValueOrDefault()).ToList();
	}

	public static DateTime GetRequiredParamDateTime(this ActionDescription action, string paramName)
	{
		DateTime? paramDateTime = action.GetParamDateTime(paramName);
		if (!paramDateTime.HasValue)
		{
			throw new ActionParamsException(paramName);
		}
		return paramDateTime.Value;
	}

	public static string GetParamString(this ActionDescription action, string paramName)
	{
		StringProperty param = action.GetParam<StringProperty>(paramName);
		return (param != null) ? param.Value : null;
	}

	public static List<string> GetParamsString(this ActionDescription action, string paramName)
	{
		List<StringProperty> source = action.GetParams<StringProperty>(paramName);
		return source.Select((StringProperty x) => x.Value).ToList();
	}

	public static string GetRequiredParamString(this ActionDescription action, string paramName)
	{
		StringProperty param = action.GetParam<StringProperty>(paramName);
		if (param == null)
		{
			throw new ActionParamsException(paramName);
		}
		return param.Value;
	}

	public static decimal? GetParamDecimal(this ActionDescription action, string paramName)
	{
		NumericProperty param = action.GetParam<NumericProperty>(paramName);
		return (param != null) ? param.Value : ((decimal?)null);
	}

	public static List<decimal> GetParamsDecimal(this ActionDescription action, string paramName)
	{
		List<NumericProperty> source = action.GetParams<NumericProperty>(paramName);
		return source.Select((NumericProperty x) => x.Value.GetValueOrDefault()).ToList();
	}

	public static decimal GetRequiredParamDecimal(this ActionDescription action, string paramName)
	{
		decimal? paramDecimal = action.GetParamDecimal(paramName);
		if (!paramDecimal.HasValue)
		{
			throw new ActionParamsException(paramName);
		}
		return paramDecimal.Value;
	}

	public static bool? GetParamBool(this ActionDescription action, string paramName)
	{
		BooleanProperty param = action.GetParam<BooleanProperty>(paramName);
		return (param != null) ? param.Value : ((bool?)null);
	}

	public static List<bool> GetParamsBoolean(this ActionDescription action, string paramName)
	{
		List<BooleanProperty> source = action.GetParams<BooleanProperty>(paramName);
		return source.Select((BooleanProperty x) => x.Value == true).ToList();
	}

	public static bool GetRequiredParamBool(this ActionDescription action, string paramName)
	{
		bool? paramBool = action.GetParamBool(paramName);
		if (!paramBool.HasValue)
		{
			throw new ActionParamsException(paramName);
		}
		return paramBool.Value;
	}

	public static T GetParam<T>(this ActionDescription action, string paramName) where T : class, Property
	{
		return action.Properties.FirstOrDefault((Property x) => x.Name == paramName) as T;
	}

	public static List<T> GetParams<T>(this ActionDescription action, string paramName) where T : class, Property
	{
		return (from x in action.Properties
			where x.Name == paramName
			select x as T).ToList();
	}
}
