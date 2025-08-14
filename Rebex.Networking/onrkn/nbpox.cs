using System;
using System.Net;
using System.Net.Sockets;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class nbpox : gipfx
{
	private const int zyqgo = 1;

	private const int kjvfo = 2;

	private readonly bool tbsla;

	public nbpox(bool socks4a)
	{
		tbsla = socks4a;
	}

	public override EndPoint kkmbs(EndPoint p0)
	{
		euscf(p0, 1);
		return p0;
	}

	public override EndPoint zebbd(ProxySocket p0)
	{
		EndPoint remoteEndPoint = p0.RemoteEndPoint;
		return euscf(remoteEndPoint, 2);
	}

	public override EndPoint ofjnw()
	{
		hvcsu(LogLevel.Debug, "Proxy", "Accepting connection through a proxy.");
		return ampsi();
	}

	private EndPoint euscf(EndPoint p0, byte p1)
	{
		IPAddress p3;
		string p4;
		int p5;
		if (p0 is Rebex.Net.DnsEndPoint p2 && 0 == 0 && (!tbsla || 1 == 0))
		{
			IPEndPoint iPEndPoint = kzurk(p2, ProxySocketExceptionStatus.NameResolutionFailure);
			p3 = iPEndPoint.Address;
			p4 = p3.ToString();
			p5 = iPEndPoint.Port;
		}
		else
		{
			gipfx.owvxn(p0, out p3, out p4, out p5);
		}
		if (p3 != null && 0 == 0 && p3.AddressFamily != AddressFamily.InterNetwork)
		{
			throw new NotSupportedException("Socks4/Socks5 does not support IPv6 protocol.");
		}
		IPEndPoint p6 = bgsra(edbsx, fdtan, ProxySocketExceptionStatus.ProxyNameResolutionFailure);
		npqbr(p6);
		ajwoc(p1, p3, p4, p5);
		return ampsi();
	}

	private void ajwoc(byte p0, IPAddress p1, string p2, int p3)
	{
		switch (p0)
		{
		case 1:
			hvcsu(LogLevel.Debug, "Proxy", "Connecting to {0}:{1} through a proxy.", p2, p3);
			break;
		case 2:
			hvcsu(LogLevel.Debug, "Proxy", "Listening for connection from {0}:{1} through a proxy.", p2, p3);
			break;
		}
		int num = 9 + meang.Length;
		if (p1 == null || 1 == 0)
		{
			num += p2.Length + 1;
		}
		byte[] array = new byte[num];
		array[0] = 4;
		array[1] = p0;
		array[2] = (byte)(p3 >> 8);
		array[3] = (byte)(p3 & 0xFF);
		long num2 = ((p1 == null) ? 16777216 : gipfx.fdsor(p1));
		array[4] = (byte)(num2 & 0xFF);
		array[5] = (byte)((num2 >> 8) & 0xFF);
		array[6] = (byte)((num2 >> 16) & 0xFF);
		array[7] = (byte)((num2 >> 24) & 0xFF);
		alsmq.GetBytes(meang, 0, meang.Length, array, 8);
		if (p1 == null || 1 == 0)
		{
			alsmq.GetBytes(p2, 0, p2.Length, array, meang.Length + 9);
		}
		msrrm(array);
	}

	private EndPoint ampsi()
	{
		byte[] array = new byte[8];
		int i;
		int num;
		for (i = base.pnwnu.Receive(array, 0, 8, SocketFlags.None); i < 8; i += num)
		{
			num = base.pnwnu.Receive(array, i, 8 - i, SocketFlags.None);
			if (num == 0)
			{
				break;
			}
		}
		if (i > 0)
		{
			fpizj(LogLevel.Verbose, "Proxy", "Received data:", array, 0, i);
		}
		if (i < 8)
		{
			throw new ProxySocketException("Socket has been closed.", ProxySocketExceptionStatus.ConnectionClosed);
		}
		if (array[1] != 90)
		{
			throw new ProxySocketException(brgjd.edcru("Socks4 request failed with code {0}.", array[1]), ProxySocketExceptionStatus.ProtocolError, array[1]);
		}
		hvcsu(LogLevel.Debug, "Proxy", "Connection established successfully.");
		return cgrbo(array, 2);
	}

	private EndPoint cgrbo(byte[] p0, int p1)
	{
		int port = (p0[p1] << 8) + p0[p1 + 1];
		long address = (uint)(p0[p1 + 5] << 24) + (p0[p1 + 4] << 16) + (p0[p1 + 3] << 8) + p0[p1 + 2];
		return new IPEndPoint(address, port);
	}
}
