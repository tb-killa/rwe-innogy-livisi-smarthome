using System;

namespace onrkn;

internal static class wcjpz
{
	public static void qtynj<TR, TRR>(this jicrh<TR> p0, njvzu<TRR> p1) where TRR : TR
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourceTask");
		}
		if (!p1.IsCompleted || 1 == 0)
		{
			throw new ArgumentException("sourceTask");
		}
		if (!p0.jyefr(p1))
		{
			p0.spaxx((TR)(object)p1.islme);
		}
	}

	public static void txsju<TR>(this jicrh<TR> p0, exkzi p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourceTask");
		}
		if (!p1.IsCompleted || 1 == 0)
		{
			throw new ArgumentException("sourceTask");
		}
		if (!p0.jyefr(p1))
		{
			p0.spaxx(default(TR));
		}
	}

	private static bool jyefr<TR>(this jicrh<TR> p0, exkzi p1)
	{
		if (p1.ijeei && 0 == 0)
		{
			p0.feggc(p1.mnscz.mfkfw);
			return true;
		}
		if (p1.lctag && 0 == 0)
		{
			p0.ajblh();
			return true;
		}
		return false;
	}
}
