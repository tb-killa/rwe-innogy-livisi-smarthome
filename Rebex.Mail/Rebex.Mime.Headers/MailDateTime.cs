using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public class MailDateTime : IHeader
{
	private static readonly string[] lpyhz = new string[7] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

	private static readonly string[] efqot = new string[12]
	{
		"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct",
		"Nov", "Dec"
	};

	private readonly DateTime xopxt;

	private readonly TimeSpan cajhi;

	public DateTime LocalTime => xopxt.ToLocalTime();

	public DateTime UniversalTime => xopxt;

	public DateTime OriginalTime
	{
		get
		{
			DateTime dateTime = xopxt;
			DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Unspecified);
			return dateTime2 + cajhi;
		}
	}

	public TimeSpan TimeZone => cajhi;

	public MailDateTime(DateTime localTime)
	{
		DateTime dateTime = localTime;
		localTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Local);
		xopxt = localTime.ToUniversalTime();
		cajhi = System.TimeZone.CurrentTimeZone.GetUtcOffset(localTime);
	}

	public static implicit operator MailDateTime(DateTime localTime)
	{
		return new MailDateTime(localTime);
	}

	public MailDateTime(DateTime universalTime, TimeSpan timeZone)
	{
		DateTime dateTime = universalTime;
		xopxt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
		cajhi = timeZone;
	}

	public IHeader Clone()
	{
		return new MailDateTime(xopxt, cajhi);
	}

	public override string ToString()
	{
		DateTime dateTime = xopxt + cajhi;
		string text = ((dateTime.Second == 0) ? "{0}, d {1} yyyy HH:mm {2}{3:00}{4:00}" : "{0}, d {1} yyyy HH:mm:ss {2}{3:00}{4:00}");
		string p = dateTime.ToString(text, CultureInfo.InvariantCulture);
		string text2 = efqot[dateTime.Month - 1];
		char c = '+';
		if (cajhi.Ticks < 0)
		{
			c = '-';
		}
		int num = Math.Abs(cajhi.Hours);
		int num2 = Math.Abs(cajhi.Minutes);
		return brgjd.edcru(p, lpyhz[(int)dateTime.DayOfWeek], text2, c, num, num2);
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(ToString());
	}

	private static int fbxbu(string p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0024;
		IL_0006:
		if (string.Compare(p0, lpyhz[num], StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			return num;
		}
		num++;
		goto IL_0024;
		IL_0024:
		if (num < lpyhz.Length)
		{
			goto IL_0006;
		}
		return -1;
	}

	private static int lcsbr(string p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0026;
		IL_0006:
		if (string.Compare(p0, efqot[num], StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			return num + 1;
		}
		num++;
		goto IL_0026;
		IL_0026:
		if (num < efqot.Length)
		{
			goto IL_0006;
		}
		return 0;
	}

	private static int qhcdv(stzvh p0, int p1, int p2, bool p3)
	{
		p0.hdpha();
		int num = 0;
		int num2;
		for (num2 = p1; num2 > 0; num2--)
		{
			if (p0.zsywy && 0 == 0)
			{
				if (p3 && 0 == 0)
				{
					throw MimeException.dngxr(p0.bauax, "Header ends prematurely.");
				}
				return 0;
			}
			int num3 = p0.havrs - 48;
			if (num3 < 0 || num3 > 9)
			{
				if (p3 && 0 == 0)
				{
					throw MimeException.dngxr(p0.bauax, "Digit expected.");
				}
				return 0;
			}
			num = 10 * num + num3;
			p0.pfdcf();
		}
		num2 = p2 - p1;
		while (num2 > 0 && !p0.zsywy)
		{
			int num4 = p0.havrs - 48;
			if (num4 < 0 || num4 > 9)
			{
				break;
			}
			num = 10 * num + num4;
			p0.pfdcf();
			num2--;
		}
		return num;
	}

	private static TimeSpan iakqw(stzvh p0, bool p1)
	{
		int hours = qhcdv(p0, 2, 2, p3: false);
		if (p1 && 0 == 0 && (!p0.zsywy || 1 == 0) && p0.havrs == ':')
		{
			p0.pfdcf();
		}
		int minutes = qhcdv(p0, 2, 2, p3: false);
		return new TimeSpan(hours, minutes, 0);
	}

	private static TimeSpan mskft(stzvh p0)
	{
		int hours = qhcdv(p0, 2, 2, p3: false);
		p0.pfdcf();
		int minutes = qhcdv(p0, 2, 2, p3: false);
		return new TimeSpan(hours, minutes, 0);
	}

	private static int vrdkh(string p0)
	{
		string key;
		if ((key = p0.ToUpper(CultureInfo.InvariantCulture)) != null && 0 == 0)
		{
			if (czzgh.zkgyn == null || 1 == 0)
			{
				czzgh.zkgyn = new Dictionary<string, int>(13)
				{
					{ "UT", 0 },
					{ "UTC", 1 },
					{ "GMT", 2 },
					{ "AST", 3 },
					{ "ADT", 4 },
					{ "EST", 5 },
					{ "EDT", 6 },
					{ "CST", 7 },
					{ "CDT", 8 },
					{ "MST", 9 },
					{ "MDT", 10 },
					{ "PST", 11 },
					{ "PDT", 12 }
				};
			}
			if (czzgh.zkgyn.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
					return 0;
				case 3:
					return -4;
				case 4:
					return -3;
				case 5:
					return -5;
				case 6:
					return -4;
				case 7:
					return -6;
				case 8:
					return -5;
				case 9:
					return -7;
				case 10:
					return -6;
				case 11:
					return -8;
				case 12:
					return -7;
				}
			}
		}
		return int.MinValue;
	}

	internal static IHeader aohzm(stzvh p0)
	{
		TimeSpan timeSpan = new TimeSpan(0L);
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		bool flag = false;
		if (flag)
		{
			goto IL_0022;
		}
		goto IL_041e;
		IL_0022:
		p0.hdpha(',', ';');
		if (!p0.zsywy || 1 == 0)
		{
			if (p0.havrs == '+')
			{
				p0.pfdcf();
				timeSpan = iakqw(p0, p1: true);
			}
			else if (p0.havrs == '-')
			{
				p0.pfdcf();
				int bauax = p0.bauax;
				timeSpan = iakqw(p0, p1: false).Negate();
				if (timeSpan == TimeSpan.Zero && 0 == 0 && (!p0.zsywy || 1 == 0) && p0.havrs == ':')
				{
					p0.bauax = bauax;
					flag = true;
					if (flag)
					{
						goto IL_041e;
					}
				}
				p0.bauax = bauax;
				timeSpan = iakqw(p0, p1: true).Negate();
			}
			else if (p0.havrs >= '0' && p0.havrs <= '9')
			{
				bool flag2 = p0.havrs == '0';
				int num7 = qhcdv(p0, 1, 5, p3: true);
				if (!p0.zsywy || 1 == 0)
				{
					if (p0.havrs >= '0' && p0.havrs <= '9')
					{
						p0.pgtcy();
						goto IL_041e;
					}
					if (p0.havrs == '-')
					{
						p0.pfdcf();
					}
					else
					{
						p0.hdpha();
						if (p0.havrs == ':')
						{
							num4 = num7;
							p0.pfdcf();
							p0.hdpha();
							num5 = qhcdv(p0, 1, 2, p3: true);
							p0.hdpha();
							if ((!p0.zsywy || 1 == 0) && p0.havrs == ':')
							{
								p0.pfdcf();
								p0.hdpha();
								num6 = qhcdv(p0, 1, 2, p3: true);
							}
							goto IL_041e;
						}
					}
				}
				if (num7 >= 1900)
				{
					num = num7;
				}
				else if (num7 > 70 && num7 <= 99)
				{
					if (num < 0)
					{
						num = 1900 + num7;
					}
				}
				else if (num3 > 0 && num < 0 && ((num7 >= 0 && num7 <= 9 && (flag2 ? true : false)) || (num7 >= 10 && num7 <= 99)))
				{
					num = 2000 + num7;
				}
				else if (num7 >= 1 && num7 <= 31 && num3 < 0)
				{
					num3 = num7;
				}
			}
			else if (p0.havrs == '.')
			{
				p0.pfdcf();
				if (p0.zsywy ? true : false)
				{
					goto IL_0431;
				}
				p0.pgtcy();
			}
			else
			{
				string text = ((p0.havrs != '"') ? p0.pgtcy().ToString() : p0.oypkh().ToString());
				int num7 = fbxbu(text);
				if (num7 < 0)
				{
					num7 = lcsbr(text);
					if (num7 >= 1)
					{
						if (num2 < 0)
						{
							num2 = num7;
						}
					}
					else
					{
						num7 = vrdkh(text);
						if (num7 != int.MinValue)
						{
							timeSpan = new TimeSpan(num7, 0, 0);
						}
						else
						{
							int num8 = text.IndexOf('-');
							if (num8 > 0 && num8 < text.Length - 1)
							{
								num7 = lcsbr(text.Substring(0, num8));
								if (num7 >= 1)
								{
									if (num2 < 0)
									{
										num2 = num7;
									}
									p0.bauax -= text.Length - num8 - 1;
									goto IL_041e;
								}
							}
							if ((!p0.zsywy || 1 == 0) && p0.havrs == ':')
							{
								num8 = text.IndexOfAny(new char[2] { '-', '+' });
								if (num8 >= 0)
								{
									p0.bauax -= text.Length - num8 - 1;
									switch (text[num8])
									{
									case '+':
										timeSpan = mskft(p0);
										break;
									case '-':
										timeSpan = mskft(p0).Negate();
										break;
									}
								}
							}
						}
					}
				}
			}
			goto IL_041e;
		}
		goto IL_0431;
		IL_041e:
		if (!p0.zsywy)
		{
			goto IL_0022;
		}
		goto IL_0431;
		IL_0431:
		if (num < 0)
		{
			num = DateTime.Now.Year;
		}
		if (num == 9999)
		{
			timeSpan = TimeSpan.Zero;
		}
		if (num2 < 0)
		{
			num2 = 1;
		}
		if (num2 > 12)
		{
			num2 = 12;
		}
		if (num3 < 0)
		{
			num3 = 1;
		}
		if (num4 > 23)
		{
			num4 = 0;
		}
		if (num5 > 59)
		{
			num5 = 0;
		}
		if (num6 > 59)
		{
			num6 = 0;
		}
		if (timeSpan.TotalHours < -14.0 || timeSpan.TotalHours > 14.0)
		{
			timeSpan = new TimeSpan(0L);
		}
		if (flag && 0 == 0)
		{
			num4 = ((num4 != 0 && 0 == 0) ? (24 - num4) : 0);
		}
		DateTime universalTime;
		try
		{
			universalTime = new DateTime(num, num2, num3, num4, num5, num6, DateTimeKind.Utc);
		}
		catch (ArgumentOutOfRangeException)
		{
			universalTime = new DateTime(num, num2, 1, num4, num5, num6, DateTimeKind.Utc).AddDays(DateTime.DaysInMonth(num, num2) - 1);
		}
		if (flag && 0 == 0)
		{
			universalTime = universalTime.AddDays(-1.0);
		}
		universalTime -= timeSpan;
		return new MailDateTime(universalTime, timeSpan);
	}
}
