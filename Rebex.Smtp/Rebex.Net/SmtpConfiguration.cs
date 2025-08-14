using System;
using System.ComponentModel;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public class SmtpConfiguration
{
	private bool crqwt;

	private string jlrpx;

	private int lnrrg = 25;

	private string nwapq;

	private string hdrgy;

	private Proxy pulpg;

	private SmtpAuthentication apcjc;

	private SmtpDeliveryEngine oaurq;

	private SmtpDeliveryMethod spknp;

	private string oxjuh;

	private ILogWriter wokxz;

	private SmtpSettings ohjxg;

	private string iilvy;

	private CertificateChain mldbi;

	private TlsParameters ojymr;

	private SslMode yblvy;

	public string ServerName
	{
		get
		{
			return jlrpx;
		}
		set
		{
			cbkiy();
			jlrpx = value;
		}
	}

	public int ServerPort
	{
		get
		{
			return lnrrg;
		}
		set
		{
			if (value <= 0 || value > 65535)
			{
				throw hifyx.nztrs("value", value, "Port is out of range of valid values.");
			}
			cbkiy();
			lnrrg = value;
		}
	}

	public string UserName
	{
		get
		{
			return nwapq;
		}
		set
		{
			cbkiy();
			nwapq = value;
		}
	}

	public string Password
	{
		get
		{
			return hdrgy;
		}
		set
		{
			cbkiy();
			hdrgy = value;
		}
	}

	public Proxy Proxy
	{
		get
		{
			return pulpg;
		}
		set
		{
			cbkiy();
			pulpg = value;
		}
	}

	public SmtpAuthentication AuthenticationMethod
	{
		get
		{
			return apcjc;
		}
		set
		{
			cbkiy();
			apcjc = value;
		}
	}

	public SmtpDeliveryEngine DeliveryEngine
	{
		get
		{
			return oaurq;
		}
		set
		{
			cbkiy();
			oaurq = value;
		}
	}

	public SmtpDeliveryMethod DeliveryMethod
	{
		get
		{
			return spknp;
		}
		set
		{
			cbkiy();
			spknp = value;
		}
	}

	public string PickupDirectoryPath
	{
		get
		{
			return oxjuh;
		}
		set
		{
			cbkiy();
			oxjuh = value;
		}
	}

	public ILogWriter LogWriter
	{
		get
		{
			return wokxz;
		}
		set
		{
			cbkiy();
			wokxz = value;
		}
	}

	public SmtpSettings Settings
	{
		get
		{
			return ohjxg;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value", "Value cannot be null.");
			}
			ohjxg = value;
			ohjxg.rvdnp++;
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public SmtpOptions Options
	{
		get
		{
			return ohjxg.mvylp();
		}
		set
		{
			if (ohjxg.rvdnp > 1)
			{
				throw new InvalidOperationException("Invalid call of old API. Use the Settings properties instead.");
			}
			ohjxg.civyv(value);
		}
	}

	public string From
	{
		get
		{
			return iilvy;
		}
		set
		{
			cbkiy();
			iilvy = value;
		}
	}

	public CertificateChain ClientCertificate
	{
		get
		{
			return mldbi;
		}
		set
		{
			cbkiy();
			mldbi = value;
		}
	}

	public TlsParameters Parameters
	{
		get
		{
			return ojymr;
		}
		set
		{
			cbkiy();
			ojymr = value;
		}
	}

	public SslMode SslMode
	{
		get
		{
			return yblvy;
		}
		set
		{
			cbkiy();
			yblvy = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This property has been deprecated and will be removed. Use SslMode instead.", true)]
	[wptwl(false)]
	public SmtpSecurity Security
	{
		get
		{
			return (SmtpSecurity)yblvy;
		}
		set
		{
			cbkiy();
			yblvy = (SslMode)value;
		}
	}

	private void cbkiy()
	{
		if (crqwt && 0 == 0)
		{
			throw new InvalidOperationException("Cannot modify default SmtpConfiguration.");
		}
	}

	public SmtpConfiguration()
		: this(SmtpConfigurationSource.Empty)
	{
	}

	private SmtpConfiguration(SmtpConfigurationSource source, SmtpDeliveryEngine engine, bool isReadOnly)
	{
		lnrrg = 25;
		ohjxg = new SmtpSettings();
		DeliveryEngine = engine;
		crqwt = isReadOnly;
	}

	public SmtpConfiguration(SmtpConfigurationSource source)
		: this(source, SmtpDeliveryEngine.Rebex, isReadOnly: false)
	{
	}

	public SmtpConfiguration(SmtpConfigurationSource source, SmtpDeliveryEngine engine)
		: this(source, engine, isReadOnly: false)
	{
	}
}
