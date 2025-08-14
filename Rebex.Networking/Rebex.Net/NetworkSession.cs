using System;
using System.Threading;
using onrkn;

namespace Rebex.Net;

public abstract class NetworkSession : ILogWriterProvider, gnyvx
{
	private const string vsuec = "LogSensitiveData";

	private string oygmp;

	private string krqmt;

	private int atwxf;

	private qacxy jreex;

	private static int pkxfs;

	private readonly int eehgn;

	private readonly awngk ufbwe;

	private static ILogWriter btasa;

	internal awngk twmrq => ufbwe;

	public static ILogWriter DefaultLogWriter
	{
		get
		{
			return btasa;
		}
		set
		{
			btasa = value;
		}
	}

	public abstract bool IsConnected { get; }

	public abstract bool IsAuthenticated { get; }

	public virtual string UserName
	{
		get
		{
			return oygmp;
		}
		protected set
		{
			oygmp = value;
		}
	}

	public virtual string ServerName
	{
		get
		{
			return krqmt;
		}
		protected set
		{
			krqmt = value;
		}
	}

	public virtual int ServerPort
	{
		get
		{
			return atwxf;
		}
		protected set
		{
			atwxf = value;
		}
	}

	public virtual ILogWriter LogWriter
	{
		get
		{
			return ufbwe.xxboi;
		}
		set
		{
			ufbwe.xxboi = value;
		}
	}

	protected internal int InstanceId => eehgn;

	internal void nhmfm(rsljk p0)
	{
		jreex = ongpx.bjvdq(p0);
	}

	internal void stpsr(rsljk p0)
	{
		LogWriterBase.unxux(LogWriter, p0, jreex, GetType(), InstanceId);
	}

	protected NetworkSession()
	{
		eehgn = Interlocked.Increment(ref pkxfs);
		ufbwe = new awngk(GetType(), eehgn, DefaultLogWriter);
	}

	internal void rmwyv(LogLevel p0, string p1, string p2)
	{
		twmrq.rfpvf(p0, p1, p2);
	}

	internal void olfku(LogLevel p0, string p1, string p2, params object[] p3)
	{
		twmrq.byfnx(p0, p1, p2, p3);
	}

	internal void lbznr(LogLevel p0, string p1, string p2, byte[] p3, int p4, int p5)
	{
		twmrq.iyauk(p0, p1, p2, p3, p4, p5);
	}

	private object mpgiw(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0 && text == "LogSensitiveData" && 0 == 0)
		{
			return twmrq.ngqry;
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	object gnyvx.jfzti(string p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in mpgiw
		return this.mpgiw(p0);
	}

	private void xyczi(string p0, object p1)
	{
		string text;
		if ((text = p0) != null && 0 == 0 && text == "LogSensitiveData" && 0 == 0)
		{
			twmrq.ngqry = (bool)p1;
			return;
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	void gnyvx.vhvwu(string p0, object p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xyczi
		this.xyczi(p0, p1);
	}
}
