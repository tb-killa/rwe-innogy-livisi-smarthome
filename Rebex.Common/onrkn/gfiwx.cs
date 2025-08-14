using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Rebex;

namespace onrkn;

internal class gfiwx : jcgcz
{
	private enum pfaoi
	{
		zlmjm,
		womnr,
		hzyfd,
		zyods
	}

	public gfiwx()
	{
	}

	public gfiwx(DateTime dt)
		: this(dt, lsgfu(dt))
	{
	}

	public gfiwx(DateTime dt, rmkkr type)
		: base(type, ueaed(dt, type))
	{
	}

	private static rmkkr lsgfu(DateTime p0)
	{
		if (p0.ToUniversalTime().Year < 2050)
		{
			return rmkkr.keeoc;
		}
		return rmkkr.nwijl;
	}

	private static byte[] ueaed(DateTime p0, rmkkr p1)
	{
		DateTime dateTime = p0.ToUniversalTime();
		StringBuilder stringBuilder = new StringBuilder();
		if (p1 == rmkkr.keeoc)
		{
			stringBuilder.dlvlk("{0:yyMMddHHmmss}", dateTime);
		}
		else
		{
			stringBuilder.dlvlk("{0:yyyyMMddHHmmss}", dateTime);
		}
		stringBuilder.Append('Z');
		return EncodingTools.ASCII.GetBytes(stringBuilder.ToString());
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.ziztq, p0, p1);
		base.zkxnk(p0, p1, p2);
	}

	public override string ToString()
	{
		return fzcfd().ToString("s");
	}

	public DateTime fzcfd()
	{
		byte[] array = base.rtrhq;
		string text = EncodingTools.ASCII.GetString(array, 0, array.Length);
		int num = text.IndexOfAny(new char[3] { 'Z', '+', '-' });
		pfaoi pfaoi;
		if (num < 0)
		{
			num = text.Length;
			pfaoi = pfaoi.zlmjm;
			if (pfaoi == pfaoi.zlmjm)
			{
				goto IL_0082;
			}
		}
		switch (text[num])
		{
		case 'Z':
			pfaoi = pfaoi.womnr;
			if (pfaoi != pfaoi.zlmjm)
			{
				break;
			}
			goto case '+';
		case '+':
			pfaoi = pfaoi.hzyfd;
			if (pfaoi != pfaoi.zlmjm)
			{
				break;
			}
			goto case '-';
		case '-':
			pfaoi = pfaoi.zyods;
			if (pfaoi != pfaoi.zlmjm)
			{
				break;
			}
			goto default;
		default:
			throw new InvalidOperationException("Unknown time kind.");
		}
		goto IL_0082;
		IL_030a:
		if (base.ccmfu != rmkkr.nwijl)
		{
			throw new CryptographicException("Invalid time node encountered.");
		}
		throw new CryptographicException("Invalid generalized time node encountered.");
		IL_01d6:
		int[] array2;
		if (pfaoi == pfaoi.hzyfd || pfaoi == pfaoi.zyods)
		{
			try
			{
				array2[7] = int.Parse(text.Substring(num + 1, 2), CultureInfo.InvariantCulture);
				array2[8] = int.Parse(text.Substring(num + 3, 2), CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				goto IL_030a;
			}
		}
		bool flag = false;
		if (array2[5] == 60)
		{
			array2[5] = 0;
			flag = true;
		}
		try
		{
			DateTime dateTime = new DateTime(array2[0], array2[1], array2[2], array2[3], array2[4], array2[5], array2[6], (pfaoi != pfaoi.zlmjm && 0 == 0) ? DateTimeKind.Utc : DateTimeKind.Unspecified);
			if (flag && 0 == 0)
			{
				dateTime = dateTime.AddMinutes(1.0);
			}
			return pfaoi switch
			{
				pfaoi.zlmjm => dateTime, 
				pfaoi.womnr => dateTime.ToLocalTime(), 
				pfaoi.hzyfd => dateTime.AddHours(-array2[7]).AddMinutes(-array2[8]).ToLocalTime(), 
				pfaoi.zyods => dateTime.AddHours(array2[7]).AddMinutes(array2[8]).ToLocalTime(), 
				_ => throw new InvalidOperationException("Unknown time kind."), 
			};
		}
		catch (ArgumentException)
		{
		}
		goto IL_030a;
		IL_0082:
		array2 = new int[9];
		if ((pfaoi != pfaoi.hzyfd && pfaoi != pfaoi.zyods) || text.Length == num + 5)
		{
			if (base.ccmfu != rmkkr.nwijl)
			{
				if (num == 10 || num == 12)
				{
					try
					{
						int num2 = 0;
						int num3 = 0;
						if (num3 != 0)
						{
							goto IL_00c5;
						}
						goto IL_00e8;
						IL_00c5:
						array2[num2++] = int.Parse(text.Substring(num3, 2), CultureInfo.InvariantCulture);
						num3 += 2;
						goto IL_00e8;
						IL_00e8:
						if (num3 < num)
						{
							goto IL_00c5;
						}
					}
					catch (FormatException)
					{
						goto IL_030a;
					}
					if (array2[0] < 50)
					{
						array2[0] += 2000;
					}
					else
					{
						array2[0] += 1900;
					}
					goto IL_01d6;
				}
			}
			else if ((num == 10 || num == 12 || num == 14 || num == 18) && (num != 18 || text[14] == '.'))
			{
				try
				{
					array2[0] = int.Parse(text.Substring(0, 4), CultureInfo.InvariantCulture);
					int num4 = 1;
					int num5 = 4;
					if (num5 == 0)
					{
						goto IL_0184;
					}
					goto IL_01a7;
					IL_0184:
					array2[num4++] = int.Parse(text.Substring(num5, 2), CultureInfo.InvariantCulture);
					num5 += 2;
					goto IL_01a7;
					IL_01a7:
					if (num5 < 14 && num5 < num)
					{
						goto IL_0184;
					}
					if (num == 18)
					{
						array2[6] = int.Parse(text.Substring(15, 3), CultureInfo.InvariantCulture);
					}
				}
				catch (FormatException)
				{
					goto IL_030a;
				}
				goto IL_01d6;
			}
		}
		goto IL_030a;
	}

	public static gfiwx btezo(byte[] p0)
	{
		gfiwx gfiwx2 = new gfiwx();
		hfnnn.oalpn(gfiwx2, p0, 0, p0.Length);
		return gfiwx2;
	}
}
