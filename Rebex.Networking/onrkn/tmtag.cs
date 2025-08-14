using Rebex;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class tmtag : sgjhx
{
	private readonly KeyMaterialDeriver mkzge;

	public tmtag(KeyMaterialDeriver inner)
	{
		mkzge = inner;
	}

	public override byte[] ffrgi(TlsProtocol p0, string p1, SignatureHashAlgorithm p2, byte[] p3)
	{
		KeyDerivationParameters keyDerivationParameters = new KeyDerivationParameters();
		keyDerivationParameters.KeyDerivationFunction = "TLS_PRF";
		keyDerivationParameters.zdnet = (int)p0;
		keyDerivationParameters.ijxbw = p3;
		keyDerivationParameters.nscqa = EncodingTools.ASCII.GetBytes(p1);
		if (p0 >= TlsProtocol.TLS12)
		{
			keyDerivationParameters.HashAlgorithm = bpkgq.pdzpf(p2.ToString());
		}
		return mkzge.DeriveKeyMaterial(keyDerivationParameters);
	}

	public override void Dispose()
	{
		mkzge.Dispose();
	}
}
