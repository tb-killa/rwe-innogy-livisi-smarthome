using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class jyamo : fivsd
{
	internal static readonly jyamo cjoyr = new jyamo(xdgzn.ctbmq, (HashingAlgorithmId)0, (HashingAlgorithmId)0, null);

	private readonly xdgzn lkmxw;

	private readonly byte[] rmahg;

	private readonly HashingAlgorithmId rhuik;

	private readonly HashingAlgorithmId fwecw;

	public xdgzn vmeor => lkmxw;

	public HashingAlgorithmId fbcyx => rhuik;

	public HashingAlgorithmId bablj => fwecw;

	public bool jmwfp
	{
		get
		{
			if (lkmxw == xdgzn.bntzq)
			{
				return rhuik != fwecw;
			}
			return false;
		}
	}

	public bool izbcg
	{
		get
		{
			if (!jmwfp || 1 == 0)
			{
				return rhuik == HashingAlgorithmId.SHA224;
			}
			return true;
		}
	}

	public byte[] blonh => rmahg;

	public jyamo(xdgzn paddingScheme, HashingAlgorithmId hashAlgorithm, HashingAlgorithmId maskGenHashAlgorithm, byte[] inputParameter)
	{
		lkmxw = paddingScheme;
		rhuik = hashAlgorithm;
		fwecw = maskGenHashAlgorithm;
		byte[] array = inputParameter;
		if (array == null || 1 == 0)
		{
			array = new byte[0];
		}
		rmahg = array;
		epnkz();
	}

	private void epnkz()
	{
		if (izbcg && 0 == 0 && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			string text = ((jmwfp ? true : false) ? "mismatched hash algorithms" : "the specified hash algorithm");
			throw new CryptographicException("RSA/OAEP with " + text + " is not supported in FIPS-only environments.");
		}
		if (vmeor == xdgzn.bntzq)
		{
			HashingAlgorithm.jwiqd(fbcyx, p1: false);
			HashingAlgorithm.jwiqd(bablj, p1: false);
		}
	}
}
