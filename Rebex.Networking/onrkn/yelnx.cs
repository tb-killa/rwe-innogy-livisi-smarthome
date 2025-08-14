using System;

namespace onrkn;

internal sealed class yelnx
{
	public const string kyigq = "Could not send data. (Send) channel is invalid/disposed.";

	public const string cxwke = "Could not receive data. (Receive) channel is invalid/disposed.";

	public const string rokde = "The channel is not started. Call method Start first.";

	private readonly ridny iybtu;

	private readonly ridny yynkw;

	private readonly ridny cmvbn;

	private readonly jicrh<object> jvqyz;

	private readonly ridny gvgjr;

	private readonly ridny tqamg;

	private readonly jicrh<int> debwg;

	private bool izauv;

	private bool qofij;

	private bool rkobr;

	private bool wsxxd;

	private ridny etwkr;

	private apajk<Exception> wxrav;

	public bool rtcbh
	{
		get
		{
			return qofij;
		}
		set
		{
			qofij = value;
		}
	}

	public bool svsgt
	{
		get
		{
			return rkobr;
		}
		set
		{
			rkobr = value;
		}
	}

	public bool rnqba
	{
		get
		{
			return wsxxd;
		}
		private set
		{
			wsxxd = value;
		}
	}

	public bool tdlwo
	{
		get
		{
			return izauv;
		}
		set
		{
			_ = izauv;
			if ((!izauv || 1 == 0) && value && 0 == 0 && svsgt && 0 == 0 && bhgqf.nbnot && 0 == 0)
			{
				wzqky.qxocb();
			}
			izauv = value;
		}
	}

	public exkzi ozgnw => jvqyz.wmfea;

	public bool wxutg => yynkw.scnlq;

	public exkzi onwxy => debwg.wmfea;

	public jicrh<int> unruq => debwg;

	public ridny wzqky
	{
		get
		{
			return etwkr;
		}
		private set
		{
			etwkr = value;
		}
	}

	public apajk<Exception> bhgqf
	{
		get
		{
			return wxrav;
		}
		set
		{
			wxrav = value;
		}
	}

	public yelnx()
	{
		iybtu = new ridny();
		yynkw = new ridny();
		cmvbn = new ridny();
		gvgjr = new ridny();
		tqamg = new ridny();
		wzqky = new ridny();
		jvqyz = new jicrh<object>();
		debwg = new jicrh<int>();
		rtcbh = (svsgt = false);
		bhgqf = apajk<Exception>.uceou;
		tdlwo = false;
	}

	public void ngryw(Exception p0 = null)
	{
		if (p0 != null && 0 == 0)
		{
			if (bhgqf.nbnot && 0 == 0)
			{
				bhgqf = p0;
			}
			rnqba = true;
		}
		svsgt = false;
	}

	public void thkcu()
	{
		rtcbh = false;
	}

	public bool ofoci()
	{
		bool flag = yynkw.qxocb();
		if (flag && 0 == 0)
		{
			bool flag2 = (rtcbh = true);
			svsgt = flag2;
		}
		return flag;
	}

	public bool japit()
	{
		bool flag = cmvbn.qxocb();
		if (flag && 0 == 0)
		{
			bool flag2 = (rtcbh = false);
			svsgt = flag2;
		}
		return flag;
	}

	public bool vonhy()
	{
		return gvgjr.qxocb();
	}

	public bool geywj()
	{
		return tqamg.qxocb();
	}

	public void ltanj()
	{
		ngryw();
		thkcu();
	}

	public bool cqruj()
	{
		return iybtu.qxocb();
	}

	public void sfboo<TObject>()
	{
		if (jvqyz.wmfea.IsCompleted && 0 == 0)
		{
			throw new ObjectDisposedException(typeof(TObject).FullName);
		}
	}

	public void vvjim<TObject>(string p0)
	{
		if (jvqyz.wmfea.IsCompleted && 0 == 0)
		{
			throw new ObjectDisposedException(typeof(TObject).FullName, p0);
		}
	}

	public void qtzjv(Func<Exception, Exception> p0)
	{
		if (!svsgt || 1 == 0)
		{
			throw p0(new InvalidOperationException("Could not send data. (Send) channel is invalid/disposed."));
		}
	}

	public void uggrs(Func<Exception, Exception> p0)
	{
		if (!rtcbh || 1 == 0)
		{
			throw p0(new InvalidOperationException("Could not receive data. (Receive) channel is invalid/disposed."));
		}
	}

	public void dtmvf(Func<Exception, Exception> p0)
	{
		if (!yynkw.scnlq || 1 == 0)
		{
			throw p0(new InvalidOperationException("The channel is not started. Call method Start first."));
		}
	}

	public void azkaz()
	{
		jvqyz.spaxx(null);
	}

	public void vhyzo()
	{
		ngryw();
		debwg.spaxx(0);
	}
}
