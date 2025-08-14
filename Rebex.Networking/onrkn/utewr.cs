using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal class utewr
{
	private pxwkt.uimfo tpzox;

	private pxwkt.ldbtl yxdcc;

	private pxwkt.woide vmxan;

	private bool nrhrs;

	private bool aiyod;

	private Guid lrrou;

	private IntPtr ykqmd;

	private uint pvxxn;

	private int dbctr;

	private uint euqwx;

	private uint ggwob;

	private uint submd;

	public pxwkt.uimfo yqzzm
	{
		get
		{
			return tpzox;
		}
		set
		{
			tpzox = value;
		}
	}

	public pxwkt.ldbtl slboh
	{
		get
		{
			return yxdcc;
		}
		set
		{
			yxdcc = value;
		}
	}

	public pxwkt.woide lytcm
	{
		get
		{
			return vmxan;
		}
		set
		{
			vmxan = value;
		}
	}

	public bool brppw
	{
		get
		{
			return nrhrs;
		}
		set
		{
			nrhrs = value;
		}
	}

	public bool xdoaj
	{
		get
		{
			return aiyod;
		}
		set
		{
			aiyod = value;
		}
	}

	public Guid pdqfq
	{
		get
		{
			return lrrou;
		}
		set
		{
			lrrou = value;
		}
	}

	public IntPtr hnmgt
	{
		get
		{
			return ykqmd;
		}
		set
		{
			ykqmd = value;
		}
	}

	public uint mluap
	{
		get
		{
			return pvxxn;
		}
		set
		{
			pvxxn = value;
		}
	}

	public int jsmyc
	{
		get
		{
			return dbctr;
		}
		set
		{
			dbctr = value;
		}
	}

	public uint gagrb
	{
		get
		{
			return euqwx;
		}
		set
		{
			euqwx = value;
		}
	}

	public uint qooda
	{
		get
		{
			return ggwob;
		}
		set
		{
			ggwob = value;
		}
	}

	public uint mrtzr
	{
		get
		{
			return submd;
		}
		set
		{
			submd = value;
		}
	}

	public utewr(Guid destination, pxwkt.woide priority, pxwkt.ldbtl proxy)
	{
		pdqfq = destination;
		yqzzm = pxwkt.uimfo.kdzby;
		lytcm = priority;
		slboh = proxy;
		brppw = false;
		xdoaj = false;
		hnmgt = IntPtr.Zero;
		mluap = 0u;
		jsmyc = 0;
		mrtzr = 0u;
		gagrb = 0u;
		qooda = 0u;
	}

	public utewr(Guid destination, pxwkt.woide priority)
		: this(destination, priority, pxwkt.ldbtl.szsdt)
	{
	}

	public utewr(Guid destination)
		: this(destination, pxwkt.woide.wiqjw)
	{
	}

	public pxwkt.vyqyo lkkmg()
	{
		return new pxwkt.vyqyo
		{
			wdjzn = Marshal.SizeOf(typeof(pxwkt.vyqyo)),
			nwgia = Convert.ToInt32(xdoaj),
			apsvy = Convert.ToInt32(brppw),
			qnfzr = slboh,
			dsvwl = yqzzm,
			lojav = lytcm,
			htfes = pdqfq,
			xlkvi = hnmgt,
			ghqfl = jsmyc,
			sfrqh = mrtzr,
			eqtsb = gagrb,
			bitok = mluap,
			yiaps = qooda
		};
	}
}
