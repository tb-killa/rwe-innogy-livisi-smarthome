using System;
using System.Net;
using System.Net.Sockets;
using Rebex.Net;

namespace onrkn;

internal class bavnf : ISocket, IDisposable, vrloh
{
	private Socket tankx;

	private int xpksl = 60000;

	private int? uxvsm;

	private int? quric;

	private bool pmwgt;

	private volatile bool joued;

	public Socket ewsij => tankx;

	public bool Connected
	{
		get
		{
			if (tankx == null || 1 == 0)
			{
				return false;
			}
			return tankx.Connected;
		}
	}

	public EndPoint LocalEndPoint
	{
		get
		{
			lelow();
			return tankx.LocalEndPoint;
		}
	}

	public EndPoint RemoteEndPoint
	{
		get
		{
			lelow();
			return tankx.RemoteEndPoint;
		}
	}

	public int Timeout
	{
		get
		{
			return xpksl;
		}
		set
		{
			if (value <= 0)
			{
				value = -1;
			}
			else if (value < 1000)
			{
				value = 1000;
			}
			xpksl = value;
		}
	}

	public int whtys
	{
		get
		{
			return uxvsm.GetValueOrDefault();
		}
		set
		{
			if (value <= 0)
			{
				uxvsm = null;
			}
			else
			{
				uxvsm = Math.Max(1024, value);
			}
		}
	}

	public int zvlaa
	{
		get
		{
			return quric.GetValueOrDefault();
		}
		set
		{
			if (value <= 0)
			{
				quric = null;
			}
			else
			{
				quric = Math.Max(1024, value);
			}
		}
	}

	public bool utooc
	{
		get
		{
			return pmwgt;
		}
		set
		{
			pmwgt = value;
		}
	}

	internal int olmfw
	{
		get
		{
			lelow();
			return tankx.Available;
		}
	}

	public IntPtr dulwa
	{
		get
		{
			lelow();
			return tankx.Handle;
		}
	}

	private apajk<ISocket> azfan => (apajk<ISocket>)(ISocket)this;

	private apajk<Socket> jdwyv => ewsij;

	public bavnf()
	{
		pmwgt = true;
	}

	public bavnf(Socket socket)
	{
		if (socket == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		tankx = socket;
	}

	private void lelow()
	{
		if (tankx == null || 1 == 0)
		{
			throw new InvalidOperationException("Not connected.");
		}
	}

	public void Close()
	{
		if (tankx != null && 0 == 0)
		{
			joued = true;
			tankx.Close();
		}
	}

	public void hdyyf(AddressFamily p0)
	{
		if (tankx == null || 1 == 0)
		{
			tankx = new Socket(p0, SocketType.Stream, ProtocolType.Tcp);
		}
		else if (tankx.Connected && 0 == 0)
		{
			throw new InvalidOperationException("Socket is already connected.");
		}
	}

	public void Connect(EndPoint remoteEP)
	{
		if (remoteEP == null || 1 == 0)
		{
			throw new ArgumentNullException("remoteEP");
		}
		if (!(remoteEP is IPEndPoint iPEndPoint) || 1 == 0)
		{
			throw new InvalidOperationException("Only IP endpoints are supported.");
		}
		hdyyf(iPEndPoint.AddressFamily);
		try
		{
			tankx.Connect(remoteEP);
			if (joued && 0 == 0)
			{
				throw new SocketException(995);
			}
		}
		catch (ObjectDisposedException)
		{
			if (joued && 0 == 0)
			{
				throw new SocketException(995);
			}
			throw;
		}
		catch (SocketException)
		{
			if (joued && 0 == 0)
			{
				throw new SocketException(995);
			}
			throw;
		}
	}

	public void Connect(string serverName, int serverPort)
	{
		IPEndPoint iPEndPoint = auilw.bolwk(serverName, serverPort);
		if (iPEndPoint == null || 1 == 0)
		{
			IPHostEntry p = auilw.qennv(serverName);
			iPEndPoint = auilw.tulbp(p, serverPort);
		}
		Connect(iPEndPoint);
	}

	public bool Poll(int microSeconds, SocketSelectMode mode)
	{
		lelow();
		return tankx.Poll(microSeconds, (SelectMode)mode);
	}

	public int Receive(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		lelow();
		return tankx.Receive(buffer, offset, count, socketFlags);
	}

	public int Send(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		lelow();
		return tankx.Send(buffer, offset, count, socketFlags);
	}

	public void Shutdown(SocketShutdown how)
	{
		lelow();
		tankx.Shutdown(how);
	}

	internal IAsyncResult byilq(byte[] p0, int p1, int p2, SocketFlags p3, AsyncCallback p4, object p5)
	{
		lelow();
		return tankx.BeginSend(p0, p1, p2, p3, p4, p5);
	}

	internal int fmrlr(IAsyncResult p0)
	{
		lelow();
		return tankx.EndSend(p0);
	}

	internal IAsyncResult squpq(byte[] p0, int p1, int p2, SocketFlags p3, AsyncCallback p4, object p5)
	{
		lelow();
		return tankx.BeginReceive(p0, p1, p2, p3, p4, p5);
	}

	internal int lrjsv(IAsyncResult p0)
	{
		lelow();
		return tankx.EndReceive(p0);
	}

	public void Dispose()
	{
		Close();
	}
}
