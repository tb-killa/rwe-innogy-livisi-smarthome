using System;
using System.Text;

namespace onrkn;

internal class rcbhr
{
	private const string gsdzi = "OID.";

	private const string onrmj = "=";

	private const string uhnqw = ", ";

	private const string fnbzz = " + ";

	public static string hufkx(byte[] p0)
	{
		if (p0 == null || false || p0.Length == 0 || 1 == 0)
		{
			return string.Empty;
		}
		suzxs suzxs2 = new suzxs();
		hfnnn.qnzgo(suzxs2, p0);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		for (int num = suzxs2.Count - 1; num >= 0; stringBuilder.Append(kohwe(suzxs2[num])), num--)
		{
			if (flag && 0 == 0)
			{
				flag = false;
				if (!flag)
				{
					continue;
				}
			}
			stringBuilder.Append(", ");
		}
		return stringBuilder.ToString();
	}

	private static string kohwe(hjdlb p0)
	{
		if (p0.Count == 0 || 1 == 0)
		{
			throw new ozsgc("Distinguished name in binary format is not correct. Missing 'Attribute Type and Value' part in Relative Distinguished Name.");
		}
		string value = " + ";
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		int num = 0;
		if (num != 0)
		{
			goto IL_0031;
		}
		goto IL_0060;
		IL_0049:
		stringBuilder.Append(nytfe(p0[num]));
		num++;
		goto IL_0060;
		IL_0060:
		if (num < p0.Count)
		{
			goto IL_0031;
		}
		return stringBuilder.ToString();
		IL_0031:
		if (flag && 0 == 0)
		{
			flag = false;
			if (!flag)
			{
				goto IL_0049;
			}
		}
		stringBuilder.Append(value);
		goto IL_0049;
	}

	private static string nytfe(wusby p0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		ubfew ubfew2 = ubfew.umnjs(p0.sdyid.Value);
		stringBuilder.Append(ubfew2.wmofh);
		stringBuilder.Append("=");
		if (ubfew2.wmofh.IndexOf("OID.", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			xbxhi(p0, stringBuilder);
		}
		else
		{
			string dcokg = vesyi.onwas(p0.fajfk).dcokg;
			if (dcokg == null || 1 == 0)
			{
				xbxhi(p0, stringBuilder);
			}
			else
			{
				stringBuilder.Append(hvbig(dcokg));
			}
		}
		return stringBuilder.ToString();
	}

	private static void xbxhi(wusby p0, StringBuilder p1)
	{
		p1.Append('#');
		p1.Append(BitConverter.ToString(p0.fajfk).Replace("-", ""));
	}

	private static string hvbig(string p0)
	{
		bool flag = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_0008;
		}
		goto IL_0027;
		IL_0008:
		char c = p0[num];
		if (c == '"' || c == ',' || c == '\\')
		{
			flag = true;
		}
		num++;
		goto IL_0027;
		IL_0027:
		if (num >= p0.Length)
		{
			if (flag && 0 == 0)
			{
				return "\"" + p0 + "\"";
			}
			return p0;
		}
		goto IL_0008;
	}
}
