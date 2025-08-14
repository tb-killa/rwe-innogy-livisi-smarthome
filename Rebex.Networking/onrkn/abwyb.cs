using System;
using Rebex;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class abwyb : DeriveBytes
{
	private SignatureHashAlgorithm ciawn;

	private xlpsx bojge;

	private xlpsx ilcod;

	private xlpsx vyefg;

	public override void Reset()
	{
		if (ciawn != SignatureHashAlgorithm.MD5SHA1)
		{
			if (bojge != null && 0 == 0)
			{
				bojge.Reset();
			}
		}
		else
		{
			ilcod.Reset();
			vyefg.Reset();
		}
	}

	public override byte[] GetBytes(int cb)
	{
		if (ciawn != SignatureHashAlgorithm.MD5SHA1)
		{
			return bojge.GetBytes(cb);
		}
		byte[] bytes = ilcod.GetBytes(cb);
		byte[] bytes2 = vyefg.GetBytes(cb);
		byte[] array = new byte[cb];
		int num = 0;
		if (num != 0)
		{
			goto IL_003c;
		}
		goto IL_004b;
		IL_004b:
		if (num < cb)
		{
			goto IL_003c;
		}
		return array;
		IL_003c:
		array[num] = (byte)(bytes[num] ^ bytes2[num]);
		num++;
		goto IL_004b;
	}

	public abwyb(SignatureHashAlgorithm alg, byte[] secret, string label, byte[] seed)
	{
		ciawn = alg;
		byte[] bytes = EncodingTools.ASCII.GetBytes(label);
		byte[] array = new byte[bytes.Length + seed.Length];
		Array.Copy(bytes, 0, array, 0, bytes.Length);
		Array.Copy(seed, 0, array, bytes.Length, seed.Length);
		if (ciawn != SignatureHashAlgorithm.MD5SHA1)
		{
			HashingAlgorithmId hashingAlgorithmId;
			switch (alg)
			{
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
				throw new TlsException(mjddr.qssln, "Unsupported algorithm.");
			}
			bojge = new xlpsx(hashingAlgorithmId, secret, array);
		}
		else
		{
			int num = (secret.Length + 1) / 2;
			byte[] array2 = new byte[num];
			byte[] array3 = new byte[num];
			Array.Copy(secret, 0, array2, 0, num);
			Array.Copy(secret, secret.Length - num, array3, 0, num);
			ilcod = new xlpsx((HashingAlgorithmId)65543, array2, array);
			vyefg = new xlpsx(HashingAlgorithmId.SHA1, array3, array);
		}
	}

	public void jfzmp()
	{
	}
}
