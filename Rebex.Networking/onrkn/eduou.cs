using System;
using System.IO;
using Rebex;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class eduou : kumym
{
	private readonly HashingAlgorithmId otlgs;

	public eduou(string hostKeyAlgorithm, HashingAlgorithmId hashAlgId)
		: base(hostKeyAlgorithm)
	{
		otlgs = hashAlgId;
	}

	public override void kuyvo(SshSession p0, byte[] p1, byte[] p2, byte[] p3, byte[] p4, out qwrgb p5, out byte[] p6, out SshPublicKey p7)
	{
		bool flag = p0.Parameters.srcjt(p0.ServerIdentification, p0.ServerInfo);
		int minimumDiffieHellmanKeySize = p0.Parameters.MinimumDiffieHellmanKeySize;
		int num;
		int num2;
		int num3;
		if (flag && 0 == 0)
		{
			p0.iejdf(LogLevel.Debug, "Group exchange (legacy form).");
			num = (num2 = (num3 = minimumDiffieHellmanKeySize));
			tsvcd p8 = new tsvcd(num2);
			p0.ialsm(p8, p1: true);
		}
		else
		{
			p0.iejdf(LogLevel.Debug, "Group exchange.");
			num = minimumDiffieHellmanKeySize;
			num2 = Math.Max(1024, minimumDiffieHellmanKeySize);
			num3 = Math.Max(2048, minimumDiffieHellmanKeySize);
			swskm p9 = new swskm(num, num2, num3);
			p0.ialsm(p9, p1: true);
		}
		byte[] array = p0.luglr(xfdwt.ykvft);
		ephou ephou2 = new ephou(array, 0, array.Length, EncodingTools.ASCII);
		p0.iejdf(LogLevel.Debug, "Negotiating key.");
		byte[] array2 = ephou2.edhny();
		byte[] array3 = ephou2.hhtrk();
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		byte[] array4;
		byte[] p11;
		byte[] array5;
		byte[] array6;
		try
		{
			asymmetricKeyAlgorithm.ImportKey(new DiffieHellmanParameters
			{
				G = array3,
				P = array2
			});
			int keySize = asymmetricKeyAlgorithm.KeySize;
			p0.cnfnb(LogLevel.Debug, "Received {0}-bit Diffie-Hellman prime (minimum allowed size is {1} bits).", keySize, minimumDiffieHellmanKeySize);
			if (keySize < minimumDiffieHellmanKeySize)
			{
				throw new SshException(tcpjq.ziezw, "Diffie-Hellman prime received from the server is considered weak.");
			}
			array4 = kumym.yejxo(asymmetricKeyAlgorithm.zimkk());
			drlgy p10 = new drlgy(array4);
			p0.ialsm(p10, p1: true);
			array = p0.luglr(xfdwt.ykvft);
			trppn trppn2 = new trppn(array, 0, array.Length, EncodingTools.ASCII);
			p11 = trppn2.ymtom();
			array5 = trppn2.mlmdb();
			array6 = trppn2.ornst();
			byte[] secret = kumym.yejxo(asymmetricKeyAlgorithm.fevai(array6));
			p5 = new yaeae(otlgs, secret);
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
		kumym.pqmau(memoryStream, array5);
		if (!flag || 1 == 0)
		{
			kumym.ozpwr(memoryStream, num);
		}
		kumym.ozpwr(memoryStream, num2);
		if (!flag || 1 == 0)
		{
			kumym.ozpwr(memoryStream, num3);
		}
		kumym.pqmau(memoryStream, array2);
		kumym.pqmau(memoryStream, array3);
		kumym.pqmau(memoryStream, array4);
		kumym.pqmau(memoryStream, array6);
		p6 = p5.zhupj(memoryStream.ToArray(), null);
		Exception ex = xdrcl(array5, p6, p11, out p7);
		if (ex != null && 0 == 0)
		{
			throw ex;
		}
	}
}
