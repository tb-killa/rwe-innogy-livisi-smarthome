using System;
using System.Globalization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Extensions;

public static class EntityExtensionMethods
{
	public static CultureInfo DEFAULT_CULTURE = CultureInfo.InvariantCulture;

	public static string GetStringValue(this decimal? d)
	{
		if (!d.HasValue)
		{
			return null;
		}
		return d.Value.ToString(DEFAULT_CULTURE.NumberFormat);
	}

	public static decimal? GetDecimalValue(this string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return null;
		}
		return Convert.ToDecimal(s, DEFAULT_CULTURE.NumberFormat);
	}
}
