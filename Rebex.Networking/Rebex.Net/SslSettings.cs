using System;
using System.Collections.Generic;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public abstract class SslSettings
{
	private TlsParameters rwanz;

	private bool usdkl;

	private ValidationOptions zypmi;

	private bool mkmfj;

	public bool SslAcceptAllCertificates
	{
		get
		{
			return usdkl;
		}
		set
		{
			usdkl = value;
		}
	}

	public TlsVersion SslAllowedVersions
	{
		get
		{
			return rwanz.Version;
		}
		set
		{
			rwanz.Version = value;
		}
	}

	public string SslServerName
	{
		get
		{
			return rwanz.CommonName;
		}
		set
		{
			rwanz.CommonName = value;
		}
	}

	public SignatureHashAlgorithm SslPreferredHashAlgorithm
	{
		get
		{
			return rwanz.PreferredHashAlgorithm;
		}
		set
		{
			rwanz.PreferredHashAlgorithm = value;
		}
	}

	public TlsCipherSuite SslAllowedSuites
	{
		get
		{
			return rwanz.AllowedSuites;
		}
		set
		{
			rwanz.AllowedSuites = value;
		}
	}

	public TlsEllipticCurve SslAllowedCurves
	{
		get
		{
			return rwanz.AllowedCurves;
		}
		set
		{
			rwanz.AllowedCurves = value;
		}
	}

	public bool SslAllowVulnerableSuites
	{
		get
		{
			return rwanz.AllowVulnerableSuites;
		}
		set
		{
			rwanz.AllowVulnerableSuites = value;
		}
	}

	public bool SslDoNotInsertEmptyFragment
	{
		get
		{
			return (rwanz.Options & TlsOptions.DoNotInsertEmptyFragment) != 0;
		}
		set
		{
			if (value && 0 == 0)
			{
				rwanz.Options |= TlsOptions.DoNotInsertEmptyFragment;
			}
			else
			{
				rwanz.Options &= ~TlsOptions.DoNotInsertEmptyFragment;
			}
		}
	}

	public bool SslRenegotiationExtensionEnabled
	{
		get
		{
			return (rwanz.Options & TlsOptions.DisableRenegotiationExtension) == 0;
		}
		set
		{
			if (value && 0 == 0)
			{
				rwanz.Options &= ~TlsOptions.DisableRenegotiationExtension;
			}
			else
			{
				rwanz.Options |= TlsOptions.DisableRenegotiationExtension;
			}
		}
	}

	public bool SslExtendedMasterSecretEnabled
	{
		get
		{
			return (rwanz.Options & TlsOptions.DisableExtendedMasterSecret) == 0;
		}
		set
		{
			if (value && 0 == 0)
			{
				rwanz.Options &= ~TlsOptions.DisableExtendedMasterSecret;
			}
			else
			{
				rwanz.Options |= TlsOptions.DisableExtendedMasterSecret;
			}
		}
	}

	public bool SslServerNameIndicationEnabled
	{
		get
		{
			return (rwanz.Options & TlsOptions.DisableServerNameIndication) == 0;
		}
		set
		{
			if (value && 0 == 0)
			{
				rwanz.Options &= ~TlsOptions.DisableServerNameIndication;
			}
			else
			{
				rwanz.Options |= TlsOptions.DisableServerNameIndication;
			}
		}
	}

	public ICertificateVerifier SslServerCertificateVerifier
	{
		get
		{
			return rwanz.CertificateVerifier;
		}
		set
		{
			rwanz.CertificateVerifier = value;
		}
	}

	public ICertificateRequestHandler SslClientCertificateRequestHandler
	{
		get
		{
			return rwanz.CertificateRequestHandler;
		}
		set
		{
			rwanz.CertificateRequestHandler = value;
		}
	}

	public int SslMinimumDiffieHellmanKeySize
	{
		get
		{
			return rwanz.MinimumDiffieHellmanKeySize;
		}
		set
		{
			rwanz.MinimumDiffieHellmanKeySize = value;
		}
	}

	public TlsSession SslSession
	{
		get
		{
			return rwanz.Session;
		}
		set
		{
			rwanz.Session = value;
		}
	}

	public ValidationOptions SslServerCertificateValidationOptions
	{
		get
		{
			return zypmi;
		}
		set
		{
			zypmi = value;
		}
	}

	public bool SslStrictKeyUsageValidation
	{
		get
		{
			return mkmfj;
		}
		set
		{
			mkmfj = value;
		}
	}

	protected TlsParameters ToParameters()
	{
		return rwanz.Clone();
	}

	internal TlsParameters mfcks(TlsParameters p0, rlpyd p1, string p2, Func<string, Exception> p3)
	{
		p0 = ((p0 != null) ? p0.Clone() : p1.mlept.ToParameters());
		if (p3 != null && 0 == 0)
		{
			if (p0.Version == TlsVersion.None || 1 == 0)
			{
				throw p3("At least one of available TLS/SSL protocol versions must be specified.");
			}
			if (p0.AllowedSuites == TlsCipherSuite.None)
			{
				throw p3("No TLS/SSL cipher was specified, or none of the specified ciphers is currently usable.");
			}
		}
		if (p0.CommonName == null || false || p0.CommonName.Length == 0 || 1 == 0)
		{
			p0.CommonName = p2;
		}
		p0.CertificateVerifier = new uchjr(p1, p0.CertificateVerifier);
		return p0;
	}

	public SslSettings()
	{
		rwanz = new TlsParameters();
	}

	public ICollection<TlsCipherSuite> GetPreferredSuites()
	{
		return rwanz.GetPreferredSuites();
	}

	public void SetPreferredSuites(params TlsCipherSuite[] suites)
	{
		rwanz.SetPreferredSuites(suites);
	}
}
