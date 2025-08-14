using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Security.Cryptography;

public class RSAManaged : RSA, eatps, dzjkq, ntowq, lncnv
{
	private class czkjw : imfrk
	{
		public eatps xaunu(PrivateKeyInfo p0)
		{
			RSAParameters rSAParameters = p0.GetRSAParameters();
			RSAManaged rSAManaged = new RSAManaged();
			rSAManaged.ImportParameters(rSAParameters);
			return rSAManaged;
		}

		public eatps neqkn(PublicKeyInfo p0)
		{
			RSAParameters rSAParameters = p0.GetRSAParameters();
			RSAManaged rSAManaged = new RSAManaged();
			rSAManaged.ImportParameters(rSAParameters);
			return rSAManaged;
		}

		public eatps poerm()
		{
			throw new NotSupportedException("Key generation is not supported for managed RSA.");
		}
	}

	private const int rfucc = 256;

	private const int oygyp = 16384;

	private bdjih tgblv;

	private bdjih msiki;

	private bdjih idtjw;

	private bdjih tqgdi;

	private bdjih vnzys;

	private bdjih dhixf;

	private bdjih yjiwz;

	private bdjih nxnes;

	public override string SignatureAlgorithm => "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

	public override string KeyExchangeAlgorithm => "RSA-PKCS1-KeyEx";

	private string yhrsr => "rsa" + KeySizeValue;

	private AsymmetricKeyAlgorithmId sfgrh => AsymmetricKeyAlgorithmId.RSA;

	public RSAManaged()
		: this(1024, skipFipsCheck: false)
	{
	}

	public RSAManaged(int keySize)
		: this(keySize, skipFipsCheck: false)
	{
	}

	internal RSAManaged(bool skipFipsCheck)
		: this(1024, skipFipsCheck)
	{
	}

	private RSAManaged(int keySize, bool skipFipsCheck)
	{
		if ((!skipFipsCheck || 1 == 0) && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0 && (object)GetType() == typeof(RSAManaged))
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
		if (keySize < 256 || keySize > 16384)
		{
			throw new ArgumentException("Invalid key size.", "keySize");
		}
		KeySizeValue = keySize;
		LegalKeySizesValue = new KeySizes[1]
		{
			new KeySizes(256, 16384, 1)
		};
	}

	private void eqxbs()
	{
		if (tgblv != null && 0 == 0)
		{
			return;
		}
		throw new CryptographicException("Key generation is not supported.");
	}

	private void vgykq()
	{
		if (tqgdi < vnzys && 0 == 0)
		{
			bdjih bdjih = tqgdi;
			tqgdi = vnzys;
			vnzys = bdjih;
		}
		bdjih bdjih2 = tqgdi - 1;
		bdjih bdjih3 = vnzys - 1;
		idtjw = msiki.rjdow(bdjih2 * bdjih3);
		dhixf = idtjw.fvbmy(bdjih2);
		yjiwz = idtjw.fvbmy(bdjih3);
		nxnes = vnzys.rjdow(tqgdi);
	}

	public override RSAParameters ExportParameters(bool includePrivateParameters)
	{
		eqxbs();
		RSAParameters result = new RSAParameters
		{
			Exponent = msiki.kskce(p0: false),
			Modulus = tgblv.kskce(p0: false)
		};
		if (includePrivateParameters && 0 == 0 && idtjw != null && 0 == 0)
		{
			result.D = idtjw.kskce(p0: false);
			if (tqgdi != null && 0 == 0 && vnzys != null && 0 == 0 && dhixf != null && 0 == 0 && yjiwz != null && 0 == 0 && nxnes != null && 0 == 0)
			{
				result.P = tqgdi.kskce(p0: false);
				result.Q = vnzys.kskce(p0: false);
				result.DP = dhixf.kskce(p0: false);
				result.DQ = yjiwz.kskce(p0: false);
				result.InverseQ = nxnes.kskce(p0: false);
			}
		}
		return result;
	}

