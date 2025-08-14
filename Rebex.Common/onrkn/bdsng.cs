using System.Security.Cryptography;

namespace onrkn;

internal static class bdsng
{
	public static int ykeyb = 8;

	internal static riucd gysto(ICryptoTransform p0)
	{
		return lesgh(p0, ykeyb);
	}

	internal static riucd lesgh(ICryptoTransform p0, int p1)
	{
		return p1 switch
		{
			2 => new iexya(p0), 
			4 => new vgnkg(p0), 
			8 => new rktqp(p0), 
			_ => new rioxq(p0), 
		};
	}
}
