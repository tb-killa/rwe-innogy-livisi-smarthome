using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class yaeae : qwrgb
{
	private readonly IHashTransform phesd;

	private readonly byte[] qntmz;

	private readonly byte[] ussti;

	public yaeae(HashingAlgorithmId algId, byte[] secret)
	{
		phesd = new HashingAlgorithm(algId).CreateTransform();
		qntmz = secret;
		ussti = BitConverter.GetBytes(secret.Length);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(ussti, 0, ussti.Length);
		}
	}

	public override byte[] zhupj(byte[] p0, byte[] p1)
	{
		if (p0 != null && 0 == 0)
		{
			phesd.Process(p0, 0, p0.Length);
		}
		phesd.Process(ussti, 0, ussti.Length);
		phesd.Process(qntmz, 0, qntmz.Length);
		if (p1 != null && 0 == 0)
		{
			phesd.Process(p1, 0, p1.Length);
		}
		byte[] hash = phesd.GetHash();
		phesd.Reset();
		return hash;
	}

	public override void ngsco()
	{
		phesd.Dispose();
	}
}
