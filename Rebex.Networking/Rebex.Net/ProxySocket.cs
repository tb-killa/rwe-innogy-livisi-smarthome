using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using onrkn;

namespace Rebex.Net;

public class ProxySocket : phvuu, ISocketExt, ISocket, IDisposable, vrloh
{
	private enum qaulf
	{
		rmzeo,
		hrney,
		zwugw,
		ihpxa,
		zsxnz,
		qcbbv
	}

	private sealed class upewj
	{
		public bool yrfsf;

		public bool lzbqo;

		public gipfx qegfx;

		public ProxySocket druis;

		public void mivgz()
		{
			lock (druis.yzrif)
			{
				if (!yrfsf || 1 == 0)
				{
					lzbqo = true;
					qegfx.eesdi();
				}
			}
		}
	}

	private sealed class aymeq
	{
		public bool qbkmo;

		public gipfx ergqu;

		public ProxySocket aqzgc;

		public void ktxmb()
		{
			lock (aqzgc.yzrif)
			{
				if (!qbkmo || 1 == 0)
				{
					ergqu.eesdi();
				}
			}
		}
	}

	private sealed class emuju
	{
		public bool zkbca;

		public gipfx slxjy;

		public ProxySocket iteup;

		public void mbljg()
		{
			lock (iteup.yzrif)
			{
				if (!zkbca || 1 == 0)
				{
					slxjy.eesdi();
				}
			}
		}
	}

	private sealed class crphq
	{
		public ProxySocket vgqun;

		public string iwswb;

		public int skppe;

		public object jqjey()
		{
			vgqun.Connect(iwswb, skppe);
			return null;
		}
	}

	private sealed class earbz
	{
		public ProxySocket miyys;

		public EndPoint gmvsb;

		public object qlkyw()
		{
			miyys.Connect(gmvsb);
			return null;
		}
	}

	private sealed class wuyvz
	{
		public ProxySocket ktoiu;

		public ISocket xkysb;

		public EndPoint touva()
		{
			return ktoiu.Listen(xkysb);
		}
	}

	private readonly object yzrif = new object();

	private qaulf eulad;

	private bavnf gkzoe;

	private int sddrl;

	private gipfx nyazy;

	private Proxy hrdkk;

	private EndPoint czynz;

	public PortRange LocalPortRange => hrdkk.LocalPortRange;

	public string ProxyHost => hrdkk.Host;

	public int ProxyPort => hrdkk.Port;

	public ProxyType ProxyType => hrdkk.ProxyType;

	public ProxyAuthentication AuthenticationMethod => hrdkk.AuthenticationMethod;

	public string UserName => hrdkk.UserName;

	public string Password => hrdkk.Password;

	public string Domain => hrdkk.Domain;

	public int Timeout
	{
		get
		{
			if (sddrl == int.MaxValue)
			{
				return -1;
			}
			return sddrl;
		}
		set
		{
			if (value < -1)
			{
				throw hifyx.nztrs("value", value, "Timeout is out of range of valid values.");
			}
			if (value < 1)
			{
				sddrl = -1;
			}
			else if (value < 500)
			{
				sddrl = 500;
			}
			else
			{
				sddrl = value;
			}
		}
	}

	public Socket Socket
	{
		get
		{
			bavnf bavnf = gkzoe;
			if (bavnf == null || 1 == 0)
			{
				return null;
			}
			return bavnf.ewsij;
		}
	}

	private ISocketFactory omibf => hrdkk;

	public EndPoint RemoteEndPoint
	{
		get
		{
			lock (yzrif)
			{
				bavnf bavnf = iwqry(qaulf.zwugw);
				if (bavnf == null || 1 == 0)
				{
					return null;
				}
				EndPoint remoteEndPoint = czynz;
				if (remoteEndPoint == null || 1 == 0)
				{
					remoteEndPoint = bavnf.RemoteEndPoint;
				}
				return remoteEndPoint;
			}
		}
	}

	public EndPoint LocalEndPoint
	{
		get
		{
			bavnf bavnf = iwqry(qaulf.zwugw);
			if (bavnf == null || 1 == 0)
			{
				return null;
			}
			return bavnf.LocalEndPoint as IPEndPoint;
		}
	}

	public SocketInformation Information
	{
		get
		{
			lock (yzrif)
			{
				bavnf bavnf = iwqry(qaulf.zwugw);
				if (bavnf == null || 1 == 0)
				{
					return null;
				}
				return new SocketInformation(this);
			}
		}
	}

