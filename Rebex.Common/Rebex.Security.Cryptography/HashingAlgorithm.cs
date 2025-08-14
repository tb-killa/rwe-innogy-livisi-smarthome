using System;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class HashingAlgorithm : IDisposable
{
	private class mdfld : IHashTransform, IDisposable
	{
		private readonly HashAlgorithm bbydu;

		private readonly byte[] osnez;

		private int hoqcs => bbydu.HashSize;

		public mdfld(HashAlgorithm inner)
		{
			bbydu = inner;
			osnez = new byte[0];
		}

		private void dtsrr(byte[] p0, int p1, int p2)
		{
			bbydu.TransformBlock(p0, p1, p2, p0, p1);
		}

		void IHashTransform.Process(byte[] p0, int p1, int p2)
		{
			//ILSpy generated this explicit interface implementation from .override directive in dtsrr
			this.dtsrr(p0, p1, p2);
		}

		private byte[] gfhpl()
		{
			bbydu.TransformFinalBlock(osnez, 0, 0);
			return bbydu.Hash;
		}

		byte[] IHashTransform.GetHash()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gfhpl
			return this.gfhpl();
		}

		private void egigo()
		{
			bbydu.Initialize();
		}

		void IHashTransform.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in egigo
			this.egigo();
		}

		private void gbqfi()
		{
			bbydu.Clear();
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gbqfi
			this.gbqfi();
		}
	}

	internal const int mydmi = 65536;

	private HashingAlgorithmId mgxun;

	private bool lghcd;

	private HashingAlgorithmKeyMode ldacm;

	private byte[] xejlh;

	public HashingAlgorithmId Algorithm => mgxun;

	public int HashSize => 8 * (kfowy(mgxun) ?? 32);

	public HashingAlgorithmKeyMode KeyMode
	{
		get
		{
			return ldacm;
		}
		set
		{
			switch (value)
			{
			default:
				throw hifyx.nztrs("value", value, "Unsupported key mode.");
			case HashingAlgorithmKeyMode.None:
			case HashingAlgorithmKeyMode.HMAC:
				ldacm = value;
				break;
			}
		}
	}

	public byte[] GetKey()
	{
		return xejlh;
	}

	public void SetKey(byte[] key)
	{
		if (key == null || 1 == 0)
		{
			xejlh = null;
		}
		else
		{
			xejlh = (byte[])key.Clone();
		}
	}

	public HashingAlgorithm(HashingAlgorithmId algorithm)
	{
		lghcd = (algorithm & (HashingAlgorithmId)65536) != 0;
		mgxun = algorithm & (HashingAlgorithmId)65535;
		jwiqd(mgxun, lghcd);
	}

	internal HashingAlgorithm(HashingAlgorithmId algorithm, bool force)
		: this((force ? true : false) ? (algorithm | (HashingAlgorithmId)65536) : algorithm)
	{
	}

	internal static void jwiqd(HashingAlgorithmId p0, bool p1)
	{
		bool useFipsAlgorithmsOnly = CryptoHelper.UseFipsAlgorithmsOnly;
		switch (p0)
		{
		case HashingAlgorithmId.SHA224:
		case HashingAlgorithmId.MD4:
		case HashingAlgorithmId.MD5:
		case (HashingAlgorithmId)9:
			if (useFipsAlgorithmsOnly && 0 == 0 && (!p1 || 1 == 0))
			{
				throw new CryptographicException(string.Concat("Algorithm '", p0, "' is not allowed in FIPS mode."));
			}
			break;
		default:
			throw new CryptographicException("Algorithm not supported.");
		case HashingAlgorithmId.SHA1:
		case HashingAlgorithmId.SHA256:
		case HashingAlgorithmId.SHA384:
		case HashingAlgorithmId.SHA512:
			break;
		}
	}

	public static bool IsSupported(HashingAlgorithmId algorithm)
	{
		bool result = (algorithm & (HashingAlgorithmId)65536) != 0;
		algorithm &= (HashingAlgorithmId)65535;
		bool useFipsAlgorithmsOnly = CryptoHelper.UseFipsAlgorithmsOnly;
		switch (algorithm)
		{
		case HashingAlgorithmId.SHA224:
		case HashingAlgorithmId.MD4:
		case HashingAlgorithmId.MD5:
		case (HashingAlgorithmId)9:
			if (useFipsAlgorithmsOnly && 0 == 0)
			{
				return result;
			}
			return true;
		case HashingAlgorithmId.SHA1:
			return true;
		case HashingAlgorithmId.SHA256:
		case HashingAlgorithmId.SHA384:
		case HashingAlgorithmId.SHA512:
			return true;
		default:
			return false;
		}
	}

	internal static bool aczrm(HashingAlgorithmId p0, bool p1)
	{
		return IsSupported((p1 ? true : false) ? (p0 | (HashingAlgorithmId)65536) : p0);
	}

	internal static int? kfowy(HashingAlgorithmId p0)
	{
		switch (p0)
		{
		case HashingAlgorithmId.MD4:
		case HashingAlgorithmId.MD5:
			return 16;
		case HashingAlgorithmId.SHA1:
			return 20;
		case HashingAlgorithmId.SHA224:
			return 28;
		case HashingAlgorithmId.SHA256:
			return 32;
		case HashingAlgorithmId.SHA384:
			return 48;
		case HashingAlgorithmId.SHA512:
			return 64;
		default:
			return null;
		}
	}

	private static HashAlgorithm sewvv(HashingAlgorithmId p0, bool p1)
	{
		if (!pcmss.ftkxd || 1 == 0)
		{
			return new SHA2Managed(p0);
		}
		return new pcmss(p0);
	}

	public HashAlgorithm ToHashAlgorithm()
	{
		return cbwhw(p0: true);
	}

	internal HashAlgorithm cbwhw(bool p0)
	{
		HashAlgorithm hashAlgorithm = null;
		if (ldacm == HashingAlgorithmKeyMode.None || 1 == 0)
		{
			if (xejlh != null && 0 == 0)
			{
				throw new CryptographicException("Key not supported in this mode.");
			}
			hashAlgorithm = vtcmi(mgxun, lghcd);
		}
		else
		{
			if (ldacm != HashingAlgorithmKeyMode.HMAC)
			{
				throw new CryptographicException("Invalid mode.");
			}
			if ((xejlh == null || 1 == 0) && (!p0 || 1 == 0))
			{
				throw new CryptographicException("Key is required in this mode.");
			}
			if (hashAlgorithm == null || 1 == 0)
			{
				hashAlgorithm = new HMAC(vtcmi(mgxun, lghcd), xejlh);
			}
		}
		return hashAlgorithm;
	}

	public IHashTransform CreateTransform()
	{
		HashAlgorithm p = cbwhw(p0: false);
		return ytacd(p);
	}

	internal static HashAlgorithm vtcmi(HashingAlgorithmId p0, bool p1)
	{
		HashAlgorithm hashAlgorithm = null;
		switch (p0)
		{
		case HashingAlgorithmId.SHA1:
			hashAlgorithm = SHA1.Create();
			break;
		case HashingAlgorithmId.SHA256:
		case HashingAlgorithmId.SHA384:
		case HashingAlgorithmId.SHA512:
			hashAlgorithm = sewvv(p0, p1);
			break;
		}
		if (hashAlgorithm != null && 0 == 0)
		{
			return hashAlgorithm;
		}
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			if (!p1 || 1 == 0)
			{
				throw new CryptographicException(string.Concat("Algorithm '", p0, "' is not allowed in FIPS mode."));
			}
			switch (p0)
			{
			case HashingAlgorithmId.SHA224:
				return new SHA2Managed(p0);
			case HashingAlgorithmId.MD5:
				return new MD5Managed(skipFipsCheck: true);
			case HashingAlgorithmId.MD4:
				return new MD4Managed(skipFipsCheck: true);
			}
		}
		switch (p0)
		{
		case (HashingAlgorithmId)9:
			return new mecsr();
		case HashingAlgorithmId.SHA224:
			return new SHA2Managed(p0);
		case HashingAlgorithmId.MD4:
			return new MD4Managed();
		case HashingAlgorithmId.MD5:
			if (CryptoHelper.bwwly && 0 == 0)
			{
				return new MD5Managed(skipFipsCheck: true);
			}
			return MD5.Create();
		default:
			throw new CryptographicException("Invalid algorithm.");
		}
	}

	internal static IHashTransform ytacd(HashAlgorithm p0)
	{
		if (p0 is MD5Managed mD5Managed && 0 == 0)
		{
			return mD5Managed.uhvte();
		}
		if (p0 is MD4Managed mD4Managed && 0 == 0)
		{
			return mD4Managed.hkaul();
		}
		return new mdfld(p0);
	}

	public byte[] ComputeHash(byte[] input)
	{
		return ComputeHash(input, 0, input.Length);
	}

	public byte[] ComputeHash(byte[] input, int offset, int count)
	{
		IHashTransform hashTransform = CreateTransform();
		try
		{
			hashTransform.Process(input, offset, count);
			return hashTransform.GetHash();
		}
		finally
		{
			if (hashTransform != null && 0 == 0)
			{
				hashTransform.Dispose();
			}
		}
	}

	public static byte[] ComputeHash(HashingAlgorithmId algorithm, byte[] input)
	{
		return ComputeHash(algorithm, input, 0, input.Length);
	}

	public static byte[] ComputeHash(HashingAlgorithmId algorithm, byte[] input, int offset, int count)
	{
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(algorithm);
		try
		{
			return hashingAlgorithm.ComputeHash(input, offset, count);
		}
		finally
		{
			if (hashingAlgorithm != null && 0 == 0)
			{
				((IDisposable)hashingAlgorithm).Dispose();
			}
		}
	}

	public void Dispose()
	{
	}
}
