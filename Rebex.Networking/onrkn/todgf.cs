using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal static class todgf
{
	public static ISocketFactory njpxf(ISocketFactory p0, ILogWriter p1)
	{
		if (p0 == null || 1 == 0)
		{
			Proxy proxy = new Proxy();
			proxy.LogWriter = p1;
			return proxy;
		}
		if (p0 is Proxy proxy2 && 0 == 0)
		{
			Proxy proxy = proxy2.Clone();
			proxy.LogWriter = p1;
			return proxy;
		}
		return p0;
	}

	public static SocketState gnzjs(ISocket p0)
	{
		if (p0 is ISocketExt socketExt && 0 == 0)
		{
			return socketExt.GetConnectionState();
		}
		if (!p0.Connected || 1 == 0)
		{
			return SocketState.NotConnected;
		}
		return SocketState.Connected;
	}

	private static object hwzhl(Type p0, params object[] p1)
	{
		Type[] array = new Type[p1.Length];
		int num = 0;
		if (num != 0)
		{
			goto IL_000f;
		}
		goto IL_002e;
		IL_000f:
		array[num] = ((p1[num] == null) ? null : p1[num].GetType());
		num++;
		goto IL_002e;
		IL_002e:
		if (num >= p1.Length)
		{
			ConstructorInfo constructor = p0.GetConstructor(array);
			if ((object)constructor == null || 1 == 0)
			{
				throw new MissingMethodException();
			}
			return constructor.Invoke(p1);
		}
		goto IL_000f;
	}

	public static IPAddress fhroz(EndPoint p0)
	{
		if (!(p0 is IPEndPoint iPEndPoint) || 1 == 0)
		{
			return null;
		}
		return iPEndPoint.Address;
	}

	private static IList<IPAddress> sgjtt(IPHostEntry p0)
	{
		List<IPAddress> list = new List<IPAddress>();
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_0029;
		IL_000c:
		IPAddress iPAddress = p0.AddressList[num];
		if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
		{
			list.Add(iPAddress);
		}
		num++;
		goto IL_0029;
		IL_0039:
		int num2;
		IPAddress iPAddress2 = p0.AddressList[num2];
		if (iPAddress2.AddressFamily == AddressFamily.InterNetworkV6)
		{
			list.Add(iPAddress2);
		}
		num2++;
		goto IL_005a;
		IL_005a:
		if (num2 < p0.AddressList.Length)
		{
			goto IL_0039;
		}
		return list;
		IL_0029:
		if (num < p0.AddressList.Length)
		{
			goto IL_000c;
		}
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_0039;
		}
		goto IL_005a;
	}

	public static IList<IPAddress> pjnkb(string p0, int p1)
	{
		new List<IPAddress>();
		IPEndPoint iPEndPoint = auilw.bolwk(p0, p1);
		if (iPEndPoint != null && 0 == 0)
		{
			return new IPAddress[1] { iPEndPoint.Address };
		}
		try
		{
			IPHostEntry p2 = auilw.qennv(p0);
			IList<IPAddress> list = sgjtt(p2);
			if (list.Count == 0 || 1 == 0)
			{
				throw new SocketException(11001);
			}
			return list;
		}
		catch (Exception ex)
		{
			throw new ProxySocketException("Unable to resolve host name: " + ex.Message, ProxySocketExceptionStatus.NameResolutionFailure, ex);
		}
	}

	public static string innqj(EndPoint p0)
	{
		if (p0 is IPEndPoint iPEndPoint && 0 == 0)
		{
			if (iPEndPoint.Address == null || 1 == 0)
			{
				return null;
			}
			return iPEndPoint.Address.ToString();
		}
		if (!(p0 is Rebex.Net.DnsEndPoint dnsEndPoint) || 1 == 0)
		{
			return null;
		}
		return dnsEndPoint.Host;
	}

	public static bool hudgy(Exception p0)
	{
		for (Exception ex = p0; ex != null; ex = ex.InnerException)
		{
			if (ex is TimeoutException && 0 == 0)
			{
				return true;
			}
			if (ex is NetworkSessionException ex2 && 0 == 0 && ex2.Status == NetworkSessionExceptionStatus.Timeout)
			{
				return true;
			}
			if (ex is SocketException p1 && 0 == 0 && p1.skehp() == 10060)
			{
				return true;
			}
		}
		return false;
	}
}
