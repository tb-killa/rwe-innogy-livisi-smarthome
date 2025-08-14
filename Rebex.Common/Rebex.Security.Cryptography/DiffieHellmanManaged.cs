using System;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class DiffieHellmanManaged : DiffieHellman, hibhk
{
	private bdjih uojda;

	private bdjih lfbuj;

	private bdjih tcfew;

	private bdjih pbnpr;

	private RandomNumberGenerator eeifc;

	private int xymry;

	public override string SignatureAlgorithm
	{
		get
		{
			throw new NotSupportedException("Diffie-Hellman does not support signatures.");
		}
	}

	public override string KeyExchangeAlgorithm => "Diffie-Hellman";

	internal static int lelko()
	{
		return 2048;
	}

	public DiffieHellmanManaged()
		: this(1024)
	{
	}

	public DiffieHellmanManaged(int keySize)
	{
		xymry = lelko();
		if (((keySize % 8 != 0) ? true : false) || keySize < 128 || keySize > xymry)
		{
			throw new CryptographicException("Unsupported key size ({0}).", keySize.ToString());
		}
		KeySizeValue = keySize;
		LegalKeySizesValue = new KeySizes[1]
		{
			new KeySizes(128, xymry, 8)
		};
		eeifc = CryptoHelper.CreateRandomNumberGenerator();
	}

	public override byte[] GetPublicKey()
	{
		rkvbt();
		return pbnpr.kskce(p0: false);
	}

	private byte[] nlfny(byte[] p0)
	{
		return rqsoi(p0, p1: false);
	}

	byte[] hibhk.ovrid(byte[] p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nlfny
		return this.nlfny(p0);
	}

	public override byte[] GetSharedSecretKey(byte[] otherPublicKey)
	{
		return rqsoi(otherPublicKey, p1: true);
	}

	private byte[] rqsoi(byte[] p0, bool p1)
	{
		rkvbt();
		bdjih bdjih = bdjih.foxoi(p0);
		bdjih bdjih2 = bdjih.hbhui(tcfew, uojda);
		return bdjih2.kskce(p1);
	}

	private void rkvbt()
	{
		if (!(uojda != null))
		{
			lfbuj = 5;
			int num = KeySizeValue / 8;
			byte[] array = new byte[num];
			do
			{
				eeifc.GetBytes(array);
				array[0] |= 128;
				array[num - 1] |= 1;
				uojda = bdjih.foxoi(array);
			}
			while (!uojda.fsxwh(80));
			uisfi();
		}
	}

	private void uisfi()
	{
		int num = KeySizeValue / 8;
		byte[] array = new byte[num];
		eeifc.GetBytes(array);
		tcfew = bdjih.foxoi(array);
		pbnpr = lfbuj.hbhui(tcfew, uojda);
	}

	public override void ImportParameters(DiffieHellmanParameters parameters)
	{
		parameters = DiffieHellman.qftrp(parameters, p1: true);
		uojda = bdjih.foxoi(parameters.P);
		if (parameters.G != null && 0 == 0)
		{
			lfbuj = bdjih.foxoi(parameters.G);
		}
		else
		{
			lfbuj = null;
		}
		KeySizeValue = uojda.jaioo();
		if (KeySizeValue < 128 || KeySizeValue > xymry)
		{
			throw new CryptographicException("Unsupported key size ({0}).", KeySizeValue.ToString());
		}
		if (parameters.X != null && 0 == 0)
		{
			tcfew = bdjih.foxoi(parameters.X);
			if (parameters.Y != null && 0 == 0)
			{
				pbnpr = bdjih.foxoi(parameters.Y);
			}
			else if (lfbuj != null && 0 == 0)
			{
				pbnpr = lfbuj.hbhui(tcfew, uojda);
			}
		}
		else
		{
			if (lfbuj == null && 0 == 0)
			{
				throw new CryptographicException("Missing G parameter.");
			}
			uisfi();
		}
	}

	public override DiffieHellmanParameters ExportParameters(bool includePrivateParameters)
	{
		rkvbt();
		DiffieHellmanParameters result = new DiffieHellmanParameters
		{
			P = uojda.kskce(p0: false)
		};
		if (lfbuj != null && 0 == 0)
		{
			result.G = lfbuj.kskce(p0: false);
		}
		if (includePrivateParameters && 0 == 0)
		{
			result.X = tcfew.kskce(p0: false);
		}
		result.Y = pbnpr.kskce(p0: false);
		return result;
	}

	protected override void Dispose(bool disposing)
	{
		eeifc = null;
	}
}
