namespace onrkn;

internal class oxbyz : ssdlg
{
	private string xpimt;

	private int yrjve;

	public string xtrgh
	{
		get
		{
			return xpimt;
		}
		set
		{
			xpimt = value;
		}
	}

	public int mzdmu
	{
		get
		{
			return yrjve;
		}
		set
		{
			yrjve = value;
		}
	}

	internal oxbyz(fpnng model)
		: base(model)
	{
		xpimt = null;
		yrjve = 0;
	}

	internal oxbyz(string color, int colorId, fpnng model)
		: base(model)
	{
		xpimt = color;
		yrjve = colorId;
	}

	public override bool Equals(object o)
	{
		bool result = false;
		if (o is oxbyz && 0 == 0 && o != null && 0 == 0)
		{
			oxbyz oxbyz2 = (oxbyz)o;
			if (xtrgh == oxbyz2.xtrgh && 0 == 0)
			{
				result = true;
			}
		}
		return result;
	}

	public override int GetHashCode()
	{
		return xtrgh.GetHashCode();
	}
}
