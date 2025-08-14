using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Rebex;
using Rebex.Security.Authentication;

namespace onrkn;

internal abstract class udlmn : IDisposable
{
	private sealed class ignir
	{
		public Action<int> fihey;

		public int hlyfg;

		public udlmn uijme;

		public void jlanl(ArraySegment<byte> p0)
		{
			uijme.kdbdu(p0.Array, p0.Offset, p0.Count);
			hlyfg += p0.Count;
			if (fihey != null && 0 == 0)
			{
				fihey(hlyfg);
			}
		}
	}

	private sealed class tmsmb
	{
		public ignir hzszk;

		public MemoryStream cbmyb;

		public void okrsx(ArraySegment<byte> p0)
		{
			string text = p0.Count.ToString("X", dahxy.ldled);
			int num = 0;
			if (num != 0)
			{
				goto IL_0023;
			}
			goto IL_003a;
			IL_0023:
			cbmyb.WriteByte((byte)text[num]);
			num++;
			goto IL_003a;
			IL_003a:
			if (num >= text.Length)
			{
				cbmyb.WriteByte(13);
				cbmyb.WriteByte(10);
				cbmyb.Write(p0.Array, p0.Offset, p0.Count);
				cbmyb.WriteByte(13);
				cbmyb.WriteByte(10);
				hzszk.uijme.kdbdu(cbmyb.GetBuffer(), 0, (int)cbmyb.Position);
				hzszk.hlyfg += p0.Count;
				if (hzszk.fihey != null && 0 == 0)
				{
					hzszk.fihey(hzszk.hlyfg);
				}
				cbmyb.Position = 0L;
				return;
			}
			goto IL_0023;
		}
	}

	private sealed class rprzy
	{
		public StreamWriter fanrw;

		public bool tbsuu;

		public bool gspqi;

