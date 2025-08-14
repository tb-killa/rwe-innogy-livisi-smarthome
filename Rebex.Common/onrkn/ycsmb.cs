using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class ycsmb : wmgeg
{
	public PrivateKeyInfo tmkiq(wmbjj p0, string p1)
	{
		p0.dmqqk();
		byte[] publicKey = p0.jcckr();
		byte[] privateKey = p0.jcckr();
		string text = bpkgq.mjwcm(p1);
		tsnbe privateKey2 = new tsnbe(privateKey, publicKey, text);
		return new PrivateKeyInfo(privateKey2, AsymmetricKeyAlgorithmId.ECDsa);
	}

	public qbkfb ojgoe(PrivateKeyInfo p0)
	{
		byte[] p1 = p0.GetPublicKey().ToBytes();
		wmbjj wmbjj2 = new wmbjj();
		string p2 = bpkgq.wmvaf(p0.KeyAlgorithm);
		wmbjj2.vokoa(p2);
		wmbjj2.qtrnf(p1);
		byte[] p3 = p0.hsjue();
		tsnbe tsnbe2 = new tsnbe();
		hfnnn.qnzgo(tsnbe2, p3);
		wmbjj2.cwcwc(tsnbe2.rpaiz.rtrhq);
		wmbjj wmbjj3 = new wmbjj();
		wmbjj3.vokoa(p0.jvnzi);
		wmbjj3.vokoa(p2);
		wmbjj3.qtrnf(p1);
		return new qbkfb(wmbjj3.ihelo(), wmbjj2.ihelo());
	}
}
