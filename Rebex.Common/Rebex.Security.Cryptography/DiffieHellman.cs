using System;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public abstract class DiffieHellman : AsymmetricAlgorithm, eatps, ibhso, hibhk
{
	private static readonly byte[] obwaf = new byte[1] { 2 };

	private AsymmetricKeyAlgorithmId uhmve => AsymmetricKeyAlgorithmId.DiffieHellman;

	private string towhc => "dh";

	private byte[] ogmzs()
	{
		return GetPublicKey();
	}

	byte[] ibhso.craet()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ogmzs
		return this.ogmzs();
	}

	private byte[] phjpy(byte[] p0)
	{
		return GetSharedSecretKey(p0);
	}

	byte[] hibhk.ovrid(byte[] p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in phjpy
		return this.phjpy(p0);
	}

	public DiffieHellman()
	{
	}

	internal void ekwvc(int p0)
	{
		if (!CryptoHelper.dtvom(LegalKeySizes, p0) || 1 == 0)
		{
			throw new CryptographicException("Unsupported key size ({0}).", p0.ToString());
		}
		KeySizeValue = p0;
	}

	public abstract byte[] GetPublicKey();

	public virtual byte[] GetSharedSecretKey(byte[] otherPublicKey)
	{
		throw new NotImplementedException();
	}

	public abstract void ImportParameters(DiffieHellmanParameters param);

	public abstract DiffieHellmanParameters ExportParameters(bool includePrivateParameters);

	internal static DiffieHellmanParameters qftrp(DiffieHellmanParameters p0, bool p1)
	{
		if (p0.P == null || false || p0.P.Length == 0 || 1 == 0)
		{
			throw new CryptographicException("Missing P parameter.", "parameters");
		}
		byte[] array = jlfbq.cnbay(p0.P);
		if (array.Length == 1)
		{
			throw new CryptographicException("Invalid prime modulus.");
		}
		if (p1 && 0 == 0)
		{
			p0.P = array;
			if (p0.G != null && 0 == 0)
			{
				p0.G = jlfbq.cnbay(p0.G);
			}
			if (p0.X != null && 0 == 0)
			{
				p0.X = jlfbq.cnbay(p0.X);
			}
			if (p0.Y != null && 0 == 0)
			{
				p0.Y = jlfbq.cnbay(p0.Y);
			}
		}
		return p0;
	}

	public static byte[] GetOakleyGenerator()
	{
		return (byte[])obwaf.Clone();
	}

	public static byte[] GetOakleyGroup2()
	{
		return fuipf.paitk(2).P;
	}

	public static byte[] GetOakleyGroup14()
	{
		return fuipf.paitk(14).P;
	}
}
