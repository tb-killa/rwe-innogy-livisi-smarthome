using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography;

public static class CryptoHelper
{
	private sealed class qisqe
	{
		public gajry gbpaf;

		public Func<byte[], int, int, byte[]> laecz(byte[] p0, byte[] p1, byte[] p2)
		{
			gbpaf.tglzc(p1, 0, p1.Length);
			gbpaf.yirig(p0);
			if (p2 != null && 0 == 0 && p2.Length > 0)
			{
				gbpaf.seoke(p2);
			}
			return vzfaf;
		}

		public byte[] vzfaf(byte[] p0, int p1, int p2)
		{
			byte[] array = new byte[p2];
			gbpaf.yajzn(p0, p1, p2, array, 0);
			return array;
		}
	}

	private const string xrehr = "CustomApiKey";

	private const string njtbp = "UseNonSilentTransfer";

	private const string kdmgv = "ForceAesCtr";

	private const string jyqji = "PreferLocalMachineStore";

	private const string hhkum = "__REBEX_AES_GCM_DECRYPTOR_FACTORY__";

	private const string brxhl = "IntrinsicsFunctionsEnabled";

	private const string rcrcu = "OldReinterpretCastEnabled";

	private static readonly object hrtbj = new object();

	private static readonly byte[] jkvzn = new byte[517]
	{
		48, 130, 2, 1, 48, 130, 1, 106, 160, 3,
		2, 1, 2, 2, 1, 1, 48, 13, 6, 9,
		42, 134, 72, 134, 247, 13, 1, 1, 11, 5,
		0, 48, 45, 49, 43, 48, 41, 6, 3, 85,
		4, 3, 30, 34, 0, 115, 0, 117, 0, 112,
		0, 112, 0, 111, 0, 114, 0, 116, 0, 64,
		0, 114, 0, 101, 0, 98, 0, 101, 0, 120,
		0, 46, 0, 110, 0, 101, 0, 116, 48, 30,
		23, 13, 49, 53, 49, 50, 51, 49, 50, 51,
		48, 48, 48, 48, 90, 23, 13, 53, 53, 49,
		50, 51, 48, 50, 51, 48, 48, 48, 48, 90,
		48, 45, 49, 43, 48, 41, 6, 3, 85, 4,
		3, 30, 34, 0, 115, 0, 117, 0, 112, 0,
		112, 0, 111, 0, 114, 0, 116, 0, 64, 0,
		114, 0, 101, 0, 98, 0, 101, 0, 120, 0,
		46, 0, 110, 0, 101, 0, 116, 48, 129, 159,
		48, 13, 6, 9, 42, 134, 72, 134, 247, 13,
		1, 1, 1, 5, 0, 3, 129, 141, 0, 48,
		129, 137, 2, 129, 129, 0, 214, 16, 226, 79,
		21, 7, 244, 132, 171, 151, 5, 63, 105, 67,
		85, 35, 10, 91, 215, 32, 62, 194, 165, 187,
		19, 3, 248, 247, 229, 87, 189, 165, 70, 227,
		134, 76, 85, 107, 55, 14, 169, 18, 234, 196,
		241, 166, 132, 89, 252, 133, 234, 227, 31, 14,
		184, 206, 50, 64, 82, 171, 123, 100, 206, 65,
		51, 232, 186, 74, 199, 92, 167, 32, 20, 244,
		210, 221, 10, 239, 0, 77, 58, 139, 51, 17,
		81, 232, 247, 143, 16, 32, 237, 22, 110, 121,
		144, 201, 193, 174, 116, 186, 78, 225, 153, 106,
		233, 204, 243, 55, 83, 247, 205, 236, 167, 132,
		239, 212, 175, 152, 3, 180, 237, 0, 130, 92,
		176, 144, 218, 223, 2, 3, 1, 0, 1, 163,
		49, 48, 47, 48, 14, 6, 3, 85, 29, 15,
		1, 1, 255, 4, 4, 3, 2, 7, 128, 48,
		29, 6, 3, 85, 29, 14, 4, 22, 4, 20,
		242, 66, 203, 115, 106, 246, 69, 5, 17, 84,
		63, 23, 40, 0, 103, 141, 230, 250, 176, 65,
		48, 13, 6, 9, 42, 134, 72, 134, 247, 13,
		1, 1, 11, 5, 0, 3, 129, 129, 0, 202,
		213, 15, 63, 117, 188, 53, 242, 116, 105, 113,
		6, 203, 62, 17, 79, 161, 156, 183, 154, 96,
		95, 154, 134, 150, 86, 253, 165, 72, 196, 232,
		137, 139, 93, 76, 53, 231, 15, 249, 43, 3,
		12, 196, 119, 243, 71, 85, 18, 186, 247, 137,
		24, 55, 229, 140, 153, 121, 166, 210, 220, 88,
		105, 150, 14, 38, 62, 125, 243, 79, 233, 143,
		46, 224, 198, 145, 92, 81, 80, 128, 39, 243,
		109, 163, 47, 152, 48, 107, 23, 57, 216, 232,
		117, 114, 29, 72, 147, 105, 147, 223, 237, 38,
		241, 160, 167, 208, 176, 179, 28, 38, 233, 226,
		222, 110, 11, 110, 255, 28, 43, 54, 230, 123,
		222, 160, 177, 24, 88, 113, 127
	};

