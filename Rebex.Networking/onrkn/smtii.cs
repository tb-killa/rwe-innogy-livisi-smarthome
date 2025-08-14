using Rebex.Security.Certificates;

namespace onrkn;

internal class smtii
{
	private string llucy;

	private nxtme<string> gzkuk;

	private bool zexgk;

	private nxtme<CertificateChain> qgsti;

	private nxtme<CertificateChain> cwlka;

	private string jagsa;

	public string duqkj
	{
		get
		{
			return llucy;
		}
		private set
		{
			llucy = value;
		}
	}

	public nxtme<string> elihi
	{
		get
		{
			return gzkuk;
		}
		private set
		{
			gzkuk = value;
		}
	}

	public bool yliig
	{
		get
		{
			return zexgk;
		}
		private set
		{
			zexgk = value;
		}
	}

	public nxtme<CertificateChain> ohkim
	{
		get
		{
			return qgsti;
		}
		private set
		{
			qgsti = value;
		}
	}

	public nxtme<CertificateChain> vubgy
	{
		get
		{
			return cwlka;
		}
		set
		{
			cwlka = value;
		}
	}

	public string vwhek
	{
		get
		{
			return jagsa;
		}
		set
		{
			jagsa = value;
		}
	}

	public smtii(string serverName, nxtme<string> applicationProtocols, bool isCertificateRequired)
		: this(serverName, applicationProtocols, nxtme<CertificateChain>.gihlo, isCertificateRequired)
	{
	}

	public smtii(string serverName, nxtme<string> applicationProtocols, nxtme<CertificateChain> defaultServerCertificates, bool isCertificateRequired)
	{
		duqkj = serverName;
		elihi = applicationProtocols;
		yliig = isCertificateRequired;
		ohkim = defaultServerCertificates;
	}

	public void uxhkn(nxtme<CertificateChain> p0)
	{
		ohkim = p0;
	}
}
