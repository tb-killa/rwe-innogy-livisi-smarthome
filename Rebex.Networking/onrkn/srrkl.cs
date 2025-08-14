using System;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class srrkl : vumfh
{
	private ICryptoTransform nbqrv;

	private bool enayj;

	private int qmpkg;

	public override ICryptoTransform qctmg => nbqrv;

	public srrkl(IHashTransform mac, ICryptoTransform cipher, bool cbc, TlsProtocol version)
		: base(mac, version)
	{
		nbqrv = cipher;
		enayj = cbc;
		qmpkg = nbqrv.InputBlockSize;
	}

	public override int bvfhg(byte[] p0, int p1)
	{
		byte[] array = cjumt(p0, 0, p1);
		int num2;
		if (enayj && 0 == 0)
		{
			int num = (qmpkg - (p1 - 5 + array.Length + 1) % qmpkg) % qmpkg;
			num2 = p1 + array.Length + 1 + num;
			Array.Copy(array, 0, p0, p1, array.Length);
			int num3;
			switch (base.cvjqj)
			{
			case TlsProtocol.TLS11:
			case TlsProtocol.TLS12:
				Array.Copy(p0, 5, p0, 5 + qmpkg, p1 + array.Length);
				p1 += qmpkg;
				num2 += qmpkg;
				jtxhe.ubsib(p0, 5, qmpkg);
				goto case TlsProtocol.TLS10;
			case TlsProtocol.TLS10:
				num3 = 0;
				if (num3 != 0)
				{
					goto IL_00b9;
				}
				goto IL_00c8;
			case TlsProtocol.SSL30:
				{
					jtxhe.ubsib(p0, p1 + array.Length, num);
					p0[p1 + array.Length + num] = (byte)num;
					break;
				}
				IL_00c8:
				if (num3 > num)
				{
					break;
				}
				goto IL_00b9;
				IL_00b9:
				p0[p1 + array.Length + num3] = (byte)num;
				num3++;
				goto IL_00c8;
			}
			nbqrv.TransformBlock(p0, 5, num2 - 5, p0, 5);
		}
		else
		{
			num2 = p1 + array.Length;
			nbqrv.TransformBlock(p0, 5, p1 - 5, p0, 5);
			nbqrv.TransformBlock(array, 0, array.Length, p0, p1);
		}
		p0[3] = (byte)(num2 - 5 >> 8);
		p0[4] = (byte)((num2 - 5) & 0xFF);
		sldfg();
		return num2;
	}

	public override void egphd()
	{
		nbqrv.Dispose();
		base.egphd();
	}
}
