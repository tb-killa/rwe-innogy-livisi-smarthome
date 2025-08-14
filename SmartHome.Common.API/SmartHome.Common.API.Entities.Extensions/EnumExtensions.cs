using System;
using System.Linq;
using System.Reflection;

namespace SmartHome.Common.API.Entities.Extensions;

public static class EnumExtensions
{
	public static bool TryParseNullableEnum<TEnum>(this string enumStringValue, out TEnum? enumValue) where TEnum : struct
	{
		enumValue = null;
		if (enumStringValue.IsNullOrEmpty())
		{
			return true;
		}
		try
		{
			enumValue = (TEnum)Enum.Parse(typeof(TEnum), enumStringValue, ignoreCase: true);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static bool TryParse<TEnum>(this string value, bool ignoreCase, out TEnum enumValue) where TEnum : struct
	{
		enumValue = default(TEnum);
		if (value.IsNullOrEmpty())
		{
			return false;
		}
		try
		{
			enumValue = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static string GetDescription<TEnum>(this TEnum value) where TEnum : struct, IConvertible
	{
		Type enumType = typeof(TEnum);
		if (enumType.IsEnum || enumType.IsSubclassOf(typeof(Enum)))
		{
			MemberInfo memberInfo = enumType.GetMember(value.ToString()).FirstOrDefault((MemberInfo m) => (object)m.DeclaringType == enumType);
			object[] customAttributes = memberInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), inherit: false);
			if (customAttributes.Any())
			{
				return ((EnumDescriptionAttribute)customAttributes.First()).description;
			}
		}
		return "";
	}
}
