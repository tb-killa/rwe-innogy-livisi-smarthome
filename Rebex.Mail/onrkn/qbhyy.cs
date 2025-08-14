using System;

namespace onrkn;

internal class qbhyy : ssdlg
{
	private int bnexa;

	internal int dpodp
	{
		get
		{
			return bnexa;
		}
		set
		{
			bnexa = value;
		}
	}

	internal qbhyy(fpnng model)
		: base(model)
	{
		bnexa = -1;
	}

	internal qbhyy(int fontSize, fpnng model)
		: base(model)
	{
		if (fontSize <= 0)
		{
			bnexa = -1;
		}
		else
		{
			bnexa = fontSize;
		}
	}

	internal string ldvnz()
	{
		return twxvb(p0: false);
	}

	internal string twxvb(bool p0)
	{
		double num = ((dpodp != -1) ? ((double)dpodp) : 12.0);
		if (dpodp == -1 && (!p0 || 1 == 0))
		{
			return string.Empty;
		}
		if (p0 && 0 == 0)
		{
			num = Math.Round(num *= 0.6, 1);
		}
		return brgjd.edcru("font-size:{0}pt;", num);
	}

	public override bool Equals(object o)
	{
		bool result = false;
		if (o is qbhyy && 0 == 0 && o != null && 0 == 0)
		{
			qbhyy qbhyy2 = (qbhyy)o;
			if (dpodp == qbhyy2.dpodp)
			{
				result = true;
			}
		}
		return result;
	}

	public override int GetHashCode()
	{
		return dpodp.GetHashCode();
	}
}
