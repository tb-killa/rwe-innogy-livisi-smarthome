using System;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class kbyou : vumfh
{
	private ICryptoTransform qlvgi;

	private int bscdu;

	private bool pncoz;

	private int jvncf;

	public override ICryptoTransform qctmg => qlvgi;

	public kbyou(IHashTransform mac, ICryptoTransform cipher, bool cbc, TlsProtocol version)
		: base(mac, version)
	{
		qlvgi = cipher;
		bscdu = mac.HashSize / 8;
		jvncf = qlvgi.InputBlockSize;
		pncoz = cbc;
	}

	public override int bvfhg(byte[] p0, int p1)
	{
		try
		{
			qlvgi.TransformBlock(p0, 5, p1 - 5, p0, 5);
		}
		catch (Exception inner)
		{
			throw new TlsException(mjddr.wdkjl, inner);
		}
		int num = p1 - bscdu;
		bool flag = false;
		if (pncoz && 0 == 0)
		{
			byte b = p0[p1 - 1];
			num -= b + 1;
			if (num < 5)
			{
				throw new TlsException(mjddr.mmnuq);
			}
			if (base.cvjqj >= TlsProtocol.TLS10)
			{
				for (int i = num + bscdu; i < p1; i++)
				{
					if (p0[i] != b)
					{
						flag = true;
					}
				}
			}
			if (base.cvjqj >= TlsProtocol.TLS11)
			{
				num -= jvncf;
				if (num < 5)
				{
					throw new TlsException(mjddr.mmnuq);
				}
				Array.Copy(p0, 5 + jvncf, p0, 5, num - 5 + bscdu);
			}
		}
		p0[3] = (byte)(num - 5 >> 8);
		p0[4] = (byte)((num - 5) & 0xFF);
		byte[] array = cjumt(p0, 0, num);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_00ea;
		}
		goto IL_00ff;
		IL_00ea:
		if (p0[num + num2] != array[num2])
		{
			flag = true;
		}
		num2++;
		goto IL_00ff;
		IL_00ff:
		if (num2 >= bscdu)
		{
			if (flag && 0 == 0)
			{
				throw new TlsException(mjddr.mmnuq);
			}
			sldfg();
			return num;
		}
		goto IL_00ea;
	}

	public override void egphd()
	{
		qlvgi.Dispose();
		base.egphd();
	}
}
