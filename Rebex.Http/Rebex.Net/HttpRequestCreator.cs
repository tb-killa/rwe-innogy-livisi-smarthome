using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using onrkn;

namespace Rebex.Net;

public class HttpRequestCreator : IWebRequestCreate, rlpyd, gnyvx
{
	private const string tvexz = "LogSensitiveData";

	private static int zjccg;

	private readonly int rskis;

	private readonly awngk cfuzf;

	private readonly gurbh rcuso;

	private readonly awxwg mctbc;

	private readonly rtmzu liiie;

	private EventHandler<SslCertificateValidationEventArgs> biwhs;

	private HttpSettings fywiu;

	private ISocketFactory gjcvx;

	internal int yffyb => rskis;

	internal awngk xrpuh => cfuzf;

	internal rtmzu acxvn => liiie;

	public HttpSettings Settings
	{
		get
		{
			return fywiu;
		}
		private set
		{
			fywiu = value;
		}
	}

	public ILogWriter LogWriter
	{
		get
		{
			return cfuzf.xxboi;
		}
		set
		{
			cfuzf.xxboi = value;
		}
	}

	internal ISocketFactory arqgx
	{
		get
		{
			return gjcvx;
		}
		private set
		{
			gjcvx = value;
		}
	}

	public Proxy Proxy
	{
		get
		{
			return arqgx as Proxy;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value", "Value cannot be null.");
			}
			arqgx = value;
		}
	}

	private bool hjvbc => biwhs != null;

	private SslSettings hkcau => Settings;

	public event EventHandler<SslCertificateValidationEventArgs> ValidatingCertificate
	{
		add
		{
			EventHandler<SslCertificateValidationEventArgs> eventHandler = biwhs;
			EventHandler<SslCertificateValidationEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SslCertificateValidationEventArgs> value2 = (EventHandler<SslCertificateValidationEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref biwhs, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SslCertificateValidationEventArgs> eventHandler = biwhs;
			EventHandler<SslCertificateValidationEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SslCertificateValidationEventArgs> value2 = (EventHandler<SslCertificateValidationEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref biwhs, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public void SetSocketFactory(ISocketFactory factory)
	{
		object obj = factory;
		if (obj == null || 1 == 0)
		{
			obj = new Proxy();
		}
		arqgx = (ISocketFactory)obj;
	}

	public HttpRequestCreator()
	{
		dahxy.kdslf();
		try
		{
			ongpx.bjvdq(ddlhf.ikljh());
		}
		catch (fwwdw fwwdw)
		{
			throw new WebException(fwwdw.Message, fwwdw, WebExceptionStatus.RequestCanceled, null);
		}
		rskis = Interlocked.Increment(ref zjccg);
		cfuzf = new awngk(GetType(), rskis, NetworkSession.DefaultLogWriter);
		mctbc = new awxwg(this);
		rcuso = new gurbh();
		liiie = new rtmzu(this);
		Settings = new HttpSettings();
		arqgx = new Proxy();
	}

	public void Register()
	{
		WebRequest.RegisterPrefix("http://", kdhjp.euiqt);
		WebRequest.RegisterPrefix("https://", kdhjp.euiqt);
		kdhjp.euiqt.bgkai(this);
	}

	private WebRequest ujgif(Uri p0)
	{
		return Create(p0);
	}

	WebRequest IWebRequestCreate.Create(Uri p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ujgif
		return this.ujgif(p0);
	}

	public HttpRequest Create(Uri uri)
	{
		return new HttpRequest(uri, this);
	}

	public HttpRequest Create(string uriString)
	{
		return Create(new Uri(uriString));
	}

	internal void zauho()
	{
		mctbc.yybwt();
		liiie.bpzkr();
	}

	private void osqbo(SslCertificateValidationEventArgs p0)
	{
		EventHandler<SslCertificateValidationEventArgs> eventHandler = biwhs;
		if (eventHandler != null && 0 == 0)
		{
			eventHandler(this, p0);
		}
	}

	private void vminb(SslCertificateValidationEventArgs p0)
	{
		if (((rlpyd)this).clxxv && 0 == 0)
		{
			osqbo(p0);
		}
	}

	void rlpyd.pyugx(SslCertificateValidationEventArgs p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vminb
		this.vminb(p0);
	}

	internal void nigxk(LogLevel p0, string p1, string p2)
	{
		cfuzf.rfpvf(p0, p1, p2);
	}

	internal void tdcir(LogLevel p0, string p1, string p2, params object[] p3)
	{
		cfuzf.byfnx(p0, p1, p2, p3);
	}

	internal bool nahka(ixwxt p0, ISocketFactory p1, string p2)
	{
		if (!Settings.HttpSessionCacheEnabled || 1 == 0)
		{
			return false;
		}
		return mctbc.wpuyo(p1, p2, p0);
	}

	internal bool mxllt(string p0, out ixwxt p1, out ISocketFactory p2)
	{
		if (!Settings.HttpSessionCacheEnabled || 1 == 0)
		{
			p1 = null;
			p2 = null;
			return false;
		}
		p2 = arqgx;
		p1 = mctbc.gsuol(p2, p0);
		return p1 != null;
	}

	internal void gmuvh(TlsSession p0, string p1)
	{
		if (Settings.SslSessionCacheEnabled ? true : false)
		{
			rcuso.wzwsv(p1, p0);
		}
	}

	internal TlsSession kfgrm(string p0)
	{
		if (!Settings.SslSessionCacheEnabled || 1 == 0)
		{
			return null;
		}
		return rcuso.twxbv(p0);
	}

	internal IEnumerable<KeyValuePair<string, TlsSession>> hzbui()
	{
		return rcuso;
	}

	private object meaur(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0 && text == "LogSensitiveData" && 0 == 0)
		{
			return xrpuh.ngqry;
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	object gnyvx.jfzti(string p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in meaur
		return this.meaur(p0);
	}

	private void xltfk(string p0, object p1)
	{
		string text;
		if ((text = p0) != null && 0 == 0 && text == "LogSensitiveData" && 0 == 0)
		{
			xrpuh.ngqry = (bool)p1;
			return;
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	void gnyvx.vhvwu(string p0, object p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xltfk
		this.xltfk(p0, p1);
	}
}
