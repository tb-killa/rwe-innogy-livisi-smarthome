using Rebex.Net;

namespace onrkn;

internal class auaka
{
	private TlsCipherSuite vsgkm;

	private TlsProtocol fefap;

	private TlsVersion iowrb;

	private bool frasu;

	private bool diylg;

	private bool wqqpw;

	public TlsCipherSuite zjvps
	{
		get
		{
			return vsgkm;
		}
		private set
		{
			vsgkm = value;
		}
	}

	public TlsProtocol lmbkk
	{
		get
		{
			return fefap;
		}
		private set
		{
			fefap = value;
		}
	}

	public TlsVersion nxzna
	{
		get
		{
			return iowrb;
		}
		private set
		{
			iowrb = value;
		}
	}

	public bool ikkfq
	{
		get
		{
			return frasu;
		}
		private set
		{
			frasu = value;
		}
	}

	public bool xaaih
	{
		get
		{
			return diylg;
		}
		private set
		{
			diylg = value;
		}
	}

	public bool vcjly
	{
		get
		{
			return wqqpw;
		}
		private set
		{
			wqqpw = value;
		}
	}

	public auaka(TlsCipherSuite allowedSuites, TlsProtocol protocol, TlsVersion version, bool secureRenegotiationEnabled, bool extendedMasterSecretEnabled, bool tls12ClientHelloExists)
	{
		zjvps = allowedSuites;
		lmbkk = protocol;
		nxzna = version;
		ikkfq = secureRenegotiationEnabled;
		xaaih = extendedMasterSecretEnabled;
		vcjly = tls12ClientHelloExists;
	}
}
