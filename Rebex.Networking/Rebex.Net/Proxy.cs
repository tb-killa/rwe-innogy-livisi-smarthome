using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using onrkn;

namespace Rebex.Net;

public class Proxy : ISocketFactory, IWebProxy
{
	[NonSerialized]
	private NetworkCredential nraos;

	[NonSerialized]
	private string ajdop;

	[NonSerialized]
	private string iehwg;

	[NonSerialized]
	private string fvysr;

	[NonSerialized]
	private ProxyType kkath;

	[NonSerialized]
	private ProxyAuthentication tuajf;

	[NonSerialized]
	private string jmbkl;

	[NonSerialized]
	private int gziew;

	[NonSerialized]
	private PortRange pbibv;

	[NonSerialized]
	private WebProxy dhaub;

	[NonSerialized]
	private int mlibg;

	[NonSerialized]
	private Encoding ngfqy;

	[NonSerialized]
	private ILogWriter uvttj;

	private string kqhml;

	public string Host
	{
		get
		{
			return jmbkl;
		}
		set
		{
			if (value != null && 0 == 0 && value.IndexOf("://", StringComparison.Ordinal) >= 0)
			{
				if (!Uri.TryCreate(value, UriKind.Absolute, out var result) || 1 == 0)
				{
					throw new ArgumentException("Hostname is invalid.", "value");
				}
				if (result.Scheme != "http" && 0 == 0)
				{
					throw new ArgumentException("Hostname is invalid.", "value");
				}
				jmbkl = result.Host;
				gziew = result.Port;
			}
			else
			{
				jmbkl = value;
			}
		}
	}

	public int Port
	{
		get
		{
			return gziew;
		}
		set
		{
			if (value < 1 || value > 65535)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			gziew = value;
		}
	}

	public ProxyType ProxyType
	{
		get
		{
			return kkath;
		}
		set
		{
			if (!Enum.IsDefined(typeof(ProxyType), value) || 1 == 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			kkath = value;
		}
	}

	public ProxyAuthentication AuthenticationMethod
	{
		get
		{
			return tuajf;
		}
		set
		{
			if (!Enum.IsDefined(typeof(ProxyAuthentication), value) || 1 == 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			tuajf = value;
		}
	}

	public string HttpUserAgent
	{
		get
		{
			return kqhml;
		}
		set
		{
			kqhml = value;
		}
	}

	public ICredentials Credentials
	{
		get
		{
			return nraos;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				nraos = null;
				ajdop = null;
				iehwg = null;
				fvysr = null;
				return;
			}
			if (!(value is NetworkCredential) || 1 == 0)
			{
				throw new NotSupportedException("Unsupported credential type.");
			}
			nraos = (NetworkCredential)value;
			ajdop = nraos.UserName;
			iehwg = nraos.Domain;
			fvysr = nraos.Password;
		}
	}

	public string UserName
	{
		get
		{
			return ajdop;
		}
		set
		{
			if (nraos == null || 1 == 0)
			{
				nraos = new NetworkCredential();
			}
			nraos.UserName = value;
			ajdop = value;
		}
	}

	public string Password
	{
		get
		{
			return fvysr;
		}
		set
		{
			if (nraos == null || 1 == 0)
			{
				nraos = new NetworkCredential();
			}
			nraos.Password = value;
			fvysr = value;
		}
	}

	public string Domain
	{
		get
		{
			return iehwg;
		}
		set
		{
			if (nraos == null || 1 == 0)
			{
				nraos = new NetworkCredential();
			}
			nraos.Domain = value;
			iehwg = value;
		}
	}

	public PortRange LocalPortRange
	{
		get
		{
			return pbibv;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				value = PortRange.Any;
			}
			pbibv = value;
		}
	}

	public ILogWriter LogWriter
	{
		get
		{
			return uvttj;
		}
		set
		{
			uvttj = value;
		}
	}

	public bool BypassProxyOnLocal
	{
		get
		{
			return dhaub.BypassProxyOnLocal;
		}
		set
		{
			dhaub.BypassProxyOnLocal = value;
		}
	}

	public Encoding Encoding
	{
		get
		{
			return ngfqy;
		}
		set
		{
			ngfqy = value;
		}
	}

	public int SendRetryTimeout
	{
		get
		{
			return mlibg;
		}
		set
		{
			if (value < -1)
			{
				throw hifyx.nztrs("value", value, "Timeout is out of range of valid values.");
			}
			if (value < 1)
			{
				value = -1;
			}
			else if (value < 1000)
			{
				value = 1000;
			}
			mlibg = value;
		}
	}

	private ISocket egnpm()
	{
		return new ProxySocket(this);
	}

	ISocket ISocketFactory.CreateSocket()
	{
		//ILSpy generated this explicit interface implementation from .override directive in egnpm
		return this.egnpm();
	}

