using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal abstract class gipfx
{
	protected readonly object zxhzq;

	protected ProxyAuthentication wbsqt;

	protected string zweft;

	protected ProxyType gnyvf;

	protected string meang;

	protected string gobbp;

	protected string tyocn;

	protected string edbsx;

	protected int fdtan;

	protected PortRange lgsdc;

	protected Encoding alsmq;

	protected int? ycqma;

	protected int? hfqbh;

	private bavnf opzmj;

	private bool vcjnm;

	protected int njwqv;

	private ILogWriter bdocn;

	private static int fhdku;

	private readonly int xtoet;

	protected bavnf pnwnu => opzmj;

	protected gipfx()
	{
		zxhzq = new object();
		xtoet = Interlocked.Increment(ref fhdku);
		fdtan = -1;
		lgsdc = PortRange.Any;
		alsmq = EncodingTools.Default;
		ycqma = 8192;
		hfqbh = 8192;
	}

	public abstract EndPoint kkmbs(EndPoint p0);

	public abstract EndPoint zebbd(ProxySocket p0);

	public abstract EndPoint ofjnw();

	protected void fpizj(LogLevel p0, string p1, string p2, byte[] p3, int p4, int p5)
	{
		ILogWriter logWriter = bdocn;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(ProxySocket), xtoet, p1, p2, p3, p4, p5);
		}
	}

	protected void hvcsu(LogLevel p0, string p1, string p2, params object[] p3)
	{
		ILogWriter logWriter = bdocn;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(message: (p3 != null && 0 == 0 && ((p3.Length != 0) ? true : false)) ? brgjd.edcru(p2, p3) : p2, level: p0, objectType: typeof(ProxySocket), objectId: xtoet, area: p1);
		}
	}

	public virtual void eesdi()
	{
		bavnf bavnf2;
		lock (zxhzq)
		{
			bavnf2 = opzmj;
			opzmj = null;
			vcjnm = true;
		}
		if (bavnf2 != null && 0 == 0)
		{
			bavnf2.Close();
		}
	}

	public bavnf ljwfg()
	{
		bavnf bavnf2;
		lock (zxhzq)
		{
			bavnf2 = opzmj;
			opzmj = null;
			vcjnm = true;
		}
		if (bavnf2 == null || 1 == 0)
		{
			throw new InvalidOperationException("Operation is not allowed in the current state.");
		}
		return bavnf2;
	}

	protected void fgokm(bavnf p0)
	{
		lock (zxhzq)
		{
			opzmj = p0;
		}
	}

	protected void npqbr(IPEndPoint p0)
	{
		bavnf bavnf2 = new bavnf();
		lock (zxhzq)
		{
			if (vcjnm && 0 == 0)
			{
				throw new ProxySocketException("An operation timed out.", ProxySocketExceptionStatus.Timeout);
			}
			opzmj = bavnf2;
		}
		if (lgsdc != PortRange.Any)
		{
			bavnf bavnf3 = bavnf2;
			if (bavnf3 != null && 0 == 0)
			{
				bavnf3.hdyyf(p0.AddressFamily);
				lgsdc.lboxg(bavnf3.ewsij, IPAddress.Any);
			}
		}
		string p1 = ((gnyvf == ProxyType.None) ? "Connecting to {1}:{2} (no proxy)." : "Connecting to {0} proxy at {1}:{2}.");
		hvcsu(LogLevel.Debug, "Proxy", p1, Proxy.uhklq(gnyvf), p0.Address, p0.Port);
		pnwnu.Connect(p0);
		hvcsu(LogLevel.Debug, "Proxy", "Connection established.");
	}

	protected void msrrm(byte[] p0)
	{
		fpizj(LogLevel.Verbose, "Proxy", "Sending data:", p0, 0, p0.Length);
		int num = 0;
		int num2 = p0.Length;
		while (num2 > 0)
		{
			int num3 = pnwnu.Send(p0, num, num2, SocketFlags.None);
			num += num3;
			num2 -= num3;
		}
	}

	protected IPEndPoint kzurk(Rebex.Net.DnsEndPoint p0, ProxySocketExceptionStatus p1)
	{
		wqwvw(p0, out var p2, out var p3);
		return bgsra(p2, p3, p1);
	}

	protected IPEndPoint bgsra(string p0, int p1, ProxySocketExceptionStatus p2)
	{
		IPEndPoint iPEndPoint = auilw.bolwk(p0, p1);
		if (iPEndPoint != null && 0 == 0)
		{
			return iPEndPoint;
		}
		hvcsu(LogLevel.Debug, "Proxy", "Resolving '{0}'.", p0);
		try
		{
			IPHostEntry p3 = auilw.qennv(p0);
			return auilw.tulbp(p3, p1);
		}
		catch (SocketException innerException)
		{
			throw new ProxySocketException("Unable to resolve hostname.", p2, innerException);
		}
	}

	protected static long fdsor(IPAddress p0)
	{
		byte[] addressBytes = p0.GetAddressBytes();
		if (addressBytes.Length != 4)
		{
			throw new ArgumentOutOfRangeException("address");
		}
		return (uint)(addressBytes[0] + (addressBytes[1] << 8) + (addressBytes[2] << 16) + (addressBytes[3] << 24));
	}

	protected static void wqwvw(EndPoint p0, out string p1, out int p2)
	{
		if (p0 is IPEndPoint iPEndPoint && 0 == 0)
		{
			p1 = iPEndPoint.Address.ToString();
			p2 = iPEndPoint.Port;
			return;
		}
		if (!(p0 is Rebex.Net.DnsEndPoint dnsEndPoint) || 1 == 0)
		{
			throw new ProxySocketException("Unsupported endpoint.", ProxySocketExceptionStatus.UnclassifiableError);
		}
		p1 = dnsEndPoint.Host;
		p2 = dnsEndPoint.Port;
	}

	protected static void owvxn(EndPoint p0, out IPAddress p1, out string p2, out int p3)
	{
		if (p0 is IPEndPoint iPEndPoint && 0 == 0)
		{
			p1 = iPEndPoint.Address;
			p2 = p1.ToString();
			p3 = iPEndPoint.Port;
			return;
		}
		if (!(p0 is Rebex.Net.DnsEndPoint dnsEndPoint) || 1 == 0)
		{
			throw new ProxySocketException("Unsupported endpoint.", ProxySocketExceptionStatus.UnclassifiableError);
		}
		p2 = dnsEndPoint.Host;
		p3 = dnsEndPoint.Port;
		p1 = auilw.bolwk(p2, p3)?.Address;
	}

	public static gipfx xvlrv(Proxy p0, int p1)
	{
		return yhdsm(p0, p1, p2: false);
	}

	public static gipfx rldhz(Proxy p0, int p1, EndPoint p2)
	{
		wqwvw(p2, out var p3, out var p4);
		bool p5 = p0.IsBypassed(p3, p4);
		return yhdsm(p0, p1, p5);
	}

	private static gipfx yhdsm(Proxy p0, int p1, bool p2)
	{
		ProxyType proxyType = p0.ProxyType;
		string host = p0.Host;
		if (p2 && 0 == 0)
		{
			proxyType = ProxyType.None;
		}
		if (proxyType != ProxyType.None && 0 == 0)
		{
			if (host == null || 1 == 0)
			{
				throw new ProxySocketException("Hostname cannot be null.", ProxySocketExceptionStatus.ProxyNameResolutionFailure);
			}
			if (host.Trim().Length == 0 || 1 == 0)
			{
				throw new ProxySocketException("Hostname cannot be empty.", ProxySocketExceptionStatus.ProxyNameResolutionFailure);
			}
		}
		gipfx gipfx2 = proxyType switch
		{
			ProxyType.None => new nufwy(), 
			ProxyType.Socks4 => new nbpox(socks4a: false), 
			ProxyType.Socks4a => new nbpox(socks4a: true), 
			ProxyType.Socks5 => new ejpvr(), 
			ProxyType.HttpConnect => new rbuzv(), 
			_ => throw new InvalidOperationException("Unsupported proxy type."), 
		};
		gipfx2.lgsdc = p0.LocalPortRange;
		gipfx2.njwqv = p1;
		gipfx2.alsmq = p0.Encoding;
		gipfx2.bdocn = p0.LogWriter;
		gipfx2.gnyvf = proxyType;
		if (proxyType != ProxyType.None && 0 == 0)
		{
			gipfx2.wbsqt = p0.AuthenticationMethod;
			gipfx2.zweft = p0.HttpUserAgent;
			gipfx2.edbsx = host;
			gipfx2.fdtan = p0.Port;
			string userName = p0.UserName;
			string password = p0.Password;
			string domain = p0.Domain;
			if (userName != null && 0 == 0)
			{
				gipfx2.meang = userName;
			}
			else
			{
				gipfx2.meang = "";
			}
			if (password != null && 0 == 0)
			{
				gipfx2.gobbp = password;
			}
			else
			{
				gipfx2.gobbp = "";
			}
			if (domain != null && 0 == 0)
			{
				gipfx2.tyocn = domain;
			}
			else
			{
				gipfx2.tyocn = "";
			}
		}
		return gipfx2;
	}
}
