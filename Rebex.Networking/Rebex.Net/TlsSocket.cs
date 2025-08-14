using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public class TlsSocket : ISocketExt, ISocket, phvuu, IDisposable
{
	private class srxgp : ISocketFactory
	{
		public ISocket CreateSocket()
		{
			return new TlsSocket();
		}
	}

	private sealed class bfuwf
	{
		public TlsSocket ikgqj;

		public byte[] wvrim;

		public int hyhro;

		public int otlvh;

		public SocketFlags nfzen;

		public int vjash()
		{
			return ikgqj.Send(wvrim, hyhro, otlvh, nfzen);
		}
	}

	private sealed class fvdwn
	{
		public TlsSocket lvnwb;

		public byte[] jipew;

		public int bvnil;

		public int odqiv;

		public SocketFlags swzsi;

		public int ecdet()
		{
			return lvnwb.Receive(jipew, bvnil, odqiv, swzsi);
		}
	}

	private const string itirm = "Async methods are not supported by the TlsSocket class. Please use TlsClientSocket class instead.";

	private ISocket czfze;

	private readonly vffxo skgjm = new vffxo(-1);

	private pxaob ssgvt;

	private bool oztju;

	private TlsParameters okegw;

	private ILogWriter ybwgu;

	private readonly awngk juoge;

	private ridny yyaom;

	private ridny dgocm;

	private readonly int cduem;

	private readonly string nqpsh;

	private string dtteq;

	private TlsSocket mxjud;

	private mggni bsobw;

	private rdvtd hnsea;

	private static int dzxdv;

	private EventHandler<TlsDebugEventArgs> esxpe;

	private TlsDebugLevel wsrpw = TlsDebugLevel.Important;

	private static srxgp sdrul = new srxgp();

	private object kdwxg;

	private int jmyvi;

	public object Context
	{
		get
		{
			return kdwxg;
		}
		set
		{
			kdwxg = value;
		}
	}

	public Socket Socket => czfze.ygaej().kqgyp.xjbxa(null);

	public int Timeout
	{
		get
		{
			return skgjm.eqjbt;
		}
		set
		{
			skgjm.ltwwl(value);
			ISocket socket = czfze;
			if (socket != null && 0 == 0)
			{
				socket.Timeout = skgjm.aeqtf;
			}
		}
	}

	public EndPoint RemoteEndPoint => czfze.RemoteEndPoint;

	public EndPoint LocalEndPoint => czfze.LocalEndPoint;

	public SocketInformation Information => new SocketInformation(czfze);

	public bool Connected
	{
		get
		{
			pxaob pxaob = ssgvt;
			if (pxaob != null && 0 == 0)
			{
				return pxaob.cfvix;
			}
			return czfze.Connected;
		}
	}

	public int Available
	{
		get
		{
			njvzu<int> njvzu = hnsea.knwel();
			njvzu.xgngc();
			return njvzu.islme;
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("SessionID property has been deprecated and will be removed. Use the Session property instead.", true)]
	public string SessionID
	{
		get
		{
			pxaob pxaob = ssgvt;
			if (pxaob != null && 0 == 0)
			{
				return pxaob.obkur;
			}
			return null;
		}
	}

	public TlsSession Session
	{
		get
		{
			pxaob pxaob = ssgvt;
			if (pxaob != null && 0 == 0)
			{
				return pxaob.swkdt();
			}
			return null;
		}
	}

	public TlsCipher Cipher
	{
		get
		{
			pxaob pxaob = ssgvt;
			if (pxaob != null && 0 == 0)
			{
				return pxaob.qmszr;
			}
			return TlsCipher.tpdwy;
		}
	}

	public CertificateChain ServerCertificate
	{
		get
		{
			pxaob pxaob = ssgvt;
			if (pxaob != null && 0 == 0)
			{
				return pxaob.vqoau;
			}
			return null;
		}
	}

	public CertificateChain ClientCertificate
	{
		get
		{
			pxaob pxaob = ssgvt;
			if (pxaob != null && 0 == 0)
			{
				return pxaob.eqpjs;
			}
			return null;
		}
	}

	public bool IsSecure
	{
		get
		{
			if (ssgvt == null || 1 == 0)
			{
				return false;
			}
			return true;
		}
	}

	public TlsConnectionEnd Entity => okegw.Entity;

	public TlsCompressionMethod CompressionMethod => TlsCompressionMethod.None;

	internal awngk zznhh => juoge;

	internal int pxzpp => cduem;

	public TlsParameters Parameters
	{
		get
		{
			return ajfrn();
		}
		set
		{
			qzczp(value);
		}
	}

	[Obsolete("TlsDebug event API has been deprecated and will be removed. Use LogWriter instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public TlsDebugLevel DebugLevel
	{
		get
		{
			return wsrpw;
		}
		set
		{
			wsrpw = value;
		}
	}

	internal int ywlea
	{
		get
		{
			return jmyvi;
		}
		private set
		{
			jmyvi = value;
		}
	}

	public ILogWriter LogWriter
	{
		get
		{
			return ybwgu;
		}
		set
		{
			ybwgu = value;
			pxaob pxaob = ssgvt;
			if (pxaob != null && 0 == 0)
			{
				pxaob.sfzgr = value;
			}
		}
	}

	public ISocketFactory Factory => sdrul;

	internal TlsSocket xlbxi
	{
		get
		{
			TlsSocket tlsSocket = mxjud;
			if (tlsSocket == null || 1 == 0)
			{
				tlsSocket = this;
			}
			return tlsSocket;
		}
		set
		{
			mxjud = value;
		}
	}

	[wptwl(false)]
	[Obsolete("TlsDebug event API has been deprecated and will be removed. Use LogWriter instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler<TlsDebugEventArgs> Debug
	{
		add
		{
			jbkzi += value;
		}
		remove
		{
			jbkzi -= value;
		}
	}

	internal event EventHandler<TlsDebugEventArgs> jbkzi
	{
		add
		{
			EventHandler<TlsDebugEventArgs> eventHandler = esxpe;
			EventHandler<TlsDebugEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TlsDebugEventArgs> value2 = (EventHandler<TlsDebugEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref esxpe, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<TlsDebugEventArgs> eventHandler = esxpe;
			EventHandler<TlsDebugEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TlsDebugEventArgs> value2 = (EventHandler<TlsDebugEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref esxpe, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	private ISocketExt ejbun()
	{
		if (!(czfze is ISocketExt result) || 1 == 0)
		{
			throw new NotSupportedException("This functionality is not supported by this socket type.");
		}
		return result;
	}

	public SocketState GetConnectionState()
	{
		if (todgf.gnzjs(czfze) == SocketState.Connected)
		{
			return SocketState.Connected;
		}
		pxaob pxaob = ssgvt;
		if (pxaob != null && 0 == 0)
		{
			if (pxaob.dgctd && 0 == 0)
			{
				return SocketState.Connected;
			}
			if (pxaob.nvoco > 0)
			{
				return SocketState.Connected;
			}
		}
		return SocketState.NotConnected;
	}

	public bool Poll(int microSeconds, SocketSelectMode mode)
	{
		pxaob pxaob = ssgvt;
		if (pxaob != null && 0 == 0)
		{
			while (true)
			{
				if (pxaob.nvoco > 0)
				{
					return true;
				}
				if (!pxaob.dgctd || 1 == 0)
				{
					if (pxaob.mgfog == hlkgm.tqqib)
					{
						return true;
					}
					if (!czfze.Poll(microSeconds, mode) || 1 == 0)
					{
						break;
					}
				}
				pxaob.upurk();
			}
			return false;
		}
		return czfze.Poll(microSeconds, mode);
	}

	private bool jktjn(int p0)
	{
		return Poll(p0, SocketSelectMode.SelectRead);
	}

	bool phvuu.hznqz(int p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jktjn
		return this.jktjn(p0);
	}

	internal void xjtph()
	{
		if (oztju && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
	}

	public override string ToString()
	{
		return nqpsh;
	}

	public override int GetHashCode()
	{
		return cduem;
	}

	internal virtual TlsParameters ajfrn()
	{
		return okegw;
	}

	internal virtual void qzczp(TlsParameters p0)
	{
		if (okegw.ikufo && 0 == 0)
		{
			throw new InvalidOperationException("Cannot change parameters that are being used for a socket at the moment.");
		}
		if (p0 == null || 1 == 0)
		{
			p0 = new TlsParameters();
		}
		if (p0.ikufo && 0 == 0)
		{
			okegw = p0.Clone();
		}
		else
		{
			okegw = p0;
		}
	}

	private void xfaqk(TlsDebugEventType p0, TlsDebugEventSource p1, TlsDebugLevel p2)
	{
		switch (p0)
		{
		case TlsDebugEventType.Alert:
			return;
		case TlsDebugEventType.ResumingCachedSession:
			ybwgu.Write(LogLevel.Info, typeof(TlsSocket), czfze.GetHashCode(), "TLS", "Resuming session.");
			return;
		}
		TlsDebugEventGroup tlsDebugEventGroup = (TlsDebugEventGroup)((int)p0 >> 16);
		StringBuilder stringBuilder = new StringBuilder();
		switch (tlsDebugEventGroup)
		{
		case TlsDebugEventGroup.Info:
			stringBuilder.Append("Info ");
			break;
		case TlsDebugEventGroup.StateChange:
			if (p0 == TlsDebugEventType.Secured)
			{
				ybwgu.Write(LogLevel.Info, typeof(TlsSocket), czfze.GetHashCode(), "TLS", brgjd.edcru("Connection secured using cipher: {0}.", Cipher));
				byte[] tjkch = ssgvt.tjkch;
				ybwgu.Write(LogLevel.Verbose, typeof(TlsSocket), czfze.GetHashCode(), "TLS", "Session ID: ", tjkch, 0, tjkch.Length);
			}
			return;
		}
		stringBuilder.Append(trgij(tlsDebugEventGroup));
		stringBuilder.Append(':');
		stringBuilder.Append(oxiuw(p0));
		switch (p1)
		{
		case TlsDebugEventSource.Sent:
			stringBuilder.Append(" was sent.");
			break;
		case TlsDebugEventSource.Received:
			stringBuilder.Append(" was received.");
			break;
		}
		ybwgu.Write((p2 == TlsDebugLevel.Important) ? LogLevel.Info : LogLevel.Debug, typeof(TlsSocket), czfze.GetHashCode(), "TLS", stringBuilder.ToString());
	}

	private static string trgij(TlsDebugEventGroup p0)
	{
		return p0 switch
		{
			TlsDebugEventGroup.Info => "Info", 
			TlsDebugEventGroup.HandshakeMessage => "HandshakeMessage", 
			TlsDebugEventGroup.Alert => "Alert", 
			TlsDebugEventGroup.StateChange => "StateChange", 
			TlsDebugEventGroup.CipherSpec => "CipherSpec", 
			_ => "Group" + p0, 
		};
	}

	private static string oxiuw(TlsDebugEventType p0)
	{
		return p0 switch
		{
			TlsDebugEventType.ResumingCachedSession => "ResumingCachedSession", 
			TlsDebugEventType.UnknownMessageType => "UnknownMessageType", 
			TlsDebugEventType.UnexpectedException => "UnexpectedException", 
			TlsDebugEventType.HelloRequest => "HelloRequest", 
			TlsDebugEventType.ClientHello => "ClientHello", 
			TlsDebugEventType.ServerHello => "ServerHello", 
			TlsDebugEventType.Certificate => "Certificate", 
			TlsDebugEventType.ServerKeyExchange => "ServerKeyExchange", 
			TlsDebugEventType.CertificateRequest => "CertificateRequest", 
			TlsDebugEventType.ServerHelloDone => "ServerHelloDone", 
			TlsDebugEventType.CertificateVerify => "CertificateVerify", 
			TlsDebugEventType.ClientKeyExchange => "ClientKeyExchange", 
			TlsDebugEventType.Finished => "Finished", 
			TlsDebugEventType.UnknownHandshakeMessage => "UnknownHandshakeMessage", 
			TlsDebugEventType.Alert => "Alert", 
			TlsDebugEventType.Negotiating => "Negotiating", 
			TlsDebugEventType.Secured => "Secured", 
			TlsDebugEventType.Closed => "Closed", 
			TlsDebugEventType.ChangeCipherSpec => "ChangeCipherSpec", 
			_ => "Type" + p0, 
		};
	}

	[Obsolete("TlsDebug event API has been deprecated and will be removed. Use LogWriter instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	protected virtual void OnDebug(TlsDebugEventArgs e)
	{
		EventHandler<TlsDebugEventArgs> eventHandler = esxpe;
		if (eventHandler != null && 0 == 0)
		{
			eventHandler(this, e);
		}
	}

	internal void ivtmn(TlsDebugEventType p0, TlsDebugEventSource p1, TlsDebugLevel p2)
	{
		if (ybwgu != null && 0 == 0 && ybwgu.Level <= LogLevel.Info)
		{
			xfaqk(p0, p1, p2);
		}
		if (p2 <= wsrpw)
		{
			EventHandler<TlsDebugEventArgs> eventHandler = esxpe;
			if (eventHandler != null)
			{
				TlsDebugEventArgs e = new TlsDebugEventArgs(p0, p1, p2);
				eventHandler(this, e);
			}
		}
	}

	internal void jkyaz(TlsDebugEventType p0, TlsDebugEventSource p1, TlsDebugLevel p2, qoqui p3)
	{
		if (ybwgu != null && 0 == 0 && ybwgu.Level <= LogLevel.Info)
		{
			xfaqk(p0, p1, p2);
		}
		if (p2 <= wsrpw)
		{
			EventHandler<TlsDebugEventArgs> eventHandler = esxpe;
			if (eventHandler != null)
			{
				TlsDebugEventArgs e = new TlsDebugEventArgs(p0, p1, p2, p3);
				eventHandler(this, e);
			}
		}
	}

	internal void kxlvu(TlsDebugEventType p0, TlsDebugEventSource p1, TlsDebugLevel p2, byte[] p3, int p4, int p5)
	{
		if (ybwgu != null && 0 == 0 && ybwgu.Level <= LogLevel.Info)
		{
			xfaqk(p0, p1, p2);
		}
		if (p2 <= wsrpw)
		{
			EventHandler<TlsDebugEventArgs> eventHandler = esxpe;
			if (eventHandler != null)
			{
				TlsDebugEventArgs e = new TlsDebugEventArgs(p0, p1, p2, p3, p4, p5);
				eventHandler(this, e);
			}
		}
	}

	public TlsSocket()
		: this(new ProxySocket())
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public TlsSocket(AddressFamily addressFamily)
		: this()
	{
	}

	public TlsSocket(Socket socket)
		: this(new bavnf(socket))
	{
		if (socket.SocketType != SocketType.Stream)
		{
			throw new ArgumentException("Only stream sockets are supported.", "socket");
		}
		if (!socket.Blocking || 1 == 0)
		{
			throw new ArgumentException("Non-blocking sockets are not supported.", "socket");
		}
		if (socket.ProtocolType == ProtocolType.Tcp)
		{
			return;
		}
		throw new ArgumentException("Only TCP sockets are supported.", "socket");
	}

	public TlsSocket(ISocket socket)
	{
		czfze = socket;
		okegw = new TlsParameters();
		ywlea = ((czfze == null) ? GetHashCode() : czfze.GetHashCode());
		bsobw = null;
		hnsea = rdvtd.omrjr(czfze);
		cduem = Interlocked.Increment(ref dzxdv);
		nqpsh = brgjd.edcru("{0}({1})", GetType().Name, cduem);
		juoge = new awngk(GetType(), cduem);
		yyaom = new ridny();
		dgocm = new ridny();
	}

	public int Send(byte[] buffer)
	{
		dahxy.gbxkl(buffer, "buffer");
		return Send(buffer, 0, buffer.Length, SocketFlags.None);
	}

	public int Send(byte[] buffer, SocketFlags socketFlags)
	{
		dahxy.gbxkl(buffer, "buffer");
		return Send(buffer, 0, buffer.Length, socketFlags);
	}

	public int Send(byte[] buffer, int size, SocketFlags socketFlags)
	{
		return Send(buffer, 0, size, socketFlags);
	}

	public int Send(byte[] buffer, int offset, int size)
	{
		return Send(buffer, offset, size, SocketFlags.None);
	}

	public int Send(ArraySegment<byte> buffer)
	{
		return Send(buffer.Array, buffer.Offset, buffer.Count, SocketFlags.None);
	}

	public int Receive(byte[] buffer)
	{
		dahxy.gbxkl(buffer, "buffer");
		return Receive(buffer, 0, buffer.Length, SocketFlags.None);
	}

	public int Receive(byte[] buffer, SocketFlags socketFlags)
	{
		dahxy.gbxkl(buffer, "buffer");
		return Receive(buffer, 0, buffer.Length, socketFlags);
	}

	public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
	{
		return Receive(buffer, 0, size, socketFlags);
	}

	public int Receive(byte[] buffer, int offset, int size)
	{
		return Receive(buffer, offset, size, SocketFlags.None);
	}

	public int Receive(ArraySegment<byte> buffer)
	{
		return Receive(buffer.Array, buffer.Offset, buffer.Count, SocketFlags.None);
	}

	public void Negotiate()
	{
		pxaob pxaob = ssgvt;
		if (pxaob != null && 0 == 0)
		{
			throw new InvalidOperationException("Socket is already secured.");
		}
		okegw.ijvgu();
		hcuvh(p0: false);
		pxaob = (ssgvt = new pxaob(this, czfze, okegw, dtteq));
		pxaob.elbgc = skgjm;
		pxaob.sfzgr = ybwgu;
		pxaob.sksvo();
		hnsea.ipebe = pxaob;
	}

	public void Renegotiate()
	{
		pxaob pxaob = ssgvt;
		if (pxaob == null || 1 == 0)
		{
			throw new InvalidOperationException("Socket has not been secured yet.");
		}
		pxaob.sksvo();
	}

	internal virtual void hcuvh(bool p0)
	{
		try
		{
			ongpx.bjvdq(iezrl());
		}
		catch (fwwdw fwwdw)
		{
			throw new TlsException(fwwdw.Message);
		}
		okegw = okegw.tcdgr();
	}

	internal virtual rsljk iezrl()
	{
		return lzles.jksyb();
	}

	public void Unprotect()
	{
		pxaob pxaob = ssgvt;
		ssgvt = null;
		if (pxaob == null || 1 == 0)
		{
			throw new InvalidOperationException("Socket has not been secured yet.");
		}
		pxaob.jumxv();
		hnsea.ipebe = null;
	}

	public void Connect(EndPoint remoteEP)
	{
		dahxy.gbxkl(remoteEP, "remoteEP");
		dtteq = todgf.innqj(remoteEP);
		xcqzm(remoteEP);
	}

	internal virtual void xcqzm(EndPoint p0)
	{
		czfze.Connect(p0);
	}

	public void Connect(string serverName, int serverPort)
	{
		dahxy.mydnv(serverName, "serverName");
		dahxy.kykxy(serverPort, "serverPort");
		dtteq = serverName;
		iwqmt(serverName, serverPort);
	}

	internal virtual void iwqmt(string p0, int p1)
	{
		czfze.Connect(p0, p1);
	}

	public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
	{
		dahxy.gbxkl(remoteEP, "remoteEP");
		dtteq = todgf.innqj(remoteEP);
		return oiaos(remoteEP, callback, state);
	}

	internal virtual IAsyncResult oiaos(EndPoint p0, AsyncCallback p1, object p2)
	{
		return ejbun().BeginConnect(p0, p1, p2);
	}

	public IAsyncResult BeginConnect(string serverName, int serverPort, AsyncCallback callback, object state)
	{
		dahxy.mydnv(serverName, "serverName");
		dahxy.kykxy(serverPort, "serverPort");
		dtteq = serverName;
		return wbetw(serverName, serverPort, callback, state);
	}

	internal virtual IAsyncResult wbetw(string p0, int p1, AsyncCallback p2, object p3)
	{
		return ejbun().BeginConnect(p0, p1, p2, p3);
	}

	public void EndConnect(IAsyncResult asyncResult)
	{
		aayyx(asyncResult);
	}

	internal virtual void aayyx(IAsyncResult p0)
	{
		ejbun().EndConnect(p0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public EndPoint Listen(ISocket controlSocket)
	{
		cqoxa();
		if (controlSocket is TlsSocket tlsSocket && 0 == 0)
		{
			controlSocket = tlsSocket.czfze;
		}
		return ejbun().Listen(controlSocket);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public IAsyncResult BeginListen(ISocket controlSocket, AsyncCallback callback, object state)
	{
		cqoxa();
		if (controlSocket is TlsSocket tlsSocket && 0 == 0)
		{
			controlSocket = tlsSocket.czfze;
		}
		return ejbun().BeginListen(controlSocket, callback, state);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public EndPoint EndListen(IAsyncResult asyncResult)
	{
		cqoxa();
		return ejbun().EndListen(asyncResult);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ISocket Accept()
	{
		xvmls();
		ISocket socket = ejbun().Accept();
		return new TlsSocket(socket);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public IAsyncResult BeginAccept(AsyncCallback callback, object state)
	{
		xvmls();
		return ejbun().BeginAccept(callback, state);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ISocket EndAccept(IAsyncResult asyncResult)
	{
		xvmls();
		ISocket socket = ejbun().EndAccept(asyncResult);
		return new TlsSocket(socket);
	}

	internal virtual void cqoxa()
	{
	}

	internal virtual void xvmls()
	{
	}

	public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		dahxy.valft(buffer, "buffer", offset, "offset", size, "size");
		if (!dgocm.qxocb() || 1 == 0)
		{
			throw new InvalidOperationException("Another Send operation is in progress.");
		}
		try
		{
			pxaob pxaob = ssgvt;
			if (pxaob == null || 1 == 0)
			{
				return czfze.Send(buffer, offset, size, socketFlags);
			}
			pxaob.hzfdo(buffer, offset, size);
			return size;
		}
		finally
		{
			dgocm.fafuc();
		}
	}

	public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		bfuwf bfuwf = new bfuwf();
		bfuwf.wvrim = buffer;
		bfuwf.hyhro = offset;
		bfuwf.otlvh = size;
		bfuwf.nfzen = socketFlags;
		bfuwf.ikgqj = this;
		dahxy.valft(bfuwf.wvrim, "buffer", bfuwf.hyhro, "offset", bfuwf.otlvh, "size");
		return rxpjc.tzeev(bfuwf.vjash, callback, state);
	}

	public int EndSend(IAsyncResult asyncResult)
	{
		return rxpjc.wzgzd<int>(asyncResult);
	}

	public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		dahxy.valft(buffer, "buffer", offset, "offset", size, "size");
		if (!yyaom.qxocb() || 1 == 0)
		{
			throw new InvalidOperationException("Another Receive operation is in progress.");
		}
		try
		{
			pxaob pxaob = ssgvt;
			if (pxaob == null || 1 == 0)
			{
				return czfze.Receive(buffer, offset, size, socketFlags);
			}
			return pxaob.ozzpy(buffer, offset, size, p3: false);
		}
		finally
		{
			yyaom.fafuc();
		}
	}

	internal int dnszr(byte[] p0, int p1, int p2, bool p3)
	{
		dahxy.valft(p0, "buffer", p1, "offset", p2, "size");
		if (!yyaom.qxocb() || 1 == 0)
		{
			throw new InvalidOperationException("Another Receive operation is in progress.");
		}
		try
		{
			pxaob pxaob = ssgvt;
			if (pxaob == null || 1 == 0)
			{
				throw new InvalidOperationException("Non-blocking receive is available only for TLS connections.");
			}
			return pxaob.ozzpy(p0, p1, p2, p3);
		}
		finally
		{
			yyaom.fafuc();
		}
	}

	public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		fvdwn fvdwn = new fvdwn();
		fvdwn.jipew = buffer;
		fvdwn.bvnil = offset;
		fvdwn.odqiv = size;
		fvdwn.swzsi = socketFlags;
		fvdwn.lvnwb = this;
		dahxy.valft(fvdwn.jipew, "buffer", fvdwn.bvnil, "offset", fvdwn.odqiv, "size");
		return rxpjc.tzeev(fvdwn.ecdet, callback, state);
	}

	public int EndReceive(IAsyncResult asyncResult)
	{
		return rxpjc.wzgzd<int>(asyncResult);
	}

	public IAsyncResult BeginNegotiate(AsyncCallback callback, object state)
	{
		exkzi p = ooesf();
		return ihlbr.fgnul(callback, state, p);
	}

	public void EndNegotiate(IAsyncResult asyncResult)
	{
		ihlbr.reuwd(asyncResult);
	}

	public object GetObject(object arg)
	{
		pxaob pxaob = ssgvt;
		if (pxaob == null || 1 == 0)
		{
			return null;
		}
		if (arg is string text && 0 == 0)
		{
			if (text == "ReadStateCipher")
			{
				return pxaob.xsiiu.qctmg;
			}
			if (text == "WriteStateCipher")
			{
				return pxaob.ytkdw.qctmg;
			}
		}
		return null;
	}

	public void Shutdown(SocketShutdown how)
	{
		czfze.Shutdown(how);
	}

	public void Close()
	{
		if (!oztju)
		{
			oztju = true;
			pxaob pxaob = ssgvt;
			ssgvt = null;
			if (pxaob != null && 0 == 0)
			{
				pxaob.bmpty();
				bsobw = null;
			}
			if (bsobw != null && 0 == 0)
			{
				bsobw.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Close();
	}

	private exkzi ooesf()
	{
		return rxpjc.oxwba(Negotiate);
	}

	internal static TlsSocket xxegh(ISocket p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		return new TlsSocket(p0);
	}
}
