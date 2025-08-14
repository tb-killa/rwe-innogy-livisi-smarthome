using System;
using System.Collections;
using System.Reflection;
using Org.BouncyCastle.Utilities.Date;

namespace Org.BouncyCastle.Utilities;

internal abstract class Enums
{
	internal static Enum GetEnumValue(Type enumType, string s)
	{
		if (!IsEnumType(enumType))
		{
			throw new ArgumentException("Not an enumeration type", "enumType");
		}
		if (s.Length > 0 && char.IsLetter(s[0]) && s.IndexOf(',') < 0)
		{
			s = s.Replace('-', '_');
			s = s.Replace('/', '_');
			FieldInfo field = enumType.GetField(s, BindingFlags.Static | BindingFlags.Public);
			if ((object)field != null)
			{
				return (Enum)field.GetValue(null);
			}
		}
		throw new ArgumentException();
	}

	internal static Array GetEnumValues(Type enumType)
	{
		if (!IsEnumType(enumType))
		{
			throw new ArgumentException("Not an enumeration type", "enumType");
		}
		IList list = Platform.CreateArrayList();
		FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			list.Add(fieldInfo.GetValue(enumType));
		}
		object[] array2 = new object[list.Count];
		list.CopyTo(array2, 0);
		return array2;
	}

	internal static Enum GetArbitraryValue(Type enumType)
	{
		Array enumValues = GetEnumValues(enumType);
		int index = (int)(DateTimeUtilities.CurrentUnixMs() & 0x7FFFFFFF) % enumValues.Length;
		return (Enum)enumValues.GetValue(index);
	}

	internal static bool IsEnumType(Type t)
	{
		return t.IsEnum;
	}
}
