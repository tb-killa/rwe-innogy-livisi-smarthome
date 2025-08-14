using System;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class ufjug : aicnd
{
	private readonly ICryptoTransform qzzjs;

	private readonly int avwhv;

	private int aiwbr;

	public override int ptjhi => 4;

	public ufjug(agxpx previousState, IHashTransform mac, ICryptoTransform transform, bool zlib)
		: base(previousState, mac, zlib)
	{
		qzzjs = transform;
		avwhv = mac.HashSize / 8;
		aiwbr = -1;
	}

	public override int iadch(byte[] p0, int p1, int p2)
	{
		if (aiwbr >= 0)
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
		return (aiwbr = p0[p1 + 1] * 65536 + p0[p1 + 2] * 256 + p0[p1 + 3]) + 4 + avwhv;
	}

	public override int keqao(ArraySegment<byte> p0, out byte[] p1)
	{
		if (aiwbr < 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		bool flag = false;
		byte[] array = aanup(p0.Array, p0.Offset, aiwbr + 4);
		int num = 0;
		if (num != 0)
		{
			goto IL_003b;
		}
		goto IL_0064;
		IL_0064:
		if (num >= avwhv)
		{
			if (flag && 0 == 0)
			{
				throw new SshException(tcpjq.zbwim, "Message authentication code is invalid.");
			}
			try
			{
				qzzjs.TransformBlock(p0.Array, p0.Offset + 4, aiwbr, p0.Array, p0.Offset + 4);
			}
			catch (Exception inner)
			{
				throw new SshException(tcpjq.zbwim, "Error while decrypting data.", inner);
			}
			int num2 = p0.Array[p0.Offset + 4];
			int result = vinqr(p0, out p1, aiwbr - num2 - 1);
			aiwbr = -1;
			return result;
		}
		goto IL_003b;
		IL_003b:
		flag |= array[num] != p0.Array[p0.Offset + aiwbr + 4 + num];
		num++;
		goto IL_0064;
	}

	public override void bwbpr()
	{
		qzzjs.Dispose();
		base.bwbpr();
	}
}
