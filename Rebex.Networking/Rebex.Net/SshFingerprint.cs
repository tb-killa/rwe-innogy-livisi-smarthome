using System;
using System.ComponentModel;
using System.Globalization;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Net;

public class SshFingerprint
{
	private readonly byte[] ynsri;

	private SshFingerprint(byte[] data)
	{
		ynsri = data;
	}

	public static SshFingerprint Compute(byte[] key)
	{
		if (key == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		return new SshFingerprint(key);
	}

	public static SshFingerprint FromBase64String(string encodedKey)
	{
		if (encodedKey == null || 1 == 0)
		{
			throw new ArgumentNullException("encodedKey");
		}
		byte[] data;
		try
		{
			data = Convert.FromBase64String(encodedKey);
		}
		catch (FormatException)
		{
			throw new ArgumentException("Invalid Base-64 encoding.", "encodedKey");
		}
		return new SshFingerprint(data);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("ToArray() method has been deprecated. Please use ToArray(SignatureHashAlgorithm) instead.", false)]
	[wptwl(false)]
	public byte[] ToArray()
	{
		return ToArray(SignatureHashAlgorithm.SHA256);
	}

	public override string ToString()
	{
		return ToString(SignatureHashAlgorithm.SHA256, base64: true);
	}

	public byte[] ToArray(SignatureHashAlgorithm algorithm)
	{
		HashingAlgorithmId hashingAlgorithmId;
		switch (algorithm)
		{
		case SignatureHashAlgorithm.MD4:
			hashingAlgorithmId = HashingAlgorithmId.MD4;
			if (hashingAlgorithmId != 0)
			{
				break;
			}
			goto case SignatureHashAlgorithm.MD5;
		case SignatureHashAlgorithm.MD5:
			hashingAlgorithmId = HashingAlgorithmId.MD5;
			if (hashingAlgorithmId != 0)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA1;
		case SignatureHashAlgorithm.SHA1:
			hashingAlgorithmId = HashingAlgorithmId.SHA1;
			if (hashingAlgorithmId != 0)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA224;
		case SignatureHashAlgorithm.SHA224:
			hashingAlgorithmId = HashingAlgorithmId.SHA224;
			if (hashingAlgorithmId != 0)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA256;
		case SignatureHashAlgorithm.SHA256:
			hashingAlgorithmId = HashingAlgorithmId.SHA256;
			if (hashingAlgorithmId != 0)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA384;
		case SignatureHashAlgorithm.SHA384:
			hashingAlgorithmId = HashingAlgorithmId.SHA384;
			if (hashingAlgorithmId != 0)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA512;
		case SignatureHashAlgorithm.SHA512:
			hashingAlgorithmId = HashingAlgorithmId.SHA512;
			if (hashingAlgorithmId != 0)
			{
				break;
			}
			goto default;
		default:
			hashingAlgorithmId = (HashingAlgorithmId)0;
			break;
		}
		hashingAlgorithmId |= (HashingAlgorithmId)65536;
		return new HashingAlgorithm(hashingAlgorithmId).ComputeHash(ynsri);
	}

	public string ToString(SignatureHashAlgorithm algorithm)
	{
		return ToString(algorithm, base64: false);
	}

	public string ToString(SignatureHashAlgorithm algorithm, bool base64)
	{
		byte[] array = ToArray(algorithm);
		if (base64 && 0 == 0)
		{
			return Convert.ToBase64String(array).TrimEnd('=');
		}
		return BitConverter.ToString(array).ToLower(CultureInfo.InvariantCulture).Replace('-', ':');
	}
}
