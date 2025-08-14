using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class jpwxz : hwwit
{
	private readonly Certificate ndfog;

	private readonly AsymmetricKeyAlgorithm ecnhc;

	private readonly AlgorithmIdentifier stodq;

	private readonly SignatureFormat zuzwm;

	public AlgorithmIdentifier kjail => stodq;

	public jpwxz(AsymmetricKeyAlgorithm algorithm, SignatureHashAlgorithm? signatureHashAlgorithm)
	{
		ecnhc = algorithm;
		stodq = algorithm.xdrvo(signatureHashAlgorithm);
		zuzwm = ((algorithm.Algorithm == AsymmetricKeyAlgorithmId.EdDsa) ? SignatureFormat.Raw : SignatureFormat.Pkcs);
	}

	public jpwxz(Certificate certificationAuthority, SignatureHashAlgorithm? signatureHashAlgorithm)
	{
		ndfog = certificationAuthority;
		stodq = AlgorithmIdentifier.lehcn(certificationAuthority.lukcl(), signatureHashAlgorithm);
		zuzwm = ((certificationAuthority.KeyAlgorithm == KeyAlgorithm.ED25519) ? SignatureFormat.Raw : SignatureFormat.Pkcs);
	}

	public byte[] noegy(byte[] p0)
	{
		HashingAlgorithmId hashAlgorithm = stodq.vvmoi(p0: true);
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.HashAlgorithm = hashAlgorithm;
		signatureParameters.Format = zuzwm;
		if (ndfog != null && 0 == 0)
		{
			return ndfog.SignMessage(p0, signatureParameters);
		}
		return ecnhc.SignMessage(p0, signatureParameters);
	}
}
