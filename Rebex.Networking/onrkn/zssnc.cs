using System;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class zssnc : crosq
{
	private readonly ICryptoTransform xjzyc;

	private readonly int rdiqr;

	private readonly int skgcp;

	public zssnc(agxpx previousState, IHashTransform mac, ICryptoTransform transform, int? compressionLevel)
		: base(previousState, mac, compressionLevel)
	{
		xjzyc = transform;
		skgcp = mac.HashSize / 8;
		rdiqr = Math.Max(xjzyc.InputBlockSize, 8);
	}

	public override int vagtd(byte[] p0, int p1, byte[] p2)
	{
		p1 = lzctp(p0, p1, p2, 0);
		int num = zcxez(p2, p1);
		xjzyc.TransformBlock(p2, 4, num, p2, 4);
		byte[] array = aanup(p2, 0, 4 + num);
		array.CopyTo(p2, 4 + num);
		return 4 + num + skgcp;
	}

	private int zcxez(byte[] p0, int p1)
	{
		int num = (p1 / rdiqr + 1) * rdiqr;
		int num2 = num - p1 - 1;
		if (num2 < 4)
		{
			num += rdiqr;
			num2 += rdiqr;
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
		xjzyc.Dispose();
		base.bwbpr();
	}
}
