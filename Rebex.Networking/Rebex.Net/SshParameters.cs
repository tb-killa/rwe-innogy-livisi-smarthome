using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Net;

public class SshParameters : gnyvx
{
	private sealed class gehfd
	{
		public List<string> slwpb;

		public bool nsktv;

		public bool fgkhx;

		public bool kwktd;

		public bool nlrsh;

		public bool gjijz;

		public bool afbmo;

		public bool mljhx;

		public bool xftkd;

		public SshParameters onebi;

		public void qsedr()
		{
			if (afbmo && 0 == 0)
			{
				if (nsktv && 0 == 0)
				{
					slwpb.AddRange(new string[3] { "aes256-ctr", "aes192-ctr", "aes128-ctr" });
				}
				if (fgkhx && 0 == 0)
				{
					slwpb.AddRange(new string[1] { "3des-ctr" });
				}
				if (kwktd && 0 == 0)
				{
					slwpb.AddRange(new string[3] { "twofish256-ctr", "twofish192-ctr", "twofish128-ctr" });
				}
				if (nlrsh && 0 == 0)
				{
					slwpb.AddRange(new string[1] { "blowfish-ctr" });
				}
			}
		}

		public void kqkfp()
		{
			if (xftkd && 0 == 0 && nsktv && 0 == 0)
			{
				slwpb.AddRange(new string[2] { "aes256-gcm@openssh.com", "aes128-gcm@openssh.com" });
			}
		}

		public void beiyw()
		{
			if (mljhx && 0 == 0)
			{
				if (nsktv && 0 == 0)
				{
					slwpb.AddRange(new string[3] { "aes256-cbc", "aes192-cbc", "aes128-cbc" });
				}
				if (fgkhx && 0 == 0)
				{
					slwpb.AddRange(new string[1] { "3des-cbc" });
				}
				if (kwktd && 0 == 0)
				{
					slwpb.AddRange(new string[4] { "twofish256-cbc", "twofish192-cbc", "twofish128-cbc", "twofish-cbc" });
				}
				if (nlrsh && 0 == 0)
				{
					slwpb.AddRange(new string[1] { "blowfish-cbc" });
				}
			}
		}

		public void vmpft()
		{
			if (gjijz && 0 == 0)
			{
				slwpb.Add("arcfour256");
				slwpb.Add("arcfour128");
				if (onebi.hecgb != null && 0 == 0)
				{
					slwpb.Add("arcfour");
				}
			}
		}
	}

	private const SshEncryptionAlgorithm optfj = SshEncryptionAlgorithm.TripleDES | SshEncryptionAlgorithm.AES | SshEncryptionAlgorithm.Twofish | SshEncryptionAlgorithm.Chacha20Poly1305;

	private const SshHostKeyAlgorithm mcdrp = SshHostKeyAlgorithm.RSA | SshHostKeyAlgorithm.DSS | SshHostKeyAlgorithm.Certificate | SshHostKeyAlgorithm.ED25519 | SshHostKeyAlgorithm.ECDsaNistP256;

	private const SshKeyExchangeAlgorithm cchwd = SshKeyExchangeAlgorithm.DiffieHellmanGroup1SHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroup14SHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA256 | SshKeyExchangeAlgorithm.ECDiffieHellmanNistP256 | SshKeyExchangeAlgorithm.Curve25519 | SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA256;

	private const SshMacAlgorithm lqryo = SshMacAlgorithm.SHA1 | SshMacAlgorithm.SHA256 | SshMacAlgorithm.SHA512;

	private const string uukks = "ExtensionNegotiation";

	private const string fpkyo = "SoftwareVersion";

	private bool zmrol;

	private SshMacAlgorithm jvdvq = SshMacAlgorithm.SHA1 | SshMacAlgorithm.SHA256 | SshMacAlgorithm.SHA512;

	private string[] nuqgy;

	private SshEncryptionAlgorithm pxdyd = SshEncryptionAlgorithm.TripleDES | SshEncryptionAlgorithm.AES | SshEncryptionAlgorithm.Twofish | SshEncryptionAlgorithm.Chacha20Poly1305;

	private string[] hecgb;

	private SshKeyExchangeAlgorithm kioir = SshKeyExchangeAlgorithm.DiffieHellmanGroup1SHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroup14SHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA256 | SshKeyExchangeAlgorithm.ECDiffieHellmanNistP256 | SshKeyExchangeAlgorithm.Curve25519 | SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA256;

	private string[] tgorw;

	private SshHostKeyAlgorithm lowqm = SshHostKeyAlgorithm.RSA | SshHostKeyAlgorithm.DSS | SshHostKeyAlgorithm.Certificate | SshHostKeyAlgorithm.ED25519 | SshHostKeyAlgorithm.ECDsaNistP256;

	private string[] fyfve;

	private SshHostKeyAlgorithm ytcvy = SshHostKeyAlgorithm.RSA;

	private bool ehtaq = true;

	private bool hwidk;

	private int zfwom = 5;

