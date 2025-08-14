using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Rebex.Security.Authentication;
using onrkn;

namespace Rebex.Net;

public class SshSession : NetworkSession, kehni, IDisposable
{
	internal delegate bool nivbm<T0, T1>(out T0 result, T1 state);

	[Flags]
	internal enum gvyyw
	{
		pafow = 0,
		ynwhp = 1,
		zcykt = 2
	}

	private class gmumv : ISocketFactory
	{
		private readonly SshSession eowxg;

		public gmumv(SshSession session)
		{
			eowxg = session;
		}

		public ISocket CreateSocket()
		{
			return new qseon(eowxg);
		}

		public override string ToString()
		{
			return "SSH";
		}
	}

	private sealed class xigtf
	{
		public ISocket ucrsx;

		public void ojnmx()
		{
			try
			{
				ucrsx.Close();
			}
			catch
			{
			}
		}
	}

	private sealed class quslf
	{
		public byte[] zcmfx;

		public SshSession bnryj;

		public void qktxy(object p0)
		{
			try
			{
				bnryj.nubmi(zcmfx);
			}
			catch (Exception ex)
			{
				bnryj.azafp = ex;
				bnryj.cnfnb(LogLevel.Error, "Error occured during session negotiation: {0}", ex);
			}
		}
	}

	private sealed class gxsjo
	{
		public SshSession oxzye;

		public int mklcx;
	}

	private sealed class mtwdz
	{
		public SshChannel zgxox;
	}

	private sealed class ortzz
	{
		public EventHandler<ForwardingRequestEventArgs> wydoj;
	}

	private sealed class zqyoc
	{
		public ortzz ejxnt;

		public mtwdz yabxf;

		public gxsjo rujct;

		public ForwardingRequestEventArgs bdedy;

		public void wleiv(object p0)
		{
			try
			{
				ejxnt.wydoj(rujct.oxzye, bdedy);
			}
			catch (Exception ex)
			{
				rujct.oxzye.azafp = ex;
				rujct.oxzye.cnfnb(LogLevel.Error, "Error occured in ForwardingRequest event handler: {0}", ex);
			}
		}
	}

	private sealed class zolcg
	{
		public mtwdz kxota;

		public gxsjo cjsvq;

		public byte[] qdrwv;

		public void wdldf()
		{
			lock (cjsvq.oxzye.ezlld)
			{
				kxota.zgxox.flyta(qdrwv, 0, cjsvq.mklcx);
				cjsvq.oxzye.gorjz();
			}
		}
	}

	public const int DefaultPort = 22;

	internal const string mabsy = "SshSession";

	private const int wktdw = 36864;

	private const int kawlu = 1048576;

	private const int hknkg = 36864;

	private int hktnh = 60000;

	private ISocket cuvve;

	private string ytgyr;

	private Encoding iqpel = Encoding.UTF8;

	private SshParameters iuiut = new SshParameters();

	private SshCipher nokzs = SshCipher.vqarl;

	private SshPublicKey zxvkp;

	private byte[] wqdwd;

	private readonly object rtnkt = new object();

	private readonly ManualResetEvent imuyy = new ManualResetEvent(initialState: true);

	private agxpx iqbhb = new kyxoc();

	private agxpx tvvka = new kyxoc();

	private SshOptions owtja;

	private byte[] prxvb = new byte[36864];

	private byte[] nvvbw = new byte[36864];

	private readonly tndeg hlixy = new tndeg(Encoding.UTF8);

	private int xphnc;

	private tjlhp wnqou;

	private int? fkoox;

	private readonly Hashtable hhkfd = new Hashtable();

	private SshChannel dzdri;

	private SshState rlefr;

	private string geeon;

	private bool itjvo;

	private bool ymcmy;

	private readonly object pjfyi = new object();

	private bool osrxb;

	private bool arhvv;

	private bool ptkth;

	private Exception qcmsf;

	private Exception azafp;

	private readonly Queue ipqqm = new Queue();

	private readonly Queue fhvuk = new Queue();

	private readonly Queue ijbwr = new Queue();

	private byte[] hnzfs;

	private byte[] zibdy;

	private string wfqkr;

	private bool dtgnk;

	private bool ixphs;

	private readonly Queue gniwx = new Queue();

	private readonly Dictionary<int, SshForwardingHandle> okxuk = new Dictionary<int, SshForwardingHandle>();

	private EventHandler<SshMessageEventArgs> qijgi;

	private EventHandler<SshFingerprintEventArgs> yspyf;

	private EventHandler<ForwardingRequestEventArgs> ivsxk;

	private EventHandler<SshAuthenticationRequestEventArgs> zxlre;

	private object umooo = new object();

	private List<ManualResetEvent> ezlld = new List<ManualResetEvent>();

	private List<ManualResetEvent> fffyg = new List<ManualResetEvent>();

	private int gpxwl;

	private gmumv rcwyn;

	private SshServerInfo ltwhe;

	private static Action<Exception> izxgv;

	protected bool HasFingerprintEventHandler => yspyf != null;

	public bool HasForwardingRequestEventHandler => ivsxk != null;

	protected bool HasAuthenticationRequestEventHandler => zxlre != null;

	internal object mrfzh => pjfyi;

	public int Timeout
	{
		get
		{
			if (hktnh == int.MaxValue)
			{
				return -1;
			}
			return hktnh;
		}
		set
		{
			if (value < -1)
			{
				throw hifyx.nztrs("value", value, "Timeout is out of range of valid values.");
			}
			if (value < 1)
			{
				hktnh = int.MaxValue;
			}
			else if (value < 1000)
			{
				hktnh = 1000;
			}
			else
			{
				hktnh = value;
			}
		}
	}

	public ISocket Socket => cuvve;

	public Encoding Encoding
	{
		get
		{
			return iqpel;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (iqpel != value)
			{
				iqpel = value;
			}
		}
	}

	public EndPoint RemoteEndPoint
	{
		get
		{
			ISocket socket = cuvve;
			if (socket == null || 1 == 0)
			{
				throw new InvalidOperationException("Not connected to the server.");
			}
			return socket.RemoteEndPoint;
		}
	}

	public EndPoint LocalEndPoint
	{
		get
		{
			ISocket socket = cuvve;
			if (socket == null || 1 == 0)
			{
				throw new InvalidOperationException("Not connected to the server.");
			}
			return socket.LocalEndPoint;
		}
	}

	public SocketInformation Information
	{
		get
		{
			ISocket socket = cuvve;
			if (socket == null || 1 == 0)
			{
				throw new InvalidOperationException("Not connected to the server.");
			}
			return new SocketInformation(socket);
		}
	}

	[Obsolete("This property has been deprecated. Please use IsConnected instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public bool Connected => IsConnected;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This property has been deprecated. Please use IsAuthenticated instead.", false)]
	[wptwl(false)]
	public bool Authenticated => IsAuthenticated;

	public override bool IsConnected
	{
		get
		{
			ISocket socket = cuvve;
			if (socket == null || 1 == 0)
			{
				return false;
			}
			return socket.Connected;
		}
	}

	public override bool IsAuthenticated => ymcmy;

	public SshParameters Parameters
	{
		get
		{
			return iuiut;
		}
		set
		{
			if (iuiut.wqpgg && 0 == 0)
			{
				throw new InvalidOperationException("Cannot change parameters that are being used for a session.");
			}
			if (value == null || 1 == 0)
			{
				value = new SshParameters();
			}
			if (value.wqpgg && 0 == 0)
			{
				iuiut = value.Clone();
			}
			else
			{
				iuiut = value;
			}
		}
	}

	public SshOptions Options
	{
		get
		{
			return owtja;
		}
		set
		{
			owtja = value;
		}
	}

	public SshCipher Cipher => nokzs;

	public SshFingerprint Fingerprint => zxvkp.Fingerprint;

	public SshServerInfo ServerInfo
	{
		get
		{
			return ltwhe;
		}
		private set
		{
			ltwhe = value;
		}
	}

	public SshPublicKey ServerKey => zxvkp;

	public SshState State => rlefr;

	public string ServerIdentification => wfqkr;

	internal bool rsqfy => itjvo;

