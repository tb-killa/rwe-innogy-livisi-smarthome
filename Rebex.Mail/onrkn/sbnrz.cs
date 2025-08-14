using System.Collections.Generic;
using System.Text;

namespace onrkn;

internal class sbnrz
{
	private Decoder utwhi;

	private int rcrqh = -1;

	public readonly sbnrz hcbnv;

	public readonly List<mmgqv> khhoe = new List<mmgqv>();

	public bool yxvxm
	{
		get
		{
			if (hcbnv != null && 0 == 0)
			{
				return hcbnv.hcbnv == null;
			}
			return true;
		}
	}

	public Decoder krbty
	{
		get
		{
			if (utwhi == null || 1 == 0)
			{
				utwhi = hcbnv.krbty;
			}
			return utwhi;
		}
	}

	public int rakkx
	{
		get
		{
			if (rcrqh == -1)
			{
				rcrqh = hcbnv.rakkx;
			}
			return rcrqh;
		}
	}

	public void fpzoh(Decoder p0)
	{
		utwhi = p0;
	}

	public void efcpq(int p0)
	{
		rcrqh = p0;
	}

	public sbnrz(sbnrz parent)
	{
		hcbnv = parent;
	}

	public override string ToString()
	{
		if (khhoe.Count != 0 && 0 == 0)
		{
			return khhoe[0].ToString();
		}
		return string.Empty;
	}
}
