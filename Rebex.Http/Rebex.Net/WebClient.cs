using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using onrkn;

namespace Rebex.Net;

public class WebClient : IDisposable
{
	private sealed class zmkln
	{
		public HttpRequest qxxlq;

		public WebClient rwxlz;

		public void paned()
		{
			lock (rwxlz.yplup)
			{
				rwxlz.hszuy.Remove(qxxlq);
			}
		}
	}

	private sealed class mkodv
	{
		public Stream bkcxt;

		public WebClient xpbtz;

		public void cxhxe(HttpRequest p0)
		{
			Stream stream = xpbtz.tksiu(p0);
			try
			{
				stream.alskc(bkcxt);
			}
			finally
			{
				if (stream != null && 0 == 0)
				{
					((IDisposable)stream).Dispose();
				}
			}
		}
	}

	private sealed class wcroq
	{
		public HttpRequest xvvag;

		public WebClient xstnu;

		public void yrdvt()
		{
			try
			{
				WebResponse response = xvvag.GetResponse();
				xstnu.qgnpi = response.Headers;
				response.Close();
			}
			finally
			{
				lock (xstnu.yplup)
				{
					xstnu.hszuy.Remove(xvvag);
				}
			}
		}
	}

	private sealed class uewrk
	{
		public WebClient xfvaj;

		public byte[] cahkj;

		public byte[] mzgjn(HttpRequest p0)
		{
			Stream stream = xfvaj.idccg(p0, cahkj.Length);
			try
			{
				stream.Write(cahkj, 0, cahkj.Length);
			}
			finally
			{
				if (stream != null && 0 == 0)
				{
					((IDisposable)stream).Dispose();
				}
			}
			Stream stream2 = xfvaj.tksiu(p0);
			try
			{
				return xfvaj.aikha(stream2);
			}
			finally
			{
				if (stream2 != null && 0 == 0)
				{
					((IDisposable)stream2).Dispose();
				}
			}
		}
	}

	private sealed class yxfrz
	{
		public WebClient rupkw;

		public string jpvfu;

		public string rrvge(HttpRequest p0)
		{
			long p1 = rupkw.ezcfo.GetByteCount(jpvfu);
			Stream stream = rupkw.idccg(p0, p1);
			try
			{
				brgjd.jqfnk(jpvfu, stream, rupkw.ezcfo);
			}
			finally
			{
				if (stream != null && 0 == 0)
				{
					((IDisposable)stream).Dispose();
				}
			}
			Stream stream2 = rupkw.tksiu(p0);
			try
			{
				return brgjd.qfhzc(stream2, rupkw.ezcfo);
			}
			finally
			{
				if (stream2 != null && 0 == 0)
				{
					((IDisposable)stream2).Dispose();
				}
			}
		}
	}

	private sealed class rpjiq
	{
		public WebClient bzryt;

		public string btmlv;

		private static vtdxm.urozv mscjh;

		public byte[] wodlk(HttpRequest p0)
		{
			string p1 = btmlv;
			if (mscjh == null || 1 == 0)
			{
				mscjh = utdjj;
			}
			FileStream fileStream = vtdxm.xorlw(p1, FileMode.Open, FileAccess.Read, FileShare.Read, mscjh);
			try
			{
				long length = fileStream.Length;
				Stream stream = bzryt.idccg(p0, length);
				try
				{
					fileStream.alskc(stream);
				}
				finally
				{
					if (stream != null && 0 == 0)
					{
						((IDisposable)stream).Dispose();
					}
				}
			}
			finally
			{
				if (fileStream != null && 0 == 0)
				{
					((IDisposable)fileStream).Dispose();
				}
			}
			Stream stream2 = bzryt.tksiu(p0);
			try
			{
				return bzryt.aikha(stream2);
			}
			finally
			{
				if (stream2 != null && 0 == 0)
				{
					((IDisposable)stream2).Dispose();
				}
			}
		}

		private static Exception utdjj(string p0, Exception p1)
		{
			return new WebException(p0, p1);
		}
	}

	private sealed class ellzc
	{
		public WebClient eavaj;

		public NameValueCollection jiqli;

		public byte[] jgehk(HttpRequest p0)
		{
			p0.ContentType = "application/x-www-form-urlencoded";
			long p1 = dtjod.kkxgc(jiqli, eavaj.ezcfo);
			Stream stream = eavaj.idccg(p0, p1);
			try
			{
				dtjod.kbrxv(jiqli, stream, eavaj.ezcfo);
			}
			finally
			{
				if (stream != null && 0 == 0)
				{
					((IDisposable)stream).Dispose();
				}
			}
			Stream stream2 = eavaj.tksiu(p0);
			try
			{
				return eavaj.aikha(stream2);
			}
			finally
			{
				if (stream2 != null && 0 == 0)
				{
					((IDisposable)stream2).Dispose();
				}
			}
		}
	}

