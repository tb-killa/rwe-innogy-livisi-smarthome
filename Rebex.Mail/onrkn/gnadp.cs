namespace onrkn;

internal class gnadp : ssdlg
{
	internal enum jgvrf
	{
		ygqoh,
		adbuo,
		tmrex,
		inivp,
		dujps,
		ngqub,
		dwbpf,
		kbfsy,
		cbccy
	}

	private bool yiscr;

	private bool rndom;

	private bool zmphf;

	private bool hqvuf;

	private bool vxegb;

	private bool nlenp;

	private bool ctqjh;

	private bool pmtdv;

	private bool orjxg;

	private bool slzvi;

	public bool lljxt
	{
		get
		{
			return yiscr;
		}
		set
		{
			yiscr = value;
		}
	}

	public bool jdcus
	{
		get
		{
			return zmphf;
		}
		set
		{
			zmphf = value;
		}
	}

	public bool hchkn
	{
		get
		{
			return rndom;
		}
		set
		{
			rndom = value;
		}
	}

	public bool xsiqy
	{
		get
		{
			return hqvuf;
		}
		set
		{
			hqvuf = value;
		}
	}

	public bool ywrnu
	{
		get
		{
			return vxegb;
		}
		set
		{
			vxegb = value;
		}
	}

	public bool iekwa
	{
		get
		{
			return nlenp;
		}
		set
		{
			nlenp = value;
		}
	}

	public bool uqwbx
	{
		get
		{
			return ctqjh;
		}
		set
		{
			ctqjh = value;
		}
	}

	public bool qsrru
	{
		get
		{
			return pmtdv;
		}
		set
		{
			pmtdv = value;
		}
	}

	public bool geabl
	{
		get
		{
			return orjxg;
		}
		set
		{
			orjxg = value;
		}
	}

	public bool lfwjp
	{
		get
		{
			return slzvi;
		}
		set
		{
			slzvi = value;
		}
	}

	internal gnadp(fpnng model)
		: base(model)
	{
	}

	public override bool Equals(object o)
	{
		if (!(o is gnadp gnadp2) || 1 == 0)
		{
			return false;
		}
		if (lljxt == gnadp2.lljxt && jdcus == gnadp2.jdcus && hchkn == gnadp2.hchkn && xsiqy == gnadp2.xsiqy && ywrnu == gnadp2.ywrnu && iekwa == gnadp2.iekwa && uqwbx == gnadp2.uqwbx && qsrru == gnadp2.qsrru && geabl == gnadp2.geabl)
		{
			return lfwjp == gnadp2.lfwjp;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return lljxt.GetHashCode() ^ jdcus.GetHashCode() ^ hchkn.GetHashCode() ^ xsiqy.GetHashCode() ^ ywrnu.GetHashCode() ^ iekwa.GetHashCode() ^ uqwbx.GetHashCode() ^ qsrru.GetHashCode() ^ geabl.GetHashCode() ^ lfwjp.GetHashCode();
	}
}
