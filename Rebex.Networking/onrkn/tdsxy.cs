using System;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class tdsxy : crosq
{
	private readonly ICryptoTransform vjccq;

	private readonly int nivfs;

	private readonly int itxof;

	private readonly byte[] jxhmo;

	public tdsxy(agxpx previousState, IHashTransform mac, ICryptoTransform transform, CipherMode mode, int? compressionLevel)
		: base(previousState, mac, compressionLevel)
	{
		if (mode == CipherMode.CBC)
		{
			jxhmo = new byte[5];
			jxhmo[0] = 2;
		}
		vjccq = transform;
		itxof = mac.HashSize / 8;
		nivfs = Math.Max(vjccq.InputBlockSize, 8);
	}

	public override int vagtd(byte[] p0, int p1, byte[] p2)
	{
		int p3 = 0;
		if (jxhmo != null && 0 == 0)
		{
			p3 = ofalh(jxhmo, 5, p2, p3);
		}
		return ofalh(p0, p1, p2, p3);
	}

	private int ofalh(byte[] p0, int p1, byte[] p2, int p3)
	{
		p1 = lzctp(p0, p1, p2, p3);
		int num = fzizm(p2, p3, p1);
		byte[] array = aanup(p2, p3, num);
		array.CopyTo(p2, p3 + num);
		vjccq.TransformBlock(p2, p3, num, p2, p3);
		return p3 + num + itxof;
	}

	private int fzizm(byte[] p0, int p1, int p2)
	{
		int num = ((p2 + 5 - 1) / nivfs + 1) * nivfs;
		int num2 = num - p2 - 5;
		if (num2 < 4)
		{
			num += nivfs;
			num2 += nivfs;
		}
		int num3 = p2 + num2 + 1;
		if (num3 > 16777215)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Attempt to send a packet that is too long - {0} bytes.", num3));
		}
		jlfbq.lyicr(p0, p1, num3);
		p0[p1 + 4] = (byte)num2;
		jtxhe.ubsib(p0, p1 + p2 + 5, num2);
		return num;
	}

	public override void bwbpr()
	{
		vjccq.Dispose();
		base.bwbpr();
	}
}