	public override void ImportParameters(RSAParameters parameters)
	{
		if (parameters.Exponent == null || false || parameters.Modulus == null)
		{
			throw new CryptographicException("Required parameters are missing.");
		}
		msiki = bdjih.foxoi(parameters.Exponent);
		tgblv = bdjih.foxoi(parameters.Modulus);
		KeySizeValue = tgblv.jaioo();
		if (KeySizeValue < 256 || KeySizeValue > 16384)
		{
			throw new CryptographicException("Invalid key size ({0}).", KeySizeValue.ToString());
		}
		if (parameters.P != null && 0 == 0 && parameters.Q != null && 0 == 0)
		{
			tqgdi = bdjih.foxoi(parameters.P);
			vnzys = bdjih.foxoi(parameters.Q);
			if (parameters.D == null || false || parameters.DP == null || false || parameters.DQ == null || false || parameters.InverseQ == null)
			{
				vgykq();
				return;
			}
			idtjw = bdjih.foxoi(parameters.D);
			dhixf = bdjih.foxoi(parameters.DP);
			yjiwz = bdjih.foxoi(parameters.DQ);
			nxnes = bdjih.foxoi(parameters.InverseQ);
		}
		else
		{
			tqgdi = null;
			vnzys = null;
			dhixf = null;
			yjiwz = null;
			nxnes = null;
			if (parameters.D != null && 0 == 0)
			{
				idtjw = bdjih.foxoi(parameters.D);
			}
			else
			{
				idtjw = null;
			}
		}
	}

	private CspParameters esphg()
	{
		return null;
	}

	CspParameters lncnv.iqqfj()
	{
		//ILSpy generated this explicit interface implementation from .override directive in esphg
		return this.esphg();
	}

	private PublicKeyInfo xgpzw()
	{
		RSAParameters parameters = ExportParameters(includePrivateParameters: false);
		return new PublicKeyInfo(parameters);
	}

	PublicKeyInfo lncnv.kptoi()
	{
		//ILSpy generated this explicit interface implementation from .override directive in xgpzw
		return this.xgpzw();
	}

	private PrivateKeyInfo aypwd(bool p0)
	{
		RSAParameters parameters = ExportParameters(includePrivateParameters: true);
		return new PrivateKeyInfo(parameters);
	}

	PrivateKeyInfo lncnv.jbbgs(bool p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in aypwd
		return this.aypwd(p0);
	}

	private static string xmfqp(object p0)
	{
		if (p0 is byte[] array && 0 == 0)
		{
			switch (array.Length)
			{
			case 16:
				return "1.2.840.113549.2.5";
			case 20:
				return "1.3.14.3.2.26";
			case 36:
				return "MD5SHA1";
			case 28:
				return "2.16.840.1.101.3.4.2.4";
			case 32:
				return "2.16.840.1.101.3.4.2.1";
			case 48:
				return "2.16.840.1.101.3.4.2.2";
			case 64:
				return "2.16.840.1.101.3.4.2.3";
			}
		}
		else if (p0 is HashAlgorithm && 0 == 0)
		{
			if (p0 is MD5 && 0 == 0)
			{
				return "1.2.840.113549.2.5";
			}
			if (p0 is SHA1 && 0 == 0)
			{
				return "1.3.14.3.2.26";
			}
			if (p0 is MD5SHA1 && 0 == 0)
			{
				return "MD5SHA1";
			}
		}
		throw new CryptographicException("Unsupported hash algorithm.");
	}

