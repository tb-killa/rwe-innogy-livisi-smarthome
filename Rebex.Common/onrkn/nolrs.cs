using System;
using Rebex;

namespace onrkn;

internal static class nolrs
{
	public static bool sftbn(this sjhqe p0, LogLevel p1)
	{
		return p0.Level <= p1;
	}

	public static void byfnx(this sjhqe p0, LogLevel p1, string p2, string p3, params object[] p4)
	{
		if (p0.Level <= p1)
		{
			string p5 = brgjd.edcru(p3, p4);
			p0.rfpvf(p1, p2, p5);
		}
	}

	public static void gstqy(this sjhqe p0, LogLevel p1, string p2, Func<string> p3)
	{
		if (p0.Level <= p1)
		{
			p0.rfpvf(p1, p2, p3());
		}
	}

	public static void vasdg(this sjhqe p0, LogLevel p1, string p2, string p3, nxtme<byte> p4)
	{
		if (p0.Level <= p1)
		{
			p0.iyauk(p1, p2, p3, p4.lthjd, p4.frlfs, p4.tvoem);
		}
	}

	public static void tivkc(this sjhqe p0, LogLevel p1, string p2, string p3, Exception p4)
	{
		if (p0.Level <= p1)
		{
			p0.rfpvf(p1, p2, brgjd.edcru("{0} {1}", p3, p4));
		}
	}
}
