using System;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class ieqlp : wmgeg
{
	public const int xvyxx = 32;

	public const int egqpc = 64;

	public PrivateKeyInfo tmkiq(wmbjj p0, string p1)
	{
		byte[] array = p0.jcckr();
		if (array.Length != 32)
		{
			throw new ArgumentException();
		}
		byte[] array2 = p0.jcckr();
		if (array2.Length != 64)
		{
			throw new ArgumentException();
		}
		AlgorithmIdentifier algorithm = new AlgorithmIdentifier("1.3.101.112");
		return new PrivateKeyInfo(algorithm, array2, null, AsymmetricKeyAlgorithmId.EdDsa);
	}

	public qbkfb ojgoe(PrivateKeyInfo p0)
	{
		byte[] p1 = p0.GetPublicKey().ToBytes();
		wmbjj wmbjj2 = new wmbjj();
		wmbjj2.qtrnf(p1);
		wmbjj2.qtrnf(p0.hsjue());
		wmbjj wmbjj3 = new wmbjj();
		wmbjj3.vokoa("ssh-ed25519");
		wmbjj3.qtrnf(p1);
		return new qbkfb(wmbjj3.ihelo(), wmbjj2.ihelo());
	}
}
