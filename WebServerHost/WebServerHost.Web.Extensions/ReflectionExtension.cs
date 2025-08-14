using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebServerHost.Web.Extensions;

internal static class ReflectionExtension
{
	public static bool HasAttributeOfType<T>(this ICustomAttributeProvider cap) where T : Attribute
	{
		return cap.GetCustomAttributes(inherit: true).OfType<T>().Any();
	}

	public static T GetAttribute<T>(this ICustomAttributeProvider cap) where T : Attribute
	{
		return cap.GetAllAttributes<T>().FirstOrDefault();
	}

	public static ICollection<T> GetAllAttributes<T>(this ICustomAttributeProvider cap) where T : Attribute
	{
		if (cap.HasAttributeOfType<T>())
		{
			return cap.GetCustomAttributes(inherit: true).OfType<T>().ToList();
		}
		return new List<T>();
	}

	public static ICollection<T> GetAllAttributesOf<T>(this ICustomAttributeProvider cap) where T : Attribute
	{
		return (from a in cap.GetCustomAttributes(inherit: true)
			where (object)a.GetType() == typeof(T) || a.GetType().IsSubclassOf(typeof(T))
			select a).Cast<T>().ToList();
	}
}
