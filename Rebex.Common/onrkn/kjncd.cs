using System;

namespace onrkn;

internal static class kjncd
{
	private const double kftwl = -657435.0;

	private const double gfkin = 2958466.0;

	private const decimal bwthy = 10000m;

	private static readonly DateTime lbbyi = new DateTime(1899, 12, 30);

	private static readonly DateTime wdbsa = new DateTime(100, 1, 1);

	public static decimal gndni(long p0)
	{
		return pegyx(p0);
	}

	public static DateTime xpkid(double p0)
	{
		return cysxo(p0);
	}

	public static long wdexs(decimal p0)
	{
		return ibqvw(p0);
	}

	public static double hlvpo(DateTime p0)
	{
		return nftzh(p0);
	}

	private static DateTime cysxo(double p0)
	{
		if ((double.IsNaN(p0) ? true : false) || p0 <= -657435.0 || p0 >= 2958466.0)
		{
			throw new ArgumentException(brgjd.edcru("Invalid OLE Automation Date value {0}.", p0), "value");
		}
		if (Math.Sign(p0) == -1)
		{
			double num = Convert.ToInt32(p0);
			p0 = num + (num - p0);
		}
		return lbbyi.AddDays(p0);
	}

	private static double nftzh(DateTime p0)
	{
		if (p0 < wdbsa && 0 == 0)
		{
			if (p0.Date == DateTime.MinValue.Date && 0 == 0)
			{
				return TimeSpan.FromTicks(p0.Ticks).TotalDays;
			}
			throw new OverflowException(brgjd.edcru("The value '{0}' cannot be represented as an OLE Automation Date.", p0));
		}
		double num = p0.Subtract(lbbyi).TotalDays;
		if (Math.Sign(num) == -1)
		{
			double num2 = 0.0 - Math.Ceiling(0.0 - num);
			num = num2 + (num2 - num);
		}
		return num;
	}

	private static decimal pegyx(long p0)
	{
		return new decimal(p0) / 10000m;
	}

	private static long ibqvw(decimal p0)
	{
		return (long)Math.Round(p0 * 10000m);
	}
}
