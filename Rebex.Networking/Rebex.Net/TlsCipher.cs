using System;
using System.Collections.Generic;
using System.Text;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public class TlsCipher
{
	private sealed class cwqlq
	{
		public List<TlsCipherSuite> erdnf;

		public void pmdao(TlsCipherSuite p0, TlsCipherSuiteId p1)
		{
			azljv.Add(p0, p1);
			tsseo.Add(p1, p0);
			erdnf.Add(p0);
		}
	}

	private const string yujlt = "None";

	internal const int vangv = 16;

	private TlsBulkCipherAlgorithm azlfh;

	private TlsBulkCipherMode pprwy;

	private TlsMacAlgorithm wcsxr;

	private int itklu;

	private int rjptj;

	private int zmhbs;

	private int ohycr;

	private int otqzp;

	private int ykqsm;

	private int yaoyb;

	private TlsProtocol rkyom;

	private TlsKeyExchangeAlgorithm whcxe;

	private TlsCipherSuiteId tyyhf;

	private SignatureHashAlgorithm rzynb;

	private readonly bool tsvej;

	internal static readonly TlsCipher tpdwy;

	private static readonly object ddxfb;

	private static readonly Dictionary<TlsCipherSuite, TlsCipherSuiteId> azljv;

	private static readonly Dictionary<TlsCipherSuiteId, TlsCipherSuite> tsseo;

	private static readonly TlsCipherSuite[] dukbt;

	public TlsBulkCipherAlgorithm CipherAlgorithm => azlfh;

	public TlsBulkCipherMode CipherMode => pprwy;

	public TlsMacAlgorithm MacAlgorithm => wcsxr;

	public bool Cbc => pprwy == TlsBulkCipherMode.CBC;

	public int KeyMaterialSize => itklu;

	public int KeySize => rjptj;

	public int EffectiveKeySize => zmhbs;

	public int BlockSize => ohycr;

	internal int mxsqt
	{
		get
		{
			if (pprwy != TlsBulkCipherMode.GCM)
			{
				return 0;
			}
			return 16;
		}
	}

	public int MacSize => otqzp;

	internal int iclvv => ykqsm;

	public bool Exportable => yaoyb > 0;

	internal int zorll => yaoyb;

	public TlsProtocol Protocol => rkyom;

	public TlsKeyExchangeAlgorithm KeyExchangeAlgorithm => whcxe;

	internal KeyAlgorithm hflum
	{
		get
		{
			switch (whcxe)
			{
			case TlsKeyExchangeAlgorithm.RSA:
			case TlsKeyExchangeAlgorithm.DHE_RSA:
			case TlsKeyExchangeAlgorithm.ECDHE_RSA:
				return KeyAlgorithm.RSA;
			case TlsKeyExchangeAlgorithm.DHE_DSS:
				return KeyAlgorithm.DSA;
			case TlsKeyExchangeAlgorithm.ECDHE_ECDSA:
				return KeyAlgorithm.ECDsa;
			default:
				return KeyAlgorithm.Unsupported;
			}
		}
	}

	public string Suite => tyyhf.ToString();

	internal SignatureHashAlgorithm mabiw => rzynb;

	internal bool qzrcc => tsvej;

	internal TlsVersion lvtco()
	{
		return rkyom switch
		{
			TlsProtocol.SSL30 => TlsVersion.SSL30, 
			TlsProtocol.TLS10 => TlsVersion.TLS10, 
			TlsProtocol.TLS11 => TlsVersion.TLS11, 
			TlsProtocol.TLS12 => TlsVersion.TLS12, 
			_ => TlsVersion.None, 
		};
	}

	public override string ToString()
	{
		if (tsvej && 0 == 0)
		{
			return "None";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(tvgwn(rkyom));
		stringBuilder.Append(", ");
		if (yaoyb > 0)
		{
			stringBuilder.Append("Exportable ");
		}
		switch (whcxe)
		{
		case TlsKeyExchangeAlgorithm.DHE_DSS:
			stringBuilder.Append("DSS with ephemeral Diffie-Hellman");
			break;
		case TlsKeyExchangeAlgorithm.DHE_RSA:
			stringBuilder.Append("RSA with ephemeral Diffie-Hellman");
			break;
		case TlsKeyExchangeAlgorithm.ECDHE_RSA:
			stringBuilder.Append("RSA with ephemeral ECDH");
			break;
		case TlsKeyExchangeAlgorithm.ECDHE_ECDSA:
			stringBuilder.Append("ECDSA with ephemeral ECDH");
			break;
		case TlsKeyExchangeAlgorithm.ECDHE:
			stringBuilder.Append("ephemeral ECDH");
			break;
		default:
			stringBuilder.Append("RSA");
			break;
		}
		stringBuilder.Append(", ");
		stringBuilder.arumx(azlfh);
		stringBuilder.Append(" with ");
		stringBuilder.arumx(zmhbs * 8);
		stringBuilder.Append("-bit key");
		switch (pprwy)
		{
		case TlsBulkCipherMode.Stream:
			stringBuilder.Append(" in stream mode, ");
			stringBuilder.arumx(wcsxr);
			break;
		case TlsBulkCipherMode.CBC:
			stringBuilder.Append(" in CBC mode, ");
			stringBuilder.arumx(wcsxr);
			break;
		case TlsBulkCipherMode.GCM:
			if (azlfh != TlsBulkCipherAlgorithm.Chacha20Poly1305)
			{
				stringBuilder.Append(" in GCM mode,");
			}
			else
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(" AEAD");
			break;
		}
		return stringBuilder.ToString();
	}

	internal static string tvgwn(TlsProtocol p0)
	{
		return p0 switch
		{
			TlsProtocol.TLS12 => "TLS 1.2", 
			TlsProtocol.TLS11 => "TLS 1.1", 
			TlsProtocol.TLS10 => "TLS 1.0", 
			TlsProtocol.SSL30 => "SSL 3.0", 
			_ => "(unknown protocol)", 
		};
	}

	internal TlsCipher(TlsBulkCipherAlgorithm cipherAlgorithm, TlsMacAlgorithm macAlgorithm, TlsBulkCipherMode cipherMode, int keyMaterialSize, int keySize, int effectiveKeySize, int blockSize, int macSize, int ivSize, int exportable, TlsProtocol protocol, TlsKeyExchangeAlgorithm keyExchangeAlgorithm, TlsCipherSuiteId suite, SignatureHashAlgorithm derivationHashAlgorithm, bool isNone = false)
	{
		azlfh = cipherAlgorithm;
		wcsxr = macAlgorithm;
		pprwy = cipherMode;
		itklu = keyMaterialSize;
		rjptj = keySize;
		zmhbs = effectiveKeySize;
		ohycr = blockSize;
		otqzp = macSize;
		ykqsm = ivSize;
		yaoyb = exportable;
		rkyom = protocol;
		whcxe = keyExchangeAlgorithm;
		tyyhf = suite;
		rzynb = derivationHashAlgorithm;
		tsvej = isNone;
	}

	private static TlsCipher ydijb(int p0, TlsBulkCipherAlgorithm p1, TlsMacAlgorithm p2, int p3, int p4, int p5, int p6, int p7, TlsKeyExchangeAlgorithm p8, int p9, TlsCipherSuiteId p10)
	{
		return arlul(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, SignatureHashAlgorithm.SHA256, (p6 > 1) ? TlsBulkCipherMode.CBC : TlsBulkCipherMode.Stream, p6);
	}

	private static TlsCipher arlul(int p0, TlsBulkCipherAlgorithm p1, TlsMacAlgorithm p2, int p3, int p4, int p5, int p6, int p7, TlsKeyExchangeAlgorithm p8, int p9, TlsCipherSuiteId p10, SignatureHashAlgorithm p11, TlsBulkCipherMode p12, int p13)
	{
		TlsProtocol tlsProtocol;
		switch (p9)
		{
		case 768:
			tlsProtocol = TlsProtocol.SSL30;
			if (tlsProtocol != TlsProtocol.None)
			{
				break;
			}
			goto case 769;
		case 769:
			tlsProtocol = TlsProtocol.TLS10;
			if (tlsProtocol != TlsProtocol.None)
			{
				break;
			}
			goto case 770;
		case 770:
			tlsProtocol = TlsProtocol.TLS11;
			if (tlsProtocol != TlsProtocol.None)
			{
				break;
			}
			goto case 771;
		case 771:
			tlsProtocol = TlsProtocol.TLS12;
			if (tlsProtocol == TlsProtocol.None)
			{
				goto default;
			}
			break;
		default:
			throw new ArgumentException("Unsupported TLS/SSL protocol version.", "protocolVersion");
		}
		return new TlsCipher(p1, p2, p12, p3, p4, p5, p6, p7, p13, p0, tlsProtocol, p8, p10, p11);
	}

	internal static TlsCipher lfexb(TlsCipherSuiteId p0, int p1)
	{
		TlsMacAlgorithm p2 = TlsMacAlgorithm.MD5;
		TlsMacAlgorithm p3 = TlsMacAlgorithm.SHA1;
		TlsMacAlgorithm p4 = TlsMacAlgorithm.SHA256;
		TlsMacAlgorithm p5 = TlsMacAlgorithm.SHA384;
		TlsMacAlgorithm p6 = TlsMacAlgorithm.None;
		return p0 switch
		{
			TlsCipherSuiteId.RSA_EXPORT_WITH_RC4_40_MD5 => ydijb(512, TlsBulkCipherAlgorithm.RC4, p2, 5, 16, 5, 0, 16, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_RC4_128_MD5 => ydijb(0, TlsBulkCipherAlgorithm.RC4, p2, 16, 16, 16, 0, 16, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_RC4_128_SHA => ydijb(0, TlsBulkCipherAlgorithm.RC4, p3, 16, 16, 16, 0, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_EXPORT_WITH_RC2_CBC_40_MD5 => ydijb(512, TlsBulkCipherAlgorithm.RC2, p2, 5, 16, 5, 8, 16, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_EXPORT_WITH_DES40_CBC_SHA => ydijb(512, TlsBulkCipherAlgorithm.DES, p3, 5, 8, 5, 8, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_DES_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.DES, p3, 8, 8, 7, 8, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_3DES_EDE_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.TripleDES, p3, 24, 24, 21, 8, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_EXPORT1024_WITH_DES_CBC_SHA => ydijb(1024, TlsBulkCipherAlgorithm.DES, p3, 8, 8, 7, 8, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_EXPORT1024_WITH_RC4_56_SHA => ydijb(1024, TlsBulkCipherAlgorithm.RC4, p3, 7, 16, 7, 0, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_AES_128_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 16, 16, 16, 16, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_AES_256_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 32, 32, 32, 16, 20, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_AES_128_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 16, 16, 16, 16, 32, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_AES_256_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 32, 32, 32, 16, 32, TlsKeyExchangeAlgorithm.RSA, p1, p0), 
			TlsCipherSuiteId.RSA_WITH_AES_128_GCM_SHA256 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 16, 16, 16, 16, 0, TlsKeyExchangeAlgorithm.RSA, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.RSA_WITH_AES_256_GCM_SHA384 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.RSA, p1, p0, SignatureHashAlgorithm.SHA384, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.DHE_RSA_EXPORT_WITH_DES40_CBC_SHA => ydijb(512, TlsBulkCipherAlgorithm.DES, p3, 5, 8, 5, 8, 20, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0), 
			TlsCipherSuiteId.DHE_RSA_WITH_DES_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.DES, p3, 8, 8, 7, 8, 20, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0), 
			TlsCipherSuiteId.DHE_RSA_WITH_3DES_EDE_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.TripleDES, p3, 24, 24, 21, 8, 20, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0), 
			TlsCipherSuiteId.DHE_RSA_WITH_AES_128_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 16, 16, 16, 16, 20, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0), 
			TlsCipherSuiteId.DHE_RSA_WITH_AES_256_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 32, 32, 32, 16, 20, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0), 
			TlsCipherSuiteId.DHE_RSA_WITH_AES_128_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 16, 16, 16, 16, 32, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0), 
			TlsCipherSuiteId.DHE_RSA_WITH_AES_256_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 32, 32, 32, 16, 32, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0), 
			TlsCipherSuiteId.DHE_RSA_WITH_AES_128_GCM_SHA256 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 16, 16, 16, 16, 0, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.DHE_RSA_WITH_AES_256_GCM_SHA384 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0, SignatureHashAlgorithm.SHA384, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.DHE_RSA_WITH_CHACHA20_POLY1305_SHA256 => arlul(0, TlsBulkCipherAlgorithm.Chacha20Poly1305, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.DHE_RSA, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 12), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_128_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 16, 16, 16, 16, 20, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_256_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 32, 32, 32, 16, 20, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.TripleDES, p3, 24, 24, 21, 8, 20, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_RC4_128_SHA => ydijb(0, TlsBulkCipherAlgorithm.RC4, p3, 16, 16, 16, 0, 20, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 16, 16, 16, 16, 32, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 => arlul(0, TlsBulkCipherAlgorithm.AES, p5, 32, 32, 32, 16, 48, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0, SignatureHashAlgorithm.SHA384, TlsBulkCipherMode.CBC, 16), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 16, 16, 16, 16, 0, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0, SignatureHashAlgorithm.SHA384, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256 => arlul(0, TlsBulkCipherAlgorithm.Chacha20Poly1305, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.ECDHE_ECDSA, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 12), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_AES_128_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 16, 16, 16, 16, 20, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_AES_256_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 32, 32, 32, 16, 20, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.TripleDES, p3, 24, 24, 21, 8, 20, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_RC4_128_SHA => ydijb(0, TlsBulkCipherAlgorithm.RC4, p3, 16, 16, 16, 0, 20, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_AES_128_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 16, 16, 16, 16, 32, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_AES_256_CBC_SHA384 => arlul(0, TlsBulkCipherAlgorithm.AES, p5, 32, 32, 32, 16, 48, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0, SignatureHashAlgorithm.SHA384, TlsBulkCipherMode.CBC, 16), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_AES_128_GCM_SHA256 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 16, 16, 16, 16, 0, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_AES_256_GCM_SHA384 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0, SignatureHashAlgorithm.SHA384, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 => arlul(0, TlsBulkCipherAlgorithm.Chacha20Poly1305, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.ECDHE_RSA, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 12), 
			TlsCipherSuiteId.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA => ydijb(512, TlsBulkCipherAlgorithm.DES, p3, 5, 8, 5, 8, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_DES_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.DES, p3, 8, 8, 7, 8, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_3DES_EDE_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.TripleDES, p3, 24, 24, 21, 8, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA => ydijb(1024, TlsBulkCipherAlgorithm.DES, p3, 8, 8, 7, 8, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA => ydijb(1024, TlsBulkCipherAlgorithm.RC4, p3, 7, 16, 7, 0, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_RC4_128_SHA => ydijb(0, TlsBulkCipherAlgorithm.RC4, p3, 16, 16, 16, 0, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_AES_128_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 16, 16, 16, 16, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_AES_256_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 32, 32, 32, 16, 20, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_AES_128_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 16, 16, 16, 16, 32, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_AES_256_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 32, 32, 32, 16, 32, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0), 
			TlsCipherSuiteId.DHE_DSS_WITH_AES_128_GCM_SHA256 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 16, 16, 16, 16, 0, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0, SignatureHashAlgorithm.SHA256, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.DHE_DSS_WITH_AES_256_GCM_SHA384 => arlul(0, TlsBulkCipherAlgorithm.AES, p6, 32, 32, 32, 16, 0, TlsKeyExchangeAlgorithm.DHE_DSS, p1, p0, SignatureHashAlgorithm.SHA384, TlsBulkCipherMode.GCM, 4), 
			TlsCipherSuiteId.DH_anon_WITH_RC4_128_MD5 => ydijb(0, TlsBulkCipherAlgorithm.RC4, p2, 16, 16, 16, 0, 16, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			TlsCipherSuiteId.DH_anon_WITH_3DES_EDE_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.TripleDES, p3, 24, 24, 21, 8, 20, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			TlsCipherSuiteId.DH_anon_WITH_DES_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.DES, p3, 8, 8, 7, 8, 20, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			TlsCipherSuiteId.DH_anon_WITH_AES_128_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 16, 16, 16, 16, 20, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			TlsCipherSuiteId.DH_anon_WITH_AES_256_CBC_SHA => ydijb(0, TlsBulkCipherAlgorithm.AES, p3, 32, 32, 32, 16, 20, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			TlsCipherSuiteId.DH_anon_WITH_AES_128_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 16, 16, 16, 16, 32, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			TlsCipherSuiteId.DH_anon_WITH_AES_256_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.AES, p4, 32, 32, 32, 16, 32, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			TlsCipherSuiteId.DH_anon_WITH_TWOFISH_CBC_SHA256 => ydijb(0, TlsBulkCipherAlgorithm.Twofish, p4, 32, 32, 32, 16, 32, TlsKeyExchangeAlgorithm.DH_anon, p1, p0), 
			_ => null, 
		};
	}

	internal static TlsCipherSuiteId plzju(TlsCipherSuite p0)
	{
		lock (ddxfb)
		{
			if (!azljv.TryGetValue(p0, out var value) || 1 == 0)
			{
				return TlsCipherSuiteId.NULL;
			}
			return value;
		}
	}

	internal static TlsCipherSuite jnahn(TlsCipherSuiteId p0)
	{
		lock (ddxfb)
		{
			if (!tsseo.TryGetValue(p0, out var value) || 1 == 0)
			{
				return TlsCipherSuite.None;
			}
			return value;
		}
	}

	internal static ICollection<TlsCipherSuite> xnjzt(TlsCipherSuite p0, ICollection<TlsCipherSuite> p1)
	{
		TlsCipherSuite tlsCipherSuite = TlsCipherSuite.None;
		List<TlsCipherSuite> list = new List<TlsCipherSuite>();
		IEnumerator<TlsCipherSuite> enumerator = p1.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				TlsCipherSuite current = enumerator.Current;
				if ((current & p0) == current)
				{
					tlsCipherSuite |= current;
					list.Add(current);
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		TlsCipherSuite[] array = dukbt;
		int num = 0;
		if (num != 0)
		{
			goto IL_0069;
		}
		goto IL_0089;
		IL_0089:
		if (num >= array.Length)
		{
			return list;
		}
		goto IL_0069;
		IL_0069:
		TlsCipherSuite tlsCipherSuite2 = array[num];
		if ((tlsCipherSuite2 & p0) == tlsCipherSuite2 && (tlsCipherSuite2 & tlsCipherSuite) == 0)
		{
			list.Add(tlsCipherSuite2);
		}
		num++;
		goto IL_0089;
	}

	static TlsCipher()
	{
		tpdwy = new TlsCipher(TlsBulkCipherAlgorithm.None, TlsMacAlgorithm.None, TlsBulkCipherMode.Stream, 0, 0, 0, 0, 0, 0, 0, TlsProtocol.None, TlsKeyExchangeAlgorithm.None, TlsCipherSuiteId.NULL, SignatureHashAlgorithm.MD5, isNone: true);
		cwqlq cwqlq = new cwqlq();
		ddxfb = new object();
		azljv = new Dictionary<TlsCipherSuite, TlsCipherSuiteId>();
		tsseo = new Dictionary<TlsCipherSuiteId, TlsCipherSuite>();
		cwqlq.erdnf = new List<TlsCipherSuite>();
		Action<TlsCipherSuite, TlsCipherSuiteId> action = cwqlq.pmdao;
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256, TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384, TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256, TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384, TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384);
		action(TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256, TlsCipherSuiteId.ECDHE_RSA_WITH_AES_128_GCM_SHA256);
		action(TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384, TlsCipherSuiteId.ECDHE_RSA_WITH_AES_256_GCM_SHA384);
		action(TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA256, TlsCipherSuiteId.ECDHE_RSA_WITH_AES_128_CBC_SHA256);
		action(TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA384, TlsCipherSuiteId.ECDHE_RSA_WITH_AES_256_CBC_SHA384);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA, TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_128_CBC_SHA);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA, TlsCipherSuiteId.ECDHE_ECDSA_WITH_AES_256_CBC_SHA);
		action(TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA, TlsCipherSuiteId.ECDHE_RSA_WITH_AES_128_CBC_SHA);
		action(TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA, TlsCipherSuiteId.ECDHE_RSA_WITH_AES_256_CBC_SHA);
		action(TlsCipherSuite.DHE_RSA_WITH_AES_256_GCM_SHA384, TlsCipherSuiteId.DHE_RSA_WITH_AES_256_GCM_SHA384);
		action(TlsCipherSuite.DHE_RSA_WITH_AES_128_GCM_SHA256, TlsCipherSuiteId.DHE_RSA_WITH_AES_128_GCM_SHA256);
		action(TlsCipherSuite.DHE_RSA_WITH_AES_256_CBC_SHA256, TlsCipherSuiteId.DHE_RSA_WITH_AES_256_CBC_SHA256);
		action(TlsCipherSuite.DHE_RSA_WITH_AES_128_CBC_SHA256, TlsCipherSuiteId.DHE_RSA_WITH_AES_128_CBC_SHA256);
		action(TlsCipherSuite.RSA_WITH_AES_256_GCM_SHA384, TlsCipherSuiteId.RSA_WITH_AES_256_GCM_SHA384);
		action(TlsCipherSuite.RSA_WITH_AES_128_GCM_SHA256, TlsCipherSuiteId.RSA_WITH_AES_128_GCM_SHA256);
		action(TlsCipherSuite.RSA_WITH_AES_256_CBC_SHA256, TlsCipherSuiteId.RSA_WITH_AES_256_CBC_SHA256);
		action(TlsCipherSuite.RSA_WITH_AES_128_CBC_SHA256, TlsCipherSuiteId.RSA_WITH_AES_128_CBC_SHA256);
		action(TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384, TlsCipherSuiteId.DHE_DSS_WITH_AES_256_GCM_SHA384);
		action(TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256, TlsCipherSuiteId.DHE_DSS_WITH_AES_128_GCM_SHA256);
		action(TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256, TlsCipherSuiteId.DHE_DSS_WITH_AES_256_CBC_SHA256);
		action(TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256, TlsCipherSuiteId.DHE_DSS_WITH_AES_128_CBC_SHA256);
		action(TlsCipherSuite.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256, TlsCipherSuiteId.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256, TlsCipherSuiteId.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256);
		action(TlsCipherSuite.DHE_RSA_WITH_CHACHA20_POLY1305_SHA256, TlsCipherSuiteId.DHE_RSA_WITH_CHACHA20_POLY1305_SHA256);
		action(TlsCipherSuite.DHE_RSA_WITH_AES_128_CBC_SHA, TlsCipherSuiteId.DHE_RSA_WITH_AES_128_CBC_SHA);
		action(TlsCipherSuite.DHE_RSA_WITH_AES_256_CBC_SHA, TlsCipherSuiteId.DHE_RSA_WITH_AES_256_CBC_SHA);
		action(TlsCipherSuite.RSA_WITH_AES_128_CBC_SHA, TlsCipherSuiteId.RSA_WITH_AES_128_CBC_SHA);
		action(TlsCipherSuite.RSA_WITH_AES_256_CBC_SHA, TlsCipherSuiteId.RSA_WITH_AES_256_CBC_SHA);
		action(TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA, TlsCipherSuiteId.DHE_DSS_WITH_AES_128_CBC_SHA);
		action(TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA, TlsCipherSuiteId.DHE_DSS_WITH_AES_256_CBC_SHA);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA, TlsCipherSuiteId.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA);
		action(TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA, TlsCipherSuiteId.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA);
		action(TlsCipherSuite.DHE_RSA_WITH_3DES_EDE_CBC_SHA, TlsCipherSuiteId.DHE_RSA_WITH_3DES_EDE_CBC_SHA);
		action(TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA, TlsCipherSuiteId.RSA_WITH_3DES_EDE_CBC_SHA);
		action(TlsCipherSuite.DHE_DSS_WITH_3DES_EDE_CBC_SHA, TlsCipherSuiteId.DHE_DSS_WITH_3DES_EDE_CBC_SHA);
		action(TlsCipherSuite.ECDHE_ECDSA_WITH_RC4_128_SHA, TlsCipherSuiteId.ECDHE_ECDSA_WITH_RC4_128_SHA);
		action(TlsCipherSuite.ECDHE_RSA_WITH_RC4_128_SHA, TlsCipherSuiteId.ECDHE_RSA_WITH_RC4_128_SHA);
		action(TlsCipherSuite.RSA_WITH_RC4_128_SHA, TlsCipherSuiteId.RSA_WITH_RC4_128_SHA);
		action(TlsCipherSuite.RSA_WITH_RC4_128_MD5, TlsCipherSuiteId.RSA_WITH_RC4_128_MD5);
		action(TlsCipherSuite.DHE_DSS_WITH_RC4_128_SHA, TlsCipherSuiteId.DHE_DSS_WITH_RC4_128_SHA);
		action(TlsCipherSuite.DHE_RSA_WITH_DES_CBC_SHA, TlsCipherSuiteId.DHE_RSA_WITH_DES_CBC_SHA);
		action(TlsCipherSuite.RSA_WITH_DES_CBC_SHA, TlsCipherSuiteId.RSA_WITH_DES_CBC_SHA);
		action(TlsCipherSuite.DHE_DSS_WITH_DES_CBC_SHA, TlsCipherSuiteId.DHE_DSS_WITH_DES_CBC_SHA);
		action(TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA, TlsCipherSuiteId.RSA_EXPORT1024_WITH_DES_CBC_SHA);
		action(TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA, TlsCipherSuiteId.RSA_EXPORT1024_WITH_RC4_56_SHA);
		action(TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA, TlsCipherSuiteId.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA);
		action(TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA, TlsCipherSuiteId.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA);
		action(TlsCipherSuite.DHE_RSA_EXPORT_WITH_DES40_CBC_SHA, TlsCipherSuiteId.DHE_RSA_EXPORT_WITH_DES40_CBC_SHA);
		action(TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5, TlsCipherSuiteId.RSA_EXPORT_WITH_RC4_40_MD5);
		action(TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5, TlsCipherSuiteId.RSA_EXPORT_WITH_RC2_CBC_40_MD5);
		action(TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA, TlsCipherSuiteId.RSA_EXPORT_WITH_DES40_CBC_SHA);
		action(TlsCipherSuite.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA, TlsCipherSuiteId.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA);
		action(TlsCipherSuite.DH_anon_WITH_AES_256_CBC_SHA256, TlsCipherSuiteId.DH_anon_WITH_AES_256_CBC_SHA256);
		action(TlsCipherSuite.DH_anon_WITH_AES_128_CBC_SHA256, TlsCipherSuiteId.DH_anon_WITH_AES_128_CBC_SHA256);
		action((TlsCipherSuite)72057594037927936L, TlsCipherSuiteId.DH_anon_WITH_TWOFISH_CBC_SHA256);
		action(TlsCipherSuite.DH_anon_WITH_AES_256_CBC_SHA, TlsCipherSuiteId.DH_anon_WITH_AES_256_CBC_SHA);
		action(TlsCipherSuite.DH_anon_WITH_AES_128_CBC_SHA, TlsCipherSuiteId.DH_anon_WITH_AES_128_CBC_SHA);
		action(TlsCipherSuite.DH_anon_WITH_RC4_128_MD5, TlsCipherSuiteId.DH_anon_WITH_RC4_128_MD5);
		action(TlsCipherSuite.DH_anon_WITH_3DES_EDE_CBC_SHA, TlsCipherSuiteId.DH_anon_WITH_3DES_EDE_CBC_SHA);
		action(TlsCipherSuite.DH_anon_WITH_DES_CBC_SHA, TlsCipherSuiteId.DH_anon_WITH_DES_CBC_SHA);
		dukbt = cwqlq.erdnf.ToArray();
	}
}
