using System;
using System.Net;
using System.Net.Sockets;
using Rebex.Net;

namespace onrkn;

internal class nufwy : gipfx
{
	private Socket jhslq;

	public override EndPoint kkmbs(EndPoint p0)
	{
		IPEndPoint iPEndPoint = p0 as IPEndPoint;
		if (iPEndPoint == null || 1 == 0)
		{
			if (!(p0 is Rebex.Net.DnsEndPoint dnsEndPoint) || 1 == 0)
			{
				throw new ProxySocketException("Unsupported endpoint.", ProxySocketExceptionStatus.UnclassifiableError);
			}
			iPEndPoint = bgsra(dnsEndPoint.Host, dnsEndPoint.Port, ProxySocketExceptionStatus.NameResolutionFailure);
		}
		npqbr(iPEndPoint);
		return base.pnwnu.RemoteEndPoint;
	}

	public override EndPoint zebbd(ProxySocket p0)
	{
		IPEndPoint iPEndPoint = (IPEndPoint)p0.LocalEndPoint;
		lock (zxhzq)
		{
			jhslq = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		}
		lgsdc.lboxg(jhslq, iPEndPoint.Address);
		jhslq.Listen(0);
		return (IPEndPoint)jhslq.LocalEndPoint;
	}

	public override EndPoint ofjnw()
	{
		Socket socket;
		lock (zxhzq)
		{
			socket = jhslq;
			jhslq = null;
		}
		if (socket == null || 1 == 0)
		{
			throw new InvalidOperationException("Operation is not allowed in the current state.");
		}
		Socket socket2 = socket.Accept();
		socket.Close();
		bavnf p = new bavnf(socket2);
		fgokm(p);
		return socket2.RemoteEndPoint;
	}

	public override void eesdi()
	{
		base.eesdi();
		Socket socket;
		lock (zxhzq)
		{
			socket = jhslq;
			jhslq = null;
		}
		if (socket != null && 0 == 0)
		{
			socket.Close();
		}
	}
}