	public IntPtr Handle
	{
		get
		{
			bavnf bavnf = iwqry(qaulf.zwugw);
			if (bavnf == null || 1 == 0)
			{
				return IntPtr.Zero;
			}
			return bavnf.dulwa;
		}
	}

	public int Available
	{
		get
		{
			bavnf bavnf = iwqry(qaulf.zwugw);
			if (bavnf == null || 1 == 0)
			{
				return 0;
			}
			return bavnf.olmfw;
		}
	}

	public bool Connected
	{
		get
		{
			bavnf bavnf = iwqry(qaulf.zwugw);
			if (bavnf == null || 1 == 0)
			{
				return false;
			}
			return bavnf.Connected;
		}
	}

	private apajk<ISocket> dceve => (apajk<ISocket>)(ISocket)this;

	private apajk<Socket> rdavx => Socket;

	private void uqtxk()
	{
		if (eulad == qaulf.qcbbv)
		{
			throw new ObjectDisposedException(GetType().Name, "The socket has been already closed.");
		}
	}

	private bavnf iwqry(qaulf p0)
	{
		lock (yzrif)
		{
			if (eulad != p0)
			{
				return null;
			}
			return gkzoe;
		}
	}

	public bool Poll(int microSeconds, SocketSelectMode mode)
	{
		bavnf bavnf = iwqry(qaulf.zwugw);
		if (bavnf == null || 1 == 0)
		{
			return true;
		}
		try
		{
			return bavnf.Poll(microSeconds, mode);
		}
		catch (SocketException p)
		{
			int num = p.skehp();
			if (num == 10038)
			{
				Close();
				return true;
			}
			throw;
		}
	}

	private bool ufirk(int p0)
	{
		return Poll(p0, SocketSelectMode.SelectRead);
	}

	bool phvuu.hznqz(int p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ufirk
		return this.ufirk(p0);
	}

	public SocketState GetConnectionState()
	{
		bavnf bavnf = iwqry(qaulf.zwugw);
		if (bavnf == null || 1 == 0)
		{
			return SocketState.NotConnected;
		}
		if (bavnf.olmfw > 0)
		{
			return SocketState.Connected;
		}
		if (!bavnf.Connected || 1 == 0)
		{
			return SocketState.NotConnected;
		}
		if (!bavnf.Poll(100, SocketSelectMode.SelectRead) || 1 == 0)
		{
			return SocketState.Connected;
		}
		if (bavnf.olmfw > 0)
		{
			return SocketState.Connected;
		}
		return SocketState.NotConnected;
	}

	public ProxySocket()
		: this(new Proxy())
	{
	}

