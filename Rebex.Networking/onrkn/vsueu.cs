using System;
using Rebex.Net;

namespace onrkn;

internal class vsueu : crosq
{
	private readonly fhryo tfput;

	private readonly int wfddi;

	private readonly byte[] zkuci;

	private readonly byte[] fngny;

	private long sbjsf;

	public vsueu(agxpx previousState, fhryo transform, byte[] nonce, int? compressionLevel)
		: base(previousState, null, compressionLevel)
	{
		tfput = transform;
		wfddi = Math.Max(tfput.InputBlockSize, 8);
		zkuci = nonce;
		sbjsf = jlfbq.zdgzv(nonce, 4);
		fngny = new byte[4];
	}

	public override int vagtd(byte[] p0, int p1, byte[] p2)
	{
		p1 = lzctp(p0, p1, p2, 0);
		int p3 = ajscm(p2, p1);
		jlfbq.emxnl(zkuci, 4, sbjsf);
		sbjsf++;
		tfput.yirig(zkuci);
		Array.Copy(p2, 0, fngny, 0, 4);
		tfput.seoke(fngny);
		p3 = tfput.yajzn(p2, 4, p3, p2, 4);
		tfput.mdexb(p2, 4 + p3);
		gedjb();
		return p3 + 4 + 16;
	}

	private int ajscm(byte[] p0, int p1)
	{
		int num = (p1 / wfddi + 1) * wfddi;
		int num2 = num - p1 - 1;
		if (num2 < 4)
		{
			num += wfddi;
			num2 += wfddi;
		}
		int num3 = p1 + num2 + 1;
		if (num3 > 16777215)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Attempt to send a packet that is too long - {0} bytes.", num3));
		}
		jlfbq.lyicr(p0, 0, num3);
		p0[4] = (byte)num2;
		jtxhe.ubsib(p0, p1 + 5, num2);
		return num;
	}

	public override void bwbpr()
	{
		tfput.Dispose();
		base.bwbpr();
	}
}
