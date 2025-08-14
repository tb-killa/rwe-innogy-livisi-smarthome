using System;

namespace onrkn;

internal class qqxey
{
	public const char tmdla = '\uffff';

	public static readonly char[] itedo = new char[6] { ' ', '\t', '\r', '\n', '\v', '\f' };

	public static readonly char[] grtbj = new char[25]
	{
		' ', '\t', '\r', '\n', '\v', '\f', '\u0085', '\u00a0', '\u1680', '\u2000',
		'\u2001', '\u2002', '\u2003', '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200a',
		'\u2028', '\u2029', '\u202f', '\u205f', '\u3000'
	};

	private readonly char[] ktpxe;

	private int qztpe;

	private string chrem;

	public string avksa
	{
		get
		{
			return chrem;
		}
		private set
		{
			chrem = value;
		}
	}

	public int snrid
	{
		get
		{
			return qztpe;
		}
		set
		{
			if (value < 0 || value > avksa.Length)
			{
				throw hifyx.nztrs("value", value, "Argument is out of range of valid values.");
			}
			qztpe = value;
		}
	}

	public int vhybw => avksa.Length - snrid;

	public bool tkduk => snrid >= avksa.Length;

	public char eelpd
	{
		get
		{
			if (!tkduk || 1 == 0)
			{
				return avksa[snrid];
			}
			return '\uffff';
		}
	}

	public qqxey(string text)
		: this(text, itedo)
	{
	}

	public qqxey(string text, char[] whitespaces)
	{
		if (text == null || 1 == 0)
		{
			throw new ArgumentNullException("text");
		}
		if (whitespaces == null || 1 == 0)
		{
			throw new ArgumentNullException("whitespaces");
		}
		avksa = text;
		ktpxe = whitespaces;
	}

	public bool ejpqg()
	{
		if (!tkduk || 1 == 0)
		{
			return Array.IndexOf(ktpxe, avksa[snrid]) >= 0;
		}
		return false;
	}

	public void oqtdm()
	{
		while ((!tkduk || 1 == 0) && (ejpqg() ? true : false))
		{
			snrid++;
		}
	}

	public char cehpy(bool p0 = false)
	{
		if (tkduk && 0 == 0)
		{
			if (p0 && 0 == 0)
			{
				throw new pqotq("Not enough data.");
			}
			return '\uffff';
		}
		return avksa[snrid++];
	}

	public string chaht(int p0)
	{
		if (p0 > vhybw)
		{
			throw new pqotq("Not enough data.");
		}
		snrid += p0;
		return avksa.Substring(snrid - p0, p0);
	}

	public char cycsv(char p0)
	{
		if (eelpd != p0)
		{
			throw new pqotq(brgjd.edcru("Unexpected character at position {0}.", snrid));
		}
		return cehpy();
	}

	public char kyvqr(params char[] p0)
	{
		if (Array.IndexOf(p0, eelpd) < 0)
		{
			throw new pqotq(brgjd.edcru("Unexpected character at position {0}.", snrid));
		}
		return cehpy();
	}

	public char ivswy(bool p0 = true)
	{
		if ((tkduk ? true : false) || ejpqg())
		{
			return cehpy(p0);
		}
		throw new pqotq("Not a white space.");
	}

	public char buoqv(char p0, bool p1 = true)
	{
		if ((tkduk ? true : false) || eelpd == p0 || ejpqg())
		{
			return cehpy(p1);
		}
		throw new pqotq(brgjd.edcru("Unexpected character at position {0}.", snrid));
	}

	public string odgye(bool p0 = false)
	{
		int num = snrid;
		while ((!tkduk || 1 == 0) && !ejpqg())
		{
			snrid++;
		}
		int num2 = snrid;
		if (p0 && 0 == 0)
		{
			oqtdm();
		}
		if (num == num2)
		{
			return string.Empty;
		}
		return avksa.Substring(num, num2 - num);
	}

	public string ebqio(params char[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("separators");
		}
		int num = snrid;
		while ((!tkduk || 1 == 0) && Array.IndexOf(p0, avksa[snrid]) < 0)
		{
			snrid++;
		}
		if (num == snrid)
		{
			return string.Empty;
		}
		return avksa.Substring(num, snrid - num);
	}

	public int vuszx(int? p0 = null, int? p1 = null)
	{
		int num = p0 ?? int.MaxValue;
		if (num <= 0)
		{
			throw hifyx.nztrs("maxDigits", p0, "Argument must be positive number.");
		}
		int num2 = snrid;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0037;
		}
		goto IL_0083;
		IL_0087:
		if (num2 == snrid)
		{
			if (p1.HasValue && 0 == 0)
			{
				return p1.Value;
			}
			throw new pqotq("Not a number.");
		}
		return int.Parse(avksa.Substring(num2, snrid - num2));
		IL_0037:
		if ((!tkduk || 1 == 0) && avksa[snrid] >= '0' && avksa[snrid] <= '9')
		{
			snrid++;
			num3++;
			goto IL_0083;
		}
		goto IL_0087;
		IL_0083:
		if (num3 < num)
		{
			goto IL_0037;
		}
		goto IL_0087;
	}

	public int vwuje(int p0, bool p1 = false)
	{
		if (p0 > vhybw)
		{
			throw new pqotq("Not enough data.");
		}
		int num = snrid;
		bool flag = true;
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_002a;
		}
		goto IL_00a2;
		IL_00a2:
		if (num3 >= p0)
		{
			snrid = num;
			return num2;
		}
		goto IL_002a;
		IL_002a:
		if (flag && 0 == 0 && p1 && 0 == 0 && avksa[num] == ' ')
		{
			num++;
		}
		else
		{
			if (avksa[num] < '0' || avksa[num] > '9')
			{
				throw new pqotq("Not a number.");
			}
			num2 *= 10;
			num2 += avksa[num++] - 48;
			flag = false;
		}
		num3++;
		goto IL_00a2;
	}

	public int ivcgj(int p0)
	{
		int num = vwuje(2);
		if (brgjd.kmfkd(eelpd) && 0 == 0)
		{
			num *= 100;
			return num + vwuje(2);
		}
		if (num < p0)
		{
			return num + 2000;
		}
		return num + 1900;
	}

	public TimeSpan jykum()
	{
		if (tkduk && 0 == 0)
		{
			throw new pqotq("Not enough data.");
		}
		if (eelpd == '+' || eelpd == '-' || brgjd.kmfkd(eelpd))
		{
			bool flag = eelpd == '-';
			if (!brgjd.kmfkd(eelpd) || 1 == 0)
			{
				snrid++;
			}
			int num = vwuje(2);
			if (eelpd == ':')
			{
				snrid++;
			}
			int num2 = vwuje(2);
			if (num > 23 || num2 > 59)
			{
				throw new pqotq("Not a valid time zone.");
			}
			TimeSpan result = new TimeSpan(num, num2, 0);
			if (flag && 0 == 0)
			{
				return result.Negate();
			}
			return result;
		}
		int num3 = snrid;
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_00e5;
		}
		goto IL_010e;
		IL_010e:
		if (num4 >= 3 || tkduk)
		{
			return brgjd.weval(avksa.Substring(num3, snrid - num3));
		}
		goto IL_00e5;
		IL_00e5:
		if (brgjd.mcaae(eelpd) && 0 == 0)
		{
			snrid++;
		}
		num4++;
		goto IL_010e;
	}
}
