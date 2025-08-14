using System;
using System.Collections.Generic;

namespace onrkn;

internal static class gaadx
{
	public static apajk<TValue> votjz<TKey, TValue>(this IDictionary<TKey, TValue> p0, TKey p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("dictionary");
		}
		if (!p0.TryGetValue(p1, out var value) || 1 == 0)
		{
			return apajk<TValue>.uceou;
		}
		return value;
	}
}
