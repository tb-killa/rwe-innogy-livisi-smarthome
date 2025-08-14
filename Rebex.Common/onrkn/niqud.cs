using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class niqud : wmgeg
{
	public PrivateKeyInfo tmkiq(wmbjj p0, string p1)
	{
		return new PrivateKeyInfo(new DSAParameters
		{
			P = p0.jcckr(),
			Q = p0.jcckr(),
			G = p0.jcckr(),
			Y = p0.jcckr(),
			X = p0.jcckr()
		});
	}

	public qbkfb ojgoe(PrivateKeyInfo p0)
	{
		DSAParameters dSAParameters = p0.GetDSAParameters();
		wmbjj wmbjj2 = new wmbjj();
		wmbjj2.cwcwc(dSAParameters.P);
		wmbjj2.cwcwc(dSAParameters.Q);
		wmbjj2.cwcwc(dSAParameters.G);
		wmbjj2.cwcwc(dSAParameters.Y);
		wmbjj2.cwcwc(dSAParameters.X);
		wmbjj wmbjj3 = new wmbjj();
		wmbjj3.vokoa("ssh-dss");
		wmbjj3.cwcwc(dSAParameters.P);
		wmbjj3.cwcwc(dSAParameters.Q);
		wmbjj3.cwcwc(dSAParameters.G);
		wmbjj3.cwcwc(dSAParameters.Y);
		return new qbkfb(wmbjj3.ihelo(), wmbjj2.ihelo());
	}
}
