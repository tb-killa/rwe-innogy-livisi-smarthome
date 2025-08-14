using System;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal class onash : sgjhx
{
	private readonly byte[] etlvz;

	public byte[] sxggy => etlvz;

	public onash(byte[] preMasterSecret)
	{
		etlvz = preMasterSecret;
	}

	public override byte[] ffrgi(TlsProtocol p0, string p1, SignatureHashAlgorithm p2, byte[] p3)
	{
		if (p0 < TlsProtocol.TLS12)
		{
			p2 = SignatureHashAlgorithm.MD5SHA1;
		}
		abwyb abwyb2 = new abwyb(p2, etlvz, p1, p3);
		try
		{
			return abwyb2.GetBytes(48);
		}
		finally
		{
			abwyb2.Reset();
		}
	}

	public override void Dispose()
	{
		Array.Clear(etlvz, 0, etlvz.Length);
	}
}
