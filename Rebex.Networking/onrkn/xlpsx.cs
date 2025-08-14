using Rebex.Security.Cryptography;

namespace onrkn;

internal class xlpsx : DeriveBytes
{
	private byte[] cajxx;

	private byte[] ilcny;

	private IHashTransform sbwfe;

	private nmhgd zjnfd;

	public xlpsx(HashingAlgorithmId alg, byte[] secret, byte[] label_seed)
	{
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(alg);
		hashingAlgorithm.KeyMode = HashingAlgorithmKeyMode.HMAC;
		hashingAlgorithm.SetKey(secret);
		sbwfe = hashingAlgorithm.CreateTransform();
		ilcny = label_seed;
		zjnfd = new nmhgd();
		Reset();
	}

	public override void Reset()
	{
		zjnfd.nqnih();
		cajxx = ilcny;
	}

	public override byte[] GetBytes(int count)
	{
		while (zjnfd.Length < count)
		{
			sbwfe.Process(cajxx, 0, cajxx.Length);
			cajxx = sbwfe.GetHash();
			sbwfe.Reset();
			sbwfe.Process(cajxx, 0, cajxx.Length);
			sbwfe.Process(ilcny, 0, ilcny.Length);
			byte[] hash = sbwfe.GetHash();
			sbwfe.Reset();
			zjnfd.Write(hash, 0, hash.Length);
		}
		byte[] result = zjnfd.rjmxb(0, count);
		zjnfd.ejbiu(count);
		return result;
	}

	public void snzje()
	{
		sbwfe.Dispose();
		cajxx = null;
		ilcny = null;
	}
}