	private sealed class hwtmy
	{
		public EventHandler<WebClientProgressChangedEventArgs> olhuy;

		public WebClient bgubu;

		public void bcijh(object p0, yvnjx p1)
		{
			olhuy(bgubu, new WebClientProgressChangedEventArgs(p1));
		}
	}

	private sealed class cmgil
	{
		public EventHandler<WebClientProgressChangedEventArgs> jtaiq;

		public WebClient ruwmq;

		public void bnsln(object p0, yvnjx p1)
		{
			jtaiq(ruwmq, new WebClientProgressChangedEventArgs(p1));
		}
	}

	private sealed class opumo
	{
		public Action<HttpRequest> cqsyd;

		public object xjwuj(HttpRequest p0)
		{
			cqsyd(p0);
			return null;
		}
	}

	private readonly object yplup;

	private static int wotgy;

	private readonly int mwbms;

	private readonly sjhqe jgwfl;

	private readonly HttpRequestCreator enbby;

	private ICredentials yfhge;

	private Dictionary<HttpRequest, object> hszuy;

	private int ltldl;

	private WebHeaderCollection vodig;

	private WebHeaderCollection qgnpi;

	private Encoding ezcfo = Encoding.UTF8;

	private EventHandler<WebClientProgressChangedEventArgs> xcslb;

	private EventHandler<WebClientProgressChangedEventArgs> bxbvh;

	private string rqlto;

	private static vtdxm.urozv uprck;

	public int Timeout
	{
		get
		{
			return ltldl;
		}
		set
		{
			if (value < -1)
			{
				throw hifyx.nztrs("value", value, "Timeout is out of range of valid values.");
			}
			if (value < 1)
			{
				value = -1;
			}
			else if (value < 1000)
			{
				throw hifyx.nztrs("value", value, "Minimal value is 1000.");
			}
			ltldl = value;
		}
	}

	public ILogWriter LogWriter
	{
		get
		{
			return enbby.LogWriter;
		}
		set
		{
			enbby.LogWriter = value;
		}
	}

	public Proxy Proxy
	{
		get
		{
			return enbby.Proxy;
		}
		set
		{
			enbby.Proxy = value;
		}
	}

	public string BaseAddress
	{
		get
		{
			return rqlto;
		}
		set
		{
			rqlto = value;
		}
	}

	public WebHeaderCollection Headers
	{
		get
		{
			if (vodig != null && 0 == 0)
			{
				return vodig;
			}
			return vodig = new WebHeaderCollection();
		}
		set
		{
			vodig = value;
		}
	}

	public WebHeaderCollection ResponseHeaders
	{
		get
		{
			if (qgnpi != null && 0 == 0)
			{
				return qgnpi;
			}
			return qgnpi = new WebHeaderCollection();
		}
	}

	public ICredentials Credentials
	{
		get
		{
			return yfhge;
		}
		set
		{
			if (value is NetworkCredential && 0 == 0)
			{
				yfhge = (NetworkCredential)value;
				return;
			}
			if (value == null)
			{
				yfhge = null;
				return;
			}
			throw new ArgumentException("Only NetworkCredential values are supported.", "value");
		}
	}

	public HttpSettings Settings => enbby.Settings;

	public Encoding Encoding
	{
		get
		{
			return ezcfo;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			ezcfo = value;
		}
	}

	public event EventHandler<SslCertificateValidationEventArgs> ValidatingCertificate
	{
		add
		{
			enbby.ValidatingCertificate += value;
		}
		remove
		{
			enbby.ValidatingCertificate -= value;
		}
	}