		public void oefuq(string p0, string p1)
		{
			if (tbsuu && 0 == 0 && string.Equals(p0, "Expect", StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				fanrw.WriteLine("{0}: {1}", p0, p1);
				if (pjyrs.lyxka(p1, "100-continue", StringComparison.OrdinalIgnoreCase, ',') && 0 == 0)
				{
					tbsuu = false;
				}
			}
			else if (gspqi && 0 == 0 && string.Equals(p0, "Authorization", StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				fanrw.WriteLine("{0}: {1}", p0, "**********");
			}
			else
			{
				fanrw.WriteLine("{0}: {1}", p0, p1);
			}
		}
	}

	private const string yqlwr = "Rebex Web Client";

	private const int trvax = 16384;

	private static int osigk;

	private readonly int fpdhz;

	private readonly string iuvhg;

	private readonly string yoshi;

	private readonly int tdlak;

	private string desuq;

	private pjyrs efdfr;

	private Stream drphl;

	private bool dvzvw;

	private long? asxua;

	private Action<int> lhglv;

	private nymgm hzcfb;

	private bool zvnhh;

	private bool fvxic;

	private bool pbvqi;

	private bool gjmpp;

	private bool xlsya;

	private int pzppt;

	private int thura;

	private Encoding gcgyc;

	private readonly awngk uumle;

	private Func<bool> rmhie;

	private znuay pgail;

	private bool farto;

	private fklrq gszgq;

	private fklrq skyln;

	private bool qbygf;

	private bool ugmph;

	private bool toyot;

	private string nllih;

	private string esgpc;

	private DecompressionMethods txkyx;

	private string vmmpz;

	private int uaqro;

	private string qtkog;

	private static Func<bool> ayoag;

	private static Func<bool> ywjxj;

	protected awngk elpzg => uumle;

	public bool qxzph
	{
		get
		{
			if (pgail == null || 1 == 0)
			{
				return false;
			}
			return pgail.mpjbd;
		}
	}

	public znuay pqqdp => pgail;

	public bool byzzx
	{
		get
		{
			return farto;
		}
		set
		{
			farto = value;
		}
	}

	public int hxrxh => fpdhz;

	public fklrq zhaap
	{
		get
		{
			return gszgq;
		}
		set
		{
			gszgq = value;
		}
	}

	public fklrq jcwhm
	{
		get
		{
			return skyln;
		}
		private set
		{
			skyln = value;
		}
	}

	public bool igycn
	{
		get
		{
			return qbygf;
		}
		set
		{
			qbygf = value;
		}
	}

	public bool ykbvb
	{
		get
		{
			return ugmph;
		}
		set
		{
			ugmph = value;
		}
	}

	public bool btccd
	{
		get
		{
			return toyot;
		}
		set
		{
			toyot = value;
		}
	}

	public string uvhpp
	{
		get
		{
			return nllih;
		}
		set
		{
			nllih = value;
		}
	}

	public string zeibp
	{
		get
		{
			return esgpc;
		}
		set
		{
			esgpc = value;
		}
	}

	public string frvew
	{
		get
		{
			return efdfr["Content-Type"];
		}
		set
		{
			efdfr["Content-Type"] = value;
		}
	}

	public string qzlvp
	{
		get
		{
			return efdfr["Connection"];
		}
		set
		{
			efdfr["Connection"] = value;
		}
	}

	public DecompressionMethods xpfip
	{
		get
		{
			return txkyx;
		}
		set
		{
			txkyx = value;
		}
	}

	internal bool mtumb
	{
		get
		{
			string text = efdfr["Connection"];
			if (text == null || 1 == 0)
			{
				return true;
			}
			return string.Equals(text, "keep-alive", StringComparison.OrdinalIgnoreCase);
		}
	}

	public string tdnkk
	{
		get
		{
			return vmmpz;
		}
		set
		{
			vmmpz = value;
		}
	}

	public pjyrs isvcv
	{
		get
		{
			return efdfr;
		}
		set
		{
			efdfr = value;
		}
	}

	public bool ulbsw => zvnhh;

	public bool yeeln => fvxic;

	public int pjvho
	{
		get
		{
			return pzppt;
		}
		set
		{
			pzppt = value;
			if (hzcfb != null && 0 == 0)
			{
				hzcfb.Timeout = value;
			}
		}
	}

	public int kqxuj
	{
		get
		{
			return uaqro;
		}
		set
		{
			uaqro = value;
		}
	}

	public string hvcjz
	{
		get
		{
			return qtkog;
		}
		set
		{
			qtkog = value;
		}
	}

	private bool luxxj => iuvhg.Equals("https", StringComparison.OrdinalIgnoreCase);

	public string atnsu => yoshi;

	public int spmvf => tdlak;

	public string xbcyy => desuq;

	public void pezdy(ILogWriter p0)
	{
		poazj(p0, null);
	}

	public void poazj(ILogWriter p0, Func<bool> p1)
	{
		uumle.xxboi = p0;
		if (p1 == null || 1 == 0)
		{
			if (ayoag == null || 1 == 0)
			{
				ayoag = asohw;
			}
			rmhie = ayoag;
		}
		else
		{
			rmhie = p1;
		}
	}

	public void sesmx(znuay p0)
	{
		znuay obj = p0;
		if (obj == null || 1 == 0)
		{
			obj = new znuay();
		}
		pgail = obj;
	}

	internal void qruww(LogLevel p0, string p1, string p2, byte[] p3, int p4, int p5)
	{
		uumle.iyauk(p0, p1, p2, p3, p4, p5);
	}

	internal void iptfx(LogLevel p0, string p1, string p2, params object[] p3)
	{
		uumle.byfnx(p0, p1, p2, p3);
	}

	public udlmn(Uri baseUri)
	{
		fpdhz = Interlocked.Increment(ref osigk);
		uumle = new awngk(null, null);
		if (ywjxj == null || 1 == 0)
		{
			ywjxj = svlkm;
		}
		rmhie = ywjxj;
		iuvhg = baseUri.Scheme;
		yoshi = baseUri.Host;
		tdlak = baseUri.Port;
		efdfr = new pjyrs();
		tdnkk = "GET";
		hvcjz = "Rebex Web Client";
		kqxuj = 350;
		pzppt = 120000;
		gcgyc = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
	}

	protected nymgm lbevt()
	{
		nymgm nymgm2 = hzcfb;
		if (nymgm2 == null || 1 == 0)
		{
			throw new InvalidOperationException("Not connected.");
		}
		hzcfb = null;
		return nymgm2;
	}

	internal void maskh()
	{
		if (qxzph && 0 == 0)
		{
			throw new ujepc("Operation was canceled.", ezmya.ydksh);
		}
	}

	public void htvzd()
	{
		pgail.pvutk();
		nymgm nymgm2 = hzcfb;
		if (nymgm2 != null && 0 == 0)
		{
			try
			{
				iptfx(LogLevel.Debug, "HTTP", "HTTP request was canceled. Aborting HTTP session ({0}).", hxrxh);
				nymgm2.fvtwr();
			}
			catch (Exception ex)
			{
				iptfx(LogLevel.Info, "HTTP", "Error while closing socket of canceled HTTP session ({0}): {1}", hxrxh, ex);
			}
		}
	}

	public void Dispose()
	{
		owxvb();
	}

	private void owxvb()
	{
		nymgm nymgm2 = hzcfb;
		if (nymgm2 != null && 0 == 0)
		{
			try
			{
				iptfx(LogLevel.Debug, "HTTP", "Closing HTTP session ({0}).", hxrxh);
				nymgm2.fvtwr();
			}
			catch (Exception ex)
			{
				iptfx(LogLevel.Error, "HTTP", "Error while closing socket of HTTP session ({0}): {1}", hxrxh, ex);
			}
			hzcfb = null;
		}
	}

	public bool wugxx()
	{
		try
		{
			if (hzcfb == null || false || qxzph)
			{
				return false;
			}
			if (hzcfb.ooaym(p0: false) && 0 == 0)
			{
				return false;
			}
			if (hzcfb.swbbq && 0 == 0)
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			LogLevel p = ((ex is SocketException || ex is ObjectDisposedException) ? LogLevel.Debug : LogLevel.Error);
			iptfx(p, "HTTP", "Socket is no longer usable: {0}", ex);
			owxvb();
			return false;
		}
	}

	public void mzzkd(string p0)
	{
		abooq("GET", p0, null);
	}

	public void abooq(string p0, string p1, pjyrs p2)
	{
		if (desuq != null && 0 == 0)
		{
			throw new InvalidOperationException("A request has already been started.");
		}
		desuq = p1;
		tdnkk = p0;
		drphl = null;
		dvzvw = false;
		asxua = null;
		lhglv = null;
		if (p2 == null || 1 == 0)
		{
			efdfr.lenjz();
		}
		else
		{
			efdfr = p2;
		}
	}

	public void swlkd(Stream p0, bool p1, long? p2, Action<int> p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("requestStream");
		}
		if (drphl != null && 0 == 0)
		{
			throw new InvalidOperationException("A request stream has already been acquired.");
		}
		drphl = p0;
		dvzvw = p1;
		asxua = ((p2.HasValue && 0 == 0 && p2.Value >= 0) ? p2 : ((long?)null));
		lhglv = p3;
	}