	private static readonly byte[] ytqms = new byte[32]
	{
		225, 137, 3, 213, 239, 189, 215, 12, 165, 200,
		209, 98, 241, 109, 157, 248, 18, 157, 68, 219,
		192, 137, 189, 227, 123, 197, 103, 57, 185, 94,
		30, 25
	};

	private static readonly byte[] xnddr = new byte[128]
	{
		58, 220, 93, 187, 151, 124, 227, 128, 70, 182,
		71, 21, 195, 111, 196, 151, 248, 205, 23, 171,
		104, 123, 171, 182, 81, 116, 214, 233, 140, 35,
		62, 163, 53, 173, 67, 138, 45, 209, 244, 189,
		220, 229, 101, 224, 196, 227, 252, 191, 222, 23,
		109, 182, 197, 254, 121, 2, 205, 1, 38, 152,
		65, 54, 139, 173, 81, 24, 51, 80, 180, 70,
		48, 131, 36, 239, 76, 66, 217, 159, 36, 79,
		181, 46, 150, 64, 224, 130, 147, 143, 233, 114,
		133, 4, 238, 187, 58, 218, 113, 69, 80, 27,
		67, 95, 89, 168, 246, 177, 48, 112, 200, 102,
		192, 6, 224, 126, 235, 69, 197, 37, 98, 237,
		131, 13, 227, 72, 106, 226, 90, 222
	};

	private static bool? isfsf;

	private static bool? yovfh;

	private static bool? ztplh;

	private static RandomNumberGenerator muegx;

	private static readonly object vpfud = new object();

	private static bool? jiivl;

	private static string psbst;

	private static bool nwlqq;

	private static bool ewsmb;

	private static bool cboto;

	private static bool cfadw;

	private static Func<byte[], int, int, Func<byte[], byte[], byte[], Func<byte[], int, int, byte[]>>> jjfdt;

