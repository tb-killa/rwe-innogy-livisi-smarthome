using System;
using System.Security.Cryptography;

namespace onrkn;

internal class fmjea : zwcrs, gajry, ktjcg, IDisposable
{
	public const string edpxx = "Authentication tag check failed.";

	public const string bucng = "Authentication tag must be exactly 16 bytes long.";

	private nxtme<byte> uxqcy;

	private readonly byte[] skzgj;

	private readonly nxtme<byte> tzcvm;

	public fmjea(byte[] key)
		: base(key, cmzdt.uzvuc)
	{
		skzgj = sxztb<byte>.ahblv.vfhlp(16);
		tzcvm = skzgj.pynmq(0, 16);
	}

	public void tglzc(byte[] p0, int p1, int p2)
	{
		dahxy.valft(p0, "buffer", p1, "offset", p2, "count");
		if (p2 != 16)
		{
			throw new ArgumentException("Authentication tag must be exactly 16 bytes long.");
		}
		uxqcy = p0.pynmq(p1, p2);
	}

	protected override void rjybx(bool p0)
	{
		if (skzgj != null && 0 == 0)
		{
			Array.Clear(skzgj, 0, skzgj.Length);
			sxztb<byte>.ahblv.uqydw(skzgj);
		}
		base.rjybx(p0);
	}

	protected override void gocdi()
	{
		base.gocdi();
		nxtme<byte> p = tzcvm;
		nxtme<byte> p2 = uxqcy;
		uqckg(p);
		if (!jlfbq.xzsez(p, p2) || 1 == 0)
		{
			throw new CryptographicException("Authentication tag check failed.");
		}
	}
}
