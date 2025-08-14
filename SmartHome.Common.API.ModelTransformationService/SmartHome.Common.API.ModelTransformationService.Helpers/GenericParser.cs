using System;
using System.Globalization;

namespace SmartHome.Common.API.ModelTransformationService.Helpers;

internal static class GenericParser
{
	public static bool Parse<T>(string input, out T result)
	{
		result = default(T);
		Type type = typeof(T);
		if (type.IsNullableType())
		{
			type = Nullable.GetUnderlyingType(type);
		}
		try
		{
			if (type.IsEnum || type.IsSubclassOf(typeof(Enum)))
			{
				result = (T)Enum.Parse(type, input, ignoreCase: true);
			}
			else
			{
				result = (T)Parse(type, input);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static bool IsNullableType(this Type type)
	{
		if ((object)type == null)
		{
			return false;
		}
		return (object)Nullable.GetUnderlyingType(type) != null;
	}

	private static object Parse(Type type, string input)
	{
		CultureInfo provider = new CultureInfo("en-US");
		NumberStyles style = NumberStyles.Integer | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint;
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.Boolean:
			return bool.Parse(input);
		case TypeCode.Byte:
			return byte.Parse(input);
		case TypeCode.Char:
			if (input.Length == 1)
			{
				return input[0];
			}
			throw new FormatException("String must be exacly one char loong");
		case TypeCode.Decimal:
			return decimal.Parse(input, style, provider);
		case TypeCode.Double:
			return double.Parse(input, style, provider);
		case TypeCode.Int16:
			return short.Parse(input);
		case TypeCode.Int32:
			return int.Parse(input);
		case TypeCode.Int64:
			return long.Parse(input);
		case TypeCode.SByte:
			return sbyte.Parse(input);
		case TypeCode.Single:
			return float.Parse(input, style, provider);
		case TypeCode.String:
			return input;
		case TypeCode.UInt16:
			return ushort.Parse(input);
		case TypeCode.UInt32:
			return uint.Parse(input);
		case TypeCode.UInt64:
			return ulong.Parse(input);
		case TypeCode.DateTime:
			return DateTime.Parse(input, null);
		default:
			throw new InvalidOperationException();
		}
	}
}
