using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Net;

public class TlsParameters
{
	private const string axyoa = "TlsSocket12_PreferTaskBasedCore_Option";

	internal static readonly CertificateChain[] okowo = new CertificateChain[0];

	internal static readonly string[] myvyg = new string[0];

	private bool vyrrv;

	private TlsVersion umpjn = TlsVersion.Any;

	private TlsConnectionEnd inzvz = TlsConnectionEnd.Client;

	private TlsOptions vlqhl;

	private bool xvdes;

	private bool vbsyg;

	private TlsCipherSuite kwfnu = TlsCipherSuite.Fast | TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_GCM_SHA384;

	private ttceu vxacn = ttceu.bjuoa;

	private SignatureHashAlgorithm lxzun = SignatureHashAlgorithm.SHA256;

	private TlsCipherSuite[] pvywt = new TlsCipherSuite[0];

	private TlsEllipticCurve huuqr = TlsEllipticCurve.All;

	private string osymj;

	private ICertificateVerifier miknw = Rebex.Net.CertificateVerifier.Default;

	private gddeo jhxwm;

	private CertificateChain[] bkuuh;

	private ICertificateRequestHandler egnyn = Rebex.Net.CertificateRequestHandler.NoCertificate;

	private TlsSession pokty;

	private string okrsj;

	private DistinguishedName[] ekvua;

	private TlsCertificatePolicy cukla;

	private DiffieHellmanParameters ehnyy;

	private RSAParameters gpomc;

	private int dtofj = 1024;

	private bool plohi;

	private TlsCipherSuite? tsbnm;

	private webdt mzvqb;

	private static Func<string, Exception> bqrse;

	private static Func<Certificate, bool> ruabh;

	internal bool ikufo => vyrrv;

	public TlsVersion Version
	{
		get
		{
			return umpjn;
		}
		set
		{
			uzqkc();
			umpjn = value;
		}
	}

	public bool AllowDeprecatedVersions
	{
		get
		{
			return xvdes;
		}
		set
		{
			uzqkc();
			xvdes = value;
		}
	}

	internal bool aljgi => false;

	public TlsConnectionEnd Entity
	{
		get
		{
			return inzvz;
		}
		set
		{
			uzqkc();
			inzvz = value;
		}
	}

	public TlsOptions Options
	{
		get
		{
			return vlqhl;
		}
		set
		{
			vlqhl = value;
		}
	}

	public bool AllowVulnerableSuites
	{
		get
		{
			return vbsyg;
		}
		set
		{
			uzqkc();
			vbsyg = value;
		}
	}

	public TlsCipherSuite AllowedSuites
	{
		get
		{
			return kwfnu;
		}
		set
		{
			uzqkc();
			kwfnu = value;
		}
	}

	internal TlsCipherSuite? bimgq
	{
		get
		{
			return tsbnm;
		}
		set
		{
			tsbnm = value;
		}
	}

	public TlsEllipticCurve AllowedCurves
	{
		get
		{
			return huuqr;
		}
		set
		{
			uzqkc();
			huuqr = value;
		}
	}

	internal ttceu ggdwh
	{
		get
		{
			return vxacn;
		}
		set
		{
			uzqkc();
			vxacn = value;
		}
	}

	public SignatureHashAlgorithm PreferredHashAlgorithm
	{
		get
		{
			return lxzun;
		}
		set
		{
			uzqkc();
			lxzun = value;
		}
	}

	public string CommonName
	{
		get
		{
			return osymj;
		}
		set
		{
			uzqkc();
			osymj = value;
		}
	}

	public ICertificateVerifier CertificateVerifier
	{
		get
		{
			return miknw;
		}
		set
		{
			uzqkc();
			if (value == null || 1 == 0)
			{
				value = Rebex.Net.CertificateVerifier.AcceptAll;
			}
			miknw = value;
		}
	}

	internal gddeo mfxqd
	{
		get
		{
			gddeo gddeo = jhxwm;
			if (gddeo == null || 1 == 0)
			{
				gddeo = (jhxwm = ishzk.hwcpz);
			}
			return gddeo;
		}
		set
		{
			uzqkc();
			jhxwm = value;
		}
	}

	public CertificateChain Certificate
	{
		get
		{
			CertificateChain[] array = bkuuh;
			if (array == null || false || array.Length == 0 || 1 == 0)
			{
				return null;
			}
			return array[0];
		}
		set
		{
			uzqkc();
			if (value == null || 1 == 0)
			{
				bkuuh = okowo;
				return;
			}
			bkuuh = new CertificateChain[1] { value };
		}
	}

