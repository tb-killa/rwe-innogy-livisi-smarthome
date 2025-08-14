using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using Rebex.IO;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal static class bpkgq
{
	public const int cuaxy = 1;

	public const int xwjzq = 2;

	public const int etiho = 3;

	public const int lximl = 4;

	public const int xzwbb = 5;

	public const int kamqo = 6;

	public const int swvgo = 7;

	public const int whixi = 8;

	public const int yusjl = 9;

	public static string qoowv(ChecksumAlgorithm p0)
	{
		return p0 switch
		{
			ChecksumAlgorithm.CRC32 => "crc32", 
			ChecksumAlgorithm.MD5 => "md5", 
			ChecksumAlgorithm.SHA1 => "sha1", 
			ChecksumAlgorithm.SHA224 => "sha224", 
			ChecksumAlgorithm.SHA256 => "sha256", 
			ChecksumAlgorithm.SHA384 => "sha384", 
			ChecksumAlgorithm.SHA512 => "sha512", 
			_ => throw hifyx.nztrs("algorithm", p0, "Argument is out of range of valid values."), 
		};
	}

	public static string[] rlhit()
	{
		return new string[7] { "crc32", "md5", "sha1", "sha224", "sha256", "sha384", "sha512" };
	}

	public static void xqnvu(ChecksumAlgorithm p0, string p1)
	{
		switch (p0)
		{
		case ChecksumAlgorithm.SHA1:
		case ChecksumAlgorithm.SHA224:
		case ChecksumAlgorithm.SHA256:
		case ChecksumAlgorithm.SHA384:
		case ChecksumAlgorithm.SHA512:
		case ChecksumAlgorithm.MD5:
		case ChecksumAlgorithm.CRC32:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Argument is out of range of valid values.");
	}

	public static AlgorithmIdentifier zvown(string p0, SignatureHashAlgorithm? p1)
	{
		if (p0.StartsWith("rsa", StringComparison.Ordinal) && 0 == 0)
		{
			p0 = "rsa";
		}
		else if (p0.StartsWith("dsa", StringComparison.Ordinal) && 0 == 0)
		{
			p0 = "dsa";
		}
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "rsa" || text == "ssh-rsa")
			{
				return AlgorithmIdentifier.zcpsk(p1);
			}
			if (text == "dsa" || text == "ssh-dss")
			{
				return AlgorithmIdentifier.xhphy(p1);
			}
			if (text == "dh")
			{
				throw new CryptographicException("Not supported for this key algorithm.");
			}
		}
		kqdqs(p0, out var p2, out var p3);
		return AlgorithmIdentifier.glnvf(p2, p3, p1);
	}

	public static bool guhie(string p0, out AsymmetricKeyAlgorithmId p1, out int p2)
	{
		if (p0 == null || 1 == 0)
		{
			p1 = AsymmetricKeyAlgorithmId.RSA;
			p2 = 0;
			return false;
		}
		if (p0.StartsWith("rsa", StringComparison.Ordinal) && 0 == 0)
		{
			p1 = AsymmetricKeyAlgorithmId.RSA;
		}
		else
		{
			string p3;
			string p4;
			int p5;
			int p6;
			HashingAlgorithmId p7;
			if (!p0.StartsWith("dsa", StringComparison.Ordinal))
			{
				return fmlhx(p0, out p3, out p1, out p4, out p2, out p5, out p6, out p7);
			}
			p1 = AsymmetricKeyAlgorithmId.DSA;
		}
		if (!brgjd.bnrqx(p0.Substring(3), out p2) || 1 == 0)
		{
			return false;
		}
		return true;
	}

	public static AlgorithmIdentifier aykug(string p0)
	{
		kqdqs(p0, out var p1, out var p2);
		return AlgorithmIdentifier.xhnfa(p1, p2);
	}

	public static bool fmlhx(string p0, out string p1, out AsymmetricKeyAlgorithmId p2, out string p3, out int p4, out int p5, out int p6, out HashingAlgorithmId p7)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.lsnzl == null || 1 == 0)
			{
				fnfqw.lsnzl = new Dictionary<string, int>(29)
				{
					{ "ed25519-sha512", 0 },
					{ "ecdh-sha2-curve25519", 1 },
					{ "curve25519-sha256", 2 },
					{ "ECDSA_P256", 3 },
					{ "ECDsaP256", 4 },
					{ "ecdsa-sha2-nistp256", 5 },
					{ "ECDSA_P384", 6 },
					{ "ECDsaP384", 7 },
					{ "ecdsa-sha2-nistp384", 8 },
					{ "ECDSA_P521", 9 },
					{ "ECDsaP521", 10 },
					{ "ecdsa-sha2-nistp521", 11 },
					{ "ECDH_P256", 12 },
					{ "ECDiffieHellmanP256", 13 },
					{ "ecdh-sha2-nistp256", 14 },
					{ "ECDH_P384", 15 },
					{ "ECDiffieHellmanP384", 16 },
					{ "ecdh-sha2-nistp384", 17 },
					{ "ECDH_P521", 18 },
					{ "ECDiffieHellmanP521", 19 },
					{ "ecdh-sha2-nistp521", 20 },
					{ "ecdsa-sha2-brainpoolp256r1", 21 },
					{ "ecdsa-sha2-brainpoolp384r1", 22 },
					{ "ecdsa-sha2-brainpoolp512r1", 23 },
					{ "ecdsa-sha2-secp256k1", 24 },
					{ "ecdh-sha2-brainpoolp256r1", 25 },
					{ "ecdh-sha2-brainpoolp384r1", 26 },
					{ "ecdh-sha2-brainpoolp512r1", 27 },
					{ "ecdh-sha2-secp256k1", 28 }
				};
			}
			if (fnfqw.lsnzl.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					p1 = "ed25519-sha512";
					p2 = AsymmetricKeyAlgorithmId.EdDsa;
					p3 = "1.3.101.112";
					p4 = 256;
					p5 = 32;
					p6 = 64;
					p7 = HashingAlgorithmId.SHA512;
					goto IL_0521;
				case 1:
				case 2:
					p1 = "ecdh-sha2-curve25519";
					p2 = AsymmetricKeyAlgorithmId.ECDH;
					p3 = "1.3.101.110";
					p4 = 256;
					p5 = 32;
					p6 = 32;
					p7 = HashingAlgorithmId.SHA256;
					goto IL_0521;
				case 3:
				case 4:
				case 5:
					p1 = "ecdsa-sha2-nistp256";
					p2 = AsymmetricKeyAlgorithmId.ECDsa;
					p3 = "1.2.840.10045.3.1.7";
					p4 = 256;
					p5 = 32;
					p6 = 32;
					p7 = HashingAlgorithmId.SHA256;
					goto IL_0521;
				case 6:
				case 7:
				case 8:
					p1 = "ecdsa-sha2-nistp384";
					p2 = AsymmetricKeyAlgorithmId.ECDsa;
					p3 = "1.3.132.0.34";
					p4 = 384;
					p5 = 48;
					p6 = 48;
					p7 = HashingAlgorithmId.SHA384;
					goto IL_0521;
				case 9:
				case 10:
				case 11:
					p1 = "ecdsa-sha2-nistp521";
					p2 = AsymmetricKeyAlgorithmId.ECDsa;
					p3 = "1.3.132.0.35";
					p4 = 521;
					p5 = 66;
					p6 = 64;
					p7 = HashingAlgorithmId.SHA512;
					goto IL_0521;
				case 12:
				case 13:
				case 14:
					p1 = "ecdh-sha2-nistp256";
					p2 = AsymmetricKeyAlgorithmId.ECDH;
					p3 = "1.2.840.10045.3.1.7";
					p4 = 256;
					p5 = 32;
					p6 = 32;
					p7 = HashingAlgorithmId.SHA256;
					goto IL_0521;
				case 15:
				case 16:
				case 17:
					p1 = "ecdh-sha2-nistp384";
					p2 = AsymmetricKeyAlgorithmId.ECDH;
					p3 = "1.3.132.0.34";
					p4 = 384;
					p5 = 48;
					p6 = 48;
					p7 = HashingAlgorithmId.SHA384;
					goto IL_0521;
				case 18:
				case 19:
				case 20:
					p1 = "ecdh-sha2-nistp521";
					p2 = AsymmetricKeyAlgorithmId.ECDH;
					p3 = "1.3.132.0.35";
					p4 = 521;
					p5 = 66;
					p6 = 64;
					p7 = HashingAlgorithmId.SHA512;
					goto IL_0521;
				case 21:
					p1 = "ecdsa-sha2-brainpoolp256r1";
					p2 = AsymmetricKeyAlgorithmId.ECDsa;
					p3 = "1.3.36.3.3.2.8.1.1.7";
					p4 = 256;
					p5 = 32;
					p6 = 32;
					p7 = HashingAlgorithmId.SHA256;
					goto IL_0521;
				case 22:
					p1 = "ecdsa-sha2-brainpoolp384r1";
					p2 = AsymmetricKeyAlgorithmId.ECDsa;
					p3 = "1.3.36.3.3.2.8.1.1.11";
					p4 = 384;
					p5 = 48;
					p6 = 48;
					p7 = HashingAlgorithmId.SHA384;
					goto IL_0521;
				case 23:
					p1 = "ecdsa-sha2-brainpoolp512r1";
					p2 = AsymmetricKeyAlgorithmId.ECDsa;
					p3 = "1.3.36.3.3.2.8.1.1.13";
					p4 = 512;
					p5 = 64;
					p6 = 64;
					p7 = HashingAlgorithmId.SHA512;
					goto IL_0521;
				case 24:
					p1 = "ecdsa-sha2-secp256k1";
					p2 = AsymmetricKeyAlgorithmId.ECDsa;
					p3 = "1.3.132.0.10";
					p4 = 256;
					p5 = 32;
					p6 = 32;
					p7 = HashingAlgorithmId.SHA256;
					goto IL_0521;
				case 25:
					p1 = "ecdh-sha2-brainpoolp256r1";
					p2 = AsymmetricKeyAlgorithmId.ECDH;
					p3 = "1.3.36.3.3.2.8.1.1.7";
					p4 = 256;
					p5 = 32;
					p6 = 32;
					p7 = HashingAlgorithmId.SHA256;
					goto IL_0521;
				case 26:
					p1 = "ecdh-sha2-brainpoolp384r1";
					p2 = AsymmetricKeyAlgorithmId.ECDH;
					p3 = "1.3.36.3.3.2.8.1.1.11";
					p4 = 384;
					p5 = 48;
					p6 = 48;
					p7 = HashingAlgorithmId.SHA384;
					goto IL_0521;
				case 27:
					p1 = "ecdh-sha2-brainpoolp512r1";
					p2 = AsymmetricKeyAlgorithmId.ECDH;
					p3 = "1.3.36.3.3.2.8.1.1.13";
					p4 = 512;
					p5 = 64;
					p6 = 64;
					p7 = HashingAlgorithmId.SHA512;
					goto IL_0521;
				case 28:
					{
						p1 = "ecdh-sha2-secp256k1";
						p2 = AsymmetricKeyAlgorithmId.ECDsa;
						p3 = "1.3.132.0.10";
						p4 = 256;
						p5 = 32;
						p6 = 32;
						p7 = HashingAlgorithmId.SHA256;
						goto IL_0521;
					}
					IL_0521:
					return true;
				}
			}
		}
		p1 = null;
		p2 = AsymmetricKeyAlgorithmId.ECDsa;
		p3 = null;
		p4 = 0;
		p5 = 0;
		p6 = 0;
		p7 = (HashingAlgorithmId)0;
		return false;
	}

	public static string pguks(AsymmetricKeyAlgorithmId p0, string p1, int p2, bool p3)
	{
		string text;
		switch (p0)
		{
		case AsymmetricKeyAlgorithmId.ECDsa:
			text = "ecdsa";
			break;
		case AsymmetricKeyAlgorithmId.ECDH:
			text = "ecdh";
			break;
		case AsymmetricKeyAlgorithmId.EdDsa:
			text = "ed";
			break;
		case AsymmetricKeyAlgorithmId.DiffieHellman:
			if (p2 >= 1024 && p2 <= 4096)
			{
				return "dh";
			}
			goto default;
		case AsymmetricKeyAlgorithmId.RSA:
			if (p2 >= 256 && p2 <= 16384)
			{
				return "rsa" + p2;
			}
			goto default;
		case AsymmetricKeyAlgorithmId.DSA:
			if (p2 >= 256 && p2 <= CryptoHelper.ngetc())
			{
				return "dsa" + p2;
			}
			goto default;
		default:
			if (p3 && 0 == 0)
			{
				throw new CryptographicException("Algorithm not supported.");
			}
			return null;
		}
		p1 = p1?.ToLower(dahxy.ldled);
		AsymmetricKeyAlgorithmId asymmetricKeyAlgorithmId = p0;
		string key;
		if ((key = p1) == null)
		{
			goto IL_0393;
		}
		if (fnfqw.pbooh == null || 1 == 0)
		{
			fnfqw.pbooh = new Dictionary<string, int>(24)
			{
				{ "nistp256", 0 },
				{ "secp256r1", 1 },
				{ "1.2.840.10045.3.1.7", 2 },
				{ "nistp384", 3 },
				{ "secp384r1", 4 },
				{ "1.3.132.0.34", 5 },
				{ "nistp521", 6 },
				{ "secp521r1", 7 },
				{ "1.3.132.0.35", 8 },
				{ "ed25519", 9 },
				{ "1.3.101.112", 10 },
				{ "1.3.6.1.4.1.11591.15.1", 11 },
				{ "x25519", 12 },
				{ "curve25519", 13 },
				{ "1.3.101.110", 14 },
				{ "brainpoolp256r1", 15 },
				{ "1.3.36.3.3.2.8.1.1.7", 16 },
				{ "brainpoolp384r1", 17 },
				{ "1.3.36.3.3.2.8.1.1.11", 18 },
				{ "brainpoolp512r1", 19 },
				{ "1.3.36.3.3.2.8.1.1.13", 20 },
				{ "secp256k1", 21 },
				{ "1.3.132.0.10", 22 },
				{ "none", 23 }
			};
		}
		if (!fnfqw.pbooh.TryGetValue(key, out var value))
		{
			goto IL_039c;
		}
		switch (value)
		{
		case 0:
		case 1:
		case 2:
			break;
		case 3:
		case 4:
		case 5:
			goto IL_02e3;
		case 6:
		case 7:
		case 8:
			goto IL_02fa;
		case 9:
		case 10:
		case 11:
			goto IL_0311;
		case 12:
		case 13:
		case 14:
			goto IL_032a;
		case 15:
		case 16:
			goto IL_0343;
		case 17:
		case 18:
			goto IL_0357;
		case 19:
		case 20:
			goto IL_036b;
		case 21:
		case 22:
			goto IL_037f;
		case 23:
			goto IL_0393;
		default:
			goto IL_039c;
		}
		int num = 256;
		text += "-sha2-nistp256";
		goto IL_03b4;
		IL_03b4:
		if (p0 != asymmetricKeyAlgorithmId)
		{
			throw new CryptographicException("Curve not supported for the specified algorithm.");
		}
		if (p2 != 0 && 0 == 0 && p2 != num)
		{
			if (p3 && 0 == 0)
			{
				throw new CryptographicException("Specified key size not supported.");
			}
			return null;
		}
		return text;
		IL_0393:
		num = 0;
		p2 = 0;
		goto IL_03b4;
		IL_037f:
		num = 256;
		text += "-sha2-secp256k1";
		goto IL_03b4;
		IL_032a:
		asymmetricKeyAlgorithmId = AsymmetricKeyAlgorithmId.ECDH;
		num = 256;
		text += "-sha2-curve25519";
		goto IL_03b4;
		IL_0311:
		asymmetricKeyAlgorithmId = AsymmetricKeyAlgorithmId.EdDsa;
		num = 256;
		text += "25519-sha512";
		goto IL_03b4;
		IL_039c:
		if (p3 && 0 == 0)
		{
			throw new CryptographicException("Algorithm or curve not supported.");
		}
		return null;
		IL_02fa:
		num = 521;
		text += "-sha2-nistp521";
		goto IL_03b4;
		IL_036b:
		num = 512;
		text += "-sha2-brainpoolp512r1";
		goto IL_03b4;
		IL_0357:
		num = 384;
		text += "-sha2-brainpoolp384r1";
		goto IL_03b4;
		IL_0343:
		num = 256;
		text += "-sha2-brainpoolp256r1";
		goto IL_03b4;
		IL_02e3:
		num = 384;
		text += "-sha2-nistp384";
		goto IL_03b4;
	}

	public static HashingAlgorithmId mfcal(AsymmetricKeyAlgorithmId p0, int p1)
	{
		switch (p0)
		{
		case AsymmetricKeyAlgorithmId.ECDsa:
			if (p1 < 384)
			{
				return HashingAlgorithmId.SHA256;
			}
			if (p1 == 384)
			{
				return HashingAlgorithmId.SHA384;
			}
			return HashingAlgorithmId.SHA512;
		case AsymmetricKeyAlgorithmId.RSA:
			return HashingAlgorithmId.SHA1;
		case AsymmetricKeyAlgorithmId.DSA:
			return HashingAlgorithmId.SHA1;
		case AsymmetricKeyAlgorithmId.EdDsa:
			return HashingAlgorithmId.SHA512;
		default:
			return HashingAlgorithmId.SHA256;
		}
	}

	public static HashingAlgorithmId qivxr(string p0)
	{
		if (!fmlhx(p0, out var _, out var _, out var _, out var _, out var _, out var _, out var p7) || 1 == 0)
		{
			return HashingAlgorithmId.SHA256;
		}
		return p7;
	}

	public static string mjwcm(string p0)
	{
		string key;
		if ((key = p0.ToLower(dahxy.ldled)) != null && 0 == 0)
		{
			if (fnfqw.ntzrd == null || 1 == 0)
			{
				fnfqw.ntzrd = new Dictionary<string, int>(41)
				{
					{ "1.2.840.10045.4.3.1", 0 },
					{ "1.2.840.10045.4.3.2", 1 },
					{ "1.2.840.10045.3.1.7", 2 },
					{ "ecdsa-sha2-nistp256", 3 },
					{ "ecdh-sha2-nistp256", 4 },
					{ "nistp256", 5 },
					{ "1.2.840.10045.4.3.3", 6 },
					{ "1.3.132.0.34", 7 },
					{ "ecdsa-sha2-nistp384", 8 },
					{ "ecdh-sha2-nistp384", 9 },
					{ "nistp384", 10 },
					{ "1.2.840.10045.4.3.4", 11 },
					{ "1.3.132.0.35", 12 },
					{ "ecdsa-sha2-nistp521", 13 },
					{ "ecdh-sha2-nistp521", 14 },
					{ "nistp521", 15 },
					{ "1.3.36.3.3.2.8.1.1.7", 16 },
					{ "ecdsa-sha2-brainpoolp256r1", 17 },
					{ "ecdh-sha2-brainpoolp256r1", 18 },
					{ "brainpoolp256r1", 19 },
					{ "1.3.36.3.3.2.8.1.1.11", 20 },
					{ "ecdsa-sha2-brainpoolp384r1", 21 },
					{ "ecdh-sha2-brainpoolp384r1", 22 },
					{ "brainpoolp384r1", 23 },
					{ "1.3.36.3.3.2.8.1.1.13", 24 },
					{ "ecdsa-sha2-brainpoolp512r1", 25 },
					{ "ecdh-sha2-brainpoolp512r1", 26 },
					{ "brainpoolp512r1", 27 },
					{ "1.3.132.0.10", 28 },
					{ "ecdsa-sha2-secp256k1", 29 },
					{ "ecdh-sha2-secp256k1", 30 },
					{ "secp256k1", 31 },
					{ "1.3.101.112", 32 },
					{ "ssh-ed25519", 33 },
					{ "ed25519-sha512", 34 },
					{ "ed25519", 35 },
					{ "1.3.101.110", 36 },
					{ "ecdh-sha2-curve25519", 37 },
					{ "curve25519-sha256", 38 },
					{ "x25519", 39 },
					{ "curve25519", 40 }
				};
			}
			if (fnfqw.ntzrd.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					return "1.2.840.10045.3.1.7";
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					return "1.3.132.0.34";
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
					return "1.3.132.0.35";
				case 16:
				case 17:
				case 18:
				case 19:
					return "1.3.36.3.3.2.8.1.1.7";
				case 20:
				case 21:
				case 22:
				case 23:
					return "1.3.36.3.3.2.8.1.1.11";
				case 24:
				case 25:
				case 26:
				case 27:
					return "1.3.36.3.3.2.8.1.1.13";
				case 28:
				case 29:
				case 30:
				case 31:
					return "1.3.132.0.10";
				case 32:
				case 33:
				case 34:
				case 35:
					return "1.3.101.112";
				case 36:
				case 37:
				case 38:
				case 39:
				case 40:
					return "1.3.101.110";
				}
			}
		}
		return null;
	}

	public static string kvrab(string p0)
	{
		string key;
		if ((key = p0.ToLower(dahxy.ldled)) != null && 0 == 0)
		{
			if (fnfqw.evulc == null || 1 == 0)
			{
				fnfqw.evulc = new Dictionary<string, int>(38)
				{
					{ "1.2.840.10045.4.3.1", 0 },
					{ "1.2.840.10045.4.3.2", 1 },
					{ "1.2.840.10045.3.1.7", 2 },
					{ "ecdsa-sha2-nistp256", 3 },
					{ "nistp256", 4 },
					{ "1.2.840.10045.4.3.3", 5 },
					{ "1.3.132.0.34", 6 },
					{ "ecdsa-sha2-nistp384", 7 },
					{ "nistp384", 8 },
					{ "1.2.840.10045.4.3.4", 9 },
					{ "1.3.132.0.35", 10 },
					{ "ecdsa-sha2-nistp521", 11 },
					{ "nistp521", 12 },
					{ "1.3.36.3.3.2.8.1.1.7", 13 },
					{ "ecdsa-sha2-brainpoolp256r1", 14 },
					{ "ecdh-sha2-brainpoolp256r1", 15 },
					{ "brainpoolp256r1", 16 },
					{ "1.3.36.3.3.2.8.1.1.11", 17 },
					{ "ecdsa-sha2-brainpoolp384r1", 18 },
					{ "ecdh-sha2-brainpoolp384r1", 19 },
					{ "brainpoolp384r1", 20 },
					{ "1.3.36.3.3.2.8.1.1.13", 21 },
					{ "ecdsa-sha2-brainpoolp512r1", 22 },
					{ "ecdh-sha2-brainpoolp512r1", 23 },
					{ "brainpoolp512r1", 24 },
					{ "1.3.101.112", 25 },
					{ "ssh-ed25519", 26 },
					{ "ed25519-sha512", 27 },
					{ "ed25519", 28 },
					{ "1.3.132.0.10", 29 },
					{ "ecdsa-sha2-secp256k1", 30 },
					{ "ecdh-sha2-secp256k1", 31 },
					{ "secp256k1", 32 },
					{ "1.3.101.110", 33 },
					{ "curve25519-sha256", 34 },
					{ "ecdh-sha2-curve25519", 35 },
					{ "x25519", 36 },
					{ "curve25519", 37 }
				};
			}
			if (fnfqw.evulc.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					return "nistp256";
				case 5:
				case 6:
				case 7:
				case 8:
					return "nistp384";
				case 9:
				case 10:
				case 11:
				case 12:
					return "nistp521";
				case 13:
				case 14:
				case 15:
				case 16:
					return "brainpoolp256r1";
				case 17:
				case 18:
				case 19:
				case 20:
					return "brainpoolp384r1";
				case 21:
				case 22:
				case 23:
				case 24:
					return "brainpoolp512r1";
				case 25:
				case 26:
				case 27:
				case 28:
					return "ed25519";
				case 29:
				case 30:
				case 31:
				case 32:
					return "secp256k1";
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
					return "x25519";
				}
			}
		}
		return null;
	}

	public static string wmvaf(AlgorithmIdentifier p0)
	{
		string text = cafoz(p0);
		if (text == null || 1 == 0)
		{
			return null;
		}
		return kvrab(text);
	}

	public static string cafoz(AlgorithmIdentifier p0)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		string value;
		if ((value = p0.Oid.Value) != null && 0 == 0)
		{
			if (value == "1.3.101.112")
			{
				return "1.3.101.112";
			}
			if (value == "1.3.101.110")
			{
				return "1.3.101.110";
			}
		}
		if (p0.edeag() != AsymmetricKeyAlgorithmId.ECDsa)
		{
			return null;
		}
		byte[] parameters = p0.Parameters;
		if (parameters == null || false || parameters.Length < 1 || parameters[0] != 6)
		{
			return null;
		}
		return wyjqw.ewqkw(parameters).scakm.Value;
	}

	public static SignatureHashAlgorithm gjkao(string p0)
	{
		p0 = mjwcm(p0);
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.hdvol == null || 1 == 0)
			{
				fnfqw.hdvol = new Dictionary<string, int>(8)
				{
					{ "1.2.840.10045.3.1.7", 0 },
					{ "1.3.36.3.3.2.8.1.1.7", 1 },
					{ "1.3.132.0.10", 2 },
					{ "1.3.132.0.34", 3 },
					{ "1.3.36.3.3.2.8.1.1.11", 4 },
					{ "1.3.132.0.35", 5 },
					{ "1.3.36.3.3.2.8.1.1.13", 6 },
					{ "1.3.101.112", 7 }
				};
			}
			if (fnfqw.hdvol.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
					return SignatureHashAlgorithm.SHA256;
				case 3:
				case 4:
					return SignatureHashAlgorithm.SHA384;
				case 5:
				case 6:
					return SignatureHashAlgorithm.SHA512;
				case 7:
					return SignatureHashAlgorithm.SHA512;
				}
			}
		}
		return SignatureHashAlgorithm.Unsupported;
	}

	public static HashingAlgorithmId pdzpf(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			return (HashingAlgorithmId)0;
		}
		string key;
		if ((key = p0.ToUpper(CultureInfo.InvariantCulture)) != null && 0 == 0)
		{
			if (fnfqw.xjsqb == null || 1 == 0)
			{
				fnfqw.xjsqb = new Dictionary<string, int>(22)
				{
					{ "CRC32", 0 },
					{ "1.3.14.3.2.26", 1 },
					{ "SHA1", 2 },
					{ "SHA-1", 3 },
					{ "2.16.840.1.101.3.4.2.4", 4 },
					{ "SHA224", 5 },
					{ "SHA-224", 6 },
					{ "2.16.840.1.101.3.4.2.1", 7 },
					{ "SHA2", 8 },
					{ "SHA-2", 9 },
					{ "SHA256", 10 },
					{ "SHA-256", 11 },
					{ "2.16.840.1.101.3.4.2.2", 12 },
					{ "SHA384", 13 },
					{ "SHA-384", 14 },
					{ "2.16.840.1.101.3.4.2.3", 15 },
					{ "SHA512", 16 },
					{ "SHA-512", 17 },
					{ "1.2.840.113549.2.5", 18 },
					{ "MD5", 19 },
					{ "1.2.840.113549.2.4", 20 },
					{ "MD4", 21 }
				};
			}
			if (fnfqw.xjsqb.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					return (HashingAlgorithmId)9;
				case 1:
				case 2:
				case 3:
					return HashingAlgorithmId.SHA1;
				case 4:
				case 5:
				case 6:
					return HashingAlgorithmId.SHA224;
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					return HashingAlgorithmId.SHA256;
				case 12:
				case 13:
				case 14:
					return HashingAlgorithmId.SHA384;
				case 15:
				case 16:
				case 17:
					return HashingAlgorithmId.SHA512;
				case 18:
				case 19:
					return HashingAlgorithmId.MD5;
				case 20:
				case 21:
					return HashingAlgorithmId.MD4;
				}
			}
		}
		return (HashingAlgorithmId)0;
	}

	public static int jgpbh(SignatureHashAlgorithm p0)
	{
		if (p0 == SignatureHashAlgorithm.MD5SHA1)
		{
			return 36;
		}
		int? num = HashingAlgorithm.kfowy(wrqur(p0));
		if (num.HasValue && 0 == 0)
		{
			return num.Value;
		}
		throw hifyx.nztrs("alg", p0, "Unsupported hash algorithm.");
	}

	public static HashingAlgorithmId wrqur(SignatureHashAlgorithm p0)
	{
		return p0 switch
		{
			SignatureHashAlgorithm.SHA1 => HashingAlgorithmId.SHA1, 
			SignatureHashAlgorithm.SHA224 => HashingAlgorithmId.SHA224, 
			SignatureHashAlgorithm.SHA256 => HashingAlgorithmId.SHA256, 
			SignatureHashAlgorithm.SHA384 => HashingAlgorithmId.SHA384, 
			SignatureHashAlgorithm.SHA512 => HashingAlgorithmId.SHA512, 
			SignatureHashAlgorithm.MD5 => HashingAlgorithmId.MD5, 
			SignatureHashAlgorithm.MD4 => HashingAlgorithmId.MD4, 
			_ => (HashingAlgorithmId)0, 
		};
	}

	public static SignatureHashAlgorithm vfyof(HashingAlgorithmId p0)
	{
		return p0 switch
		{
			HashingAlgorithmId.SHA1 => SignatureHashAlgorithm.SHA1, 
			HashingAlgorithmId.SHA224 => SignatureHashAlgorithm.SHA224, 
			HashingAlgorithmId.SHA256 => SignatureHashAlgorithm.SHA256, 
			HashingAlgorithmId.SHA384 => SignatureHashAlgorithm.SHA384, 
			HashingAlgorithmId.SHA512 => SignatureHashAlgorithm.SHA512, 
			HashingAlgorithmId.MD5 => SignatureHashAlgorithm.MD5, 
			HashingAlgorithmId.MD4 => SignatureHashAlgorithm.MD4, 
			_ => SignatureHashAlgorithm.Unsupported, 
		};
	}

	public static string hdrmd(HashingAlgorithmId p0)
	{
		return p0 switch
		{
			HashingAlgorithmId.SHA1 => "SHA1", 
			HashingAlgorithmId.SHA224 => "SHA-224", 
			HashingAlgorithmId.SHA256 => "SHA-256", 
			HashingAlgorithmId.SHA384 => "SHA-384", 
			HashingAlgorithmId.SHA512 => "SHA-512", 
			HashingAlgorithmId.MD5 => "MD5", 
			HashingAlgorithmId.MD4 => "MD4", 
			_ => p0.ToString(), 
		};
	}

	public static string yifkw(SymmetricKeyAlgorithmId p0)
	{
		return p0 switch
		{
			SymmetricKeyAlgorithmId.ArcTwo => "RC2", 
			SymmetricKeyAlgorithmId.ArcFour => "RC4", 
			SymmetricKeyAlgorithmId.AES => "AES", 
			SymmetricKeyAlgorithmId.Blowfish => "Blowfish", 
			SymmetricKeyAlgorithmId.Twofish => "Twofish", 
			SymmetricKeyAlgorithmId.DES => "DES", 
			SymmetricKeyAlgorithmId.TripleDES => "3DES", 
			_ => p0.ToString(), 
		};
	}

	public static string xptdi(SignaturePaddingScheme p0)
	{
		return p0 switch
		{
			SignaturePaddingScheme.Pkcs1 => "RSASSA-PKCS1-v1_5", 
			SignaturePaddingScheme.Pss => "RSASSA-PSS", 
			_ => p0.ToString(), 
		};
	}

	public static string veeep(EncryptionPaddingScheme p0)
	{
		return p0 switch
		{
			EncryptionPaddingScheme.Pkcs1 => "RSAES-PKCS1-v1_5", 
			EncryptionPaddingScheme.Oaep => "RSAES-OAEP", 
			_ => p0.ToString(), 
		};
	}

	public static string alqlw(SignatureFormat p0)
	{
		return p0 switch
		{
			SignatureFormat.Pkcs => "PKCS #7", 
			SignatureFormat.Raw => "RAW", 
			_ => p0.ToString(), 
		};
	}

	public static string ueibh(HashingAlgorithmId p0)
	{
		return p0 switch
		{
			HashingAlgorithmId.SHA1 => "sha1", 
			HashingAlgorithmId.SHA224 => "sha224", 
			HashingAlgorithmId.SHA256 => "sha256", 
			HashingAlgorithmId.SHA384 => "sha384", 
			HashingAlgorithmId.SHA512 => "sha512", 
			HashingAlgorithmId.MD5 => "md5", 
			HashingAlgorithmId.MD4 => "md4", 
			(HashingAlgorithmId)9 => "crc32", 
			_ => null, 
		};
	}

	public static string bjusl(AsymmetricKeyAlgorithmId p0)
	{
		return p0 switch
		{
			AsymmetricKeyAlgorithmId.RSA => "RSA", 
			AsymmetricKeyAlgorithmId.DSA => "DSA", 
			AsymmetricKeyAlgorithmId.ECDsa => "ECDSA", 
			AsymmetricKeyAlgorithmId.EdDsa => "EdDSA", 
			_ => null, 
		};
	}

	public static string zyzdj(string p0)
	{
		return bjusl(aersr(p0));
	}

	public static AsymmetricKeyAlgorithmId aersr(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.khpvp == null || 1 == 0)
			{
				fnfqw.khpvp = new Dictionary<string, int>(16)
				{
					{ "1.2.840.113549.1.1.4", 0 },
					{ "1.2.840.113549.1.1.1", 1 },
					{ "1.2.840.113549.1.1.5", 2 },
					{ "1.2.840.113549.1.1.14", 3 },
					{ "1.2.840.113549.1.1.11", 4 },
					{ "1.2.840.113549.1.1.12", 5 },
					{ "1.2.840.113549.1.1.13", 6 },
					{ "1.2.840.10040.4.1", 7 },
					{ "1.2.840.10040.4.3", 8 },
					{ "1.2.840.10045.2.1", 9 },
					{ "1.2.840.10045.4.1", 10 },
					{ "1.2.840.10045.4.3.1", 11 },
					{ "1.2.840.10045.4.3.2", 12 },
					{ "1.2.840.10045.4.3.3", 13 },
					{ "1.2.840.10045.4.3.4", 14 },
					{ "1.3.101.112", 15 }
				};
			}
			if (fnfqw.khpvp.TryGetValue(key, out var value) && 0 == 0)
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
					return AsymmetricKeyAlgorithmId.RSA;
				case 7:
				case 8:
					return AsymmetricKeyAlgorithmId.DSA;
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
					return AsymmetricKeyAlgorithmId.ECDsa;
				case 15:
					return AsymmetricKeyAlgorithmId.EdDsa;
				}
			}
		}
		return (AsymmetricKeyAlgorithmId)(-1);
	}

	public static AsymmetricKeyAlgorithmId frvei(KeyAlgorithm p0, bool p1)
	{
		AsymmetricKeyAlgorithmId asymmetricKeyAlgorithmId = zjdcx(p0);
		if (asymmetricKeyAlgorithmId == (AsymmetricKeyAlgorithmId)(-1) && p1 && 0 == 0)
		{
			throw hifyx.nztrs("algorithm", p0, "Argument is out of range of valid values.");
		}
		return asymmetricKeyAlgorithmId;
	}

	public static AsymmetricKeyAlgorithmId zjdcx(KeyAlgorithm p0)
	{
		return p0 switch
		{
			KeyAlgorithm.RSA => AsymmetricKeyAlgorithmId.RSA, 
			KeyAlgorithm.DSA => AsymmetricKeyAlgorithmId.DSA, 
			KeyAlgorithm.ECDsa => AsymmetricKeyAlgorithmId.ECDsa, 
			KeyAlgorithm.ED25519 => AsymmetricKeyAlgorithmId.EdDsa, 
			_ => (AsymmetricKeyAlgorithmId)(-1), 
		};
	}

	public static int kgsco(string p0)
	{
		string key;
		if ((key = mjwcm(p0)) != null && 0 == 0)
		{
			if (fnfqw.wiuvo == null || 1 == 0)
			{
				fnfqw.wiuvo = new Dictionary<string, int>(9)
				{
					{ "1.3.36.3.3.2.8.1.1.7", 0 },
					{ "1.2.840.10045.3.1.7", 1 },
					{ "1.3.132.0.10", 2 },
					{ "1.3.36.3.3.2.8.1.1.11", 3 },
					{ "1.3.132.0.34", 4 },
					{ "1.3.36.3.3.2.8.1.1.13", 5 },
					{ "1.3.132.0.35", 6 },
					{ "1.3.101.112", 7 },
					{ "1.3.101.110", 8 }
				};
			}
			if (fnfqw.wiuvo.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
					return 256;
				case 3:
				case 4:
					return 384;
				case 5:
					return 512;
				case 6:
					return 521;
				case 7:
				case 8:
					return 256;
				}
			}
		}
		return 0;
	}

	private static void kqdqs(string p0, out string p1, out string p2)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.wviha == null || 1 == 0)
			{
				fnfqw.wviha = new Dictionary<string, int>(18)
				{
					{ "ed25519-sha512", 0 },
					{ "ssh-ed25519", 1 },
					{ "ecdh-sha2-curve25519", 2 },
					{ "curve25519-sha256", 3 },
					{ "ecdh-sha2-nistp256", 4 },
					{ "ecdsa-sha2-nistp256", 5 },
					{ "ecdh-sha2-nistp384", 6 },
					{ "ecdsa-sha2-nistp384", 7 },
					{ "ecdh-sha2-nistp521", 8 },
					{ "ecdsa-sha2-nistp521", 9 },
					{ "ecdh-sha2-brainpoolp256r1", 10 },
					{ "ecdsa-sha2-brainpoolp256r1", 11 },
					{ "ecdh-sha2-brainpoolp384r1", 12 },
					{ "ecdsa-sha2-brainpoolp384r1", 13 },
					{ "ecdh-sha2-brainpoolp512r1", 14 },
					{ "ecdsa-sha2-brainpoolp512r1", 15 },
					{ "ecdsa-sha2-secp256k1", 16 },
					{ "ecdh-sha2-secp256k1", 17 }
				};
			}
			if (fnfqw.wviha.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
					p1 = "1.3.101.112";
					p2 = "1.3.101.112";
					return;
				case 2:
				case 3:
					p1 = "1.3.101.110";
					p2 = "1.3.101.110";
					return;
				case 4:
				case 5:
					p1 = "1.2.840.10045.2.1";
					p2 = "1.2.840.10045.3.1.7";
					return;
				case 6:
				case 7:
					p1 = "1.2.840.10045.2.1";
					p2 = "1.3.132.0.34";
					return;
				case 8:
				case 9:
					p1 = "1.2.840.10045.2.1";
					p2 = "1.3.132.0.35";
					return;
				case 10:
				case 11:
					p1 = "1.2.840.10045.2.1";
					p2 = "1.3.36.3.3.2.8.1.1.7";
					return;
				case 12:
				case 13:
					p1 = "1.2.840.10045.2.1";
					p2 = "1.3.36.3.3.2.8.1.1.11";
					return;
				case 14:
				case 15:
					p1 = "1.2.840.10045.2.1";
					p2 = "1.3.36.3.3.2.8.1.1.13";
					return;
				case 16:
				case 17:
					p1 = "1.2.840.10045.2.1";
					p2 = "1.3.132.0.10";
					return;
				}
			}
		}
		throw new CryptographicException("Unexpected key algorithm.");
	}

	public static string oiant(SymmetricKeyAlgorithmId p0, int? p1)
	{
		switch (p0)
		{
		case SymmetricKeyAlgorithmId.AES:
		{
			int valueOrDefault = p1.GetValueOrDefault();
			if (p1.HasValue)
			{
				switch (valueOrDefault)
				{
				case 128:
					return "2.16.840.1.101.3.4.1.2";
				case 192:
					return "2.16.840.1.101.3.4.1.22";
				case 256:
					return "2.16.840.1.101.3.4.1.42";
				}
				break;
			}
			return "2.16.840.1.101.3.4.1.42";
		}
		case SymmetricKeyAlgorithmId.TripleDES:
			return "1.2.840.113549.3.7";
		case SymmetricKeyAlgorithmId.Twofish:
			return "1.3.6.1.4.1.25258.3.3";
		case SymmetricKeyAlgorithmId.DES:
			return "1.3.14.3.2.7";
		case SymmetricKeyAlgorithmId.ArcTwo:
			return "1.2.840.113549.3.2";
		case SymmetricKeyAlgorithmId.ArcFour:
			return "1.2.840.113549.3.4";
		case SymmetricKeyAlgorithmId.Blowfish:
			return "1.3.6.1.4.1.3029.1.1.2";
		}
		return null;
	}

	public static SymmetricKeyAlgorithmId zqwip(string p0, out int p1)
	{
		p1 = 0;
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.cqqsh == null || 1 == 0)
			{
				fnfqw.cqqsh = new Dictionary<string, int>(9)
				{
					{ "2.16.840.1.101.3.4.1.2", 0 },
					{ "2.16.840.1.101.3.4.1.22", 1 },
					{ "2.16.840.1.101.3.4.1.42", 2 },
					{ "1.2.840.113549.3.7", 3 },
					{ "1.3.6.1.4.1.25258.3.3", 4 },
					{ "1.3.14.3.2.7", 5 },
					{ "1.2.840.113549.3.2", 6 },
					{ "1.2.840.113549.3.4", 7 },
					{ "1.3.6.1.4.1.3029.1.1.2", 8 }
				};
			}
			if (fnfqw.cqqsh.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					p1 = 128;
					return SymmetricKeyAlgorithmId.AES;
				case 1:
					p1 = 192;
					return SymmetricKeyAlgorithmId.AES;
				case 2:
					p1 = 256;
					return SymmetricKeyAlgorithmId.AES;
				case 3:
					return SymmetricKeyAlgorithmId.TripleDES;
				case 4:
					return SymmetricKeyAlgorithmId.Twofish;
				case 5:
					return SymmetricKeyAlgorithmId.DES;
				case 6:
					return SymmetricKeyAlgorithmId.ArcTwo;
				case 7:
					return SymmetricKeyAlgorithmId.ArcFour;
				case 8:
					return SymmetricKeyAlgorithmId.Blowfish;
				}
			}
		}
		return (SymmetricKeyAlgorithmId)(-1);
	}
}
