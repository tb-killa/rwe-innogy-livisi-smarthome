using System;
using System.Reflection;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSettings;

public static class ShcTimeZoneExtensions
{
	public static string GetStringValue(this ShcTimeZone shcTimeZone)
	{
		string result = null;
		Type type = shcTimeZone.GetType();
		FieldInfo field = type.GetField(shcTimeZone.ToString());
		StringValueAttribute[] array = field.GetCustomAttributes(typeof(StringValueAttribute), inherit: false) as StringValueAttribute[];
		if (array.Length > 0)
		{
			result = array[0].Value;
		}
		return result;
	}

	public static ShcTimeZone ParseAsShcTimeZone(this string stringValue, bool ignoreCase)
	{
		object obj = null;
		string strA = null;
		if (string.IsNullOrEmpty(stringValue))
		{
			throw new ArgumentNullException("Can't parse an empty string");
		}
		Type typeFromHandle = typeof(ShcTimeZone);
		FieldInfo[] fields = typeFromHandle.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			StringValueAttribute[] array = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), inherit: false) as StringValueAttribute[];
			if (array.Length > 0)
			{
				strA = array[0].Value;
			}
			if (string.Compare(strA, stringValue, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
			{
				obj = Enum.Parse(typeFromHandle, fieldInfo.Name, ignoreCase: false);
				break;
			}
		}
		return (ShcTimeZone)obj;
	}
}
