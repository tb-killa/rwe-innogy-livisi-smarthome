using System.Collections.Generic;

namespace onrkn;

internal class brnhb
{
	internal enum aimxq
	{
		ptazv,
		jzpmm,
		dyunt,
		xrpup,
		gayhi,
		iktvx,
		tmasq,
		gwvzv
	}

	internal enum sqtkl
	{
		nbfux,
		trhgg,
		lsavc,
		wuozm,
		rtyql
	}

	private aimxq mbzcq;

	private string gjrqt;

	private string oezvf;

	private int qlcwa;

	private int lyevi;

	private int kzkdo;

	private int ycfeb;

	private static readonly Dictionary<aimxq, string> jupgz;

	public aimxq kunwh
	{
		get
		{
			return mbzcq;
		}
		set
		{
			mbzcq = value;
		}
	}

	public string hmtqe
	{
		get
		{
			return gjrqt;
		}
		set
		{
			if (value == "Tms Rmn" && 0 == 0)
			{
				gjrqt = "Times New Roman";
			}
			else
			{
				gjrqt = value;
			}
		}
	}

	public string ekgcj
	{
		get
		{
			return oezvf;
		}
		set
		{
			oezvf = value;
		}
	}

	public int aiofa
	{
		get
		{
			return qlcwa;
		}
		set
		{
			if (value >= 0 && value <= 2)
			{
				qlcwa = value;
			}
			else
			{
				qlcwa = 0;
			}
		}
	}

	public int coxhl
	{
		get
		{
			return lyevi;
		}
		set
		{
			if (value >= 0 && value <= int.MaxValue)
			{
				lyevi = value;
			}
			else
			{
				lyevi = 0;
			}
		}
	}

	public int uhkfj
	{
		get
		{
			return kzkdo;
		}
		set
		{
			if (value >= 0 && value <= int.MaxValue)
			{
				kzkdo = value;
			}
			else
			{
				kzkdo = 0;
			}
		}
	}

	public int thzuv
	{
		get
		{
			return ycfeb;
		}
		set
		{
			ycfeb = value;
		}
	}

	static brnhb()
	{
		jupgz = new Dictionary<aimxq, string>();
		jupgz.Add(aimxq.ptazv, null);
		jupgz.Add(aimxq.jzpmm, "serif");
		jupgz.Add(aimxq.dyunt, "sans-serif");
		jupgz.Add(aimxq.xrpup, "monospace");
		jupgz.Add(aimxq.gayhi, "cursive");
		jupgz.Add(aimxq.iktvx, null);
		jupgz.Add(aimxq.tmasq, null);
		jupgz.Add(aimxq.gwvzv, null);
	}

	public string rwejs()
	{
		jupgz.TryGetValue(kunwh, out var value);
		return value;
	}

	private string xdqzb()
	{
		if (!string.IsNullOrEmpty(hmtqe) || 1 == 0)
		{
			return hmtqe;
		}
		if (!string.IsNullOrEmpty(ekgcj) || 1 == 0)
		{
			return ekgcj;
		}
		return string.Empty;
	}

	internal string xtprc()
	{
		string text = rwejs();
		string text2 = xdqzb();
		if ((text == null || 1 == 0) && (hmtqe == null || 1 == 0))
		{
			return "";
		}
		if (hmtqe != null && 0 == 0 && text != null && 0 == 0)
		{
			return brgjd.edcru("font-family:{0},{1};", text2, text);
		}
		if (hmtqe != null && 0 == 0)
		{
			return brgjd.edcru("font-family:{0};", text2);
		}
		return brgjd.edcru("font-family:{0};", text);
	}
}
