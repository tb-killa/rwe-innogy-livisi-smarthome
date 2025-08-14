using System;
using Rebex.Net;

namespace onrkn;

internal class xwmdx : aicnd
{
	private readonly byte[] wifyt;

	private readonly byte[] blzic;

	private readonly byte[] ppbge;

	private readonly byte[] ayheq;

	private readonly byte[] ewqvv;

	private int amued;

	private vifpo fqkja;

	private vifpo lbgmq;

	private bvxhw yjsfr;

	private byte[] mdybl;

	public override int ptjhi => 4;

	public xwmdx(agxpx previousState, byte[] key, bool zlib)
		: base(previousState, null, zlib)
	{
		wifyt = new byte[32];
		blzic = new byte[32];
		Array.Copy(key, 32, wifyt, 0, 32);
		Array.Copy(key, 0, blzic, 0, 32);
		ppbge = new byte[12];
		ayheq = new byte[32];
		ewqvv = new byte[4];
		amued = -1;
		mdybl = new byte[16];
		fqkja = null;
		lbgmq = null;
		yjsfr = null;
	}

	public override int iadch(byte[] p0, int p1, int p2)
	{
		if (amued >= 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		if (p2 < ptjhi)
		{
			throw new InvalidOperationException("Unexpected decoder error.");
		}
		gedjb();
		jlfbq.emxnl(ppbge, 4, syelh);
		if (fqkja == null || 1 == 0)
		{
			fqkja = vifpo.nukst(wifyt, ppbge, 0);
		}
		else
		{
			fqkja.vpvaw(ppbge, 0);
		}
		try
		{
			fqkja.ivxhj(p0, p1, 4, ewqvv, 0);
		}
		catch (Exception inner)
		{
			throw new SshException(tcpjq.zbwim, "Error while decrypting data.", inner);
		}
		return 4;
	}

	public override int lyhbb(byte[] p0, int p1, int p2)
	{
		if (ewqvv[0] != 0 && 0 == 0)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		return (amued = jlfbq.yyxrz(ewqvv, 0)) + 4 + 16;
	}

	public override int keqao(ArraySegment<byte> p0, out byte[] p1)
	{
		if (amued < 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		byte[] array = p0.Array;
		int offset = p0.Offset;
		int count = p0.Count;
		bool flag;
		try
		{
			Array.Clear(ayheq, 0, ayheq.Length);
			if (lbgmq == null || 1 == 0)
			{
				lbgmq = vifpo.nukst(blzic, ppbge, 0);
			}
			else
			{
				lbgmq.vpvaw(ppbge, 0);
			}
			lbgmq.ivxhj(ayheq, 0, 32, ayheq, 0);
			if (count < amued + 4 + 16)
			{
				flag = false;
				if (!flag)
				{
					goto IL_0135;
				}
			}
			if (yjsfr == null || 1 == 0)
			{
				yjsfr = bvxhw.aztdt(ayheq);
			}
			else
			{
				yjsfr.gaxag(ayheq);
			}
			yjsfr.Process(array, offset, amued + 4);
			yjsfr.zzsom(mdybl);
			flag = jlfbq.ccahg(mdybl, 0, array, offset + amued + 4, 16);
			goto IL_0135;
			IL_0135:
			if (flag && 0 == 0)
			{
				lbgmq.ivxhj(array, offset + 4, amued, array, offset + 4);
				ewqvv.CopyTo(array, offset);
			}
		}
		catch (Exception inner)
		{
			throw new SshException(tcpjq.zbwim, "Error while decrypting data.", inner);
		}
		if (!flag || 1 == 0)
		{
			throw new SshException(tcpjq.zbwim, "Message authentication code is invalid.");
		}
		int num = array[offset + 4];
		if (num < 4)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		int num2 = amued - num - 1;
		if (num2 < 1)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		int result = vinqr(p0, out p1, num2);
		amued = -1;
		return result;
	}

	public override void bwbpr()
	{
		if (fqkja != null && 0 == 0)
		{
			fqkja.Dispose();
		}
		if (lbgmq != null && 0 == 0)
		{
			lbgmq.Dispose();
		}
		if (yjsfr != null && 0 == 0)
		{
			((IDisposable)yjsfr).Dispose();
		}
		base.bwbpr();
	}
}
