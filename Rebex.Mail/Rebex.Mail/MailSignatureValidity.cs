using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;

namespace Rebex.Mail;

public class MailSignatureValidity
{
	private readonly SignatureValidationResult fhxfw;

	private readonly string[] vmfbb;

	private readonly MailSignatureStatus hzbat;

	public MailSignatureStatus Status => hzbat;

	public SignatureValidationStatus SignatureValidationStatus => fhxfw.Status;

	public ValidationStatus CertificateValidationStatus => fhxfw.CertificateValidationStatus;

	public bool Valid => hzbat == (MailSignatureStatus)0;

	public string[] GetMissingSignatures()
	{
		return (string[])vmfbb.Clone();
	}

	internal MailSignatureValidity(SignatureValidationResult result, string[] missingSignatures, MailSignatureStatus status)
	{
		fhxfw = result;
		vmfbb = missingSignatures;
		hzbat = status;
		if ((fhxfw.Status & SignatureValidationStatus.CertificateNotValid) != 0)
		{
			hzbat |= MailSignatureStatus.CertificateNotValid;
		}
		if ((fhxfw.Status & SignatureValidationStatus.CertificateNotAvailable) != 0)
		{
			hzbat |= MailSignatureStatus.CertificateNotAvailable;
		}
		if ((result.Status & ~SignatureValidationStatus.CertificateNotValid) != 0)
		{
			hzbat |= MailSignatureStatus.SignatureNotValid;
		}
	}
}