	protected abstract phvuu dkczn(string p0, int p1, bool p2);

	public void nplxx()
	{
		bool flag = false;
		try
		{
			if (hzcfb != null && 0 == 0 && hzcfb.hznqz(0) && 0 == 0)
			{
				owxvb();
			}
			if (hzcfb == null || 1 == 0)
			{
				iptfx(LogLevel.Info, "HTTP", "{0}onnecting to '{1}://{2}:{3}'...", (zvnhh ? true : false) ? "Rec" : "C", iuvhg, yoshi, tdlak);
				if (!byzzx || 1 == 0)
				{
					LogWriterBase.knfwp(elpzg, kcrsy.fqzqd(), null, null, fpdhz, p5: true);
				}
				phvuu socket = dkczn(yoshi, tdlak, luxxj);
				hzcfb = new nymgm(socket, this);
				zvnhh = true;
				pbvqi = true;
				thura = 0;
			}
			flag = true;
		}
		finally
		{
			if (!flag || 1 == 0)
			{
				owxvb();
			}
		}
	}

	private void kdbdu(byte[] p0, int p1, int p2)
	{
		while (p2 > 0)
		{
			maskh();
			int num = hzcfb.Send(p0, p1, p2);
			if (!uumle.kdlxj || 1 == 0)
			{
				qruww(LogLevel.Verbose, "HTTP", "Raw data:", p0, p1, num);
			}
			p1 += num;
			p2 -= num;
		}
	}

