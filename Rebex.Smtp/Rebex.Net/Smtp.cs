using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Rebex.Mail;
using Rebex.Mime;
using Rebex.Mime.Headers;
using Rebex.Security.Authentication;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Net;

public class Smtp : NetworkSession, rlpyd, IDisposable
{
	private enum qgzes
	{
		nwsxa,
		kaema,
		cexhk,
		uvvua,
		bcbzx,
		rbiid,
		auvji,
		fipmb,
		dsuml,
		mrmuh,
		vtdxh,
		whlpx,
		nehnv,
		csxiu,
		drqtr,
		uzpuu,
		pxfwq,
		zgqju,
		lffeu
	}

	private sealed class vektr
	{
		public IPHostEntry asdtn;

		public Exception porhw;

		public ManualResetEvent rqcje;

		public string chdso;

		public void eyxvv(IAsyncResult p0)
		{
			try
			{
				zippk(LogLevel.Debug, "SMTP", "Finishing Dns.GetHostEntry for '{0}'.", chdso);
				asdtn = auilw.dpbpb(p0);
				zippk(LogLevel.Debug, "SMTP", "Retrieved HostEntry with {1} addresses for '{0}'.", chdso, asdtn.AddressList.Length);
			}
			catch (SocketException ex)
			{
				int num = ex.skehp();
				zippk(LogLevel.Debug, "SMTP", "Socket error {2} occurred for '{0}': {1}", chdso, ex, num);
				int num2 = num;
				if (num2 == 10109 || num2 == 11001 || num2 == 11004)
				{
					asdtn = new IPHostEntry();
				}
				else
				{
					porhw = new SmtpException(auilw.jczqg(num), ex, SmtpExceptionStatus.NameResolutionFailure);
				}
			}
			catch (Exception ex2)
			{
				zippk(LogLevel.Debug, "SMTP", "An exception occurred for '{0}': {1}", chdso, ex2);
				porhw = new SmtpException("Unable to resolve domain.", ex2, SmtpExceptionStatus.NameResolutionFailure);
			}
			finally
			{
				rqcje.Set();
			}
		}
	}

	public const int DefaultPort = 25;

	public const int DefaultImplicitSslPort = 465;

	public const int AlternativeExplicitSslPort = 587;

	internal const string jyzdy = "Smtp";

	private const int euooi = 16384;

	private Encoding mhmxd = Encoding.UTF8;

	private SmtpSettings opmvy;

	private int ludkc = 60000;

	private int azgjl = 3000;

	private ISocketFactory bljbu;

	private object clkzc = new object();

	private SmtpExtensions cgfdl = SmtpExtensions.All;

	private string kqadm;

	private DeliveryStatusNotificationConditions epdro = DeliveryStatusNotificationConditions.Failure;

	private DeliveryStatusNotificationOriginalMessageMethod hrzrm;

	private readonly byte[] rysxu = new byte[98304];

	private readonly byte[] ewuzd = new byte[5] { 13, 10, 46, 13, 10 };

	private zwrli wmcaj;

	private SmtpState edlng;

	private SmtpTransferState sreio;

	private ubdvb necug;

	private ailrt hneot;

	private bool ekbih;

	private bool govro;

	private int pmkhl;

	private bool iqwmr;

	private bool jleud;

	private string dxtrz;

	private SmtpAuthentication[] rqhxv;

	private SmtpAuthentication[] lotqk;

	private SmtpExtensions rjbbu;

	private string zovug;

	private int ocjtq;

	private bool ykyyc;

	private TlsDebugLevel nozao = TlsDebugLevel.Important;

	private EventHandler<TlsDebugEventArgs> tuvqw;

	private EventHandler<SmtpResponseReadEventArgs> opjxr;

	private EventHandler<SmtpCommandSentEventArgs> rvozw;

	private EventHandler<SmtpStateChangedEventArgs> xzteq;

	private EventHandler<SmtpTransferProgressEventArgs> mzphe;

	private EventHandler<SmtpRejectedRecipientEventArgs> fjqqr;

	private EventHandler<SmtpSendingMessageEventArgs> ktscm;

	private EventHandler<SslCertificateValidationEventArgs> gjeyg;

	private static Func<string, Exception> dcwar;

	internal int ixikc => base.InstanceId;

