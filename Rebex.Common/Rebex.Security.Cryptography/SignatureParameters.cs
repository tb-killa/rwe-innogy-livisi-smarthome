using System;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography;

public class SignatureParameters
{
	private HashingAlgorithmId pqmyx;

	private HashingAlgorithmId rlryo;

	private SignatureFormat lbprl;

	private SignaturePaddingScheme xxylq;

	private int? jsmai;

	private bool skyus;

	private HashingAlgorithmId yaxbs;

	private static Func<string, Exception> ngber;

	public HashingAlgorithmId HashAlgorithm
	{
		get
		{
			return pqmyx;
		}
		set
		{
			pqmyx = value;
			sqxle = (HashingAlgorithmId)0;
		}
	}

	internal HashingAlgorithmId sqxle
	{
		get
		{
			return rlryo;
		}
		set
		{
			rlryo = value;
		}
	}

	public SignatureFormat Format
	{
		get
		{
			return lbprl;
		}
		set
		{
			lbprl = value;
		}
	}

	public SignaturePaddingScheme PaddingScheme
	{
		get
		{
			return xxylq;
		}
		set
		{
			xxylq = value;
		}
	}

	public int? SaltLength
	{
		get
		{
			return jsmai;
		}
		set
		{
			jsmai = value;
		}
	}

	public bool Silent
	{
		get
		{
			return skyus;
		}
		set
		{
			skyus = value;
		}
	}

	internal HashingAlgorithmId aogyv
	{
		get
		{
			return yaxbs;
		}
		set
		{
			yaxbs = value;
		}
	}

	public SignatureParameters()
	{
		Silent = true;
		aogyv = HashingAlgorithmId.SHA1;
	}

	internal static SignatureParameters preyi(SignatureHashAlgorithm p0, string p1)
	{
		HashingAlgorithmId hashingAlgorithmId = bpkgq.wrqur(p0);
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
		{
			throw new ArgumentException("Unsupported hash algorithm.", p1);
		}
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.Format = SignatureFormat.Pkcs;
		signatureParameters.HashAlgorithm = hashingAlgorithmId;
		return signatureParameters;
	}

	internal SignatureFormat ppgdd()
	{
		SignatureFormat format = Format;
		switch (format)
		{
		case (SignatureFormat)0:
		case SignatureFormat.Raw:
		case SignatureFormat.Pkcs:
			return format;
		default:
			throw new InvalidOperationException(string.Concat("Unknown signature format '", format, "'."));
		}
	}

	internal static void wdmbv(SignatureParameters p0, AsymmetricKeyAlgorithmId p1, int p2, Func<string, Exception> p3)
	{
		if ((p1 == AsymmetricKeyAlgorithmId.RSA || 1 == 0) && p0 != null && 0 == 0 && p0.PaddingScheme == SignaturePaddingScheme.Pss)
		{
			HashingAlgorithmId hashAlgorithm = p0.HashAlgorithm;
			if (hashAlgorithm == (HashingAlgorithmId)0 || 1 == 0)
			{
				hashAlgorithm = p0.aogyv;
			}
			int? num = HashingAlgorithm.kfowy(hashAlgorithm);
			if (!num.HasValue || 1 == 0)
			{
				throw new CryptographicException("Unsupported hash algorithm.");
			}
			int? saltLength = p0.SaltLength;
			int num2 = ((saltLength.HasValue ? true : false) ? saltLength.GetValueOrDefault() : num.Value);
			int num3 = (p2 + 7) / 8;
			if (num3 < num.Value + num2 + 2)
			{
				throw p3("Key is too short for signing specified message using PSS/" + bpkgq.hdrmd(hashAlgorithm) + ".");
			}
		}
	}

