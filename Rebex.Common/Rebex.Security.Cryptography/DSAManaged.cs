using System;
using System.ComponentModel;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Security.Cryptography;

public class DSAManaged : DSA, eatps, dzjkq, lncnv
{
	private class itpxm : imfrk
	{
		public eatps xaunu(PrivateKeyInfo p0)
		{
			DSAParameters parameters = p0.fwtfa();
			DSAManaged dSAManaged = new DSAManaged();
			dSAManaged.ImportParameters(parameters);
			return dSAManaged;
		}

		public eatps neqkn(PublicKeyInfo p0)
		{
			DSAParameters dSAParameters = p0.GetDSAParameters();
			DSAManaged dSAManaged = new DSAManaged();
			dSAManaged.ImportParameters(dSAParameters);
			return dSAManaged;
		}

		public eatps poerm()
		{
			throw new NotSupportedException("Key generation is not supported for managed DSA.");
		}
	}

	internal const int zchnr = 512;

	internal const int vhlzm = 8192;

	private bdjih jgjbg;

	private bdjih seztw;

	private bdjih jdpyq;

	private bdjih syfgd;

	private int ofkny;

	private byte[] vlwtq;

	private bdjih zafni;

	private bdjih macun;

	public override string SignatureAlgorithm => "http://www.w3.org/2000/09/xmldsig#dsa-sha1";

	public override string KeyExchangeAlgorithm => null;

	private string qpaty => "dsa" + KeySizeValue;

	private AsymmetricKeyAlgorithmId lxptx => AsymmetricKeyAlgorithmId.DSA;

	public DSAManaged()
		: this(1024, null, skipFipsCheck: false)
	{
	}

	public DSAManaged(int keySize)
		: this(keySize, null, skipFipsCheck: false)
	{
	}

	public DSAManaged(int keySize, byte[] seed)
		: this(keySize, seed, skipFipsCheck: false)
	{
	}

	internal DSAManaged(bool skipFipsCheck)
		: this(1024, null, skipFipsCheck)
	{
	}

