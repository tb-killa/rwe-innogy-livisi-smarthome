using System;
using System.Net;
using System.Net.Sockets;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class ejpvr : gipfx
{
	private const int tfnhr = 1;

	private const int vwyid = 2;

	public override EndPoint kkmbs(EndPoint p0)
	{
		ydxrf(p0, 1);
		return p0;
	}

	public override EndPoint zebbd(ProxySocket p0)
	{
		EndPoint remoteEndPoint = p0.RemoteEndPoint;
		return ydxrf(remoteEndPoint, 2);
	}

	public override EndPoint ofjnw()
	{
		hvcsu(LogLevel.Debug, "Proxy", "Accepting connection through a proxy.");
		return lbubu();
	}

	private EndPoint ydxrf(EndPoint p0, byte p1)
	{
		gipfx.owvxn(p0, out var p2, out var p3, out var p4);
		if (p2 != null && 0 == 0 && p2.AddressFamily != AddressFamily.InterNetwork)
		{
			throw new NotSupportedException("Socks4/Socks5 does not support IPv6 protocol.");
		}
		IPEndPoint p5 = bgsra(edbsx, fdtan, ProxySocketExceptionStatus.ProxyNameResolutionFailure);
		npqbr(p5);
		qlynw();
		gaubt(p1, p2, p3, p4);
		return lbubu();
	}

	private void gsoca(byte[] p0, int p1, int p2)
	{
		while (p2 > 0)
		{
			int num = base.pnwnu.Receive(p0, p1, p2, SocketFlags.None);
			if (num == 0)
			{
				break;
			}
			p1 += num;
			p2 -= num;
		}
		if (p1 > 0)
		{
			fpizj(LogLevel.Verbose, "Proxy", "Received data:", p0, 0, p1);
		}
		if (p2 <= 0)
		{
			return;
		}
		throw new ProxySocketException("Socket has been closed.", ProxySocketExceptionStatus.ConnectionClosed);
	}

	private void qlynw()
	{
		byte[] p = new byte[4] { 5, 2, 0, 2 };
		msrrm(p);
		byte[] array = new byte[2];
		gsoca(array, 0, 2);
		switch (array[0])
		{
		case 0:
			throw new ProxySocketException("Socks5 not supported by the Socks server.", ProxySocketExceptionStatus.ProtocolError);
		default:
			throw new ProxySocketException("Invalid Socks5 response.", ProxySocketExceptionStatus.ServerProtocolViolation);
		case 5:
			switch (array[1])
			{
			case 2:
			{
				byte[] array2 = new byte[3 + meang.Length + gobbp.Length];
				alsmq.GetBytes(meang, 0, meang.Length, array2, 2);
				alsmq.GetBytes(gobbp, 0, gobbp.Length, array2, 3 + meang.Length);
				array2[0] = 1;
				array2[1] = (byte)meang.Length;
				array2[2 + meang.Length] = (byte)gobbp.Length;
				msrrm(array2);
				byte[] array3 = new byte[2];
				gsoca(array3, 0, 2);
				if (array3[0] != 1)
				{
					throw new ProxySocketException("Invalid Socks5 response.", ProxySocketExceptionStatus.ServerProtocolViolation);
				}
				if (array3[1] != 0 && 0 == 0)
				{
					throw new ProxySocketException("Authentication failed.", ProxySocketExceptionStatus.ProtocolError);
				}
				break;
			}
			case byte.MaxValue:
				throw new ProxySocketException("No acceptable authentication method found.", ProxySocketExceptionStatus.ProtocolError);
			default:
				throw new ProxySocketException("Invalid Socks5 response.", ProxySocketExceptionStatus.ServerProtocolViolation);
			case 0:
				break;
			}
			break;
		}
	}

	private void gaubt(byte p0, IPAddress p1, string p2, int p3)
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
		byte[] array;
		if (p1 != null && 0 == 0)
		{
			long num = gipfx.fdsor(p1);
			array = new byte[10]
			{
				0,
				0,
				0,
				1,
				(byte)(num & 0xFF),
				(byte)((num >> 8) & 0xFF),
				(byte)((num >> 16) & 0xFF),
				(byte)((num >> 24) & 0xFF),
				0,
				0
			};
		}
		else
		{
			array = new byte[7 + p2.Length];
			array[3] = 3;
			array[4] = (byte)p2.Length;
			alsmq.GetBytes(p2, 0, p2.Length, array, 5);
		}
		array[0] = 5;
		array[1] = p0;
		array[2] = 0;
		array[array.Length - 2] = (byte)(p3 >> 8);
		array[array.Length - 1] = (byte)(p3 & 0xFF);
		msrrm(array);
	}

	private EndPoint lbubu()
	{
		byte[] array = new byte[4];
		int num = base.pnwnu.Receive(array, 0, 4, SocketFlags.None);
		if (num < 4)
		{
			gsoca(array, num, 4 - num);
		}
		if (array[0] != 5)
		{
			throw new ProxySocketException("Invalid Socks5 response.", ProxySocketExceptionStatus.ServerProtocolViolation);
		}
		int num2 = array[1];
		switch (num2)
		{
		case 1:
			throw new ProxySocketException("General server failure.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 2:
			throw new ProxySocketException("Connection not allowed by ruleset.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 3:
			throw new ProxySocketException("Network unreachable.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 4:
			throw new ProxySocketException("Host unreachable.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 5:
			throw new ProxySocketException("Connection refused.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 6:
			throw new ProxySocketException("TTL expired.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 7:
			throw new ProxySocketException("Command not supported.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 8:
			throw new ProxySocketException("Address type not supported.", ProxySocketExceptionStatus.ProtocolError, num2);
		default:
			throw new ProxySocketException("Unknown error.", ProxySocketExceptionStatus.ProtocolError, num2);
		case 0:
		{
			EndPoint result = array[3] switch
			{
				1 => wzfjq(), 
				3 => emddr(), 
				4 => throw new ProxySocketException("Server returned IPv6 address, which is not supported yet.", ProxySocketExceptionStatus.ProtocolError), 
				_ => throw new ProxySocketException("Unknown address type.", ProxySocketExceptionStatus.ProtocolError), 
			};
			hvcsu(LogLevel.Debug, "Proxy", "Connection established successfully.");
			return result;
		}
		}
	}

	private IPEndPoint wzfjq()
	{
		byte[] array = new byte[6];
		gsoca(array, 0, 6);
		int port = (array[4] << 8) + array[5];
		long address = (uint)(array[3] << 24) + (array[2] << 16) + (array[1] << 8) + array[0];
		return new IPEndPoint(address, port);
	}

	private Rebex.Net.DnsEndPoint emddr()
	{
		byte[] array = new byte[257];
		gsoca(array, 0, 1);
		int num = array[0];
		gsoca(array, 0, num + 2);
		string host = EncodingTools.ASCII.GetString(array, 0, num);
		int port = (array[num] << 8) + array[num + 1];
		return new Rebex.Net.DnsEndPoint(host, port);
	}
}