	internal static void ffnfz(SignatureParameters p0, AsymmetricKeyAlgorithmId p1, int p2, out SignatureFormat p3, out HashingAlgorithmId p4, out mrxvh p5)
	{
		HashingAlgorithmId hashingAlgorithmId = (HashingAlgorithmId)0;
		HashingAlgorithmId hashingAlgorithmId2;
		int? num;
		goies goies;
		if (p0 == null || 1 == 0)
		{
			p3 = (SignatureFormat)0;
			hashingAlgorithmId2 = (HashingAlgorithmId)0;
			num = null;
			goies = ((p1 == AsymmetricKeyAlgorithmId.RSA) ? goies.lfkki : goies.gbwxv);
		}
		else
		{
			p3 = p0.ppgdd();
			hashingAlgorithmId2 = p0.HashAlgorithm;
			num = p0.SaltLength;
			if (hashingAlgorithmId2 != 0 && 0 == 0)
			{
				switch (p1)
				{
				case AsymmetricKeyAlgorithmId.DSA:
					if (hashingAlgorithmId2 != HashingAlgorithmId.SHA1)
					{
						throw new CryptographicException("Only SHA-1 is supported for DSA algorithm.");
					}
					break;
				case AsymmetricKeyAlgorithmId.EdDsa:
					if (hashingAlgorithmId2 != HashingAlgorithmId.SHA512)
					{
						throw new CryptographicException("Only SHA-512 is supported for EdDSA algorithm.");
					}
					break;
				}
			}
			if (p1 == AsymmetricKeyAlgorithmId.RSA || 1 == 0)
			{
				switch (p0.PaddingScheme)
				{
				case SignaturePaddingScheme.Default:
				case SignaturePaddingScheme.Pkcs1:
					goies = goies.lfkki;
					if (goies != 0)
					{
						break;
					}
					goto case SignaturePaddingScheme.Pss;
				case SignaturePaddingScheme.Pss:
					if (ngber == null || 1 == 0)
					{
						ngber = stphv;
					}
					wdmbv(p0, AsymmetricKeyAlgorithmId.RSA, p2, ngber);
					goies = goies.mrskp;
					hashingAlgorithmId = p0.sqxle;
					break;
				default:
					throw new CryptographicException("Specified padding scheme is not supported for RSA algorithm.");
				}
			}
			else
			{
				if (p0.PaddingScheme != SignaturePaddingScheme.Default && 0 == 0)
				{
					throw new CryptographicException("Specified padding scheme is only supported for RSA algorithm.");
				}
				goies = goies.gbwxv;
			}
		}
		if (hashingAlgorithmId2 == (HashingAlgorithmId)0 || 1 == 0)
		{
			hashingAlgorithmId2 = ((((p1 != AsymmetricKeyAlgorithmId.RSA) ? true : false) || p0 == null) ? bpkgq.mfcal(p1, p2) : p0.aogyv);
		}
		p4 = hashingAlgorithmId2;
		SignatureHashAlgorithm signatureHashAlgorithm = bpkgq.vfyof(hashingAlgorithmId2);
		if (signatureHashAlgorithm == SignatureHashAlgorithm.Unsupported)
		{
			throw new CryptographicException("Unsupported hash algorithm.");
		}
		SignatureHashAlgorithm signatureHashAlgorithm2 = SignatureHashAlgorithm.Unsupported;
		if (goies == goies.mrskp)
		{
			if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
			{
				signatureHashAlgorithm2 = signatureHashAlgorithm;
			}
			else
			{
				signatureHashAlgorithm2 = bpkgq.vfyof(hashingAlgorithmId);
				if (signatureHashAlgorithm2 == SignatureHashAlgorithm.Unsupported)
				{
					throw new CryptographicException("Unsupported hash algorithm.");
				}
			}
		}
		if (!num.HasValue || 1 == 0)
		{
			if (goies == goies.mrskp)
			{
				int? num2 = HashingAlgorithm.kfowy(hashingAlgorithmId2);
				num = ((num2.HasValue ? true : false) ? num2.GetValueOrDefault() : 0);
			}
			else
			{
				num = 0;
			}
		}
		p5 = new mrxvh(signatureHashAlgorithm, signatureHashAlgorithm2, goies, num.Value);
	}

	private static Exception stphv(string p0)
	{
		return new CryptographicException(p0);
	}
}
