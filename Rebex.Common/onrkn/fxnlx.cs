using System;
using System.Reflection;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal abstract class fxnlx : jayrg, eatps, ntowq, dzjkq, lncnv
{
	protected readonly AsymmetricAlgorithm eyanf;

	private readonly bool ryakq;

	public int KeySize => eyanf.KeySize;

	public abstract AsymmetricKeyAlgorithmId bptsq { get; }

	public abstract string janem { get; }

	protected fxnlx(AsymmetricAlgorithm asymmetric, bool ownsAlgorithm)
	{
		eyanf = asymmetric;
		ryakq = ownsAlgorithm;
	}

	public static fxnlx vffdv(AsymmetricAlgorithm p0, bool p1)
	{
		fxnlx fxnlx2 = mdxtm.zpyak(p0, p1);
		if (fxnlx2 != null && 0 == 0)
		{
			return fxnlx2;
		}
		return null;
	}

	public static fxnlx ghyhn(AsymmetricAlgorithm p0, bool p1)
	{
		fxnlx fxnlx2 = vffdv(p0, p1);
		if (fxnlx2 == null || 1 == 0)
		{
			throw new CryptographicException("Unsupported key algorithm.");
		}
		return fxnlx2;
	}

	public override bool knvjq(jyamo p0)
	{
		return false;
	}

	public virtual byte[] sfbms(byte[] p0, jyamo p1)
	{
		throw new CryptographicException("Encryption is not supported for this algorithm.");
	}

	public override byte[] lhhds(byte[] p0, jyamo p1)
	{
		throw new CryptographicException("Decryption is not supported for this algorithm.");
	}

	public override bool vmedb(mrxvh p0)
	{
		return false;
	}

	public override byte[] rypyi(byte[] p0, mrxvh p1)
	{
		throw new CryptographicException("Signing is not supported for this algorithm.");
	}

	public virtual bool cbzmp(byte[] p0, byte[] p1, mrxvh p2)
	{
		throw new CryptographicException("Signature verification is not supported for this algorithm.");
	}

	protected override void temua(bool p0)
	{
		if (ryakq && 0 == 0 && p0 && 0 == 0)
		{
			eyanf.Clear();
		}
	}

	public virtual PublicKeyInfo kptoi()
	{
		throw new CryptographicException("Not supported for this key algorithm.");
	}

	public override PrivateKeyInfo jbbgs(bool p0)
	{
		throw new CryptographicException("Not supported for this key algorithm.");
	}

	public override CspParameters iqqfj()
	{
		return null;
	}

	[rbjhl("windows")]
	protected static CspParameters izxdl(CspKeyContainerInfo p0)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		if (p0.KeyContainerName == null || 1 == 0)
		{
			return null;
		}
		CspParameters cspParameters = new CspParameters();
		cspParameters.ProviderName = p0.ProviderName;
		cspParameters.ProviderType = p0.ProviderType;
		cspParameters.KeyContainerName = p0.KeyContainerName;
		cspParameters.KeyNumber = (int)p0.KeyNumber;
		cspParameters.Flags = CspProviderFlags.UseExistingKey;
		if (p0.MachineKeyStore && 0 == 0)
		{
			cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
		}
		return cspParameters;
	}

	[rbjhl("windows")]
	public static CspParameters laldo(PrivateKeyInfo p0, string p1, bool p2)
	{
		CspParameters cspParameters = new CspParameters();
		cspParameters.KeyContainerName = p1;
		cspParameters.Flags = ((p2 ? true : false) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
		switch (p0.KeyAlgorithm.edeag())
		{
		case AsymmetricKeyAlgorithmId.RSA:
		{
			RSAParameters rSAParameters = p0.GetRSAParameters();
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
			try
			{
				if (!dahxy.umrab || 1 == 0)
				{
					rSACryptoServiceProvider.PersistKeyInCsp = true;
					rSACryptoServiceProvider.ImportParameters(rSAParameters);
				}
				else
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
					rSACryptoServiceProvider.ImportParameters(rSAParameters);
					rSACryptoServiceProvider.PersistKeyInCsp = true;
				}
				return izxdl(rSACryptoServiceProvider.CspKeyContainerInfo);
			}
			finally
			{
				if (rSACryptoServiceProvider != null && 0 == 0)
				{
					((IDisposable)rSACryptoServiceProvider).Dispose();
				}
			}
		}
		case AsymmetricKeyAlgorithmId.DSA:
		{
			if (!dahxy.umrab || 1 == 0)
			{
				cspParameters.KeyNumber = 2;
				cspParameters.ProviderType = 13;
				cspParameters.ProviderName = "Microsoft Base DSS and Diffie-Hellman Cryptographic Provider";
			}
			DSAParameters parameters = p0.fwtfa();
			DSACryptoServiceProvider dSACryptoServiceProvider = new DSACryptoServiceProvider(cspParameters);
			try
			{
				if (!dahxy.umrab || 1 == 0)
				{
					dSACryptoServiceProvider.PersistKeyInCsp = true;
					dSACryptoServiceProvider.ImportParameters(parameters);
				}
				else
				{
					dSACryptoServiceProvider.PersistKeyInCsp = false;
					dSACryptoServiceProvider.ImportParameters(parameters);
					dSACryptoServiceProvider.PersistKeyInCsp = true;
					dSACryptoServiceProvider.GetType().InvokeMember("OnKeyGenerated", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, dSACryptoServiceProvider, new object[2]
					{
						dSACryptoServiceProvider,
						EventArgs.Empty
					});
				}
				return izxdl(dSACryptoServiceProvider.CspKeyContainerInfo);
			}
			finally
			{
				if (dSACryptoServiceProvider != null && 0 == 0)
				{
					((IDisposable)dSACryptoServiceProvider).Dispose();
				}
			}
		}
		default:
			throw new CryptographicException("Unable to store the private key.");
		}
	}
}
