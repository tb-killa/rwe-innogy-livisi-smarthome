using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class SymmetricKeyAlgorithm : IDisposable
{
	internal const int pjkxy = 65536;

	private PaddingMode oebvu;

	private CipherMode tcxdk;

	private int icnmd;

	private int sctou;

	private int jlieq;

	private byte[] lmozh;

	private byte[] ggzxt;

	private readonly SymmetricKeyAlgorithmId pzuqs;

	private readonly bool moajz;

	public SymmetricKeyAlgorithmId Algorithm => pzuqs;

	public int BlockSize
	{
		get
		{
			return icnmd;
		}
		set
		{
			if (value < 8 || value > 1024 || (value & 7) != 0)
			{
				throw new ArgumentException("Invalid block size.", "value");
			}
			icnmd = value;
		}
	}

	public int KeySize
	{
		get
		{
			return sctou;
		}
		set
		{
			if (value < 8 || value > 1024 || (value & 7) != 0)
			{
				throw new ArgumentException("Invalid key size.", "value");
			}
			if (ggzxt != null && 0 == 0 && ggzxt.Length * 8 != value)
			{
				throw new CryptographicException("Desired key size doesn't match the size of the current key.");
			}
			sctou = value;
		}
	}

	public int EffectiveKeySize
	{
		get
		{
			return jlieq;
		}
		set
		{
			if (value < 0 || value > 1024 || (value & 7) != 0)
			{
				throw new ArgumentException("Invalid effective key size.", "value");
			}
			jlieq = value;
		}
	}

	public PaddingMode Padding
	{
		get
		{
			return oebvu;
		}
		set
		{
			switch (value)
			{
			default:
				throw hifyx.nztrs("value", value, "Unsupported padding mode.");
			case PaddingMode.None:
			case PaddingMode.PKCS7:
			case PaddingMode.Zeros:
				oebvu = value;
				break;
			}
		}
	}

	public CipherMode Mode
	{
		get
		{
			return tcxdk;
		}
		set
		{
			switch (value)
			{
			default:
				throw hifyx.nztrs("value", value, "Unsupported cipher mode.");
			case CipherMode.CBC:
			case CipherMode.ECB:
			case CipherMode.OFB:
			case CipherMode.CFB:
			case CipherMode.CTS:
				tcxdk = value;
				break;
			}
		}
	}

	internal string nwvkz
	{
		get
		{
			if (Mode != CipherMode.CBC)
			{
				throw new NotSupportedException("Cipher mode must be CBC.");
			}
			if (Algorithm == SymmetricKeyAlgorithmId.AES)
			{
				return brgjd.edcru("aes{0}-cbc", KeySize);
			}
			throw new NotSupportedException("This algorithm does not have SSH compatible name or is not supported yet.");
		}
	}

	internal static SymmetricKeyAlgorithm ciqvc(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (fnfqw.saomb == null || 1 == 0)
			{
				fnfqw.saomb = new Dictionary<string, int>(6)
				{
					{ "none", 0 },
					{ "aes128-cbc", 1 },
					{ "AES-128-CBC", 2 },
					{ "aes256-cbc", 3 },
					{ "AES-256-CBC", 4 },
					{ "DES-EDE3-CBC", 5 }
				};
			}
			if (fnfqw.saomb.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					return null;
				case 1:
				case 2:
				{
					SymmetricKeyAlgorithm symmetricKeyAlgorithm3 = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.AES);
					symmetricKeyAlgorithm3.KeySize = 128;
					symmetricKeyAlgorithm3.Padding = PaddingMode.None;
					symmetricKeyAlgorithm3.Mode = CipherMode.CBC;
					return symmetricKeyAlgorithm3;
				}
				case 3:
				case 4:
				{
					SymmetricKeyAlgorithm symmetricKeyAlgorithm2 = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.AES);
					symmetricKeyAlgorithm2.KeySize = 256;
					symmetricKeyAlgorithm2.Padding = PaddingMode.None;
					symmetricKeyAlgorithm2.Mode = CipherMode.CBC;
					return symmetricKeyAlgorithm2;
				}
				case 5:
				{
					SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.TripleDES);
					symmetricKeyAlgorithm.KeySize = 192;
					symmetricKeyAlgorithm.Padding = PaddingMode.None;
					symmetricKeyAlgorithm.Mode = CipherMode.CBC;
					return symmetricKeyAlgorithm;
				}
				}
			}
		}
		throw new CryptographicException(brgjd.edcru("Unsupported private key encryption: '{0}'.", p0));
	}

	public byte[] GetIV()
	{
		return lmozh;
	}

	public byte[] GetKey()
	{
		return ggzxt;
	}

	public void DeriveKey(DeriveBytes generator)
	{
		ggzxt = generator.GetBytes(KeySize / 8);
	}

	public void DeriveIV(DeriveBytes generator)
	{
		lmozh = generator.GetBytes(BlockSize / 8);
	}

	public void GenerateKey()
	{
		ggzxt = CryptoHelper.GetRandomBytes(KeySize / 8);
	}

	public void GenerateIV()
	{
		lmozh = CryptoHelper.GetRandomBytes(BlockSize / 8);
	}

	public void SetKey(byte[] key)
	{
		if (key == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (key.Length < 1 || key.Length > 128)
		{
			throw new ArgumentException("Invalid key size.", "key");
		}
		ggzxt = (byte[])key.Clone();
		sctou = key.Length * 8;
	}

	public void SetIV(byte[] iv)
	{
		lmozh = iv;
	}

	public ICryptoTransform CreateEncryptor()
	{
		return odfxu(p0: true);
	}

	public ICryptoTransform CreateDecryptor()
	{
		return odfxu(p0: false);
	}

	private ICryptoTransform odfxu(bool p0)
	{
		SymmetricAlgorithm symmetricAlgorithm = hwqsr();
		try
		{
			return (p0 ? true : false) ? symmetricAlgorithm.CreateEncryptor() : symmetricAlgorithm.CreateDecryptor();
		}
		finally
		{
			if (symmetricAlgorithm != null && 0 == 0)
			{
				((IDisposable)symmetricAlgorithm).Dispose();
			}
		}
	}

	private void wylbt()
	{
		if (ggzxt == null || 1 == 0)
		{
			throw new CryptographicException("Missing key.");
		}
		if (pzuqs == SymmetricKeyAlgorithmId.ArcFour)
		{
			return;
		}
		bool flag;
		switch (tcxdk)
		{
		case CipherMode.CBC:
			flag = true;
			if (flag)
			{
				break;
			}
			goto case CipherMode.ECB;
		case CipherMode.ECB:
			flag = false;
			if (!flag)
			{
				break;
			}
			goto default;
		default:
			throw new CryptographicException("Cipher mode is not supported.");
		}
		if (flag && 0 == 0)
		{
			if (lmozh == null || 1 == 0)
			{
				throw new CryptographicException("Missing IV.");
			}
			if (lmozh.Length * 8 != BlockSize)
			{
				throw new CryptographicException("IV length does not match the block size.");
			}
		}
		else if (lmozh != null && 0 == 0)
		{
			throw new CryptographicException("IV not supported in ECB mode.");
		}
	}

	internal SymmetricAlgorithm hwqsr()
	{
		wylbt();
		return ToSymmetricAlgorithm();
	}

	public SymmetricAlgorithm ToSymmetricAlgorithm()
	{
		SymmetricAlgorithm symmetricAlgorithm = null;
		SymmetricKeyAlgorithmId symmetricKeyAlgorithmId = pzuqs;
		if (symmetricKeyAlgorithmId == SymmetricKeyAlgorithmId.TripleDES)
		{
			symmetricAlgorithm = new TripleDESCryptoServiceProvider();
		}
		if (symmetricAlgorithm == null || 1 == 0)
		{
			if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0 && (!moajz || 1 == 0))
			{
				if (pzuqs == SymmetricKeyAlgorithmId.AES || 1 == 0)
				{
					throw new CryptographicException("FIPS-certified AES implementation is not available.");
				}
				throw new CryptographicException("Algorithm '" + bpkgq.yifkw(pzuqs) + "' is not allowed in FIPS mode.");
			}
			switch (pzuqs)
			{
			case SymmetricKeyAlgorithmId.AES:
				symmetricAlgorithm = new RijndaelManaged();
				break;
			case SymmetricKeyAlgorithmId.Twofish:
				symmetricAlgorithm = new TwofishManaged(moajz);
				break;
			case SymmetricKeyAlgorithmId.ArcTwo:
			{
				RC2 rC;
				if ((jlieq == 0 || false || jlieq == sctou) && (!CryptoHelper.bwwly || 1 == 0))
				{
					rC = RC2.Create();
				}
				else
				{
					rC = new ArcTwoManaged(moajz);
					rC.EffectiveKeySize = jlieq;
				}
				symmetricAlgorithm = rC;
				break;
			}
			case SymmetricKeyAlgorithmId.ArcFour:
				symmetricAlgorithm = new ArcFourManaged(moajz);
				break;
			case SymmetricKeyAlgorithmId.Blowfish:
				symmetricAlgorithm = new BlowfishManaged(moajz);
				break;
			case SymmetricKeyAlgorithmId.DES:
				symmetricAlgorithm = DES.Create();
				break;
			}
			if (symmetricAlgorithm == null || 1 == 0)
			{
				throw new CryptographicException("Invalid algorithm.");
			}
		}
		symmetricAlgorithm.BlockSize = icnmd;
		symmetricAlgorithm.KeySize = sctou;
		if (pzuqs != SymmetricKeyAlgorithmId.ArcFour)
		{
			symmetricAlgorithm.Mode = tcxdk;
			symmetricAlgorithm.Padding = oebvu;
		}
		if (ggzxt != null && 0 == 0)
		{
			symmetricAlgorithm.Key = ggzxt;
		}
		if (lmozh != null && 0 == 0)
		{
			symmetricAlgorithm.IV = lmozh;
		}
		return symmetricAlgorithm;
	}

	public void Dispose()
	{
	}

	public SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId algorithm)
	{
		moajz = (algorithm & (SymmetricKeyAlgorithmId)65536) != 0;
		pzuqs = algorithm & (SymmetricKeyAlgorithmId)65535;
		tcxdk = CipherMode.CBC;
		oebvu = PaddingMode.PKCS7;
		switch (pzuqs)
		{
		case SymmetricKeyAlgorithmId.AES:
			sctou = 256;
			icnmd = 128;
			break;
		case SymmetricKeyAlgorithmId.TripleDES:
			sctou = 192;
			icnmd = 64;
			break;
		case SymmetricKeyAlgorithmId.Twofish:
			sctou = 192;
			icnmd = 128;
			break;
		case SymmetricKeyAlgorithmId.DES:
			sctou = 64;
			icnmd = 64;
			break;
		case SymmetricKeyAlgorithmId.ArcTwo:
			sctou = 128;
			icnmd = 64;
			break;
		case SymmetricKeyAlgorithmId.ArcFour:
			sctou = 128;
			icnmd = 8;
			break;
		case SymmetricKeyAlgorithmId.Blowfish:
			sctou = 256;
			icnmd = 64;
			break;
		default:
			throw new CryptographicException("Algorithm not supported.");
		}
	}

	internal SymmetricKeyAlgorithm(string oid)
	{
		if (oid == null || 1 == 0)
		{
			throw new ArgumentNullException("oid");
		}
		oid = oid.ToUpper(CultureInfo.InvariantCulture);
		string key;
		if ((key = oid) != null && 0 == 0)
		{
			if (fnfqw.tcsda == null || 1 == 0)
			{
				fnfqw.tcsda = new Dictionary<string, int>(14)
				{
					{ "3DES", 0 },
					{ "TripleDES", 1 },
					{ "1.2.840.113549.3.7", 2 },
					{ "DES", 3 },
					{ "1.3.14.3.2.7", 4 },
					{ "AES", 5 },
					{ "AES128", 6 },
					{ "2.16.840.1.101.3.4.1.2", 7 },
					{ "AES192", 8 },
					{ "2.16.840.1.101.3.4.1.22", 9 },
					{ "AES256", 10 },
					{ "2.16.840.1.101.3.4.1.42", 11 },
					{ "RC2", 12 },
					{ "1.2.840.113549.3.2", 13 }
				};
			}
			if (fnfqw.tcsda.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
					pzuqs = SymmetricKeyAlgorithmId.TripleDES;
					sctou = 192;
					icnmd = 64;
					goto IL_0239;
				case 3:
				case 4:
					pzuqs = SymmetricKeyAlgorithmId.DES;
					sctou = 64;
					icnmd = 64;
					goto IL_0239;
				case 5:
				case 6:
				case 7:
					pzuqs = SymmetricKeyAlgorithmId.AES;
					sctou = 128;
					icnmd = 128;
					goto IL_0239;
				case 8:
				case 9:
					pzuqs = SymmetricKeyAlgorithmId.AES;
					sctou = 192;
					icnmd = 128;
					goto IL_0239;
				case 10:
				case 11:
					pzuqs = SymmetricKeyAlgorithmId.AES;
					sctou = 256;
					icnmd = 128;
					goto IL_0239;
				case 12:
				case 13:
					{
						pzuqs = SymmetricKeyAlgorithmId.ArcTwo;
						sctou = 128;
						icnmd = 64;
						goto IL_0239;
					}
					IL_0239:
					tcxdk = CipherMode.CBC;
					oebvu = PaddingMode.PKCS7;
					return;
				}
			}
		}
		throw new CryptographicException(brgjd.edcru("Unsupported encryption algorithm ({0}).", oid));
	}

	public static bool IsSupported(SymmetricKeyAlgorithmId algorithm)
	{
		bool flag = (algorithm & (SymmetricKeyAlgorithmId)65536) != 0;
		algorithm &= (SymmetricKeyAlgorithmId)65535;
		if (!CryptoHelper.UseFipsAlgorithmsOnly || false || flag)
		{
			switch (algorithm)
			{
			case SymmetricKeyAlgorithmId.DES:
				return !CryptoHelper.bwwly;
			case SymmetricKeyAlgorithmId.AES:
			case SymmetricKeyAlgorithmId.Twofish:
			case SymmetricKeyAlgorithmId.ArcTwo:
			case SymmetricKeyAlgorithmId.ArcFour:
			case SymmetricKeyAlgorithmId.Blowfish:
				return true;
			}
		}
		SymmetricKeyAlgorithmId symmetricKeyAlgorithmId = algorithm;
		if (symmetricKeyAlgorithmId == SymmetricKeyAlgorithmId.TripleDES)
		{
			return true;
		}
		return false;
	}
}
