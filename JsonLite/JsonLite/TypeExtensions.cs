using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonLite;

internal static class TypeExtensions
{
	public static bool Implements(this Type type, Type @interface)
	{
		if (@interface.IsGenericType)
		{
			return type.GetInterfaces().Any((Type i) => i.IsGenericType && (object)i.GetGenericTypeDefinition() == @interface);
		}
		return type.GetInterfaces().Contains(@interface);
	}

	public static Type GetTypeOfElements(this Type type)
	{
		if (type.IsArray)
		{
			return type.GetElementType();
		}
		if (type.Implements(typeof(IDictionary)))
		{
			return typeof(KeyValuePairWrapper);
		}
		if (type.Implements(typeof(IEnumerable)))
		{
			return type.GetGenericArguments()[0];
		}
		return null;
	}

	public static bool IsNumber(this Type type)
	{
		if ((object)type != typeof(sbyte) && (object)type != typeof(byte) && (object)type != typeof(short) && (object)type != typeof(ushort) && (object)type != typeof(int) && (object)type != typeof(uint) && (object)type != typeof(long) && (object)type != typeof(ulong) && (object)type != typeof(float) && (object)type != typeof(double))
		{
			return (object)type == typeof(decimal);
		}
		return true;
	}

	public static MemberInfo[] GetFieldsAndProperties(this Type type)
	{
		List<MemberInfo> list = new List<MemberInfo>();
		list.AddRange(type.GetFields());
		list.AddRange(type.GetProperties());
		return list.ToArray();
	}

	public static bool IsNullableType(this Type type)
	{
		if ((object)type == null)
		{
			return false;
		}
		return (object)Nullable.GetUnderlyingType(type) != null;
	}
}