	private SshEncryptionMode ryjrs = SshEncryptionMode.Any;

	private SshAuthenticationMethod tpsdf = SshAuthenticationMethod.Any;

	private int ocjhm = 1024;

	private int lbzjk = 1023;

	private bool? rusii;

	private bool qeeeu;

	private string yybtv;

	private int cvejt = 129024;

	internal bool wqpgg => zmrol;

	[DefaultValue(SshMacAlgorithm.SHA1 | SshMacAlgorithm.SHA256 | SshMacAlgorithm.SHA512)]
	public SshMacAlgorithm MacAlgorithms
	{
		get
		{
			return jvdvq;
		}
		set
		{
			ofafk();
			if (value == SshMacAlgorithm.None)
			{
				throw new ArgumentException("Missing algorithm.", "value");
			}
			jvdvq = value;
		}
	}

	[DefaultValue(SshEncryptionAlgorithm.TripleDES | SshEncryptionAlgorithm.AES | SshEncryptionAlgorithm.Twofish | SshEncryptionAlgorithm.Chacha20Poly1305)]
	public SshEncryptionAlgorithm EncryptionAlgorithms
	{
		get
		{
			return pxdyd;
		}
		set
		{
			ofafk();
			if (value == SshEncryptionAlgorithm.None)
			{
				throw new ArgumentException("Missing algorithm.", "value");
			}
			pxdyd = value;
		}
	}

	public SshEncryptionMode EncryptionModes
	{
		get
		{
			return ryjrs;
		}
		set
		{
			ofafk();
			if (value == SshEncryptionMode.None || 1 == 0)
			{
				throw new ArgumentException("Missing algorithm.", "value");
			}
			ryjrs = value;
		}
	}

	public SshAuthenticationMethod AuthenticationMethods
	{
		get
		{
			return tpsdf;
		}
		set
		{
			ofafk();
			if (value == SshAuthenticationMethod.None || 1 == 0)
			{
				throw new ArgumentException("Missing algorithm.", "value");
			}
			tpsdf = value;
		}
	}

	[DefaultValue(SshKeyExchangeAlgorithm.DiffieHellmanGroup1SHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroup14SHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA1 | SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA256 | SshKeyExchangeAlgorithm.ECDiffieHellmanNistP256 | SshKeyExchangeAlgorithm.Curve25519 | SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA256)]
	public SshKeyExchangeAlgorithm KeyExchangeAlgorithms
	{
		get
		{
			return kioir;
		}
		set
		{
			ofafk();
			if (value == SshKeyExchangeAlgorithm.None || 1 == 0)
			{
				throw new ArgumentException("Missing algorithm.", "value");
			}
			kioir = value;
		}
	}

	[DefaultValue(SshHostKeyAlgorithm.RSA | SshHostKeyAlgorithm.DSS | SshHostKeyAlgorithm.Certificate | SshHostKeyAlgorithm.ED25519 | SshHostKeyAlgorithm.ECDsaNistP256)]
	public SshHostKeyAlgorithm HostKeyAlgorithms
	{
		get
		{
			return lowqm;
		}
		set
		{
			ofafk();
			if (value == SshHostKeyAlgorithm.None || 1 == 0)
			{
				throw new ArgumentException("Missing algorithm.", "value");
			}
			lowqm = value;
		}
	}

	public SshHostKeyAlgorithm PreferredHostKeyAlgorithm
	{
		get
		{
			return ytcvy;
		}
		set
		{
			ofafk();
			switch (value)
			{
			case SshHostKeyAlgorithm.None:
				throw new ArgumentException("Missing algorithm.", "value");
			default:
				throw new ArgumentException("Unsupported host key algorithm.", "value");
			case SshHostKeyAlgorithm.RSA:
			case SshHostKeyAlgorithm.DSS:
			case SshHostKeyAlgorithm.Certificate:
			case SshHostKeyAlgorithm.ED25519:
			case SshHostKeyAlgorithm.ECDsaNistP256:
			case SshHostKeyAlgorithm.ECDsaNistP384:
			case SshHostKeyAlgorithm.ECDsaNistP521:
				ytcvy = value;
				break;
			}
		}
	}

	public bool EnabledHostKeyAlgorithmsWithSha2
	{
		get
		{
			return ehtaq;
		}
		set
		{
			ehtaq = value;
		}
	}

	public int MinimumDiffieHellmanKeySize
	{
		get
		{
			return ocjhm;
		}
		set
		{
			if (value < 512 || value > 16384)
			{
				throw hifyx.nztrs("value", value, "Unsupported key size.");
			}
			ocjhm = value;
		}
	}

	public int MinimumRsaKeySize
	{
		get
		{
			return lbzjk;
		}
		set
		{
			if (value < 512 || value > 16384)
			{
				throw hifyx.nztrs("value", value, "Unsupported key size.");
			}
			lbzjk = value;
		}
	}

	public bool Compression
	{
		get
		{
			return hwidk;
		}
		set
		{
			hwidk = value;
		}
	}

