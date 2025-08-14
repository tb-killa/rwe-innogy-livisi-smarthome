using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Rebex.Security.Cryptography;

public class EllipticCurveDsa : EllipticCurveAlgorithm
{
	public EllipticCurveDsa(string oid, string curveName)
		: base(oid, curveName)
	{
	}

	public byte[] SignHash(byte[] hash)
	{
		if (hash == null)
		{
			throw new ArgumentNullException("hash");
		}
		DsaDigestSigner signer = new DsaDigestSigner(new ECDsaSigner(), new NullDigest());
		return Sign(hash, signer);
	}

	public byte[] SignMessage(byte[] message)
	{
		if (message == null)
		{
			throw new ArgumentNullException("message");
		}
		ISigner signer = SignerUtilities.GetSigner(base.SignatureAlgorithm);
		return Sign(message, signer);
	}

	private byte[] Sign(byte[] data, ISigner signer)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		EnsurePrivate();
		signer.Init(forSigning: true, base.PrivateKey);
		signer.BlockUpdate(data, 0, data.Length);
		byte[] signature = signer.GenerateSignature();
		int keySize = (base.BitLength + 7) / 8;
		return DecodeSignature(signature, keySize);
	}

	public bool VerifyHash(byte[] hash, byte[] signature)
	{
		if (hash == null)
		{
			throw new ArgumentNullException("hash");
		}
		DsaDigestSigner signer = new DsaDigestSigner(new ECDsaSigner(), new NullDigest());
		return Verify(hash, signature, signer);
	}

	public bool VerifyMessage(byte[] message, byte[] signature)
	{
		if (message == null)
		{
			throw new ArgumentNullException("message");
		}
		ISigner signer = SignerUtilities.GetSigner(base.SignatureAlgorithm);
		return Verify(message, signature, signer);
	}

	private bool Verify(byte[] data, byte[] signature, ISigner signer)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (signature == null)
		{
			throw new ArgumentNullException("signature");
		}
		EnsurePublic();
		int num = (base.BitLength + 7) / 8;
		if (signature.Length != num * 2)
		{
			return false;
		}
		signature = EncodeSignature(signature, num);
		signer.Init(forSigning: false, base.PublicKey);
		signer.BlockUpdate(data, 0, data.Length);
		return signer.VerifySignature(signature);
	}

	private static byte[] DecodeSignature(byte[] signature, int keySize)
	{
		Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray(signature);
		Asn1SequenceParser parser = asn1Sequence.Parser;
		DerInteger derInteger = (DerInteger)parser.ReadObject();
		DerInteger derInteger2 = (DerInteger)parser.ReadObject();
		byte[] array = derInteger.Value.ToByteArrayUnsigned();
		byte[] array2 = derInteger2.Value.ToByteArrayUnsigned();
		if (array.Length > keySize || array2.Length > keySize)
		{
			throw new InvalidOperationException("Invalid ECDSA signature.");
		}
		signature = new byte[keySize * 2];
		array.CopyTo(signature, keySize - array.Length);
		array2.CopyTo(signature, keySize * 2 - array2.Length);
		return signature;
	}

	private static byte[] EncodeSignature(byte[] signature, int keySize)
	{
		byte[] array = new byte[keySize];
		byte[] array2 = new byte[keySize];
		Array.Copy(signature, 0, array, 0, keySize);
		Array.Copy(signature, keySize, array2, 0, keySize);
		DerInteger derInteger = new DerInteger(new BigInteger(1, array));
		DerInteger derInteger2 = new DerInteger(new BigInteger(1, array2));
		DerSequence derSequence = new DerSequence(derInteger, derInteger2);
		return derSequence.GetDerEncoded();
	}
}
