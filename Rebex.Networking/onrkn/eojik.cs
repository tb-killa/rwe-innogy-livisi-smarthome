using System;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class eojik : IHashTransform, IDisposable
{
	private IHashTransform qchum;

	private IHashTransform biqrb;

	public int HashSize => 288;

	public eojik(SignatureHashAlgorithm alg)
	{
		HashingAlgorithmId hashingAlgorithmId;
		switch (alg)
		{
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
			goto case SignatureHashAlgorithm.MD5SHA1;
		case SignatureHashAlgorithm.MD5SHA1:
			qchum = new HashingAlgorithm((HashingAlgorithmId)65543).CreateTransform();
			biqrb = new HashingAlgorithm(HashingAlgorithmId.SHA1).CreateTransform();
			return;
		default:
			throw new TlsException(mjddr.qssln, "Unsupported algorithm.");
		}
		qchum = new HashingAlgorithm(hashingAlgorithmId).CreateTransform();
	}

	public void Reset()
	{
		if (qchum != null && 0 == 0)
		{
			qchum.Reset();
		}
		if (biqrb != null && 0 == 0)
		{
			biqrb.Reset();
		}
	}

	public void Process(byte[] array, int ibStart, int cbSize)
	{
		if (qchum != null && 0 == 0)
		{
			qchum.Process(array, ibStart, cbSize);
		}
		if (biqrb != null && 0 == 0)
		{
			biqrb.Process(array, ibStart, cbSize);
		}
	}

	public byte[] GetHash()
	{
		if (biqrb == null || 1 == 0)
		{
			return qchum.GetHash();
		}
		byte[] array = new byte[36];
		Array.Copy(qchum.GetHash(), 0, array, 0, 16);
		Array.Copy(biqrb.GetHash(), 0, array, 16, 20);
		return array;
	}

	public byte[] qgbqt(byte[] p0)
	{
		byte[] array = new byte[36];
		if (qchum != null && 0 == 0)
		{
			ecjay ecjay2 = new ecjay(HashingAlgorithmId.MD5, p0, oayqn.dyaln);
			byte[] sourceArray = ecjay2.gatyz(qchum);
			Array.Copy(sourceArray, 0, array, 0, 16);
		}
		if (biqrb != null && 0 == 0)
		{
			ecjay ecjay3 = new ecjay(HashingAlgorithmId.SHA1, p0, oayqn.dyaln);
			byte[] sourceArray2 = ecjay3.gatyz(biqrb);
			Array.Copy(sourceArray2, 0, array, 16, 20);
		}
		return array;
	}

	public void Dispose()
	{
		if (qchum != null && 0 == 0)
		{
			qchum.Dispose();
		}
		if (biqrb != null && 0 == 0)
		{
			biqrb.Dispose();
		}
	}
}
