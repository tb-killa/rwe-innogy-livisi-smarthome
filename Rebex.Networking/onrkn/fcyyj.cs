using System;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class fcyyj : aicnd
{
	private readonly ICryptoTransform oqthm;

	private readonly bool wkpbh;

	private readonly int ubulc;

	private readonly int ggfto;

	private int rcfrv;

	private int tgvuu;

	public override int ptjhi => ggfto;

	public fcyyj(agxpx previousState, IHashTransform mac, ICryptoTransform transform, CipherMode mode, bool zlib)
		: base(previousState, mac, zlib)
	{
		oqthm = transform;
		wkpbh = mode == CipherMode.CBC;
		ubulc = mac.HashSize / 8;
		ggfto = Math.Max(oqthm.InputBlockSize, 8);
		rcfrv = -1;
	}

	public override int iadch(byte[] p0, int p1, int p2)
	{
		if (rcfrv >= 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		try
		{
			oqthm.TransformBlock(p0, p1, ggfto, p0, p1);
			return ggfto;
		}
		catch (Exception inner)
		{
			throw new SshException(tcpjq.zbwim, "Error while decrypting data.", inner);
		}
	}

	public override int lyhbb(byte[] p0, int p1, int p2)
	{
		if (p0[p1] != 0 && 0 == 0)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		int num = p0[p1 + 1] * 65536 + p0[p1 + 2] * 256 + p0[p1 + 3];
		tgvuu = p0[p1 + 4];
		if (tgvuu < 4)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		int num2 = num - tgvuu - 1;
		if (num2 < 1)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		rcfrv = num2;
		return num + 4 + ubulc;
	}

	public override int keqao(ArraySegment<byte> p0, out byte[] p1)
	{
		if (rcfrv < 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		int num = rcfrv + tgvuu + 5 - ggfto;
		if (num > 0)
		{
			try
			{
				oqthm.TransformBlock(p0.Array, p0.Offset + ggfto, num, p0.Array, p0.Offset + ggfto);
			}
			catch (Exception inner)
			{
				throw new SshException(tcpjq.zbwim, "Error while decrypting data.", inner);
			}
		}
		bool flag = false;
		byte[] array = aanup(p0.Array, p0.Offset, num + ggfto);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_009f;
		}
		goto IL_00cc;
		IL_009f:
		flag |= array[num2] != p0.Array[p0.Offset + num + ggfto + num2];
		num2++;
		goto IL_00cc;
		IL_00cc:
		if (num2 >= ubulc)
		{
			if (flag && 0 == 0)
			{
				throw new SshException(tcpjq.zbwim, "Message authentication code is invalid.");
			}
			int result = vinqr(p0, out p1, rcfrv);
			rcfrv = -1;
			return result;
		}
		goto IL_009f;
	}

	public override void bwbpr()
	{
		oqthm.Dispose();
		base.bwbpr();
	}
}
