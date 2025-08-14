using System;

namespace onrkn;

internal static class lwuxa
{
	private static Func<string, bool> mhgvj;

	private static Func<string, bool> iieee;

	private static Func<string, bool> xwmmk;

	private static Func<string, bool> qmkin;

	public static Func<string, bool> uqeet
	{
		get
		{
			return mhgvj;
		}
		private set
		{
			mhgvj = value;
		}
	}

	public static Func<string, bool> piqpl
	{
		get
		{
			return iieee;
		}
		private set
		{
			iieee = value;
		}
	}

	public static Func<string, bool> zzxcy
	{
		get
		{
			return xwmmk;
		}
		private set
		{
			xwmmk = value;
		}
	}

	public static Func<string, bool> llima
	{
		get
		{
			return qmkin;
		}
		private set
		{
			qmkin = value;
		}
	}

	static lwuxa()
	{
		fobhw();
	}

	private static void fobhw()
	{
		uqeet = clugp;
		piqpl = uqeet.zrwgu(fwblf);
		zzxcy = uqeet.zrwgu(dqybg);
		llima = uqeet.zrwgu(hmghm);
	}

	private static bool dqybg(string p0)
	{
		return p0.StartsWith("dsa", StringComparison.OrdinalIgnoreCase);
	}

	private static bool fwblf(string p0)
	{
		return p0.StartsWith("rsa", StringComparison.OrdinalIgnoreCase);
	}

	private static bool clugp(string p0)
	{
		return !string.IsNullOrEmpty(p0);
	}

	public static bool hmghm(string p0)
	{
		return p0.StartsWith("dh", StringComparison.OrdinalIgnoreCase);
	}
}
