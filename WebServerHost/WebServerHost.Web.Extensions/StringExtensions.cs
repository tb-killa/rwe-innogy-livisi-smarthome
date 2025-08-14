using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartHome.Common.API.Entities.Extensions;

namespace WebServerHost.Web.Extensions;

internal static class StringExtensions
{
	public static string[] SplitRouteParts(this string route)
	{
		return (from r in route.Split('/')
			where r.IsNotEmptyOrNull()
			select r).ToArray();
	}

	public static bool IsParamTemplateRoutePart(this string routePart)
	{
		if (routePart[0] == '{')
		{
			return routePart[routePart.Length - 1] == '}';
		}
		return false;
	}

	public static string GetRouteParamName(this string routePart)
	{
		if (routePart.IsParamTemplateRoutePart())
		{
			return routePart.Substring(1, routePart.Length - 2);
		}
		return routePart;
	}

	public static string[] SplitBy(this string original, char separator)
	{
		List<string> list = new List<string>();
		int num = original.IndexOf(separator);
		if (num >= 0)
		{
			string text = original.Substring(0, num);
			if (text.IsNotEmptyOrNull())
			{
				list.Add(text);
			}
			string text2 = original.Substring(num + 1, original.Length - num - 1);
			if (text2.IsNotEmptyOrNull())
			{
				list.Add(text2);
			}
		}
		else if (original.Length > 0)
		{
			list.Add(original);
		}
		return list.ToArray();
	}

	public static string Trim(this string original, char character)
	{
		StringBuilder stringBuilder = new StringBuilder(original);
		while (stringBuilder[0] == character)
		{
			stringBuilder.Remove(0, 1);
		}
		while (stringBuilder[stringBuilder.Length - 1] == character)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return stringBuilder.ToString();
	}
}
