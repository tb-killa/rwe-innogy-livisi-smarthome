using System;
using System.Globalization;
using System.Text;

namespace onrkn;

internal static class cehpk
{
	public static void dlvlk(this StringBuilder p0, string p1, object p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("builder");
		}
		p0.AppendFormat(CultureInfo.InvariantCulture, p1, new object[1] { p2 });
	}

	public static void mwigd(this StringBuilder p0, string p1, object p2, object p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("builder");
		}
		p0.AppendFormat(CultureInfo.InvariantCulture, p1, new object[2] { p2, p3 });
	}

	public static void fazck(this StringBuilder p0, string p1, params object[] p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("builder");
		}
		p0.AppendFormat(CultureInfo.InvariantCulture, p1, p2);
	}

	public static void arumx(this StringBuilder p0, object p1)
	{
		p0.dlvlk("{0}", p1);
	}
}