	internal static bool jspwz
	{
		get
		{
			if (!isfsf.HasValue || 1 == 0)
			{
				try
				{
					Certificate certificate = new Certificate(jkvzn);
					ValidationResult validationResult = CertificateChain.btnex(CertificateChain.fqjtz(), certificate, null, ValidationOptions.IgnoreTimeNotValid | ValidationOptions.AllowUnknownCa, null, p5: true);
					isfsf = validationResult.Valid;
					RSAParameters rSAParameters = certificate.GetRSAParameters();
					RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
					rSACryptoServiceProvider.ImportParameters(rSAParameters);
					ObjectIdentifier objectIdentifier = RSAManaged.ncjdi("SHA256");
					string value = objectIdentifier.Value;
					bool? flag = isfsf;
					isfsf = ((byte)(rSACryptoServiceProvider.VerifyHash(ytqms, value, xnddr) ? 1 : 0) != 0) & flag;
				}
				catch
				{
					isfsf = false;
				}
			}
			return isfsf.Value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public static bool ForceManagedAes
	{
		get
		{
			return nwlqq;
		}
		set
		{
			nwlqq = value;
		}
	}

	internal static bool bwwly
	{
		get
		{
			if (!yovfh.HasValue || 1 == 0)
			{
				try
				{
					if (dahxy.ucaou && 0 == 0)
					{
						new MD5CryptoServiceProvider().ComputeHash(new byte[1], 0, 1);
					}
					yovfh = false;
				}
				catch (InvalidOperationException)
				{
					yovfh = true;
				}
			}
			return yovfh.Value;
		}
	}

	public static bool UseFipsAlgorithmsOnly
	{
		get
		{
			if (!ztplh.HasValue || 1 == 0)
			{
				ztplh = bwwly;
			}
			return ztplh.Value;
		}
		set
		{
			ztplh = value;
		}
	}

	internal static bool wyrkl => false;

	internal static bool mdumc
	{
		get
		{
			return ewsmb;
		}
		set
		{
			ewsmb = value;
		}
	}

	internal static bool ebikb
	{
		get
		{
			return cboto;
		}
		set
		{
			cboto = value;
		}
	}

	internal static bool ibqug
	{
		get
		{
			return cfadw;
		}
		set
		{
			cfadw = value;
		}
	}

	internal static bool egtui(KeySizes p0, int p1)
	{
		if (p1 < p0.MinSize || p1 > p0.MaxSize)
		{
			return false;
		}
		int num = p1 - p0.MinSize;
		if (p0.SkipSize == 0 || 1 == 0)
		{
			return num == 0;
		}
		int num2 = num % p0.SkipSize;
		return num2 == 0;
	}

	internal static bool dtvom(KeySizes[] p0, int p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_001f;
		IL_0006:
		if (egtui(p0[num], p1) && 0 == 0)
		{
			return true;
		}
		num++;
		goto IL_001f;
		IL_001f:
		if (num < p0.Length)
		{
			goto IL_0006;
		}
		return false;
	}

	internal static void hzwkb(ref byte[] p0, ref byte[] p1, int p2, int p3, CipherMode p4, KeySizes[] p5)
	{
		if (p0 == null || 1 == 0)
		{
			p0 = GetRandomBytes(p2 / 8);
		}
		else if (!dtvom(p5, p0.Length * 8) || 1 == 0)
		{
			throw new CryptographicException("Invalid key size.");
		}
		if (p4 != CipherMode.ECB)
		{
			if (p1 == null || 1 == 0)
			{
				p1 = GetRandomBytes(p3 / 8);
			}
			else if (p1.Length * 8 != p3)
			{
				throw new CryptographicException("Invalid IV size.");
			}
		}
		else
		{
			p1 = null;
		}
	}

	internal static void pndgf(RandomNumberGenerator p0)
	{
		muegx = p0;
	}

	public static RandomNumberGenerator CreateRandomNumberGenerator()
	{
		RandomNumberGenerator randomNumberGenerator = muegx;
		if (randomNumberGenerator != null && 0 == 0)
		{
			return randomNumberGenerator;
		}
		return RandomNumberGenerator.Create();
	}

	public static byte[] GetRandomBytes(int count)
	{
		byte[] array = new byte[count];
		if (count > 0)
		{
			GetRandomBytes(array);
		}
		return array;
	}

	public static void GetRandomBytes(byte[] buffer)
	{
		CreateRandomNumberGenerator().GetBytes(buffer);
	}

	internal static byte[] aqljw(int p0)
	{
		byte[] array = new byte[p0];
		CreateRandomNumberGenerator().GetNonZeroBytes(array);
		return array;
	}

	public static byte[] DecodeSignature(byte[] encodedSignature, KeyAlgorithm keyAlgorithm)
	{
		return keyAlgorithm switch
		{
			KeyAlgorithm.RSA => encodedSignature, 
			KeyAlgorithm.DSA => lmjia.bhrbh(encodedSignature, 40), 
			_ => throw new CryptographicException("Unsupported signature algorithm."), 
		};
	}

	public static byte[] EncodeSignature(byte[] signature, KeyAlgorithm keyAlgorithm)
	{
		if (signature == null)
		{
			throw new ArgumentNullException("signature");
		}
		return keyAlgorithm switch
		{
			KeyAlgorithm.RSA => signature, 
			KeyAlgorithm.DSA => lmjia.xfocq(signature, 40), 
			_ => throw new CryptographicException("Unsupported signature algorithm."), 
		};
	}

	public static byte[] EncodeString(string value, Encoding encoding)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (encoding == null || 1 == 0)
		{
			throw new ArgumentNullException("encoding");
		}
		string webName = encoding.WebName;
		rmkkr rmkkr;
		if (webName.Equals("us-ascii", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			rmkkr = rmkkr.dzwiy;
			if (rmkkr != 0)
			{
				goto IL_007e;
			}
		}
		if (webName.Equals("utf-8", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			rmkkr = rmkkr.xiwym;
			if (rmkkr != 0)
			{
				goto IL_007e;
			}
		}
		throw new ArgumentException("Encoding not supported.", "encoding");
		IL_007e:
		byte[] bytes = encoding.GetBytes(value);
		vesyi p = new vesyi(rmkkr, bytes);
		return fxakl.kncuz(p);
	}

	internal static bool srzqu(AsymmetricAlgorithm p0)
	{
		if (p0 is RSACryptoServiceProvider rSACryptoServiceProvider && 0 == 0)
		{
			return rSACryptoServiceProvider.PublicOnly;
		}
		if (p0 is DSACryptoServiceProvider dSACryptoServiceProvider && 0 == 0)
		{
			return dSACryptoServiceProvider.PublicOnly;
		}
		return false;
	}

	internal static eatps qgafy(DiffieHellmanParameters p0, xtsej p1)
	{
		DiffieHellman diffieHellman = null;
		if (p0.P == null || 1 == 0)
		{
			throw new CryptographicException("Missing prime modulus.");
		}
		byte[] array = jlfbq.cnbay(p0.P);
		int num = array.Length * 8;
		string text = "Provider is unusable.";
		if (p1 == xtsej.dipgs || 1 == 0)
		{
			bool? flag;
			lock (vpfud)
			{
				flag = jiivl;
				if ((!flag.HasValue || 1 == 0) && ((dahxy.xzevd ? true : false) || !gmetq.kiqyt() || 1 == 0))
				{
					flag = (jiivl = false);
				}
			}
			int num2 = 0;
			bool? flag2 = flag;
			if ((((flag2 == true) ? true : false) || !flag2.HasValue) && 0 == 0)
			{
				int num3 = 0;
				if (p0.G != null && 0 == 0)
				{
					num3 = jlfbq.jlhar(p0.G);
				}
				bool flag3 = false;
				num2 = DiffieHellmanCryptoServiceProvider.mdswo();
				if (num2 == 0 || 1 == 0)
				{
					flag3 = true;
					text = "Unable to determine maximum supported key size.";
				}
				if (num > num2)
				{
					flag3 = true;
					text = "Maximum supported key size is " + num2 + ".";
				}
				else if (num % 64 != 0 && 0 == 0)
				{
					flag3 = true;
					text = "Key size not divisible by 64.";
				}
				else if ((array[0] & 0x80) == 0 || 1 == 0)
				{
					flag3 = true;
					text = "Key size not divisible by 8.";
				}
				else if (num3 > array.Length)
				{
					flag3 = true;
					text = "Unsupported group size.";
				}
				if (!flag3 || 1 == 0)
				{
					bool flag4 = false;
					try
					{
						diffieHellman = new DiffieHellmanCryptoServiceProvider();
						diffieHellman.ImportParameters(p0);
						flag = true;
						flag4 = true;
					}
					catch (CryptographicException ex)
					{
						flag = true;
						text = ex.Message;
					}
					catch (SecurityException)
					{
						flag = false;
					}
					if (flag.HasValue && 0 == 0)
					{
						lock (vpfud)
						{
							jiivl = flag;
						}
					}
					if (flag4 && 0 == 0)
					{
						return diffieHellman;
					}
				}
			}
		}
		if (UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Diffie-Hellman CSP not available or doesn't support this key, and managed Diffie-Hellman forbidden in FIPS-only mode.");
		}
		int num4 = DiffieHellmanManaged.lelko();
		if (num <= num4)
		{
			diffieHellman = rmnyn.hnofn();
			diffieHellman.ImportParameters(p0);
			return diffieHellman;
		}
		throw new CryptographicException("Diffie-Hellman key not supported (" + num + " bits). " + text);
	}

	internal static int ngetc()
	{
		return 4096;
	}

	internal static int jtnnl()
	{
		if (dahxy.xzevd && 0 == 0)
		{
			return 4096;
		}
		return DiffieHellmanCryptoServiceProvider.mdswo();
	}

	internal static bool viqaa(RSAParameters p0)
	{
		bool result = p0.D != null && 0 == 0 && p0.D.Length > 0;
		bool flag = p0.P != null && 0 == 0 && p0.P.Length > 0;
		bool flag2 = p0.Q != null && 0 == 0 && p0.Q.Length > 0;
		if (flag && 0 == 0 && flag2 && 0 == 0)
		{
			return result;
		}
		return false;
	}

	internal static bool xkcuh(DSAParameters p0)
	{
		if (p0.X != null && 0 == 0)
		{
			return p0.X.Length > 0;
		}
		return false;
	}

	internal static bool fsfdo(DiffieHellmanParameters p0)
	{
		if (p0.X != null && 0 == 0)
		{
			return p0.X.Length > 0;
		}
		return false;
	}

	private static bool grxoy(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.jrxjs == null || 1 == 0)
			{
				fnfqw.jrxjs = new Dictionary<string, int>(9)
				{
					{ "SHA256", 0 },
					{ "SHA-256", 1 },
					{ "2.16.840.1.101.3.4.2.1", 2 },
					{ "SHA384", 3 },
					{ "SHA-384", 4 },
					{ "2.16.840.1.101.3.4.2.2", 5 },
					{ "SHA512", 6 },
					{ "SHA-512", 7 },
					{ "2.16.840.1.101.3.4.2.3", 8 }
				};
			}
			if (fnfqw.jrxjs.TryGetValue(key, out var value) && 0 == 0)
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
				case 8:
					return true;
				}
			}
		}
		return false;
	}

	internal static bool ehtve(RSA p0, byte[] p1, string p2, byte[] p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("rsa");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		if (p3 == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		ObjectIdentifier objectIdentifier = RSAManaged.ncjdi(p2);
		if (objectIdentifier != null && 0 == 0)
		{
			p2 = objectIdentifier.Value;
		}
		bool flag = false;
		if (p0 is RSACryptoServiceProvider rSACryptoServiceProvider && 0 == 0)
		{
			p3 = tobtz(p0, p3);
			if (p2 != "MD5SHA1" && 0 == 0 && (!grxoy(p2) || false || jspwz))
			{
				try
				{
					return rSACryptoServiceProvider.VerifyHash(p1, p2, p3);
				}
				catch (CryptographicException)
				{
				}
			}
			flag = true;
		}
		if (flag && 0 == 0)
		{
			if (UseFipsAlgorithmsOnly && 0 == 0)
			{
				throw new CryptographicException("Unable to verify hash using RSA with " + p2 + " in FIPS-compliant mode.");
			}
			RSAParameters parameters = p0.ExportParameters(includePrivateParameters: false);
			RSAManaged rSAManaged = new RSAManaged();
			try
			{
				rSAManaged.ImportParameters(parameters);
				return rSAManaged.VerifyHash(p1, p2, p3);
			}
			finally
			{
				if (rSAManaged != null && 0 == 0)
				{
					((IDisposable)rSAManaged).Dispose();
				}
			}
		}
		return RSAManaged.isjim(p0, p1, p2, p3);
	}

	private static byte[] tobtz(RSA p0, byte[] p1)
	{
		int num = p0.KeySize / 8;
		if (p1.Length < num)
		{
			byte[] array = new byte[num];
			p1.CopyTo(array, num - p1.Length);
			p1 = array;
		}
		return p1;
	}

	internal static byte[] vjjer(RSA p0, byte[] p1, string p2)
	{
		int num = p0.KeySize / 8;
		int num2 = 16;
		byte[] array;
		do
		{
			array = tmdul(p0, p1, p2);
			if (array.Length >= num)
			{
				return array;
			}
			num2--;
		}
		while (num2 > 0);
		return tobtz(p0, array);
	}

	internal static byte[] tmdul(RSA p0, byte[] p1, string p2)
	{
		ObjectIdentifier objectIdentifier = RSAManaged.ncjdi(p2);
		if (objectIdentifier != null && 0 == 0)
		{
			p2 = objectIdentifier.Value;
		}
		Exception ex = null;
		bool flag = false;
		if (p0 is RSACryptoServiceProvider rSACryptoServiceProvider && 0 == 0)
		{
			if (p2 != "MD5SHA1" && 0 == 0)
			{
				if (!grxoy(p2) || false || jspwz)
				{
					try
					{
						return rSACryptoServiceProvider.SignHash(p1, p2);
					}
					catch (CryptographicException)
					{
					}
				}
				if (!rSACryptoServiceProvider.CspKeyContainerInfo.Exportable || 1 == 0)
				{
					throw new CryptographicException("Unable to sign hash using this RSACryptoServiceProvider.");
				}
			}
			flag = true;
		}
		if (flag && 0 == 0)
		{
			if (UseFipsAlgorithmsOnly && 0 == 0)
			{
				throw new CryptographicException("Unable to sign hash using RSA with " + p2 + " in FIPS-compliant mode.");
			}
			RSAParameters parameters;
			try
			{
				parameters = p0.ExportParameters(includePrivateParameters: true);
			}
			catch (CryptographicException ex3)
			{
				string message = "Unable to sign hash using RSA with " + p2 + " and unable to transfer the key to a more capable RSA provider.";
				object ex4 = ex;
				if (ex4 == null || 1 == 0)
				{
					ex4 = ex3;
				}
				throw new CryptographicException(message, (Exception)ex4);
			}
			RSAManaged rSAManaged = new RSAManaged();
			try
			{
				rSAManaged.ImportParameters(parameters);
				return rSAManaged.SignHash(p1, p2);
			}
			finally
			{
				if (rSAManaged != null && 0 == 0)
				{
					((IDisposable)rSAManaged).Dispose();
				}
			}
		}
		return RSAManaged.owihy(p0, p1, p2);
	}

	private static bool bulms(CryptographicException p0)
	{
		string text = psbst;
		if (text == null || 1 == 0)
		{
			CryptographicException ex = new CryptographicException(-2146893816);
			text = (psbst = ex.Message);
		}
		return p0.Message == text;
	}

	internal static byte[] zxtjl(Stream p0, int p1)
	{
		byte[] array;
		if (p0.CanSeek && 0 == 0)
		{
			if (p0.Length > p1)
			{
				throw new CryptographicException("File or stream is too long.");
			}
			array = new byte[(int)p0.Length];
			p0.Read(array, 0, array.Length);
		}
		else
		{
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				byte[] array2 = new byte[1024];
				for (int num = p0.Read(array2, 0, array2.Length); num > 0; num = p0.Read(array2, 0, array2.Length))
				{
					memoryStream.Write(array2, 0, num);
					if (memoryStream.Length > p1)
					{
						throw new CryptographicException("File or stream is too long.");
					}
				}
				array = new byte[(int)memoryStream.Length];
				memoryStream.Position = 0L;
				memoryStream.Read(array, 0, array.Length);
			}
			finally
			{
				if (memoryStream != null && 0 == 0)
				{
					((IDisposable)memoryStream).Dispose();
				}
			}
		}
		return array;
	}

	internal static Exception zdhth(string p0)
	{
		return new CryptographicException(p0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public static void SetOption(object instance, string name, object value)
	{
		string text;
		if ((instance == null || 1 == 0) && (text = name) != null && 0 == 0)
		{
			if (text == "CustomApiKey")
			{
				if (!(value is string) || 1 == 0)
				{
					throw new ArgumentException("Invalid argument.");
				}
				return;
			}
			if (text == "PreferLocalMachineStore")
			{
				if (value is bool && 0 == 0)
				{
					mdumc = (bool)value;
					return;
				}
				throw new ArgumentException("Invalid argument.");
			}
			if (text == "UseNonSilentTransfer")
			{
				if (value is bool && 0 == 0)
				{
					ebikb = (bool)value;
					return;
				}
				throw new ArgumentException("Invalid argument.");
			}
			if (text == "ForceAesCtr")
			{
				if (value is bool && 0 == 0)
				{
					ibqug = (bool)value;
					return;
				}
				throw new ArgumentException("Invalid argument.");
			}
			if (text == "IntrinsicsFunctionsEnabled")
			{
				return;
			}
			if (text == "OldReinterpretCastEnabled")
			{
				if (value is bool && 0 == 0)
				{
					bool iatkb = (bool)value;
					peekn.iatkb = iatkb;
					return;
				}
				throw new ArgumentException("Invalid argument.");
			}
		}
		if (!(instance is gnyvx gnyvx) || 1 == 0)
		{
			throw new InvalidOperationException("Unsupported option.");
		}
		gnyvx.vhvwu(name, value);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static object GetOption(object instance, string name)
	{
		string text;
		if ((instance == null || 1 == 0) && (text = name) != null && 0 == 0)
		{
			if (text == "PreferLocalMachineStore")
			{
				return mdumc;
			}
			if (text == "UseNonSilentTransfer")
			{
				return ebikb;
			}
			if (text == "ForceAesCtr")
			{
				return ibqug;
			}
			if (text == "__REBEX_AES_GCM_DECRYPTOR_FACTORY__")
			{
				return cfpgz();
			}
			if (text == "IntrinsicsFunctionsEnabled")
			{
				return false;
			}
			if (text == "OldReinterpretCastEnabled")
			{
				return peekn.iatkb;
			}
		}
		if (!(instance is gnyvx gnyvx) || 1 == 0)
		{
			throw new InvalidOperationException("Unsupported option.");
		}
		return gnyvx.jfzti(name);
	}

	private static Func<byte[], int, int, Func<byte[], byte[], byte[], Func<byte[], int, int, byte[]>>> cfpgz()
	{
		if (jjfdt == null || 1 == 0)
		{
			jjfdt = heefn;
		}
		return jjfdt;
	}

	private static Func<byte[], byte[], byte[], Func<byte[], int, int, byte[]>> heefn(byte[] p0, int p1, int p2)
	{
		qisqe qisqe = new qisqe();
		qisqe.gbpaf = wfcez.yrtwh("ManagedRebexAesGcm").smvfj(p0, prjlw.mrnmb);
		return qisqe.laecz;
	}
}
