using System;
using System.Net;
using System.Net.Sockets;
using Rebex;

namespace onrkn;

internal class npgzc : IDisposable
{
	private readonly EndPoint hmsdi;

	private readonly object eebgw = new object();

	private Socket nuiyv;

	private readonly yddoj jlvvq;

	private volatile bool qfekv;

	private int? iyibo;

	private int? jljbi;

	private bool? bdvlk;

	private int xvldy;

	public EndPoint sccha => hmsdi;

	public yddoj ajyct => jlvvq;

	public int? miqrz
	{
		get
		{
			return iyibo;
		}
		set
		{
			iyibo = value;
		}
	}

	public int? lbdar
	{
		get
		{
			return jljbi;
		}
		set
		{
			jljbi = value;
		}
	}

	public bool? ehjhd
	{
		get
		{
			return bdvlk;
		}
		set
		{
			bdvlk = value;
		}
	}

	public int ahknl
	{
		get
		{
			return xvldy;
		}
		set
		{
			xvldy = value;
		}
	}

	public bool rzuye => qfekv;

	public npgzc(EndPoint ep, string hostname, yddoj runner)
	{
		hmsdi = ep;
		jlvvq = runner;
		ahknl = 16;
		if (ep is IPEndPoint && 0 == 0)
		{
			nuiyv = ajqxc(ep, p1: true);
			return;
		}
		throw new ArgumentException("Unknown endpoint type.", "ep");
	}

	private static Socket ajqxc(EndPoint p0, bool p1)
	{
		Socket socket = new Socket(p0.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			socket.Bind(p0);
			return socket;
		}
		catch (SocketException ex)
		{
			if (!p1 || 1 == 0)
			{
				return null;
			}
			if (ex.skehp() == 10048)
			{
				throw new InvalidOperationException("The specified socket is already in use.", ex);
			}
			throw;
		}
	}

	public void itzax()
	{
		lock (eebgw)
		{
			if (qfekv && 0 == 0)
			{
				throw new InvalidOperationException("Already listening.");
			}
			Socket socket = nuiyv;
			if (socket == null || 1 == 0)
			{
				socket = ajqxc(hmsdi, p1: true);
			}
			nuiyv = socket;
			nuiyv.Listen(ahknl);
			nuiyv.BeginAccept(ytbai, nuiyv);
			qfekv = true;
		}
		lock (eebgw)
		{
			IPEndPoint iPEndPoint = hmsdi as IPEndPoint;
			IPEndPoint iPEndPoint2 = nuiyv.LocalEndPoint as IPEndPoint;
			if (iPEndPoint != null && 0 == 0 && iPEndPoint2 != null && 0 == 0 && (iPEndPoint.Port == 0 || 1 == 0))
			{
				iPEndPoint.Port = iPEndPoint2.Port;
			}
		}
	}

	public void cznyx()
	{
		lock (eebgw)
		{
			if (!qfekv || 1 == 0)
			{
				throw new InvalidOperationException("Not listening.");
			}
			qfekv = false;
			nuiyv.Close();
			nuiyv = ajqxc(hmsdi, p1: false);
		}
	}

	public void Dispose()
	{
		lock (eebgw)
		{
			qfekv = false;
			Socket socket = nuiyv;
			if (socket != null && 0 == 0)
			{
				socket.Close();
			}
		}
	}

	private void ytbai(IAsyncResult p0)
	{
		Socket socket = null;
		try
		{
			Socket socket2 = (Socket)p0.AsyncState;
			try
			{
				socket = socket2.EndAccept(p0);
			}
			catch (Exception ex)
			{
				if (!qfekv || 1 == 0)
				{
					return;
				}
				if (ex is ObjectDisposedException || ex is SocketException)
				{
					jlvvq.ewpna(LogLevel.Debug, "Incoming connection request aborted.", null);
				}
				else
				{
					jlvvq.ewpna(LogLevel.Info, "Error while accepting connection.", ex);
				}
			}
			socket2.BeginAccept(ytbai, socket2);
		}
		catch (ObjectDisposedException p1)
		{
			if (qfekv && 0 == 0)
			{
				jlvvq.ewpna(LogLevel.Error, "Listener was unexpectedly disposed.", p1);
			}
		}
		catch (Exception p2)
		{
			jlvvq.ewpna(LogLevel.Error, "Fatal error while accepting connections.", p2);
		}
		if (socket != null && 0 == 0)
		{
			try
			{
				jlvvq.rslch(socket);
			}
			catch (Exception p3)
			{
				jlvvq.ewpna(LogLevel.Error, "Error while starting session.", p3);
			}
		}
	}
}