	private bool jeaws => gjeyg != null;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("TlsDebug event API has been deprecated and will be removed. Use LogWriter instead.", true)]
	public TlsDebugLevel TlsDebugLevel
	{
		get
		{
			return nozao;
		}
		set
		{
			lock (this)
			{
				nozao = value;
				if (wmcaj != null && 0 == 0)
				{
					wmcaj.rjtyx(value, tuvqw);
				}
			}
		}
	}

	private bool jmzhh
	{
		get
		{
			lock (clkzc)
			{
				return govro;
			}
		}
	}

	public Proxy Proxy
	{
		get
		{
			lock (clkzc)
			{
				if (bljbu == null || 1 == 0)
				{
					bljbu = new Proxy();
				}
				return bljbu as Proxy;
			}
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			lock (clkzc)
			{
				bljbu = value;
			}
		}
	}

	public Encoding Encoding
	{
		get
		{
			return mhmxd;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			lock (clkzc)
			{
				mhmxd = value;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Options property has been deprecated. Please use Settings property instead.", false)]
	[wptwl(false)]
	public SmtpOptions Options
	{
		get
		{
			return opmvy.mvylp();
		}
		set
		{
			if (opmvy.rvdnp > 1)
			{
				throw new InvalidOperationException("Invalid call of old API. Use the Settings properties instead.");
			}
			lock (clkzc)
			{
				opmvy.civyv(value);
			}
		}
	}

	public SmtpSettings Settings
	{
		get
		{
			return opmvy;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value", "Value cannot be null.");
			}
			lock (clkzc)
			{
				opmvy = value;
				opmvy.rvdnp++;
			}
		}
	}

	private SslSettings bqpjz => Settings;

	public SmtpExtensions SupportedExtensions
	{
		get
		{
			lock (clkzc)
			{
				zdiys();
				return rjbbu;
			}
		}
	}

	public SmtpExtensions EnabledExtensions
	{
		get
		{
			return cgfdl;
		}
		set
		{
			lock (clkzc)
			{
				cgfdl = value;
			}
		}
	}

	public bool IsSecured
	{
		get
		{
			lock (clkzc)
			{
				zdiys();
				return ykyyc;
			}
		}
	}

	public TlsSocket TlsSocket
	{
		get
		{
			lock (clkzc)
			{
				zdiys();
				return wmcaj.xfnnf;
			}
		}
	}

	public int Timeout
	{
		get
		{
			return ludkc;
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
				value = 1000;
			}
			lock (clkzc)
			{
				ludkc = value;
			}
		}
	}

	public int AbortTimeout
	{
		get
		{
			return azgjl;
		}
		set
		{
			if (value < 1000)
			{
				value = 1000;
			}
			lock (clkzc)
			{
				azgjl = value;
			}
		}
	}

	public SmtpState State => edlng;

	public EndPoint LocalEndPoint
	{
		get
		{
			lock (clkzc)
			{
				zdiys();
				return wmcaj.iapdj;
			}
		}
	}

	public EndPoint RemoteEndPoint
	{
		get
		{
			lock (clkzc)
			{
				zdiys();
				return wmcaj.vqncl;
			}
		}
	}

	public override bool IsConnected => iqwmr;

	public override bool IsAuthenticated => jleud;

	public DeliveryStatusNotificationConditions DeliveryStatusNotificationConditions
	{
		get
		{
			return epdro;
		}
		set
		{
			lock (clkzc)
			{
				epdro = value;
			}
		}
	}

	public DeliveryStatusNotificationOriginalMessageMethod DeliveryStatusNotificationOriginalMessageMethod
	{
		get
		{
			return hrzrm;
		}
		set
		{
			lock (clkzc)
			{
				hrzrm = value;
			}
		}
	}

	public int MaxMailSize
	{
		get
		{
			lock (clkzc)
			{
				zdiys();
				return Math.Max(0, ocjtq);
			}
		}
	}

	public string ServerDomain
	{
		get
		{
			lock (clkzc)
			{
				zdiys();
				return zovug;
			}
		}
	}

	public string ClientDomain
	{
		get
		{
			return kqadm;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			value = value.Trim();
			if (value.Length == 0 || 1 == 0)
			{
				throw new ArgumentException("String cannot have zero length.", "value");
			}
			lock (clkzc)
			{
				kqadm = value;
			}
		}
	}

	public bool IsBusy => ekbih;

	public static Version Version => dahxy.clyhv(typeof(Smtp));

	public event EventHandler<SmtpResponseReadEventArgs> ResponseRead
	{
		add
		{
			EventHandler<SmtpResponseReadEventArgs> eventHandler = opjxr;
			EventHandler<SmtpResponseReadEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpResponseReadEventArgs> value2 = (EventHandler<SmtpResponseReadEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref opjxr, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SmtpResponseReadEventArgs> eventHandler = opjxr;
			EventHandler<SmtpResponseReadEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpResponseReadEventArgs> value2 = (EventHandler<SmtpResponseReadEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref opjxr, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SmtpCommandSentEventArgs> CommandSent
	{
		add
		{
			EventHandler<SmtpCommandSentEventArgs> eventHandler = rvozw;
			EventHandler<SmtpCommandSentEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpCommandSentEventArgs> value2 = (EventHandler<SmtpCommandSentEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref rvozw, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SmtpCommandSentEventArgs> eventHandler = rvozw;
			EventHandler<SmtpCommandSentEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpCommandSentEventArgs> value2 = (EventHandler<SmtpCommandSentEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref rvozw, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SmtpStateChangedEventArgs> StateChanged
	{
		add
		{
			EventHandler<SmtpStateChangedEventArgs> eventHandler = xzteq;
			EventHandler<SmtpStateChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpStateChangedEventArgs> value2 = (EventHandler<SmtpStateChangedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref xzteq, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SmtpStateChangedEventArgs> eventHandler = xzteq;
			EventHandler<SmtpStateChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpStateChangedEventArgs> value2 = (EventHandler<SmtpStateChangedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref xzteq, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SmtpTransferProgressEventArgs> TransferProgress
	{
		add
		{
			EventHandler<SmtpTransferProgressEventArgs> eventHandler = mzphe;
			EventHandler<SmtpTransferProgressEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpTransferProgressEventArgs> value2 = (EventHandler<SmtpTransferProgressEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref mzphe, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SmtpTransferProgressEventArgs> eventHandler = mzphe;
			EventHandler<SmtpTransferProgressEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpTransferProgressEventArgs> value2 = (EventHandler<SmtpTransferProgressEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref mzphe, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SmtpRejectedRecipientEventArgs> RejectedRecipient
	{
		add
		{
			EventHandler<SmtpRejectedRecipientEventArgs> eventHandler = fjqqr;
			EventHandler<SmtpRejectedRecipientEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpRejectedRecipientEventArgs> value2 = (EventHandler<SmtpRejectedRecipientEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref fjqqr, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SmtpRejectedRecipientEventArgs> eventHandler = fjqqr;
			EventHandler<SmtpRejectedRecipientEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpRejectedRecipientEventArgs> value2 = (EventHandler<SmtpRejectedRecipientEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref fjqqr, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SmtpSendingMessageEventArgs> SendingMessage
	{
		add
		{
			EventHandler<SmtpSendingMessageEventArgs> eventHandler = ktscm;
			EventHandler<SmtpSendingMessageEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpSendingMessageEventArgs> value2 = (EventHandler<SmtpSendingMessageEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ktscm, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SmtpSendingMessageEventArgs> eventHandler = ktscm;
			EventHandler<SmtpSendingMessageEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SmtpSendingMessageEventArgs> value2 = (EventHandler<SmtpSendingMessageEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ktscm, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<SslCertificateValidationEventArgs> ValidatingCertificate
	{
		add
		{
			EventHandler<SslCertificateValidationEventArgs> eventHandler = gjeyg;
			EventHandler<SslCertificateValidationEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SslCertificateValidationEventArgs> value2 = (EventHandler<SslCertificateValidationEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref gjeyg, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SslCertificateValidationEventArgs> eventHandler = gjeyg;
			EventHandler<SslCertificateValidationEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SslCertificateValidationEventArgs> value2 = (EventHandler<SslCertificateValidationEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref gjeyg, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("TlsDebug event API has been deprecated and will be removed. Use LogWriter instead.", true)]
	public event EventHandler<TlsDebugEventArgs> TlsDebug
	{
		add
		{
			lock (this)
			{
				tuvqw = (EventHandler<TlsDebugEventArgs>)Delegate.Combine(tuvqw, value);
				if (wmcaj != null && 0 == 0)
				{
					wmcaj.rjtyx(nozao, tuvqw);
				}
			}
		}
		remove
		{
			lock (this)
			{
				tuvqw = (EventHandler<TlsDebugEventArgs>)Delegate.Remove(tuvqw, value);
				if (wmcaj != null && 0 == 0)
				{
					wmcaj.rjtyx(nozao, tuvqw);
				}
			}
		}
	}

	private void ohbjh<T>(Action<T> p0, T p1)
	{
		if (hneot == null || 1 == 0)
		{
			p0(p1);
		}
		else
		{
			hneot.lgcuq(p0, p1);
		}
	}

	private void qfcsw<T>(Action<T> p0, T p1)
	{
		if (hneot == null || 1 == 0)
		{
			p0(p1);
		}
		else
		{
			hneot.icidn(p0, p1);
		}
	}

	protected virtual void OnResponseRead(SmtpResponseReadEventArgs e)
	{
		if (opjxr != null && 0 == 0)
		{
			opjxr(this, e);
		}
	}

	protected virtual void OnCommandSent(SmtpCommandSentEventArgs e)
	{
		if (rvozw != null && 0 == 0)
		{
			rvozw(this, e);
		}
	}

	protected virtual void OnStateChanged(SmtpStateChangedEventArgs e)
	{
		if (xzteq != null && 0 == 0)
		{
			xzteq(this, e);
		}
	}

	private static string kjqal(SmtpState p0)
	{
		return p0 switch
		{
			SmtpState.Disconnected => "Disconnected", 
			SmtpState.Connecting => "Connecting", 
			SmtpState.Ready => "Ready", 
			SmtpState.Sending => "Sending", 
			SmtpState.Reading => "Reading", 
			SmtpState.Processing => "Processing", 
			SmtpState.Pipelining => "Pipelining", 
			SmtpState.Disposed => "Disposed", 
			_ => "None", 
		};
	}

	private void msrsv(SmtpState p0, SmtpState p1)
	{
		if (base.twmrq.Level <= LogLevel.Debug)
		{
			olfku(LogLevel.Debug, "Info", "State changed from '{0}' to '{1}'.", kjqal(p0), kjqal(p1));
		}
		if (xzteq != null && 0 == 0)
		{
			qfcsw(OnStateChanged, new SmtpStateChangedEventArgs(p0, p1));
		}
	}

	protected virtual void OnTransferProgress(SmtpTransferProgressEventArgs e)
	{
		if (mzphe != null && 0 == 0)
		{
			mzphe(this, e);
		}
	}

	private void ufqdh(long p0, byte[] p1, int p2, int p3, long p4)
	{
		if (mzphe != null && 0 == 0)
		{
			if (!opmvy.ReportTransferredData || 1 == 0)
			{
				qfcsw(OnTransferProgress, new SmtpTransferProgressEventArgs(sreio, p0, p3, p4, null, 0));
			}
			else
			{
				ohbjh(OnTransferProgress, new SmtpTransferProgressEventArgs(sreio, p0, p3, p4, p1, p2));
			}
		}
	}

	private void rjzyh(SmtpRejectedRecipientEventArgs p0)
	{
		if (fjqqr != null && 0 == 0)
		{
			fjqqr(this, p0);
		}
	}

	private bool ubhoc(string p0, SmtpResponse p1)
	{
		if (fjqqr == null || 1 == 0)
		{
			return false;
		}
		SmtpRejectedRecipientEventArgs e = new SmtpRejectedRecipientEventArgs(p0, p1.sxztf(), ignore: false);
		ohbjh(rjzyh, e);
		return e.Ignore;
	}

	private void ajhyn(string p0)
	{
		if (LogWriter != null && 0 == 0)
		{
			rmwyv(LogLevel.Info, "Command", p0);
		}
		if (rvozw != null && 0 == 0)
		{
			qfcsw(OnCommandSent, new SmtpCommandSentEventArgs(p0));
		}
	}

	internal void zotwr(string p0)
	{
		if (LogWriter != null && 0 == 0)
		{
			rmwyv(LogLevel.Info, "Response", p0);
		}
		if (opjxr != null && 0 == 0)
		{
			qfcsw(OnResponseRead, new SmtpResponseReadEventArgs(p0));
		}
	}

	private void axsof(MimeMessage p0, Stream p1)
	{
		if (ktscm != null && 0 == 0)
		{
			ohbjh(boruq, new SmtpSendingMessageEventArgs(p0, p1));
		}
	}

	private void kqfsy(SslCertificateValidationEventArgs p0)
	{
		EventHandler<SslCertificateValidationEventArgs> eventHandler = gjeyg;
		if (eventHandler != null && 0 == 0)
		{
			eventHandler(this, p0);
		}
	}

	private void fqzvk(SslCertificateValidationEventArgs p0)
	{
		if (((rlpyd)this).clxxv && 0 == 0)
		{
			ohbjh(kqfsy, p0);
		}
	}

	void rlpyd.pyugx(SslCertificateValidationEventArgs p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fqzvk
		this.fqzvk(p0);
	}

	private void boruq(SmtpSendingMessageEventArgs p0)
	{
		if (ktscm != null && 0 == 0)
		{
			ktscm(this, p0);
		}
	}

	internal static void zippk(LogLevel p0, string p1, string p2, params object[] p3)
	{
		ILogWriter defaultLogWriter = NetworkSession.DefaultLogWriter;
		if (defaultLogWriter != null && 0 == 0 && defaultLogWriter.Level <= p0)
		{
			defaultLogWriter.Write(message: (p3 != null && 0 == 0 && ((p3.Length != 0) ? true : false)) ? brgjd.edcru(p2, p3) : p2, level: p0, objectType: typeof(Smtp), objectId: 0, area: p1);
		}
	}

	[Obsolete("TlsDebug event API has been deprecated and will be removed. Use LogWriter instead.", true)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected virtual void OnTlsDebug(TlsDebugEventArgs e)
	{
		EventHandler<TlsDebugEventArgs> eventHandler = tuvqw;
		if (eventHandler != null && 0 == 0)
		{
			eventHandler(this, e);
		}
	}

	private void xzaqz()
	{
		lock (clkzc)
		{
			if (necug != null && 0 == 0)
			{
				if (necug.jzuxt != Thread.CurrentThread.ManagedThreadId)
				{
					rmwyv(LogLevel.Error, "Info", "Another operation is pending.");
					throw new SmtpException("Another operation is pending.", SmtpExceptionStatus.Pending);
				}
				return;
			}
			if (ekbih && 0 == 0)
			{
				rmwyv(LogLevel.Error, "Info", "Another operation is pending.");
				throw new SmtpException("Another operation is pending.", SmtpExceptionStatus.Pending);
			}
			if (edlng == SmtpState.Disposed)
			{
				throw new ObjectDisposedException("Smtp", "Smtp object was disposed.");
			}
			ekbih = true;
		}
	}

	private void vevwx()
	{
		lock (clkzc)
		{
			if (necug == null)
			{
				if ((!ekbih || 1 == 0) && edlng != SmtpState.Disconnected && 0 == 0)
				{
					rmwyv(LogLevel.Error, "Info", "Smtp object is not locked.");
					throw new InvalidOperationException("Smtp object is not locked.");
				}
				ekbih = false;
				govro = false;
			}
		}
	}

	private void agxiv(SmtpTransferState p0)
	{
		if (sreio != p0)
		{
			lock (clkzc)
			{
				sreio = p0;
			}
			ufqdh(0L, null, 0, 0, 0L);
		}
	}

	private void taivh(SmtpState p0)
	{
		if (edlng == p0)
		{
			return;
		}
		SmtpState smtpState = edlng;
		switch (smtpState)
		{
		case SmtpState.Disposed:
			throw new ObjectDisposedException("Smtp");
		case SmtpState.Disconnected:
			if (p0 != SmtpState.Connecting && p0 != SmtpState.Disposed)
			{
				throw new InvalidOperationException("Not connected to the server.");
			}
			edlng = p0;
			msrsv(smtpState, p0);
			return;
		}
		switch (p0)
		{
		case SmtpState.Connecting:
			throw new InvalidOperationException("Already connected to the server.");
		case SmtpState.Sending:
			if (smtpState != SmtpState.Ready && smtpState != SmtpState.Processing && smtpState != SmtpState.Pipelining)
			{
				throw new InvalidOperationException("Cannot send command to the server because the response for previous one was not received.");
			}
			break;
		}
		edlng = p0;
		msrsv(smtpState, p0);
	}

	private void zdiys()
	{
		if (!iqwmr || 1 == 0)
		{
			throw new InvalidOperationException("Not connected to the server.");
		}
	}

	public Smtp()
	{
		dahxy.kdslf();
		kqadm = auilw.ymclc();
		vuuzx();
		Settings = new SmtpSettings();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		lock (clkzc)
		{
			if (edlng != SmtpState.Disposed)
			{
				SmtpState p = edlng;
				edlng = SmtpState.Disposed;
				if (disposing && 0 == 0)
				{
					msrsv(p, SmtpState.Disposed);
				}
			}
		}
		if (disposing && 0 == 0)
		{
			yfdko();
		}
		else
		{
			vuuzx();
		}
	}

	private void yfdko()
	{
		zwrli zwrli = wmcaj;
		if (zwrli != null && 0 == 0)
		{
			zwrli.kldkv();
		}
		iqwmr = false;
		agxiv(SmtpTransferState.None);
		if (edlng != SmtpState.Disposed)
		{
			taivh(SmtpState.Disconnected);
			vuuzx();
		}
	}

	private void vuuzx()
	{
		lock (clkzc)
		{
			wmcaj = null;
			necug = null;
			hneot = null;
			ekbih = false;
			govro = false;
			pmkhl = 0;
			iqwmr = false;
			jleud = false;
			dxtrz = null;
			rqhxv = new SmtpAuthentication[0];
			lotqk = rqhxv;
			rjbbu = (SmtpExtensions)0L;
			zovug = null;
			ocjtq = 0;
			ServerName = null;
			ServerPort = 0;
			UserName = null;
			ykyyc = false;
		}
	}

	~Smtp()
	{
		Dispose(disposing: false);
	}

	private void ijyuq()
	{
		if (!govro || 1 == 0)
		{
			pmkhl = Environment.TickCount;
		}
	}

	internal void leaaz()
	{
		lock (clkzc)
		{
			int num = Environment.TickCount - pmkhl;
			if (ludkc > 0 && num > ludkc)
			{
				throw new SmtpException("Timeout exceeded.", SmtpExceptionStatus.Timeout);
			}
			if (govro && 0 == 0 && azgjl > 0 && num > azgjl)
			{
				throw new SmtpException("The operation was aborted.", SmtpExceptionStatus.OperationAborted);
			}
		}
	}

	internal void nnqer(IAsyncResult p0)
	{
		while (!p0.IsCompleted)
		{
			leaaz();
			Thread.Sleep(1);
		}
	}

	private void tssug(Exception p0)
	{
		if (base.twmrq.Level <= LogLevel.Error)
		{
			rmwyv(LogLevel.Error, "Info", p0.ToString());
		}
		if (edlng == SmtpState.Disposed && (!(p0 is ObjectDisposedException) || 1 == 0))
		{
			throw new SmtpException("Smtp object was disposed while an operation was in progress.", p0, SmtpExceptionStatus.OperationFailure);
		}
	}

	private static IAsyncResult chtfa(bool p0, Smtp p1, qgzes p2, AsyncCallback p3, object p4, params object[] p5)
	{
		if (p1 != null && 0 == 0)
		{
			return p1.fndvk(p0, p2, p3, p4, p5);
		}
		ubdvb ubdvb = new ubdvb(p0, jrzti, null, p2, null, p3, p4, p5);
		ubdvb.nvhuz();
		return ubdvb;
	}

	private IAsyncResult fndvk(bool p0, qgzes p1, AsyncCallback p2, object p3, params object[] p4)
	{
		lock (clkzc)
		{
			if ((ekbih ? true : false) || necug != null)
			{
				throw new SmtpException("Another operation is pending.", SmtpExceptionStatus.Pending);
			}
			if (edlng == SmtpState.Disposed)
			{
				throw new ObjectDisposedException("Smtp", "Smtp object was disposed.");
			}
			ubdvb ubdvb = new ubdvb(p0, jrzti, this, p1, null, p2, p3, p4);
			ekbih = true;
			necug = ubdvb;
			hneot = ailrt.dvgbg();
			ubdvb.nvhuz();
			return ubdvb;
		}
	}

	private static object auoks(IAsyncResult p0, Smtp p1, string p2, out Exception p3, params Enum[] p4)
	{
		ubdvb ubdvb = ubdvb.ovklt(p0, p1, p2, p4);
		if (p1 != null && 0 == 0)
		{
			lock (p1.clkzc)
			{
				p1.necug = null;
				p1.hneot = null;
				p1.ekbih = false;
				p1.govro = false;
			}
		}
		p3 = ubdvb.oybzl;
		return ubdvb.dcenx;
	}

	private static object sufpd(IAsyncResult p0, Smtp p1, string p2, params Enum[] p3)
	{
		Exception p4;
		object result = auoks(p0, p1, p2, out p4, p3);
		if (p4 != null && 0 == 0)
		{
			if (p4 is SmtpException e && 0 == 0)
			{
				throw new SmtpException(e);
			}
			throw new SmtpException("Asynchronous call exception.", p4, SmtpExceptionStatus.AsyncError);
		}
		return result;
	}

	public void Abort()
	{
		lock (clkzc)
		{
			if (!ekbih || 1 == 0)
			{
				rmwyv(LogLevel.Info, "Info", "Not aborting operation, nothing to abort.");
				return;
			}
			if (govro && 0 == 0)
			{
				rmwyv(LogLevel.Info, "Info", "Already aborting operation.");
				return;
			}
			rmwyv(LogLevel.Info, "Info", "Aborting operation.");
			ijyuq();
			govro = true;
		}
	}

	private SmtpResponse vbxqs(string p0)
	{
		xzaqz();
		try
		{
			zdiys();
			fiwtd(p0);
			return kdcdj(2);
		}
		catch (Exception p1)
		{
			tssug(p1);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	private void fiwtd(string p0)
	{
		jvzip(p0, null);
	}

	private void jvzip(string p0, string p1)
	{
		try
		{
			if (edlng != SmtpState.Pipelining)
			{
				taivh(SmtpState.Sending);
			}
			ijyuq();
			byte[] bytes = Encoding.GetBytes(p0 + "\r\n");
			IDisposable disposable = base.twmrq.cllqt(p1 != null);
			try
			{
				wmcaj.bhjds(bytes, 0, bytes.Length);
			}
			finally
			{
				if (disposable != null && 0 == 0)
				{
					disposable.Dispose();
				}
			}
			if (p1 != null && 0 == 0)
			{
				ajhyn(p1);
			}
			else
			{
				ajhyn(p0);
			}
			if (edlng != SmtpState.Pipelining)
			{
				taivh(SmtpState.Reading);
			}
		}
		catch (SmtpException ex)
		{
			if (ex.Status == SmtpExceptionStatus.ConnectionClosed)
			{
				yfdko();
			}
			throw;
		}
		catch (SocketException ex2)
		{
			yfdko();
			throw new SmtpException(ex2.Message, ex2, SmtpExceptionStatus.SendFailure);
		}
	}

	private SmtpResponse kdcdj(int p0)
	{
		return bkglx(p0, p1: true);
	}

	private SmtpResponse bkglx(int p0, bool p1)
	{
		switch (edlng)
		{
		case SmtpState.Disposed:
			throw new ObjectDisposedException("Smtp");
		default:
			throw new InvalidOperationException("No response from the server is expected.");
		case SmtpState.Reading:
		case SmtpState.Pipelining:
		{
			SmtpResponse smtpResponse;
			try
			{
				ijyuq();
				while (true)
				{
					smtpResponse = wmcaj.tzkjp();
					if (smtpResponse == null)
					{
						if (!p1 || 1 == 0)
						{
							return null;
						}
						leaaz();
						Thread.Sleep(1);
						continue;
					}
					break;
				}
			}
			catch (SmtpException ex)
			{
				if (ex.Status == SmtpExceptionStatus.ConnectionClosed)
				{
					yfdko();
				}
				throw;
			}
			catch (SocketException ex2)
			{
				yfdko();
				throw new SmtpException(ex2.Message, ex2, SmtpExceptionStatus.SocketError);
			}
			if (edlng != SmtpState.Pipelining && smtpResponse.Group != 1)
			{
				if (smtpResponse.Group == 3)
				{
					taivh(SmtpState.Processing);
				}
				else
				{
					taivh(SmtpState.Ready);
				}
			}
			if ((rjbbu & cgfdl & SmtpExtensions.EnhancedStatusCodes) != 0)
			{
				smtpResponse.gogbt();
			}
			smtpResponse = smtpResponse.sxztf();
			if (smtpResponse.Group == p0 || p0 == 0 || 1 == 0)
			{
				return smtpResponse;
			}
			throw new SmtpException(smtpResponse);
		}
		}
	}

	private void ytvsb()
	{
		taivh(SmtpState.Sending);
		fiwtd("RSET");
		kdcdj(2);
	}

	private void rrllx(mtlib p0, ref long p1, long p2)
	{
		int num = p0.lfdxc(rysxu, 0, rysxu.Length);
		string text = "BDAT " + num;
		if (p0.kqoib && 0 == 0)
		{
			text += " LAST";
		}
		fiwtd(text);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0053;
		}
		goto IL_0095;
		IL_0053:
		int num3 = Math.Min(16384, num - num2);
		wmcaj.bhjds(rysxu, num2, num3);
		p1 += num3;
		ufqdh(p1, rysxu, num2, num3, p2);
		num2 += num3;
		goto IL_0095;
		IL_0095:
		if (num2 >= num)
		{
			return;
		}
		goto IL_0053;
	}

	private void drxnj(mtlib p0, long p1)
	{
		if (edlng != SmtpState.Pipelining)
		{
			taivh(SmtpState.Sending);
		}
		agxiv(SmtpTransferState.Sending);
		try
		{
			long num = 0L;
			do
			{
				int num2 = p0.lfdxc(rysxu, 0, 16384);
				wmcaj.bhjds(rysxu, 0, num2);
				num += num2;
				ufqdh(num, rysxu, 0, num2, p1);
			}
			while (!p0.kqoib);
			if (p0.zpiat && 0 == 0)
			{
				wmcaj.bhjds(ewuzd, 0, 5);
				num += 5;
				ufqdh(num, ewuzd, 0, 5, p1);
			}
			else
			{
				wmcaj.bhjds(ewuzd, 2, 3);
				num += 3;
				ufqdh(num, ewuzd, 2, 3, p1);
			}
		}
		finally
		{
			agxiv(SmtpTransferState.None);
		}
		if (edlng != SmtpState.Pipelining)
		{
			taivh(SmtpState.Reading);
		}
		kdcdj(2);
	}

	private void urlzs(string p0, string p1)
	{
		fiwtd("MAIL FROM:<" + p0 + ">" + p1);
		SmtpResponse smtpResponse = kdcdj(0);
		if (smtpResponse.Group != 2)
		{
			if (smtpResponse.Code == 452 || smtpResponse.Code == 552)
			{
				ytvsb();
			}
			throw new SmtpException(smtpResponse);
		}
	}

	private void qyzgc(bool p0, string[] p1, mtlib p2, string p3, long p4)
	{
		ArrayList arrayList = new ArrayList();
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0014;
		}
		goto IL_0094;
		IL_0014:
		if (jmzhh && 0 == 0)
		{
			ytvsb();
			throw new SmtpException("The operation was aborted.", SmtpExceptionStatus.OperationAborted);
		}
		fiwtd("RCPT TO:<" + p1[num2] + ">" + p3);
		SmtpResponse smtpResponse = kdcdj(0);
		if (smtpResponse.Group != 2)
		{
			if (!ubhoc(p1[num2], smtpResponse) || 1 == 0)
			{
				arrayList.Add(new SmtpRejectedRecipient(p1[num2], smtpResponse.sxztf()));
			}
		}
		else
		{
			num++;
		}
		num2++;
		goto IL_0094;
		IL_0094:
		if (num2 >= p1.Length)
		{
			if (arrayList.Count > 0)
			{
				ytvsb();
				throw new SmtpException("One or more recipients rejected. Call SmtpException.GetRejectedRecipients() to get a collection of rejected email addresses.", SmtpExceptionStatus.OperationFailure, (SmtpRejectedRecipient[])arrayList.ToArray(typeof(SmtpRejectedRecipient)));
			}
			if (num == 0 || 1 == 0)
			{
				ytvsb();
				return;
			}
			if (jmzhh && 0 == 0)
			{
				ytvsb();
				throw new SmtpException("The operation was aborted.", SmtpExceptionStatus.OperationAborted);
			}
			if (!p0 || 1 == 0)
			{
				fiwtd("DATA");
				kdcdj(3);
				drxnj(p2, p4);
				return;
			}
			long p5 = 0L;
			agxiv(SmtpTransferState.Sending);
			try
			{
				do
				{
					rrllx(p2, ref p5, p4);
					SmtpResponse smtpResponse2 = kdcdj(0);
					if (smtpResponse2.Group != 2)
					{
						ytvsb();
						throw new SmtpException(smtpResponse2);
					}
					if (jmzhh && 0 == 0)
					{
						ytvsb();
						throw new SmtpException("The operation was aborted.", SmtpExceptionStatus.OperationAborted);
					}
				}
				while (!p2.kqoib);
				return;
			}
			finally
			{
				agxiv(SmtpTransferState.None);
			}
		}
		goto IL_0014;
	}

	private void orquw(bool p0, string[] p1, mtlib p2, string p3, long p4)
	{
		taivh(SmtpState.Pipelining);
		ArrayList arrayList = new ArrayList();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_001d;
		}
		goto IL_00a5;
		IL_001d:
		if (!jmzhh || 1 == 0)
		{
			fiwtd("RCPT TO:<" + p1[num3] + ">" + p3);
			SmtpResponse smtpResponse = bkglx(0, p1: false);
			if (smtpResponse != null && 0 == 0)
			{
				num2++;
				if (smtpResponse.Group != 2)
				{
					if (!ubhoc(p1[num3], smtpResponse) || 1 == 0)
					{
						arrayList.Add(new SmtpRejectedRecipient(p1[num2 - 1], smtpResponse.sxztf()));
					}
				}
				else
				{
					num++;
				}
			}
			num3++;
			goto IL_00a5;
		}
		goto IL_00fc;
		IL_00fc:
		while (num2 < p1.Length)
		{
			SmtpResponse smtpResponse2 = kdcdj(0);
			num2++;
			if (smtpResponse2.Group != 2)
			{
				if (!ubhoc(p1[num2 - 1], smtpResponse2) || 1 == 0)
				{
					arrayList.Add(new SmtpRejectedRecipient(p1[num2 - 1], smtpResponse2.sxztf()));
				}
			}
			else
			{
				num++;
			}
		}
		if (jmzhh && 0 == 0)
		{
			ytvsb();
			throw new SmtpException("The operation was aborted.", SmtpExceptionStatus.OperationAborted);
		}
		if (arrayList.Count > 0)
		{
			ytvsb();
			throw new SmtpException("One or more recipients rejected. Call SmtpException.GetRejectedRecipients() to get a collection of rejected email addresses.", SmtpExceptionStatus.OperationFailure, (SmtpRejectedRecipient[])arrayList.ToArray(typeof(SmtpRejectedRecipient)));
		}
		if (num == 0 || 1 == 0)
		{
			ytvsb();
		}
		else if (p0 && 0 == 0)
		{
			agxiv(SmtpTransferState.Sending);
			try
			{
				int num4 = 0;
				long p5 = 0L;
				SmtpException ex = null;
				while (true)
				{
					if ((!p2.kqoib || 1 == 0) && (ex == null || 1 == 0))
					{
						rrllx(p2, ref p5, p4);
						num4++;
					}
					SmtpResponse smtpResponse3 = bkglx(0, p2.kqoib);
					if (smtpResponse3 != null && 0 == 0)
					{
						num4--;
						if (smtpResponse3.Group != 2)
						{
							while (num4 > 0)
							{
								kdcdj(0);
								num4--;
							}
							if (ex == null || 1 == 0)
							{
								ex = new SmtpException(smtpResponse3);
							}
						}
					}
					if ((p2.kqoib ? true : false) || ex != null)
					{
						if (num4 == 0)
						{
							break;
						}
						if (smtpResponse3 == null || 1 == 0)
						{
							Thread.Sleep(10);
						}
					}
					if (jmzhh && 0 == 0)
					{
						ex = new SmtpException("The operation was aborted.", SmtpExceptionStatus.OperationAborted);
					}
				}
				if (ex != null && 0 == 0)
				{
					ytvsb();
					throw ex;
				}
			}
			finally
			{
				agxiv(SmtpTransferState.None);
			}
			taivh(SmtpState.Ready);
		}
		else
		{
			fiwtd("DATA");
			kdcdj(3);
			if (jmzhh && 0 == 0)
			{
				ytvsb();
				throw new SmtpException("The operation was aborted.", SmtpExceptionStatus.OperationAborted);
			}
			drxnj(p2, p4);
			taivh(SmtpState.Ready);
		}
		return;
		IL_00a5:
		if (num3 < p1.Length)
		{
			goto IL_001d;
		}
		goto IL_00fc;
	}

	[evron]
	public string Connect(string serverName)
	{
		return kitaa(serverName, 25, null, SslMode.None);
	}

	[evron]
	public string Connect(string serverName, int serverPort)
	{
		return kitaa(serverName, serverPort, null, SslMode.None);
	}

	[wptwl(false)]
	[Obsolete("This overload of the Connect method has been deprecated and will be removed. Use another variant and specify parameters using Settings object instead.", true)]
	[evron(OldStyleOnly = true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string Connect(string serverName, int serverPort, TlsParameters parameters, SmtpSecurity security)
	{
		return kitaa(serverName, serverPort, parameters, (SslMode)security);
	}

	[evron]
	public string Connect(string serverName, int serverPort, SslMode security)
	{
		return kitaa(serverName, serverPort, null, security);
	}

	[evron]
	public string Connect(string serverName, SslMode security)
	{
		return kitaa(serverName, 0, null, security);
	}

	[evron]
	internal string kitaa(string p0, int p1, TlsParameters p2, SslMode p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("serverName");
		}
		p0 = p0.Trim();
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Hostname cannot be empty.", "serverName");
		}
		if (p1 == 0 || 1 == 0)
		{
			p1 = p3 switch
			{
				SslMode.Implicit => 465, 
				SslMode.Explicit => 587, 
				_ => 25, 
			};
		}
		if (!auilw.pogqj(p0) || 1 == 0)
		{
			throw new ArgumentException("Hostname is invalid.", "serverName");
		}
		if (p1 < 1 || p1 > 65535)
		{
			throw hifyx.nztrs("serverPort", p1, "Port is out of range of valid values.");
		}
		try
		{
			nhmfm(rgtlm.tvbqu());
		}
		catch (fwwdw fwwdw)
		{
			throw new SmtpException(fwwdw.Message);
		}
		return encpd(p0, p1, p2, p3);
	}

	private string encpd(string p0, int p1, TlsParameters p2, SslMode p3)
	{
		xzaqz();
		try
		{
			try
			{
				taivh(SmtpState.Connecting);
				ijyuq();
				olfku(LogLevel.Info, "Info", "Connecting to {0}:{1} using {2}.", p0, p1, "Smtp");
				stpsr(rgtlm.tvbqu());
				if (bljbu != null && 0 == 0)
				{
					olfku(LogLevel.Info, "Info", "Using proxy {0}.", bljbu);
				}
				wmcaj = new zwrli(this, bljbu);
				wmcaj.fekql(p0, p1);
				ServerName = p0;
				ServerPort = p1;
				if (p3 == SslMode.Implicit)
				{
					wmcaj.rjtyx(nozao, tuvqw);
					p2 = kyzhw(p2);
					wmcaj.lateg(p2);
					ykyyc = true;
				}
				taivh(SmtpState.Reading);
				SmtpResponse smtpResponse = kdcdj(2);
				string raw = smtpResponse.Raw;
				ufzbp();
				iqwmr = true;
				if (p3 == SslMode.Explicit)
				{
					dqjqh(p2);
				}
				vevwx();
				return raw;
			}
			catch (SocketException ex)
			{
				yfdko();
				throw new SmtpException(ex.Message, ex, SmtpExceptionStatus.SocketError);
			}
			catch (ProxySocketException ex2)
			{
				yfdko();
				SmtpExceptionStatus smtpExceptionStatus;
				switch (ex2.Status)
				{
				case ProxySocketExceptionStatus.ConnectFailure:
					smtpExceptionStatus = SmtpExceptionStatus.ConnectFailure;
					if (smtpExceptionStatus == SmtpExceptionStatus.ConnectFailure)
					{
						break;
					}
					goto case ProxySocketExceptionStatus.NameResolutionFailure;
				case ProxySocketExceptionStatus.NameResolutionFailure:
					smtpExceptionStatus = SmtpExceptionStatus.NameResolutionFailure;
					if (smtpExceptionStatus != SmtpExceptionStatus.ConnectFailure)
					{
						break;
					}
					goto case ProxySocketExceptionStatus.ProxyNameResolutionFailure;
				case ProxySocketExceptionStatus.ProxyNameResolutionFailure:
					smtpExceptionStatus = SmtpExceptionStatus.ProxyNameResolutionFailure;
					if (smtpExceptionStatus != SmtpExceptionStatus.ConnectFailure)
					{
						break;
					}
					goto case ProxySocketExceptionStatus.ConnectionClosed;
				case ProxySocketExceptionStatus.ConnectionClosed:
					smtpExceptionStatus = SmtpExceptionStatus.ConnectionClosed;
					if (smtpExceptionStatus != SmtpExceptionStatus.ConnectFailure)
					{
						break;
					}
					goto default;
				default:
					smtpExceptionStatus = SmtpExceptionStatus.SocketError;
					break;
				}
				throw new SmtpException(ex2.Message, ex2, smtpExceptionStatus);
			}
			catch
			{
				yfdko();
				throw;
			}
		}
		catch (Exception p4)
		{
			tssug(p4);
			throw;
		}
	}

	[evron]
	public string Disconnect()
	{
		return cylsq();
	}

	private string cylsq()
	{
		xzaqz();
		try
		{
			if (edlng == SmtpState.Disconnected || 1 == 0)
			{
				vevwx();
				return "";
			}
			try
			{
				fiwtd("QUIT");
				if (edlng == SmtpState.Ready || edlng == SmtpState.Processing)
				{
					SmtpResponse smtpResponse = kdcdj(2);
					return smtpResponse.Description;
				}
			}
			catch (InvalidOperationException)
			{
			}
			catch (SmtpException)
			{
			}
			finally
			{
				yfdko();
			}
			return "";
		}
		catch (Exception p)
		{
			tssug(p);
			throw;
		}
	}

	private string dlcvw(string p0)
	{
		string pattern = brgjd.edcru("^{0}(?:\\x20(.*))?$", p0);
		Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		Match match = regex.Match(dxtrz);
		if (!match.Success || 1 == 0)
		{
			return null;
		}
		return match.Result("$1");
	}

	private void ufzbp()
	{
		fiwtd("EHLO " + kqadm);
		SmtpResponse smtpResponse = kdcdj(0);
		switch (smtpResponse.Group)
		{
		case 5:
			fiwtd("HELO " + kqadm);
			smtpResponse = kdcdj(2);
			if (smtpResponse.Description.IndexOf('\n') < 0)
			{
				zovug = smtpResponse.Description;
			}
			else
			{
				zovug = null;
			}
			dxtrz = "";
			break;
		case 2:
		{
			string text = dqids.tjhiv(smtpResponse);
			dxtrz = "";
			if (text != null && 0 == 0)
			{
				int num = text.IndexOf('\n');
				if (num > 0)
				{
					zovug = text.Substring(0, num);
					dxtrz = text.Substring(num + 1);
				}
			}
			break;
		}
		default:
			throw new SmtpException(smtpResponse);
		}
		SmtpExtensions smtpExtensions = (SmtpExtensions)0L;
		if (dlcvw("PIPELINING") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.Pipelining;
		}
		if (dlcvw("8BITMIME") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.EightBitMime;
		}
		if (dlcvw("BINARYMIME") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.BinaryMime;
		}
		if (dlcvw("CHUNKING") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.Chunking;
		}
		if (dlcvw("ETRN") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.EnhancedTurn;
		}
		if (dlcvw("ENHANCEDSTATUSCODE") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.EnhancedStatusCodes;
		}
		if (dlcvw("DSN") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.DeliveryStatusNotifications;
		}
		if (dlcvw("STARTTLS") != null && 0 == 0)
		{
			smtpExtensions |= SmtpExtensions.ExplicitSecurity;
		}
		rjbbu = smtpExtensions;
		string text2 = dlcvw("AUTH");
		if (text2 != null && 0 == 0)
		{
			text2 = " " + text2.ToUpper(CultureInfo.InvariantCulture) + " ";
			List<SmtpAuthentication> list = new List<SmtpAuthentication>();
			if (text2.IndexOf(" CRAM-MD5 ") >= 0)
			{
				list.Add(SmtpAuthentication.CramMD5);
			}
			if (text2.IndexOf(" DIGEST-MD5 ") >= 0)
			{
				list.Add(SmtpAuthentication.DigestMD5);
			}
			if (text2.IndexOf(" PLAIN ") >= 0)
			{
				list.Add(SmtpAuthentication.Plain);
			}
			if (text2.IndexOf(" LOGIN ") >= 0)
			{
				list.Add(SmtpAuthentication.Login);
			}
			if (dahxy.itsqr && 0 == 0)
			{
				if (text2.IndexOf(" NTLM ") >= 0)
				{
					list.Add(SmtpAuthentication.Ntlm);
				}
				if (text2.IndexOf(" GSSAPI ") >= 0)
				{
					list.Add(SmtpAuthentication.GssApi);
				}
			}
			if (text2.IndexOf(" XOAUTH2 ") >= 0)
			{
				list.Add(SmtpAuthentication.OAuth20);
			}
			rqhxv = list.ToArray();
			lotqk = (SmtpAuthentication[])rqhxv.Clone();
		}
		else
		{
			rqhxv = new SmtpAuthentication[0];
			lotqk = rqhxv;
		}
		string text3 = dlcvw("SIZE");
		if (text3 == null)
		{
			return;
		}
		rjbbu |= SmtpExtensions.MessageSizeDeclaration;
		ocjtq = -1;
		if (text3.Length > 0)
		{
			try
			{
				ocjtq = (int)Math.Min(2147483647L, Convert.ToInt64(text3, CultureInfo.InvariantCulture));
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[evron(OldStyleOnly = true)]
	[Obsolete("This overload of the Secure method has been deprecated and will be removed. Use another variant and specify parameters using Settings object instead.", true)]
	public void Secure(TlsParameters parameters)
	{
		ktnck(parameters);
	}

	private void ktnck(TlsParameters p0)
	{
		xzaqz();
		try
		{
			zdiys();
			dqjqh(p0);
		}
		catch (Exception p1)
		{
			tssug(p1);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Secure()
	{
		ktnck(null);
	}

	private TlsParameters kyzhw(TlsParameters p0)
	{
		SmtpSettings smtpSettings = opmvy;
		string serverName = ServerName;
		if (dcwar == null || 1 == 0)
		{
			dcwar = vvrhh;
		}
		return smtpSettings.mfcks(p0, this, serverName, dcwar);
	}

	private void dqjqh(TlsParameters p0)
	{
		if ((rjbbu & SmtpExtensions.ExplicitSecurity) == 0)
		{
			throw new SmtpException("Explicit TLS/SSL is not supported by the SMTP server.", SmtpExceptionStatus.OperationFailure);
		}
		if ((cgfdl & SmtpExtensions.ExplicitSecurity) == 0)
		{
			throw new SmtpException("Explicit TLS/SSL is supported by the server, but was disabled at the client.", SmtpExceptionStatus.OperationFailure);
		}
		p0 = kyzhw(p0);
		wmcaj.rjtyx(nozao, tuvqw);
		fiwtd("STARTTLS");
		kdcdj(2);
		wmcaj.lateg(p0);
		ufzbp();
		ykyyc = true;
	}

	private void dzpes(string p0, string[] p1, string p2, Stream p3, TransferEncoding p4)
	{
		bool flag = (rjbbu & cgfdl & SmtpExtensions.Chunking) != 0;
		long num = 0L;
		if (p3.CanSeek && 0 == 0)
		{
			num = p3.Length;
		}
		if (!flag || 1 == 0)
		{
			num += 3;
		}
		mtlib p5 = new mtlib(p3, !flag);
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num2;
		if ((rjbbu & cgfdl & SmtpExtensions.DeliveryStatusNotifications) != 0)
		{
			switch (hrzrm)
			{
			case DeliveryStatusNotificationOriginalMessageMethod.FullMessage:
				stringBuilder.Append(" RET=FULL");
				break;
			case DeliveryStatusNotificationOriginalMessageMethod.HeadersOnly:
				stringBuilder.Append(" RET=HDRS");
				break;
			}
			if (p2 != null && 0 == 0 && p2.Length > 0)
			{
				stringBuilder.Append(" ENVID=");
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_00e1;
				}
				goto IL_014f;
			}
			goto IL_0159;
		}
		goto IL_01fa;
		IL_0159:
		stringBuilder2.Append(" NOTIFY=");
		bool flag2 = true;
		if ((epdro & DeliveryStatusNotificationConditions.Success) != DeliveryStatusNotificationConditions.None && 0 == 0)
		{
			flag2 = false;
			stringBuilder2.Append("SUCCESS,");
		}
		if ((epdro & DeliveryStatusNotificationConditions.Delay) != DeliveryStatusNotificationConditions.None && 0 == 0)
		{
			flag2 = false;
			stringBuilder2.Append("DELAY,");
		}
		if ((epdro & DeliveryStatusNotificationConditions.Failure) != DeliveryStatusNotificationConditions.None && 0 == 0)
		{
			flag2 = false;
			stringBuilder2.Append("FAILURE,");
		}
		if (flag2 && 0 == 0)
		{
			stringBuilder2.Append("NEVER");
		}
		else
		{
			stringBuilder2.Length--;
		}
		goto IL_01fa;
		IL_014f:
		if (num2 < p2.Length)
		{
			goto IL_00e1;
		}
		goto IL_0159;
		IL_01fa:
		switch (p4)
		{
		case TransferEncoding.EightBit:
			stringBuilder.Append(" BODY=8BITMIME");
			break;
		case TransferEncoding.Binary:
			stringBuilder.Append(" BODY=BINARYMIME");
			break;
		}
		if (p3.CanSeek && 0 == 0 && (rjbbu & cgfdl & SmtpExtensions.MessageSizeDeclaration) != 0 && ocjtq != 0 && 0 == 0)
		{
			stringBuilder.dlvlk(" SIZE={0}", p3.Length);
		}
		urlzs(p0, stringBuilder.ToString());
		if ((rjbbu & cgfdl & SmtpExtensions.Pipelining) == 0)
		{
			qyzgc(flag, p1, p5, stringBuilder2.ToString(), num);
		}
		else
		{
			orquw(flag, p1, p5, stringBuilder2.ToString(), num);
		}
		return;
		IL_00e1:
		if (p2[num2] < '\u0080')
		{
			if (p2[num2] < '!' || p2[num2] > '~' || p2[num2] == '+' || p2[num2] == '=')
			{
				stringBuilder.dlvlk("+{0:X2}", (int)p2[num2]);
			}
			else
			{
				stringBuilder.Append(p2[num2]);
			}
		}
		num2++;
		goto IL_014f;
	}

	private static TransferEncoding ahqcz(MimeEntity p0, TransferEncoding p1)
	{
		int num;
		if (p0.IsMultipart && 0 == 0)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0018;
			}
			goto IL_0032;
		}
		goto IL_0040;
		IL_0032:
		if (num < p0.Parts.Count)
		{
			goto IL_0018;
		}
		goto IL_0040;
		IL_0040:
		if (p0.ContentMessage != null && 0 == 0)
		{
			p1 = ahqcz(p0.ContentMessage, p1);
		}
		switch (p0.TransferEncoding)
		{
		case TransferEncoding.EightBit:
			if (p1 != TransferEncoding.Unknown && p1 != TransferEncoding.Binary)
			{
				p1 = TransferEncoding.EightBit;
			}
			break;
		case TransferEncoding.Binary:
			if (p1 != TransferEncoding.Unknown)
			{
				p1 = TransferEncoding.Binary;
			}
			break;
		default:
			p1 = TransferEncoding.Unknown;
			break;
		case TransferEncoding.QuotedPrintable:
		case TransferEncoding.Base64:
		case TransferEncoding.SevenBit:
			break;
		}
		return p1;
		IL_0018:
		p1 = ahqcz(p0.Parts[num], p1);
		num++;
		goto IL_0032;
	}

	private static void dxbwq(MimeEntity p0, TransferEncoding p1, TransferEncoding p2)
	{
		int num;
		if (p0.IsMultipart && 0 == 0)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0018;
			}
			goto IL_002f;
		}
		goto IL_003d;
		IL_002f:
		if (num < p0.Parts.Count)
		{
			goto IL_0018;
		}
		goto IL_003d;
		IL_003d:
		if (p0.ContentMessage != null && 0 == 0)
		{
			dxbwq(p0.ContentMessage, p1, p2);
		}
		if (p0.TransferEncoding == p1)
		{
			if (p0.ReadOnly && 0 == 0)
			{
				throw new SmtpException("A message part uses a content-transfer-encoding which is not accepted by the server and cannot be changed.", SmtpExceptionStatus.OperationFailure);
			}
			if (p0.ContentType.MediaType.StartsWith("multipart/") && 0 == 0)
			{
				p2 = TransferEncoding.SevenBit;
			}
			if (p0.ContentType.MediaType.StartsWith("message/") && 0 == 0)
			{
				p2 = TransferEncoding.SevenBit;
			}
			p0.TransferEncoding = p2;
		}
		return;
		IL_0018:
		dxbwq(p0.Parts[num], p1, p2);
		num++;
		goto IL_002f;
	}

	internal static void yxofe(MimeMessage p0)
	{
		TransferEncoding transferEncoding = (TransferEncoding)(-2);
		while (true)
		{
			TransferEncoding transferEncoding2 = ahqcz(p0, TransferEncoding.SevenBit);
			switch (transferEncoding2)
			{
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
			case TransferEncoding.SevenBit:
				return;
			}
			if (transferEncoding2 == transferEncoding)
			{
				throw new SmtpException("Unsupported MIME message encoding.", SmtpExceptionStatus.OperationFailure);
			}
			transferEncoding = transferEncoding2;
			dxbwq(p0, transferEncoding2, TransferEncoding.Base64);
		}
	}

	private static void qipiy(string p0, string p1, out MailAddress p2, out MailAddressCollection p3, bool p4)
	{
		try
		{
			if (p0 != null && 0 == 0)
			{
				p2 = new MailAddress(p0);
				if ((!p4 || 1 == 0) && (p2.Address == null || false || p2.Address.Length == 0 || 1 == 0))
				{
					p2 = null;
				}
			}
			else
			{
				p2 = null;
			}
		}
		catch (MimeException ex)
		{
			if (ex.Status != MimeExceptionStatus.HeaderParserError)
			{
				throw;
			}
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "sender", ex.Message), ex);
		}
		catch (ArgumentException ex2)
		{
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "sender", ex2.Message), ex2);
		}
		try
		{
			if (p1 != null && 0 == 0)
			{
				p3 = new MailAddressCollection(p1);
			}
			else
			{
				p3 = null;
			}
		}
		catch (MimeException ex3)
		{
			if (ex3.Status != MimeExceptionStatus.HeaderParserError)
			{
				throw;
			}
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "recipients", ex3.Message), ex3);
		}
		catch (ArgumentException ex4)
		{
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "recipients", ex4.Message), ex4);
		}
	}

	private static MimeMessage ljcln(string p0, string p1, string p2, string p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("from");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("to");
		}
		if (p2 == null || 1 == 0)
		{
			p2 = "";
		}
		if (p3 == null || 1 == 0)
		{
			p3 = "";
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "from");
		}
		if (p1.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "to");
		}
		MailAddressCollection value;
		try
		{
			value = new MailAddressCollection(p0);
		}
		catch (MimeException ex)
		{
			if (ex.Status != MimeExceptionStatus.HeaderParserError)
			{
				throw;
			}
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "from", ex.Message), ex);
		}
		catch (ArgumentException ex2)
		{
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "from", ex2.Message), ex2);
		}
		MailAddressCollection value2;
		try
		{
			value2 = new MailAddressCollection(p1);
		}
		catch (MimeException ex3)
		{
			if (ex3.Status != MimeExceptionStatus.HeaderParserError)
			{
				throw;
			}
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "to", ex3.Message), ex3);
		}
		catch (ArgumentException ex4)
		{
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "to", ex4.Message), ex4);
		}
		Unstructured value3;
		try
		{
			value3 = new Unstructured(p2);
		}
		catch (MimeException ex5)
		{
			if (ex5.Status != MimeExceptionStatus.HeaderParserError)
			{
				throw;
			}
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "subject", ex5.Message), ex5);
		}
		catch (ArgumentException ex6)
		{
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "subject", ex6.Message), ex6);
		}
		MimeMessage mimeMessage = new MimeMessage();
		mimeMessage.Headers.Add(new MimeHeader("From", value));
		mimeMessage.Headers.Add(new MimeHeader("To", value2));
		mimeMessage.Date = new MailDateTime(DateTime.Now);
		mimeMessage.MessageId = new MessageId();
		mimeMessage.Headers.Add(new MimeHeader("Subject", value3));
		try
		{
			mimeMessage.SetContent(p3);
			return mimeMessage;
		}
		catch (MimeException ex7)
		{
			if (ex7.Status != MimeExceptionStatus.OperationError)
			{
				throw;
			}
			throw new ArgumentException(brgjd.edcru("Invalid {0} value: {1}", "body", ex7.Message), ex7);
		}
	}

	private bool rqblp(string p0)
	{
		if (p0.IndexOf('@') < 0 && p0.IndexOf(':') >= 0)
		{
			return true;
		}
		if (p0.IndexOf(';') >= 0)
		{
			throw new SmtpException(brgjd.edcru("Invalid address: '{0}'.", p0), SmtpExceptionStatus.OperationFailure);
		}
		return false;
	}

	private void vcipv(MimeMessage p0, Stream p1, MailAddress p2, MailAddressCollection p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("mail");
		}
		MailAddressCollection mailAddressCollection;
		if (p3 == null || 1 == 0)
		{
			mailAddressCollection = new MailAddressCollection();
			mailAddressCollection.AddRange(p0.To);
			mailAddressCollection.AddRange(p0.CC);
			mailAddressCollection.AddRange(p0.Bcc);
		}
		else
		{
			mailAddressCollection = p3;
		}
		ArrayList arrayList = new ArrayList();
		int num = 0;
		if (num != 0)
		{
			goto IL_005f;
		}
		goto IL_009c;
		IL_0194:
		int num2;
		if (num2 < p0.From.Count)
		{
			goto IL_013e;
		}
		goto IL_01a3;
		IL_035b:
		if (p1 == null || 1 == 0)
		{
			if (p0.Date == null || 1 == 0)
			{
				p0.Date = new MailDateTime(DateTime.Now);
			}
			p0.Options |= MimeOptions.DoNotWriteBcc;
			if (!Settings.SendWithNoBuffer || 1 == 0)
			{
				opjbe opjbe = new opjbe();
				p0.Save(opjbe);
				opjbe.Position = 0L;
				p1 = opjbe;
			}
			else
			{
				p1 = p0.ToStream();
			}
		}
		axsof(p0, p1);
		string text;
		string[] p4;
		TransferEncoding transferEncoding;
		dzpes(text, p4, p0.EnvelopeId, p1, transferEncoding);
		return;
		IL_013e:
		MailAddress mailAddress = p0.From[num2];
		if (mailAddress.Address != null && 0 == 0 && mailAddress.Address.Length > 0)
		{
			text = mailAddress.Address;
			if (p0.From.Count > 1)
			{
				p0.Sender = mailAddress;
			}
			goto IL_01a3;
		}
		num2++;
		goto IL_0194;
		IL_01a3:
		bool allowNullSender;
		if (text == null || false || text.Length == 0 || 1 == 0)
		{
			if (!allowNullSender || 1 == 0)
			{
				throw new SmtpException("No sender found in the message.", SmtpExceptionStatus.OperationFailure);
			}
			text = "";
		}
		goto IL_022a;
		IL_009c:
		if (num < mailAddressCollection.Count)
		{
			goto IL_005f;
		}
		if (arrayList.Count == 0 || 1 == 0)
		{
			throw new SmtpException("No recipients found in the message.", SmtpExceptionStatus.OperationFailure);
		}
		allowNullSender = Settings.AllowNullSender;
		p4 = (string[])arrayList.ToArray(typeof(string));
		text = null;
		if (p2 == null || 1 == 0)
		{
			if (p0.Sender != null && 0 == 0)
			{
				text = p0.Sender.Address;
				if (text.Length == 0 || 1 == 0)
				{
					text = null;
				}
			}
			if (text == null || 1 == 0)
			{
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_013e;
				}
				goto IL_0194;
			}
			goto IL_01a3;
		}
		text = p2.Address;
		if (text == null || false || text.Length == 0 || 1 == 0)
		{
			if (!allowNullSender || 1 == 0)
			{
				throw new SmtpException("No sender specified.", SmtpExceptionStatus.OperationFailure);
			}
			text = "";
		}
		goto IL_022a;
		IL_022a:
		if (!Settings.SkipContentTransferEncodingCheck || 1 == 0)
		{
			transferEncoding = ahqcz(p0, TransferEncoding.SevenBit);
			switch (transferEncoding)
			{
			case TransferEncoding.EightBit:
			{
				SmtpExtensions smtpExtensions = SmtpExtensions.EightBitMime;
				if ((rjbbu & cgfdl & smtpExtensions) == smtpExtensions)
				{
					break;
				}
				if (p1 != null && 0 == 0)
				{
					throw new SmtpException("A message part uses a content-transfer-encoding which is not accepted by the server and cannot be changed.", SmtpExceptionStatus.OperationFailure);
				}
				dxbwq(p0, TransferEncoding.EightBit, TransferEncoding.Base64);
				transferEncoding = TransferEncoding.Base64;
				if (transferEncoding != TransferEncoding.QuotedPrintable)
				{
					break;
				}
				goto case TransferEncoding.Binary;
			}
			case TransferEncoding.Binary:
			{
				SmtpExtensions smtpExtensions = SmtpExtensions.Chunking | SmtpExtensions.BinaryMime;
				if ((rjbbu & cgfdl & smtpExtensions) == smtpExtensions)
				{
					break;
				}
				if (p1 != null && 0 == 0)
				{
					throw new SmtpException("A message part uses a content-transfer-encoding which is not accepted by the server and cannot be changed.", SmtpExceptionStatus.OperationFailure);
				}
				dxbwq(p0, TransferEncoding.Binary, TransferEncoding.Base64);
				transferEncoding = ahqcz(p0, TransferEncoding.SevenBit);
				if (transferEncoding != TransferEncoding.EightBit)
				{
					break;
				}
				goto case TransferEncoding.EightBit;
			}
			default:
				if (p1 != null && 0 == 0)
				{
					throw new SmtpException("A message part uses a content-transfer-encoding which is not accepted by the server and cannot be changed.", SmtpExceptionStatus.OperationFailure);
				}
				dxbwq(p0, TransferEncoding.Unknown, TransferEncoding.Base64);
				transferEncoding = ahqcz(p0, TransferEncoding.SevenBit);
				if (transferEncoding == TransferEncoding.EightBit)
				{
					goto case TransferEncoding.EightBit;
				}
				if (transferEncoding != TransferEncoding.Binary)
				{
					break;
				}
				goto case TransferEncoding.Binary;
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
			case TransferEncoding.SevenBit:
				break;
			}
		}
		else
		{
			if ((rjbbu & cgfdl & SmtpExtensions.EightBitMime) != 0)
			{
				transferEncoding = TransferEncoding.EightBit;
				if (transferEncoding != TransferEncoding.QuotedPrintable)
				{
					goto IL_035b;
				}
			}
			transferEncoding = TransferEncoding.SevenBit;
		}
		goto IL_035b;
		IL_005f:
		MailAddress mailAddress2 = mailAddressCollection[num];
		if (mailAddress2.Address.Length > 0 && (!rqblp(mailAddress2.Address) || 1 == 0))
		{
			arrayList.Add(mailAddress2.Address);
		}
		num++;
		goto IL_009c;
	}

	private void nroxg(string p0, string p1, SmtpAuthentication p2, GssApiProvider p3)
	{
		string text = p0;
		string text2 = "";
		string text3 = "";
		string text4 = "AUTH ";
		switch (p2)
		{
		case SmtpAuthentication.CramMD5:
		{
			if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
			{
				throw new SmtpException("CramMD5 not supported in FIPS-compliant mode.", SmtpExceptionStatus.OperationFailure);
			}
			fiwtd(text4 + "CRAM-MD5");
			text2 = kdcdj(3).Description.Trim();
			byte[] array = Convert.FromBase64String(text2);
			text3 = mhmxd.GetString(array, 0, array.Length);
			jrbbo jrbbo = new jrbbo(mhmxd.GetBytes(p1));
			byte[] array2 = jrbbo.eyirg(mhmxd.GetBytes(text3));
			string text7 = BitConverter.ToString(array2).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
			byte[] bytes = mhmxd.GetBytes(text + " " + text7);
			string p5 = Convert.ToBase64String(bytes, 0, bytes.Length);
			fiwtd(p5);
			kdcdj(2);
			break;
		}
		case SmtpAuthentication.DigestMD5:
			try
			{
				fiwtd(text4 + "DIGEST-MD5");
				text2 = kdcdj(3).Description.Trim();
				diwlk diwlk = new diwlk(text2, "smtp");
				string p4 = diwlk.llgzn(text, p1);
				fiwtd(p4);
				text2 = kdcdj(3).Description.Trim();
				if (!diwlk.nxuzu(text2) || 1 == 0)
				{
					throw new SmtpException("Bad DIGEST-MD5 response was received from the server.", SmtpExceptionStatus.OperationFailure);
				}
				fiwtd("");
				kdcdj(2);
			}
			catch (CryptographicException ex)
			{
				throw new SmtpException(ex.Message, ex, SmtpExceptionStatus.OperationFailure);
			}
			break;
		case SmtpAuthentication.Plain:
		{
			string s = "\0" + text + "\0" + p1;
			byte[] bytes2 = mhmxd.GetBytes(s);
			s = Convert.ToBase64String(bytes2, 0, bytes2.Length);
			jvzip(text4 + "PLAIN " + s, text4 + "PLAIN **********");
			kdcdj(2);
			break;
		}
		case SmtpAuthentication.Login:
		{
			fiwtd(text4 + "LOGIN");
			kdcdj(3);
			byte[] bytes3 = mhmxd.GetBytes(text);
			text = Convert.ToBase64String(bytes3, 0, bytes3.Length);
			fiwtd(text);
			kdcdj(3);
			byte[] bytes4 = mhmxd.GetBytes(p1);
			p1 = Convert.ToBase64String(bytes4, 0, bytes4.Length);
			jvzip(p1, "**********");
			kdcdj(2);
			break;
		}
		case SmtpAuthentication.OAuth20:
		{
			fiwtd(text4 + "XOAUTH2 " + p1);
			SmtpResponse smtpResponse2 = bkglx(0, p1: true);
			if (smtpResponse2.Group == 2)
			{
				break;
			}
			if (smtpResponse2.Code == 334)
			{
				string p6;
				string text8 = auilw.vydkr(smtpResponse2.Description, out p6);
				if (text8 != null && 0 == 0)
				{
					SmtpException ex2 = new SmtpException(p6, SmtpExceptionStatus.ProtocolError);
					ex2.Data["OAuth"] = text8;
					throw ex2;
				}
			}
			throw new SmtpException(smtpResponse2);
		}
		case SmtpAuthentication.Ntlm:
		case SmtpAuthentication.GssApi:
		{
			if (dahxy.xzevd && 0 == 0)
			{
				throw new SmtpException("SSPI authentication is not supported on non-Windows systems.", SmtpExceptionStatus.OperationFailure);
			}
			olfku(LogLevel.Debug, "Info", "Attempting {0} authentication.", p2.ToString().ToUpper(CultureInfo.InvariantCulture));
			SspiAuthentication sspiAuthentication = null;
			try
			{
				if (p2 == SmtpAuthentication.Ntlm)
				{
					text4 += "NTLM ";
					sspiAuthentication = new SspiAuthentication("NTLM", SspiDataRepresentation.Native, ServerName, (SspiRequirements)0, text, p1, null);
				}
				else
				{
					if (p3 == null || 1 == 0)
					{
						p3 = GssApiProvider.GetSspiProvider("Kerberos", null, p0, p1, null);
					}
					string text5 = p3.GetParameter(0) as string;
					string text6 = p3.GetParameter(1) as string;
					text = p3.GetParameter(2) as string;
					p1 = p3.GetParameter(3) as string;
					string userDomain = p3.GetParameter(4) as string;
					if ((text6 == null || 1 == 0) && text5 != "NTLM" && 0 == 0)
					{
						text6 = "smtp/" + ServerName;
					}
					sspiAuthentication = new SspiAuthentication(text5, SspiDataRepresentation.Network, text6, SspiRequirements.Connection | SspiRequirements.Integrity, text, p1, userDomain);
					text4 += "GSSAPI ";
				}
				byte[] challenge = null;
				bool complete;
				while (true)
				{
					byte[] nextMessage = sspiAuthentication.GetNextMessage(challenge, out complete);
					if (nextMessage != null && 0 == 0)
					{
						fiwtd(text4 + Convert.ToBase64String(nextMessage, 0, nextMessage.Length));
					}
					else
					{
						fiwtd(text4);
					}
					text4 = string.Empty;
					if (complete ? true : false)
					{
						break;
					}
					text2 = kdcdj(3).Description;
					challenge = Convert.FromBase64String(text2);
				}
				SmtpResponse smtpResponse = kdcdj(0);
				if (smtpResponse.Group != 2)
				{
					if (smtpResponse.Group != 3)
					{
						throw new SmtpException(smtpResponse);
					}
					text2 = smtpResponse.Description;
					challenge = Convert.FromBase64String(text2);
					challenge = sspiAuthentication.Unwrap(challenge, out var qop);
					byte[] nextMessage = new byte[4] { 1, 0, 0, 0 };
					qop = -2147483647;
					nextMessage = sspiAuthentication.Wrap(nextMessage, qop, out complete);
					fiwtd(Convert.ToBase64String(nextMessage, 0, nextMessage.Length));
					kdcdj(2);
				}
			}
			finally
			{
				if (sspiAuthentication != null && 0 == 0)
				{
					sspiAuthentication.Dispose();
				}
			}
			break;
		}
		default:
			throw new ArgumentException("Unknown authentication method.", "method");
		}
		UserName = p0;
		jleud = true;
	}

	[evron]
	public void Send(MimeMessage mail, string sender, string recipients)
	{
		if (mail == null || 1 == 0)
		{
			throw new ArgumentNullException("mail");
		}
		qipiy(sender, recipients, out var p, out var p2, Settings.AllowNullSender);
		oexqv(mail, p, p2);
	}

	private void oexqv(MimeMessage p0, MailAddress p1, MailAddressCollection p2)
	{
		xzaqz();
		try
		{
			zdiys();
			vcipv((MimeMessage)p0.Clone(), null, p1, p2);
		}
		catch (Exception p3)
		{
			tssug(p3);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Send(MailMessage mail, string sender, string recipients)
	{
		if (mail == null || 1 == 0)
		{
			throw new ArgumentNullException("mail");
		}
		qipiy(sender, recipients, out var p, out var p2, Settings.AllowNullSender);
		bbsly(mail, p, p2);
	}

	private void bbsly(MailMessage p0, MailAddress p1, MailAddressCollection p2)
	{
		xzaqz();
		try
		{
			zdiys();
			vcipv(p0.ToMimeMessage(), null, p1, p2);
		}
		catch (Exception p3)
		{
			tssug(p3);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Send(MimeMessage mail)
	{
		Send(mail, null, null);
	}

	[evron]
	public void Send(MailMessage mail)
	{
		Send(mail, null, null);
	}

	[evron]
	public void Send(string from, string to, string subject, string body)
	{
		cmqdf(from, to, subject, body);
	}

	private void cmqdf(string p0, string p1, string p2, string p3)
	{
		MimeMessage p4 = ljcln(p0, p1, p2, p3);
		xzaqz();
		try
		{
			zdiys();
			vcipv(p4, null, null, null);
		}
		catch (Exception p5)
		{
			tssug(p5);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Send(Stream input, string sender, string recipients)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		wwuev(input, sender, recipients);
	}

	[evron]
	public void Send(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		wwuev(input, null, null);
	}

	private void wwuev(Stream p0, string p1, string p2)
	{
		xzaqz();
		try
		{
			zdiys();
			qipiy(p1, p2, out var p3, out var p4, Settings.AllowNullSender);
			if (!Settings.SendWithNoBuffer || 1 == 0)
			{
				MimeMessage mimeMessage = new MimeMessage();
				mimeMessage.Options = MimeOptions.DoNotParseMimeTree;
				mimeMessage.Load(p0);
				vcipv(mimeMessage, null, p3, p4);
				return;
			}
			if (!p0.CanSeek || 1 == 0)
			{
				throw new InvalidOperationException("MIME message stream is not seekable.");
			}
			long position = p0.Position;
			MimeMessage mimeMessage2 = new MimeMessage();
			mimeMessage2.Options = MimeOptions.OnlyParseHeaders | MimeOptions.DoNotCloseStreamAfterLoad;
			mimeMessage2.Load(p0);
			p0.Seek(position, SeekOrigin.Begin);
			p0.Flush();
			vcipv(mimeMessage2, p0, p3, p4);
		}
		catch (Exception p5)
		{
			tssug(p5);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Send(string fileName)
	{
		Send(fileName, null, null);
	}

	[evron]
	public void Send(string fileName, string sender, string recipients)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		if (fileName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "fileName");
		}
		zujhf(fileName, sender, recipients);
	}

	private void zujhf(string p0, string p1, string p2)
	{
		Stream stream;
		try
		{
			zdiys();
			stream = File.OpenRead(p0);
		}
		catch (Exception p3)
		{
			tssug(p3);
			throw;
		}
		try
		{
			wwuev(stream, p1, p2);
		}
		finally
		{
			stream.Close();
		}
	}

	[evron]
	public void Login(SmtpAuthentication method)
	{
		kkcad(method);
	}

	private void kkcad(SmtpAuthentication p0)
	{
		switch (p0)
		{
		case SmtpAuthentication.Ntlm:
		case SmtpAuthentication.GssApi:
			if (dahxy.hdfhq && 0 == 0)
			{
				throw new NotSupportedException("Method is not supported on this platform.");
			}
			break;
		case SmtpAuthentication.Auto:
			if (Array.IndexOf(rqhxv, SmtpAuthentication.GssApi, 0, rqhxv.Length) >= 0)
			{
				p0 = SmtpAuthentication.GssApi;
				break;
			}
			if (Array.IndexOf(rqhxv, SmtpAuthentication.Ntlm, 0, rqhxv.Length) >= 0)
			{
				p0 = SmtpAuthentication.Ntlm;
				break;
			}
			throw new SmtpException("None of the supported authentication methods is accepted by the server.", SmtpExceptionStatus.OperationFailure);
		case SmtpAuthentication.OAuth20:
			throw new SmtpException(brgjd.edcru("You have to specify token for '{0}' authentication method.", p0), SmtpExceptionStatus.OperationFailure);
		default:
			throw new SmtpException(brgjd.edcru("You have to specify username and password for '{0}' authentication method.", p0), SmtpExceptionStatus.OperationFailure);
		}
		xzaqz();
		try
		{
			zdiys();
			if (jleud && 0 == 0)
			{
				throw new InvalidOperationException("Already authenticated.");
			}
			if (Array.IndexOf(rqhxv, p0, 0, rqhxv.Length) < 0)
			{
				throw new SmtpException(brgjd.edcru("Authentication method '{0}' is not supported. Consider using 'Auto' or use another method from the list returned by GetSupportedAuthenticationMethods.", p0), SmtpExceptionStatus.OperationFailure);
			}
			nroxg(null, null, p0, null);
		}
		catch (Exception p1)
		{
			tssug(p1);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Login(GssApiProvider provider)
	{
		lacal(provider);
	}

	private void lacal(GssApiProvider p0)
	{
		xzaqz();
		try
		{
			zdiys();
			if (jleud && 0 == 0)
			{
				throw new InvalidOperationException("Already authenticated.");
			}
			if (Array.IndexOf(rqhxv, SmtpAuthentication.GssApi, 0, rqhxv.Length) < 0)
			{
				if (dahxy.itsqr && 0 == 0)
				{
					brgjd.edcru("Authentication method '{0}' is not supported by the server. Consider using 'Auto' or use another method from the list returned by GetSupportedAuthenticationMethods.", SmtpAuthentication.GssApi);
				}
				else
				{
					brgjd.edcru("Authentication method '{0}' is not supported. Consider using 'Auto' or use another method from the list returned by GetSupportedAuthenticationMethods.", SmtpAuthentication.GssApi);
				}
				throw new SmtpException(brgjd.edcru("Authentication method '{0}' is not supported. Consider using 'Auto' or use another method from the list returned by GetSupportedAuthenticationMethods.", SmtpAuthentication.GssApi), SmtpExceptionStatus.OperationFailure);
			}
			nroxg(null, null, SmtpAuthentication.GssApi, p0);
		}
		catch (Exception p1)
		{
			tssug(p1);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Login(string userName, string password, SmtpAuthentication method)
	{
		if (userName == null || 1 == 0)
		{
			throw new ArgumentNullException("userName");
		}
		if (userName.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "userName");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		rlbqd(userName, password, method);
	}

	private void rlbqd(string p0, string p1, SmtpAuthentication p2)
	{
		xzaqz();
		try
		{
			zdiys();
			if (jleud && 0 == 0)
			{
				throw new InvalidOperationException("Already authenticated.");
			}
			if (p2 == SmtpAuthentication.Auto || 1 == 0)
			{
				bool isSecured = IsSecured;
				bool useFipsAlgorithmsOnly = CryptoHelper.UseFipsAlgorithmsOnly;
				List<SmtpAuthentication> list = new List<SmtpAuthentication>();
				if ((!isSecured || 1 == 0) && (!useFipsAlgorithmsOnly || 1 == 0))
				{
					list.Add(SmtpAuthentication.CramMD5);
					list.Add(SmtpAuthentication.DigestMD5);
				}
				list.Add(SmtpAuthentication.Plain);
				list.Add(SmtpAuthentication.Login);
				if (isSecured && 0 == 0 && (!useFipsAlgorithmsOnly || 1 == 0))
				{
					list.Add(SmtpAuthentication.CramMD5);
					list.Add(SmtpAuthentication.DigestMD5);
				}
				if (dahxy.gusxd(p0) && 0 == 0)
				{
					list.Add(SmtpAuthentication.GssApi);
					list.Add(SmtpAuthentication.Ntlm);
				}
				using (List<SmtpAuthentication>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext() ? true : false)
					{
						SmtpAuthentication current = enumerator.Current;
						if (Array.IndexOf(rqhxv, current, 0, rqhxv.Length) >= 0)
						{
							nroxg(p0, p1, current, null);
							return;
						}
					}
				}
				throw new SmtpException("None of the supported authentication methods is accepted by the server.", SmtpExceptionStatus.OperationFailure);
			}
			if (Array.IndexOf(rqhxv, p2, 0, rqhxv.Length) < 0)
			{
				throw new SmtpException(brgjd.edcru("Authentication method '{0}' is not supported. Consider using 'Auto' or use another method from the list returned by GetSupportedAuthenticationMethods.", p2), SmtpExceptionStatus.OperationFailure);
			}
			if (p2 == SmtpAuthentication.OAuth20)
			{
				p1 = ecitw.cbyuh(p0, p1);
			}
			nroxg(p0, p1, p2, null);
		}
		catch (Exception p3)
		{
			tssug(p3);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void Login(string token, SmtpAuthentication method)
	{
		if (token == null || 1 == 0)
		{
			throw new ArgumentNullException("token");
		}
		if (token.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "token");
		}
		if (method != SmtpAuthentication.OAuth20)
		{
			throw new SmtpException(brgjd.edcru("Authentication method '{0}' is not token-based.", method), SmtpExceptionStatus.OperationFailure);
		}
		rlbqd(null, token, method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[evron(OldStyleOnly = true)]
	[wptwl(false)]
	[Obsolete("This method is seldom useful and has been deprecated", false)]
	public SmtpResponse Verify(string to)
	{
		if (to == null || 1 == 0)
		{
			throw new ArgumentNullException("to");
		}
		if (to.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "to");
		}
		int num = to.IndexOf('@');
		int num2 = to.LastIndexOf('@');
		if (num != num2)
		{
			throw new ArgumentException("A specified mailbox is invalid.", "to");
		}
		MailAddress mailAddress;
		try
		{
			mailAddress = new MailAddress(to);
		}
		catch (MimeException innerException)
		{
			throw new ArgumentException("A specified mailbox is invalid.", innerException);
		}
		string host = mailAddress.Host;
		string user = mailAddress.User;
		if (host.Length == 0 || false || user.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("A specified mailbox is invalid.", "to");
		}
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_00be;
		}
		goto IL_0119;
		IL_015e:
		int num4;
		if (num4 < user.Length)
		{
			goto IL_012b;
		}
		return byzun(mailAddress);
		IL_012b:
		char c = user[num4];
		if (c < ' ' || c >= '\u007f' || c == '@')
		{
			throw new ArgumentException("A specified mailbox is invalid.", "to");
		}
		num4++;
		goto IL_015e;
		IL_00be:
		char c2 = host[num3];
		if ((c2 < 'a' || c2 > 'z') && (c2 < 'A' || c2 > 'Z') && (c2 < '0' || c2 > '9'))
		{
			switch (c2)
			{
			default:
				throw new ArgumentException("A specified mailbox is invalid.", "to");
			case '-':
			case '.':
				break;
			}
		}
		num3++;
		goto IL_0119;
		IL_0119:
		if (num3 < host.Length)
		{
			goto IL_00be;
		}
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_012b;
		}
		goto IL_015e;
	}

	private SmtpResponse byzun(MailAddress p0)
	{
		xzaqz();
		try
		{
			zdiys();
			fiwtd("VRFY <" + p0.Address + ">");
			return kdcdj(0);
		}
		catch (Exception p1)
		{
			tssug(p1);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public void SendCommand(string command)
	{
		if (command == null || 1 == 0)
		{
			throw new ArgumentNullException("command");
		}
		if (command.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Command has zero length.", "command");
		}
		yewdz(command);
	}

	private void yewdz(string p0)
	{
		xzaqz();
		try
		{
			zdiys();
			fiwtd(p0);
		}
		catch (Exception p1)
		{
			tssug(p1);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	[evron]
	public SmtpResponse ReadResponse()
	{
		return ouakr();
	}

	private SmtpResponse ouakr()
	{
		xzaqz();
		try
		{
			zdiys();
			return kdcdj(0);
		}
		catch (Exception p)
		{
			tssug(p);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	private SocketException msjqd()
	{
		xzaqz();
		try
		{
			zdiys();
			ISocket csxcc = wmcaj.csxcc;
			if (csxcc != null && 0 == 0 && todgf.gnzjs(csxcc) != SocketState.Connected)
			{
				return new SocketException(10054);
			}
			return null;
		}
		catch (Exception p)
		{
			tssug(p);
			throw;
		}
		finally
		{
			vevwx();
		}
	}

	public void CheckConnectionState()
	{
		SocketException ex = msjqd();
		if (ex == null || 1 == 0)
		{
			return;
		}
		int num = ex.skehp();
		SmtpExceptionStatus smtpExceptionStatus;
		if (num == 10054)
		{
			smtpExceptionStatus = SmtpExceptionStatus.ConnectionClosed;
			if (smtpExceptionStatus != SmtpExceptionStatus.ConnectFailure)
			{
				goto IL_002f;
			}
		}
		smtpExceptionStatus = SmtpExceptionStatus.SocketError;
		goto IL_002f;
		IL_002f:
		throw new SmtpException(ex, smtpExceptionStatus);
	}

	public SmtpConnectionState GetConnectionState()
	{
		if (edlng == SmtpState.Disposed)
		{
			throw new ObjectDisposedException("Smtp");
		}
		if (!iqwmr || 1 == 0)
		{
			return new SmtpConnectionState(connected: false, 0);
		}
		SocketException ex = msjqd();
		if (ex == null || 1 == 0)
		{
			return SmtpConnectionState.qzdff;
		}
		return new SmtpConnectionState(connected: false, ex.skehp());
	}

	public override string ToString()
	{
		return brgjd.edcru("{0}({1})", "Smtp", base.InstanceId);
	}

	[evron]
	public void Login(string userName, string password)
	{
		Login(userName, password, SmtpAuthentication.Auto);
	}

	[evron]
	public void KeepAlive()
	{
		vbxqs("NOOP");
	}

	[Obsolete("This method is seldom useful and has been deprecated", false)]
	[wptwl(false)]
	[evron(OldStyleOnly = true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void EnhancedTurn(string nodeName)
	{
		if (nodeName == null || 1 == 0)
		{
			throw new ArgumentNullException("nodeName");
		}
		if (nodeName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "nodeName");
		}
		vbxqs("ETRN " + nodeName);
	}

	public void SetSocketFactory(ISocketFactory factory)
	{
		xzaqz();
		bljbu = factory;
		vevwx();
	}

	public SmtpAuthentication[] GetSupportedAuthenticationMethods()
	{
		lock (clkzc)
		{
			zdiys();
			return lotqk;
		}
	}

	public static void Send(MimeMessage message, string serverName, int serverPort)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		Smtp smtp = new Smtp();
		try
		{
			smtp.Connect(serverName, serverPort);
			smtp.Send(message);
			smtp.Disconnect();
		}
		finally
		{
			smtp.Dispose();
		}
	}

	public static void Send(MailMessage message, string serverName, int serverPort)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		if (serverName == null || 1 == 0)
		{
			throw new ArgumentNullException("serverName");
		}
		if (serverName.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Hostname cannot be empty.", "serverName");
		}
		if (serverPort < 1 || serverPort > 65535)
		{
			throw hifyx.nztrs("serverPort", serverPort, "Port is out of range of valid values.");
		}
		Send(message.ToMimeMessage(), serverName, serverPort);
	}

	public static void Send(string fileName, string serverName, int serverPort)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		if (fileName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "fileName");
		}
		if (serverName == null || 1 == 0)
		{
			throw new ArgumentNullException("serverName");
		}
		if (serverName.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Hostname cannot be empty.", "serverName");
		}
		if (serverPort < 1 || serverPort > 65535)
		{
			throw hifyx.nztrs("serverPort", serverPort, "Port is out of range of valid values.");
		}
		aiskz(fileName, serverName, serverPort);
	}

	private static void aiskz(string p0, string p1, int p2)
	{
		MimeMessage mimeMessage = new MimeMessage();
		mimeMessage.Options = MimeOptions.DoNotParseMimeTree;
		mimeMessage.Load(p0);
		Send(mimeMessage, p1, p2);
	}

	public static void Send(string from, string to, string subject, string body, string serverName, int serverPort)
	{
		if (serverPort < 1 || serverPort > 65535)
		{
			throw hifyx.nztrs("serverPort", serverPort, "Port is out of range of valid values.");
		}
		uuknj(from, to, subject, body, serverName, serverPort);
	}

	private static void uuknj(string p0, string p1, string p2, string p3, string p4, int p5)
	{
		MimeMessage message = ljcln(p0, p1, p2, p3);
		Send(message, p4, p5);
	}

	public static void Send(MimeMessage message, string serverName)
	{
		Send(message, serverName, 25);
	}

	public static void Send(MailMessage message, string serverName)
	{
		Send(message, serverName, 25);
	}

	public static void Send(string fileName, string serverName)
	{
		Send(fileName, serverName, 25);
	}

	public static void Send(string from, string to, string subject, string body, string serverName)
	{
		Send(from, to, subject, body, serverName, 25);
	}

	public static void Send(MimeMessage message, SmtpConfiguration configuration)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		if (configuration == null || 1 == 0)
		{
			throw new ArgumentNullException("configuration");
		}
		if (configuration.DeliveryMethod != SmtpDeliveryMethod.PickupDirectory && (configuration.ServerName == null || false || configuration.ServerName.Length == 0 || 1 == 0))
		{
			throw new ArgumentException("Server name not specified in the configuration.", "configuration");
		}
		if (configuration.DeliveryMethod == SmtpDeliveryMethod.PickupDirectory)
		{
			string pickupDirectoryPath = configuration.PickupDirectoryPath;
			if (pickupDirectoryPath == null || 1 == 0)
			{
				throw new ArgumentException("Pickup directory not specified in the configuration.", "configuration");
			}
			MailSpool.Send(MailServerType.Iis, message, pickupDirectoryPath);
			return;
		}
		Smtp smtp = new Smtp();
		try
		{
			smtp.Settings = configuration.Settings.Clone();
			smtp.LogWriter = configuration.LogWriter;
			if (configuration.Proxy != null && 0 == 0)
			{
				smtp.Proxy = configuration.Proxy;
			}
			TlsParameters tlsParameters = configuration.Parameters;
			if (configuration.ClientCertificate != null && 0 == 0)
			{
				tlsParameters = ((tlsParameters != null) ? tlsParameters.Clone() : new TlsParameters());
				tlsParameters.CertificateRequestHandler = CertificateRequestHandler.CreateRequestHandler(configuration.ClientCertificate);
			}
			smtp.kitaa(configuration.ServerName, configuration.ServerPort, tlsParameters, configuration.SslMode);
			if (configuration.UserName != null && 0 == 0 && configuration.UserName.Length != 0 && 0 == 0 && configuration.Password != null && 0 == 0)
			{
				smtp.Login(configuration.UserName, configuration.Password, configuration.AuthenticationMethod);
			}
			smtp.Send(message);
			smtp.Disconnect();
		}
		finally
		{
			if (smtp != null && 0 == 0)
			{
				((IDisposable)smtp).Dispose();
			}
		}
	}

	public static void Send(MailMessage message, SmtpConfiguration configuration)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		Send(message.ToMimeMessage(), configuration);
	}

	public static void Send(string fileName, SmtpConfiguration configuration)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		if (fileName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "fileName");
		}
		gbxjw(fileName, configuration);
	}

	private static void gbxjw(string p0, SmtpConfiguration p1)
	{
		MimeMessage mimeMessage = new MimeMessage();
		mimeMessage.Options = MimeOptions.DoNotParseMimeTree;
		mimeMessage.Load(p0);
		Send(mimeMessage, p1);
	}

	public static void Send(string from, string to, string subject, string body, SmtpConfiguration configuration)
	{
		lwjif(from, to, subject, body, configuration);
	}

	private static void lwjif(string p0, string p1, string p2, string p3, SmtpConfiguration p4)
	{
		MimeMessage message = ljcln(p0, p1, p2, p3);
		Send(message, p4);
	}

	public static SmtpRejectedRecipient[] SendDirect(MailMessage message)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		return SendDirect(message.ToMimeMessage());
	}

	public static SmtpRejectedRecipient[] SendDirect(string from, string to, string subject, string body)
	{
		MimeMessage message = ljcln(from, to, subject, body);
		return SendDirect(message);
	}

	public static string[] ResolveDomainMX(string domain)
	{
		return ResolveDomainMX(domain, 10000);
	}

	public static string[] ResolveDomainMX(string domain, int timeout)
	{
		vektr vektr = new vektr();
		vektr.chdso = domain;
		if (dahxy.xzevd && 0 == 0)
		{
			throw new NotSupportedException("Method is not supported on this platform.");
		}
		zippk(LogLevel.Debug, "SMTP", "ResolveDomainMX('{0}', '{1}') started.", vektr.chdso, timeout);
		zippk(LogLevel.Debug, "SMTP", "Creating EndPoint for '{0}:25' of '{0}'.", vektr.chdso);
		IPEndPoint iPEndPoint = auilw.bolwk(vektr.chdso, 25);
		string[] array;
		int num;
		if (iPEndPoint == null || 1 == 0)
		{
			try
			{
				zippk(LogLevel.Debug, "SMTP", "Getting DNS list of '{0}'.", vektr.chdso);
				array = pvdtt.pkjxw();
			}
			catch (Exception innerException)
			{
				throw new SmtpException("Unable to determine DNS servers.", innerException, SmtpExceptionStatus.NameResolutionFailure);
			}
			zippk(LogLevel.Debug, "SMTP", "DNS returned {1} entries for '{0}'.", vektr.chdso, array.Length);
			if (array.Length != 0 && 0 == 0)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_0122;
				}
				goto IL_02ed;
			}
		}
		goto IL_02f6;
		IL_02ed:
		if (num < array.Length)
		{
			goto IL_0122;
		}
		goto IL_02f6;
		IL_02f6:
		zippk(LogLevel.Debug, "SMTP", "Starting Dns.GetHostEntry for '{0}'.", vektr.chdso);
		vektr.asdtn = null;
		vektr.porhw = null;
		vektr.rqcje = new ManualResetEvent(initialState: false);
		auilw.znhgl(vektr.chdso, vektr.eyxvv);
		if (!vektr.rqcje.WaitOne(timeout, exitContext: false) || 1 == 0)
		{
			throw new SmtpException("DNS query timed out.", SmtpExceptionStatus.Timeout);
		}
		vektr.rqcje.Close();
		if (vektr.porhw != null && 0 == 0)
		{
			throw vektr.porhw;
		}
		auilw.tulbp(vektr.asdtn, 25);
		return new string[1] { vektr.chdso };
		IL_0122:
		try
		{
			zippk(LogLevel.Debug, "SMTP", "Creating EndPoint for '{1}:25' of '{0}'.", vektr.chdso, array[num]);
			iPEndPoint = auilw.bolwk(array[num], 25);
			string[] array2;
			string[] array3;
			int num2;
			if (iPEndPoint != null)
			{
				zippk(LogLevel.Debug, "SMTP", "Resolving MX of '{0}'.", vektr.chdso, iPEndPoint.Address);
				array2 = ewxsp.dhjwv(iPEndPoint.Address, vektr.chdso, timeout);
				zippk(LogLevel.Debug, "SMTP", "Received {1} MX records for '{0}'.", vektr.chdso, array2.Length);
				if (array2.Length == 0 || 1 == 0)
				{
					goto IL_02f6;
				}
				array3 = array2;
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_0200;
				}
				goto IL_0239;
			}
			goto end_IL_0122;
			IL_0200:
			string text = array3[num2];
			zippk(LogLevel.Debug, "SMTP", " * '{1}' MX record for '{0}'.", vektr.chdso, text);
			num2++;
			goto IL_0239;
			IL_0239:
			if (num2 >= array3.Length)
			{
				return array2;
			}
			goto IL_0200;
			end_IL_0122:;
		}
		catch (SmtpException ex)
		{
			zippk(LogLevel.Debug, "SMTP", "An exception occurred for '{0}': {1}", vektr.chdso, ex);
			if (ex.Status != SmtpExceptionStatus.Timeout || num == array.Length - 1)
			{
				throw;
			}
		}
		catch (SocketException ex2)
		{
			int num3 = ex2.skehp();
			zippk(LogLevel.Debug, "SMTP", "Socket error {2} occurred for '{0}': {1}", vektr.chdso, ex2, num3);
			if (num == array.Length - 1)
			{
				throw new SmtpException("Unable to resolve DNS records.", ex2, SmtpExceptionStatus.NameResolutionFailure);
			}
		}
		num++;
		goto IL_02ed;
	}

	public static SmtpRejectedRecipient[] SendDirect(MimeMessage message)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		if (dahxy.xzevd && 0 == 0)
		{
			throw new NotSupportedException("Method is not supported on this platform.");
		}
		MailAddressCollection mailAddressCollection = new MailAddressCollection();
		mailAddressCollection.AddRange(message.To);
		mailAddressCollection.AddRange(message.CC);
		mailAddressCollection.AddRange(message.Bcc);
		Dictionary<string, MailAddressCollection> dictionary = new Dictionary<string, MailAddressCollection>(StringComparer.OrdinalIgnoreCase);
		int num = 0;
		if (num != 0)
		{
			goto IL_0070;
		}
		goto IL_00ed;
		IL_00ed:
		if (num >= mailAddressCollection.Count)
		{
			bxqhd bxqhd = new bxqhd();
			using (Dictionary<string, MailAddressCollection>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext() ? true : false)
				{
					KeyValuePair<string, MailAddressCollection> current = enumerator.Current;
					string key = current.Key;
					mailAddressCollection = current.Value;
					StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
					mailAddressCollection.Encode(stringWriter);
					string addresses = stringWriter.ToString();
					Smtp smtp = new Smtp();
					try
					{
						string[] array = ResolveDomainMX(key);
						bool flag = false;
						Exception ex = null;
						int num2;
						if (array != null && 0 == 0 && array.Length > 0)
						{
							num2 = 0;
							if (num2 != 0)
							{
								goto IL_0174;
							}
							goto IL_019c;
						}
						ex = new SmtpException("Unable to resolve domain.", SmtpExceptionStatus.NameResolutionFailure);
						smtp.tssug(ex);
						goto IL_01bc;
						IL_01bc:
						int num3;
						if (!flag || 1 == 0)
						{
							num3 = 0;
							if (num3 != 0)
							{
								goto IL_01cf;
							}
							goto IL_01eb;
						}
						smtp.RejectedRecipient += bxqhd.pyyod;
						smtp.xzaqz();
						try
						{
							smtp.zdiys();
							smtp.vcipv((MimeMessage)message.Clone(), null, null, new MailAddressCollection(addresses));
						}
						catch (Exception ex2)
						{
							int num4;
							if (ex2 is SmtpException ex3 && 0 == 0 && ((ex3.Response.Code == 552 && ex3.Response.Description.IndexOf("550 No such user") >= 0) || (ex3.Response.Code == 554 && ex3.Response.Description.IndexOf("yahoo.com account") >= 0)))
							{
								num4 = 0;
								if (num4 != 0)
								{
									goto IL_02b5;
								}
								goto IL_02d1;
							}
							smtp.tssug(ex2);
							throw;
							IL_02b5:
							bxqhd.uyeot(mailAddressCollection[num4].Address, ex3);
							num4++;
							goto IL_02d1;
							IL_02d1:
							if (num4 < mailAddressCollection.Count)
							{
								goto IL_02b5;
							}
							goto end_IL_014b;
						}
						finally
						{
							smtp.vevwx();
						}
						smtp.Disconnect();
						continue;
						IL_01eb:
						if (num3 >= mailAddressCollection.Count)
						{
							continue;
						}
						goto IL_01cf;
						IL_019c:
						if (num2 < array.Length)
						{
							goto IL_0174;
						}
						goto IL_01bc;
						IL_01cf:
						bxqhd.uyeot(mailAddressCollection[num3].Address, ex);
						num3++;
						goto IL_01eb;
						IL_0174:
						try
						{
							smtp.Connect(array[num2]);
							flag = true;
						}
						catch (SmtpException ex4)
						{
							ex = ex4;
							smtp.Disconnect();
							goto IL_0196;
						}
						goto IL_01bc;
						IL_0196:
						num2++;
						goto IL_019c;
						end_IL_014b:;
					}
					finally
					{
						smtp.Dispose();
					}
					break;
				}
			}
			return bxqhd.zvlxm;
		}
		goto IL_0070;
		IL_0070:
		MailAddress mailAddress = mailAddressCollection[num];
		if (mailAddress.Host != null && 0 == 0 && mailAddress.Host.Length != 0 && 0 == 0)
		{
			string host = mailAddress.Host;
			if (!dictionary.TryGetValue(host, out var value) || 1 == 0)
			{
				value = (dictionary[host] = new MailAddressCollection());
			}
			value.Add(new MailAddress(mailAddress.Address));
		}
		num++;
		goto IL_00ed;
	}

	private static object jrzti(object p0, Enum p1, object[] p2)
	{
		Smtp smtp = (Smtp)p0;
		switch ((qgzes)(object)p1)
		{
		case qgzes.nwsxa:
			return smtp.vbxqs((string)p2[0]);
		case qgzes.kaema:
			return smtp.encpd((string)p2[0], (int)p2[1], (TlsParameters)p2[2], (SslMode)p2[3]);
		case qgzes.cexhk:
			return smtp.cylsq();
		case qgzes.uvvua:
			smtp.ktnck((TlsParameters)p2[0]);
			return null;
		case qgzes.bcbzx:
			smtp.oexqv((MimeMessage)p2[0], (MailAddress)p2[1], (MailAddressCollection)p2[2]);
			return null;
		case qgzes.rbiid:
			smtp.bbsly((MailMessage)p2[0], (MailAddress)p2[1], (MailAddressCollection)p2[2]);
			return null;
		case qgzes.auvji:
			smtp.cmqdf((string)p2[0], (string)p2[1], (string)p2[2], (string)p2[3]);
			return null;
		case qgzes.fipmb:
			smtp.wwuev((Stream)p2[0], (string)p2[1], (string)p2[2]);
			return null;
		case qgzes.dsuml:
			smtp.zujhf((string)p2[0], (string)p2[1], (string)p2[2]);
			return null;
		case qgzes.mrmuh:
			smtp.kkcad((SmtpAuthentication)p2[0]);
			return null;
		case qgzes.vtdxh:
			smtp.lacal((GssApiProvider)p2[0]);
			return null;
		case qgzes.whlpx:
			smtp.rlbqd((string)p2[0], (string)p2[1], (SmtpAuthentication)p2[2]);
			return null;
		case qgzes.nehnv:
			return smtp.byzun((MailAddress)p2[0]);
		case qgzes.csxiu:
			smtp.yewdz((string)p2[0]);
			return null;
		case qgzes.drqtr:
			return smtp.ouakr();
		case qgzes.uzpuu:
			aiskz((string)p2[0], (string)p2[1], (int)p2[2]);
			return null;
		case qgzes.pxfwq:
			uuknj((string)p2[0], (string)p2[1], (string)p2[2], (string)p2[3], (string)p2[4], (int)p2[5]);
			return null;
		case qgzes.zgqju:
			gbxjw((string)p2[0], (SmtpConfiguration)p2[1]);
			return null;
		case qgzes.lffeu:
			lwjif((string)p2[0], (string)p2[1], (string)p2[2], (string)p2[3], (SmtpConfiguration)p2[4]);
			return null;
		default:
			throw new InvalidOperationException("Invalid asynchronous method.");
		}
	}

	private void miawp(IAsyncResult p0)
	{
		sufpd(p0, this, null, (qgzes)p0.AsyncState);
	}

	private static void fquwj(IAsyncResult p0)
	{
		sufpd(p0, null, null, (qgzes)p0.AsyncState);
	}

	public IAsyncResult BeginConnect(string serverName, AsyncCallback callback, object state)
	{
		return gtftg(serverName, 25, null, SslMode.None, callback, state);
	}

	public IAsyncResult BeginConnect(string serverName, int serverPort, AsyncCallback callback, object state)
	{
		return gtftg(serverName, serverPort, null, SslMode.None, callback, state);
	}

	[Obsolete("This overload of the Connect method has been deprecated and will be removed. Use another variant and specify parameters using Settings object instead.", true)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public IAsyncResult BeginConnect(string serverName, int serverPort, TlsParameters parameters, SmtpSecurity security, AsyncCallback callback, object state)
	{
		return gtftg(serverName, serverPort, parameters, (SslMode)security, callback, state);
	}

	public IAsyncResult BeginConnect(string serverName, int serverPort, SslMode security, AsyncCallback callback, object state)
	{
		return gtftg(serverName, serverPort, null, security, callback, state);
	}

	public IAsyncResult BeginConnect(string serverName, SslMode security, AsyncCallback callback, object state)
	{
		return gtftg(serverName, 0, null, security, callback, state);
	}

	public string EndConnect(IAsyncResult asyncResult)
	{
		return kevaj(asyncResult, "Connect");
	}

	internal IAsyncResult gtftg(string p0, int p1, TlsParameters p2, SslMode p3, AsyncCallback p4, object p5)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("serverName");
		}
		p0 = p0.Trim();
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Hostname cannot be empty.", "serverName");
		}
		if (p1 == 0 || 1 == 0)
		{
			p1 = p3 switch
			{
				SslMode.Implicit => 465, 
				SslMode.Explicit => 587, 
				_ => 25, 
			};
		}
		if (!auilw.pogqj(p0) || 1 == 0)
		{
			throw new ArgumentException("Hostname is invalid.", "serverName");
		}
		if (p1 < 1 || p1 > 65535)
		{
			throw hifyx.nztrs("serverPort", p1, "Port is out of range of valid values.");
		}
		try
		{
			nhmfm(rgtlm.tvbqu());
		}
		catch (fwwdw fwwdw)
		{
			throw new SmtpException(fwwdw.Message);
		}
		return chtfa(false, this, qgzes.kaema, p4, p5, p0, p1, p2, p3);
	}

	private string kevaj(IAsyncResult p0, string p1)
	{
		return (string)sufpd(p0, this, p1, qgzes.kaema);
	}

	public IAsyncResult BeginDisconnect(AsyncCallback callback, object state)
	{
		return chtfa(false, this, qgzes.cexhk, callback, state);
	}

	public string EndDisconnect(IAsyncResult asyncResult)
	{
		return (string)sufpd(asyncResult, this, "Disconnect", qgzes.cexhk);
	}

	[wptwl(false)]
	[Obsolete("This overload of the Secure method has been deprecated and will be removed. Use another variant and specify parameters using Settings object instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public IAsyncResult BeginSecure(TlsParameters parameters, AsyncCallback callback, object state)
	{
		return chtfa(false, this, qgzes.uvvua, callback, state, parameters);
	}

	public IAsyncResult BeginSecure(AsyncCallback callback, object state)
	{
		object[] p = new object[1];
		return chtfa(p0: false, this, qgzes.uvvua, callback, state, p);
	}

	public void EndSecure(IAsyncResult asyncResult)
	{
		sufpd(asyncResult, this, "Secure", qgzes.uvvua);
	}

	public IAsyncResult BeginSend(MimeMessage mail, string sender, string recipients, AsyncCallback callback, object state)
	{
		if (mail == null || 1 == 0)
		{
			throw new ArgumentNullException("mail");
		}
		qipiy(sender, recipients, out var p, out var p2, Settings.AllowNullSender);
		return chtfa(false, this, qgzes.bcbzx, callback, state, mail, p, p2);
	}

	public IAsyncResult BeginSend(MailMessage mail, string sender, string recipients, AsyncCallback callback, object state)
	{
		if (mail == null || 1 == 0)
		{
			throw new ArgumentNullException("mail");
		}
		qipiy(sender, recipients, out var p, out var p2, Settings.AllowNullSender);
		return chtfa(false, this, qgzes.rbiid, callback, state, mail, p, p2);
	}

	public IAsyncResult BeginSend(MimeMessage mail, AsyncCallback callback, object state)
	{
		return BeginSend(mail, null, null, callback, state);
	}

	public IAsyncResult BeginSend(MailMessage mail, AsyncCallback callback, object state)
	{
		return BeginSend(mail, null, null, callback, state);
	}

	public IAsyncResult BeginSend(string from, string to, string subject, string body, AsyncCallback callback, object state)
	{
		return chtfa(false, this, qgzes.auvji, callback, state, from, to, subject, body);
	}

	public IAsyncResult BeginSend(Stream input, string sender, string recipients, AsyncCallback callback, object state)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		return chtfa(false, this, qgzes.fipmb, callback, state, input, sender, recipients);
	}

	public IAsyncResult BeginSend(Stream input, AsyncCallback callback, object state)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		return chtfa(false, this, qgzes.fipmb, callback, state, input, null, null);
	}

	public IAsyncResult BeginSend(string fileName, AsyncCallback callback, object state)
	{
		return BeginSend(fileName, null, null, callback, state);
	}

	public IAsyncResult BeginSend(string fileName, string sender, string recipients, AsyncCallback callback, object state)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		if (fileName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "fileName");
		}
		return chtfa(false, this, qgzes.dsuml, callback, state, fileName, sender, recipients);
	}

	public void EndSend(IAsyncResult asyncResult)
	{
		sufpd(asyncResult, this, "Send", qgzes.bcbzx, qgzes.rbiid, qgzes.auvji, qgzes.fipmb, qgzes.dsuml);
	}

	public IAsyncResult BeginLogin(SmtpAuthentication method, AsyncCallback callback, object state)
	{
		return chtfa(false, this, qgzes.mrmuh, callback, state, method);
	}

	public IAsyncResult BeginLogin(GssApiProvider provider, AsyncCallback callback, object state)
	{
		return chtfa(false, this, qgzes.vtdxh, callback, state, provider);
	}

	public IAsyncResult BeginLogin(string userName, string password, SmtpAuthentication method, AsyncCallback callback, object state)
	{
		if (userName == null || 1 == 0)
		{
			throw new ArgumentNullException("userName");
		}
		if (userName.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "userName");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		return chtfa(false, this, qgzes.whlpx, callback, state, userName, password, method);
	}

	public IAsyncResult BeginLogin(string token, SmtpAuthentication method, AsyncCallback callback, object state)
	{
		if (token == null || 1 == 0)
		{
			throw new ArgumentNullException("token");
		}
		if (token.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "token");
		}
		if (method != SmtpAuthentication.OAuth20)
		{
			throw new SmtpException(brgjd.edcru("Authentication method '{0}' is not token-based.", method), SmtpExceptionStatus.OperationFailure);
		}
		return chtfa(false, this, qgzes.whlpx, callback, state, null, token, method);
	}

	public IAsyncResult BeginLogin(string userName, string password, AsyncCallback callback, object state)
	{
		return BeginLogin(userName, password, SmtpAuthentication.Auto, callback, state);
	}

	public void EndLogin(IAsyncResult asyncResult)
	{
		sufpd(asyncResult, this, "Login", qgzes.mrmuh, qgzes.vtdxh, qgzes.whlpx);
	}

	[Obsolete("This method is seldom useful and has been deprecated", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public IAsyncResult BeginVerify(string to, AsyncCallback callback, object state)
	{
		if (to == null || 1 == 0)
		{
			throw new ArgumentNullException("to");
		}
		if (to.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "to");
		}
		int num = to.IndexOf('@');
		int num2 = to.LastIndexOf('@');
		if (num != num2)
		{
			throw new ArgumentException("A specified mailbox is invalid.", "to");
		}
		MailAddress mailAddress;
		try
		{
			mailAddress = new MailAddress(to);
		}
		catch (MimeException innerException)
		{
			throw new ArgumentException("A specified mailbox is invalid.", innerException);
		}
		string host = mailAddress.Host;
		string user = mailAddress.User;
		if (host.Length == 0 || false || user.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("A specified mailbox is invalid.", "to");
		}
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_00be;
		}
		goto IL_0119;
		IL_015e:
		int num4;
		if (num4 < user.Length)
		{
			goto IL_012b;
		}
		return chtfa(false, this, qgzes.nehnv, callback, state, mailAddress);
		IL_012b:
		char c = user[num4];
		if (c < ' ' || c >= '\u007f' || c == '@')
		{
			throw new ArgumentException("A specified mailbox is invalid.", "to");
		}
		num4++;
		goto IL_015e;
		IL_00be:
		char c2 = host[num3];
		if ((c2 < 'a' || c2 > 'z') && (c2 < 'A' || c2 > 'Z') && (c2 < '0' || c2 > '9'))
		{
			switch (c2)
			{
			default:
				throw new ArgumentException("A specified mailbox is invalid.", "to");
			case '-':
			case '.':
				break;
			}
		}
		num3++;
		goto IL_0119;
		IL_0119:
		if (num3 < host.Length)
		{
			goto IL_00be;
		}
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_012b;
		}
		goto IL_015e;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This method is seldom useful and has been deprecated", false)]
	public SmtpResponse EndVerify(IAsyncResult asyncResult)
	{
		return (SmtpResponse)sufpd(asyncResult, this, "Verify", qgzes.nehnv);
	}

	public IAsyncResult BeginSendCommand(string command, AsyncCallback callback, object state)
	{
		if (command == null || 1 == 0)
		{
			throw new ArgumentNullException("command");
		}
		if (command.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Command has zero length.", "command");
		}
		return chtfa(false, this, qgzes.csxiu, callback, state, command);
	}

	public void EndSendCommand(IAsyncResult asyncResult)
	{
		sufpd(asyncResult, this, "SendCommand", qgzes.csxiu);
	}

	public IAsyncResult BeginReadResponse(AsyncCallback callback, object state)
	{
		return chtfa(false, this, qgzes.drqtr, callback, state);
	}

	public SmtpResponse EndReadResponse(IAsyncResult asyncResult)
	{
		return (SmtpResponse)sufpd(asyncResult, this, "ReadResponse", qgzes.drqtr);
	}

	public IAsyncResult BeginKeepAlive(AsyncCallback callback, object state)
	{
		return chtfa(false, this, qgzes.nwsxa, callback, state, "NOOP");
	}

	public void EndKeepAlive(IAsyncResult asyncResult)
	{
		sufpd(asyncResult, this, "KeepAlive", qgzes.nwsxa);
	}

	[Obsolete("This method is seldom useful and has been deprecated", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public IAsyncResult BeginEnhancedTurn(string nodeName, AsyncCallback callback, object state)
	{
		if (nodeName == null || 1 == 0)
		{
			throw new ArgumentNullException("nodeName");
		}
		if (nodeName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "nodeName");
		}
		return chtfa(false, this, qgzes.nwsxa, callback, state, "ETRN " + nodeName);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This method is seldom useful and has been deprecated", false)]
	public void EndEnhancedTurn(IAsyncResult asyncResult)
	{
		sufpd(asyncResult, this, "EnhancedTurn", qgzes.nwsxa);
	}

	private static Exception vvrhh(string p0)
	{
		return new SmtpException(p0, SmtpExceptionStatus.OperationFailure);
	}
}