	public int CompressionLevel
	{
		get
		{
			return zfwom;
		}
		set
		{
			if (value < 0 || value > 9)
			{
				throw hifyx.nztrs("value", value, "Invalid compression level. Possible values are 0-9.");
			}
			zfwom = value;
		}
	}

	public bool? UseLegacyGroupExchange
	{
		get
		{
			return rusii;
		}
		set
		{
			rusii = value;
		}
	}

	public int MaximumPacketSize
	{
		get
		{
			return cvejt;
		}
		set
		{
			if (value < 4096 || value > 131072)
			{
				throw hifyx.nztrs("value", value, "Argument is out of range of valid values.");
			}
			cvejt = value;
		}
	}

	public SshParameters Clone()
	{
		SshParameters sshParameters = new SshParameters();
		sshParameters.jvdvq = jvdvq;
		sshParameters.nuqgy = nuqgy;
		sshParameters.pxdyd = pxdyd;
		sshParameters.hecgb = hecgb;
		sshParameters.kioir = kioir;
		sshParameters.tgorw = tgorw;
		sshParameters.lowqm = lowqm;
		sshParameters.fyfve = fyfve;
		sshParameters.ytcvy = ytcvy;
		sshParameters.ehtaq = ehtaq;
		sshParameters.hwidk = hwidk;
		sshParameters.zfwom = zfwom;
		sshParameters.ryjrs = ryjrs;
		sshParameters.tpsdf = tpsdf;
		sshParameters.ocjhm = ocjhm;
		sshParameters.lbzjk = lbzjk;
		sshParameters.qeeeu = qeeeu;
		sshParameters.yybtv = yybtv;
		sshParameters.rusii = rusii;
		sshParameters.cvejt = cvejt;
		return sshParameters;
	}

	internal SshParameters saltn()
	{
		SshParameters sshParameters = Clone();
		sshParameters.zmrol = true;
		return sshParameters;
	}

	private void ofafk()
	{
		if (zmrol && 0 == 0)
		{
			throw new InvalidOperationException("Cannot change parameters that are being used for a session.");
		}
	}

