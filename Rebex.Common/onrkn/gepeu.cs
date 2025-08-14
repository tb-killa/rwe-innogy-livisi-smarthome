using System;
using System.Text;

namespace onrkn;

internal static class gepeu
{
	private static string[] jssdx = new string[96]
	{
		"IBM037", "IBM437", "IBM500", "ASMO-708", "DOS-720", "ibm737", "ibm775", "ibm850", "ibm852", "IBM855",
		"ibm857", "IBM00858", "IBM860", "ibm861", "DOS-862", "IBM863", "IBM864", "IBM865", "cp866", "ibm869",
		"IBM870", "windows-874", "cp875", "IBM1026", "IBM01047", "IBM01140", "IBM01141", "IBM01142", "IBM01143", "IBM01144",
		"IBM01145", "IBM01146", "IBM01147", "IBM01148", "IBM01149", "windows-1250", "windows-1251", "Windows-1252", "windows-1253", "windows-1254",
		"windows-1255", "windows-1256", "windows-1257", "windows-1258", "macintosh", "x-mac-arabic", "x-mac-hebrew", "x-mac-greek", "x-mac-cyrillic", "x-mac-romanian",
		"x-mac-ukrainian", "x-mac-thai", "x-mac-ce", "x-mac-icelandic", "x-mac-turkish", "x-mac-croatian", "x-IA5", "x-IA5-German", "x-IA5-Swedish", "x-IA5-Norwegian",
		"us-ascii", "x-cp20269", "IBM273", "IBM277", "IBM278", "IBM280", "IBM284", "IBM285", "IBM290", "IBM297",
		"IBM420", "IBM423", "IBM424", "x-EBCDIC-KoreanExtended", "IBM-Thai", "koi8-r", "IBM871", "IBM880", "IBM905", "IBM00924",
		"cp1025", "koi8-u", "iso-8859-1", "iso-8859-2", "iso-8859-3", "iso-8859-4", "iso-8859-5", "iso-8859-6", "iso-8859-7", "iso-8859-8",
		"iso-8859-9", "iso-8859-11", "iso-8859-13", "iso-8859-15", "x-Europa", "iso-8859-8-i"
	};

	internal static bool qdrhv(Encoding p0)
	{
		string[] array = jssdx;
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_002f;
		IL_000c:
		string strA = array[num];
		if (string.Compare(strA, p0.WebName, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			return true;
		}
		num++;
		goto IL_002f;
		IL_002f:
		if (num < array.Length)
		{
			goto IL_000c;
		}
		return false;
	}
}
