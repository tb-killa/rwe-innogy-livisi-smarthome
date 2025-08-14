using Rebex.Security.Cryptography;

namespace onrkn;

internal class betck : DeriveBytes
{
	private byte[] mcbvh;

	private byte[] icemt;

	private IHashTransform vizmx;

	private IHashTransform guflt;

	private int jdkgm;

	private byte[] nzyao;

	private nmhgd lukuv = new nmhgd();

	public betck(byte[] secret, byte[] seed)
	{
		icemt = secret;
		mcbvh = seed;
		vizmx = new HashingAlgorithm(HashingAlgorithmId.MD5).CreateTransform();
		guflt = new HashingAlgorithm(HashingAlgorithmId.SHA1).CreateTransform();
		nzyao = new byte[26];
		Reset();
	}

	public override void Reset()
	{
		lukuv.nqnih();
		jdkgm = 1;
	}

	public override byte[] GetBytes(int count)
	{
		while (lukuv.Length < count)
		{
			int num = 0;
			if (num != 0)
			{
				goto IL_000e;
			}
			goto IL_0024;
			IL_000e:
			nzyao[num] = (byte)(64 + jdkgm);
			num++;
			goto IL_0024;
			IL_0024:
			if (num >= jdkgm)
			{
				guflt.Reset();
				guflt.Process(nzyao, 0, jdkgm);
				guflt.Process(icemt, 0, icemt.Length);
				guflt.Process(mcbvh, 0, mcbvh.Length);
				vizmx.Reset();
				vizmx.Process(icemt, 0, icemt.Length);
				vizmx.Process(guflt.GetHash(), 0, 20);
				lukuv.Write(vizmx.GetHash(), 0, 16);
				jdkgm++;
				continue;
			}
			goto IL_000e;
		}
		byte[] result = lukuv.rjmxb(0, count);
		lukuv.ejbiu(count);
		return result;
	}

	public void fipbe()
	{
	}
}
