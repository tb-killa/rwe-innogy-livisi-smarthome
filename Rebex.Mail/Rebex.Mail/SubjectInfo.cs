using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Mail;

public class SubjectInfo
{
	private readonly SubjectIdentifier rjukb;

	private readonly CertificateChain kpjpu;

	private readonly SignatureHashAlgorithm vfzsx;

	private readonly MailSignatureParameters kbfiw;

	private readonly MailEncryptionParameters uwlqw;

	public SubjectIdentifier Identifier => rjukb;

	public Certificate Certificate
	{
		get
		{
			if (kpjpu == null || false || kpjpu.Count == 0 || 1 == 0)
			{
				return null;
			}
			return kpjpu[0];
		}
	}

	public CertificateChain CertificateChain => kpjpu;

	public SignatureHashAlgorithm DigestAlgorithm => vfzsx;

	public MailSignatureParameters SignatureParameters => kbfiw;

	public MailEncryptionParameters EncryptionParameters => uwlqw;

	internal SubjectInfo(RecipientInfo recipient, EnvelopedData data)
	{
		rjukb = recipient.RecipientIdentifier;
		kpjpu = recipient.CertificateChain;
		vfzsx = SignatureHashAlgorithm.Unsupported;
		uwlqw = new MailEncryptionParameters(recipient.GetEncryptionParameters(), wcxxf.ojbcd(data));
	}

	internal SubjectInfo(SignerInfo signer, MailSignatureStyle style)
	{
		rjukb = signer.SignerIdentifier;
		kpjpu = signer.CertificateChain;
		vfzsx = signer.ToDigestAlgorithm();
		kbfiw = new MailSignatureParameters(signer.GetSignatureParameters(), style);
	}
}
