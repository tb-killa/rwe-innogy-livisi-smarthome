using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using onrkn;

namespace Rebex.Net;

public class PortRange
{
	private int akfos;

	private int wpagn;

	[NonSerialized]
	private int dusko;

	private static int[] ztipb;

	private static int xjpvc;

	private static Random nhqpk;

	private static readonly object norzy = new object();

	public static readonly PortRange Any = new PortRange();

	private int xqpmm()
	{
		if ((akfos == 0 || 1 == 0) && (wpagn == 0 || 1 == 0))
		{
			return 0;
		}
		lock (norzy)
		{
			if (ztipb == null || 1 == 0)
			{
				nhqpk = new Random();
				ztipb = new int[1024];
			}
			if (dusko > wpagn || dusko < akfos)
			{
				dusko = akfos;
			}
			int num = dusko;
			int num2;
			int num3;
			if (akfos != wpagn)
			{
				num2 = wpagn + 1 - akfos;
				num3 = 0;
				if (num3 != 0)
				{
					goto IL_00af;
				}
				goto IL_00e4;
			}
			goto IL_00e9;
			IL_00e9:
			dusko = num + 1;
			ztipb[xjpvc] = num;
			xjpvc = (xjpvc + 1) % ztipb.Length;
			return num;
			IL_00af:
			if (Array.IndexOf(ztipb, num) >= 0 && 0 == 0)
			{
				num = akfos + nhqpk.Next() % num2;
				num3++;
				goto IL_00e4;
			}
			goto IL_00e9;
			IL_00e4:
			if (num3 < 32)
			{
				goto IL_00af;
			}
			goto IL_00e9;
		}
	}

	private int gzmds(int p0)
	{
		if ((akfos == 0 || 1 == 0) && (wpagn == 0 || 1 == 0))
		{
			return 0;
		}
		lock (norzy)
		{
			if (ztipb == null || 1 == 0)
			{
				nhqpk = new Random();
				ztipb = new int[1024];
			}
			p0 = ((p0 == wpagn) ? akfos : (p0 + 1));
			ztipb[xjpvc] = p0;
			xjpvc = (xjpvc + 1) % ztipb.Length;
			return p0;
		}
	}

	internal void lboxg(Socket p0, IPAddress p1)
	{
		int num = wpagn + 1 - akfos;
		if (num == 1 || num > 32)
		{
			num = Math.Min(num, 128);
			while (num-- > 0)
			{
				int p2 = xqpmm();
				if (ambra(p0, p1, p2) && 0 == 0)
				{
					return;
				}
			}
		}
		else
		{
			int num2 = -1;
			int num3 = xqpmm();
			while (num2 != num3)
			{
				num2 = ((num2 >= 0) ? gzmds(num2) : num3);
				if (ambra(p0, p1, num2) && 0 == 0)
				{
					return;
				}
			}
		}
		throw new ProxySocketException("The specified port range doesn't contain any free ports.", ProxySocketExceptionStatus.ConnectFailure);
	}

	private bool ambra(Socket p0, IPAddress p1, int p2)
	{
		IPEndPoint localEP = new IPEndPoint(p1, p2);
		try
		{
			p0.Bind(localEP);
			return true;
		}
		catch (SocketException ex)
		{
			if (ex.skehp() != 10048)
			{
				throw new ProxySocketException(ex);
			}
		}
		return false;
	}

	private PortRange()
	{
		akfos = 0;
		wpagn = 0;
		dusko = 0;
	}

	public PortRange(int port)
		: this(port, port)
	{
	}

	public PortRange(int portMin, int portMax)
	{
		if (portMin < 1 || portMin > 65535)
		{
			throw hifyx.nztrs("portMin", portMin, "Port is out of range of valid values.");
		}
		if (portMax < 1 || portMax > 65535)
		{
			throw hifyx.nztrs("portMax", portMax, "Port is out of range of valid values.");
		}
		if (portMin > portMax)
		{
			throw new ArgumentException("Invalid port bounds.", "portMin");
		}
		akfos = portMin;
		wpagn = portMax;
		dusko = portMin;
	}

	public override string ToString()
	{
		if (akfos == 0 || 1 == 0)
		{
			return "any";
		}
		if (akfos == wpagn)
		{
			return akfos.ToString(CultureInfo.InvariantCulture);
		}
		return brgjd.edcru("{0}-{1}", akfos, wpagn);
	}
}
