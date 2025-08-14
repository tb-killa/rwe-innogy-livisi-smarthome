using System;

namespace onrkn;

internal class rywyx : crosq
{
	private readonly byte[] hzodi;

	private readonly byte[] epjhb;

	private readonly byte[] hqoiw;

	private readonly byte[] mvqws;

	private bvxhw blogr;

	private vifpo czpdy;

	private vifpo xkwjp;

	public rywyx(agxpx previousState, byte[] key, int? compressionLevel)
		: base(previousState, null, compressionLevel)
	{
		hzodi = new byte[32];
		epjhb = new byte[32];
		Array.Copy(key, 32, hzodi, 0, 32);
		Array.Copy(key, 0, epjhb, 0, 32);
		hqoiw = new byte[12];
		mvqws = new byte[32];
		czpdy = null;
		xkwjp = null;
		blogr = null;
	}

	public override int vagtd(byte[] p0, int p1, byte[] p2)
	{
		p1 = lzctp(p0, p1, p2, 0);
		int num = (p1 | 7) - p1;
		if (num < 4)
		{
			num += 8;
		}
		gedjb();
		jlfbq.emxnl(hqoiw, 4, syelh);
		jlfbq.lyicr(p2, 0, p1 + num + 1);
		if (czpdy == null || 1 == 0)
		{
			czpdy = vifpo.nukst(hzodi, hqoiw, 0);
		}
		else
		{
			czpdy.vpvaw(hqoiw, 0);
		}
		czpdy.ivxhj(p2, 0, 4, p2, 0);
		if (xkwjp == null || 1 == 0)
		{
			xkwjp = vifpo.nukst(epjhb, hqoiw, 0);
		}
		else
		{
			xkwjp.vpvaw(hqoiw, 0);
		}
		Array.Clear(mvqws, 0, mvqws.Length);
		xkwjp.ivxhj(mvqws, 0, 32, mvqws, 0);
		p2[4] = (byte)num;
		jtxhe.ubsib(p2, p1 + 5, num);
		p1 += num + 1;
		xkwjp.ivxhj(p2, 4, p1, p2, 4);
		if (blogr == null || 1 == 0)
		{
			blogr = bvxhw.aztdt(mvqws);
		}
		else
		{
			blogr.gaxag(mvqws);
		}
		blogr.Process(p2, 0, p1 + 4);
		blogr.zzsom(p2.myshu(p1 + 4, 16));
		return p1 + 4 + 16;
	}

	public override void bwbpr()
	{
		if (czpdy != null && 0 == 0)
		{
			czpdy.Dispose();
		}
		if (xkwjp != null && 0 == 0)
		{
			xkwjp.Dispose();
		}
		if (blogr != null && 0 == 0)
		{
			((IDisposable)blogr).Dispose();
		}
		base.bwbpr();
	}
}
