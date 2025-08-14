using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class vbhtv : KeyMaterialDeriver
{
	private readonly byte[] jbzqi;

	private HashingAlgorithmId tvwsq;

	public byte[] ttsih => jbzqi;

	public vbhtv(byte[] sharedSecret, HashingAlgorithmId defaultHashAlgorithm)
	{
		jbzqi = sharedSecret;
		tvwsq = defaultHashAlgorithm;
	}

	public override byte[] DeriveKeyMaterial(KeyDerivationParameters parameters)
	{
		if (parameters == null || 1 == 0)
		{
			throw new ArgumentNullException("parameters");
		}
		HashingAlgorithmId algorithm = (((parameters.HashAlgorithm != 0) ? true : false) ? parameters.HashAlgorithm : tvwsq);
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(algorithm);
		try
		{
			string keyDerivationFunction;
			if ((keyDerivationFunction = parameters.KeyDerivationFunction) != null && 0 == 0)
			{
				if (!(keyDerivationFunction == "HASH") || 1 == 0)
				{
					if (!(keyDerivationFunction == "HMAC"))
					{
						goto IL_0096;
					}
					hashingAlgorithm.KeyMode = HashingAlgorithmKeyMode.HMAC;
					hashingAlgorithm.SetKey(parameters.HmacKey);
				}
				IHashTransform hashTransform = hashingAlgorithm.CreateTransform();
				try
				{
					byte[] secretPrepend = parameters.SecretPrepend;
					if (secretPrepend != null && 0 == 0)
					{
						hashTransform.Process(secretPrepend, 0, secretPrepend.Length);
					}
					hashTransform.Process(jbzqi, 0, jbzqi.Length);
					byte[] secretAppend = parameters.SecretAppend;
					if (secretAppend != null && 0 == 0)
					{
						hashTransform.Process(secretAppend, 0, secretAppend.Length);
					}
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
			goto IL_0096;
			IL_0096:
			throw new NotSupportedException("Key derivation function not supported.");
		}
		finally
		{
			if (hashingAlgorithm != null && 0 == 0)
			{
				((IDisposable)hashingAlgorithm).Dispose();
			}
		}
	}
}
