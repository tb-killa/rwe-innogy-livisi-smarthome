using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class ecjay : IHashTransform, IDisposable
{
	private IHashTransform augsn;

	private HashingAlgorithmId xlrkg;

	private byte[] imwof;

	private byte[] omcce;

	private oayqn hrulx;

	private byte[] wxyup;

	private int koxze;

	public int HashSize
	{
		get
		{
			return koxze;
		}
		private set
		{
			koxze = value;
		}
	}

	public ecjay(HashingAlgorithmId alg, oayqn type)
		: this(alg, null, type)
	{
	}

	public ecjay(HashingAlgorithmId alg, byte[] rgbKey, oayqn type)
	{
		int num;
		switch (alg)
		{
		case HashingAlgorithmId.MD5:
			num = 48;
			HashSize = 128;
			break;
		case HashingAlgorithmId.SHA1:
			num = 40;
			HashSize = 160;
			break;
		default:
			throw new ArgumentException("Unsupported algorithm.");
		}
		xlrkg = alg;
		hrulx = type;
		imwof = new byte[num];
		omcce = new byte[num];
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0066;
		}
		goto IL_007e;
		IL_007e:
		if (num2 >= num)
		{
			if (rgbKey == null || 1 == 0)
			{
				RandomNumberGenerator randomNumberGenerator = CryptoHelper.CreateRandomNumberGenerator();
				wxyup = new byte[64];
				randomNumberGenerator.GetBytes(wxyup);
			}
			else
			{
				wxyup = (byte[])rgbKey.Clone();
			}
			return;
		}
		goto IL_0066;
		IL_0066:
		imwof[num2] = 54;
		omcce[num2] = 92;
		num2++;
		goto IL_007e;
	}

	public void Reset()
	{
		augsn = null;
	}

	public void Process(byte[] array, int ibStart, int cbSize)
	{
		if (augsn == null || 1 == 0)
		{
			augsn = new HashingAlgorithm(xlrkg).CreateTransform();
			if (hrulx == oayqn.bsnif || 1 == 0)
			{
				augsn.Process(wxyup, 0, wxyup.Length);
				augsn.Process(imwof, 0, imwof.Length);
			}
		}
		augsn.Process(array, ibStart, cbSize);
	}

	public byte[] GetHash()
	{
		return gatyz(augsn);
	}

	public byte[] gatyz(IHashTransform p0)
	{
		if (hrulx != oayqn.bsnif && 0 == 0)
		{
			p0.Process(wxyup, 0, wxyup.Length);
			p0.Process(imwof, 0, imwof.Length);
		}
		IHashTransform hashTransform = new HashingAlgorithm(xlrkg).CreateTransform();
		hashTransform.Process(wxyup, 0, wxyup.Length);
		hashTransform.Process(omcce, 0, omcce.Length);
		byte[] hash = p0.GetHash();
		hashTransform.Process(hash, 0, hash.Length);
		hash = hashTransform.GetHash();
		hashTransform.Dispose();
		return hash;
	}

	public void Dispose()
	{
		augsn.Dispose();
	}
}