	internal CertificateChain[] qopqb
	{
		get
		{
			CertificateChain[] array = bkuuh;
			if (array == null || 1 == 0)
			{
				array = (bkuuh = okowo);
			}
			return array;
		}
		set
		{
			uzqkc();
			bkuuh = value;
		}
	}

	public ICertificateRequestHandler CertificateRequestHandler
	{
		get
		{
			return egnyn;
		}
		set
		{
			uzqkc();
			if (value == null || 1 == 0)
			{
				value = Rebex.Net.CertificateRequestHandler.NoCertificate;
			}
			egnyn = value;
		}
	}

	[wptwl(false)]
	[Obsolete("SessionID property has been deprecated and will be removed. Use the Session property instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string SessionID
	{
		get
		{
			return okrsj;
		}
		set
		{
			uzqkc();
			if (value == null || 1 == 0)
			{
				okrsj = null;
			}
			else
			{
				okrsj = value;
			}
		}
	}

	public TlsSession Session
	{
		get
		{
			if (pokty != null && 0 == 0)
			{
				return pokty;
			}
			if (okrsj != null && 0 == 0)
			{
				return TlsSession.relpg(okrsj);
			}
			return null;
		}
		set
		{
			uzqkc();
			if (value == null || 1 == 0)
			{
				pokty = null;
			}
			else
			{
				pokty = value;
			}
		}
	}

	public DistinguishedName[] AcceptableAuthorities
	{
		get
		{
			return ekvua;
		}
		set
		{
			uzqkc();
			ekvua = value;
		}
	}

	public TlsCertificatePolicy CertificatePolicy
	{
		get
		{
			return cukla;
		}
		set
		{
			uzqkc();
			cukla = value;
		}
	}

	public DiffieHellmanParameters EphemeralDiffieHellmanParameters
	{
		get
		{
			return ehnyy;
		}
		set
		{
			uzqkc();
			ehnyy = value;
		}
	}

	public int MinimumDiffieHellmanKeySize
	{
		get
		{
			return dtofj;
		}
		set
		{
			if (value < 512 || value > 16384)
			{
				throw hifyx.nztrs("value", value, "Unsupported key size.");
			}
			dtofj = value;
		}
	}

	public RSAParameters TemporaryRSAParameters
	{
		get
		{
			return gpomc;
		}
		set
		{
			uzqkc();
			gpomc = value;
		}
	}

	internal bool xixla
	{
		get
		{
			return plohi;
		}
		set
		{
			plohi = value;
		}
	}

	internal webdt qnpvc
	{
		get
		{
			return mzvqb;
		}
		set
		{
			mzvqb = value;
		}
	}

	public TlsParameters Clone()
	{
		TlsParameters tlsParameters = new TlsParameters();
		tlsParameters.umpjn = umpjn;
		tlsParameters.inzvz = inzvz;
		tlsParameters.vlqhl = vlqhl;
		tlsParameters.xvdes = xvdes;
		tlsParameters.vbsyg = vbsyg;
		tlsParameters.kwfnu = kwfnu;
		tlsParameters.huuqr = huuqr;
		tlsParameters.osymj = osymj;
		tlsParameters.miknw = miknw;
		tlsParameters.jhxwm = ((jhxwm == null || false || (object)jhxwm.GetType() == typeof(ishzk)) ? null : jhxwm);
		tlsParameters.bkuuh = bkuuh;
		tlsParameters.egnyn = egnyn;
		tlsParameters.pokty = pokty;
		tlsParameters.okrsj = okrsj;
		tlsParameters.ekvua = ekvua;
		tlsParameters.cukla = cukla;
		tlsParameters.gpomc = gpomc;
		tlsParameters.ehnyy = ehnyy;
		tlsParameters.dtofj = dtofj;
		tlsParameters.pvywt = pvywt;
		tlsParameters.xixla = plohi;
		tlsParameters.vxacn = vxacn;
		tlsParameters.lxzun = lxzun;
		tlsParameters.qnpvc = qnpvc;
		tlsParameters.bimgq = bimgq;
		return tlsParameters;
	}

	internal TlsParameters tcdgr()
	{
		TlsParameters tlsParameters = Clone();
		tlsParameters.vyrrv = true;
		return tlsParameters;
	}

	private void uzqkc()
	{
		if (vyrrv && 0 == 0)
		{
			throw new InvalidOperationException("Cannot change parameters that are being used for a socket at the moment.");
		}
	}

	internal string uachz()
	{
		if (umpjn == TlsVersion.SSL30)
		{
			return "SSL";
		}
		if ((umpjn & TlsVersion.SSL30) == 0 || 1 == 0)
		{
			return "TLS";
		}
		return "TLS/SSL";
	}

	internal TlsVersion vhnmk()
	{
		if (!CryptoHelper.UseFipsAlgorithmsOnly || 1 == 0)
		{
			return umpjn & (TlsVersion.Any | TlsVersion.SSL30);
		}
		return umpjn & TlsVersion.Any;
	}

	internal TlsCipherSuite tznry()
	{
		TlsCipherSuite tlsCipherSuite = kwfnu;
		if (!vbsyg || 1 == 0)
		{
			tlsCipherSuite &= ~(TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_RSA_EXPORT_WITH_DES40_CBC_SHA);
		}
		if (!wfcez.riopy(16, 16) || 1 == 0)
		{
			tlsCipherSuite &= ~(TlsCipherSuite.RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384);
		}
		if (!CryptoHelper.UseFipsAlgorithmsOnly || 1 == 0)
		{
			return tlsCipherSuite;
		}
		if (!SymmetricKeyAlgorithm.IsSupported(SymmetricKeyAlgorithmId.AES) || 1 == 0)
		{
			return tlsCipherSuite & (TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DHE_DSS_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DH_anon_WITH_3DES_EDE_CBC_SHA);
		}
		return tlsCipherSuite & (TlsCipherSuite.Weak | TlsCipherSuite.Fast | TlsCipherSuite.DHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DH_anon_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DH_anon_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DH_anon_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DH_anon_WITH_AES_128_CBC_SHA | TlsCipherSuite.DH_anon_WITH_AES_256_CBC_SHA);
	}

	public ICollection<TlsCipherSuite> GetPreferredSuites()
	{
		return (TlsCipherSuite[])pvywt.Clone();
	}

	public void SetPreferredSuites(params TlsCipherSuite[] suites)
	{
		if (suites == null || 1 == 0)
		{
			throw new ArgumentNullException("suites");
		}
		pvywt = suites;
	}

	internal static urofm nbrrb(Certificate p0)
	{
		string key;
		if ((key = p0.awyrh()) != null && 0 == 0)
		{
			if (awprl.mmowy == null || 1 == 0)
			{
				awprl.mmowy = new Dictionary<string, int>(6)
				{
					{ "1.2.840.10045.3.1.7", 0 },
					{ "1.3.132.0.34", 1 },
					{ "1.3.132.0.35", 2 },
					{ "1.3.36.3.3.2.8.1.1.7", 3 },
					{ "1.3.36.3.3.2.8.1.1.11", 4 },
					{ "1.3.36.3.3.2.8.1.1.13", 5 }
				};
			}
			if (awprl.mmowy.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					return urofm.lzkxw;
				case 1:
					return urofm.eaxxm;
				case 2:
					return urofm.vmdec;
				case 3:
					return urofm.mfnhs;
				case 4:
					return urofm.hfvjd;
				case 5:
					return urofm.bnugk;
				}
			}
		}
		return (urofm)0;
	}

	internal bool tokwu(urofm p0, out TlsEllipticCurve p1, out string p2, out string p3, out int p4)
	{
		switch (p0)
		{
		case urofm.lzkxw:
			p1 = TlsEllipticCurve.NistP256;
			p2 = "1.2.840.10045.3.1.7";
			p3 = "NIST P-256 curve";
			p4 = 32;
			return true;
		case urofm.eaxxm:
			p1 = TlsEllipticCurve.NistP384;
			p2 = "1.3.132.0.34";
			p4 = 48;
			p3 = "NIST P-384 curve";
			return true;
		case urofm.vmdec:
			p1 = TlsEllipticCurve.NistP521;
			p2 = "1.3.132.0.35";
			p4 = 66;
			p3 = "NIST P-521 curve";
			return true;
		case urofm.mfnhs:
			p1 = TlsEllipticCurve.BrainpoolP256R1;
			p2 = "1.3.36.3.3.2.8.1.1.7";
			p3 = "Brainpool P-256 R1 curve";
			p4 = 32;
			return true;
		case urofm.hfvjd:
			p1 = TlsEllipticCurve.BrainpoolP384R1;
			p2 = "1.3.36.3.3.2.8.1.1.11";
			p4 = 48;
			p3 = "Brainpool P-384 R1 curve";
			return true;
		case urofm.bnugk:
			p1 = TlsEllipticCurve.BrainpoolP512R1;
			p2 = "1.3.36.3.3.2.8.1.1.13";
			p4 = 64;
			p3 = "Brainpool P-512 R1 curve";
			return true;
		case urofm.qeprw:
			p1 = TlsEllipticCurve.Curve25519;
			p2 = "curve25519";
			p4 = 32;
			p3 = "Curve 25519";
			return true;
		default:
			p1 = TlsEllipticCurve.None;
			p2 = null;
			p4 = 0;
			p3 = null;
			return false;
		}
	}

	internal bool uocnj()
	{
		if ((vlqhl & TlsOptions.DisableExtendedMasterSecret) != TlsOptions.None && 0 == 0)
		{
			return false;
		}
		return AsymmetricKeyAlgorithm.ckgxm();
	}

	internal bool apjxo(AsymmetricKeyAlgorithmId p0, string p1)
	{
		return AsymmetricKeyAlgorithm.iexxf(p0, p1, 0) == zxjln.iuckt;
	}

	internal bool cfmws(urofm p0, TlsProtocol p1)
	{
		if (!tokwu(p0, out var p2, out var p3, out var _, out var _) || 1 == 0)
		{
			return false;
		}
		if ((p2 & AllowedCurves) != p2)
		{
			return false;
		}
		return AsymmetricKeyAlgorithm.iexxf(AsymmetricKeyAlgorithmId.ECDH, p3, 0) switch
		{
			zxjln.iuckt => true, 
			zxjln.dwzpe => false, 
			_ => false, 
		};
	}

	public TlsParameters()
	{
		vlqhl |= TlsOptions.DisableExtendedMasterSecret;
	}

	internal void ijvgu()
	{
		if (Entity != TlsConnectionEnd.Server || CertificatePolicy == TlsCertificatePolicy.NoClientCertificate || false || (AcceptableAuthorities != null && 0 == 0 && ((AcceptableAuthorities.Length != 0) ? true : false)))
		{
			return;
		}
		throw new InvalidOperationException("Certificate policy set to request client certificate, but no accepted certificate authorities were specified.");
	}

	internal void wnzfz()
	{
		if (Entity != TlsConnectionEnd.Server || bkuuh == null)
		{
			return;
		}
		CertificateChain[] array = bkuuh;
		int num = 0;
		if (num != 0)
		{
			goto IL_002a;
		}
		goto IL_0077;
		IL_0077:
		if (num >= array.Length)
		{
			return;
		}
		goto IL_002a;
		IL_002a:
		CertificateChain certificateChain = array[num];
		if (certificateChain != null && 0 == 0 && certificateChain.LeafCertificate != null && 0 == 0 && (!certificateChain.LeafCertificate.HasPrivateKey() || 1 == 0))
		{
			throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Specified server certificate has no associated private key.", null);
		}
		num++;
		goto IL_0077;
	}

	internal void ylflp(nxtme<CertificateChain> p0)
	{
		if (Entity == TlsConnectionEnd.Server)
		{
			if (bqrse == null || 1 == 0)
			{
				bqrse = eoecc;
			}
			xolbj(p0, bqrse);
		}
	}

	internal static void xolbj(nxtme<CertificateChain> p0, Func<string, Exception> p1)
	{
		if (p0.hvbtp && 0 == 0)
		{
			throw p1("Server certificate was not specified.");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0028;
		}
		goto IL_00ff;
		IL_00ff:
		if (num >= p0.tvoem)
		{
			return;
		}
		goto IL_0028;
		IL_0028:
		CertificateChain certificateChain = p0[num];
		if (certificateChain == null || 1 == 0)
		{
			throw p1(brgjd.edcru("Server certificate at index {0} is null.", num));
		}
		if (ruabh == null || 1 == 0)
		{
			ruabh = odlgu;
		}
		if (certificateChain.Any(ruabh) && 0 == 0)
		{
			throw p1(brgjd.edcru("Server certificate at index {0} is invalid.", num));
		}
		if (!certificateChain.LeafCertificate.HasPrivateKey() || 1 == 0)
		{
			throw p1(brgjd.edcru("Server certificate at index {0} has no associated private key.", num));
		}
		num++;
		goto IL_00ff;
	}

	internal void vplfc(smtii p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("clientHelloInfo");
		}
		if (jhxwm == null || false || object.ReferenceEquals(jhxwm, ishzk.hwcpz))
		{
			p0.vubgy = p0.ohkim;
		}
		else
		{
			mfxqd.enhzj(p0);
		}
	}

	internal webdt auypu()
	{
		if ((vlqhl & TlsOptions.DoNotCacheSessions) != TlsOptions.None)
		{
			return null;
		}
		webdt edema = qnpvc;
		if (edema == null || 1 == 0)
		{
			edema = sicek.edema;
		}
		return edema;
	}

	private static Exception eoecc(string p0)
	{
		return new TlsException(rtzwv.iogyt, mjddr.jhrgr, p0, null);
	}

	private static bool odlgu(Certificate p0)
	{
		return p0 == null;
	}
}