	public event EventHandler<SshMessageEventArgs> BannerReceived
	{
		add
		{
			EventHandler<SshMessageEventArgs> eventHandler = qijgi;
			EventHandler<SshMessageEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshMessageEventArgs> value2 = (EventHandler<SshMessageEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref qijgi, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SshMessageEventArgs> eventHandler = qijgi;
			EventHandler<SshMessageEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshMessageEventArgs> value2 = (EventHandler<SshMessageEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref qijgi, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SshFingerprintEventArgs> FingerprintCheck
	{
		add
		{
			EventHandler<SshFingerprintEventArgs> eventHandler = yspyf;
			EventHandler<SshFingerprintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshFingerprintEventArgs> value2 = (EventHandler<SshFingerprintEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref yspyf, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SshFingerprintEventArgs> eventHandler = yspyf;
			EventHandler<SshFingerprintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshFingerprintEventArgs> value2 = (EventHandler<SshFingerprintEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref yspyf, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<ForwardingRequestEventArgs> ForwardingRequest
	{
		add
		{
			EventHandler<ForwardingRequestEventArgs> eventHandler = ivsxk;
			EventHandler<ForwardingRequestEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ForwardingRequestEventArgs> value2 = (EventHandler<ForwardingRequestEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ivsxk, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<ForwardingRequestEventArgs> eventHandler = ivsxk;
			EventHandler<ForwardingRequestEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ForwardingRequestEventArgs> value2 = (EventHandler<ForwardingRequestEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ivsxk, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SshAuthenticationRequestEventArgs> AuthenticationRequest
	{
		add
		{
			EventHandler<SshAuthenticationRequestEventArgs> eventHandler = zxlre;
			EventHandler<SshAuthenticationRequestEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshAuthenticationRequestEventArgs> value2 = (EventHandler<SshAuthenticationRequestEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref zxlre, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SshAuthenticationRequestEventArgs> eventHandler = zxlre;
			EventHandler<SshAuthenticationRequestEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshAuthenticationRequestEventArgs> value2 = (EventHandler<SshAuthenticationRequestEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref zxlre, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnBannerReceived(SshMessageEventArgs e)
	{
		if (qijgi != null && 0 == 0)
		{
			qijgi(this, e);
		}
	}

	private void inepc(string p0)
	{
		cnfnb(LogLevel.Info, "Received banner:\n{0}", p0);
		OnBannerReceived(new SshMessageEventArgs(p0));
	}

	protected virtual void OnFingerprintCheck(SshFingerprintEventArgs e)
	{
		if (yspyf != null && 0 == 0)
		{
			yspyf(this, e);
		}
		else
		{
			e.Accept = true;
		}
	}

	private void arqxs(SshPublicKey p0)
	{
		SshFingerprintEventArgs e = new SshFingerprintEventArgs(p0);
		try
		{
			OnFingerprintCheck(e);
		}
		catch (Exception ex)
		{
			cnfnb(LogLevel.Info, "FingerprintCheck event handler failed: {0}", ex);
			throw new SshException(tcpjq.ziezw, "Error while checking server key.");
		}
		if (!e.Accept || 1 == 0)
		{
			iejdf(LogLevel.Info, "Server key rejected.");
			throw new SshException(tcpjq.ziezw, "The server key was rejected by the client.");
		}
	}

	protected virtual void OnAuthenticationRequest(SshAuthenticationRequestEventArgs e)
	{
		if (zxlre != null && 0 == 0)
		{
			zxlre(this, e);
		}
		else
		{
			e.Ignore();
		}
	}

	private SshAuthenticationRequestItemCollection seple(string p0, string p1, string[] p2, bool[] p3)
	{
		iejdf(LogLevel.Info, "Authentication request.");
		SshAuthenticationRequestEventArgs e = new SshAuthenticationRequestEventArgs(p0, p1, p2, p3);
		OnAuthenticationRequest(e);
		if (e.Cancel && 0 == 0)
		{
			iejdf(LogLevel.Info, "Authentication canceled.");
			throw new SshException(tcpjq.rlxea, "Authentication canceled.");
		}
		if (e.sradj && 0 == 0 && p2.Length > 0)
		{
			iejdf(LogLevel.Info, "Authentication cannot continue, no request handler initialized.");
			throw new SshException(tcpjq.rlxea, "Interactive authentication cannot continue due to missing request handler.");
		}
		return e.Items;
	}

	private bool zzyxb()
	{
		if (wfqkr == null || 1 == 0)
		{
			return false;
		}
		if (wfqkr.IndexOf("-Cisco-1") >= 0)
		{
			return true;
		}
		return false;
	}

	private bool bwgpj()
	{
		if (wfqkr == null || 1 == 0)
		{
			return false;
		}
		if (wfqkr.IndexOf("-2.") < 0)
		{
			return false;
		}
		if (wfqkr.IndexOf("F-SECURE") < 0)
		{
			return false;
		}
		if (wfqkr.IndexOf("-2.0.") >= 0)
		{
			return true;
		}
		if (wfqkr.IndexOf("-2.1 ") >= 0)
		{
			return true;
		}
		if (wfqkr.IndexOf("-2.1.0") >= 0)
		{
			return true;
		}
		if (wfqkr.IndexOf("-2.2.0") >= 0)
		{
			return true;
		}
		if (wfqkr.IndexOf("-2.3.0") >= 0)
		{
			return true;
		}
		return false;
	}

	internal bool lfvnz()
	{
		if (wfqkr == null || 1 == 0)
		{
			return false;
		}
		if (wfqkr.IndexOf("Bitvise ") >= 0)
		{
			return true;
		}
		return false;
	}

	internal void iejdf(LogLevel p0, string p1)
	{
		base.twmrq.rfpvf(p0, "SSH", p1);
	}

	internal void cnfnb(LogLevel p0, string p1, params object[] p2)
	{
		base.twmrq.byfnx(p0, "SSH", p1, p2);
	}

	internal void oocpi(LogLevel p0, string p1, byte[] p2, int p3, int p4)
	{
		base.twmrq.iyauk(p0, "SSH", p1, p2, p3, p4);
	}

	internal void oftwj(Exception p0)
	{
		if (base.twmrq.Level <= LogLevel.Error)
		{
			iejdf(LogLevel.Error, p0.ToString());
		}
	}

	internal void woqmx(LogLevel p0, Exception p1)
	{
		if (base.twmrq.Level <= p0)
		{
			iejdf(p0, p1.ToString());
		}
	}

	private static Socket bhaum(Socket p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		return p0;
	}

	public SshSession()
		: this((ISocket)null)
	{
	}

	public SshSession(Socket socket)
		: this(new bavnf(bhaum(socket)))
	{
	}

	public SshSession(ISocket socket)
	{
		if (socket != null && 0 == 0)
		{
			cuvve = socket;
			if (cuvve.Connected && 0 == 0)
			{
				rlefr = SshState.Connecting;
			}
		}
		ytgyr = null;
		try
		{
			nhmfm(lzles.jksyb());
		}
		catch (fwwdw fwwdw)
		{
			throw new SshException(fwwdw.Message);
		}
	}

	private bool? sgsfq(int p0, int p1)
	{
		if (xphnc >= p0)
		{
			return true;
		}
		if (rlefr == SshState.Closed)
		{
			return false;
		}
		if (p0 > nvvbw.Length)
		{
			if (p0 >= 1048576)
			{
				throw new SshException(tcpjq.svqut, brgjd.edcru("Received a packet that is too long - {0} bytes.", p0));
			}
			int num;
			for (num = nvvbw.Length; num < p0; num *= 2)
			{
			}
			byte[] destinationArray = new byte[num];
			Array.Copy(nvvbw, 0, destinationArray, 0, xphnc);
			nvvbw = destinationArray;
		}
		ISocket socket = cuvve;
		if (socket == null || 1 == 0)
		{
			return false;
		}
		try
		{
			do
			{
				if (p1 >= 0 && (!socket.Poll(p1 * 1000, SocketSelectMode.SelectRead) || 1 == 0))
				{
					return null;
				}
				int num2 = ((socket.Connected && 0 == 0) ? socket.Receive(nvvbw, xphnc, nvvbw.Length - xphnc, SocketFlags.None) : 0);
				if (num2 == 0 || 1 == 0)
				{
					lock (pjfyi)
					{
						rlefr = SshState.Closed;
						geeon = "The connection was closed by the server.";
						socket.Close();
					}
					iejdf(LogLevel.Debug, "SSH connection closed.");
					return false;
				}
				xphnc += num2;
			}
			while (xphnc < p0);
			return true;
		}
		catch (SocketException ex)
		{
			if ((xphnc == 0 || 1 == 0) && ex.skehp() == 10053)
			{
				lock (pjfyi)
				{
					rlefr = SshState.Closed;
					geeon = "The connection was aborted.";
					socket.Close();
				}
				iejdf(LogLevel.Info, "SSH connection aborted by the server.");
				return false;
			}
			geeon = "The connection was lost.";
			throw gejpg(tcpjq.jgdcv, ex.astkw(), ex);
		}
	}

	internal void ialsm(mkuxt p0, bool p1)
	{
		bool flag = false;
		while (true)
		{
			lock (pjfyi)
			{
				switch (rlefr)
				{
				case SshState.Closed:
				{
					object obj = geeon;
					if (obj == null || 1 == 0)
					{
						obj = "The SSH connection failed.";
					}
					throw new SshException(SshExceptionStatus.ConnectionClosed, (string)obj);
				}
				default:
					throw new SshException(tcpjq.svqut, "Cannot perform requested operation in current session state.");
				case SshState.KeyExchange:
				case SshState.Ready:
					if (rlefr != SshState.KeyExchange || p1)
					{
						if (!osrxb || 1 == 0)
						{
							osrxb = true;
							goto end_IL_0006;
						}
						flag = true;
						ipqqm.Enqueue(p0);
						goto end_IL_0006;
					}
					break;
				}
			}
			Thread.Sleep(0);
			continue;
			end_IL_0006:
			break;
		}
		if (flag && 0 == 0)
		{
			while (!p0.lmnkf)
			{
				if (qcmsf != null && 0 == 0)
				{
					throw gejpg(tcpjq.svqut, "Error while sending packet.", qcmsf);
				}
				Thread.Sleep(0);
			}
			return;
		}
		try
		{
			while (true)
			{
				hlixy.SetLength(0L);
				p0.jfjrs(hlixy);
				byte[] buffer = hlixy.GetBuffer();
				xbrcx xbrcx = (xbrcx)buffer[0];
				int num = (int)hlixy.Length;
				if (base.twmrq.Level <= LogLevel.Verbose)
				{
					if ((xbrcx == xbrcx.ekvcc || xbrcx == xbrcx.xppji) && (!base.twmrq.ngqry || 1 == 0))
					{
						iejdf(LogLevel.Verbose, brgjd.edcru("Sending packet {0} ({1} hidden bytes).", ligmw(xbrcx), num));
					}
					else
					{
						oocpi(LogLevel.Verbose, brgjd.edcru("Sending packet {0} ({1} bytes).", ligmw(xbrcx), num), buffer, 0, num);
					}
				}
				if (prxvb.Length < num + 8192)
				{
					prxvb = new byte[num + 8192];
				}
				num = tvvka.vagtd(buffer, num, prxvb);
				cuvve.Send(prxvb, 0, num, SocketFlags.None);
				p0.lmnkf = true;
				lock (pjfyi)
				{
					if (ipqqm.Count == 0 || 1 == 0)
					{
						osrxb = false;
						break;
					}
					p0 = (mkuxt)ipqqm.Dequeue();
				}
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ((!(ex is ObjectDisposedException)) ? gejpg(tcpjq.svqut, "Error while sending packet.", ex) : (ex = new SshException(SshExceptionStatus.ConnectionClosed, "The connection was closed by the server.")));
			lock (pjfyi)
			{
				osrxb = false;
				qcmsf = ex;
			}
			bajpc(ex2);
			throw ex2;
		}
	}

	private static SshException gejpg(tcpjq p0, string p1, Exception p2)
	{
		if (p2 is SocketException p3 && 0 == 0)
		{
			switch (p3.skehp())
			{
			case 10053:
				p1 = "The connection was aborted.";
				break;
			case 10054:
				p1 = "The connection was closed by the server.";
				break;
			case 10038:
				p1 = "The connection was lost.";
				break;
			}
			return new SshException(tcpjq.jgdcv, p1, p2);
		}
		return new SshException(p0, p1, p2);
	}

	internal void adxeo(mkuxt p0)
	{
		ialsm(p0, p1: false);
	}

	private int zojkn(out byte[] p0)
	{
		if (itjvo && 0 == 0)
		{
			throw new SshException(SshExceptionStatus.ConnectionClosed, "The SSH connection failed.");
		}
		int ptjhi = iqbhb.ptjhi;
		bool? flag = sgsfq(ptjhi, -1);
		if (!flag.HasValue || 1 == 0)
		{
			p0 = null;
			return 0;
		}
		if (!flag.Value || 1 == 0)
		{
			if (xphnc > 0 && cuvve != null && 0 == 0)
			{
				throw new SshException(SshExceptionStatus.ConnectionClosed, "The connection was closed by the server.");
			}
			p0 = null;
			return -1;
		}
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		int p1 = 0;
		int num = 0;
		try
		{
			flag2 = true;
			p1 = iqbhb.iadch(nvvbw, 0, ptjhi);
			flag3 = true;
			num = iqbhb.lyhbb(nvvbw, 0, ptjhi);
			flag2 = false;
			if (num > xphnc)
			{
				flag = sgsfq(num, hktnh);
				if (!flag.HasValue || 1 == 0)
				{
					throw new SshException(SshExceptionStatus.Timeout, "The operation was not completed within the specified time limit.");
				}
				if (!flag.Value || 1 == 0)
				{
					if (cuvve == null)
					{
						p0 = null;
						return -1;
					}
					throw new SshException(SshExceptionStatus.ConnectionClosed, "The connection was closed by the server.");
				}
			}
			flag2 = true;
			int result = iqbhb.keqao(new ArraySegment<byte>(nvvbw, 0, num), out p0);
			flag4 = true;
			flag2 = false;
			xphnc -= num;
			Array.Copy(nvvbw, num, nvvbw, 0, xphnc);
			return result;
		}
		finally
		{
			if (flag2 && 0 == 0)
			{
				if (!flag3 || 1 == 0)
				{
					iejdf(LogLevel.Debug, "SSH packet decrypting failed, closing connection.");
				}
				else if (!flag4 || 1 == 0)
				{
					iejdf(LogLevel.Debug, "SSH packet header decoding failed, closing connection.");
					oocpi(LogLevel.Verbose, "Raw header:", nvvbw, 0, p1);
				}
				else
				{
					iejdf(LogLevel.Debug, "SSH packet decoding failed, closing connection.");
					oocpi(LogLevel.Verbose, "Raw packet:", nvvbw, 0, num);
				}
				lock (pjfyi)
				{
					itjvo = true;
					rlefr = SshState.Closed;
					geeon = "The SSH connection failed.";
					cuvve.Close();
				}
			}
		}
	}

	internal void bajpc(Exception p0)
	{
		SshState sshState = rlefr;
		if (sshState == SshState.None || sshState == SshState.Closed)
		{
			return;
		}
		try
		{
			bool flag = false;
			uint reason;
			string description;
			if (p0 != null && 0 == 0)
			{
				reason = 2u;
				if (p0 is SshException ex && 0 == 0)
				{
					if (!ex.everl || 1 == 0)
					{
						return;
					}
					flag = ex.drbjp;
					if (ex.kpcdk > tcpjq.bxwwb)
					{
						reason = (uint)ex.kpcdk;
					}
					description = ex.Message;
				}
				else
				{
					description = "Internal error.";
				}
			}
			else
			{
				reason = 11u;
				description = "Session closed.";
			}
			if (!flag || 1 == 0)
			{
				try
				{
					pbfja p1 = new pbfja(reason, description);
					ialsm(p1, p1: true);
					return;
				}
				catch
				{
					return;
				}
			}
		}
		finally
		{
			xigtf xigtf = new xigtf();
			lock (pjfyi)
			{
				rlefr = SshState.Closed;
				xigtf.ucrsx = cuvve;
				cuvve = null;
			}
			if (xigtf.ucrsx != null && 0 == 0)
			{
				Action p2 = xigtf.ojnmx;
				if (izxgv == null || 1 == 0)
				{
					izxgv = ixmuk;
				}
				dahxy.nqapv(p2, izxgv, 2000);
			}
		}
	}

	[wptwl(false)]
	[Obsolete("This method has been deprecated. Please use Disconnect or Dispose instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Close()
	{
		Disconnect();
	}

	public void Disconnect()
	{
		try
		{
			bajpc(null);
		}
		catch (Exception p)
		{
			oftwj(p);
			throw;
		}
	}

	public void Dispose()
	{
		lock (pjfyi)
		{
			if (rlefr != SshState.Closed)
			{
				rlefr = SshState.Closed;
				if (cuvve != null && 0 == 0)
				{
					cuvve.Close();
				}
				if (wnqou != null && 0 == 0)
				{
					wnqou.Dispose();
				}
			}
		}
	}

	private static string ligmw(xbrcx p0)
	{
		return p0 switch
		{
			xbrcx.pzsaq => "SSH_MSG_DISCONNECT", 
			xbrcx.szzgl => "SSH_MSG_IGNORE", 
			xbrcx.lfusn => "SSH_MSG_UNIMPLEMENTED", 
			xbrcx.ivzex => "SSH_MSG_DEBUG", 
			xbrcx.mesei => "SSH_MSG_SERVICE_REQUEST", 
			xbrcx.xqrye => "SSH_MSG_SERVICE_ACCEPT", 
			xbrcx.efaml => "SSH_MSG_EXT_INFO", 
			xbrcx.eacea => "SSH_MSG_KEXINIT", 
			xbrcx.oxjft => "SSH_MSG_NEWKEYS", 
			xbrcx.ucfvi => "SSH_MSG_KEX_DH_GEX_INIT", 
			xbrcx.okwtv => "SSH_MSG_KEX_DH_GEX_REPLY", 
			xbrcx.hztww => "SSH_MSG_KEX_DH_GEX_REQUEST", 
			xbrcx.ekvcc => "SSH_MSG_USERAUTH_REQUEST", 
			xbrcx.lucfy => "SSH_MSG_USERAUTH_FAILURE", 
			xbrcx.melpn => "SSH_MSG_USERAUTH_SUCCESS", 
			xbrcx.oakxh => "SSH_MSG_USERAUTH_BANNER", 
			xbrcx.rmvct => "SSH_MSG_USERAUTH_GSSAPI_EXCHANGE_COMPLETE", 
			xbrcx.boivn => "SSH_MSG_USERAUTH_GSSAPI_ERROR", 
			xbrcx.ufibt => "SSH_MSG_USERAUTH_GSSAPI_ERRTOK", 
			xbrcx.hwkhx => "SSH_MSG_USERAUTH_GSSAPI_MIC", 
			xbrcx.dhhsz => "SSH_MSG_GLOBAL_REQUEST", 
			xbrcx.jyumi => "SSH_MSG_REQUEST_SUCCESS", 
			xbrcx.jxiab => "SSH_MSG_REQUEST_FAILURE", 
			xbrcx.ocrnu => "SSH_MSG_CHANNEL_OPEN", 
			xbrcx.tvjsn => "SSH_MSG_CHANNEL_OPEN_CONFIRMATION", 
			xbrcx.fhpzg => "SSH_MSG_CHANNEL_OPEN_FAILURE", 
			xbrcx.jczcw => "SSH_MSG_CHANNEL_WINDOW_ADJUST", 
			xbrcx.punhv => "SSH_MSG_CHANNEL_DATA", 
			xbrcx.ifafm => "SSH_MSG_CHANNEL_EXTENDED_DATA", 
			xbrcx.nvcjs => "SSH_MSG_CHANNEL_EOF", 
			xbrcx.evspj => "SSH_MSG_CHANNEL_CLOSE", 
			xbrcx.anpby => "SSH_MSG_CHANNEL_REQUEST", 
			xbrcx.xgswk => "SSH_MSG_CHANNEL_SUCCESS", 
			xbrcx.vqsre => "SSH_MSG_CHANNEL_FAILURE", 
			xbrcx.xnxhe => "SSH_MSG_KEX_30", 
			xbrcx.bsevy => "SSH_MSG_KEX_31", 
			xbrcx.lwoyc => "SSH_MSG_USERAUTH_60", 
			xbrcx.xppji => "SSH_MSG_USERAUTH_61", 
			_ => "UNKNOWN_PACKET_" + (int)p0, 
		};
	}

	private bool qqksm()
	{
		if (azafp != null && 0 == 0)
		{
			SshException ex = ((!(azafp is SshException ex2)) ? new SshException(SshExceptionStatus.ConnectionClosed, "Error while receiving data.") : new SshException(ex2.kpcdk, ex2.Message, ex2));
			throw ex;
		}
		if (wnqou == null || 1 == 0)
		{
			if (rlefr == SshState.Closed)
			{
				return false;
			}
			throw new SshException(SshExceptionStatus.ConnectionClosed, "The SSH connection failed.");
		}
		switch (rlefr)
		{
		case SshState.Closed:
			if (itjvo && 0 == 0)
			{
				throw new SshException(SshExceptionStatus.ConnectionClosed, "The SSH connection failed.");
			}
			return false;
		case SshState.KeyExchange:
		case SshState.Ready:
			return true;
		default:
			throw new SshException(tcpjq.svqut, "Cannot perform requested operation in current session state.");
		}
	}

	internal T lrcya<T, S>(nivbm<T, S> p0, S p1)
	{
		return ryvxm(p0, hktnh, gvyyw.pafow, p1, default(T), default(T));
	}

	internal T ryvxm<T, S>(nivbm<T, S> p0, int p1, gvyyw p2, S p3, T p4, T p5)
	{
		ManualResetEvent manualResetEvent = null;
		try
		{
			lock (pjfyi)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				int? num = fkoox;
				if (managedThreadId == num.GetValueOrDefault() && num.HasValue && 0 == 0)
				{
					throw new InvalidOperationException("Invalid usage of receiver thread.");
				}
			}
			T result;
			lock (ezlld)
			{
				if (p0(out result, p3) && 0 == 0)
				{
					return result;
				}
				if (p1 == 0 || 1 == 0)
				{
					if ((p2 & gvyyw.zcykt) == 0 || 1 == 0)
					{
						throw new SshException(SshExceptionStatus.Timeout, "The operation was not completed within the specified time limit.");
					}
					return p4;
				}
				if (fffyg.Count > 0)
				{
					int index = fffyg.Count - 1;
					manualResetEvent = fffyg[index];
					fffyg.RemoveAt(index);
				}
				else
				{
					manualResetEvent = new ManualResetEvent(initialState: false);
				}
				ezlld.Add(manualResetEvent);
			}
			int num2 = Environment.TickCount + p1;
			while (true)
			{
				if (!qqksm() || 1 == 0)
				{
					if ((p2 & gvyyw.ynwhp) == 0 || 1 == 0)
					{
						object obj = geeon;
						if (obj == null || 1 == 0)
						{
							obj = "The connection was closed by the server.";
						}
						throw new SshException(SshExceptionStatus.ConnectionClosed, (string)obj);
					}
					return p5;
				}
				if (!manualResetEvent.WaitOne(p1, exitContext: false) || 1 == 0)
				{
					p1 = num2 - Environment.TickCount;
					if (p1 < 0)
					{
						if ((p2 & gvyyw.zcykt) == 0 || 1 == 0)
						{
							throw new SshException(SshExceptionStatus.Timeout, "The operation was not completed within the specified time limit.");
						}
						return p4;
					}
					continue;
				}
				lock (ezlld)
				{
					if (p0(out result, p3))
					{
						break;
					}
					manualResetEvent.Reset();
					ezlld.Add(manualResetEvent);
					continue;
				}
			}
			return result;
		}
		finally
		{
			if (manualResetEvent != null && 0 == 0)
			{
				lock (ezlld)
				{
					ezlld.Remove(manualResetEvent);
					manualResetEvent.Reset();
					fffyg.Add(manualResetEvent);
				}
			}
		}
	}

	private void giefo()
	{
		lock (pjfyi)
		{
			if (wnqou != null && 0 == 0)
			{
				return;
			}
			wnqou = new tjlhp(this, "Packet receiver");
		}
		lock (umooo)
		{
			while (xphnc > 0 && ckrwz())
			{
			}
		}
		wnqou.lefxl();
	}

	private bool dfyfe()
	{
		bool flag;
		try
		{
			fkoox = Thread.CurrentThread.ManagedThreadId;
			do
			{
				flag = ckrwz();
			}
			while (flag && 0 == 0 && xphnc > 0);
		}
		catch (Exception ex)
		{
			flag = false;
			bajpc(ex);
			azafp = ex;
			cnfnb(LogLevel.Error, "Error occured while receiving SSH packet: {0}", ex);
		}
		finally
		{
			fkoox = null;
		}
		if (!flag || 1 == 0)
		{
			lock (pjfyi)
			{
				wnqou = null;
			}
			lock (ezlld)
			{
				gorjz();
			}
		}
		return flag;
	}

	bool kehni.jcfcv()
	{
		//ILSpy generated this explicit interface implementation from .override directive in dfyfe
		return this.dfyfe();
	}

	private void gorjz()
	{
		using (List<ManualResetEvent>.Enumerator enumerator = ezlld.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				ManualResetEvent current = enumerator.Current;
				current.Set();
			}
		}
		ezlld.Clear();
	}

	private bool ckrwz()
	{
		WaitCallback waitCallback = null;
		quslf quslf = new quslf();
		quslf.bnryj = this;
		quslf.zcmfx = null;
		lock (umooo)
		{
			bool flag;
			int num;
			byte[] p;
			lock (rtnkt)
			{
				if (iqbhb == null || 1 == 0)
				{
					flag = true;
					num = 0;
					p = null;
				}
				else
				{
					flag = false;
					num = zojkn(out p);
				}
			}
			if (flag && 0 == 0)
			{
				if (!imuyy.WaitOne(hktnh, exitContext: false) || 1 == 0)
				{
					azafp = new SshException(SshExceptionStatus.OperationFailure, "Missing read state.");
					return false;
				}
				return true;
			}
			if (num < 0)
			{
				return false;
			}
			if (num == 0 || 1 == 0)
			{
				return true;
			}
			if (base.twmrq.Level <= LogLevel.Verbose)
			{
				oocpi(LogLevel.Verbose, brgjd.edcru("Received packet {0} ({1} bytes).", ligmw((xbrcx)p[0]), num), p, 0, num);
			}
			if (p[0] != 20 || rlefr == SshState.KeyExchange)
			{
				lock (ezlld)
				{
					bool result = tqwmn(p, 0, num);
					gorjz();
					return result;
				}
			}
			rlefr = SshState.KeyExchange;
			quslf.zcmfx = new byte[num];
			Array.Copy(p, 0, quslf.zcmfx, 0, num);
		}
		if (quslf.zcmfx != null && 0 == 0)
		{
			if (waitCallback == null || 1 == 0)
			{
				waitCallback = quslf.qktxy;
			}
			if (!bvilq.eiuho(waitCallback) || 1 == 0)
			{
				throw new SshException(SshExceptionStatus.OperationFailure, "Unable to start key renegotiation.");
			}
		}
		return true;
	}

	private bool tqwmn(byte[] p0, int p1, int p2)
	{
		Action<Exception> action = null;
		gxsjo gxsjo = new gxsjo();
		gxsjo.mklcx = p2;
		gxsjo.oxzye = this;
		int num = p0[p1];
		if (num == 0 || 1 == 0)
		{
			ialsm(new meotu(iqbhb.syelh), p1: true);
			return true;
		}
		if (num < 50)
		{
			return ybcmx(p0, p1, gxsjo.mklcx);
		}
		if (num < 80)
		{
			switch (num)
			{
			case 52:
				iqbhb.lhpip();
				break;
			case 50:
				ialsm(new meotu(iqbhb.syelh), p1: true);
				return true;
			}
			byte[] array = new byte[gxsjo.mklcx];
			Array.Copy(p0, p1, array, 0, gxsjo.mklcx);
			lock (pjfyi)
			{
				if (arhvv && 0 == 0)
				{
					fhvuk.Enqueue(array);
				}
			}
			if (!arhvv || 1 == 0)
			{
				throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", num));
			}
			return true;
		}
		if (num < 90)
		{
			switch ((xbrcx)(byte)num)
			{
			case xbrcx.dhhsz:
			{
				fnofn fnofn = new fnofn(p0, p1, gxsjo.mklcx, iqpel);
				if (fnofn.tkyxc && 0 == 0)
				{
					hbgtr p4 = new hbgtr();
					ialsm(p4, p1: true);
				}
				break;
			}
			case xbrcx.jyumi:
			case xbrcx.jxiab:
			{
				yibwq yibwq;
				lock (gniwx.SyncRoot)
				{
					if (gniwx.Count == 0 || 1 == 0)
					{
						throw new SshException(SshExceptionStatus.UnexpectedMessage, brgjd.edcru("Unexpected {0} message received.", "SSH_MSG_REQUEST_FAILURE"));
					}
					yibwq = (yibwq)gniwx.Dequeue();
				}
				try
				{
					if (num == 81)
					{
						qegof p3 = new qegof(p0, p1, gxsjo.mklcx, iqpel, yibwq.nnxoj);
						yibwq.prarm(p3);
						int num2 = yibwq.nnxoj switch
						{
							bvkts.cpoos => (int)(uint)yibwq.ncfwa.uqqhd[1], 
							bvkts.frokg => (int)(uint)yibwq.nnfen[0], 
							_ => 0, 
						};
						if (num2 == 0)
						{
							break;
						}
						lock (okxuk)
						{
							if (okxuk.ContainsKey(num2) && 0 == 0)
							{
								throw new SshException(SshExceptionStatus.UnexpectedMessage, brgjd.edcru("Unexpected {0} message received.", "SSH_MSG_REQUEST_SUCCESS"));
							}
							okxuk.Add(num2, new SshForwardingHandle());
						}
						break;
					}
					new hbgtr(p0, p1, gxsjo.mklcx, iqpel);
				}
				finally
				{
					yibwq.sbtkj();
				}
				break;
			}
			default:
				ialsm(new meotu(iqbhb.syelh), p1: true);
				break;
			}
			return true;
		}
		if (num < 128)
		{
			mtwdz mtwdz = new mtwdz();
			if (num == 90)
			{
				yxshh yxshh = new yxshh(p0, p1, gxsjo.mklcx, iqpel);
				if (yxshh.kixek == "forwarded-tcpip" && 0 == 0)
				{
					int key = (int)(uint)yxshh.aryxn[1];
					SshForwardingHandle value;
					lock (okxuk)
					{
						okxuk.TryGetValue(key, out value);
					}
					if (value != null && 0 == 0)
					{
						ortzz ortzz = new ortzz();
						ortzz.wydoj = ivsxk;
						if (ortzz.wydoj == null)
						{
							value.xsmvd(yxshh);
							return true;
						}
						zqyoc zqyoc = new zqyoc();
						zqyoc.ejxnt = ortzz;
						zqyoc.yabxf = mtwdz;
						zqyoc.rujct = gxsjo;
						zqyoc.bdedy = new ForwardingRequestEventArgs(this, value, yxshh);
						if (ThreadPool.QueueUserWorkItem(zqyoc.wleiv) && 0 == 0)
						{
							return true;
						}
						iejdf(LogLevel.Error, "Unable to queue forwarding request event.");
					}
				}
				vjabb(yxshh);
				return true;
			}
			if (gxsjo.mklcx < 5)
			{
				throw new SshException(tcpjq.svqut, brgjd.edcru("Unsupported packet {0}.", num));
			}
			uint num3 = jtxhe.lhazq(p0, p1 + 1);
			mtwdz.zgxox = hhkfd[num3] as SshChannel;
			if (mtwdz.zgxox == null || 1 == 0)
			{
				if (num == 101 && dtgnk && 0 == 0)
				{
					iejdf(LogLevel.Debug, "Ignoring packet 101 received by closed channel.");
					return true;
				}
				if (dzdri != null && 0 == 0 && dzdri.evxyv == num3 && num == 95)
				{
					iejdf(LogLevel.Debug, "Ignoring data packet received by closed channel. Consider enabling PostponeChannelClose workaround.");
					return true;
				}
				if (dzdri != null && 0 == 0 && num == 98)
				{
					iejdf(LogLevel.Debug, "Ignoring request packet received by closed channel. Consider enabling PostponeChannelClose workaround.");
					return true;
				}
				throw new SshException(tcpjq.svqut, "Packet for non-existent channel received.");
			}
			if ((owtja & SshOptions.PostponeChannelClose) != SshOptions.None && 0 == 0 && num == 97)
			{
				zolcg zolcg = new zolcg();
				zolcg.kxota = mtwdz;
				zolcg.cjsvq = gxsjo;
				zolcg.qdrwv = new byte[gxsjo.mklcx];
				Array.Copy(p0, p1, zolcg.qdrwv, 0, gxsjo.mklcx);
				Action p5 = zolcg.wdldf;
				if (action == null || 1 == 0)
				{
					action = mgyfh;
				}
				dahxy.nqapv(p5, action, 200);
			}
			else
			{
				mtwdz.zgxox.flyta(p0, p1, gxsjo.mklcx);
			}
			return true;
		}
		throw new SshException(tcpjq.svqut, brgjd.edcru("Unsupported packet {0}.", num));
	}

	private bool ybcmx(byte[] p0, int p1, int p2)
	{
		switch (p0[p1])
		{
		case 1:
		{
			pbfja pbfja = new pbfja(p0, p1, p2, iqpel);
			string text = pbfja.psojj;
			if (text.Length > 0)
			{
				text = text[0].ToString().ToUpper(CultureInfo.InvariantCulture) + text.Substring(1);
				text = text.TrimEnd('.');
			}
			geeon = "Disconnected by the server ('" + text + "').";
			iejdf(LogLevel.Info, geeon);
			bajpc(null);
			return false;
		}
		case 2:
			return true;
		case 7:
			iejdf(LogLevel.Debug, "Server supports extension info extension.");
			return true;
		case 4:
		{
			cuapx cuapx = new cuapx(p0, p1, p2, iqpel);
			cnfnb(LogLevel.Debug, "Received debug message: {0}", cuapx.jwgky);
			return true;
		}
		case 3:
			throw new SshException(SshExceptionStatus.OperationFailure, "Requested service is not implemented.");
		case 21:
			if (rlefr != SshState.KeyExchange)
			{
				throw new SshException(SshExceptionStatus.UnexpectedMessage, brgjd.edcru("Unexpected {0} message received.", "SSH_MSG_NEWKEYS"));
			}
			lock (rtnkt)
			{
				if (iqbhb == null || 1 == 0)
				{
					throw new SshException(tcpjq.svqut, "Duplicate SSH_MSG_NEWKEYS received.");
				}
				iqbhb.bwbpr();
				iqbhb = null;
				imuyy.Reset();
			}
			break;
		default:
			if (p0[p1] < 30)
			{
				throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", p0[p1]));
			}
			break;
		case 6:
		case 20:
			break;
		}
		byte[] array = new byte[p2];
		Array.Copy(p0, p1, array, 0, p2);
		lock (pjfyi)
		{
			ijbwr.Enqueue(array);
		}
		return true;
	}

	private bool atals(out byte[] p0, object p1)
	{
		Queue queue = (Queue)p1;
		lock (pjfyi)
		{
			if (queue.Count > 0)
			{
				p0 = (byte[])queue.Dequeue();
			}
			else
			{
				p0 = null;
			}
		}
		if (p0 != null && 0 == 0)
		{
			return true;
		}
		return false;
	}

	internal byte[] luglr(xfdwt p0)
	{
		byte[] array = lrcya<byte[], Queue>(p1: (p0 != xfdwt.hylsk) ? ijbwr : fhvuk, p0: atals);
		if (array == null || 1 == 0)
		{
			throw new SshException(SshExceptionStatus.Timeout, "The operation was not completed within the specified time limit.");
		}
		return array;
	}

	public void Connect(string serverName)
	{
		Connect(serverName, 22);
	}

	public void Connect(string serverName, int serverPort)
	{
		if (serverName == null || 1 == 0)
		{
			throw new ArgumentNullException("serverName", "Hostname cannot be null.");
		}
		if (serverName.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Hostname cannot be empty.", "serverName");
		}
		if (serverPort < 1 || serverPort > 65535)
		{
			throw hifyx.nztrs("serverPort", serverPort, "Port is out of range of valid values.");
		}
		try
		{
			lock (pjfyi)
			{
				if (rlefr != SshState.None && 0 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "The session has already been connected.");
				}
				rlefr = SshState.Connecting;
				if (cuvve == null || 1 == 0)
				{
					cuvve = new ProxySocket();
					cuvve.Timeout = hktnh;
				}
				cnfnb(LogLevel.Info, "Connecting to {0}:{1} using {2}.", serverName, serverPort, "SshSession");
				stpsr(lzles.jksyb());
				cuvve.Connect(serverName, serverPort);
				dustp();
				ServerName = serverName;
				ServerPort = serverPort;
				if (ytgyr == null || 1 == 0)
				{
					ytgyr = serverName;
				}
			}
		}
		catch (Exception p)
		{
			oftwj(p);
			throw;
		}
		nubmi(null);
	}

	public void Connect(EndPoint remoteEP)
	{
		try
		{
			lock (pjfyi)
			{
				if (rlefr != SshState.None && 0 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "The session has already been connected.");
				}
				rlefr = SshState.Connecting;
				if (cuvve == null || 1 == 0)
				{
					cuvve = new ProxySocket();
					cuvve.Timeout = hktnh;
				}
				cnfnb(LogLevel.Info, "Connecting to {0} using {1}.", remoteEP, "SshSession");
				stpsr(lzles.jksyb());
				cuvve.Connect(remoteEP);
				dustp();
				if (remoteEP is IPEndPoint iPEndPoint && 0 == 0)
				{
					ServerName = iPEndPoint.Address.ToString();
					ServerPort = iPEndPoint.Port;
				}
				else
				{
					ServerName = null;
					ServerPort = 0;
				}
			}
		}
		catch (Exception p)
		{
			oftwj(p);
			throw;
		}
		nubmi(null);
	}

	private void dustp()
	{
		iuiut = iuiut.saltn();
		string s = iuiut.ybnef();
		hnzfs = iqpel.GetBytes(s);
		byte[] array = new byte[hnzfs.Length + 2];
		hnzfs.CopyTo(array, 0);
		array[array.Length - 2] = 13;
		array[array.Length - 1] = 10;
		bool flag = (owtja & SshOptions.WaitForServerWelcomeMessage) == 0;
		if (flag && 0 == 0)
		{
			oocpi(LogLevel.Verbose, "Sending data:", array, 0, array.Length);
			cuvve.Send(array, 0, array.Length, SocketFlags.None);
		}
		bool flag2 = false;
		xphnc = 0;
		int tickCount = Environment.TickCount;
		int num = 0;
		if (num != 0)
		{
			goto IL_00b8;
		}
		goto IL_04ad;
		IL_03af:
		bool flag3;
		if (s[4] >= '1' && s[4] <= '9' && s[5] >= '0' && s[5] <= '9')
		{
			flag3 = true;
		}
		goto IL_03de;
		IL_01fa:
		int num2;
		byte[] array2;
		int num4;
		if (nvvbw[xphnc + num2] == 10)
		{
			int num3 = ((num2 <= 0 || nvvbw[xphnc + num2 - 1] != 13) ? num2 : (num2 - 1));
			num3 += xphnc;
			array2 = new byte[num3];
			Array.Copy(nvvbw, 0, array2, 0, num3);
			s = iqpel.GetString(array2, 0, array2.Length);
			num4 -= num2 + 1;
			Array.Copy(nvvbw, xphnc + (num2 + 1), nvvbw, 0, num4);
			xphnc = 0;
			num2 = -1;
			num++;
			if (num == 1 && s.Length >= 4 && char.IsNumber(s[0]) && 0 == 0 && char.IsNumber(s[1]) && 0 == 0 && char.IsNumber(s[2]) && 0 == 0 && (s[3] == ' ' || s[3] == '-'))
			{
				if (s.IndexOf("SMTP", StringComparison.OrdinalIgnoreCase) >= 0 || s.IndexOf("mail", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					throw new SshException(tcpjq.kxdpn, "Rebex SFTP/SSH does not support SMTP protocol. Use Rebex Secure Mail component instead.");
				}
				throw new SshException(tcpjq.kxdpn, "Rebex SFTP/SSH does not support FTP or FTP/SSL protocol. Use Rebex FTP/SSL component instead.");
			}
			if (s.StartsWith("SSH-") && 0 == 0 && s.Length >= 6)
			{
				flag3 = false;
				if (s[5] != '.')
				{
					goto IL_03af;
				}
				if ((s.StartsWith("SSH-1.99") ? true : false) || (s[4] >= '2' && s[4] <= '9'))
				{
					flag3 = true;
					if (!flag3)
					{
						goto IL_03af;
					}
				}
				goto IL_03de;
			}
		}
		num2++;
		goto IL_0479;
		IL_0482:
		xphnc += num4;
		if (xphnc >= nvvbw.Length)
		{
			throw new SshException(tcpjq.svqut, "Welcome message is too long.");
		}
		goto IL_04ad;
		IL_00b8:
		if (!cuvve.Poll(1000000, SocketSelectMode.SelectRead) || 1 == 0)
		{
			int num5 = Environment.TickCount - tickCount;
			if (num5 > hktnh)
			{
				throw new SshException(SshExceptionStatus.Timeout, "Timeout exceeded while waiting for welcome message. Make sure you are connecting to an SSH or SFTP server.");
			}
			goto IL_04ad;
		}
		try
		{
			num4 = cuvve.Receive(nvvbw, xphnc, nvvbw.Length - xphnc, SocketFlags.None);
			oocpi(LogLevel.Verbose, "Received data:", nvvbw, xphnc, num4);
		}
		catch (SocketException ex)
		{
			lock (pjfyi)
			{
				rlefr = SshState.Closed;
				cuvve.Close();
			}
			switch (ex.skehp())
			{
			case 10054:
				throw new SshException(SshExceptionStatus.ConnectionClosed, "The connection was closed by the server. Make sure you are connecting to an SSH or SFTP server.");
			case 10053:
				throw new SshException(SshExceptionStatus.ConnectionClosed, "The connection was aborted.");
			default:
				throw new SshException("Error while receiving data.", ex, SshExceptionStatus.UnclassifiableError);
			}
		}
		if (num4 == 0 || 1 == 0)
		{
			lock (pjfyi)
			{
				rlefr = SshState.Closed;
				cuvve.Close();
			}
			throw new SshException(SshExceptionStatus.ConnectionClosed, "The connection was closed by the server. Make sure you are connecting to an SSH or SFTP server.");
		}
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_01fa;
		}
		goto IL_0479;
		IL_0479:
		if (num2 < num4)
		{
			goto IL_01fa;
		}
		goto IL_0482;
		IL_04ad:
		if (flag2 && 0 == 0)
		{
			if (!flag || 1 == 0)
			{
				oocpi(LogLevel.Verbose, "Sending data:", array, 0, array.Length);
				cuvve.Send(array, 0, array.Length, SocketFlags.None);
			}
			lock (pjfyi)
			{
				rlefr = SshState.KeyExchange;
				return;
			}
		}
		goto IL_00b8;
		IL_03de:
		if (!flag3 || 1 == 0)
		{
			throw new SshException(tcpjq.clcxg, "Unsupported protocol version.");
		}
		cnfnb(LogLevel.Debug, "Server is '{0}'.", s);
		wfqkr = s;
		dtgnk = wfqkr.IndexOf("-OpenSSH_") >= 0;
		ixphs = wfqkr.IndexOf("Sun_SSH_1") >= 0 || wfqkr.IndexOf("OpenSSH_3.") >= 0;
		flag2 = true;
		zibdy = array2;
		goto IL_0482;
	}

	private void nubmi(byte[] p0)
	{
		lock (pjfyi)
		{
			switch (rlefr)
			{
			case SshState.Ready:
				if (p0 != null && 0 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Cannot perform requested operation in current session state.");
				}
				rlefr = SshState.KeyExchange;
				break;
			default:
				throw new SshException(SshExceptionStatus.OperationFailure, "Cannot perform requested operation in current session state.");
			case SshState.KeyExchange:
				break;
			}
			giefo();
		}
		iejdf(LogLevel.Info, "Negotiation started.");
		SshServerInfo p1 = null;
		try
		{
			SshParameters sshParameters = iuiut;
			ckzrf ckzrf = new ckzrf(sshParameters, wfqkr);
			ialsm(ckzrf, p1: true);
			byte[] p2 = ckzrf.wavkv(iqpel);
			if (p0 == null || 1 == 0)
			{
				p0 = luglr(xfdwt.ykvft);
			}
			ckzrf ckzrf2 = new ckzrf(p0, 0, p0.Length, iqpel);
			p1 = (ServerInfo = new SshServerInfo(ckzrf2));
			ovpxz ovpxz = ckzrf2.ixswi(ckzrf);
			ovpxz ovpxz2 = ckzrf2.rtpcs(ckzrf);
			eswpb eswpb = ((ovpxz != null && 0 == 0 && (ovpxz.fepmd ? true : false)) ? null : ckzrf2.hijph(ckzrf));
			eswpb eswpb2 = ((ovpxz2 != null && 0 == 0 && (ovpxz2.fepmd ? true : false)) ? null : ckzrf2.iwqiu(ckzrf));
			string text = ckzrf2.onfqr(ckzrf);
			string text2 = ckzrf2.zkxxn(ckzrf);
			SshKeyExchangeAlgorithm p3;
			string p4;
			SshHostKeyAlgorithm p5;
			string p6;
			kumym kumym = ckzrf2.dvufx(ckzrf, out p3, out p4, out p5, out p6);
			ikgnw.qzuiv(ckzrf2, p3, p5, ovpxz, ovpxz2, eswpb, eswpb2);
			if (ckzrf2.xcdcy && 0 == 0 && (!ckzrf2.oqcyi(ckzrf) || 1 == 0))
			{
				byte[] array = luglr(xfdwt.ykvft);
				if (array[0] < 30)
				{
					throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", array[0]));
				}
			}
			agxpx p7 = iqbhb;
			kumym.kuyvo(this, hnzfs, zibdy, p2, p0, out var p8, out var p9, out var p10);
			zxvkp = p10;
			if (zxvkp.KeyAlgorithm == SshHostKeyAlgorithm.RSA)
			{
				int keySize = zxvkp.KeySize;
				int minimumRsaKeySize = sshParameters.MinimumRsaKeySize;
				cnfnb(LogLevel.Debug, "Received {0}-bit RSA server key (minimum allowed size is {1} bits).", keySize, minimumRsaKeySize);
				if (keySize < minimumRsaKeySize)
				{
					throw new SshException(tcpjq.ziezw, brgjd.edcru("The {0}'s RSA key ({1} bits) is weaker than expected minimum ({2} bits).", "server", keySize, minimumRsaKeySize));
				}
			}
			arqxs(zxvkp);
			if (wqdwd == null || 1 == 0)
			{
				wqdwd = (byte[])p9.Clone();
			}
			rxjtj rxjtj = p8.eoate(p9, wqdwd);
			ovpxz.luhux(rxjtj.ekaai('A', ovpxz.ttokf));
			ovpxz2.luhux(rxjtj.ekaai('B', ovpxz2.ttokf));
			ovpxz.mlcki(rxjtj.ekaai('C', ovpxz.fkjgo));
			ovpxz2.mlcki(rxjtj.ekaai('D', ovpxz2.fkjgo));
			bool flag = bwgpj();
			if (eswpb != null && 0 == 0)
			{
				if (flag && 0 == 0 && eswpb.hcwod == SshMacAlgorithm.SHA1)
				{
					eswpb.inemt = rxjtj.ekaai('E', 16);
				}
				else
				{
					eswpb.inemt = rxjtj.ekaai('E', eswpb.qwzwb);
				}
			}
			if (eswpb2 != null && 0 == 0)
			{
				if (flag && 0 == 0 && eswpb2.hcwod == SshMacAlgorithm.SHA1)
				{
					eswpb2.inemt = rxjtj.ekaai('F', 16);
				}
				else
				{
					eswpb2.inemt = rxjtj.ekaai('F', eswpb2.qwzwb);
				}
			}
			p8.ngsco();
			qxzct p11 = new qxzct();
			ialsm(p11, p1: true);
			tvvka.bwbpr();
			tvvka = ovpxz.dfcac(tvvka, eswpb, (text == null) ? ((int?)null) : new int?(iuiut.CompressionLevel));
			if (text == "zlib" && 0 == 0)
			{
				tvvka.lhpip();
			}
			byte[] array2 = luglr(xfdwt.ykvft);
			new qxzct(array2, 0, array2.Length, iqpel);
			lock (pjfyi)
			{
				lock (rtnkt)
				{
					iqbhb = ovpxz2.gnmax(p7, eswpb2, text2 != null);
					if (text2 == "zlib" && 0 == 0)
					{
						iqbhb.lhpip();
					}
					nokzs = new SshCipher(eswpb, eswpb2, ovpxz, ovpxz2, p3, p4, p5, p6, text != null, text2 != null);
					rlefr = SshState.Ready;
					imuyy.Set();
				}
			}
			iejdf(LogLevel.Info, "Negotiation finished.");
		}
		catch (Exception ex)
		{
			if (ex is SshException ex2 && 0 == 0)
			{
				ex2.zgdgc(p1);
				object obj = ex2.izlra;
				if (obj == null || 1 == 0)
				{
					obj = "{0}";
				}
				ex2.izlra = "Negotiation failed. " + (string)obj;
				iejdf((ex2.nqhfo == "MicrosoftKeyDeriver") ? LogLevel.Debug : LogLevel.Error, ex2.hndyi());
				bajpc(ex);
				throw;
			}
			string p12 = "Negotiation failed. " + ex.Message;
			iejdf(LogLevel.Error, p12);
			bajpc(ex);
			SshException ex3 = new SshException(tcpjq.ziezw, "Negotiation failed.", ex);
			ex3.zgdgc(p1);
			throw ex3;
		}
	}

	public void Negotiate()
	{
		try
		{
			lock (pjfyi)
			{
				if (rlefr == SshState.Connecting)
				{
					dustp();
				}
			}
		}
		catch (Exception p)
		{
			oftwj(p);
			throw;
		}
		nubmi(null);
	}

	private byte[] tljjp()
	{
		byte[] array;
		while (true)
		{
			array = luglr(xfdwt.hylsk);
			switch (array[0])
			{
			case 52:
				iejdf(LogLevel.Debug, "Authentication successful.");
				tvvka.lhpip();
				break;
			case 53:
				goto IL_003f;
			}
			break;
			IL_003f:
			scooe scooe = new scooe(array, 0, array.Length, iqpel);
			inepc(scooe.mezfo);
		}
		return array;
	}

	public SshPasswordChangeResult ChangePassword(string userName, string oldPassword, string newPassword)
	{
		if (userName == null || 1 == 0)
		{
			throw new ArgumentNullException("userName");
		}
		if (oldPassword == null || 1 == 0)
		{
			throw new ArgumentNullException("oldPassword");
		}
		if (newPassword == null || 1 == 0)
		{
			throw new ArgumentNullException("newPassword");
		}
		try
		{
			lock (pjfyi)
			{
				if (arhvv && 0 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Authentication is already in progress.");
				}
				if (ymcmy && 0 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Already authenticated.");
				}
				if (rlefr != SshState.Ready && rlefr != SshState.KeyExchange)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Cannot perform requested operation in current session state.");
				}
				arhvv = true;
			}
			try
			{
				mnoot();
				adxeo(new fyvkx("ssh-connection", userName, oldPassword, newPassword, iqpel));
				byte[] array = tljjp();
				switch (array[0])
				{
				case 52:
					ymcmy = true;
					return SshPasswordChangeResult.Success;
				case 60:
					throw new SshException(tcpjq.rlxea, "The password was not changed because the new password was not acceptable.");
				case 51:
				{
					nqbzl nqbzl = new nqbzl(array, 0, array.Length, iqpel);
					if (nqbzl.abbgz && 0 == 0)
					{
						return SshPasswordChangeResult.ChangedButNotAuthenticated;
					}
					return SshPasswordChangeResult.Failure;
				}
				default:
					throw new SshException(tcpjq.rlxea, brgjd.edcru("Unsupported packet {0}.", array[0]));
				}
			}
			finally
			{
				lock (pjfyi)
				{
					arhvv = false;
				}
			}
		}
		catch (Exception p)
		{
			oftwj(p);
			throw;
		}
	}

	public void Authenticate(string userName)
	{
		uncxp(userName, null, null, null, p4: true);
	}

	public void Authenticate(string userName, SshPrivateKey privateKey)
	{
		if (userName == null || 1 == 0)
		{
			throw new ArgumentNullException("userName");
		}
		if (privateKey == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKey");
		}
		uncxp(userName, null, privateKey, null, p4: false);
	}

	public void Authenticate(string userName, string password)
	{
		if (userName == null || 1 == 0)
		{
			throw new ArgumentNullException("userName");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		uncxp(userName, password, null, null, p4: false);
	}

	public void Authenticate(string userName, string password, SshPrivateKey privateKey)
	{
		if (userName == null || 1 == 0)
		{
			throw new ArgumentNullException("userName");
		}
		if ((password == null || 1 == 0) && (privateKey == null || 1 == 0))
		{
			throw new ArgumentException("Either private key or password must be specified.");
		}
		uncxp(userName, password, privateKey, null, p4: false);
	}

	public void Authenticate(SshGssApiCredentials credentials)
	{
		if (credentials == null || 1 == 0)
		{
			throw new ArgumentNullException("credentials");
		}
		uncxp("", null, null, credentials, p4: false);
	}

	private void mnoot()
	{
		if (!ptkth || 1 == 0)
		{
			ialsm(new scabu("ssh-userauth"), p1: false);
			byte[] array = luglr(xfdwt.ykvft);
			new timox(array, 0, array.Length, iqpel);
			ptkth = true;
		}
	}

	private bool ivuyt(byte[] p0)
	{
		switch (p0[0])
		{
		case 64:
		{
			lajni lajni = new lajni(p0, 0, p0.Length, iqpel);
			cnfnb(LogLevel.Debug, "Server reported GSSAPI error message: {0}", lajni.ydgsz);
			return true;
		}
		case 65:
		{
			umkng umkng = new umkng(p0, 0, p0.Length);
			byte[] wsydy = umkng.wsydy;
			oocpi(LogLevel.Debug, "Server reported GSSAPI error token:", wsydy, 0, wsydy.Length);
			return true;
		}
		default:
			return false;
		}
	}

	private void teveq(string p0, string p1, SshGssApiCredentials p2)
	{
		if (dahxy.xzevd && 0 == 0)
		{
			throw new SshException(tcpjq.rlxea, "SSPI authentication is not supported on non-Windows systems.");
		}
		string[] array = p2.omips();
		if (array.Length == 0 || 1 == 0)
		{
			throw new SshException(tcpjq.rlxea, "None of the specified GSSAPI algorithms is supported.");
		}
		cnfnb(LogLevel.Debug, "Trying GSSAPI authentication ({0}).", string.Join(", ", array));
		ialsm(fyvkx.qzwyo("ssh-connection", p1, array), p1: false);
		byte[] array2;
		do
		{
			array2 = tljjp();
		}
		while (ivuyt(array2) ? true : false);
		switch (array2[0])
		{
		case 60:
		{
			hjpwi hjpwi = new hjpwi(array2, 0, array2.Length, iqpel);
			string value = hjpwi.ledxm.Value;
			cnfnb(LogLevel.Debug, "Selected algorithm is {0}.", value);
			SspiAuthentication sspiAuthentication = null;
			try
			{
				SspiRequirements sspiRequirements = SspiRequirements.Confidentiality | SspiRequirements.Integrity;
				string text;
				if ((text = value) != null && 0 == 0)
				{
					if (!(text == "1.3.6.1.4.1.311.2.2.10") || 1 == 0)
					{
						if (!(text == "1.2.840.113554.1.2.2") || 1 == 0)
						{
							goto IL_01eb;
						}
						sspiRequirements |= SspiRequirements.MutualAuthentication;
						if (p2.AllowDelegation && 0 == 0)
						{
							sspiRequirements |= SspiRequirements.Delegation;
						}
						sspiAuthentication = new SspiAuthentication("Kerberos", SspiDataRepresentation.Native, "host/" + p0, sspiRequirements, p2.UserName, p2.Password, p2.Domain);
					}
					else
					{
						sspiAuthentication = new SspiAuthentication("NTLM", SspiDataRepresentation.Native, p0, sspiRequirements, p2.UserName, p2.Password, p2.Domain);
					}
					byte[] challenge = null;
					while (true)
					{
						bool complete = false;
						byte[] nextMessage = sspiAuthentication.GetNextMessage(challenge, out complete);
						if (nextMessage.Length > 0)
						{
							adxeo(new ldkdl(nextMessage));
						}
						if (complete && 0 == 0)
						{
							tndeg tndeg = new tndeg(iqpel);
							mkuxt.lcbhj(tndeg, wqdwd, p2: false);
							mkuxt.agnqw(tndeg, 50);
							mkuxt.excko(tndeg, p1);
							mkuxt.excko(tndeg, "ssh-connection");
							mkuxt.excko(tndeg, "gssapi-with-mic");
							nextMessage = sspiAuthentication.MakeSignature(tndeg.ToArray());
							adxeo(new wjjud(nextMessage));
						}
						try
						{
							do
							{
								array2 = tljjp();
							}
							while (ivuyt(array2) ? true : false);
						}
						catch (SshException ex)
						{
							if (complete && 0 == 0 && ex.Status == SshExceptionStatus.OperationFailure)
							{
								throw new SshException(tcpjq.kxdpn, "GSSAPI authentication failed.");
							}
							throw;
						}
						switch (array2[0])
						{
						case 61:
							if (complete && 0 == 0)
							{
								throw new SshException(tcpjq.rlxea, brgjd.edcru("Unexpected packet {0}.", array2[0]));
							}
							break;
						case 51:
							new nqbzl(array2, 0, array2.Length, iqpel);
							throw new SshException(tcpjq.rlxea, "GSSAPI authentication failed.");
						case 52:
							if (complete && 0 == 0)
							{
								return;
							}
							throw new SshException(tcpjq.rlxea, brgjd.edcru("Unexpected packet {0}.", array2[0]));
						default:
							throw new SshException(tcpjq.rlxea, brgjd.edcru("Unsupported packet {0}.", array2[0]));
						}
						ldkdl ldkdl = new ldkdl(array2, 0, array2.Length, iqpel);
						challenge = ldkdl.qcnlq;
					}
				}
				goto IL_01eb;
				IL_01eb:
				throw new SshException(tcpjq.rlxea, "Unsupported GSSAPI mechanism.");
			}
			catch (SspiException ex2)
			{
				throw new SshException(tcpjq.rlxea, ex2.Message, ex2);
			}
			finally
			{
				if (sspiAuthentication != null && 0 == 0)
				{
					sspiAuthentication.Dispose();
				}
			}
		}
		case 51:
			new nqbzl(array2, 0, array2.Length, iqpel);
			throw new SshException(tcpjq.rlxea, "GSSAPI authentication refused by the server.");
		default:
			throw new SshException(tcpjq.rlxea, brgjd.edcru("Unsupported packet {0}.", array2[0]));
		}
	}

	private bool zkffz(string p0, string p1, bool p2)
	{
		if (p1 == null || 1 == 0)
		{
			string serverName = ServerName;
			string[] p3 = new string[1] { "Password:" };
			bool[] p4 = new bool[1];
			SshAuthenticationRequestItemCollection sshAuthenticationRequestItemCollection = seple(serverName, "Enter your password", p3, p4);
			p1 = sshAuthenticationRequestItemCollection[0].Response;
		}
		cnfnb(LogLevel.Debug, "Trying password authentication for '{0}'.", p0);
		ialsm(new fyvkx("ssh-connection", p0, p1, iqpel), p1: false);
		byte[] array = tljjp();
		switch (array[0])
		{
		case 52:
			ymcmy = true;
			UserName = p0;
			return true;
		case 51:
		{
			nqbzl nqbzl = new nqbzl(array, 0, array.Length, iqpel);
			if (nqbzl.abbgz && 0 == 0)
			{
				iejdf(LogLevel.Debug, "Partially successful.");
			}
			if (p2 && 0 == 0)
			{
				if (!nqbzl.abbgz || 1 == 0)
				{
					throw new SshException(tcpjq.rlxea, "A supplied password or user name is incorrect.");
				}
				if (Array.IndexOf(nqbzl.kcxck, "publickey", 0, nqbzl.kcxck.Length) < 0 || 1 == 0)
				{
					throw new SshException(tcpjq.rlxea, brgjd.edcru("Authentication was partially successful, but server requires additional authentication with: '{0}'.", string.Join(",", nqbzl.kcxck)));
				}
			}
			else
			{
				if (nqbzl.abbgz && 0 == 0)
				{
					throw new SshException(tcpjq.rlxea, brgjd.edcru("Authentication was partially successful, but server requires additional authentication with: '{0}'.", string.Join(",", nqbzl.kcxck)));
				}
				if (Array.IndexOf(nqbzl.kcxck, "password", 0, nqbzl.kcxck.Length) < 0)
				{
					if (nqbzl.kcxck.Length == 0 || 1 == 0)
					{
						throw new SshException(tcpjq.rlxea, "Authentication attempt rejected by the server.");
					}
					throw new SshException(tcpjq.rlxea, "Authentication attempt rejected by the server." + brgjd.edcru(" Alternative methods: '{0}'", string.Join(",", nqbzl.kcxck)));
				}
			}
			return false;
		}
		case 60:
			throw new SshException(SshExceptionStatus.PasswordChangeRequired, "The user must change the password first.");
		default:
			throw new SshException(tcpjq.rlxea, brgjd.edcru("Unsupported packet {0}.", array[0]));
		}
	}

	private bool vusot(string p0, string p1, bool p2)
	{
		cnfnb(LogLevel.Debug, "Trying interactive authentication for '{0}'.", p0);
		ialsm(new fyvkx("ssh-connection", p0, new string[0]), p1: false);
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		while (true)
		{
			byte[] array = tljjp();
			euswy euswy;
			switch (array[0])
			{
			case 52:
				ymcmy = true;
				UserName = p0;
				return true;
			case 60:
			{
				num++;
				byzxn byzxn = new byzxn(array, 0, array.Length, iqpel);
				euswy = null;
				bool flag4 = false;
				if (byzxn.pjmze.Length > 0)
				{
					string text = byzxn.pjmze[0];
					flag4 = text.IndexOf("assword:", StringComparison.OrdinalIgnoreCase) >= 0;
					if (!flag4 || 1 == 0)
					{
						flag4 = text.StartsWith("Password for " + p0, StringComparison.Ordinal);
					}
				}
				if (p1 != null && 0 == 0 && (!flag2 || 1 == 0) && byzxn.pjmze.Length == 1 && flag4 && 0 == 0)
				{
					flag2 = true;
					euswy = new euswy(p1);
				}
				if (euswy == null || 1 == 0)
				{
					SshAuthenticationRequestItemCollection sshAuthenticationRequestItemCollection = seple(byzxn.pfpkh, byzxn.ovtqn, byzxn.pjmze, byzxn.nerlh);
					if (sshAuthenticationRequestItemCollection.Count > 0)
					{
						flag = true;
						euswy = new euswy(sshAuthenticationRequestItemCollection.cqcma());
					}
				}
				if ((euswy == null || 1 == 0) && (byzxn.pjmze.Length == 0 || 1 == 0))
				{
					euswy = new euswy();
				}
				if (euswy == null || 1 == 0)
				{
					throw new SshException(tcpjq.rlxea, brgjd.edcru("Authentication prompt '{0}' (pass {1}) cannot be answered programmatically. Use AuthenticationRequest event instead.", string.Join(",", byzxn.pjmze), num));
				}
				break;
			}
			case 51:
			{
				nqbzl nqbzl = new nqbzl(array, 0, array.Length, iqpel);
				if (nqbzl.abbgz && 0 == 0)
				{
					iejdf(LogLevel.Debug, "Partially successful.");
				}
				bool flag3 = Array.IndexOf(nqbzl.kcxck, "keyboard-interactive", 0, nqbzl.kcxck.Length) >= 0;
				if (num > 0)
				{
					if (flag3 && 0 == 0)
					{
						if ((flag ? true : false) || !flag2 || 1 == 0)
						{
							throw new SshException(tcpjq.rlxea, "Supplied credentials not accepted by the server.");
						}
						throw new SshException(tcpjq.rlxea, "A supplied password or user name is incorrect.");
					}
					if (nqbzl.abbgz && 0 == 0)
					{
						if (p2 && 0 == 0 && Array.IndexOf(nqbzl.kcxck, "publickey", 0, nqbzl.kcxck.Length) >= 0 && 0 == 0)
						{
							return false;
						}
						throw new SshException(tcpjq.rlxea, brgjd.edcru("Authentication was partially successful, but server requires additional authentication with: '{0}'.", string.Join(",", nqbzl.kcxck)));
					}
					throw new SshException(tcpjq.rlxea, "Interactive authentication announced but rejected.");
				}
				if (nqbzl.abbgz && 0 == 0)
				{
					throw new SshException(tcpjq.rlxea, brgjd.edcru("Authentication was partially successful, but server requires additional authentication with: '{0}'.", string.Join(",", nqbzl.kcxck)));
				}
				if (flag3 && 0 == 0)
				{
					throw new SshException(tcpjq.rlxea, "A supplied user name not accepted by the server.");
				}
				if (nqbzl.kcxck.Length == 0 || 1 == 0)
				{
					throw new SshException(tcpjq.rlxea, "Authentication attempt rejected by the server.");
				}
				throw new SshException(tcpjq.rlxea, "Authentication attempt rejected by the server." + brgjd.edcru(" Alternative methods: '{0}'", string.Join(",", nqbzl.kcxck)));
			}
			default:
				throw new SshException(tcpjq.rlxea, brgjd.edcru("Unexpected packet {0}.", array[0]));
			}
			ialsm(euswy, p1: false);
		}
	}

	private void uncxp(string p0, string p1, SshPrivateKey p2, SshGssApiCredentials p3, bool p4)
	{
		try
		{
			string text = null;
			string text2;
			if (p2 != null && 0 == 0)
			{
				text2 = iuiut.jwait(p2, ServerInfo.ServerHostKeyAlgorithms);
				if (text2 == null || 1 == 0)
				{
					throw new InvalidOperationException("Private key is not supported by the server.");
				}
				if (p2.KeyAlgorithm == SshHostKeyAlgorithm.RSA)
				{
					int keySize = p2.KeySize;
					int minimumRsaKeySize = iuiut.MinimumRsaKeySize;
					if (keySize < minimumRsaKeySize)
					{
						throw new InvalidOperationException(brgjd.edcru("The {0}'s RSA key ({1} bits) is weaker than expected minimum ({2} bits).", "client", keySize, minimumRsaKeySize));
					}
				}
			}
			else
			{
				text2 = null;
			}
			lock (pjfyi)
			{
				if (arhvv && 0 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Authentication is already in progress.");
				}
				if (ymcmy && 0 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Already authenticated.");
				}
				if (rlefr != SshState.Ready && rlefr != SshState.KeyExchange)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Cannot perform requested operation in current session state.");
				}
				if (p3 != null && 0 == 0)
				{
					text = p3.TargetName;
					if (text == null || 1 == 0)
					{
						text = ytgyr;
					}
					if (text == null || 1 == 0)
					{
						throw new InvalidOperationException("The target hostname needs to be specified.");
					}
					p0 = ((!string.IsNullOrEmpty(p3.AccountName)) ? p3.AccountName : p3.UserName);
					if (p0 == null || false || p0.Length == 0 || 1 == 0)
					{
						p0 = "";
					}
					p1 = null;
				}
				arhvv = true;
			}
			if ((p0 == null || 1 == 0) && p4 && 0 == 0)
			{
				SshAuthenticationRequestItemCollection sshAuthenticationRequestItemCollection = seple(ServerName, "Enter your user name", new string[1] { "Username:" }, new bool[1] { true });
				p0 = sshAuthenticationRequestItemCollection[0].Response;
			}
			try
			{
				mnoot();
				string[] array = null;
				ialsm(new fyvkx("ssh-connection", p0), p1: false);
				byte[] array2;
				while (array == null)
				{
					array2 = tljjp();
					switch (array2[0])
					{
					case 51:
					{
						nqbzl nqbzl = new nqbzl(array2, 0, array2.Length, iqpel);
						array = nqbzl.kcxck;
						if (array.Length > 0)
						{
							cnfnb(LogLevel.Debug, "Allowed authentication methods for '{0}': {1}.", p0, string.Join(", ", array));
						}
						else
						{
							cnfnb(LogLevel.Debug, "Authentication not allowed for '{0}'.", p0);
						}
						break;
					}
					case 52:
						ymcmy = true;
						UserName = p0;
						return;
					default:
						throw new SshException(tcpjq.rlxea, brgjd.edcru("Unsupported packet {0}.", array2[0]));
					}
				}
				SshAuthenticationMethod authenticationMethods = iuiut.AuthenticationMethods;
				bool flag = Array.IndexOf(array, "publickey", 0, array.Length) >= 0 && (authenticationMethods & SshAuthenticationMethod.PublicKey) != 0;
				bool flag2 = Array.IndexOf(array, "password", 0, array.Length) >= 0 && (authenticationMethods & SshAuthenticationMethod.Password) != 0;
				bool flag3 = Array.IndexOf(array, "keyboard-interactive", 0, array.Length) >= 0 && (authenticationMethods & SshAuthenticationMethod.KeyboardInteractive) != 0;
				bool flag4 = Array.IndexOf(array, "gssapi-with-mic", 0, array.Length) >= 0 && (authenticationMethods & SshAuthenticationMethod.GssapiWithMic) != 0;
				if (p3 != null && 0 == 0 && flag4 && 0 == 0)
				{
					teveq(text, p0, p3);
					ymcmy = true;
					UserName = p0;
					return;
				}
				bool flag5;
				if (p2 != null && 0 == 0 && flag && 0 == 0)
				{
					if ((p1 != null || p4) && (owtja & SshOptions.TryPasswordFirst) != SshOptions.None && 0 == 0)
					{
						if (flag3 && 0 == 0 && (owtja & (SshOptions)2097152) != SshOptions.None)
						{
							if (vusot(p0, p1, p2: true) && 0 == 0)
							{
								ymcmy = true;
								UserName = p0;
								return;
							}
						}
						else if (flag2 && 0 == 0 && zkffz(p0, p1, p2: true) && 0 == 0)
						{
							ymcmy = true;
							UserName = p0;
							return;
						}
					}
					flag5 = true;
					if (zzyxb() && 0 == 0)
					{
						flag5 = false;
						if (!flag5)
						{
							goto IL_04d6;
						}
					}
					if ((owtja & SshOptions.EnsureKeyAcceptable) != SshOptions.None && 0 == 0)
					{
						flag5 = false;
					}
					goto IL_04d6;
				}
				goto IL_06c1;
				IL_04d6:
				bool padSignature = p2.KeyAlgorithm == SshHostKeyAlgorithm.RSA && (owtja & SshOptions.EnableSignaturePadding) != 0;
				cnfnb(LogLevel.Debug, "Trying public key authentication for '{0}' using '{1}'.", p0, text2);
				ialsm(new fyvkx("ssh-connection", p0, wqdwd, p2, text2, flag5, padSignature), p1: false);
				array2 = tljjp();
				if (array2[0] == 60)
				{
					if (flag5 && 0 == 0)
					{
						iejdf(LogLevel.Debug, "Received wrong PK_OK packet. Attempting workaround.");
					}
					ialsm(new fyvkx("ssh-connection", p0, wqdwd, p2, text2, withSignature: true, padSignature), p1: false);
					array2 = tljjp();
				}
				switch (array2[0])
				{
				case 52:
					ymcmy = true;
					UserName = p0;
					return;
				case 51:
					break;
				default:
					throw new SshException(tcpjq.rlxea, brgjd.edcru("Unsupported packet {0}.", array2[0]));
				}
				nqbzl nqbzl2 = new nqbzl(array2, 0, array2.Length, iqpel);
				if (!nqbzl2.abbgz || 1 == 0)
				{
					throw new SshException(tcpjq.rlxea, "A public key corresponding to the supplied private key was not accepted by the server or the user name is incorrect.");
				}
				iejdf(LogLevel.Debug, "Partially successful.");
				flag2 = Array.IndexOf(nqbzl2.kcxck, "password", 0, nqbzl2.kcxck.Length) >= 0;
				flag3 = Array.IndexOf(nqbzl2.kcxck, "keyboard-interactive", 0, nqbzl2.kcxck.Length) >= 0;
				if (((p1 == null || 1 == 0) && !p4) || ((!flag2 || 1 == 0) && (!flag3 || 1 == 0)))
				{
					throw new SshException(tcpjq.rlxea, brgjd.edcru("Authentication was partially successful, but server requires additional authentication with: '{0}'.", string.Join(",", nqbzl2.kcxck)));
				}
				goto IL_06c1;
				IL_06c1:
				if (p1 != null || p4)
				{
					if (flag3 && 0 == 0 && (owtja & (SshOptions)2097152) != SshOptions.None && 0 == 0)
					{
						vusot(p0, p1, p2: false);
						return;
					}
					if (flag2 && 0 == 0)
					{
						if (zkffz(p0, p1, p2: false) && 0 == 0)
						{
							return;
						}
						if (!flag3 || false || !ixphs)
						{
							throw new SshException(tcpjq.rlxea, "A supplied password or user name is incorrect.");
						}
						iejdf(LogLevel.Info, "Unsuccessful authentication, trying interactive instead.");
					}
					if (flag3 && 0 == 0 && ((owtja & (SshOptions)2097152) == 0 || 1 == 0))
					{
						vusot(p0, p1, p2: false);
						return;
					}
				}
				if (array.Length == 0 || 1 == 0)
				{
					throw new SshException(tcpjq.rlxea, "Authentication attempt rejected by the server.");
				}
				throw new SshException(tcpjq.rlxea, brgjd.edcru("No suitable authentication method is supported. Supported methods: '{0}'.", string.Join(",", array)));
			}
			finally
			{
				lock (pjfyi)
				{
					arhvv = false;
				}
			}
		}
		catch (Exception p5)
		{
			oftwj(p5);
			throw;
		}
	}

	internal void ubjbk(SshChannel p0)
	{
		lock (hhkfd.SyncRoot)
		{
			hhkfd.Remove(p0.evxyv);
			dzdri = p0;
		}
	}

	private SshChannel hkiur(SshChannelType p0, int p1)
	{
		lock (hhkfd.SyncRoot)
		{
			uint num;
			if (hhkfd.Count == 0 || 1 == 0)
			{
				num = 0u;
				if (num == 0)
				{
					goto IL_006b;
				}
			}
			byte[] array = new byte[4];
			do
			{
				jtxhe.ubsib(array, 0, array.Length);
				num = BitConverter.ToUInt32(array, 0);
				num &= 0x7FFFFFFF;
			}
			while (hhkfd.ContainsKey(num) ? true : false);
			goto IL_006b;
			IL_006b:
			SshChannel sshChannel = new SshChannel(this, num, p0, iuiut.wgwpn(p1, wfqkr), iuiut.ftsnf(wfqkr), iqpel);
			hhkfd.Add(num, sshChannel);
			return sshChannel;
		}
	}

	private bool synnq(out SshChannel p0, SshChannel p1)
	{
		if (rlefr == SshState.Closed)
		{
			p0 = null;
			return true;
		}
		if (p1.State == SshChannelState.None || 1 == 0)
		{
			p0 = null;
			return false;
		}
		p0 = p1;
		return true;
	}

	private SshChannel mfnrn(SshChannelType p0, int p1, params object[] p2)
	{
		try
		{
			lock (pjfyi)
			{
				if (!qqksm() || 1 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Cannot perform requested operation in current session state.");
				}
			}
			SshChannel sshChannel = hkiur(p0, p1);
			int maxPacketSize = Math.Max(16384, p1 - 2048);
			yxshh yxshh = new yxshh(sshChannel, maxPacketSize, p2);
			cnfnb(LogLevel.Debug, "Opening channel '{0}' (initial window size: {1}, max packet size: {2}).", yxshh.kixek, yxshh.fgjcv, yxshh.efdcm);
			adxeo(yxshh);
			sshChannel = lrcya<SshChannel, SshChannel>(synnq, sshChannel);
			if (sshChannel == null || 1 == 0)
			{
				throw new SshException(SshExceptionStatus.OperationFailure, "The request has failed.");
			}
			if (sshChannel.forqe != null && 0 == 0)
			{
				throw sshChannel.forqe;
			}
			return sshChannel;
		}
		catch (Exception p3)
		{
			oftwj(p3);
			throw;
		}
	}

	public SshChannel OpenChannel(SshChannelType type, int bufferSize)
	{
		if (type != SshChannelType.Session && 0 == 0)
		{
			throw new SshException(SshExceptionStatus.OperationFailure, "This method can only open session channels.");
		}
		return mfnrn(type, bufferSize);
	}

	public SshChannel OpenSession()
	{
		return mfnrn(SshChannelType.Session, 131072);
	}

	public SshChannel OpenTcpIpTunnel(IPEndPoint remoteEP)
	{
		return mfnrn(SshChannelType.DirectTcpIp, 131072, remoteEP.Address.ToString(), remoteEP.Port);
	}

	public SshChannel OpenTcpIpTunnel(string hostname, int port)
	{
		return mfnrn(SshChannelType.DirectTcpIp, 131072, hostname, port);
	}

	public void KeepAlive()
	{
		upraz p = new upraz(null);
		ialsm(p, p1: true);
	}

	private bool avgak(out object[] p0, yibwq p1)
	{
		p0 = null;
		if (!p1.lrugr || 1 == 0)
		{
			return false;
		}
		p0 = p1.nnfen;
		return true;
	}

	private object[] rmung(fnofn p0, bvkts p1)
	{
		yibwq yibwq = new yibwq(p0, p1);
		lock (gniwx.SyncRoot)
		{
			gniwx.Enqueue(yibwq);
			adxeo(p0);
		}
		object[] array = lrcya<object[], yibwq>(avgak, yibwq);
		if (array == null || 1 == 0)
		{
			throw new SshException(SshExceptionStatus.OperationFailure, "The request has failed.");
		}
		return array;
	}

	public SshForwardingHandle StartTcpIpForward(string address, int port)
	{
		if (address == null || 1 == 0)
		{
			throw new ArgumentNullException("address");
		}
		if (port < 0 || port > 65535)
		{
			throw hifyx.nztrs("port", port, "Port is out of range of valid values.");
		}
		try
		{
			if (port != 0 && 0 == 0)
			{
				lock (okxuk)
				{
					if (okxuk.ContainsKey(port) && 0 == 0)
					{
						throw new SshException(SshExceptionStatus.OperationFailure, "The specified port is already forwarder.");
					}
				}
			}
			if (gpxwl == 0 || 1 == 0)
			{
				gpxwl = Environment.TickCount % 3000;
			}
			int num;
			int num2;
			if ((port == 0 || 1 == 0) && dtgnk && 0 == 0)
			{
				num = 0;
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_00d1;
				}
				goto IL_017b;
			}
			fnofn p = new fnofn("tcpip-forward", true, address, port);
			bvkts bvkts;
			if (port == 0 || 1 == 0)
			{
				bvkts = bvkts.frokg;
				if (bvkts != bvkts.plpbq)
				{
					goto IL_01d8;
				}
			}
			bvkts = bvkts.cpoos;
			goto IL_01d8;
			IL_01d8:
			object[] array = rmung(p, bvkts);
			if (array == null || 1 == 0)
			{
				throw new SshException(SshExceptionStatus.OperationFailure, "Unable to start port forwarding.");
			}
			if (port == 0 || 1 == 0)
			{
				port = (int)(uint)array[0];
			}
			goto IL_0214;
			IL_017b:
			if (num2 < 16)
			{
				goto IL_00d1;
			}
			goto IL_0183;
			IL_0183:
			if (port == 0 || 1 == 0)
			{
				throw new SshException(SshExceptionStatus.OperationFailure, "Unable to start port forwarding.");
			}
			goto IL_0214;
			IL_00d1:
			if (num < 1024)
			{
				int num3 = gpxwl + 50000;
				gpxwl = (gpxwl + 7) % 9997;
				lock (okxuk)
				{
					if (okxuk.ContainsKey(num3) && 0 == 0)
					{
						num2--;
						num++;
						goto IL_0177;
					}
				}
				fnofn p2 = new fnofn("tcpip-forward", true, address, num3);
				object[] array2 = rmung(p2, bvkts.cpoos);
				if (array2 == null)
				{
					goto IL_0177;
				}
				port = num3;
			}
			goto IL_0183;
			IL_0177:
			num2++;
			goto IL_017b;
			IL_0214:
			lock (okxuk)
			{
				okxuk.TryGetValue(port, out var value);
				if (value == null || 1 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "Unable to start port forwarding.");
				}
				value.Address = address;
				value.Port = port;
				return value;
			}
		}
		catch (Exception p3)
		{
			oftwj(p3);
			throw;
		}
	}

	private void ujchj(SshForwardingHandle p0)
	{
		lock (okxuk)
		{
			okxuk.TryGetValue(p0.Port, out var value);
			if (p0 != value)
			{
				throw new SshException(SshExceptionStatus.OperationFailure, "The specified forwarding handle is no longer valid.");
			}
		}
	}

	public void StopTcpIpForward(SshForwardingHandle handle)
	{
		if (handle == null || 1 == 0)
		{
			throw new ArgumentNullException("handle");
		}
		try
		{
			ujchj(handle);
			okxuk.Remove(handle.Port);
			while (true)
			{
				yxshh yxshh = handle.herzj();
				if (yxshh == null)
				{
					break;
				}
				jgcsx p = new jgcsx(yxshh.iixpk, 0u, "");
				adxeo(p);
			}
			fnofn p2 = new fnofn("cancel-tcpip-forward", true, handle.Address, handle.Port);
			object[] array = rmung(p2, bvkts.plpbq);
			if (array == null || 1 == 0)
			{
				throw new SshException(SshExceptionStatus.OperationFailure, "Unable to stop port forwarding.");
			}
		}
		catch (Exception p3)
		{
			oftwj(p3);
			throw;
		}
	}

	internal void vjabb(yxshh p0)
	{
		jgcsx p1 = new jgcsx(p0.iixpk, 1u, "Request rejected");
		ialsm(p1, p1: true);
	}

	internal SshChannel umiyr(yxshh p0)
	{
		SshChannel sshChannel = hkiur(SshChannelType.ForwardedTcpIp, 131072);
		yvedn p1 = sshChannel.oassp(p0);
		adxeo(p1);
		return sshChannel;
	}

	private static bool ubkhh(out yxshh p0, SshForwardingHandle p1)
	{
		p0 = p1.herzj();
		return p0 != null;
	}

	public SshChannel AcceptTcpIpForward(SshForwardingHandle handle, int timeout)
	{
		if (handle == null || 1 == 0)
		{
			throw new ArgumentNullException("handle");
		}
		try
		{
			ujchj(handle);
			yxshh yxshh = ryvxm<yxshh, SshForwardingHandle>(ubkhh, timeout, gvyyw.zcykt, handle, null, null);
			if (yxshh == null || 1 == 0)
			{
				return null;
			}
			return umiyr(yxshh);
		}
		catch (Exception p)
		{
			oftwj(p);
			throw;
		}
	}

	public SshChannel AcceptTcpIpForward(SshForwardingHandle handle)
	{
		return AcceptTcpIpForward(handle, -1);
	}

	public ISocketFactory ToSocketFactory()
	{
		lock (pjfyi)
		{
			if (rcwyn == null || 1 == 0)
			{
				rcwyn = new gmumv(this);
			}
			return rcwyn;
		}
	}

	private static void ixmuk(Exception p0)
	{
	}

	private void mgyfh(Exception p0)
	{
		cnfnb(LogLevel.Error, "Error while dispatching postponed packet: {0}", p0);
	}
}
