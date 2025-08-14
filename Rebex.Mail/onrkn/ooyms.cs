using System.Text;

namespace onrkn;

internal class ooyms
{
	private string upsrl;

	private int ieaqw;

	private int hnvsd;

	private int sehzq;

	private int zorjf;

	private int detvr;

	public string fkyus
	{
		get
		{
			return upsrl;
		}
		set
		{
			upsrl = value;
		}
	}

	public int lakkm
	{
		get
		{
			return ieaqw;
		}
		set
		{
			ieaqw = rbqnv(value);
		}
	}

	public int tbeyc
	{
		get
		{
			return hnvsd;
		}
		set
		{
			hnvsd = rbqnv(value);
		}
	}

	public int ualxl
	{
		get
		{
			return sehzq;
		}
		set
		{
			sehzq = rbqnv(value);
		}
	}

	public int aylmj
	{
		get
		{
			return zorjf;
		}
		set
		{
			zorjf = rbqnv(value);
		}
	}

	public int laivf
	{
		get
		{
			return detvr;
		}
		set
		{
			detvr = rbqnv(value);
		}
	}

	public ooyms(string name, int red, int green, int blue, int tint, int shade)
	{
		upsrl = name;
		ieaqw = red;
		hnvsd = green;
		sehzq = blue;
		zorjf = tint;
		detvr = shade;
	}

	private string bnquf()
	{
		return lakkm.ToString("X2") + tbeyc.ToString("X2") + ualxl.ToString("X2");
	}

	public string jqwvy()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("." + fkyus + "\t{color: #" + bnquf() + ";}");
		return stringBuilder.ToString();
	}

	internal string ykxfe()
	{
		return bnquf() + ";";
	}

	private int rbqnv(int p0)
	{
		if (p0 < 0)
		{
			return 0;
		}
		if (p0 > 255)
		{
			return 255;
		}
		return p0;
	}
}