	private void ktauh(opjbe p0, bool p1)
	{
		Action<byte[], int> action = null;
		Action<byte[], int> action2 = null;
		iptfx(LogLevel.Debug, "HTTP", "Sending request ({0} bytes).", p0.Length);
		bool flag = p1 && 0 == 0 && !rmhie();
		IDisposable disposable = uumle.cllqt(flag);
		try
		{
			if (action == null || 1 == 0)
			{
				action = bopde;
			}
			p0.bbmih(action);
		}
		finally
		{
			if (disposable != null && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		if (flag && 0 == 0 && elpzg.Level <= LogLevel.Verbose)
		{
			opjbe opjbe2 = new opjbe(16384);
			yrgqo(opjbe2, p1: true);
			if (action2 == null || 1 == 0)
			{
				action2 = rhwkn;
			}
			opjbe2.bbmih(action2);
		}
	}

	private void asfry()
	{
		Action<ArraySegment<byte>> action = null;
		ignir ignir = new ignir();
		ignir.uijme = this;
		if (xlsya)
		{
			return;
		}
		ignir.fihey = lhglv;
		opjbe opjbe2 = drphl as opjbe;
		object obj = opjbe2;
		if (obj == null || 1 == 0)
		{
			obj = drphl.tloyl(16384);
		}
		wqywh<byte> wqywh2 = (wqywh<byte>)obj;
		string text = ((asxua.HasValue ? true : false) ? brgjd.edcru(" {0} bytes of", asxua.Value) : "");
		string text2 = ((opjbe2 == null) ? " non-buffered" : "");
		string text3 = ((dvzvw ? true : false) ? " in chunked mode" : "");
		iptfx(LogLevel.Debug, "HTTP", "Sending{0}{1} data{2}.", text, text2, text3);
		ignir.hlyfg = 0;
		if (!dvzvw || 1 == 0)
		{
			if (action == null || 1 == 0)
			{
				action = ignir.jlanl;
			}
			wqywh2.vmdbf(action);
		}
		else
		{
			Action<ArraySegment<byte>> action2 = null;
			tmsmb tmsmb = new tmsmb();
			tmsmb.hzszk = ignir;
			tmsmb.cbmyb = new MemoryStream();
			try
			{
				if (action2 == null || 1 == 0)
				{
					action2 = tmsmb.okrsx;
				}
				wqywh2.vmdbf(action2);
				tmsmb.cbmyb.WriteByte(48);
				tmsmb.cbmyb.WriteByte(13);
				tmsmb.cbmyb.WriteByte(10);
				tmsmb.cbmyb.WriteByte(13);
				tmsmb.cbmyb.WriteByte(10);
				kdbdu(tmsmb.cbmyb.GetBuffer(), 0, (int)tmsmb.cbmyb.Position);
			}
			finally
			{
				if (tmsmb.cbmyb != null && 0 == 0)
				{
					((IDisposable)tmsmb.cbmyb).Dispose();
				}
			}
		}
		if (!(drphl is opjbe) || 1 == 0)
		{
			drphl = Stream.Null;
		}
		xlsya = true;
	}

	public virtual bool utzrt(Exception p0)
	{
		return p0 is SocketException;
	}

	private void iqjhb(bool p0)
	{
		nplxx();
		iptfx(LogLevel.Info, "HTTP", "Sending request: {0} {1}", tdnkk, desuq);
		iptfx(LogLevel.Debug, "HTTP", "PreAuthenticate: {0}{1}", ykbvb, (ykbvb && 0 == 0 && isvcv["Authorization"] == null) ? " (not pre-authenticated)" : "");
		opjbe p1 = howdo();
		bool flag = false;
		try
		{
			ktauh(p1, p0);
			flag = true;
		}
		catch (Exception ex)
		{
			iptfx(LogLevel.Debug, "HTTP", "Error while sending request: {0}", ex);
			if (!utzrt(ex) || 1 == 0)
			{
				throw;
			}
		}
		if (!flag || 1 == 0)
		{
			owxvb();
			nplxx();
			ktauh(p1, p0);
		}
		thura++;
	}

	private thths exuyr()
	{
		if (hzcfb == null || 1 == 0)
		{
			throw new InvalidOperationException("Not connected.");
		}
		thths thths2 = thths.nfbia(hzcfb, this);
		if (thths2 == null || 1 == 0)
		{
			iptfx(LogLevel.Info, "HTTP", "Connection lost.");
			owxvb();
		}
		return thths2;
	}

	public thths kvanb()
	{
		if (desuq == null || 1 == 0)
		{
			throw new InvalidOperationException("No request has been started.");
		}
		bool flag = false;
		try
		{
			thths thths2 = null;
			SspiAuthentication p = null;
			bool p2 = false;
			string p3 = null;
			string p4 = null;
			if (igycn && 0 == 0)
			{
				if (fvxic && 0 == 0 && (zhaap & jcwhm) != fklrq.uzjbr && 0 == 0)
				{
					p3 = hplul(null, p1: false, out p, out p2);
					pbvqi = true;
				}
			}
			else if (ykbvb && 0 == 0 && wfupm(out p3, out p4) && 0 == 0 && (p4 != null || p3 == "Basic"))
			{
				jcwhm = pjyrs.motgl(p3);
				p3 = hplul(p4, p1: true, out p, out p2);
				pbvqi = true;
			}
			if (drphl != null && 0 == 0 && (drphl is opjbe || ((!dvzvw || 1 == 0) && (!asxua.HasValue || 1 == 0))))
			{
				asxua = drphl.Length;
			}
			int num = 0;
			while (true)
			{
				if (num >= 3 || (num > 0 && thths2 == null && xlsya))
				{
					throw new ujepc("Connection closed.", ezmya.bbuiy);
				}
				if (drphl == Stream.Null)
				{
					throw new ujepc("Unable to send request content again when stream buffering is disabled.", ezmya.ydksh);
				}
				iqjhb(p3 == "Basic");
				if (drphl != null && 0 == 0 && (!gjmpp || false || ((!hzcfb.xiadn(kqxuj) || 1 == 0) && (!hzcfb.swbbq || 1 == 0))))
				{
					asfry();
				}
				thths2 = exuyr();
				if (thths2 == null || 1 == 0)
				{
					num++;
					continue;
				}
				if (thths2.xgkmt == HttpStatusCode.SwitchingProtocols)
				{
					flag = true;
					return thths2;
				}
				if (thths2.yddmu == pyuak.ahxes)
				{
					if (gjmpp && 0 == 0)
					{
						asfry();
					}
					thths2 = exuyr();
					if (thths2 == null || 1 == 0)
					{
						num++;
						continue;
					}
					if (gjmpp && 0 == 0 && thths2.xgkmt == HttpStatusCode.Continue)
					{
						thths2 = exuyr();
						if (thths2 == null || 1 == 0)
						{
							num++;
							continue;
						}
						if (thths2.xgkmt == HttpStatusCode.Continue)
						{
							throw qqrys("Too many '100 Continue' responses.", ezmya.zmpaw, null);
						}
					}
				}
				bool flag2 = false;
				if (!thths2.qvnsr || 1 == 0)
				{
					lbevt();
					flag2 = true;
				}
				if (thths2.yddmu == pyuak.ynagd || thths2.yddmu == pyuak.yvdvx)
				{
					if (p3 != null && 0 == 0)
					{
						if (ykbvb && 0 == 0 && (p3 == "Basic" || p3 == "Digest"))
						{
							mhhgl(p3, p4);
						}
						fvxic = true;
						iptfx(LogLevel.Info, "HTTP", "Successfully authenticated using '{0}'.", p3);
					}
					break;
				}
				if (thths2.xgkmt == HttpStatusCode.ExpectationFailed || thths2.xgkmt != HttpStatusCode.Unauthorized)
				{
					break;
				}
				if (gjmpp && 0 == 0 && (!flag2 || 1 == 0))
				{
					asfry();
				}
				pbvqi = true;
				if (!xvdzq(thths2, ref p3, ref p, ref p2, out p4))
				{
					break;
				}
				thths2.jpxci();
			}
			flag = true;
			return thths2;
		}
		catch (ujepc p5)
		{
			rvhyr("Error while sending request", p5);
			throw;
		}
		catch (Exception ex)
		{
			rvhyr("Error while sending request", ex);
			throw qqrys("Error while processing request.", ezmya.mmsdg, ex);
		}
		finally
		{
			desuq = null;
			if ((!flag || 1 == 0) && hzcfb != null && 0 == 0)
			{
				owxvb();
			}
		}
	}

	private bool xvdzq(thths p0, ref string p1, ref SspiAuthentication p2, ref bool p3, out string p4)
	{
		p4 = null;
		IList<string> list = p0.virwn.llppa("WWW-Authenticate");
		IEnumerator<string> enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				string current = enumerator.Current;
				iptfx(LogLevel.Debug, "HTTP", "Server requires authentication: {0}", current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (p1 == null || 1 == 0)
		{
			jcwhm = p0.irbum;
			if (zhaap == fklrq.uzjbr || 1 == 0)
			{
				return false;
			}
			awhlf(p0);
			string text = "Digest ";
			IEnumerator<string> enumerator2 = list.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext() ? true : false)
				{
					string current2 = enumerator2.Current;
					if (current2.StartsWith(text, StringComparison.OrdinalIgnoreCase) && 0 == 0)
					{
						p4 = current2.Substring(text.Length);
						break;
					}
				}
			}
			finally
			{
				if (enumerator2 != null && 0 == 0)
				{
					enumerator2.Dispose();
				}
			}
			p1 = hplul(p4, p1: false, out p2, out p3);
			return true;
		}
		if (p2 != null && 0 == 0 && p1 != "Basic" && 0 == 0 && p1 != "Digest" && 0 == 0)
		{
			if (p3 && 0 == 0)
			{
				return false;
			}
			IEnumerator<string> enumerator3 = list.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext() ? true : false)
				{
					string current3 = enumerator3.Current;
					int num = current3.IndexOf(p1 + " ", StringComparison.OrdinalIgnoreCase);
					if (num >= 0)
					{
						num += p1.Length + 1;
						int num2 = current3.IndexOf(',', num);
						if (num2 < 0)
						{
							num2 = current3.Length;
						}
						p4 = current3.Substring(num, num2 - num);
						iptfx(LogLevel.Debug, "HTTP", "Authentication challenge:\n\t{0}", p4);
						byte[] challenge = Convert.FromBase64String(p4);
						byte[] nextMessage = p2.GetNextMessage(challenge, out p3);
						awhlf(p0);
						bmmnw(p1, nextMessage);
						return true;
					}
				}
			}
			finally
			{
				if (enumerator3 != null && 0 == 0)
				{
					enumerator3.Dispose();
				}
			}
		}
		return false;
	}

