using System;
using Elliptic;

namespace Rebex.Security.Cryptography;

public class Curve25519
{
	public const string Curve25519Sha256 = "curve25519-sha256";

	private byte[] _privateKey;

	public string Name => "curve25519-sha256";

	public static Curve25519 Create(string algorithmName)
	{
		if ("curve25519-sha256" == algorithmName)
		{
			return new Curve25519();
		}
		return null;
	}

	private void EnsurePrivateKey()
	{
		if (_privateKey == null)
		{
			_privateKey = Elliptic.Curve25519.CreateRandomPrivateKey();
		}
	}

	public byte[] GetPublicKey()
	{
		EnsurePrivateKey();
		return Elliptic.Curve25519.GetPublicKey(_privateKey);
	}

	public byte[] GetPrivateKey()
	{
		EnsurePrivateKey();
		return (byte[])_privateKey.Clone();
	}

	public void FromPublicKey(byte[] publicKey)
	{
		throw new NotSupportedException();
	}

	public void FromPrivateKey(byte[] privateKey)
	{
		if (privateKey == null)
		{
			throw new ArgumentNullException("privateKey");
		}
		_privateKey = (byte[])privateKey.Clone();
	}

	public byte[] GetSharedSecret(byte[] otherPublicKey)
	{
		if (otherPublicKey == null)
		{
			throw new ArgumentNullException("otherPublicKey");
		}
		EnsurePrivateKey();
		return Elliptic.Curve25519.GetSharedSecret(_privateKey, otherPublicKey);
	}
}
