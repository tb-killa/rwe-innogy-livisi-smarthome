using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using Rebex.Net.ConnectionManagement;
using Rebex.Security.Authentication;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public class HttpRequest : WebRequest, gnyvx
{
	private enum akvmm
	{
		kdgbj,
		mrpvu
	}

	private sealed class nmhhl
	{
		public ManualResetEvent wcyoj;

		public nzqad cwvof;

		public Exception blloi;

		public HttpRequest hhqke;

		public void sqxkj()
		{
			lock (hhqke.sremt)
			{
				wcyoj.Set();
			}
		}

		public void qimbz()
		{
			HttpResponse p;
			try
			{
				p = hhqke.xhopj();
			}
			catch (Exception ex)
			{
				blloi = ex;
				p = null;
				cwvof.geugb(blloi);
			}
			lock (hhqke.sremt)
			{
				wcyoj.Set();
				hhqke.jozxe.Set();
				hhqke.gvlmr = ujajv.slcvx(p, blloi);
			}
		}
	}

	private sealed class yxqxr
	{
		public ixwxt fjfss;

		public ISocketFactory ixezl;

		public string yqblw;

		public HttpRequest octra;

		public void fictr(nymgm p0)
		{
			bool flag = true;
			if (octra.nanex != null && 0 == 0 && (!p0.swbbq || 1 == 0) && (!octra.ttzyv || 1 == 0) && octra.nanex.nahka(fjfss, ixezl, yqblw) && 0 == 0)
			{
				flag = false;
			}
			if (flag && 0 == 0)
			{
				fjfss.Dispose();
			}
		}
	}

	private const string fafml = "Rebex HTTPS";

	private const int tapfb = 16384;

	private const string jjima = "bytes";

	private const string pfsnd = "Timeout";

	private static int pvjeb;

	private readonly int dzucm;

	private readonly awngk bmalm;

	private HttpRequestCreator nanex;

	private Uri macgp;

	private Uri fpaao;

	private string ssvpk;

	private string jtxeu;

	private WebHeaderCollection jmkym;

	private int mgmdr;

	private int uiddg;

	private ixwxt cgoto;

	private ISocketFactory fpchl;

	private ICredentials hotnl;

	private HttpResponse pzpvd;

	private int djknt;

	private bool wvveb;

	private CertificateCollection yxjoq;

	private znuay yrylu;

	private object sremt = new object();

	private ubdvb uuqhi;

	private ailrt hhqyk;

	private Action<int> cxrkt;

	private Stream hcgta;

	private wbvsh pgkok;

	private HttpResponse dwqhv;

	private hcqmh<HttpResponse, Exception> gvlmr;

	private ManualResetEvent jozxe;

	private string hnakk;

	private bool gvlru;

	private bool mlrpb;

	private bool knppf;

	private string looxv;

	private string tvmij;

	private bool vcyjy;

	private string himnq;

	private DecompressionMethods oruho;

	internal int carho => dzucm;

	internal bool ttzyv => yrylu.mpjbd;

	public bool AllowAutoRedirect
	{
		get
		{
			return gvlru;
		}
		set
		{
			gvlru = value;
		}
	}

	public bool AllowWriteStreamBuffering
	{
		get
		{
			return mlrpb;
		}
		set
		{
			mlrpb = value;
		}
	}

	public int MaximumAutomaticRedirections
	{
		get
		{
			return djknt;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			djknt = value;
		}
	}

	public string Accept
	{
		get
		{
			return jmkym["Accept"];
		}
		set
		{
			jmkym["Accept"] = value;
		}
	}

	public string Connection
	{
		get
		{
			return jmkym["Connection"];
		}
		set
		{
			jmkym["Connection"] = value;
		}
	}

	public override string ContentType
	{
		get
		{
			return jmkym["Content-Type"];
		}
		set
		{
			jmkym["Content-Type"] = value;
		}
	}

	public override long ContentLength
	{
		get
		{
			if (brgjd.hujyn(jmkym["Content-Length"], out var p) && 0 == 0)
			{
				return p;
			}
			return -1L;
		}
		set
		{
			jmkym["Content-Length"] = value.ToString();
		}
	}

	public bool SendChunked
	{
		get
		{
			string text = jmkym["Transfer-Encoding"];
			if (text != null && 0 == 0 && string.Equals(text, "chunked", StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				return true;
			}
			return false;
		}
		set
		{
			if (value && 0 == 0)
			{
				jmkym["Transfer-Encoding"] = "chunked";
			}
			else
			{
				jmkym.Remove("Transfer-Encoding");
			}
		}
	}

	public string Expect
	{
		get
		{
			return jmkym["Expect"];
		}
		set
		{
			jmkym["Expect"] = value;
		}
	}

	public bool Expect100Continue
	{
		get
		{
			return knppf;
		}
		set
		{
			knppf = value;
		}
	}

	public int ContinueTimeout
	{
		get
		{
			return uiddg;
		}
		set
		{
			if (value < 0)
			{
				throw hifyx.nztrs("value", value, "Argument must not be negative number.");
			}
			uiddg = value;
		}
	}

	public bool HaveResponse => pzpvd != null;

	public string IfModifiedSince
	{
		get
		{
			return jmkym["If-Modified-Since"];
		}
		set
		{
			jmkym["If-Modified-Since"] = value;
		}
	}

	public bool KeepAlive
	{
		get
		{
			string text = jmkym["Connection"];
			if (text != null && 0 == 0)
			{
				return string.Equals(text, "keep-alive", StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}
		set
		{
			jmkym["Connection"] = ((value ? true : false) ? "keep-alive" : "close");
		}
	}

	public override string Method
	{
		get
		{
			return ssvpk;
		}
		set
		{
			if (string.IsNullOrEmpty(value) && 0 == 0)
			{
				throw new ArgumentException("Method cannot be set to null or empty string.", "value");
			}
			ssvpk = value;
		}
	}

	public override WebHeaderCollection Headers
	{
		get
		{
			return jmkym;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			jmkym = value;
		}
	}

	public string Referer
	{
		get
		{
			return jmkym["Referer"];
		}
		set
		{
			jmkym["Referer"] = value;
		}
	}

	public Uri Address => fpaao;

	public override Uri RequestUri => macgp;

	public override int Timeout
	{
		get
		{
			return mgmdr;
		}
		set
		{
			if (value < -1)
			{
				throw hifyx.nztrs("value", value, "Timeout is out of range of valid values.");
			}
			if (value > 0 && value < 1000)
			{
				value = 1000;
			}
			mgmdr = value;
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string TransferEncoding
	{
		get
		{
			return looxv;
		}
		set
		{
			looxv = value;
		}
	}

	public override ICredentials Credentials
	{
		get
		{
			return hotnl;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				hotnl = null;
				return;
			}
			if (value is NetworkCredential || value is CredentialCache)
			{
				hotnl = value;
				return;
			}
			throw new ArgumentException("Only NetworkCredential and CredentialCache values are supported.", "value");
		}
	}

	public override string ConnectionGroupName
	{
		get
		{
			return tvmij;
		}
		set
		{
			tvmij = value;
		}
	}

	public override bool PreAuthenticate
	{
		get
		{
			return vcyjy;
		}
		set
		{
			vcyjy = value;
		}
	}

	public string UserAgent
	{
		get
		{
			return himnq;
		}
		set
		{
			himnq = value;
		}
	}

	public DecompressionMethods AutomaticDecompression
	{
		get
		{
			return oruho;
		}
		set
		{
			oruho = value;
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("HttpWebRequest.Proxy is not supported. Please use HttpRequestCreator.Proxy instead.", false)]
	public new Proxy Proxy
	{
		get
		{
			throw new NotSupportedException("Please use HttpRequestCreator.Proxy property instead.");
		}
		set
		{
			throw new NotSupportedException("Please use HttpRequestCreator.Proxy property instead.");
		}
	}

	public CertificateCollection ClientCertificates
	{
		get
		{
			return yxjoq;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("Collection cannot be null.", "value");
			}
			yxjoq = value;
		}
	}

	public HttpRequest(Uri uri, HttpRequestCreator creator)
	{
		if (uri == null && 0 == 0)
		{
			throw new ArgumentNullException("uri");
		}
		if (creator == null || 1 == 0)
		{
			creator = new HttpRequestCreator();
		}
		dzucm = Interlocked.Increment(ref pvjeb);
		macgp = uri;
		fpaao = uri;
		nanex = creator;
		yrylu = new znuay();
		bmalm = new awngk(GetType(), dzucm, nanex.LogWriter);
		jmkym = new WebHeaderCollection();
		yxjoq = new CertificateCollection();
		mgmdr = 60000;
		uiddg = 350;
		Method = "GET";
		MaximumAutomaticRedirections = 50;
		AllowAutoRedirect = true;
		UserAgent = "Rebex HTTPS";
		AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
		AllowWriteStreamBuffering = true;
		Expect100Continue = true;
	}

	public override bool Equals(object obj)
	{
		if (obj is HttpRequest httpRequest && 0 == 0)
		{
			return httpRequest.dzucm == dzucm;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return dzucm;
	}

	public void AddRange(long offset)
	{
		AddRange("bytes", offset);
	}

	public void AddRange(string unit, long offset)
	{
		if (offset < 0)
		{
			clqmp(unit, null, -offset);
		}
		else
		{
			clqmp(unit, offset, null);
		}
	}

	public void AddRange(long from, long to)
	{
		clqmp("bytes", from, to);
	}

	public void AddRange(string unit, long from, long to)
	{
		clqmp(unit, from, to);
	}

	private void clqmp(string p0, long? p1, long? p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("unit");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "unit");
		}
		if ((p1 ?? 0) < 0)
		{
			throw hifyx.nztrs("from", p1, "Argument must not be negative number.");
		}
		if ((p2 ?? 0) < 0)
		{
			throw hifyx.nztrs("to", p2, "Argument must not be negative number.");
		}
		if ((p1 ?? 0) > (p2 ?? long.MaxValue))
		{
			throw new ArgumentException("Invalid range definition ('from' cannot be greater than 'to').");
		}
		if (!brgjd.ynufr(p0, '_', '-') || 1 == 0)
		{
			throw new ArgumentException("String contains invalid characters.", "unit");
		}
		object obj = jmkym["Range"];
		if (obj == null || 1 == 0)
		{
			obj = "";
		}
		string text = (string)obj;
		int num = text.IndexOf('=');
		if (num > 0)
		{
			if (!p0.Equals(text.Substring(0, num), StringComparison.OrdinalIgnoreCase) || 1 == 0)
			{
				throw new ArgumentException(brgjd.edcru("Range header specified another unit already (new={0}, was={1}).", p0, text.Substring(0, num)), "unit");
			}
			text = brgjd.edcru("{0}, {1}-{2}", text, p1, p2);
		}
		else
		{
			if (text.Length != 0 && 0 == 0)
			{
				throw new InvalidOperationException(brgjd.edcru("Range header contains invalid value ({0}).", text));
			}
			text = brgjd.edcru("{0}={1}-{2}", p0, p1, p2);
		}
		jmkym["Range"] = text;
	}

	private static IAsyncResult fwygk(bool p0, HttpRequest p1, akvmm p2, AsyncCallback p3, object p4, params object[] p5)
	{
		if (p1 != null && 0 == 0)
		{
			return p1.fwmko(p0, p2, p3, p4, p5);
		}
		ubdvb ubdvb = new ubdvb(p0, kpkzd, null, p2, null, p3, p4, p5);
		ubdvb.nvhuz();
		return ubdvb;
	}

	private IAsyncResult fwmko(bool p0, akvmm p1, AsyncCallback p2, object p3, params object[] p4)
	{
		lock (sremt)
		{
			if (uuqhi != null || hhqyk != null)
			{
				throw new WebException("Another operation is pending.", WebExceptionStatus.Pending);
			}
			ubdvb ubdvb = (uuqhi = new ubdvb(p0, kpkzd, this, p1, null, p2, p3, p4));
			hhqyk = ailrt.dvgbg();
			ubdvb.nvhuz();
			return ubdvb;
		}
	}

	private static object kmdnw(IAsyncResult p0, HttpRequest p1, string p2, out Exception p3, params Enum[] p4)
	{
		ubdvb ubdvb = ubdvb.ovklt(p0, p1, p2, p4);
		if (p1 != null && 0 == 0)
		{
			lock (p1.sremt)
			{
				p1.uuqhi = null;
				p1.hhqyk = null;
			}
		}
		p3 = ubdvb.oybzl;
		return ubdvb.dcenx;
	}

	private static object vneix(IAsyncResult p0, HttpRequest p1, string p2, params Enum[] p3)
	{
		Exception p4;
		object result = kmdnw(p0, p1, p2, out p4, p3);
		if (p4 != null && 0 == 0)
		{
			if (p4 is WebException ex && 0 == 0)
			{
				throw new WebException("An exception occurred in asynchronous method.", ex, ex.Status, ex.Response);
			}
			throw new WebException("Asynchronous call exception.", p4);
		}
		return result;
	}

	public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
	{
		return tjzku(callback, state);
	}

	public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
	{
		return gciym(callback, state);
	}

	public override Stream EndGetRequestStream(IAsyncResult asyncResult)
	{
		return rywpy(asyncResult, "GetRequestStream");
	}

	public override WebResponse EndGetResponse(IAsyncResult asyncResult)
	{
		return dbygv(asyncResult, "GetResponse");
	}

	public override void Abort()
	{
		kyzrx();
	}

	internal void kyzrx()
	{
		yrylu.pvutk();
		ixwxt ixwxt = cgoto;
		if (ixwxt != null && 0 == 0)
		{
			ixwxt.htvzd();
		}
		HttpResponse httpResponse = dwqhv;
		if (httpResponse != null && 0 == 0)
		{
			httpResponse.dkxpa();
		}
	}

	private void masbf()
	{
		if (ttzyv && 0 == 0)
		{
			throw qonyc(null);
		}
	}

	internal void lisyx(Exception p0)
	{
		if (ttzyv && 0 == 0)
		{
			throw qonyc(p0);
		}
	}

	internal WebException qonyc(Exception p0)
	{
		if (p0 != null && 0 == 0 && (!jikdq(p0) || 1 == 0))
		{
			bmalm.byfnx(LogLevel.Info, "HTTP", "HTTP request was canceled. Ignoring error: {0}", p0);
		}
		return new WebException("Operation was canceled.", null, WebExceptionStatus.RequestCanceled, pzpvd);
	}

	private bool jikdq(Exception p0)
	{
		if (p0 != null && 0 == 0 && (!(p0 is ObjectDisposedException) || 1 == 0))
		{
			WebException ex = p0 as WebException;
			ujepc ujepc = p0 as ujepc;
			if (ex == null || false || ex.Status != WebExceptionStatus.RequestCanceled)
			{
				if (ujepc != null && 0 == 0)
				{
					return ujepc.zhmeu == ezmya.ydksh;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private NetworkCredential heisw(Uri p0, out fklrq p1)
	{
		if (Credentials == null || 1 == 0)
		{
			p1 = fklrq.uzjbr;
			return null;
		}
		if (Credentials is NetworkCredential && 0 == 0)
		{
			p1 = fklrq.hutob;
			return Credentials as NetworkCredential;
		}
		if (Credentials is CredentialCache && 0 == 0)
		{
			CredentialCache p2 = Credentials as CredentialCache;
			p1 = fklrq.uzjbr;
			NetworkCredential p3 = null;
			ehjvj(p2, p0, fklrq.sjjmf, "Negotiate", p4: true, ref p3, ref p1);
			ehjvj(p2, p0, fklrq.cepgj, "NTLM", p4: true, ref p3, ref p1);
			ehjvj(p2, p0, fklrq.cznae, "Kerberos", p4: true, ref p3, ref p1);
			ehjvj(p2, p0, fklrq.rafbm, "Digest", p4: false, ref p3, ref p1);
			ehjvj(p2, p0, fklrq.okxlf, "Basic", p4: false, ref p3, ref p1);
			return p3;
		}
		throw new InvalidOperationException("Unsupported type of Credentials.");
	}

	private void ehjvj(CredentialCache p0, Uri p1, fklrq p2, string p3, bool p4, ref NetworkCredential p5, ref fklrq p6)
	{
		if (p4 && 0 == 0 && (!SspiAuthentication.IsSupported(p3) || 1 == 0))
		{
			return;
		}
		NetworkCredential credential = p0.GetCredential(p1, p3);
		if (credential != null && 0 == 0)
		{
			if (p5 == null || 1 == 0)
			{
				p5 = credential;
			}
			else if (((p5.UserName != credential.UserName) ? true : false) || ((p5.Password != credential.Password) ? true : false) || p5.Domain != credential.Domain)
			{
				throw new InvalidOperationException(brgjd.edcru("CredentialCache has inconsistent credentials for '{0}'.", p1));
			}
			p6 |= p2;
		}
	}

	private void lwzrw()
	{
		if (cgoto != null && 0 == 0)
		{
			throw new InvalidOperationException("Request already started.");
		}
		masbf();
		fklrq p;
		NetworkCredential networkCredential = heisw(fpaao, out p);
		hnakk = ((KeepAlive ? true : false) ? zjpbq.kzafa(fpaao, networkCredential) : null);
		bool flag = nanex.mxllt(hnakk, out cgoto, out fpchl);
		if (!flag || 1 == 0)
		{
			fpchl = nanex.arqgx;
			cgoto = new ixwxt(fpaao, fpchl, nanex);
		}
		cgoto.sesmx(yrylu);
		cgoto.poazj(bmalm, qsrzi);
		bmalm.byfnx(LogLevel.Debug, "HTTP", "Using {3} HTTP session ({0}) provided by {1}({2}).", cgoto.hxrxh, nanex.GetType(), nanex.yffyb, (flag ? true : false) ? "cached" : "new");
		wvveb = false;
		cgoto.zhaap = p;
		if (networkCredential != null && 0 == 0)
		{
			cgoto.uvhpp = ((string.IsNullOrEmpty(networkCredential.Domain) ? true : false) ? networkCredential.UserName : (networkCredential.Domain + "/" + networkCredential.UserName));
			if (cgoto.uvhpp == string.Empty && 0 == 0)
			{
				cgoto.uvhpp = null;
			}
			if (cgoto.uvhpp != null && 0 == 0)
			{
				cgoto.zeibp = networkCredential.Password;
			}
			else
			{
				cgoto.zhaap &= ~(fklrq.okxlf | fklrq.rafbm);
			}
		}
		cgoto.ykbvb = PreAuthenticate;
		cgoto.mzzkd(fpaao.PathAndQuery);
		pjktg();
	}

	private void pjktg()
	{
		try
		{
			if (nanex.Settings.AutoConnectToInternet == AutoConnectType.Enabled && (!ConnectionManager.IsConnected() || 1 == 0))
			{
				bmalm.rfpvf(LogLevel.Debug, "HTTP", "AutoConnectToInternet is enabled and no connection has been detected. Establishing connection.");
				bool flag = ConnectionManager.TryConnect();
				bmalm.byfnx(LogLevel.Debug, "HTTP", "Connection established: {0}.", flag);
			}
		}
		catch (Exception ex)
		{
			bmalm.byfnx(LogLevel.Error, "HTTP", "Error occurred while trying to establish a connection: {0}", ex);
		}
	}

	public override Stream GetRequestStream()
	{
		return fhjpy();
	}

	[evron]
	internal Stream fhjpy()
	{
		if (pzpvd != null && 0 == 0)
		{
			throw new InvalidOperationException("This operation cannot be performed after the request has been submitted.");
		}
		return ulfki();
	}

	private Stream ulfki()
	{
		if (pgkok == null || 1 == 0)
		{
			pgkok = new wbvsh(jldff(), yrylu, qonyc);
		}
		masbf();
		return pgkok;
	}

	private Stream jldff()
	{
		try
		{
			nmhhl nmhhl = new nmhhl();
			nmhhl.hhqke = this;
			if (cgoto == null || 1 == 0)
			{
				lwzrw();
			}
			if (hcgta != null && 0 == 0)
			{
				return hcgta;
			}
			bool sendChunked = SendChunked;
			if (AllowWriteStreamBuffering && 0 == 0)
			{
				hcgta = new opjbe(16384);
				cgoto.swlkd(hcgta, sendChunked, null, cxrkt);
				return hcgta;
			}
			long contentLength = ContentLength;
			if (contentLength < 0 && (!sendChunked || 1 == 0))
			{
				throw new ujepc("When uploading with disabled AllowWriteStreamBuffering, either SendChunked must be enabled or ContentLength must be specified.", ezmya.ydksh);
			}
			nmhhl.wcyoj = new ManualResetEvent(initialState: false);
			nmhhl.cwvof = new nzqad(65536, contentLength, nmhhl.sqxkj);
			hcgta = nmhhl.cwvof;
			cgoto.swlkd(nmhhl.cwvof.msodx, sendChunked, contentLength, cxrkt);
			jozxe = new ManualResetEvent(initialState: false);
			nmhhl.blloi = null;
			rxpjc.oxwba(nmhhl.qimbz);
			nmhhl.wcyoj.WaitOne();
			lock (sremt)
			{
				if (nmhhl.blloi != null && 0 == 0)
				{
					throw nmhhl.blloi;
				}
			}
			return hcgta;
		}
		catch (ujepc p)
		{
			lisyx(p);
			throw zjpbq.entdh(p, pzpvd);
		}
		catch (Exception p2)
		{
			lisyx(p2);
			throw;
		}
	}

	internal void czrxc(Action<int> p0)
	{
		cxrkt = p0;
	}

	public override WebResponse GetResponse()
	{
		return ajswj();
	}

	[evron]
	internal WebResponse ajswj()
	{
		return uxwxa();
	}

	private HttpResponse uxwxa()
	{
		if (dwqhv == null || 1 == 0)
		{
			dwqhv = azxmk();
		}
		masbf();
		return dwqhv;
	}

	private HttpResponse azxmk()
	{
		ManualResetEvent manualResetEvent;
		lock (sremt)
		{
			manualResetEvent = jozxe;
		}
		if (manualResetEvent == null || 1 == 0)
		{
			return xhopj();
		}
		manualResetEvent.WaitOne();
		lock (sremt)
		{
			if (gvlmr.cdois != null && 0 == 0)
			{
				throw gvlmr.cdois;
			}
			return gvlmr.amanf;
		}
	}

	private HttpResponse xhopj()
	{
		try
		{
			if (pzpvd != null && 0 == 0)
			{
				return pzpvd;
			}
			if (cgoto == null || 1 == 0)
			{
				lwzrw();
			}
			jtxeu = Method;
			aiwkd();
			int num = 0;
			thths thths;
			TlsCipher wsksi;
			while (true)
			{
				masbf();
				ursls();
				thths = cgoto.kvanb();
				wsksi = cgoto.wsksi;
				num++;
				if (thths.yddmu != pyuak.yvdvx || !AllowAutoRedirect || false || (MaximumAutomaticRedirections > 0 && num >= MaximumAutomaticRedirections))
				{
					break;
				}
				string text = thths.virwn["Location"];
				if (string.IsNullOrEmpty(text) ? true : false)
				{
					break;
				}
				text = text.Trim();
				string text2 = jtxeu;
				int xgkmt = (int)thths.xgkmt;
				switch (xgkmt)
				{
				case 300:
				case 303:
					if (hcgta != null && 0 == 0)
					{
						text2 = "GET";
					}
					break;
				case 301:
				case 302:
				case 307:
					if (jtxeu.Equals("POST", StringComparison.OrdinalIgnoreCase) && 0 == 0 && xgkmt < 303)
					{
						text2 = "GET";
					}
					else if (hcgta != null && 0 == 0 && (!AllowWriteStreamBuffering || 1 == 0))
					{
						pzpvd = xjgbt(thths, wsksi);
						lnsle(thths, p1: true);
						throw new WebException(brgjd.edcru("The remote server returned {0} (Redirect). To complete automatic redirection buffering data needs to be enabled.", xgkmt), null, WebExceptionStatus.ProtocolError, pzpvd);
					}
					break;
				default:
					pzpvd = xjgbt(thths, wsksi);
					lnsle(thths, p1: true);
					throw new WebException(brgjd.edcru("The remote server returned an error: ({0}) {1}.", (int)thths.xgkmt, thths.vbeuo), null, WebExceptionStatus.ProtocolError, pzpvd);
				}
				jmkym.Remove("Transfer-Encoding");
				jmkym.Remove("Content-Length");
				jmkym.Remove("Expect");
				jmkym.Remove("Cookie");
				Uri result;
				bool flag = ((text.IndexOf("://") < 0) ? Uri.TryCreate(fpaao, text, out result) : Uri.TryCreate(text, UriKind.Absolute, out result));
				if (!flag || 1 == 0)
				{
					cgoto.awhlf(thths);
					thths.jpxci();
					pzpvd = xjgbt(thths, wsksi);
					throw new WebException("Unable to parse redirect location.", null, WebExceptionStatus.ServerProtocolViolation, pzpvd);
				}
				if ((!nanex.Settings.AllowRedirectDowngrade || 1 == 0) && fpaao.lslyj() && 0 == 0 && (!result.lslyj() || 1 == 0))
				{
					throw new WebException("The remote server is trying to redirect HTTPS request to an unsecured location. AllowRedirectDowngrade option needs to be enabled to allow this.", null, WebExceptionStatus.ProtocolError, pzpvd);
				}
				string leftPart = result.GetLeftPart(UriPartial.Authority);
				if (leftPart != cgoto.egvak && 0 == 0)
				{
					lnsle(thths, p1: true);
					thths.jpxci();
					fpaao = result;
					jtxeu = text2;
					lwzrw();
					aiwkd();
				}
				else
				{
					cgoto.awhlf(thths);
					thths.jpxci();
					fpaao = result;
					jtxeu = text2;
					cgoto.abooq(jtxeu, fpaao.PathAndQuery, new oxwaf(jmkym));
				}
				if (hcgta != null && 0 == 0 && (!jtxeu.Equals("GET", StringComparison.OrdinalIgnoreCase) || 1 == 0))
				{
					cgoto.swlkd(hcgta, p1: false, null, cxrkt);
				}
			}
			pzpvd = xjgbt(thths, wsksi);
			lnsle(thths, p1: false);
			if (thths.yddmu == pyuak.jojyr || thths.yddmu == pyuak.zibhv)
			{
				throw new WebException(thths.eelqe, null, WebExceptionStatus.ProtocolError, pzpvd);
			}
			return pzpvd;
		}
		catch (ujepc p)
		{
			lisyx(p);
			throw zjpbq.entdh(p, pzpvd);
		}
		catch (Exception p2)
		{
			lisyx(p2);
			throw;
		}
	}

	private void ursls()
	{
	}

	private HttpResponse tzuis(thths p0)
	{
		return xjgbt(p0, cgoto.wsksi);
	}

	private HttpResponse xjgbt(thths p0, TlsCipher p1)
	{
		return new HttpResponse(p0, jtxeu, fpaao, p1);
	}

	private void aiwkd()
	{
		if (!wvveb)
		{
			cgoto.tdnkk = jtxeu;
			cgoto.isvcv = new oxwaf(jmkym);
			cgoto.pjvho = mgmdr;
			cgoto.kqxuj = uiddg;
			cgoto.hvcjz = UserAgent;
			cgoto.xpfip = AutomaticDecompression;
			cgoto.btccd = Expect100Continue;
			cgoto.xpjhp = ClientCertificates;
			wvveb = true;
		}
	}

	private void lnsle(thths p0, bool p1)
	{
		yxqxr yxqxr = new yxqxr();
		yxqxr.octra = this;
		cgoto.kkvyk();
		yxqxr.fjfss = cgoto;
		yxqxr.ixezl = fpchl;
		yxqxr.yqblw = hnakk;
		p0.tbxqq = yxqxr.fictr;
		p0.ruquh = qgwqw;
		cgoto = null;
		fpchl = null;
		hnakk = null;
		if (p1 && 0 == 0)
		{
			yxqxr.fjfss.awhlf(p0);
		}
	}

	private object fpnon(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0 && text == "Timeout" && 0 == 0)
		{
			return mgmdr;
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	object gnyvx.jfzti(string p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fpnon
		return this.fpnon(p0);
	}

	private void ofgor(string p0, object p1)
	{
		string text;
		if ((text = p0) != null && 0 == 0 && text == "Timeout" && 0 == 0)
		{
			if (p1 is int && 0 == 0)
			{
				mgmdr = (int)p1;
				return;
			}
			throw hifyx.nztrs("value", p1, "Invalid argument.");
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	void gnyvx.vhvwu(string p0, object p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ofgor
		this.ofgor(p0, p1);
	}

	private static object kpkzd(object p0, Enum p1, object[] p2)
	{
		HttpRequest httpRequest = (HttpRequest)p0;
		return (akvmm)(object)p1 switch
		{
			akvmm.kdgbj => httpRequest.ulfki(), 
			akvmm.mrpvu => httpRequest.uxwxa(), 
			_ => throw new InvalidOperationException("Invalid asynchronous method."), 
		};
	}

	private void efhin(IAsyncResult p0)
	{
		vneix(p0, this, null, (akvmm)p0.AsyncState);
	}

	private static void ykspc(IAsyncResult p0)
	{
		vneix(p0, null, null, (akvmm)p0.AsyncState);
	}

	internal IAsyncResult tjzku(AsyncCallback p0, object p1)
	{
		if (pzpvd != null && 0 == 0)
		{
			throw new InvalidOperationException("This operation cannot be performed after the request has been submitted.");
		}
		return fwygk(false, this, akvmm.kdgbj, p0, p1);
	}

	private Stream rywpy(IAsyncResult p0, string p1)
	{
		return (Stream)vneix(p0, this, p1, akvmm.kdgbj);
	}

	internal IAsyncResult gciym(AsyncCallback p0, object p1)
	{
		return fwygk(false, this, akvmm.mrpvu, p0, p1);
	}

	private WebResponse dbygv(IAsyncResult p0, string p1)
	{
		return (WebResponse)vneix(p0, this, p1, akvmm.mrpvu);
	}

	private bool qsrzi()
	{
		return nanex.xrpuh.ngqry;
	}

	private Exception qgwqw(ujepc p0)
	{
		return zjpbq.entdh(p0, pzpvd);
	}
}
