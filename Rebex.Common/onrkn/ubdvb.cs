using System;
using System.Threading;

namespace onrkn;

internal class ubdvb : IAsyncResult
{
	public delegate object fsvyt(object obj, Enum id, object[] args);

	private bool cflgn;

	private fsvyt osson;

	private object ikton;

	private Enum cwtcu;

	private object[] hdmck;

	private object zzaus;

	private AsyncCallback dkfhz;

	private object pbsme;

	private object uwbuq;

	private Exception zinjf;

	private ManualResetEvent xszav;

	private int nreph;

	private bool lnnos;

	public int jzuxt => nreph;

	public bool annxa => cflgn;

	public Enum crens => cwtcu;

	public object hflow => zzaus;

	public object dcenx => uwbuq;

	public Exception oybzl => zinjf;

	private object rqucn => pbsme;

	private WaitHandle lkneq => xszav;

	private bool pmljq => false;

	private bool kljaq => lnnos;

	public ubdvb(bool isTask, fsvyt worker, object obj, Enum id, object tag, AsyncCallback callback, object state, object[] args)
	{
		cflgn = isTask;
		osson = worker;
		ikton = obj;
		zzaus = tag;
		cwtcu = id;
		hdmck = args;
		dkfhz = callback;
		pbsme = state;
		xszav = new ManualResetEvent(initialState: false);
	}

	public void nvhuz()
	{
		if (!bvilq.eiuho(rkajq) || 1 == 0)
		{
			throw new NotSupportedException("Unable to start asynchronous operation.");
		}
	}

	private void rkajq(object p0)
	{
		nreph = dahxy.qmuio;
		try
		{
			uwbuq = osson(ikton, cwtcu, hdmck);
		}
		catch (Exception ex)
		{
			zinjf = ex;
		}
		lnnos = true;
		xszav.Set();
		if (dkfhz != null && 0 == 0)
		{
			dkfhz(this);
		}
	}

	public static ubdvb ovklt(IAsyncResult p0, object p1, string p2, Enum[] p3)
	{
		if (!(p0 is ubdvb ubdvb2) || false || ubdvb2.ikton != p1 || Array.IndexOf(p3, ubdvb2.cwtcu) < 0)
		{
			if (p2 == null || 1 == 0)
			{
				p2 = "Async";
			}
			throw new ArgumentException(brgjd.edcru("The IAsyncResult object supplied to End{0} was not returned from the corresponding Begin{0} method on this class.", p2), "asyncResult");
		}
		if (!ubdvb2.xszav.WaitOne() || 1 == 0)
		{
			throw new NotSupportedException("Unable to finish asynchronous operation.");
		}
		return ubdvb2;
	}

	private void infoj(IAsyncResult p0)
	{
		dkfhz(this);
	}
}
