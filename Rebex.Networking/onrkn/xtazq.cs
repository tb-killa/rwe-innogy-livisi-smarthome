using System;
using Rebex.Net;

namespace onrkn;

internal class xtazq : aicnd
{
	private readonly gajry vugjc;

	private readonly byte[] khbwz;

	private readonly byte[] vvsui;

	private int vhsro;

	private long vlqqm;

	public override int ptjhi => 4;

	public xtazq(agxpx previousState, gajry transform, byte[] nonce, bool zlib)
		: base(previousState, null, zlib)
	{
		vugjc = transform;
		khbwz = nonce;
		vlqqm = jlfbq.zdgzv(nonce, 4);
		vvsui = new byte[4];
		vhsro = -1;
	}

	public override int iadch(byte[] p0, int p1, int p2)
	{
		if (vhsro >= 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		return 4;
	}

	public override int lyhbb(byte[] p0, int p1, int p2)
	{
		if (p0[p1] != 0 && 0 == 0)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		return (vhsro = p0[p1 + 1] * 65536 + p0[p1 + 2] * 256 + p0[p1 + 3]) + 4 + 16;
	}

	public override int keqao(ArraySegment<byte> p0, out byte[] p1)
	{
		if (vhsro < 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		int num2;
		try
		{
			vugjc.tglzc(p0.Array, p0.Offset + 4 + vhsro, 16);
			jlfbq.emxnl(khbwz, 4, vlqqm);
			vlqqm++;
			vugjc.yirig(khbwz);
			Array.Copy(p0.Array, p0.Offset, vvsui, 0, 4);
			vugjc.seoke(vvsui);
			vugjc.yajzn(p0.Array, p0.Offset + 4, vhsro, p0.Array, p0.Offset + 4);
			int num = p0.Array[p0.Offset + 4];
			if (num < 4)
			{
				throw new SshException(tcpjq.svqut, "Received invalid packet.");
			}
			num2 = vhsro - num - 1;
			if (num2 < 1)
			{
				throw new SshException(tcpjq.svqut, "Received invalid packet.");
			}
			vhsro = -1;
		}
		catch (Exception inner)
		{
			throw new SshException(tcpjq.zbwim, "Error while decrypting data.", inner);
		}
		gedjb();
		return vinqr(p0, out p1, num2);
	}

	public override void bwbpr()
	{
		vugjc.Dispose();
		base.bwbpr();
	}
}