	internal static ObjectIdentifier ncjdi(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.sarec == null || 1 == 0)
			{
				fnfqw.sarec = new Dictionary<string, int>(20)
				{
					{ "MD4", 0 },
					{ "1.2.840.113549.2.4", 1 },
					{ "MD5", 2 },
					{ "1.2.840.113549.2.5", 3 },
					{ "SHA1", 4 },
					{ "SHA-1", 5 },
					{ "1.3.14.3.2.26", 6 },
					{ "MD5SHA1", 7 },
					{ "SHA224", 8 },
					{ "SHA-224", 9 },
					{ "2.16.840.1.101.3.4.2.4", 10 },
					{ "SHA256", 11 },
					{ "SHA-256", 12 },
					{ "2.16.840.1.101.3.4.2.1", 13 },
					{ "SHA384", 14 },
					{ "SHA-384", 15 },
					{ "2.16.840.1.101.3.4.2.2", 16 },
					{ "SHA512", 17 },
					{ "SHA-512", 18 },
					{ "2.16.840.1.101.3.4.2.3", 19 }
				};
			}
			if (fnfqw.sarec.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
					return new ObjectIdentifier("1.2.840.113549.2.4");
				case 2:
				case 3:
					return new ObjectIdentifier("1.2.840.113549.2.5");
				case 4:
				case 5:
				case 6:
					return new ObjectIdentifier("1.3.14.3.2.26");
				case 7:
					return null;
				case 8:
				case 9:
				case 10:
					return new ObjectIdentifier("2.16.840.1.101.3.4.2.4");
				case 11:
				case 12:
				case 13:
					return new ObjectIdentifier("2.16.840.1.101.3.4.2.1");
				case 14:
				case 15:
				case 16:
					return new ObjectIdentifier("2.16.840.1.101.3.4.2.2");
				case 17:
				case 18:
				case 19:
					return new ObjectIdentifier("2.16.840.1.101.3.4.2.3");
				}
			}
		}
		throw new CryptographicException("Unsupported hash algorithm.");
	}

	internal static byte[] ffitw(byte[] p0, int p1, bool p2)
	{
		if (p0.Length < 10 || p0[0] != p1)
		{
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Bad data.");
			}
			return null;
		}
		int num = 9;
		if (num == 0)
		{
			goto IL_002e;
		}
		goto IL_0076;
		IL_002e:
		if (p1 == 1 && p0[num] != byte.MaxValue)
		{
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Bad data.");
			}
			return null;
		}
		num++;
		if (num >= p0.Length)
		{
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Bad data.");
			}
			return null;
		}
		goto IL_0076;
		IL_0076:
		if (p0[num] == 0 || 1 == 0)
		{
			byte[] array = new byte[p0.Length - num - 1];
			Array.Copy(p0, num + 1, array, 0, p0.Length - num - 1);
			return array;
		}
		goto IL_002e;
	}

	private static byte[] sxhkd(int p0, byte[] p1, int p2)
	{
		if (p1.Length > p0 - 11)
		{
			throw new ArgumentException("Length of data is bigger than allowed.", "rgb");
		}
		int num = p0 - 1;
		byte[] array = new byte[num];
		array[0] = (byte)p2;
		Array.Copy(p1, 0, array, array.Length - p1.Length, p1.Length);
		int num2 = num - 2 - p1.Length;
		byte[] array2;
		int num3;
		if (p2 == 1)
		{
			array2 = new byte[num2];
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_0058;
			}
			goto IL_0067;
		}
		array2 = CryptoHelper.aqljw(num2);
		goto IL_0075;
		IL_0067:
		if (num3 < num2)
		{
			goto IL_0058;
		}
		goto IL_0075;
		IL_0058:
		array2[num3] = byte.MaxValue;
		num3++;
		goto IL_0067;
		IL_0075:
		Array.Copy(array2, 0, array, 1, array2.Length);
		return array;
	}

	private bool gqkqg(jyamo p0)
	{
		switch (p0.vmeor)
		{
		case xdgzn.ctbmq:
		case xdgzn.bntzq:
			return true;
		default:
			return false;
		}
	}

	bool ntowq.knvjq(jyamo p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gqkqg
		return this.gqkqg(p0);
	}

	public override byte[] DecryptValue(byte[] rgb)
	{
		eqxbs();
		if (idtjw == null && 0 == 0)
		{
			throw new CryptographicException("Cannot decrypt without private key.");
		}
		if (nxnes != null && 0 == 0)
		{
			bdjih bdjih = bdjih.foxoi(rgb);
			bdjih bdjih2 = bdjih.hbhui(dhixf, tqgdi);
			bdjih bdjih3 = bdjih.hbhui(yjiwz, vnzys);
			bdjih bdjih4 = (nxnes * (bdjih2 - bdjih3)).fvbmy(tqgdi);
			bdjih bdjih5 = bdjih3 + bdjih4 * vnzys;
			return bdjih5.kskce(p0: false);
		}
		bdjih bdjih6 = bdjih.foxoi(rgb);
		bdjih bdjih7 = bdjih6.hbhui(idtjw, tgblv);
		return bdjih7.kskce(p0: false);
	}

	public override byte[] EncryptValue(byte[] rgb)
	{
		eqxbs();
		bdjih bdjih = bdjih.foxoi(rgb);
		bdjih bdjih2 = bdjih.hbhui(msiki, tgblv);
		return bdjih2.kskce(p0: false);
	}

	private byte[] rrovm(byte[] p0, jyamo p1)
	{
		return irbux(p0, p1);
	}

	byte[] ntowq.lhhds(byte[] p0, jyamo p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rrovm
		return this.rrovm(p0, p1);
	}

	private byte[] irbux(byte[] p0, jyamo p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgb");
		}
		yjfse(p1, out var p2);
		byte[] p3 = DecryptValue(p0);
		if (p2 && 0 == 0)
		{
			int num = (KeySizeValue + 7) / 8;
			HashingAlgorithm hashAlg = new HashingAlgorithm(p1.fbcyx);
			HashingAlgorithm hashAlg2 = new HashingAlgorithm(p1.bablj);
			iiyev iiyev = new iiyev(hashAlg, new meytk(hashAlg2));
			return iiyev.vkfkg(num, jlfbq.tykuz(p3, num), p1.blonh);
		}
		return ffitw(p3, 2, p2: true);
	}

	public byte[] Decrypt(byte[] rgb)
	{
		return irbux(rgb, jyamo.cjoyr);
	}

	private byte[] dmkqp(byte[] p0, jyamo p1)
	{
		return fzjrh(p0, p1);
	}

	byte[] ntowq.sfbms(byte[] p0, jyamo p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dmkqp
		return this.dmkqp(p0, p1);
	}

	private byte[] fzjrh(byte[] p0, jyamo p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgb");
		}
		yjfse(p1, out var p2);
		int num = (KeySizeValue + 7) / 8;
		byte[] rgb;
		if (p2 && 0 == 0)
		{
			HashingAlgorithm hashAlg = new HashingAlgorithm(p1.fbcyx);
			HashingAlgorithm hashAlg2 = new HashingAlgorithm(p1.bablj);
			iiyev iiyev = new iiyev(hashAlg, new meytk(hashAlg2));
			rgb = iiyev.qgeir(num, p0, 0, p0.Length, p1.blonh);
		}
		else
		{
			rgb = sxhkd(num, p0, 2);
		}
		byte[] p3 = EncryptValue(rgb);
		return jlfbq.tykuz(p3, num);
	}

	private static void yjfse(jyamo p0, out bool p1)
	{
		switch (p0.vmeor)
		{
		case xdgzn.bntzq:
			p1 = true;
			break;
		case xdgzn.ctbmq:
			p1 = false;
			break;
		default:
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		}
	}

	public byte[] Encrypt(byte[] rgb)
	{
		return fzjrh(rgb, jyamo.cjoyr);
	}

	private bool vvhle(mrxvh p0)
	{
		switch (p0.hqtwc)
		{
		case goies.lfkki:
		case goies.mrskp:
			return true;
		default:
			return false;
		}
	}

	bool dzjkq.vmedb(mrxvh p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vvhle
		return this.vvhle(p0);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This overload of VerifyHash method has been deprecated. Please use another overload instead.", false)]
	public bool VerifyHash(byte[] rgbHash, byte[] rgbSignature)
	{
		return VerifyHash(rgbHash, xmfqp(rgbHash), rgbSignature);
	}

	private bool exzsf(byte[] p0, byte[] p1, mrxvh p2)
	{
		return oboxi(p0, p1, p2);
	}

	bool dzjkq.cbzmp(byte[] p0, byte[] p1, mrxvh p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in exzsf
		return this.exzsf(p0, p1, p2);
	}

	private bool oboxi(byte[] p0, byte[] p1, mrxvh p2)
	{
		switch (p2.hqtwc)
		{
		case goies.lfkki:
		{
			string p3 = p2.faqqk.ToString();
			return isjim(this, p0, p3, p1);
		}
		case goies.mrskp:
		{
			HashingAlgorithmId algorithm = bpkgq.wrqur(p2.faqqk);
			HashingAlgorithmId algorithm2 = bpkgq.wrqur(p2.wqjdn);
			HashingAlgorithm hashAlg = new HashingAlgorithm(algorithm);
			HashingAlgorithm hashAlg2 = new HashingAlgorithm(algorithm2);
			xjmlr xjmlr = new xjmlr(hashAlg, new meytk(hashAlg2), p2.xvcnk);
			return xjmlr.onzqc(this, p0, p1);
		}
		default:
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method has been deprecated and should no longer be used.", false)]
	public static bool VerifyHash(RSA rsa, byte[] rgbHash, string algorithm, byte[] rgbSignature)
	{
		return isjim(rsa, rgbHash, algorithm, rgbSignature);
	}

	internal static bool isjim(RSA p0, byte[] p1, string p2, byte[] p3)
	{
		ObjectIdentifier objectIdentifier = ncjdi(p2);
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgbHash");
		}
		if (p1.Length > p0.KeySize / 8 - 11)
		{
			throw new ArgumentException("Length of hash is bigger than allowed.", "rgbHash");
		}
		if (p3 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgbSignature");
		}
		byte[] p4 = p0.EncryptValue(p3);
		p4 = ffitw(p4, 1, p2: false);
		if (p4 == null || 1 == 0)
		{
			return false;
		}
		byte[] p5;
		if (objectIdentifier == null || 1 == 0)
		{
			p5 = p4;
		}
		else
		{
			oztnv oztnv = new oztnv();
			hfnnn.qnzgo(oztnv, p4);
			if (objectIdentifier.Value != oztnv.gfhke.Value && 0 == 0)
			{
				return false;
			}
			p5 = oztnv.thcnt;
		}
		return zjcch.wduyr(p1, p5);
	}

	public bool VerifyHash(byte[] rgbHash, string algorithm, byte[] rgbSignature)
	{
		return isjim(this, rgbHash, algorithm, rgbSignature);
	}

	[wptwl(false)]
	[Obsolete("This overload of SignHash method has been deprecated. Please use another overload instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public byte[] SignHash(byte[] rgbHash)
	{
		return SignHash(rgbHash, xmfqp(rgbHash));
	}

	private byte[] zhzpt(byte[] p0, mrxvh p1)
	{
		return kqgln(p0, p1);
	}

	byte[] dzjkq.rypyi(byte[] p0, mrxvh p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zhzpt
		return this.zhzpt(p0, p1);
	}

	private byte[] kqgln(byte[] p0, mrxvh p1)
	{
		switch (p1.hqtwc)
		{
		case goies.lfkki:
		{
			string p2 = p1.faqqk.ToString();
			return owihy(this, p0, p2);
		}
		case goies.mrskp:
		{
			HashingAlgorithmId algorithm = bpkgq.wrqur(p1.faqqk);
			HashingAlgorithmId algorithm2 = bpkgq.wrqur(p1.wqjdn);
			HashingAlgorithm hashAlg = new HashingAlgorithm(algorithm);
			HashingAlgorithm hashAlg2 = new HashingAlgorithm(algorithm2);
			xjmlr xjmlr = new xjmlr(hashAlg, new meytk(hashAlg2), p1.xvcnk);
			return xjmlr.lnuvw(this, p0);
		}
		default:
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method has been deprecated and should no longer be used.", false)]
	[wptwl(false)]
	public static byte[] SignHash(RSA rsa, byte[] rgbHash, string algorithm)
	{
		return owihy(rsa, rgbHash, algorithm);
	}

	internal static byte[] owihy(RSA p0, byte[] p1, string p2)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgbHash");
		}
		ObjectIdentifier objectIdentifier = ncjdi(p2);
		byte[] p3;
		if (objectIdentifier == null || 1 == 0)
		{
			p3 = p1;
		}
		else
		{
			oztnv p4 = new oztnv(objectIdentifier, p1);
			p3 = fxakl.kncuz(p4);
		}
		p3 = sxhkd((p0.KeySize + 7) / 8, p3, 1);
		return p0.DecryptValue(p3);
	}

	public byte[] SignHash(byte[] rgbHash, string algorithm)
	{
		return owihy(this, rgbHash, algorithm);
	}

	public bool VerifyData(byte[] buffer, HashAlgorithm halg, byte[] signature)
	{
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		if (halg == null || 1 == 0)
		{
			throw new ArgumentNullException("halg");
		}
		if (signature == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		byte[] rgbHash = halg.ComputeHash(buffer);
		return VerifyHash(rgbHash, xmfqp(halg), signature);
	}

	public byte[] SignData(byte[] buffer, HashAlgorithm halg)
	{
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		if (halg == null || 1 == 0)
		{
			throw new ArgumentNullException("halg");
		}
		byte[] rgbHash = halg.ComputeHash(buffer);
		return SignHash(rgbHash, xmfqp(halg));
	}

	protected override void Dispose(bool disposing)
	{
	}

	internal static imfrk icbbo(string p0)
	{
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			return null;
		}
		if (!bpkgq.guhie(p0, out var p1, out var p2) || 1 == 0)
		{
			return null;
		}
		if (p1 != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			return null;
		}
		if (p2 < 256 || p2 > 16384)
		{
			return null;
		}
		return new czkjw();
	}
}