	private DSAManaged(int keySize, byte[] seed, bool skipFipsCheck)
	{
		if ((!skipFipsCheck || 1 == 0) && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
		vlwtq = seed;
		if (keySize < 512 || keySize > 8192)
		{
			throw new ArgumentException("Invalid key size.", "keySize");
		}
		KeySizeValue = keySize;
		LegalKeySizesValue = new KeySizes[1]
		{
			new KeySizes(512, 8192, 1)
		};
	}

	private static byte[] mryua(byte[] p0, int p1)
	{
		byte[] array = new byte[p0.Length];
		for (int num = p0.Length - 1; num >= 0; num--)
		{
			p1 += p0[num];
			array[num] = (byte)(p1 & 0xFF);
			p1 >>= 8;
		}
		return array;
	}

	[Obsolete("This method has been deprecated. Please use CryptoHelper.EncodeSignature instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public static byte[] EncodeSignatureToDer(byte[] signature)
	{
		return CryptoHelper.EncodeSignature(signature, KeyAlgorithm.DSA);
	}

	[Obsolete("This method has been deprecated. Please use CryptoHelper.DecodeSignature instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public static byte[] DecodeSignatureFromDer(byte[] encodedSignature)
	{
		return CryptoHelper.DecodeSignature(encodedSignature, KeyAlgorithm.DSA);
	}

	internal static DSAParameters busby(DSAParameters p0)
	{
		if (p0.P == null || 1 == 0)
		{
			throw new CryptographicException("Missing P parameters.");
		}
		if (p0.Q == null || 1 == 0)
		{
			throw new CryptographicException("Missing Q parameters.");
		}
		if (p0.G == null || 1 == 0)
		{
			throw new CryptographicException("Missing G parameters.");
		}
		int num = p0.P.Length;
		int num2 = p0.Q.Length;
		int num3 = p0.J.Length;
		p0.G = jlfbq.elzlr(p0.G, num, num, CryptoHelper.zdhth);
		if (p0.Y != null && 0 == 0)
		{
			p0.Y = jlfbq.elzlr(p0.Y, num, num, CryptoHelper.zdhth);
		}
		if (p0.X != null && 0 == 0)
		{
			p0.X = jlfbq.elzlr(p0.X, num2, num2, CryptoHelper.zdhth);
		}
		if (p0.J != null && 0 == 0)
		{
			p0.J = jlfbq.tykuz(p0.J, ((num3 + 3) | 3) - 3);
		}
		return p0;
	}

	public override DSAParameters ExportParameters(bool includePrivateParameters)
	{
		if (((jgjbg == null) ? true : false) || macun == null)
		{
			throw new CryptographicException("Key generation is not supported.");
		}
		DSAParameters p = new DSAParameters
		{
			P = jgjbg.kskce(p0: false),
			Q = seztw.kskce(p0: false),
			G = jdpyq.kskce(p0: false),
			J = syfgd.kskce(p0: false),
			Y = macun.kskce(p0: false)
		};
		if (includePrivateParameters && 0 == 0)
		{
			p.Counter = ofkny;
			if (vlwtq != null && 0 == 0)
			{
				p.Seed = (byte[])vlwtq.Clone();
			}
			p.X = zafni.kskce(p0: false);
		}
		return busby(p);
	}

	public override void ImportParameters(DSAParameters parameters)
	{
		if (parameters.P == null || false || parameters.Q == null || false || parameters.G == null || false || (parameters.Y == null && parameters.X == null))
		{
			throw new CryptographicException("Required parameters are missing.");
		}
		jgjbg = bdjih.foxoi(parameters.P);
		seztw = bdjih.foxoi(parameters.Q);
		jdpyq = bdjih.foxoi(parameters.G);
		ofkny = parameters.Counter;
		KeySizeValue = jgjbg.jaioo();
		if (KeySizeValue < 512 || KeySizeValue > 8192)
		{
			throw new CryptographicException("Invalid key size ({0}).", KeySizeValue.ToString());
		}
		if (parameters.J == null || false || parameters.J.Length == 0 || 1 == 0)
		{
			syfgd = (jgjbg - 1) / seztw;
		}
		else
		{
			syfgd = bdjih.foxoi(parameters.J);
		}
		if (parameters.X == null || false || parameters.X.Length == 0 || 1 == 0)
		{
			zafni = null;
		}
		else
		{
			zafni = bdjih.foxoi(parameters.X);
		}
		if (parameters.Seed == null || false || parameters.Seed.Length < 20)
		{
			vlwtq = null;
		}
		else
		{
			vlwtq = (byte[])parameters.Seed.Clone();
		}
		if (parameters.Y != null && 0 == 0)
		{
			macun = bdjih.foxoi(parameters.Y);
		}
		else
		{
			macun = jdpyq.hbhui(zafni, jgjbg);
		}
	}

	internal byte[] xchwk()
	{
		if (macun == null && 0 == 0)
		{
			throw new CryptographicException("Key generation is not supported.");
		}
		return macun.kskce(p0: false);
	}

	private CspParameters tecuf()
	{
		return null;
	}

	CspParameters lncnv.iqqfj()
	{
		//ILSpy generated this explicit interface implementation from .override directive in tecuf
		return this.tecuf();
	}

	private PublicKeyInfo xmjkr()
	{
		DSAParameters parameters = ExportParameters(includePrivateParameters: false);
		return new PublicKeyInfo(parameters);
	}

	PublicKeyInfo lncnv.kptoi()
	{
		//ILSpy generated this explicit interface implementation from .override directive in xmjkr
		return this.xmjkr();
	}

	private PrivateKeyInfo sbgxp(bool p0)
	{
		DSAParameters parameters = ExportParameters(includePrivateParameters: true);
		return new PrivateKeyInfo(parameters);
	}

	PrivateKeyInfo lncnv.jbbgs(bool p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sbgxp
		return this.sbgxp(p0);
	}

	public override byte[] CreateSignature(byte[] rgbHash)
	{
		if (rgbHash == null || 1 == 0)
		{
			throw new ArgumentNullException("rgbHash");
		}
		if (rgbHash.Length != 20)
		{
			throw new ArgumentException("Invalid SHA1 hash.");
		}
		bdjih bdjih = bdjih.foxoi(rgbHash);
		bdjih bdjih2 = bdjih.izifm(1, seztw - bdjih.uvfcb);
		bdjih bdjih3 = jdpyq.hbhui(bdjih2, jgjbg).fvbmy(seztw);
		bdjih bdjih4 = (bdjih2.rjdow(seztw) * (bdjih + zafni * bdjih3)).fvbmy(seztw);
		byte[] array = bdjih3.kskce(p0: false);
		byte[] array2 = bdjih4.kskce(p0: false);
		byte[] array3 = new byte[40];
		Array.Copy(array, 0, array3, 20 - array.Length, array.Length);
		Array.Copy(array2, 0, array3, 40 - array2.Length, array2.Length);
		return array3;
	}

	public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
	{
		if (rgbHash == null || 1 == 0)
		{
			throw new ArgumentNullException("rgbHash");
		}
		if (rgbSignature == null || 1 == 0)
		{
			throw new ArgumentNullException("rgbSignature");
		}
		if (rgbHash.Length != 20 || rgbSignature.Length != 40)
		{
			return false;
		}
		byte[] array = new byte[20];
		byte[] array2 = new byte[20];
		Array.Copy(rgbSignature, 0, array, 0, 20);
		Array.Copy(rgbSignature, 20, array2, 0, 20);
		bdjih bdjih = bdjih.foxoi(array);
		bdjih bdjih2 = bdjih.foxoi(array2);
		bdjih bdjih3 = bdjih.foxoi(rgbHash);
		bdjih bdjih4 = bdjih2.rjdow(seztw);
		bdjih p = (bdjih3 * bdjih4).fvbmy(seztw);
		bdjih p2 = (bdjih * bdjih4).fvbmy(seztw);
		p = jdpyq.hbhui(p, jgjbg);
		p2 = macun.hbhui(p2, jgjbg);
		bdjih bdjih5 = (p * p2).fvbmy(jgjbg).fvbmy(seztw);
		return bdjih5 == bdjih;
	}

	public byte[] SignHash(byte[] rgbHash)
	{
		return CreateSignature(rgbHash);
	}

	public bool VerifyHash(byte[] rgbHash, byte[] rgbSignature)
	{
		return VerifySignature(rgbHash, rgbSignature);
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
		return SignHash(rgbHash);
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
		byte[] rgbHash = halg.ComputeHash(buffer);
		return VerifyHash(rgbHash, signature);
	}

	private bool amzsy(mrxvh p0)
	{
		if (p0.faqqk != SignatureHashAlgorithm.SHA1)
		{
			return false;
		}
		if (p0.hqtwc != goies.gbwxv)
		{
			return false;
		}
		return true;
	}

	bool dzjkq.vmedb(mrxvh p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in amzsy
		return this.amzsy(p0);
	}

	private byte[] gcezt(byte[] p0, mrxvh p1)
	{
		return CreateSignature(p0);
	}

	byte[] dzjkq.rypyi(byte[] p0, mrxvh p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gcezt
		return this.gcezt(p0, p1);
	}

	private bool cofih(byte[] p0, byte[] p1, mrxvh p2)
	{
		if (p2.faqqk != SignatureHashAlgorithm.SHA1)
		{
			throw new CryptographicException("Unsupported hash algorithm.");
		}
		if (p2.hqtwc != goies.gbwxv)
		{
			throw new CryptographicException("Unsupported padding scheme.");
		}
		return VerifySignature(p0, p1);
	}

	bool dzjkq.cbzmp(byte[] p0, byte[] p1, mrxvh p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in cofih
		return this.cofih(p0, p1, p2);
	}

	protected override void Dispose(bool disposing)
	{
	}

	internal static imfrk vrnse(string p0)
	{
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			return null;
		}
		if (!bpkgq.guhie(p0, out var p1, out var p2) || 1 == 0)
		{
			return null;
		}
		if (p1 != AsymmetricKeyAlgorithmId.DSA)
		{
			return null;
		}
		if (p2 < 512 || p2 > 8192)
		{
			return null;
		}
		return new itpxm();
	}
}
