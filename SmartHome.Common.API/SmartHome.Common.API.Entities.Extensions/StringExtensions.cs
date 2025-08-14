using System;
using System.Linq;

namespace SmartHome.Common.API.Entities.Extensions;

public static class StringExtensions
{
	public static bool IsNullOrEmpty(this string value)
	{
		return string.IsNullOrEmpty(value);
	}

	public static bool IsNotEmptyOrNull(this string value)
	{
		return !value.IsNullOrEmpty();
	}

	public static string Format(this string format, params object[] args)
	{
		return string.Format(format, args);
	}

	public static Guid ToGuid(this string value)
	{
		if (value.IsNullOrEmpty())
		{
			throw new ArgumentException();
		}
		if (value.GuidTryParse(out var output))
		{
			return output;
		}
		return Guid.Empty;
	}

	public static bool GuidTryParse(this string value, out Guid output)
	{
		output = Guid.Empty;
		try
		{
			output = new Guid(value);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool DateTimeTryParse(this string value, out DateTime result)
	{
		result = default(DateTime);
		try
		{
			result = DateTime.Parse(value);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool EqualsIgnoreCase(this string value, string other)
	{
		return value.Equals(other, StringComparison.InvariantCultureIgnoreCase);
	}

	public static string ToMajorDotMinorVersion(this string version)
	{
		string text = string.Empty;
		if (version.IsNotEmptyOrNull())
		{
			string[] array = version.Split('.');
			if (array.Any())
			{
				text += array[0];
				if (array.Count() > 1)
				{
					text = text + "." + array[1];
				}
			}
		}
		return text;
	}

	public static string FirstToLower(this string source)
	{
		if (source.IsNotEmptyOrNull() && char.IsUpper(source[0]))
		{
			char[] array = source.ToCharArray();
			array[0] = char.ToLower(array[0]);
			return new string(array);
		}
		return source;
	}

	public static string FirstToUpper(this string source)
	{
		if (source.IsNotEmptyOrNull() && char.IsLower(source[0]))
		{
			char[] array = source.ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}
		return source;
	}
}
