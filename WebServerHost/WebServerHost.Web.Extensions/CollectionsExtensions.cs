using System;
using System.Collections.Generic;
using System.Linq;

namespace WebServerHost.Web.Extensions;

internal static class CollectionsExtensions
{
	public static bool TryGetValueIgnoreCase(this Dictionary<string, string> source, string key, out string value)
	{
		value = null;
		string text = source.Keys.FirstOrDefault((string k) => k.Equals(key, StringComparison.InvariantCultureIgnoreCase));
		if (text != null)
		{
			value = source[text];
			return true;
		}
		return false;
	}

	public static bool TryGetValueIgnoreCase(this List<KeyValuePair<string, string>> source, string key, out string value)
	{
		value = null;
		KeyValuePair<string, string> keyValuePair = source.FirstOrDefault((KeyValuePair<string, string> p) => p.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
		if (keyValuePair.Key != null)
		{
			value = keyValuePair.Value;
			return true;
		}
		return false;
	}

	public static bool SafeAny<T>(this IEnumerable<T> source)
	{
		return source?.Any() ?? false;
	}

	public static IEnumerable<TResult> SelectDistinct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		List<TResult> list = new List<TResult>();
		foreach (TSource item2 in source)
		{
			TResult item = selector(item2);
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
		return list;
	}
}