	protected virtual void mhhgl(string p0, string p1)
	{
	}

	protected virtual bool wfupm(out string p0, out string p1)
	{
		p0 = null;
		p1 = null;
		return false;
	}

	internal void rvhyr(string p0, Exception p1)
	{
		if (qxzph && 0 == 0)
		{
			if (cgsid(p1) && 0 == 0)
			{
				iptfx(LogLevel.Info, "HTTP", "Request was canceled.", p0);
			}
			else
			{
				iptfx(LogLevel.Info, "HTTP", "{0}: Request was canceled: {1}", p0, p1);
			}
		}
		else
		{
			iptfx(LogLevel.Error, "HTTP", "{0}: {1}", p0, p1);
		}
	}

	private static bool cgsid(Exception p0)
	{
		if (p0 != null && 0 == 0 && (!(p0 is ObjectDisposedException) || 1 == 0))
		{
			ujepc ujepc2 = p0 as ujepc;
			ujepc ujepc3 = p0.InnerException as ujepc;
			if (ujepc2 == null || false || ujepc2.zhmeu != ezmya.ydksh)
			{
				if (ujepc3 != null && 0 == 0)
				{
					return ujepc3.zhmeu == ezmya.ydksh;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	protected virtual ujepc qqrys(string p0, ezmya p1, Exception p2)
	{
		return new ujepc(p0, p1, p2);
	}

	public void awhlf(thths p0)
	{
		if (mtumb && 0 == 0)
		{
			iptfx(LogLevel.Debug, "HTTP", "Discarding pending response data...");
			p0.fract();
		}
	}

	private void bmmnw(string p0, byte[] p1)
	{
		string text = p0 + " " + Convert.ToBase64String(p1);
		if (p0 != "Basic" && 0 == 0)
		{
			iptfx(LogLevel.Debug, "HTTP", "Authentication message:\n\t{0}", text);
		}
		efdfr["Authorization"] = text;
	}

	private string hplul(string p0, bool p1, out SspiAuthentication p2, out bool p3)
	{
		fklrq fklrq2 = jcwhm & zhaap;
		string text = (((fklrq2 & fklrq.sjjmf) != fklrq.uzjbr && 0 == 0 && SspiAuthentication.IsSupported("Negotiate")) ? "Negotiate" : (((fklrq2 & fklrq.cepgj) != fklrq.uzjbr && 0 == 0 && SspiAuthentication.IsSupported("NTLM")) ? "NTLM" : (((fklrq2 & fklrq.cznae) == 0 || false || !SspiAuthentication.IsSupported("Kerberos")) ? null : "Kerberos")));
		string text2 = uvhpp;
		string text3 = zeibp;
		if (string.IsNullOrEmpty(text2) && 0 == 0)
		{
			text2 = null;
		}
		if (text3 == null || 1 == 0)
		{
			text3 = "";
		}
		if (text != null && 0 == 0)
		{
			iptfx(LogLevel.Info, "HTTP", "Attempting {1}'{0}' authentication.", text, (text2 != null) ? "" : "single sign-on ");
			p2 = new SspiAuthentication(text, SspiDataRepresentation.Native, yoshi, (SspiRequirements)0, text2, text3, null);
			byte[] nextMessage = p2.GetNextMessage(null, out p3);
			bmmnw(text, nextMessage);
		}
		else if (p0 != null && 0 == 0 && (fklrq2 & fklrq.rafbm) != fklrq.uzjbr)
		{
			if (text2 == null || 1 == 0)
			{
				throw new ujepc("Server requires authentication, but credentials are needed for 'Digest' authentication.", ezmya.ydksh);
			}
			p2 = null;
			p3 = true;
			diwlk diwlk2 = new diwlk(p0, iuvhg, base64: false);
			string text4 = diwlk2.vhedm(uvhpp, zeibp, tdnkk, desuq, "uri", p5: true);
			text = "Digest";
			iptfx(LogLevel.Info, "HTTP", "Attempting 'Digest' {0}authentication.", (p1 ? true : false) ? "" : "pre-");
			efdfr["Authorization"] = "Digest " + text4;
		}
		else
		{
			if ((fklrq2 & fklrq.okxlf) == 0)
			{
				throw new ujepc("Server requires authentication, but none of the authentication methods is supported or allowed.", ezmya.ydksh);
			}
			if (text2 == null || 1 == 0)
			{
				throw new ujepc("Server requires authentication, but credentials are needed for 'Basic' authentication.", ezmya.ydksh);
			}
			p2 = null;
			p3 = true;
			byte[] bytes = Encoding.UTF8.GetBytes(text2 + ":" + text3);
			text = "Basic";
			iptfx(LogLevel.Info, "HTTP", "Attempting 'Basic' {0}authentication.", (p1 ? true : false) ? "" : "pre-");
			bmmnw(text, bytes);
		}
		return text;
	}

	private opjbe howdo()
	{
		gjmpp = false;
		xlsya = false;
		if (drphl != null && 0 == 0)
		{
			if (dvzvw && 0 == 0)
			{
				efdfr["Transfer-Encoding"] = "chunked";
			}
			else
			{
				efdfr["Transfer-Encoding"] = null;
			}
			if (asxua.HasValue && 0 == 0)
			{
				efdfr["Content-Length"] = asxua.ToString();
			}
			if (pbvqi && 0 == 0 && btccd && 0 == 0 && ((dvzvw ? true : false) || (asxua ?? 0) != 0))
			{
				gjmpp = true;
			}
		}
		string p = ((((luxxj ? true : false) ? (tdlak == 443) : (tdlak == 80)) ? true : false) ? yoshi : brgjd.edcru("{0}:{1}", yoshi, tdlak));
		efdfr.uqbkm(p);
		if ((efdfr["Accept-Encoding"] == null || 1 == 0) && xpfip != DecompressionMethods.None && 0 == 0)
		{
			efdfr["Accept-Encoding"] = xpfip.nvpnh();
		}
		if ((efdfr["User-Agent"] == null || 1 == 0) && hvcjz != null && 0 == 0)
		{
			efdfr["User-Agent"] = hvcjz;
		}
		if (efdfr["Connection"] == null || 1 == 0)
		{
			efdfr["Connection"] = "keep-alive";
		}
		string text = efdfr["Connection"];
		if (text == null || 1 == 0)
		{
			iptfx(LogLevel.Debug, "HTTP", "Request Connection header not specified.");
		}
		else
		{
			iptfx(LogLevel.Debug, "HTTP", "Request Connection: {0}.", text);
		}
		opjbe opjbe2 = new opjbe(16384);
		yrgqo(opjbe2, p1: false);
		return opjbe2;
	}

	private void yrgqo(opjbe p0, bool p1)
	{
		rprzy rprzy = new rprzy();
		rprzy.gspqi = p1;
		rprzy.fanrw = new StreamWriter(p0, gcgyc);
		rprzy.fanrw.NewLine = "\r\n";
		rprzy.fanrw.AutoFlush = false;
		rprzy.fanrw.WriteLine("{0} {1} {2}", new object[3] { tdnkk, desuq, "HTTP/1.1" });
		rprzy.tbsuu = gjmpp;
		efdfr.dxkfp(rprzy.oefuq);
		if (rprzy.tbsuu && 0 == 0)
		{
			rprzy.fanrw.WriteLine("Expect: 100-continue");
		}
		rprzy.fanrw.WriteLine();
		rprzy.fanrw.Flush();
	}

	private static bool asohw()
	{
		return false;
	}

	private static bool svlkm()
	{
		return false;
	}

	private void bopde(byte[] p0, int p1)
	{
		kdbdu(p0, 0, p1);
	}

	private void rhwkn(byte[] p0, int p1)
	{
		qruww(LogLevel.Verbose, "HTTP", "Raw data:", p0, 0, p1);
	}
}
