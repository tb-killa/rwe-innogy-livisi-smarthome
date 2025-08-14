namespace onrkn;

internal class ljegp : jwatd
{
	private string rtbri;

	private string uycwp;

	private bool ivrip;

	public override cwaww hwppf => cwaww.kczcd;

	public string vozff
	{
		get
		{
			return rtbri;
		}
		private set
		{
			rtbri = value;
		}
	}

	public string wajgs
	{
		get
		{
			return uycwp;
		}
		private set
		{
			uycwp = value;
		}
	}

	public bool xvxqk
	{
		get
		{
			return ivrip;
		}
		private set
		{
			ivrip = value;
		}
	}

	public ljegp(string content)
	{
		vozff = content.Trim();
		wajgs = vozff.Trim('/').Trim();
		xvxqk = vozff.Length > wajgs.Length;
		int num = wajgs.IndexOf(' ');
		if (num > 0)
		{
			wajgs = wajgs.Substring(0, num);
		}
	}

	public override string ToString()
	{
		return vozff;
	}
}
