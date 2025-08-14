using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class fncdr : qwrgb
{
	private readonly KeyMaterialDeriver reomi;

	private readonly AsymmetricKeyAlgorithm vxzkr;

	private readonly HashingAlgorithmId yvkui;

	private byte[] mlovz;

	public fncdr(KeyMaterialDeriver deriver, AsymmetricKeyAlgorithm algorithm, HashingAlgorithmId hashAlg)
	{
		reomi = deriver;
		vxzkr = algorithm;
		yvkui = hashAlg;
		mlovz = new byte[0];
	}

	public void pwepj(int p0, int p1)
	{
		if (p1 < 0)
		{
			p1 = -p1;
			p0 += p1;
		}
		else
		{
			p0 -= p1;
			p1 = 0;
		}
		mlovz = new byte[4 + p1];
		mlovz[3] = (byte)p0;
	}

	public override byte[] zhupj(byte[] p0, byte[] p1)
	{
		byte[] array;
		if (p0 == null || 1 == 0)
		{
			array = mlovz;
		}
		else
		{
			array = new byte[p0.Length + mlovz.Length];
			Array.Copy(p0, array, p0.Length);
			mlovz.CopyTo(array, p0.Length);
		}
		KeyDerivationParameters keyDerivationParameters = new KeyDerivationParameters();
		keyDerivationParameters.KeyDerivationFunction = "HASH";
		keyDerivationParameters.HashAlgorithm = yvkui;
		keyDerivationParameters.SecretPrepend = array;
		keyDerivationParameters.SecretAppend = p1;
		return reomi.DeriveKeyMaterial(keyDerivationParameters);
	}

	public override void ngsco()
	{
		reomi.Dispose();
		vxzkr.Dispose();
	}
}
