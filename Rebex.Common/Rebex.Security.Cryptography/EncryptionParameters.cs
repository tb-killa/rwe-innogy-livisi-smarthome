using System;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class EncryptionParameters
{
	private HashingAlgorithmId eszzt;

	private EncryptionPaddingScheme uwttj;

	private HashingAlgorithmId cjcyn;

	private byte[] sjmhc;

	private bool qqyry;

	private HashingAlgorithmId wgxue;

	private static Func<string, Exception> wkvxf;

	public EncryptionPaddingScheme PaddingScheme
	{
		get
		{
			return uwttj;
		}
		set
		{
			uwttj = value;
		}
	}

	public HashingAlgorithmId HashAlgorithm
	{
		get
		{
			return eszzt;
		}
		set
		{
			eszzt = value;
			mciwu = (HashingAlgorithmId)0;
		}
	}

	internal HashingAlgorithmId mciwu
	{
		get
		{
			return cjcyn;
		}
		set
		{
			cjcyn = value;
		}
	}

	public byte[] Label
	{
		get
		{
			return sjmhc;
		}
		set
		{
			sjmhc = value;
		}
	}

	public bool Silent
	{
		get
		{
			return qqyry;
		}
		set
		{
			qqyry = value;
		}
	}

	internal HashingAlgorithmId mctrn
	{
		get
		{
			return wgxue;
		}
		set
		{
			wgxue = value;
		}
	}

	public EncryptionParameters()
	{
		PaddingScheme = EncryptionPaddingScheme.Pkcs1;
		Silent = true;
		mctrn = HashingAlgorithmId.SHA1;
	}

	internal static void radkb(EncryptionParameters p0, AsymmetricKeyAlgorithmId p1, int p2, int? p3, Func<string, Exception> p4)
	{
		if (p3.HasValue && 0 == 0 && (p1 == AsymmetricKeyAlgorithmId.RSA || 1 == 0) && p0 != null && 0 == 0 && p0.PaddingScheme == EncryptionPaddingScheme.Oaep)
		{
			HashingAlgorithmId hashAlgorithm = p0.HashAlgorithm;
			if (hashAlgorithm == (HashingAlgorithmId)0 || 1 == 0)
			{
				hashAlgorithm = p0.mctrn;
			}
			int? num = HashingAlgorithm.kfowy(hashAlgorithm);
			if (!num.HasValue || 1 == 0)
			{
				throw new CryptographicException("Unsupported hash algorithm.");
			}
			int num2 = (p2 + 7) / 8;
			int num3 = num2 - 2 * num.Value - 2;
			if (p3.Value > num3)
			{
				throw p4("Key is too short for encrypting specified message using OAEP/" + bpkgq.hdrmd(hashAlgorithm) + ".");
			}
		}
	}

	internal static void lkmtd(EncryptionParameters p0, AsymmetricKeyAlgorithmId p1, int p2, int? p3, out jyamo p4)
	{
		if (p0 == null || 1 == 0)
		{
			p4 = jyamo.cjoyr;
			return;
		}
		EncryptionPaddingScheme paddingScheme = p0.PaddingScheme;
		switch (paddingScheme)
		{
		case EncryptionPaddingScheme.Default:
		case EncryptionPaddingScheme.Pkcs1:
			p4 = jyamo.cjoyr;
			break;
		case EncryptionPaddingScheme.Oaep:
		{
			if (p1 != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
			{
				throw new CryptographicException("Specified padding scheme is only supported for RSA algorithm.");
			}
			HashingAlgorithmId hashAlgorithm = p0.HashAlgorithm;
			if (hashAlgorithm == (HashingAlgorithmId)0 || 1 == 0)
			{
				hashAlgorithm = p0.mctrn;
			}
			else if (bpkgq.ueibh(hashAlgorithm) == null || 1 == 0)
			{
				throw new CryptographicException("Unsupported hash algorithm.");
			}
			HashingAlgorithmId hashingAlgorithmId = p0.mciwu;
			if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
			{
				hashingAlgorithmId = hashAlgorithm;
			}
			else if (bpkgq.ueibh(hashingAlgorithmId) == null || 1 == 0)
			{
				throw new CryptographicException("Unsupported hash algorithm.");
			}
			if (wkvxf == null || 1 == 0)
			{
				wkvxf = xvchm;
			}
			radkb(p0, p1, p2, p3, wkvxf);
			p4 = new jyamo(xdgzn.bntzq, hashAlgorithm, hashingAlgorithmId, p0.Label);
			break;
		}
		default:
			throw new InvalidOperationException(string.Concat("Unknown encryption padding scheme '", paddingScheme, "'."));
		}
	}

	private static Exception xvchm(string p0)
	{
		return new CryptographicException(p0);
	}
}
