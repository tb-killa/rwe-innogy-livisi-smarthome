using System;

namespace onrkn;

internal abstract class jcgcz : gqgpl
{
	private byte[] fbmgu;

	private int ynsmv;

	public byte[] rtrhq => fbmgu;

	public override int qstkb => ynsmv;

	protected jcgcz()
		: base((rmkkr)0)
	{
	}

	protected jcgcz(rmkkr type, byte[] data)
		: base(type)
	{
		fbmgu = data;
		ynsmv = data.Length;
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		fbmgu = null;
		ynsmv = 0;
		base.zkxnk(p0, p1, p2);
	}

	public override void lnxah(byte[] p0, int p1, int p2)
	{
		if (fbmgu == null || 1 == 0)
		{
			fbmgu = new byte[p2];
		}
		else
		{
			int num = ynsmv + p2;
			if (fbmgu.Length < num)
			{
				num = Math.Max(fbmgu.Length * 2, num);
				if (num > 4095)
				{
					num = (num | 0xFFF) + 1;
				}
				byte[] array = new byte[num];
				fbmgu.CopyTo(array, 0);
				fbmgu = array;
			}
		}
		Array.Copy(p0, p1, fbmgu, ynsmv, p2);
		ynsmv += p2;
	}

	public override void somzq()
	{
		if (fbmgu == null || 1 == 0)
		{
			fbmgu = new byte[0];
		}
		else if (fbmgu.Length != ynsmv)
		{
			byte[] destinationArray = new byte[ynsmv];
			Array.Copy(fbmgu, 0, destinationArray, 0, ynsmv);
			fbmgu = destinationArray;
		}
	}

	public override void vlfdh(fxakl p0)
	{
		p0.loggt(base.ccmfu, fbmgu);
	}

	public byte[] ionjf()
	{
		return fxakl.kncuz(this);
	}
}