	public event EventHandler<WebClientProgressChangedEventArgs> UploadProgressChanged
	{
		add
		{
			EventHandler<WebClientProgressChangedEventArgs> eventHandler = xcslb;
			EventHandler<WebClientProgressChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<WebClientProgressChangedEventArgs> value2 = (EventHandler<WebClientProgressChangedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref xcslb, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<WebClientProgressChangedEventArgs> eventHandler = xcslb;
			EventHandler<WebClientProgressChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<WebClientProgressChangedEventArgs> value2 = (EventHandler<WebClientProgressChangedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref xcslb, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<WebClientProgressChangedEventArgs> DownloadProgressChanged
	{
		add
		{
			EventHandler<WebClientProgressChangedEventArgs> eventHandler = bxbvh;
			EventHandler<WebClientProgressChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<WebClientProgressChangedEventArgs> value2 = (EventHandler<WebClientProgressChangedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref bxbvh, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<WebClientProgressChangedEventArgs> eventHandler = bxbvh;
			EventHandler<WebClientProgressChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<WebClientProgressChangedEventArgs> value2 = (EventHandler<WebClientProgressChangedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref bxbvh, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public WebClient()
		: this(new HttpRequestCreator())
	{
	}

	protected WebClient(HttpRequestCreator creator)
	{
		if (creator == null || 1 == 0)
		{
			throw new ArgumentNullException("creator");
		}
		yplup = new object();
		mwbms = Interlocked.Increment(ref wotgy);
		jgwfl = creator.xrpuh.ymjug(GetType(), mwbms);
		hszuy = new Dictionary<HttpRequest, object>();
		enbby = creator;
		ltldl = 60000;
	}

	public void SetSocketFactory(ISocketFactory factory)
	{
		enbby.SetSocketFactory(factory);
	}

	[evron]
	public Stream OpenRead(string uri)
	{
		Uri p = new Uri(uri, UriKind.RelativeOrAbsolute);
		return vkwkv(p);
	}

	[evron]
	public Stream OpenRead(Uri uri)
	{
		return vkwkv(uri);
	}

	private Stream vkwkv(Uri p0)
	{
		return pgmaw(p0, "GET", p2: true, ktncj);
	}

	[evron]
	public WebHeaderCollection GetHeaders(string uri)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return GetHeaders(uri2);
	}

	[evron]
	public WebHeaderCollection GetHeaders(Uri uri)
	{
		return bgfsa(uri);
	}

	private WebHeaderCollection bgfsa(Uri p0)
	{
		return cxlgw(p0, "HEAD", ikzfq);
	}

	[evron]
	public byte[] DownloadData(string uri)
	{
		Uri p = new Uri(uri, UriKind.RelativeOrAbsolute);
		return gychv(p);
	}

	[evron]
	public byte[] DownloadData(Uri uri)
	{
		return gychv(uri);
	}

	private byte[] gychv(Uri p0)
	{
		return cxlgw(p0, "GET", swnjo);
	}

	[evron]
	public string DownloadString(string uri)
	{
		Uri p = new Uri(uri, UriKind.RelativeOrAbsolute);
		return yzgbs(p);
	}

	[evron]
	public string DownloadString(Uri uri)
	{
		return yzgbs(uri);
	}

	private string yzgbs(Uri p0)
	{
		return cxlgw(p0, "GET", vcggz);
	}

	[evron]
	public void DownloadFile(string uri, string filePath)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		DownloadFile(uri2, filePath);
	}

	[evron]
	public void DownloadFile(Uri uri, string filePath)
	{
		if (filePath == null || 1 == 0)
		{
			throw new ArgumentNullException("filePath");
		}
		if (filePath == string.Empty && 0 == 0)
		{
			throw new ArgumentException("File path cannot be empty.", "filePath");
		}
		ukmrc(uri, filePath);
	}

	private void ukmrc(Uri p0, string p1)
	{
		Action<HttpRequest> action = null;
		mkodv mkodv = new mkodv();
		mkodv.xpbtz = this;
		if (uprck == null || 1 == 0)
		{
			uprck = guzag;
		}
		mkodv.bkcxt = vtdxm.xorlw(p1, FileMode.Create, FileAccess.Write, FileShare.Read, uprck);
		try
		{
			if (action == null || 1 == 0)
			{
				action = mkodv.cxhxe;
			}
			rcymh(p0, "GET", action);
		}
		finally
		{
			if (mkodv.bkcxt != null && 0 == 0)
			{
				((IDisposable)mkodv.bkcxt).Dispose();
			}
		}
	}

	[evron]
	public Stream OpenWrite(string uri)
	{
		Uri p = new Uri(uri, UriKind.RelativeOrAbsolute);
		return cjimq(p, "POST");
	}

	[evron]
	public Stream OpenWrite(Uri uri)
	{
		return cjimq(uri, "POST");
	}

	[evron]
	public Stream OpenWrite(string uri, string method)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return OpenWrite(uri2, method);
	}

	[evron]
	public Stream OpenWrite(Uri uri, string method)
	{
		if (method == null || 1 == 0)
		{
			method = "POST";
		}
		return cjimq(uri, method);
	}

	private Stream cjimq(Uri p0, string p1)
	{
		return pgmaw(p0, p1, p2: true, okpnd);
	}

	[evron]
	public byte[] UploadData(string uri, byte[] data)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return UploadData(uri2, data);
	}

	[evron]
	public byte[] UploadData(Uri uri, byte[] data)
	{
		return UploadData(uri, null, data);
	}

	[evron]
	public byte[] UploadData(string uri, string method, byte[] data)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return UploadData(uri2, method, data);
	}

	[evron]
	public byte[] UploadData(Uri uri, string method, byte[] data)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (method == null || 1 == 0)
		{
			method = "POST";
		}
		return szlkt(uri, method, data);
	}

	private byte[] szlkt(Uri p0, string p1, byte[] p2)
	{
		uewrk uewrk = new uewrk();
		uewrk.cahkj = p2;
		uewrk.xfvaj = this;
		return cxlgw(p0, p1, uewrk.mzgjn);
	}

	[evron]
	public string UploadString(string uri, string data)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return UploadString(uri2, data);
	}

	[evron]
	public string UploadString(Uri uri, string data)
	{
		return UploadString(uri, null, data);
	}

	[evron]
	public string UploadString(string uri, string method, string data)
	{
		Uri p = new Uri(uri, UriKind.RelativeOrAbsolute);
		return urcai(p, method, data);
	}

	[evron]
	public string UploadString(Uri uri, string method, string data)
	{
		if (method == null || 1 == 0)
		{
			method = "POST";
		}
		return urcai(uri, method, data);
	}

	private string urcai(Uri p0, string p1, string p2)
	{
		yxfrz yxfrz = new yxfrz();
		yxfrz.jpvfu = p2;
		yxfrz.rupkw = this;
		return cxlgw(p0, p1, yxfrz.rrvge);
	}

	[evron]
	public byte[] UploadFile(string uri, string filePath)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return UploadFile(uri2, filePath);
	}

