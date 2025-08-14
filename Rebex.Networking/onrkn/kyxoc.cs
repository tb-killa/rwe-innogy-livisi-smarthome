using System;
using Rebex.Net;

namespace onrkn;

internal class kyxoc : agxpx
{
	private int poxcp = -1;

	private int ocsof = -1;

	private int bbqnm;

	private byte[] rlzlb = new byte[0];

	public override int syelh => poxcp;

	public override int ptjhi => 8;

	public override int iadch(byte[] p0, int p1, int p2)
	{
		if (ocsof >= 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		return 5;
	}

	public override int lyhbb(byte[] p0, int p1, int p2)
	{
		if (ocsof >= 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		if (p0[p1] != 0 && 0 == 0)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		int num = p0[p1 + 1] * 65536 + p0[p1 + 2] * 256 + p0[p1 + 3];
		bbqnm = p0[p1 + 4];
		if (bbqnm < 4)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		ocsof = num - bbqnm - 1;
		if (ocsof < 1)
		{
			throw new SshException(tcpjq.svqut, "Received invalid packet.");
		}
		if (rlzlb.Length < ocsof)
		{
			rlzlb = new byte[(ocsof + 8191) & -4096];
		}
		return num + 4;
	}

	public override int keqao(ArraySegment<byte> p0, out byte[] p1)
	{
		if (ocsof < 0)
		{
			throw new InvalidOperationException("Invalid decoder state.");
		}
		p1 = rlzlb;
		Array.Copy(p0.Array, p0.Offset + 5, p1, 0, ocsof);
		int result = ocsof;
		ocsof = -1;
		poxcp++;
		return result;
	}

	public override int vagtd(byte[] p0, int p1, byte[] p2)
	{
		int num = ((p1 + 5 - 1) / 8 + 1) * 8;
		int num2 = num - p1 - 5;
		if (num2 < 4)
		{
			num += 8;
			num2 += 8;
		}
		int num3 = p1 + num2 + 1;
		if (num3 > 16777215)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Attempt to send a packet that is too long - {0} bytes.", num3));
		}
		jlfbq.lyicr(p2, 0, num3);
		p2[4] = (byte)num2;
		Array.Copy(p0, 0, p2, 5, p1);
		jtxhe.ubsib(p2, p1 + 5, num2);
		poxcp++;
		return num;
	}
}