	public override string ToString()
	{
		if (kkath == ProxyType.None)
		{
			return "none";
		}
		string text = uhklq(kkath);
		return brgjd.edcru("{0} {1}:{2}", text, jmbkl, gziew);
	}

	internal static string uhklq(ProxyType p0)
	{
		return p0 switch
		{
			ProxyType.None => "none", 
			ProxyType.HttpConnect => "HTTP CONNECT", 
			ProxyType.Socks4 => "SOCKS4", 
			ProxyType.Socks4a => "SOCKS4A", 
			ProxyType.Socks5 => "SOCKS5", 
			_ => "UNKNOWN", 
		};
	}

	public virtual Proxy Clone()
	{
		Proxy proxy = CreateClone();
		proxy.Host = Host;
		proxy.Port = Port;
		proxy.BypassProxyOnLocal = BypassProxyOnLocal;
		proxy.Credentials = Credentials;
		proxy.AuthenticationMethod = AuthenticationMethod;
		proxy.HttpUserAgent = HttpUserAgent;
		proxy.Encoding = Encoding;
		proxy.LocalPortRange = LocalPortRange;
		proxy.LogWriter = LogWriter;
		proxy.ProxyType = ProxyType;
		proxy.SendRetryTimeout = SendRetryTimeout;
		return proxy;
	}

	protected virtual Proxy CreateClone()
	{
		return new Proxy();
	}

	protected virtual Uri GetProxyAddress()
	{
		string schema;
		switch (kkath)
		{
		default:
			return null;
		case ProxyType.Socks4:
			schema = "socks4";
			break;
		case ProxyType.Socks4a:
			schema = "socks4a";
			break;
		case ProxyType.Socks5:
			schema = "socks5";
			break;
		case ProxyType.HttpConnect:
			schema = "http";
			break;
		}
		return GetProxyAddress(jmbkl, gziew, schema);
	}

	protected Uri GetProxyAddress(string hostName, int port, string schema)
	{
		IPEndPoint iPEndPoint = auilw.bolwk(hostName, port);
		if (iPEndPoint == null || false || iPEndPoint.AddressFamily == AddressFamily.InterNetwork)
		{
			return new Uri(schema + "://" + hostName + ":" + port);
		}
		return new Uri(schema + "://[" + hostName + "]:" + port);
	}

	private void cgutx()
	{
		dhaub.Address = GetProxyAddress();
	}

	public Uri GetProxy(Uri destination)
	{
		if (destination == null && 0 == 0)
		{
			throw new ArgumentNullException("destination");
		}
		if (IsBypassed(destination) && 0 == 0)
		{
			return destination;
		}
		return dhaub.Address;
	}

	public bool IsBypassed(Uri host)
	{
		if (host == null && 0 == 0)
		{
			throw new ArgumentNullException("host");
		}
		cgutx();
		if (dhaub.Address == null && 0 == 0)
		{
			return true;
		}
		if (dhaub.Address != null && 0 == 0 && dhaub.Address.IsLoopback && 0 == 0 && host.IsLoopback && 0 == 0)
		{
			return dhaub.BypassProxyOnLocal;
		}
		return dhaub.IsBypassed(host);
	}

	public virtual bool IsBypassed(string hostName, int port)
	{
		Uri proxyAddress = GetProxyAddress(hostName, port, "smtp");
		return IsBypassed(proxyAddress);
	}

	public Proxy()
		: this(ProxyType.None, null, 1080)
	{
	}

	public Proxy(ProxyType proxyType, string host, int port)
		: this(proxyType, ProxyAuthentication.Basic, host, port, null)
	{
	}

	[wptwl(false)]
	[Obsolete("This constructor has been deprecated. Please use Proxy() constructor and set the properties instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Proxy(ProxyType proxyType, string host, int port, string username)
		: this(proxyType, ProxyAuthentication.Basic, host, port, new NetworkCredential(username, ""))
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This constructor has been deprecated. Please use Proxy() constructor and set the properties instead.", false)]
	[wptwl(false)]
	public Proxy(ProxyType proxyType, ProxyAuthentication authenticationMethod, string host, int port, NetworkCredential credentials)
		: this(proxyType, authenticationMethod, host, port, bypassOnLocal: false, credentials)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This constructor has been deprecated. Please use Proxy() constructor and set the properties instead.", false)]
	public Proxy(ProxyType proxyType, ProxyAuthentication authenticationMethod, string host, int port, bool bypassOnLocal, NetworkCredential credentials)
	{
		fbkvu();
		ProxyType = proxyType;
		AuthenticationMethod = authenticationMethod;
		Host = host;
		Port = port;
		BypassProxyOnLocal = bypassOnLocal;
		Credentials = credentials;
	}

	private void fbkvu()
	{
		pbibv = PortRange.Any;
		dhaub = new WebProxy();
		ngfqy = EncodingTools.Default;
	}
}
