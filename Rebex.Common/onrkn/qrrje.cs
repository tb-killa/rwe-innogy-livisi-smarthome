using System;
using System.Text;

namespace onrkn;

internal class qrrje : Encoding
{
	public const int sysxu = 20127;

	private readonly string alhwi;

	private readonly string pwgyp;

	private readonly int rlnir;

	private readonly int rulep;

	public override string WebName => alhwi;

	public override int CodePage => rlnir;

	public qrrje(string name, string title, int codePage, int maxAllowedByte)
	{
		alhwi = name;
		pwgyp = title;
		rlnir = codePage;
		rulep = maxAllowedByte;
	}

	public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
	{
		if (chars == null || 1 == 0)
		{
			throw new ArgumentNullException("chars");
		}
		if (bytes == null || 1 == 0)
		{
			throw new ArgumentNullException("bytes");
		}
		if (charIndex < 0 || charIndex > chars.Length)
		{
			throw hifyx.nztrs("charIndex", charIndex, "Input offset is outside the bounds of an array.");
		}
		if (charCount < 0 || charIndex + charCount > chars.Length)
		{
			throw hifyx.nztrs("charCount", charCount, "Count is outside the bounds of an array.");
		}
		if (byteIndex < 0 || byteIndex > bytes.Length)
		{
			throw hifyx.nztrs("byteIndex", byteIndex, "Output offset is outside the bounds of an array.");
		}
		if (byteIndex + charCount > bytes.Length)
		{
			throw new ArgumentException("Output array is too small.");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_00b0;
		}
		goto IL_00cf;
		IL_00b0:
		int num2 = chars[charIndex + num];
		if (num2 > rulep)
		{
			num2 = 63;
		}
		bytes[byteIndex + num] = (byte)num2;
		num++;
		goto IL_00cf;
		IL_00cf:
		if (num < charCount)
		{
			goto IL_00b0;
		}
		return charCount;
	}

	public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
	{
		if (bytes == null || 1 == 0)
		{
			throw new ArgumentNullException("bytes");
		}
		if (chars == null || 1 == 0)
		{
			throw new ArgumentNullException("chars");
		}
		if (byteIndex < 0 || byteIndex > bytes.Length)
		{
			throw hifyx.nztrs("byteIndex", byteIndex, "Input offset is outside the bounds of an array.");
		}
		if (byteCount < 0 || byteIndex + byteCount > bytes.Length)
		{
			throw hifyx.nztrs("byteCount", byteCount, "Count is outside the bounds of an array.");
		}
		if (charIndex < 0 || charIndex > chars.Length)
		{
			throw hifyx.nztrs("charIndex", charIndex, "Output offset is outside the bounds of an array.");
		}
		if (charIndex + byteCount > chars.Length)
		{
			throw new ArgumentException("Output array is too small.");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_00b0;
		}
		goto IL_00cf;
		IL_00b0:
		int num2 = bytes[byteIndex + num];
		if (num2 > rulep)
		{
			num2 = 63;
		}
		chars[charIndex + num] = (char)num2;
		num++;
		goto IL_00cf;
		IL_00cf:
		if (num < byteCount)
		{
			goto IL_00b0;
		}
		return byteCount;
	}

	public override int GetByteCount(char[] chars, int index, int count)
	{
		return count;
	}

	public override int GetCharCount(byte[] bytes, int index, int count)
	{
		return count;
	}

	public override int GetMaxByteCount(int charCount)
	{
		return charCount;
	}

	public override int GetMaxCharCount(int byteCount)
	{
		return byteCount;
	}
}
