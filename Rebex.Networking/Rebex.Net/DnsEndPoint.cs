using System;
using System.Net;
using System.Net.Sockets;
using onrkn;

namespace Rebex.Net;

public class DnsEndPoint : EndPoint, IComparable
{
	private string uawcc;

	private int ppwgw;

	public string Host
	{
		get
		{
			return uawcc;
		}
		private set
		{
			uawcc = value;
		}
	}

	public int Port
	{
		get
		{
			return ppwgw;
		}
		private set
		{
			ppwgw = value;
		}
	}

	public override AddressFamily AddressFamily => AddressFamily.Unspecified;

	private void deurw(int p0)
	{
		if (p0 < 0 || p0 > 65535)
		{
			throw new ArgumentOutOfRangeException("port", "Port is out of range.");
		}
		Port = p0;
	}

	public DnsEndPoint(string host, int port)
	{
		if (host == null || 1 == 0)
		{
			throw new ArgumentNullException("host");
		}
		if (port < 0 || port > 65535)
		{
			throw new ArgumentOutOfRangeException("port", "Port is out of range.");
		}
		Host = host;
		Port = port;
	}

	private int rohtq(object p0)
	{
		if (!(p0 is DnsEndPoint dnsEndPoint) || 1 == 0)
		{
			return -1;
		}
		int num = ((IComparable)Host).CompareTo((object)dnsEndPoint.Host);
		if (num != 0 && 0 == 0)
		{
			return num;
		}
		return Port.CompareTo(dnsEndPoint.Port);
	}

	int IComparable.CompareTo(object p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rohtq
		return this.rohtq(p0);
	}

	public override int GetHashCode()
	{
		return Host.GetHashCode() ^ Port;
	}

	public override bool Equals(object obj)
	{
		return ((IComparable)this).CompareTo(obj) == 0;
	}

	public override string ToString()
	{
		return brgjd.edcru("{0}:{1}", Host, Port);
	}
}
