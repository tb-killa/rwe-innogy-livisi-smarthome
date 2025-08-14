using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace onrkn;

internal abstract class rllhn
{
	private class cbxnp
	{
		private readonly Encoder apkhf;

		private readonly bool gtaae;

		private readonly char[] gcqkg;

		private readonly byte[] azaix;

		private int ulorw;

		private string zgjuo;

		private int dfpsq;

		public cbxnp(string value, Encoding encoding, bool quoted, int maxLen)
		{
			gcqkg = value.ToCharArray();
			apkhf = encoding.GetEncoder();
			gtaae = quoted;
			ulorw = 0;
			dfpsq = Math.Min(maxLen, 76);
			azaix = new byte[Math.Max(dfpsq * 2, dfpsq + 4)];
		}

		public string fctvv()
		{
			if (zgjuo != null && 0 == 0)
			{
				string result = zgjuo;
				zgjuo = null;
				return result;
			}
			if (ulorw == gcqkg.Length)
			{
				return null;
			}
			int num = 0;
			while (true)
			{
				int bytes = apkhf.GetBytes(gcqkg, ulorw, 1, azaix, num, ulorw < gcqkg.Length - 1);
				if (bytes == 3 && ulorw + 1 < gcqkg.Length && ((azaix[num] == 239 && azaix[num + 1] == 191 && azaix[num + 2] == 189) || (azaix[num] == 237 && azaix[num + 1] == 160 && azaix[num + 2] == 189)))
				{
					bytes = apkhf.GetBytes(gcqkg, ulorw, 2, azaix, num, ulorw < gcqkg.Length - 1);
					ulorw++;
				}
				ulorw++;
				num += bytes;
				if (bytes > 0)
				{
					if (gtaae && 0 == 0)
					{
						return kgbvh.qtbqs(azaix, 0, num);
					}
					if (num > dfpsq)
					{
						zgjuo = Convert.ToBase64String(azaix, 0, num);
						return "";
					}
					if (num % 3 == 0 || false || ulorw == gcqkg.Length)
					{
						break;
					}
				}
			}
			return Convert.ToBase64String(azaix, 0, num);
		}
	}