	[evron]
	public byte[] UploadFile(Uri uri, string filePath)
	{
		return UploadFile(uri, null, filePath);
	}

	[evron]
	public byte[] UploadFile(string uri, string method, string filePath)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return UploadFile(uri2, method, filePath);
	}

	[evron]
	public byte[] UploadFile(Uri uri, string method, string filePath)
	{
		if (filePath == null || 1 == 0)
		{
			throw new ArgumentNullException("filePath");
		}
		if (filePath == string.Empty && 0 == 0)
		{
			throw new ArgumentException("File path cannot be empty.", "filePath");
		}
		if (method == null || 1 == 0)
		{
			method = "POST";
		}
		return kunkn(uri, method, filePath);
	}

	private byte[] kunkn(Uri p0, string p1, string p2)
	{
		rpjiq rpjiq = new rpjiq();
		rpjiq.btmlv = p2;
		rpjiq.bzryt = this;
		return cxlgw(p0, p1, rpjiq.wodlk);
	}

	[evron]
	public byte[] UploadValues(string uri, NameValueCollection values)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return UploadValues(uri2, values);
	}

	[evron]
	public byte[] UploadValues(Uri uri, NameValueCollection values)
	{
		return UploadValues(uri, null, values);
	}

	[evron]
	public byte[] UploadValues(string uri, string method, NameValueCollection values)
	{
		Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
		return UploadValues(uri2, method, values);
	}

	[evron]
	public byte[] UploadValues(Uri uri, string method, NameValueCollection values)
	{
		if (values == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		if (method == null || 1 == 0)
		{
			method = "POST";
		}
		return csuvc(uri, method, values);
	}

	private byte[] csuvc(Uri p0, string p1, NameValueCollection p2)
	{
		ellzc ellzc = new ellzc();
		ellzc.jiqli = p2;
		ellzc.eavaj = this;
		return cxlgw(p0, p1, ellzc.jgehk);
	}

	private byte[] aikha(Stream p0)
	{
		opjbe opjbe = new opjbe();
		p0.alskc(opjbe);
		return opjbe.urpqw();
	}

	private Stream idccg(HttpRequest p0, long p1)
	{
		EventHandler<yvnjx> eventHandler = null;
		hwtmy hwtmy = new hwtmy();
		hwtmy.bgubu = this;
		Stream requestStream = p0.GetRequestStream();
		hwtmy.olhuy = xcslb;
		if (hwtmy.olhuy != null && 0 == 0)
		{
			peuid peuid = new peuid(requestStream, null, -1L, p1);
			if (eventHandler == null || 1 == 0)
			{
				eventHandler = hwtmy.bcijh;
			}
			peuid.gofvo += eventHandler;
			return peuid;
		}
		return requestStream;
	}

	private Stream tksiu(HttpRequest p0)
	{
		EventHandler<yvnjx> eventHandler = null;
		cmgil cmgil = new cmgil();
		cmgil.ruwmq = this;
		HttpResponse httpResponse = (HttpResponse)p0.GetResponse();
		qgnpi = httpResponse.Headers;
		long contentLength = httpResponse.ContentLength;
		Stream stream = httpResponse.GetResponseStream();
		cmgil.jtaiq = bxbvh;
		if (cmgil.jtaiq != null && 0 == 0)
		{
			peuid peuid = new peuid(stream, null, contentLength, -1L);
			if (eventHandler == null || 1 == 0)
			{
				eventHandler = cmgil.bnsln;
			}
			peuid.nunrb += eventHandler;
			stream = peuid;
		}
		return stream;
	}

	private Uri onkfz(Uri p0)
	{
		if (p0 == null && 0 == 0)
		{
			throw new ArgumentNullException("uri");
		}
		if ((!p0.IsAbsoluteUri || 1 == 0) && BaseAddress != null && 0 == 0 && BaseAddress != string.Empty && 0 == 0)
		{
			p0 = new Uri(new Uri(BaseAddress), p0);
		}
		if (!p0.IsAbsoluteUri || 1 == 0)
		{
			throw new ArgumentException("Uri cannot be relative.", "uri");
		}
		if (p0.Scheme != "http" && 0 == 0 && p0.Scheme != "https" && 0 == 0)
		{
			throw new WebException("Unsupported scheme in uri.");
		}
		return p0;
	}

	private void rcymh(Uri p0, string p1, Action<HttpRequest> p2)
	{
		opumo opumo = new opumo();
		opumo.cqsyd = p2;
		pgmaw(p0, p1, p2: false, opumo.xjwuj);
	}

	private T cxlgw<T>(Uri p0, string p1, Func<HttpRequest, T> p2)
	{
		return pgmaw(p0, p1, p2: false, p2);
	}

	private T pgmaw<T>(Uri p0, string p1, bool p2, Func<HttpRequest, T> p3)
	{
		Uri uri = onkfz(p0);
		HttpRequest httpRequest;
		lock (yplup)
		{
			httpRequest = enbby.Create(uri);
			hszuy.Add(httpRequest, null);
		}
		jgwfl.byfnx(LogLevel.Info, "HTTP", "Created new HTTP request ({0}) provided by {1}({2}).", httpRequest.carho, enbby.GetType(), enbby.yffyb);
		bool flag = false;
		try
		{
			httpRequest.Method = p1;
			httpRequest.Headers = zjpbq.abeze(Headers);
			httpRequest.Credentials = Credentials;
			httpRequest.Timeout = ltldl;
			return p3(httpRequest);
		}
		catch (Exception p4)
		{
			flag = true;
			httpRequest.lisyx(p4);
			throw;
		}
		finally
		{
			if ((flag ? true : false) || !p2 || 1 == 0)
			{
				lock (yplup)
				{
					hszuy.Remove(httpRequest);
				}
			}
		}
	}

	public void Cancel()
	{
		lock (yplup)
		{
			using (Dictionary<HttpRequest, object>.KeyCollection.Enumerator enumerator = hszuy.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext() ? true : false)
				{
					HttpRequest current = enumerator.Current;
					jgwfl.byfnx(LogLevel.Info, "HTTP", "Canceling HTTP request ({0}).", current.carho);
					current.kyzrx();
				}
			}
			hszuy.Clear();
		}
	}

	public void Dispose()
	{
	}

	private eejdw ktncj(HttpRequest p0)
	{
		zmkln zmkln = new zmkln();
		zmkln.qxxlq = p0;
		zmkln.rwxlz = this;
		return new eejdw(tksiu(zmkln.qxxlq), zmkln.paned);
	}

	private WebHeaderCollection ikzfq(HttpRequest p0)
	{
		Stream stream = tksiu(p0);
		try
		{
			aikha(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
		return qgnpi;
	}

	private byte[] swnjo(HttpRequest p0)
	{
		Stream stream = tksiu(p0);
		try
		{
			return aikha(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	private string vcggz(HttpRequest p0)
	{
		Stream stream = tksiu(p0);
		try
		{
			return brgjd.qfhzc(stream, ezcfo);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	private static Exception guzag(string p0, Exception p1)
	{
		return new WebException(p0, p1);
	}

	private eejdw okpnd(HttpRequest p0)
	{
		wcroq wcroq = new wcroq();
		wcroq.xvvag = p0;
		wcroq.xstnu = this;
		Stream baseStream = idccg(wcroq.xvvag, -1L);
		return new eejdw(baseStream, wcroq.yrdvt);
	}
}
