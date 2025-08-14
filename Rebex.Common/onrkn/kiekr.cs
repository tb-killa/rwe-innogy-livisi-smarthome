using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class kiekr : wmgeg
{
	public PrivateKeyInfo tmkiq(wmbjj p0, string p1)
	{
		return new PrivateKeyInfo(new RSAParameters
		{
			Modulus = p0.sklfv(),
			Exponent = p0.sklfv(),
			D = p0.sklfv(),
			InverseQ = p0.sklfv(),
			P = p0.sklfv(),
			Q = p0.sklfv()
		});
	}

	public qbkfb ojgoe(PrivateKeyInfo p0)
	{
		RSAParameters rSAParameters = p0.GetRSAParameters();
		wmbjj wmbjj2 = new wmbjj();
		wmbjj2.cwcwc(rSAParameters.Modulus);
		wmbjj2.cwcwc(rSAParameters.Exponent);
		wmbjj2.cwcwc(rSAParameters.D);
		wmbjj2.cwcwc(rSAParameters.InverseQ);
		wmbjj2.cwcwc(rSAParameters.P);
		wmbjj2.cwcwc(rSAParameters.Q);
		return new qbkfb(p0.GetPublicKey().ToBytes(), wmbjj2.ihelo());
	}
}
