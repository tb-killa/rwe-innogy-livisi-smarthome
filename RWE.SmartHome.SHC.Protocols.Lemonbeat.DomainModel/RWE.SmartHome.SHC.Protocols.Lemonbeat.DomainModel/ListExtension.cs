using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public static class ListExtension
{
	public static bool Compare<T>(this List<T> me, List<T> other) where T : IEquatable<T>
	{
		if (me == null && other == null)
		{
			return true;
		}
		if (me == null)
		{
			return false;
		}
		if (other == null)
		{
			return false;
		}
		if (other.Count != me.Count)
		{
			return false;
		}
		List<T>.Enumerator enumerator = other.GetEnumerator();
		foreach (T item in me)
		{
			enumerator.MoveNext();
			if (!item.Equals(enumerator.Current))
			{
				return false;
			}
		}
		return true;
	}

	public static bool ElementsEqual<T>(this T[] a1, T[] a2)
	{
		if (object.ReferenceEquals(a1, a2))
		{
			return true;
		}
		if (a1 == null || a2 == null)
		{
			return false;
		}
		if (a1.Length != a2.Length)
		{
			return false;
		}
		EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
		for (int i = 0; i < a1.Length; i++)
		{
			if (!equalityComparer.Equals(a1[i], a2[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool ContentEqual<T>(this List<T> compareWith, List<T> compareTo)
	{
		if (object.ReferenceEquals(compareWith, compareTo))
		{
			return true;
		}
		if (compareWith == null || compareTo == null)
		{
			return false;
		}
		return compareWith.Except(compareTo).Count() == 0;
	}
}