	public ProxySocket(Socket socket)
		: this(new Proxy())
	{
		if (socket == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		if (socket.SocketType != SocketType.Stream)
		{
			throw new ArgumentException("Only stream sockets are supported.", "socket");
		}
		if (!socket.Blocking || 1 == 0)
		{
			throw new ArgumentException("Non-blocking sockets are not supported.", "socket");
		}
		if (socket.ProtocolType != ProtocolType.Tcp)
		{
			throw new ArgumentException("Only TCP sockets are supported.", "socket");
		}
		if (!socket.Connected || 1 == 0)
		{
			throw new InvalidOperationException("Only connected sockets are allowed.");
		}
		eulad = qaulf.zwugw;
		gkzoe = new bavnf(socket);
	}

	public ProxySocket(Proxy proxy)
	{
		if (proxy == null || 1 == 0)
		{
			throw new ArgumentNullException("proxy");
		}
		hrdkk = new Proxy();
		hrdkk.ProxyType = proxy.ProxyType;
		hrdkk.AuthenticationMethod = proxy.AuthenticationMethod;
		hrdkk.HttpUserAgent = proxy.HttpUserAgent;
		hrdkk.Host = proxy.Host;
		hrdkk.Port = proxy.Port;
		hrdkk.UserName = proxy.UserName;
		hrdkk.Password = proxy.Password;
		hrdkk.Domain = proxy.Domain;
		hrdkk.Encoding = proxy.Encoding;
		hrdkk.SendRetryTimeout = proxy.SendRetryTimeout;
		hrdkk.LocalPortRange = proxy.LocalPortRange;
		hrdkk.LogWriter = proxy.LogWriter;
	}

	private ProxySocket(Proxy proxy, bavnf socket, EndPoint remoteEndPoint)
	{
		hrdkk = new Proxy();
		hrdkk.ProxyType = proxy.ProxyType;
		hrdkk.AuthenticationMethod = proxy.AuthenticationMethod;
		hrdkk.HttpUserAgent = proxy.HttpUserAgent;
		hrdkk.Host = proxy.Host;
		hrdkk.Port = proxy.Port;
		hrdkk.UserName = proxy.UserName;
		hrdkk.Password = proxy.Password;
		hrdkk.Domain = proxy.Domain;
		hrdkk.Encoding = proxy.Encoding;
		hrdkk.SendRetryTimeout = proxy.SendRetryTimeout;
		hrdkk.LogWriter = proxy.LogWriter;
		eulad = qaulf.zwugw;
		gkzoe = socket;
		czynz = remoteEndPoint;
	}

	private bavnf ktqym()
	{
		bavnf bavnf = iwqry(qaulf.zwugw);
		if (bavnf == null || 1 == 0)
		{
			uqtxk();
			throw new InvalidOperationException("Socket is not connected.");
		}
		return bavnf;
	}

	public int Send(byte[] buffer, int offset, int count)
	{
		dahxy.dionp(buffer, offset, count);
		return Send(buffer, offset, count, SocketFlags.None);
	}

	public int Send(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		dahxy.dionp(buffer, offset, count);
		bavnf bavnf = ktqym();
		int num = count;
		int num2 = bavnf.Send(buffer, offset, num, socketFlags);
		if (num2 == num)
		{
			return count;
		}
		int sendRetryTimeout = hrdkk.SendRetryTimeout;
		if (sendRetryTimeout <= 0)
		{
			sendRetryTimeout = sddrl;
		}
		offset += num2;
		num -= num2;
		int tickCount = Environment.TickCount;
		while (count > 0)
		{
			num2 = bavnf.Send(buffer, offset, num, socketFlags);
			offset += num2;
			num -= num2;
			if (num == 0 || 1 == 0)
			{
				return count;
			}
			if (sendRetryTimeout > 0)
			{
				int num3 = Environment.TickCount - tickCount;
				if (num3 >= sendRetryTimeout)
				{
					break;
				}
			}
		}
		throw new ProxySocketException("An incomplete block of data was sent.", ProxySocketExceptionStatus.SendRetryTimeout);
	}

	public int Receive(byte[] buffer, int offset, int count)
	{
		dahxy.dionp(buffer, offset, count);
		bavnf bavnf = ktqym();
		return bavnf.Receive(buffer, offset, count, SocketFlags.None);
	}

	public int Receive(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		dahxy.dionp(buffer, offset, count);
		bavnf bavnf = ktqym();
		return bavnf.Receive(buffer, offset, count, socketFlags);
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

	public int Send(byte[] buffer, int count, SocketFlags socketFlags)
	{
		return Send(buffer, 0, count, SocketFlags.None);
	}

	public IAsyncResult BeginSend(byte[] buffer, int offset, int count, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		dahxy.dionp(buffer, offset, count);
		bavnf bavnf = ktqym();
		return bavnf.byilq(buffer, offset, count, socketFlags, callback, state);
	}

	public int EndSend(IAsyncResult asyncResult)
	{
		bavnf bavnf = ktqym();
		return bavnf.fmrlr(asyncResult);
	}

	public int Receive(byte[] buffer)
	{
		dahxy.gbxkl(buffer, "buffer");
		bavnf bavnf = ktqym();
		return bavnf.Receive(buffer, 0, buffer.Length, SocketFlags.None);
	}

	public int Receive(byte[] buffer, SocketFlags socketFlags)
	{
		dahxy.gbxkl(buffer, "buffer");
		bavnf bavnf = ktqym();
		return bavnf.Receive(buffer, 0, buffer.Length, socketFlags);
	}

	public int Receive(byte[] buffer, int count, SocketFlags socketFlags)
	{
		dahxy.dionp(buffer, 0, count);
		bavnf bavnf = ktqym();
		return bavnf.Receive(buffer, 0, count, socketFlags);
	}

	public IAsyncResult BeginReceive(byte[] buffer, int offset, int count, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		dahxy.dionp(buffer, offset, count);
		bavnf bavnf = ktqym();
		return bavnf.squpq(buffer, offset, count, socketFlags, callback, state);
	}

	public int EndReceive(IAsyncResult asyncResult)
	{
		bavnf bavnf = ktqym();
		return bavnf.lrjsv(asyncResult);
	}

	public static bool IsValidHost(string host)
	{
		return auilw.pogqj(host);
	}

	public static IPEndPoint ToEndPoint(string host, int port)
	{
		return auilw.bolwk(host, port);
	}

	public static IPEndPoint ToEndPoint(IPHostEntry hostEntry, int port)
	{
		return auilw.tulbp(hostEntry, port);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("GetAddressLong method has been deprecated and will be removed. Use IPAddress.GetAddressBytes method instead.", true)]
	public static long GetAddressLong(IPAddress address)
	{
		throw new NotSupportedException("ProxySocket.GetAddressLong method has been removed. Use IPAddress.GetAddressBytes instead.");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("GetAddressBytes method has been deprecated and will be removed. Use IPAddress.GetAddressBytes method instead.", true)]
	public static byte[] GetAddressBytes(IPAddress address)
	{
		throw new NotSupportedException("ProxySocket.GetAddressBytes method has been removed. Use IPAddress.GetAddressBytes instead.");
	}

	public void Shutdown(SocketShutdown how)
	{
		bavnf bavnf = ktqym();
		bavnf.Shutdown(how);
	}

	public void Connect(string serverName, int serverPort)
	{
		if (serverName == null || 1 == 0)
		{
			throw new ArgumentNullException("serverName");
		}
		if (serverPort < 1 || serverPort > 65535)
		{
			throw new ArgumentOutOfRangeException("serverPort");
		}
		DnsEndPoint remoteEP = new DnsEndPoint(serverName, serverPort);
		Connect(remoteEP);
	}

	public void Connect(EndPoint remoteEP)
	{
		Action action = null;
		upewj upewj = new upewj();
		upewj.druis = this;
		if (remoteEP == null || 1 == 0)
		{
			throw new ArgumentNullException("remoteEP");
		}
		uqtxk();
		EndPoint endPoint = remoteEP as IPEndPoint;
		if (endPoint == null || 1 == 0)
		{
			if (!(remoteEP is DnsEndPoint dnsEndPoint) || 1 == 0)
			{
				throw new ProxySocketException("Unsupported endpoint.", ProxySocketExceptionStatus.UnclassifiableError);
			}
			endPoint = auilw.bolwk(dnsEndPoint.Host, dnsEndPoint.Port);
			if (endPoint == null || 1 == 0)
			{
				endPoint = dnsEndPoint;
			}
		}
		upewj.yrfsf = false;
		upewj.lzbqo = false;
		IDisposable disposable = null;
		lock (yzrif)
		{
			uqtxk();
			if (eulad != qaulf.rmzeo && 0 == 0)
			{
				throw new InvalidOperationException("The socket is already connected or listening.");
			}
			upewj.qegfx = gipfx.rldhz(hrdkk, sddrl, endPoint);
			if (sddrl > 0)
			{
				if (action == null || 1 == 0)
				{
					action = upewj.mivgz;
				}
				disposable = dahxy.nqapv(action, null, sddrl);
			}
			eulad = qaulf.hrney;
			nyazy = upewj.qegfx;
		}
		bool flag = false;
		EndPoint endPoint2 = null;
		try
		{
			endPoint2 = upewj.qegfx.kkmbs(endPoint);
			flag = true;
		}
		catch (SocketException e)
		{
			lock (yzrif)
			{
				if (upewj.lzbqo && 0 == 0)
				{
					throw new ProxySocketException("Connection attempt timed out.", ProxySocketExceptionStatus.Timeout);
				}
			}
			throw new ProxySocketException(e);
		}
		finally
		{
			lock (yzrif)
			{
				upewj.yrfsf = true;
				nyazy = null;
				if (flag && 0 == 0)
				{
					gkzoe = upewj.qegfx.ljwfg();
					czynz = endPoint2;
					eulad = qaulf.zwugw;
				}
				else
				{
					eulad = qaulf.qcbbv;
					upewj.qegfx.eesdi();
				}
			}
			if (disposable != null && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}

	public EndPoint Listen(ISocket controlSocket)
	{
		Action action = null;
		aymeq aymeq = new aymeq();
		aymeq.aqzgc = this;
		if (controlSocket == null || 1 == 0)
		{
			throw new ArgumentNullException("controlSocket");
		}
		uqtxk();
		if (!(controlSocket is ProxySocket p) || 1 == 0)
		{
			throw new InvalidOperationException("A socket of a same socket type and settings must be used as a control socket.");
		}
		aymeq.qbkmo = false;
		lock (yzrif)
		{
			uqtxk();
			if (eulad != qaulf.rmzeo && 0 == 0)
			{
				throw new InvalidOperationException("The socket is already connected or listening.");
			}
			aymeq.ergqu = gipfx.xvlrv(hrdkk, sddrl);
			if (sddrl > 0)
			{
				if (action == null || 1 == 0)
				{
					action = aymeq.ktxmb;
				}
				dahxy.nqapv(action, null, sddrl);
			}
			eulad = qaulf.ihpxa;
			nyazy = aymeq.ergqu;
		}
		bool flag = false;
		try
		{
			EndPoint result = aymeq.ergqu.zebbd(p);
			flag = true;
			return result;
		}
		catch (SocketException e)
		{
			throw new ProxySocketException(e);
		}
		finally
		{
			lock (yzrif)
			{
				aymeq.qbkmo = true;
				if (!flag || 1 == 0)
				{
					eulad = qaulf.qcbbv;
					nyazy = null;
					aymeq.ergqu.eesdi();
				}
			}
		}
	}

	public ISocket Accept()
	{
		Action action = null;
		emuju emuju = new emuju();
		emuju.iteup = this;
		uqtxk();
		emuju.zkbca = false;
		lock (yzrif)
		{
			emuju.slxjy = nyazy;
			if (eulad != qaulf.ihpxa || emuju.slxjy == null)
			{
				throw new InvalidOperationException("The socket is not listening.");
			}
			if (sddrl > 0)
			{
				if (action == null || 1 == 0)
				{
					action = emuju.mbljg;
				}
				dahxy.nqapv(action, null, sddrl);
			}
			eulad = qaulf.zsxnz;
		}
		try
		{
			EndPoint remoteEndPoint = emuju.slxjy.ofjnw();
			bavnf socket = emuju.slxjy.ljwfg();
			return new ProxySocket(hrdkk, socket, remoteEndPoint);
		}
		catch (SocketException e)
		{
			throw new ProxySocketException(e);
		}
		finally
		{
			lock (yzrif)
			{
				emuju.zkbca = true;
				eulad = qaulf.qcbbv;
				nyazy = null;
				emuju.slxjy.eesdi();
			}
		}
	}

	public void Close()
	{
		bavnf bavnf;
		gipfx gipfx;
		lock (yzrif)
		{
			bavnf = gkzoe;
			gipfx = nyazy;
			eulad = qaulf.qcbbv;
			gkzoe = null;
			nyazy = null;
		}
		if (bavnf != null && 0 == 0)
		{
			bavnf.Close();
		}
		if (gipfx != null && 0 == 0)
		{
			gipfx.eesdi();
		}
	}

	public IAsyncResult BeginConnect(string serverName, int serverPort, AsyncCallback callback, object state)
	{
		crphq crphq = new crphq();
		crphq.iwswb = serverName;
		crphq.skppe = serverPort;
		crphq.vgqun = this;
		return rxpjc.tzeev(crphq.jqjey, callback, state);
	}

	public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
	{
		earbz earbz = new earbz();
		earbz.gmvsb = remoteEP;
		earbz.miyys = this;
		return rxpjc.tzeev(earbz.qlkyw, callback, state);
	}

	public void EndConnect(IAsyncResult asyncResult)
	{
		rxpjc.wzgzd<object>(asyncResult);
	}

	public IAsyncResult BeginListen(ISocket controlSocket, AsyncCallback callback, object state)
	{
		wuyvz wuyvz = new wuyvz();
		wuyvz.xkysb = controlSocket;
		wuyvz.ktoiu = this;
		return rxpjc.tzeev(wuyvz.touva, callback, state);
	}

	public EndPoint EndListen(IAsyncResult asyncResult)
	{
		return rxpjc.wzgzd<EndPoint>(asyncResult);
	}

	public IAsyncResult BeginAccept(AsyncCallback callback, object state)
	{
		return rxpjc.tzeev(pfmtm, callback, state);
	}

	public ISocket EndAccept(IAsyncResult asyncResult)
	{
		return rxpjc.wzgzd<ISocket>(asyncResult);
	}

	public void Dispose()
	{
		Close();
	}

	private ISocket pfmtm()
	{
		return Accept();
	}
}
