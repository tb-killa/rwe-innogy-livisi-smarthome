using System;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class SHA2Managed : HashAlgorithm
{
	private rysmc epode;

	private bool fnrnc;

	internal SHA2Managed(HashingAlgorithmId algorithm)
	{
		epode = rysmc.miwhl(algorithm);
		HashSizeValue = epode.HashSize;
		Initialize();
	}

	public new static SHA2Managed Create()
	{
		return new SHA256Managed();
	}

	public override void Initialize()
	{
		rysmc rysmc = epode;
		if (rysmc == null || 1 == 0)
		{
			throw new ObjectDisposedException(null, "Object was disposed.");
		}
		fnrnc = false;
		rysmc.Reset();
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		rysmc rysmc = epode;
		if (rysmc == null || 1 == 0)
		{
			throw new ObjectDisposedException(null, "Object was disposed.");
		}
		if (fnrnc && 0 == 0)
		{
			throw new CryptographicException("Cannot process more data, hash was already computed.");
		}
		rysmc.Process(array, ibStart, cbSize);
	}

	protected override byte[] HashFinal()
	{
		rysmc rysmc = epode;
		if (rysmc == null || 1 == 0)
		{
			throw new ObjectDisposedException(null, "Object was disposed.");
		}
		fnrnc = true;
		return rysmc.GetHash();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing ? true : false)
		{
			rysmc rysmc = epode;
			if (rysmc != null)
			{
				epode = null;
				rysmc.Dispose();
			}
		}
	}
}
