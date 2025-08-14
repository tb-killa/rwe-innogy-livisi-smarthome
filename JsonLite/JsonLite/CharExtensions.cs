using System;

namespace JsonLite;

internal static class CharExtensions
{
	private static readonly char[] HexLetterDigit = new char[11]
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'd',
		'f'
	};

	public static bool IsWhiteSpace(this char c)
	{
		return char.IsWhiteSpace(c);
	}

	public static bool IsLetter(this char c)
	{
		return char.IsLetter(c);
	}

	public static bool IsDigit(this char c)
	{
		return char.IsDigit(c);
	}

	public static bool IsHexDigit(this char c)
	{
		bool flag = false;
		char[] hexLetterDigit = HexLetterDigit;
		foreach (char c2 in hexLetterDigit)
		{
			flag = flag || c == c2;
		}
		if (!c.IsDigit())
		{
			return flag;
		}
		return true;
	}

	public static char ParseChar(string value)
	{
		if (value.Length == 1)
		{
			return value[0];
		}
		throw new FormatException("Cannot convert string to char, it must contain exactly one 1 character");
	}
}
