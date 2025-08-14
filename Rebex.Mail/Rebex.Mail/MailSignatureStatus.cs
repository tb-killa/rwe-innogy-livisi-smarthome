using System;

namespace Rebex.Mail;

[Flags]
public enum MailSignatureStatus
{
	CertificateNotValid = 1,
	CertificateNotAvailable = 2,
	SignatureNotValid = 4,
	MissingSender = 8,
	SenderSignatureMissing = 0x10
}