	internal static bool kwmql(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (awprl.qhlis == null || 1 == 0)
			{
				awprl.qhlis = new Dictionary<string, int>(8)
				{
					{ "hmac-md5", 0 },
					{ "hmac-md5-96", 1 },
					{ "hmac-sha1-96", 2 },
					{ "diffie-hellman-group1-sha1", 3 },
					{ "arcfour256", 4 },
					{ "arcfour128", 5 },
					{ "arcfour", 6 },
					{ "blowfish-cbc", 7 }
				};
			}
			if (awprl.qhlis.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					return true;
				}
			}
		}
		return false;
	}

	internal static bool wlxtp(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (awprl.kfriy == null || 1 == 0)
			{
				awprl.kfriy = new Dictionary<string, int>(8)
				{
					{ "curve25519-sha256", 0 },
					{ "curve25519-sha256@libssh.org", 1 },
					{ "ecdh-sha2-nistp256", 2 },
					{ "ecdh-sha2-nistp384", 3 },
					{ "ecdh-sha2-nistp521", 4 },
					{ "ecdsa-sha2-nistp256", 5 },
					{ "ecdsa-sha2-nistp384", 6 },
					{ "ecdsa-sha2-nistp521", 7 }
				};
			}
			if (awprl.kfriy.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					return true;
				}
			}
		}
		return false;
	}

	public string[] GetMacAlgorithms()
	{
		if (nuqgy == null || 1 == 0)
		{
			return null;
		}
		return (string[])nuqgy.Clone();
	}

	public void SetMacAlgorithms(params string[] algorithmIds)
	{
		ofafk();
		if (algorithmIds == null || false || algorithmIds.Length == 0 || 1 == 0)
		{
			nuqgy = null;
		}
		else
		{
			nuqgy = (string[])algorithmIds.Clone();
		}
	}

	public static string[] GetSupportedMacAlgorithms()
	{
		SshParameters sshParameters = new SshParameters();
		sshParameters.MacAlgorithms = SshMacAlgorithm.Any;
		return sshParameters.woyky(null, p1: true);
	}

	internal string[] eolzs(string p0)
	{
		return woyky(p0, nuqgy != null);
	}

	private string[] woyky(string p0, bool p1)
	{
		string text = p0;
		if (text == null || 1 == 0)
		{
			text = string.Empty;
		}
		p0 = text;
		List<string> list = new List<string>();
		bool flag = p0.IndexOf("OpenSSH_6.6", StringComparison.Ordinal) >= 0;
		if (ijiif(SshMacAlgorithm.SHA256) && 0 == 0 && HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA256) && 0 == 0)
		{
			if (!flag || 1 == 0)
			{
				list.Add("hmac-sha2-256-etm@openssh.com");
			}
			list.Add("hmac-sha2-256");
		}
		if (ijiif(SshMacAlgorithm.SHA512) && 0 == 0 && HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA512) && 0 == 0)
		{
			if (!flag || 1 == 0)
			{
				list.Add("hmac-sha2-512-etm@openssh.com");
			}
			list.Add("hmac-sha2-512");
		}
		if (ijiif(SshMacAlgorithm.SHA1) && 0 == 0)
		{
			list.Add("hmac-sha1");
		}
		if (!CryptoHelper.UseFipsAlgorithmsOnly || 1 == 0)
		{
			if (ijiif(SshMacAlgorithm.MD5) && 0 == 0)
			{
				list.Add("hmac-md5");
			}
			if (p1 && 0 == 0)
			{
				if (ijiif(SshMacAlgorithm.SHA1) && 0 == 0)
				{
					list.Add("hmac-sha1-96");
				}
				if (ijiif(SshMacAlgorithm.MD5) && 0 == 0)
				{
					list.Add("hmac-md5-96");
				}
			}
		}
		list = dvgpb(list, nuqgy);
		return list.ToArray();
	}

	private bool ijiif(SshMacAlgorithm p0)
	{
		return (jvdvq & p0) == p0;
	}

	internal string[] xczlj()
	{
		if (!hwidk || 1 == 0)
		{
			return new string[3] { "none", "zlib", "zlib@openssh.com" };
		}
		return new string[3] { "zlib", "zlib@openssh.com", "none" };
	}

	public string[] GetEncryptionAlgorithms()
	{
		if (hecgb == null || 1 == 0)
		{
			return null;
		}
		return (string[])hecgb.Clone();
	}

	public void SetEncryptionAlgorithms(params string[] algorithmIds)
	{
		ofafk();
		if (algorithmIds == null || false || algorithmIds.Length == 0 || 1 == 0)
		{
			hecgb = null;
		}
		else
		{
			hecgb = (string[])algorithmIds.Clone();
		}
	}

	public static string[] GetSupportedEncryptionAlgorithms()
	{
		SshParameters sshParameters = new SshParameters();
		sshParameters.EncryptionAlgorithms = SshEncryptionAlgorithm.Any;
		sshParameters.EncryptionModes = SshEncryptionMode.Any;
		return sshParameters.yhshu(null);
	}

	internal string[] yhshu(string p0)
	{
		gehfd gehfd = new gehfd();
		gehfd.onebi = this;
		string text = p0;
		if (text == null || 1 == 0)
		{
			text = string.Empty;
		}
		p0 = text;
		gehfd.slwpb = new List<string>();
		gehfd.nsktv = pyzay(SshEncryptionAlgorithm.AES) && 0 == 0 && SymmetricKeyAlgorithm.IsSupported(SymmetricKeyAlgorithmId.AES);
		gehfd.fgkhx = pyzay(SshEncryptionAlgorithm.TripleDES) && 0 == 0 && SymmetricKeyAlgorithm.IsSupported(SymmetricKeyAlgorithmId.TripleDES);
		gehfd.kwktd = pyzay(SshEncryptionAlgorithm.Twofish) && 0 == 0 && SymmetricKeyAlgorithm.IsSupported(SymmetricKeyAlgorithmId.Twofish);
		gehfd.nlrsh = pyzay(SshEncryptionAlgorithm.Blowfish) && 0 == 0 && SymmetricKeyAlgorithm.IsSupported(SymmetricKeyAlgorithmId.Blowfish);
		gehfd.gjijz = pyzay(SshEncryptionAlgorithm.RC4) && 0 == 0 && SymmetricKeyAlgorithm.IsSupported(SymmetricKeyAlgorithmId.ArcFour);
		gehfd.afbmo = osfff(SshEncryptionMode.CTR) && 0 == 0 && (!CryptoHelper.UseFipsAlgorithmsOnly || CryptoHelper.ibqug);
		gehfd.mljhx = osfff(SshEncryptionMode.CBC);
		bool flag = osfff(SshEncryptionMode.GCM);
		gehfd.xftkd = flag && 0 == 0 && ovpxz.lsjck();
		bool flag2 = flag && 0 == 0 && pyzay(SshEncryptionAlgorithm.Chacha20Poly1305) && !CryptoHelper.UseFipsAlgorithmsOnly;
		Action action = gehfd.qsedr;
		Action action2 = gehfd.kqkfp;
		Action action3 = gehfd.beiyw;
		Action action4 = gehfd.vmpft;
		if (p0.IndexOf("OpenSSH_4", StringComparison.Ordinal) >= 0 && 0 == 0)
		{
			action3();
			action();
			action2();
		}
		else if ((p0.IndexOf("OpenSSH_6.2", StringComparison.Ordinal) >= 0 || p0.IndexOf("OpenSSH_6.3", StringComparison.Ordinal) >= 0) && 0 == 0)
		{
			action();
			action3();
			action2();
		}
		else
		{
			action2();
			action();
			action3();
		}
		if (flag2 && 0 == 0)
		{
			gehfd.slwpb.Add("chacha20-poly1305@openssh.com");
		}
		action4();
		gehfd.slwpb = dvgpb(gehfd.slwpb, hecgb);
		return gehfd.slwpb.ToArray();
	}

	private bool pyzay(SshEncryptionAlgorithm p0)
	{
		return (pxdyd & p0) == p0;
	}

	private bool osfff(SshEncryptionMode p0)
	{
		return (ryjrs & p0) == p0;
	}

	public string[] GetKeyExchangeAlgorithms()
	{
		if (tgorw == null || 1 == 0)
		{
			return null;
		}
		return (string[])tgorw.Clone();
	}

	public void SetKeyExchangeAlgorithms(params string[] algorithmIds)
	{
		ofafk();
		if (algorithmIds == null || false || algorithmIds.Length == 0 || 1 == 0)
		{
			tgorw = null;
		}
		else
		{
			tgorw = (string[])algorithmIds.Clone();
		}
	}

	private static bool flgkk(int p0, int p1, int p2)
	{
		if (p0 <= p1)
		{
			return p1 <= p2;
		}
		return false;
	}

	public static string[] GetSupportedKeyExchangeAlgorithms()
	{
		SshParameters sshParameters = new SshParameters();
		sshParameters.KeyExchangeAlgorithms = SshKeyExchangeAlgorithm.Any;
		return sshParameters.shgsc(p0: true);
	}

	internal string[] shgsc(bool p0)
	{
		return tmfuq(p0, null);
	}

	internal string[] khess()
	{
		return tmfuq(p0: true, (qeeeu ? true : false) ? null : "ext-info-c");
	}

	private string[] tmfuq(bool p0, string p1)
	{
		List<string> list = new List<string>();
		bool p2 = false;
		bool p3 = false;
		bool p4 = false;
		bool flag = HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA256);
		bool flag2 = HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA384);
		bool flag3 = HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA512);
		int p5 = ocjhm;
		int num = 2048;
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.DiffieHellman, null, 8192) && 0 == 0)
		{
			num = 8192;
			if (num != 0)
			{
				goto IL_00ba;
			}
		}
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.DiffieHellman, null, 6144) && 0 == 0)
		{
			num = 6144;
			if (num != 0)
			{
				goto IL_00ba;
			}
		}
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.DiffieHellman, null, 4096) && 0 == 0)
		{
			num = 4096;
			if (num != 0)
			{
				goto IL_00ba;
			}
		}
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.DiffieHellman, null, 3072) && 0 == 0)
		{
			num = 3072;
		}
		goto IL_00ba;
		IL_00ba:
		if (flag && 0 == 0)
		{
			if (tmxwx(SshKeyExchangeAlgorithm.Curve25519) && 0 == 0 && AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.ECDH, "1.3.101.110", 0) && 0 == 0)
			{
				list.Add("curve25519-sha256");
				list.Add("curve25519-sha256@libssh.org");
			}
			if (tmxwx(SshKeyExchangeAlgorithm.ECDiffieHellmanNistP256) && 0 == 0 && jwtcd(AsymmetricKeyAlgorithmId.ECDH, "1.2.840.10045.3.1.7", 0, out p2) && 0 == 0)
			{
				list.Add("ecdh-sha2-nistp256");
			}
		}
		if (flag2 && 0 == 0 && tmxwx(SshKeyExchangeAlgorithm.ECDiffieHellmanNistP384) && 0 == 0 && jwtcd(AsymmetricKeyAlgorithmId.ECDH, "1.3.132.0.34", 0, out p3) && 0 == 0)
		{
			list.Add("ecdh-sha2-nistp384");
		}
		if (flag3 && 0 == 0 && tmxwx(SshKeyExchangeAlgorithm.ECDiffieHellmanNistP521) && 0 == 0 && jwtcd(AsymmetricKeyAlgorithmId.ECDH, "1.3.132.0.35", 0, out p4) && 0 == 0)
		{
			list.Add("ecdh-sha2-nistp521");
		}
		if (flag && 0 == 0)
		{
			if (tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA256) && 0 == 0)
			{
				list.Add("diffie-hellman-group-exchange-sha256");
			}
			if (tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA256) && 0 == 0 && flgkk(p5, 2048, num) && 0 == 0)
			{
				list.Add("diffie-hellman-group14-sha256");
			}
		}
		if (flag3 && 0 == 0)
		{
			if (tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512) && 0 == 0 && flgkk(p5, 3072, num) && 0 == 0)
			{
				list.Add("diffie-hellman-group15-sha512");
			}
			if (tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512) && 0 == 0 && flgkk(p5, 4096, num) && 0 == 0)
			{
				list.Add("diffie-hellman-group16-sha512");
			}
			if (flag3 && 0 == 0 && tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512) && 0 == 0 && flgkk(p5, 6144, num) && 0 == 0)
			{
				list.Add("diffie-hellman-group17-sha512");
			}
			if (flag3 && 0 == 0 && tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512) && 0 == 0 && flgkk(p5, 8192, num) && 0 == 0)
			{
				list.Add("diffie-hellman-group18-sha512");
			}
		}
		if (tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA1) && 0 == 0)
		{
			list.Add("diffie-hellman-group-exchange-sha1");
		}
		if (tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanGroup14SHA1) && 0 == 0 && flgkk(p5, 2048, num) && 0 == 0)
		{
			list.Add("diffie-hellman-group14-sha1");
		}
		if (p0 && 0 == 0)
		{
			if (p2 && 0 == 0)
			{
				list.Add("ecdh-sha2-nistp256");
			}
			if (p3 && 0 == 0)
			{
				list.Add("ecdh-sha2-nistp384");
			}
			if (p4 && 0 == 0)
			{
				list.Add("ecdh-sha2-nistp521");
			}
		}
		if (tmxwx(SshKeyExchangeAlgorithm.DiffieHellmanGroup1SHA1) && 0 == 0 && flgkk(p5, 1024, num) && 0 == 0)
		{
			list.Add("diffie-hellman-group1-sha1");
		}
		list = dvgpb(list, tgorw);
		if (p1 != null && 0 == 0)
		{
			list.Add(p1);
		}
		return list.ToArray();
	}

	private static bool jwtcd(AsymmetricKeyAlgorithmId p0, string p1, int p2, out bool p3)
	{
		switch (AsymmetricKeyAlgorithm.iexxf(p0, p1, p2))
		{
		case zxjln.iuckt:
			p3 = false;
			return true;
		case zxjln.dwzpe:
			p3 = true;
			return false;
		default:
			p3 = false;
			return false;
		}
	}

	private static List<string> dvgpb(List<string> p0, string[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			return p0;
		}
		List<string> list = new List<string>();
		int num = 0;
		if (num != 0)
		{
			goto IL_001a;
		}
		goto IL_003a;
		IL_003a:
		if (num < p1.Length)
		{
			goto IL_001a;
		}
		return list;
		IL_001a:
		string item = p1[num];
		if (p0.Contains(item) && 0 == 0)
		{
			list.Add(item);
		}
		num++;
		goto IL_003a;
	}

	private bool tmxwx(SshKeyExchangeAlgorithm p0)
	{
		return (kioir & p0) == p0;
	}

	public string[] GetHostKeyAlgorithms()
	{
		if (fyfve == null || 1 == 0)
		{
			return null;
		}
		return (string[])fyfve.Clone();
	}

	public void SetHostKeyAlgorithms(params string[] algorithmIds)
	{
		ofafk();
		if (algorithmIds == null || false || algorithmIds.Length == 0 || 1 == 0)
		{
			fyfve = null;
		}
		else
		{
			fyfve = (string[])algorithmIds.Clone();
		}
	}

	internal string[] chqyr(SshHostKeyAlgorithm p0, SshHostKeyAlgorithm p1, int p2 = 4096)
	{
		bool p3 = ehtaq;
		List<hcqmh<string, SshHostKeyAlgorithm>> list = diwip(p3, p2);
		List<string> list2 = new List<string>();
		int num = 0;
		using (List<hcqmh<string, SshHostKeyAlgorithm>>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				hcqmh<string, SshHostKeyAlgorithm> current = enumerator.Current;
				string amanf = current.amanf;
				SshHostKeyAlgorithm sshHostKeyAlgorithm = current.cdois;
				if ((sshHostKeyAlgorithm & SshHostKeyAlgorithm.Certificate) != SshHostKeyAlgorithm.None && 0 == 0)
				{
					sshHostKeyAlgorithm &= ~SshHostKeyAlgorithm.Certificate;
					if ((p1 & sshHostKeyAlgorithm) != sshHostKeyAlgorithm)
					{
						continue;
					}
					sshHostKeyAlgorithm = SshHostKeyAlgorithm.Certificate;
					if (sshHostKeyAlgorithm != SshHostKeyAlgorithm.None)
					{
						goto IL_006f;
					}
				}
				if ((p0 & sshHostKeyAlgorithm) != sshHostKeyAlgorithm)
				{
					continue;
				}
				goto IL_006f;
				IL_006f:
				if (fcamo(sshHostKeyAlgorithm) && 0 == 0)
				{
					if (sshHostKeyAlgorithm == ytcvy)
					{
						list2.Insert(num, amanf);
						num++;
					}
					else
					{
						list2.Add(amanf);
					}
				}
			}
		}
		list2 = dvgpb(list2, fyfve);
		return list2.ToArray();
	}

	public static string[] GetSupportedHostKeyAlgorithms()
	{
		List<hcqmh<string, SshHostKeyAlgorithm>> list = diwip(p0: true);
		List<string> list2 = new List<string>();
		using (List<hcqmh<string, SshHostKeyAlgorithm>>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				hcqmh<string, SshHostKeyAlgorithm> current = enumerator.Current;
				string amanf = current.amanf;
				list2.Add(amanf);
			}
		}
		return list2.ToArray();
	}

	private static List<hcqmh<string, SshHostKeyAlgorithm>> diwip(bool p0, int p1 = 4096)
	{
		bool flag = (p0 ? true : false) && HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA256);
		if (p0 && 0 == 0)
		{
			HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA384);
		}
		bool flag2 = (p0 ? true : false) && HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA512);
		List<hcqmh<string, SshHostKeyAlgorithm>> list = new List<hcqmh<string, SshHostKeyAlgorithm>>();
		list.Add(new hcqmh<string, SshHostKeyAlgorithm>("ssh-dss", SshHostKeyAlgorithm.DSS));
		if (flag && 0 == 0)
		{
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("rsa-sha2-256", SshHostKeyAlgorithm.RSA));
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("ssh-rsa-sha256@ssh.com", SshHostKeyAlgorithm.RSA));
		}
		if (flag2 && 0 == 0)
		{
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("rsa-sha2-512", SshHostKeyAlgorithm.RSA));
		}
		list.Add(new hcqmh<string, SshHostKeyAlgorithm>("ssh-rsa", SshHostKeyAlgorithm.RSA));
		if (flag && 0 == 0)
		{
			if (p1 >= 2048)
			{
				list.Add(new hcqmh<string, SshHostKeyAlgorithm>("x509v3-rsa2048-sha256", SshHostKeyAlgorithm.RSA | SshHostKeyAlgorithm.Certificate));
			}
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("x509v3-sign-rsa-sha256@ssh.com", SshHostKeyAlgorithm.RSA | SshHostKeyAlgorithm.Certificate));
		}
		list.Add(new hcqmh<string, SshHostKeyAlgorithm>("x509v3-sign-rsa", SshHostKeyAlgorithm.RSA | SshHostKeyAlgorithm.Certificate));
		list.Add(new hcqmh<string, SshHostKeyAlgorithm>("x509v3-sign-dss", SshHostKeyAlgorithm.DSS | SshHostKeyAlgorithm.Certificate));
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.EdDsa, "1.3.101.112", 0) && 0 == 0)
		{
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("ssh-ed25519", SshHostKeyAlgorithm.ED25519));
		}
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.ECDsa, "1.2.840.10045.3.1.7", 0) && 0 == 0)
		{
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("ecdsa-sha2-nistp256", SshHostKeyAlgorithm.ECDsaNistP256));
		}
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.ECDsa, "1.3.132.0.34", 0) && 0 == 0)
		{
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("ecdsa-sha2-nistp384", SshHostKeyAlgorithm.ECDsaNistP384));
		}
		if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.ECDsa, "1.3.132.0.35", 0) && 0 == 0)
		{
			list.Add(new hcqmh<string, SshHostKeyAlgorithm>("ecdsa-sha2-nistp521", SshHostKeyAlgorithm.ECDsaNistP521));
		}
		return list;
	}

	private bool fcamo(SshHostKeyAlgorithm p0)
	{
		return (lowqm & p0) == p0;
	}

	internal string jwait(SshPrivateKey p0, string[] p1)
	{
		SshHostKeyAlgorithm sshHostKeyAlgorithm = p0.KeyAlgorithm;
		SshHostKeyAlgorithm sshHostKeyAlgorithm2 = SshHostKeyAlgorithm.None;
		Certificate certificate = p0.Certificate;
		int p2 = 0;
		if (certificate != null && 0 == 0)
		{
			sshHostKeyAlgorithm = SshHostKeyAlgorithm.Certificate;
			switch (certificate.KeyAlgorithm)
			{
			case KeyAlgorithm.DSA:
				sshHostKeyAlgorithm2 = SshHostKeyAlgorithm.DSS;
				if (sshHostKeyAlgorithm2 != SshHostKeyAlgorithm.None)
				{
					break;
				}
				goto case KeyAlgorithm.RSA;
			case KeyAlgorithm.RSA:
				sshHostKeyAlgorithm2 = SshHostKeyAlgorithm.RSA;
				p2 = certificate.GetKeySize();
				break;
			default:
				return null;
			}
		}
		List<string> p3 = new List<string>(chqyr(sshHostKeyAlgorithm, sshHostKeyAlgorithm2, p2));
		p3 = dvgpb(p3, p1);
		using (List<string>.Enumerator enumerator = p3.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				string current = enumerator.Current;
				if (!(current == "rsa-sha2-512") || false || p0.KeySize >= 768)
				{
					return current;
				}
			}
		}
		return sshHostKeyAlgorithm switch
		{
			SshHostKeyAlgorithm.RSA => "ssh-rsa", 
			SshHostKeyAlgorithm.DSS => "ssh-dss", 
			SshHostKeyAlgorithm.ED25519 => "ssh-ed25519", 
			SshHostKeyAlgorithm.ECDsaNistP256 => "ecdsa-sha2-nistp256", 
			SshHostKeyAlgorithm.ECDsaNistP384 => "ecdsa-sha2-nistp384", 
			SshHostKeyAlgorithm.ECDsaNistP521 => "ecdsa-sha2-nistp521", 
			SshHostKeyAlgorithm.Certificate => sshHostKeyAlgorithm2 switch
			{
				SshHostKeyAlgorithm.RSA => "x509v3-sign-rsa", 
				SshHostKeyAlgorithm.DSS => "x509v3-sign-dss", 
				_ => null, 
			}, 
			_ => null, 
		};
	}

	internal bool srcjt(string p0, SshServerInfo p1)
	{
		bool? flag = rusii;
		bool flag2;
		if (flag.HasValue)
		{
			flag2 = flag.Value;
			goto IL_0183;
		}
		flag2 = true;
		if (p0.IndexOf("Rebex", StringComparison.Ordinal) >= 0)
		{
			flag2 = false;
			if (!flag2)
			{
				goto IL_0099;
			}
		}
		if (p0.IndexOf("Cisco-", StringComparison.Ordinal) >= 0)
		{
			flag2 = false;
			if (!flag2)
			{
				goto IL_0099;
			}
		}
		if (p0.IndexOf("GlobalSCAPE", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			flag2 = false;
			if (!flag2)
			{
				goto IL_0099;
			}
		}
		if (p0.Length >= 9)
		{
			int num = p0.IndexOf("OpenSSH_", StringComparison.Ordinal);
			if (num >= 0)
			{
				int num2 = p0[num + 8] - 48;
				if (num2 < 2 || num2 > 4)
				{
					flag2 = false;
				}
			}
		}
		goto IL_0099;
		IL_0138:
		int num3;
		string[] serverHostKeyAlgorithms;
		if (num3 < serverHostKeyAlgorithms.Length)
		{
			goto IL_0116;
		}
		string[] encryptionAlgorithmsClientToServer = p1.EncryptionAlgorithmsClientToServer;
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_014f;
		}
		goto IL_0171;
		IL_0171:
		if (num4 >= encryptionAlgorithmsClientToServer.Length)
		{
			goto IL_0183;
		}
		goto IL_014f;
		IL_0099:
		string[] keyExchangeAlgorithms;
		int num5;
		if (flag2 && 0 == 0)
		{
			bool flag3 = false;
			if (p0.IndexOf("Cleo VLProxy", StringComparison.Ordinal) >= 0)
			{
				flag3 = true;
			}
			if (!flag3 || 1 == 0)
			{
				keyExchangeAlgorithms = p1.KeyExchangeAlgorithms;
				num5 = 0;
				if (num5 != 0)
				{
					goto IL_00da;
				}
				goto IL_00ff;
			}
		}
		goto IL_0183;
		IL_00da:
		string text = keyExchangeAlgorithms[num5];
		if (text.IndexOf("sha2", StringComparison.Ordinal) >= 0)
		{
			return false;
		}
		num5++;
		goto IL_00ff;
		IL_014f:
		string text2 = encryptionAlgorithmsClientToServer[num4];
		if (text2.IndexOf("-gcm", StringComparison.Ordinal) >= 0)
		{
			return false;
		}
		num4++;
		goto IL_0171;
		IL_00ff:
		if (num5 < keyExchangeAlgorithms.Length)
		{
			goto IL_00da;
		}
		serverHostKeyAlgorithms = p1.ServerHostKeyAlgorithms;
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_0116;
		}
		goto IL_0138;
		IL_0116:
		string text3 = serverHostKeyAlgorithms[num3];
		if (text3.IndexOf("sha2", StringComparison.Ordinal) >= 0)
		{
			return false;
		}
		num3++;
		goto IL_0138;
		IL_0183:
		return flag2;
	}

	internal int wgwpn(int p0, string p1)
	{
		if (p1.IndexOf("CerberusFTPServer_12.", StringComparison.Ordinal) >= 0)
		{
			return Math.Max(p0, 2097152);
		}
		return p0;
	}

	internal int ftsnf(string p0)
	{
		if (p0.IndexOf("WS_FTP-SSH_7.", StringComparison.Ordinal) >= 0)
		{
			return Math.Min(cvejt, 32768);
		}
		return cvejt;
	}

	private object rmwfm(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "ExtensionNegotiation")
			{
				return !qeeeu;
			}
			if (text == "SoftwareVersion")
			{
				return yybtv;
			}
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	object gnyvx.jfzti(string p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rmwfm
		return this.rmwfm(p0);
	}

	private void unvzz(string p0, object p1)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "ExtensionNegotiation")
			{
				qeeeu = !(p1 is bool) || !(bool)p1;
				return;
			}
			if (text == "SoftwareVersion")
			{
				yybtv = p1 as string;
				return;
			}
		}
		throw new InvalidOperationException("Unsupported option.");
	}

	void gnyvx.vhvwu(string p0, object p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in unvzz
		this.unvzz(p0, p1);
	}

	internal string ybnef()
	{
		string text = "SSH-2.0-";
		int cilda = lzles.jksyb().cilda;
		string text2 = yybtv;
		string text3 = text2;
		if (text3 == null || 1 == 0)
		{
			text3 = "RebexSSH_5.0." + cilda + ".0";
		}
		return text + text3;
	}

	internal static string jxmlr()
	{
		return new SshParameters().ybnef();
	}
}
