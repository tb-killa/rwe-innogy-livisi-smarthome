using System;
using System.Net;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal class ixwxt : rbbcu
{
	private sealed class umrzh
	{
		public string vxpqm;

		public TlsSession wdpvp;

		public TlsSocket yyxkm;

		public ixwxt nlrgi;

		public bool ptcuf()
		{
			wdpvp = yyxkm.Session;
			if (wdpvp == null || 1 == 0)
			{
				return false;
			}
			if (vxpqm != null && 0 == 0)
			{
				nlrgi.ecyyg.gmuvh(wdpvp, vxpqm);
			}
			nlrgi.ecyyg.gmuvh(wdpvp, nlrgi.zemey);
			return true;
		}
	}

	private readonly string zemey;

	private readonly string jcdch;

	private readonly HttpRequestCreator ecyyg;

	private readonly rtmzu ulyke;

	private Func<bool> lzrzt;

	private CertificateCollection bnzmy;

	private static Func<string, Exception> vdzhw;

	public string egvak => zemey;

	public CertificateCollection xpjhp
	{
		get
		{
			return bnzmy;
		}
		set
		{
			bnzmy = value;
		}
	}

	public ixwxt(Uri uri, ISocketFactory socketFactory, HttpRequestCreator creator)
		: base(uri.Host, uri.Port, string.Equals(uri.Scheme, "https", StringComparison.OrdinalIgnoreCase), socketFactory)
	{
		zemey = uri.GetLeftPart(UriPartial.Authority);
		jcdch = uri.Host;
		ecyyg = creator;
		ulyke = creator.acxvn;
	}

	protected override TlsSocket dogje(ISocket p0)
	{
		umrzh umrzh = new umrzh();
		umrzh.nlrgi = this;
		HttpSettings settings = ecyyg.Settings;
		HttpRequestCreator p1 = ecyyg;
		string p2 = jcdch;
		if (vdzhw == null || 1 == 0)
		{
			vdzhw = rnzis;
		}
		TlsParameters tlsParameters = settings.mfcks(null, p1, p2, vdzhw);
		tlsParameters.CertificateRequestHandler = new whatq(xpjhp, tlsParameters.CertificateRequestHandler);
		IPEndPoint iPEndPoint = p0.RemoteEndPoint as IPEndPoint;
		umrzh.vxpqm = ((iPEndPoint == null) ? null : brgjd.edcru("{0}[{1}]", zemey, iPEndPoint));
		umrzh.wdpvp = ((umrzh.vxpqm == null) ? null : ecyyg.kfgrm(umrzh.vxpqm));
		TlsSession tlsSession = umrzh.wdpvp;
		if (tlsSession == null || 1 == 0)
		{
			tlsSession = ecyyg.kfgrm(zemey);
			if (tlsSession == null || 1 == 0)
			{
				tlsSession = ecyyg.Settings.SslSession;
			}
		}
		tlsParameters.Session = tlsSession;
		umrzh.yyxkm = TlsSocket.xxegh(p0);
		umrzh.yyxkm.Timeout = base.pjvho;
		umrzh.yyxkm.LogWriter = base.elpzg;
		umrzh.yyxkm.Parameters = tlsParameters;
		umrzh.yyxkm.Negotiate();
		lzrzt = umrzh.ptcuf;
		kkvyk();
		return umrzh.yyxkm;
	}

	public void kkvyk()
	{
		if (lzrzt != null && 0 == 0 && lzrzt() && 0 == 0)
		{
			lzrzt = null;
		}
	}

	protected override void mhhgl(string p0, string p1)
	{
		ulyke.ptnfh(base.uvhpp, base.zeibp, base.atnsu, base.spmvf, base.xbcyy, p0, p1);
	}

	protected override bool wfupm(out string p0, out string p1)
	{
		return ulyke.wuxuf(base.uvhpp, base.zeibp, base.atnsu, base.spmvf, base.xbcyy, out p0, out p1);
	}

	private static Exception rnzis(string p0)
	{
		return new WebException(p0, WebExceptionStatus.RequestCanceled);
	}
}