	public static void soadw(TextWriter p0, string p1, bool p2, bool p3, int p4)
	{
		zncis zncis2 = p0 as zncis;
		int num;
		if (zncis2 == null || 1 == 0)
		{
			p4 = int.MaxValue;
			num = 0;
			if (num == 0)
			{
				goto IL_002b;
			}
		}
		num = zncis2.ejuhv;
		goto IL_002b;
		IL_0183:
		bool flag;
		bool flag2;
		bool flag3;
		StringBuilder stringBuilder;
		if ((!flag || 1 == 0) && (!flag2 || 1 == 0))
		{
			if (flag3 && 0 == 0)
			{
				zncis2.midzg();
			}
			p1 = stringBuilder.ToString();
		}
		goto IL_01b6;
		IL_0170:
		int num2 = num2 + 1;
		goto IL_0176;
		IL_0067:
		char c = p1[num2];
		int num3;
		int num4;
		if (c == ' ' || c == '\t')
		{
			num3 = stringBuilder.Length;
		}
		else
		{
			if (c < ' ' || c > '\u007f')
			{
				flag2 = true;
				if (flag2)
				{
					goto IL_0183;
				}
			}
			if (!p2 || 1 == 0)
			{
				if (c == '"' || c == '\\')
				{
					flag = true;
					num4++;
				}
				else if ((!flag || 1 == 0) && (!kgbvh.ynmbt(c) || 1 == 0))
				{
					flag = true;
				}
			}
		}
		num++;
		if (zncis2 != null && 0 == 0 && num + num4 >= p4)
		{
			if ((!flag3 || 1 == 0) && zncis2.ljdlj)
			{
				num3 = -1;
				num -= zncis2.ejuhv;
				flag3 = true;
				stringBuilder.Append(c);
				goto IL_0170;
			}
			if (num3 == -1 || flag)
			{
				flag2 = true;
				if (flag2)
				{
					goto IL_0183;
				}
			}
			stringBuilder.Insert(num3, "\r\n");
			num = stringBuilder.Length - num3 - 2;
			num3 = -1;
		}
		stringBuilder.Append(c);
		goto IL_0170;
		IL_002b:
		flag = p3;
		flag2 = p1.IndexOf("=?") >= 0;
		flag3 = false;
		if (!flag2 || 1 == 0)
		{
			stringBuilder = new StringBuilder();
			num3 = -1;
			num4 = 0;
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_0067;
			}
			goto IL_0176;
		}
		goto IL_01b6;
		IL_01b6:
		if (flag2 && 0 == 0)
		{
			bvfoi(p0, p1, p4);
		}
		else if (flag && 0 == 0)
		{
			p0.Write(kgbvh.rqlyl(p1, '"', '"'));
		}
		else
		{
			p0.Write(p1);
		}
		return;
		IL_0176:
		if (num2 < p1.Length)
		{
			goto IL_0067;
		}
		goto IL_0183;
	}

	public static void bvfoi(TextWriter p0, string p1, int p2)
	{
		zncis zncis2 = p0 as zncis;
		Encoding p3 = zncis2?.fdqer;
		p3 = kgbvh.gptax(p3, null, p1, out var p4);
		string text = p3.WebName.ToLower(CultureInfo.InvariantCulture);
		string text2 = Convert.ToBase64String(p4, 0, p4.Length);
		string text3 = kgbvh.qtbqs(p4, 0, p4.Length);
		string text4;
		string value;
		bool flag;
		if (text2.Length < text3.Length)
		{
			text4 = "B";
			value = text2;
			flag = false;
			if (!flag)
			{
				goto IL_0085;
			}
		}
		text4 = "Q";
		value = text3;
		flag = true;
		goto IL_0085;
		IL_0085:
		if (zncis2 == null || 1 == 0)
		{
			p0.Write("=?");
			p0.Write(text);
			p0.Write("?");
			p0.Write(text4);
			p0.Write("?");
			p0.Write(value);
			p0.Write("?=");
			return;
		}
		p2 = Math.Min(96, p2);
		int num = p2 - (8 + text.Length + text4.Length);
		cbxnp cbxnp = new cbxnp(p1, p3, flag, num);
		bool flag2 = false;
		string text5 = cbxnp.fctvv();
		if (text5.Length == 0 || 1 == 0)
		{
			zncis2.sbcih('\t');
			text5 = cbxnp.fctvv();
		}
		while (text5 != null)
		{
			if (text5.Length == 0 || 1 == 0)
			{
				text5 = cbxnp.fctvv();
				if (!flag2)
				{
					continue;
				}
				p0.Write("?=");
				p0.Write(' ');
				flag2 = false;
				if (!flag2)
				{
					continue;
				}
			}
			if (flag2 && 0 == 0)
			{
				if (zncis2.ejuhv >= p2)
				{
					p0.Write("?=");
					flag2 = false;
					zncis2.sbcih('\t');
				}
			}
			else if (zncis2.ejuhv >= num - text5.Length)
			{
				zncis2.sbcih('\t');
			}
			if (!flag2 || 1 == 0)
			{
				p0.Write("=?");
				p0.Write(text);
				p0.Write("?");
				p0.Write(text4);
				p0.Write("?");
				flag2 = true;
				p0.Write(text5);
				text5 = cbxnp.fctvv();
			}
			else
			{
				p0.Write(text5);
				text5 = cbxnp.fctvv();
			}
		}
		if (flag2 && 0 == 0)
		{
			p0.Write("?=");
		}
	}

	public static string jyteb(string p0)
	{
		return p0.Replace("\n\t", "\t").Replace("\n ", "\t").Replace("\n", "");
	}

	public static void btprl(TextWriter p0)
	{
		if (!(p0 is zncis zncis2) || 1 == 0)
		{
			p0.Write(' ');
		}
		else
		{
			zncis2.fquft(' ', '\t');
		}
	}

	public static void bnpgt(TextWriter p0, string p1, int p2)
	{
		zncis zncis2 = p0 as zncis;
		if (zncis2 == null || 1 == 0)
		{
			p2 = int.MaxValue;
		}
		else
		{
			_ = zncis2.ejuhv;
		}
		string[] array = p1.Split('\t');
		int num = 0;
		if (num != 0)
		{
			goto IL_0044;
		}
		goto IL_008f;
		IL_0044:
		string text = array[num];
		if (text.Length != 0 && 0 == 0)
		{
			if (num > 0)
			{
				if (zncis2 == null || 1 == 0)
				{
					p0.Write(' ');
				}
				else
				{
					p0.Write("\r\n\t");
				}
			}
			p0.Write(text);
		}
		num++;
		goto IL_008f;
		IL_008f:
		if (num >= array.Length)
		{
			return;
		}
		goto IL_0044;
	}
}
