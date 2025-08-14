using System;
using System.IO;
using Rebex;

namespace onrkn;

internal class nymgm : TextReader, phvuu
{
	private readonly phvuu tvwhh;

	private readonly udlmn tdwds;

	private readonly byte[] pokdx;

	private int qzbqr;

	private int llvoy;

	private bool vysmz;

	private bool fklrl;

	public int Timeout
	{
		get
		{
			return tvwhh.Timeout;
		}
		set
		{
			tvwhh.Timeout = value;
		}
	}

	public bool swbbq => vysmz;

	internal nymgm(phvuu socket, udlmn client)
	{
		tvwhh = socket;
		tdwds = client;
		pokdx = new byte[2048];
	}

	internal phvuu qgyvx(out ArraySegment<byte> p0)
	{
		p0 = new ArraySegment<byte>(pokdx, qzbqr, llvoy - qzbqr);
		fklrl = true;
		vysmz = true;
		return tvwhh;
	}

	private int wbenx(byte[] p0, int p1, int p2)
	{
		if (vysmz && 0 == 0)
		{
			tdwds.maskh();
			return 0;
		}
		if (tdwds.pjvho > 0)
		{
			tdwds.maskh();
			if (!tvwhh.hznqz(tdwds.pjvho * 1000) || 1 == 0)
			{
				throw new ujepc("Response reading timeout.", ezmya.qrtnk);
			}
		}
		tdwds.maskh();
		int num = tvwhh.Receive(p0, p1, p2);
		if (num > 0)
		{
			tdwds.qruww(LogLevel.Verbose, "HTTP", "Received data:", p0, p1, num);
		}
		else
		{
			tdwds.iptfx(LogLevel.Verbose, "HTTP", "Socket closed by the server.");
			kvodc();
		}
		return num;
	}

	public bool ooaym(bool p0)
	{
		return xiadn((p0 ? true : false) ? (-1) : 0);
	}

	public bool xiadn(int p0)
	{
		if (p0 >= 0 && (!hznqz(p0 * 1000) || 1 == 0))
		{
			return false;
		}
		if (qzbqr >= llvoy)
		{
			qzbqr = 0;
			llvoy = wbenx(pokdx, 0, pokdx.Length);
		}
		return llvoy > qzbqr;
	}

	public override int Read()
	{
		if (!ooaym(p0: true) || 1 == 0)
		{
			return -1;
		}
		int result = pokdx[qzbqr];
		qzbqr++;
		return result;
	}

	public override int Peek()
	{
		if (!ooaym(p0: true) || 1 == 0)
		{
			return -1;
		}
		return pokdx[qzbqr];
	}

	public bool hznqz(int p0)
	{
		if (vysmz && 0 == 0)
		{
			return true;
		}
		if (qzbqr < llvoy)
		{
			return true;
		}
		tdwds.maskh();
		return tvwhh.hznqz(p0);
	}

	public int Receive(byte[] buffer, int offset, int count)
	{
		int num = 0;
		if (qzbqr < llvoy)
		{
			int num2 = Math.Min(llvoy - qzbqr, count);
			Array.Copy(pokdx, qzbqr, buffer, offset, num2);
			num += num2;
			offset += num2;
			qzbqr += num2;
			count -= num2;
		}
		while (count > 0)
		{
			int num3 = wbenx(buffer, offset, count);
			if (num3 == 0)
			{
				break;
			}
			num += num3;
			offset += num3;
			count -= num3;
		}
		return num;
	}

	public int Send(byte[] buffer, int offset, int count)
	{
		tdwds.maskh();
		return tvwhh.Send(buffer, offset, count);
	}

	public override void Close()
	{
		base.Close();
		kvodc();
	}

	public void fvtwr()
	{
		base.Close();
		kvodc();
	}

	private void kvodc()
	{
		vysmz = true;
		if (!fklrl || 1 == 0)
		{
			tvwhh.Close();
		}
	}
}
