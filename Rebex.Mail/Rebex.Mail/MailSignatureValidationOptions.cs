using System;

namespace Rebex.Mail;

[Flags]
public enum MailSignatureValidationOptions
{
	SkipHeaderCheck = 1,
	SkipCertificateCheck = 2,
	IgnoreMissingNonSenderOriginators = 4
}
