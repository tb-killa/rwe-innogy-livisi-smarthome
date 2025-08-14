using System;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class vumfh : bpnki
{
	private byte[] plncb = new byte[8];

	private IHashTransform fpvod;

	private TlsProtocol vqyez;

	public TlsProtocol cvjqj => vqyez;

	public byte[] grrvs => plncb;

	public vumfh(IHashTransform mac, TlsProtocol version)
	{
		fpvod = mac;
		vqyez = version;
	}

	public void sldfg()
	{
		int num = 7;
		if (num == 0)
		{
			goto IL_0006;
		}
		goto IL_003d;
		IL_0006:
		if (plncb[num] < byte.MaxValue)
		{
			plncb[num]++;
			return;
		}
		plncb[num] = 0;
		num--;
		goto IL_003d;
		IL_003d:
		if (num >= 0)
		{
			goto IL_0006;
		}
		Array.Clear(plncb, 0, 8);
	}

	public byte[] cjumt(byte[] p0, int p1, int p2)
	{
		fpvod.Reset();
		fpvod.Process(plncb, 0, 8);
		if (vqyez >= TlsProtocol.TLS10)
		{
			fpvod.Process(p0, p1, p2);
		}
		else
		{
			fpvod.Process(p0, p1, 1);
			fpvod.Process(p0, p1 + 3, p2 - 3);
		}
		return fpvod.GetHash();
	}

	public override void egphd()
	{
		fpvod.Dispose();
		base.egphd();
	}
}
