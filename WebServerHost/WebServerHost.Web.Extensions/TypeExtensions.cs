using System;

namespace WebServerHost.Web.Extensions;

internal static class TypeExtensions
{
	public static object GetDafaultValue(this Type type)
	{
		if (type.IsValueType)
		{
			return Activator.CreateInstance(type);
		}
		return null;
	}

	public static object Parse(this Type type, string value)
	{
		if (type.IsNullableType())
		{
			return Nullable.GetUnderlyingType(type).Parse(value);
		}
		if ((object)type == typeof(string))
		{
			return value;
		}
		if ((object)type == typeof(bool))
		{
			return bool.Parse(value);
		}
		if ((object)type == typeof(byte))
		{
			return byte.Parse(value);
		}
		if ((object)type == typeof(sbyte))
		{
			return sbyte.Parse(value);
		}
		if ((object)type == typeof(short))
		{
			return short.Parse(value);
		}
		if ((object)type == typeof(ushort))
		{
			return ushort.Parse(value);
		}
		if ((object)type == typeof(int))
		{
			return int.Parse(value);
		}
		if ((object)type == typeof(uint))
		{
			return uint.Parse(value);
		}
		if ((object)type == typeof(long))
		{
			return long.Parse(value);
		}
		if ((object)type == typeof(ulong))
		{
			return ulong.Parse(value);
		}
		if ((object)type == typeof(float))
		{
			return float.Parse(value);
		}
		if ((object)type == typeof(double))
		{
			return double.Parse(value);
		}
		if ((object)type == typeof(decimal))
		{
			return decimal.Parse(value);
		}
		if ((object)type == typeof(char))
		{
			return value[0];
		}
		if ((object)type.BaseType == typeof(Enum))
		{
			return Enum.Parse(type, value, ignoreCase: true);
		}
		if ((object)type == typeof(DateTime))
		{
			return DateTime.Parse(value).ToUniversalTime();
		}
		if ((object)type == typeof(Guid))
		{
			return new Guid(value);
		}
		return null;
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
