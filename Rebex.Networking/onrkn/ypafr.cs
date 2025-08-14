using System;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class ypafr : ofuit
{
	private byte[] poeqs;

	private byte[] rmtyi;

	public override int nimwj => rmtyi.Length + 4;

	public int xpivb => (poeqs[0] << 8) + poeqs[1];

	public byte[] mujic => poeqs;

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		rmtyi.CopyTo(p0, 4);
	}

	public ypafr(byte[] buffer, int offset, int length)
		: base(nsvut.zmyoj)
	{
		rmtyi = new byte[length - 4];
		Array.Copy(buffer, 4, rmtyi, 0, length - 4);
	}

	private void xcsiv(byte[] p0, int p1)
	{
		switch (p1)
		{
		case 0:
			rmtyi = p0;
			break;
		case 1:
			if (p0.Length > 255)
			{
				throw new TlsException(mjddr.qssln, "Unexpected input.");
			}
			rmtyi = new byte[p0.Length + 1];
			rmtyi[0] = (byte)p0.Length;
			p0.CopyTo(rmtyi, 1);
			break;
		case 2:
			if (p0.Length > 65535)
			{
				throw new TlsException(mjddr.qssln, "Unexpected input.");
			}
			rmtyi = new byte[p0.Length + 2];
			rmtyi[0] = (byte)(p0.Length >> 8);
			rmtyi[1] = (byte)(p0.Length & 0xFF);
			p0.CopyTo(rmtyi, 2);
			break;
		default:
			throw new ArgumentException("Unsupported header length.", "headerLength");
		}
	}

	private void vvioz(byte[] p0, bool p1)
	{
		xcsiv(p0, (p1 ? true : false) ? 1 : 2);
	}

	private void oeceb(int p0, byte[] p1)
	{
		xcsiv(p1, (p0 != 768) ? 2 : 0);
	}

	public void ueuef(int p0, AsymmetricKeyAlgorithm p1)
	{
		byte[] p2 = p1.Encrypt(poeqs);
		oeceb(p0, p2);
	}

	private byte[] yrpar(int p0)
	{
		byte[] array;
		switch (p0)
		{
		case 0:
			array = rmtyi;
			break;
		case 1:
		{
			int num = rmtyi.Length - 1;
			if (num <= 0 || rmtyi[0] != num)
			{
				throw new TlsException(mjddr.gkkle, "Invalid {0} message.");
			}
			array = new byte[num];
			Array.Copy(rmtyi, 1, array, 0, num);
			break;
		}
		case 2:
		{
			int num = rmtyi.Length - 2;
			if (num <= 0 || rmtyi[0] * 256 + rmtyi[1] != num)
			{
				throw new TlsException(mjddr.gkkle, "Invalid {0} message.");
			}
			array = new byte[num];
			Array.Copy(rmtyi, 2, array, 0, num);
			break;
		}
		default:
			throw new ArgumentException("Unsupported header length.", "headerLength");
		}
		return array;
	}

	public byte[] ubcyg(bool p0)
	{
		return yrpar((p0 ? true : false) ? 1 : 2);
	}

	private byte[] ljxgl(int p0)
	{
		return yrpar((p0 != 768) ? 2 : 0);
	}

	public void isbyb(int p0, AsymmetricKeyAlgorithm p1)
	{
		byte[] rgb = ljxgl(p0);
		poeqs = p1.Decrypt(rgb);
	}

	public void uecax(int p0, Certificate p1)
	{
		byte[] rgb = ljxgl(p0);
		poeqs = p1.Decrypt(rgb, silent: true);
	}

	public ypafr(int protocolVersion)
		: base(nsvut.zmyoj)
	{
		poeqs = new byte[48];
		jtxhe.ubsib(poeqs, 2, 46);
		poeqs[0] = (byte)((protocolVersion >> 8) & 0xFF);
		poeqs[1] = (byte)(protocolVersion & 0xFF);
	}

	public ypafr(byte[] Yc, bool ecc)
		: base(nsvut.zmyoj)
	{
		vvioz(Yc, ecc);
	}
}
