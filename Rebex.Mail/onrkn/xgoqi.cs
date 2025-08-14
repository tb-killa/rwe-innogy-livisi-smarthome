using System.Text;

namespace onrkn;

internal class xgoqi : whhig
{
	private int hrtlg;

	private double aagyg = -1.0;

	private zbmst pdgxy;

	private bool fdiuu;

	private bool zdftv;

	private bool nqvwc;

	private bool iicmy;

	private bool afktk;

	private int ujrtc;

	private int wtphi;

	private double bdqdk;

	private bool rgbda;

	private string ndnct;

	internal cmjgd tntkx => wvgrf.xebmg;

	internal zbmst wvgrf
	{
		get
		{
			return pdgxy;
		}
		private set
		{
			pdgxy = value;
		}
	}

	internal bool vhszf
	{
		get
		{
			return fdiuu;
		}
		set
		{
			fdiuu = value;
		}
	}

	internal bool hmlhf
	{
		get
		{
			return zdftv;
		}
		set
		{
			zdftv = value;
		}
	}

	internal bool aouzi
	{
		get
		{
			return nqvwc;
		}
		set
		{
			nqvwc = value;
		}
	}

	internal bool zdrvo
	{
		get
		{
			return iicmy;
		}
		set
		{
			iicmy = value;
		}
	}

	internal bool zfnqj
	{
		get
		{
			return afktk;
		}
		set
		{
			afktk = value;
		}
	}

	internal int wudph
	{
		get
		{
			return hrtlg;
		}
		set
		{
			zfnqj = true;
			hrtlg = value;
		}
	}

	public int kgttg
	{
		get
		{
			return ujrtc;
		}
		set
		{
			ujrtc = value;
		}
	}

	public int aelyo
	{
		get
		{
			return wtphi;
		}
		set
		{
			wtphi = value;
		}
	}

	public double qonjr
	{
		get
		{
			return aagyg;
		}
		set
		{
			if (value >= 0.0)
			{
				aagyg = value;
			}
		}
	}

	public double tgyfn
	{
		get
		{
			return bdqdk;
		}
		set
		{
			bdqdk = value;
		}
	}

	public bool yethr
	{
		get
		{
			return rgbda;
		}
		set
		{
			rgbda = value;
		}
	}

	public string sgqmr
	{
		get
		{
			return ndnct;
		}
		set
		{
			ndnct = value;
		}
	}

	public xgoqi(fpnng model, zbmst parent)
		: base(model)
	{
		wvgrf = parent;
	}

	internal override string dsdoz()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("border-style:solid; padding: 1pt; border-color:black; border-width:1pt;");
		if (zfnqj && 0 == 0)
		{
			string value = hokmy.omlea(wudph);
			stringBuilder.Append(value);
		}
		if (((yethr ? true : false) || tgyfn == 0.0) && qonjr >= 0.0)
		{
			stringBuilder.dlvlk("width:{0}pt;", qonjr);
		}
		else
		{
			stringBuilder.mwigd("width:{0}{1}", tgyfn, sgqmr);
		}
		return stringBuilder.ToString();
	}
}
