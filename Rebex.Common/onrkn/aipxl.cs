using System;
using System.IO;

namespace onrkn;

internal class aipxl : gqgpl, lnabj
{
	private opjbe gyqwr;

	public override int qstkb => (int)gyqwr.Length;

	public aipxl()
		: this(new byte[0])
	{
	}

	public aipxl(byte[] data)
		: base(rmkkr.zkxoz)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		gyqwr = new opjbe();
		gyqwr.Write(data, 0, data.Length);
	}

	public aipxl(opjbe content)
		: base(rmkkr.zkxoz)
	{
		gyqwr = content;
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.zkxoz, p0, p1);
		gyqwr = new opjbe();
		base.zkxnk(p0, p1, p2);
	}

	public override void lnxah(byte[] p0, int p1, int p2)
	{
		gyqwr.Write(p0, p1, p2);
	}

	public override void somzq()
	{
	}

	public override void vlfdh(fxakl p0)
	{
		p0.pihkg(base.ccmfu, gyqwr);
	}

	public byte[] pcjxs()
	{
		return gyqwr.urpqw();
	}

	public void siums(Stream p0)
	{
		gyqwr.njguo(p0);
	}

	internal aipxl mcyhd()
	{
		return new aipxl(gyqwr);
	}

	internal void xfmlk(Action<byte[], int> p0)
	{
		gyqwr.bbmih(p0);
	}

	internal aviir<byte> rzqgs()
	{
		return gyqwr;
	}

	internal Stream ymxwm()
	{
		if (gyqwr.Length == 0)
		{
			return new MemoryStream(new byte[0], writable: false);
		}
		return gyqwr.uhxjo(0L, gyqwr.Length);
	}
}
