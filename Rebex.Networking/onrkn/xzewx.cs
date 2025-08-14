using System;
using System.IO;
using Rebex;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class xzewx : kumym
{
	private readonly int owgnl;

	private readonly HashingAlgorithmId pjoux;

	public xzewx(int group, string hostKeyAlgorithm, HashingAlgorithmId hashAlgId)
		: base(hostKeyAlgorithm)
	{
		owgnl = group;
		pjoux = hashAlgId;
	}

	public override void kuyvo(SshSession p0, byte[] p1, byte[] p2, byte[] p3, byte[] p4, out qwrgb p5, out byte[] p6, out SshPublicKey p7)
	{
		p0.iejdf(LogLevel.Debug, "Negotiating key.");
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		byte[] array;
		byte[] p9;
		byte[] array3;
		byte[] array4;
		try
		{
			DiffieHellmanParameters key = AsymmetricKeyAlgorithm.zknnz(owgnl);
			asymmetricKeyAlgorithm.ImportKey(key);
			int minimumDiffieHellmanKeySize = p0.Parameters.MinimumDiffieHellmanKeySize;
			int keySize = asymmetricKeyAlgorithm.KeySize;
			p0.cnfnb(LogLevel.Debug, "Using {0}-bit Diffie-Hellman prime (minimum allowed size is {1} bits).", keySize, minimumDiffieHellmanKeySize);
			array = kumym.yejxo(asymmetricKeyAlgorithm.zimkk());
			cblft p8 = new cblft(array);
			p0.ialsm(p8, p1: true);
			byte[] array2 = p0.luglr(xfdwt.ykvft);
			njdxl njdxl2 = new njdxl(array2, 0, array2.Length, EncodingTools.ASCII);
			p9 = njdxl2.ymtom();
			array3 = njdxl2.mlmdb();
			array4 = njdxl2.ornst();
			byte[] secret = kumym.yejxo(asymmetricKeyAlgorithm.fevai(array4));
			p5 = new yaeae(pjoux, secret);
		}
		finally
		{
			if (asymmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)asymmetricKeyAlgorithm).Dispose();
			}
		}
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
		Exception ex = xdrcl(array3, p6, p9, out p7);
		if (ex != null && 0 == 0)
		{
			throw ex;
		}
	}
}
