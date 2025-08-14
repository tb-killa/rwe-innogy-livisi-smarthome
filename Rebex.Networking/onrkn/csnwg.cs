using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal abstract class csnwg
{
	public const int bumsr = 16384;

	public const int oonzi = 17408;

	public const int hrtds = 18432;

	public const int wjtrx = 18437;

	private TlsSocket mfwtc;

	private ISocket gexoo;

	private vffxo fbdmk;

	private volatile hlkgm nntep;

	private TlsParameters xrhdm;

	private bpnki cmmje = bpnki.yiqfh;

	private bpnki aidaa = bpnki.yiqfh;

	private bpnki nmvyd;

	private bpnki nxbzq;

	private readonly nmhgd nyghu = new nmhgd();

	private int rmfpo = int.MaxValue;

	private int kljuj = -1;

	private readonly byte[] estfp = new byte[18437];

	private readonly byte[] gesma = new byte[18437];

	private int khljj;

	private readonly nmhgd mjwpv = new nmhgd();

	private bool xbvxi;

	private bool qeooa;

	private bool dmoyp;

	private bool vzxjd;

	private readonly byte[] vvcvd = new byte[18437];

	private byte[] mekls = new byte[18437];

	private MemoryStream xvysq = new MemoryStream();

	protected bool liroy;

	private readonly object tlatd = new object();

	protected int lwwzy;

	private bool vqqqm;

	private ILogWriter hrivt;

	private readonly object zjefm = new object();

	private readonly object bluvz = new object();

	private readonly object fjdwc = new object();

	public ILogWriter sfzgr
	{
		get
		{
			return hrivt;
		}
		set
		{
			hrivt = value;
		}
	}

	public ISocket vaapf => gexoo;

	public vffxo elbgc
	{
		get
		{
			return fbdmk;
		}
		set
		{
			fbdmk = value;
		}
	}

	public TlsParameters knmxu => xrhdm;

	public hlkgm mgfog => nntep;

	public TlsProtocol gzlwv => (TlsProtocol)lwwzy;

	public bool cfvix
	{
		get
		{
			lock (tlatd)
			{
				return nntep != hlkgm.tqqib || mjwpv.Length > 0;
			}
		}
	}

	internal bpnki pmfcd
	{
		get
		{
			return nmvyd;
		}
		set
		{
			nmvyd = value;
		}
	}

	internal bpnki ihtfw
	{
		get
		{
			return nxbzq;
		}
		set
		{
			nxbzq = value;
		}
	}

	internal bpnki xsiiu => cmmje;

	internal bpnki ytkdw => aidaa;

	internal bool dgctd => khljj > 0;

	public int nvoco => (int)mjwpv.Length;

	protected void otbhg(LogLevel p0, string p1, byte[] p2, int p3, int p4)
	{
		ILogWriter logWriter = hrivt;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), gexoo.GetHashCode(), "TLS", p1, p2, p3, p4);
		}
	}

	protected void kxjds(LogLevel p0, string p1, params object[] p2)
	{
		ILogWriter logWriter = hrivt;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), gexoo.GetHashCode(), "TLS", brgjd.edcru(p1, p2));
		}
	}

	protected void lvwig(LogLevel p0, string p1)
	{
		ILogWriter logWriter = hrivt;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), gexoo.GetHashCode(), "TLS", p1);
		}
	}

	protected void wgllv(LogLevel p0, Func<string> p1)
	{
		ILogWriter logWriter = hrivt;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), gexoo.GetHashCode(), "TLS", p1());
		}
	}

	protected bool gvlry()
	{
		lock (tlatd)
		{
			if (nntep == hlkgm.iucmn)
			{
				return false;
			}
			nntep = hlkgm.iucmn;
		}
		return true;
	}

	protected csnwg(TlsSocket owner, ISocket socket, TlsParameters parameters)
	{
		mfwtc = owner;
		gexoo = socket;
		xrhdm = parameters;
		lwwzy = 768;
	}

	protected abstract void doutw(byte[] p0, int p1, int p2);

	protected abstract void tcbzt(byte[] p0, int p1, int p2);

	protected abstract void kaqny();

	private void tkusd(byte[] p0, int p1, int p2)
	{
		if (nmvyd == null || 1 == 0)
		{
			throw new TlsException(mjddr.ypibb, brgjd.edcru("Received unexpected {0}.", "ChangeCipherSpec"));
		}
		if (p2 != 1)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ChangeCipherSpec"));
		}
		if (p0[p1] != 1)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ChangeCipherSpec"));
		}
		bpnki bpnki2 = cmmje;
		cmmje = nmvyd;
		nmvyd = null;
		if (bpnki2 != null && 0 == 0)
		{
			bpnki2.egphd();
		}
		mfwtc.kxlvu(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
	}

	private void remfg(byte[] p0, int p1, int p2)
	{
		doutw(p0, p1, p2);
	}

	private void fuxts(byte[] p0, int p1, int p2)
	{
		nyghu.Write(p0, p1, p2);
		while (nyghu.Length >= 4)
		{
			if (rmfpo == int.MaxValue)
			{
				rmfpo = 4 + nyghu[1] * 65536 + nyghu[2] * 256 + nyghu[3];
			}
			if (nyghu.Length >= rmfpo)
			{
				doutw(nyghu.GetBuffer(), 0, rmfpo);
				nyghu.ejbiu(rmfpo);
				rmfpo = int.MaxValue;
				continue;
			}
			break;
		}
	}

	private void bshau(byte[] p0, int p1, int p2)
	{
		if (p2 == 0 || 1 == 0)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "Alert"));
		}
		if (kljuj != -1)
		{
			tcbzt(new byte[2]
			{
				(byte)kljuj,
				p0[p1]
			}, 0, 2);
			kljuj = -1;
			p1++;
			p2--;
		}
		while (p2 > 1)
		{
			tcbzt(p0, p1, 2);
			p1 += 2;
			p2 -= 2;
		}
		if (p2 == 1)
		{
			kljuj = p0[p1];
		}
	}

	private void rwufm(byte[] p0, int p1, int p2)
	{
		lock (tlatd)
		{
			if (cmmje == bpnki.yiqfh)
			{
				throw new TlsException(mjddr.ypibb, "Received unencrypted application data.");
			}
			mjwpv.Write(p0, p1, p2);
		}
	}

	private bool xqszt(int p0)
	{
		if (khljj >= p0)
		{
			return true;
		}
		if (nntep == hlkgm.tqqib)
		{
			otbhg(LogLevel.Debug, "Incomplete TLS packet received before closed connection:", gesma, 0, khljj);
			khljj = 0;
			return false;
		}
		int aeqtf = fbdmk.aeqtf;
		int num = ((aeqtf > 0) ? Environment.TickCount : 0);
		while (true)
		{
			if (!gexoo.Poll(1000000, SocketSelectMode.SelectRead) || 1 == 0)
			{
				if (aeqtf > 0)
				{
					int num2 = Environment.TickCount - num;
					if (num2 > aeqtf)
					{
						throw new TlsException("The operation was not completed within the specified time limit.", NetworkSessionExceptionStatus.Timeout);
					}
				}
				continue;
			}
			int num3;
			try
			{
				num3 = ((gexoo.Connected && 0 == 0) ? gexoo.Receive(gesma, khljj, 18437 - khljj, SocketFlags.None) : 0);
			}
			catch (SocketException)
			{
				kxjds(LogLevel.Debug, "TLS socket error, {0} bytes of data were received.", khljj);
				throw;
			}
			if (num3 == 0 || 1 == 0)
			{
				kxjds(LogLevel.Debug, "TLS socket was closed, {0} bytes of data were received.", khljj);
				cmthd();
				txmko();
				return false;
			}
			khljj += num3;
			if (khljj >= p0)
			{
				break;
			}
		}
		return true;
	}

	private int hgsjn()
	{
		if (!xqszt(5) || 1 == 0)
		{
			return 0;
		}
		int num = 0;
		int num2 = (gesma[1] << 8) + gesma[2];
		if (num2 < 256 || num2 >= 1024)
		{
			if (nntep == hlkgm.iucmn)
			{
				num = ((gesma[0] & 0x3F) << 8) + gesma[1];
				if (num > 0)
				{
					byte b = ((((gesma[0] & 0x80) != 0) ? true : false) ? gesma[2] : gesma[3]);
					if (b == 1)
					{
						if ((gesma[0] & 0x80) == 0 || 1 == 0)
						{
							gesma[0] = b;
							gesma[1] = gesma[4];
							Array.Copy(gesma, 5, gesma, 2, khljj - 5);
							khljj -= 3;
						}
						else
						{
							gesma[0] = b;
							gesma[1] = gesma[3];
							gesma[2] = gesma[4];
							Array.Copy(gesma, 5, gesma, 3, khljj - 5);
							khljj -= 2;
						}
					}
					else
					{
						num = 0;
					}
				}
			}
			if (num == 0 || 1 == 0)
			{
				otbhg(LogLevel.Debug, "Invalid TLS packet received:", gesma, 0, khljj);
				string value = EncodingTools.ASCII.GetString(gesma, 0, 4);
				TlsException ex = new TlsException(mjddr.puqjh);
				ex.Data["TlsData"] = value;
				throw ex;
			}
		}
		else
		{
			num = (gesma[3] << 8) + gesma[4] + 5;
		}
		if (num > khljj && (!xqszt(num) || 1 == 0))
		{
			throw new TlsException("Connection was closed by the remote connection end.", NetworkSessionExceptionStatus.ConnectionClosed);
		}
		lock (tlatd)
		{
			if (lwwzy != num2)
			{
				if (nntep != hlkgm.iucmn)
				{
					throw new TlsException(mjddr.puqjh);
				}
				if (num2 < 768)
				{
					byte[] array = new byte[5] { 128, 3, 0, 0, 1 };
					gexoo.Send(array, 0, array.Length, SocketFlags.None);
				}
				lwwzy = num2;
			}
		}
		Array.Copy(gesma, 0, estfp, 0, num);
		int result = cmmje.bvfhg(estfp, num);
		khljj -= num;
		Array.Copy(gesma, num, gesma, 0, khljj);
		return result;
	}

	internal void upurk()
	{
		lock (tlatd)
		{
			if (dmoyp && 0 == 0)
			{
				Thread.Sleep(0);
				return;
			}
			dmoyp = true;
		}
		try
		{
			if (cmmje == null)
			{
				return;
			}
			int num = hgsjn();
			if (num == 0 || 1 == 0)
			{
				return;
			}
			otbhg(LogLevel.Verbose, "Received TLS packet: ", estfp, 0, num);
			if (num > 5)
			{
				switch (estfp[0])
				{
				case 1:
					remfg(estfp, 0, num);
					break;
				case 22:
					fuxts(estfp, 5, num - 5);
					break;
				case 20:
					tkusd(estfp, 5, num - 5);
					break;
				case 21:
					bshau(estfp, 5, num - 5);
					break;
				case 23:
					rwufm(estfp, 5, num - 5);
					break;
				default:
					mfwtc.kxlvu(TlsDebugEventType.UnknownMessageType, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, estfp, 5, num - 5);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			throw iuwqw(ex, (ex is TlsException) ? null : "Error while processing TLS packet");
		}
		finally
		{
			lock (tlatd)
			{
				dmoyp = false;
			}
		}
	}

	protected void jwkmn(vcedo p0, byte[] p1, int p2, int p3)
	{
		while (p3 > 16384)
		{
			jwkmn(p0, p1, p2, 16384);
			p2 += 16384;
			p3 -= 16384;
		}
		lock (zjefm)
		{
			Array.Copy(p1, p2, vvcvd, 5, p3);
			vvcvd[0] = (byte)p0;
			vvcvd[1] = (byte)(lwwzy >> 8);
			vvcvd[2] = (byte)(lwwzy & 0xFF);
			vvcvd[3] = (byte)(p3 >> 8);
			vvcvd[4] = (byte)(p3 & 0xFF);
			if (p3 > 0 && liroy && 0 == 0 && p0 == vcedo.idfcl)
			{
				mekls[0] = 23;
				mekls[1] = vvcvd[1];
				mekls[2] = vvcvd[2];
				mekls[3] = 0;
				mekls[4] = 0;
				otbhg(LogLevel.Verbose, "Sent TLS packet: ", mekls, 0, 5);
				int count = aidaa.bvfhg(mekls, 5);
				xvysq.Write(mekls, 0, count);
			}
			otbhg(LogLevel.Verbose, "Sent TLS packet: ", vvcvd, 0, p3 + 5);
			p3 = aidaa.bvfhg(vvcvd, p3 + 5);
			if (p0 == vcedo.dwbji)
			{
				bpnki bpnki2 = aidaa;
				aidaa = nxbzq;
				nxbzq = null;
				if (bpnki2 != null && 0 == 0)
				{
					bpnki2.egphd();
				}
			}
			xvysq.Write(vvcvd, 0, p3);
			lock (tlatd)
			{
				qeooa = true;
			}
		}
	}

	internal void vituh()
	{
		lock (zjefm)
		{
			if (xvysq.Length == 0)
			{
				return;
			}
			try
			{
				gexoo.Send(xvysq.GetBuffer(), 0, (int)xvysq.Length, SocketFlags.None);
			}
			finally
			{
				xvysq.SetLength(0L);
				lock (tlatd)
				{
					qeooa = false;
				}
			}
		}
	}

	internal void otnhu()
	{
		lock (tlatd)
		{
			if (nntep == hlkgm.iucmn)
			{
				nntep = hlkgm.rhxxi;
			}
			xbvxi = false;
		}
		mfwtc.ivtmn(TlsDebugEventType.Secured, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
	}

	protected void cdaod(byte p0, byte p1, bool p2)
	{
		zppmb zppmb2 = new zppmb(p0, p1);
		byte[] array = zppmb2.szrqi();
		bool flag = p0 == 2 || p1 == 0;
		if (vzxjd && 0 == 0)
		{
			if (!p2 || 1 == 0)
			{
				lock (tlatd)
				{
					nntep = hlkgm.nzcmy;
					return;
				}
			}
			flag = false;
		}
		kxjds((zppmb2.bgpwy ? true : false) ? LogLevel.Info : LogLevel.Debug, "{0} was sent.", zppmb2);
		mfwtc.kxlvu(TlsDebugEventType.Alert, TlsDebugEventSource.Sent, TlsDebugLevel.Important, array, 0, array.Length);
		if (flag && 0 == 0)
		{
			txmko();
		}
		try
		{
			jwkmn(zppmb2.qeloj, array, 0, array.Length);
			vituh();
			if (flag && 0 == 0)
			{
				lock (tlatd)
				{
					nntep = hlkgm.tqqib;
					gexoo.Shutdown(SocketShutdown.Send);
				}
				if (!p2 || 1 == 0)
				{
					cmthd();
				}
			}
		}
		catch (Exception ex)
		{
			cmthd();
			if ((ex is SocketException || ex is ObjectDisposedException) && flag && 0 == 0)
			{
				return;
			}
			throw iuwqw(ex, "Error while sending alert packet");
		}
	}

	public void hzfdo(byte[] p0, int p1, int p2)
	{
		lock (bluvz)
		{
			bool flag;
			lock (tlatd)
			{
				if (nntep == hlkgm.nzcmy || 1 == 0)
				{
					throw new TlsException("Socket has not been secured yet.");
				}
				if (nntep == hlkgm.tqqib)
				{
					throw new InvalidOperationException("Socket was closed.");
				}
				flag = xbvxi;
			}
			while (flag ? true : false)
			{
				Thread.Sleep(1);
				lock (tlatd)
				{
					flag = xbvxi;
				}
			}
			jwkmn(vcedo.idfcl, p0, p1, p2);
			try
			{
				vituh();
			}
			catch (Exception p3)
			{
				throw iuwqw(p3, "Error while sending data over TLS");
			}
		}
	}

	public int ozzpy(byte[] p0, int p1, int p2, bool p3)
	{
		lock (fjdwc)
		{
			lock (tlatd)
			{
				if (nntep == hlkgm.nzcmy || 1 == 0)
				{
					throw new TlsException("Socket has not been secured yet.");
				}
				if (mjwpv.Length == 0 && nntep == hlkgm.tqqib)
				{
					return 0;
				}
			}
			try
			{
				do
				{
					lock (tlatd)
					{
						if (mjwpv.Length > 0)
						{
							break;
						}
						if (nntep == hlkgm.tqqib)
						{
							return 0;
						}
						goto IL_009c;
					}
					IL_009c:
					upurk();
				}
				while (!p3);
				int num = Math.Min(p2, (int)mjwpv.Length);
				Array.Copy(mjwpv.GetBuffer(), 0, p0, p1, num);
				mjwpv.ejbiu(num);
				return num;
			}
			finally
			{
				lock (tlatd)
				{
					dmoyp = false;
				}
			}
		}
	}

	public void sksvo()
	{
		lock (tlatd)
		{
			if (xbvxi && 0 == 0)
			{
				return;
			}
			xbvxi = true;
		}
		if ((knmxu.Version & TlsVersion.SSL30) != TlsVersion.None && 0 == 0)
		{
			lvwig(LogLevel.Info, "Warning: SSL 3.0 has been deprecated. According to RFC 7568, it must no longer be used.");
		}
		kaqny();
		while (xbvxi ? true : false)
		{
			upurk();
			if (nntep == hlkgm.tqqib)
			{
				if ((knmxu.Options & TlsOptions.AllowCloseWhileNegotiating) != TlsOptions.None && 0 == 0)
				{
					xbvxi = false;
					break;
				}
				lvwig(LogLevel.Debug, "TLS socket closed while negotiation was in progress.");
				string text = "Connection was closed by the remote connection end.";
				if (knmxu.Version == TlsVersion.SSL30)
				{
					text += " Try enabling TLS 1.2, 1.1 or 1.0 instead of legacy SSL 3.0.";
				}
				throw new TlsException(text, NetworkSessionExceptionStatus.ConnectionClosed);
			}
		}
	}

	public void jumxv()
	{
		lock (tlatd)
		{
			if ((xbvxi ? true : false) || vzxjd)
			{
				throw new TlsException("Silent unprotect is not possible at the moment, because sending or receiving is in progress.");
			}
			if (nntep != hlkgm.rhxxi)
			{
				return;
			}
			vzxjd = true;
		}
		if ((knmxu.Options & TlsOptions.SilentUnprotect) != TlsOptions.None && 0 == 0)
		{
			if ((dmoyp ? true : false) || qeooa)
			{
				throw new TlsException("Silent unprotect is not possible at the moment, because sending or receiving is in progress.");
			}
			lock (tlatd)
			{
				nntep = hlkgm.tqqib;
				return;
			}
		}
		cdaod(1, 0, p2: true);
		while (nntep == hlkgm.rhxxi)
		{
			upurk();
		}
		if (nntep != hlkgm.tqqib)
		{
			return;
		}
		throw new TlsException("Connection was closed by the remote connection end.", NetworkSessionExceptionStatus.ConnectionClosed);
	}

	private void txmko()
	{
		if (!vqqqm || 1 == 0)
		{
			vqqqm = true;
			mfwtc.ivtmn(TlsDebugEventType.Closed, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
		}
	}

	private Exception iuwqw(Exception p0, string p1)
	{
		if (p1 != null && 0 == 0)
		{
			kxjds(LogLevel.Debug, p1 + ": {0}", p0);
		}
		else
		{
			lvwig(LogLevel.Debug, p0.ToString());
		}
		bool flag = true;
		TlsException ex = p0 as TlsException;
		int num = ((p0 is SocketException p2 && 0 == 0) ? p2.skehp() : 0);
		if (num == 10038 || num == 10053 || num == 10054)
		{
			ex = new TlsException("Connection was closed by the remote connection end.", NetworkSessionExceptionStatus.ConnectionClosed, p0);
			flag = false;
			p0 = ex;
		}
		if (nntep != hlkgm.tqqib)
		{
			lock (tlatd)
			{
				nntep = hlkgm.tqqib;
			}
			if ((ex != null && 0 == 0 && (ex.dipvo ? true : false)) || todgf.hudgy(p0))
			{
				cmthd();
			}
			else
			{
				byte p3 = (byte)((ex == null || false || ex.rkbch == null) ? 80 : ex.rkbch.jmwmm);
				try
				{
					cdaod(2, p3, p2: true);
				}
				catch
				{
				}
			}
		}
		txmko();
		if (ex != null && 0 == 0)
		{
			if (flag && 0 == 0)
			{
				return TlsException.yxoes(ex);
			}
			return ex;
		}
		return new TlsException(rtzwv.iogyt, mjddr.qssln, p0.Message, p0);
	}

	private void cmthd()
	{
		lock (tlatd)
		{
			nntep = hlkgm.tqqib;
			try
			{
				gexoo.Close();
				if (cmmje != null && 0 == 0)
				{
					cmmje.egphd();
					cmmje = null;
				}
				if (aidaa != null && 0 == 0)
				{
					aidaa.egphd();
					aidaa = null;
				}
				if (nmvyd != null && 0 == 0)
				{
					nmvyd.egphd();
					nmvyd = null;
				}
				if (nxbzq != null && 0 == 0)
				{
					nxbzq.egphd();
					nxbzq = null;
				}
			}
			catch
			{
			}
		}
	}

	public void bmpty()
	{
		lvwig(LogLevel.Debug, "Closing TLS socket.");
		lock (tlatd)
		{
			if (nntep == hlkgm.tqqib)
			{
				cmthd();
				return;
			}
			nntep = hlkgm.tqqib;
		}
		try
		{
			if (nntep != hlkgm.nzcmy && 0 == 0 && ((knmxu.Options & TlsOptions.SilentClose) == 0 || 1 == 0))
			{
				cdaod(1, 0, p2: true);
			}
		}
		catch
		{
		}
		finally
		{
			cmthd();
		}
	}
}
