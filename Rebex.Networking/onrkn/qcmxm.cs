using System;
using System.IO;
using Rebex;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class qcmxm : kumym
{
	private readonly string ednbi;

	private readonly HashingAlgorithmId vlhsv;

	private readonly int ysmpv;

	public qcmxm(string hostKeyAlgorithm, string curve, HashingAlgorithmId hashAlgorithm, int keySize)
		: base(hostKeyAlgorithm)
	{
		ednbi = curve;
		vlhsv = hashAlgorithm;
		ysmpv = keySize;
	}

	public override void kuyvo(SshSession p0, byte[] p1, byte[] p2, byte[] p3, byte[] p4, out qwrgb p5, out byte[] p6, out SshPublicKey p7)
	{
		p0.iejdf(LogLevel.Debug, "Negotiating key.");
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		asymmetricKeyAlgorithm.kvrol(AsymmetricKeyAlgorithmId.ECDH, ednbi, 0);
		byte[] array = asymmetricKeyAlgorithm.zimkk();
		xxfya p8 = new xxfya(array);
		p0.ialsm(p8, p1: true);
		byte[] array2 = p0.luglr(xfdwt.ykvft);
		ihubj ihubj2 = new ihubj(array2, 0, array2.Length, EncodingTools.ASCII);
		byte[] array3 = ihubj2.ernko();
		byte[] array4 = ihubj2.ddvbs();
		byte[] p9 = ihubj2.afokd();
		byte[] array5 = asymmetricKeyAlgorithm.qpwqy(array4);
		if (array5 == null || 1 == 0)
		{
			throw new InvalidOperationException("Key material deriver not supported.");
		}
		byte[] secret = jlfbq.twxvm(array5);
		p5 = new yaeae(vlhsv, secret);
		asymmetricKeyAlgorithm.Dispose();
		ovjbp(p0);
		MemoryStream memoryStream = new MemoryStream();
		kumym.pqmau(memoryStream, p1);
		kumym.pqmau(memoryStream, p2);
		kumym.pqmau(memoryStream, p3);
		kumym.pqmau(memoryStream, p4);
		kumym.pqmau(memoryStream, array3);
		kumym.pqmau(memoryStream, array);
		kumym.pqmau(memoryStream, array4);
		p6 = p5.zhupj(memoryStream.ToArray(), null);
		SshException ex = xdrcl(array3, p6, p9, out p7);
		if (ex != null && 0 == 0)
		{
			throw ex;
		}
	}
}
