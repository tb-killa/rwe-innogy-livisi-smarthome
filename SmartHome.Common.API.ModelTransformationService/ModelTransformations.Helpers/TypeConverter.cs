using System;

namespace ModelTransformations.Helpers;

internal static class TypeConverter
{
	public static T To<T>(IConvertible obj)
	{
		Type type = typeof(T);
		if (IsNullableType(type))
		{
			type = Nullable.GetUnderlyingType(type);
		}
		if (typeof(IConvertible).IsAssignableFrom(type))
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			TypeCode typeCode2 = obj.GetTypeCode();
			switch (typeCode)
			{
			case TypeCode.String:
				return (T)(object)obj.ToString(null);
			case TypeCode.Boolean:
				if (typeCode2 == TypeCode.Boolean)
				{
					return (T)(object)obj.ToBoolean(null);
				}
				break;
			}
			if (typeCode == TypeCode.DateTime && (typeCode2 == TypeCode.DateTime || typeCode2 == TypeCode.String))
			{
				return (T)(object)obj.ToDateTime(null);
			}
			if (typeCode2 >= TypeCode.Char && typeCode2 <= TypeCode.Decimal && typeCode >= TypeCode.Char && typeCode2 <= TypeCode.Decimal)
			{
				return (T)obj.ToType(typeof(T), null);
			}
		}
		throw new InvalidCastException();
	}

	public static bool IsNullableType(Type type)
	{
		if ((object)type == null)
		{
			return false;
		}
		return (object)Nullable.GetUnderlyingType(type) != null;
	}
}
