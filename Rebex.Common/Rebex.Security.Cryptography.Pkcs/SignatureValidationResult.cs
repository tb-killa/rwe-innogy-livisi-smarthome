using Rebex.Security.Certificates;

namespace Rebex.Security.Cryptography.Pkcs;

public class SignatureValidationResult
{
	private SignatureValidationStatus phtaq;

	private ValidationStatus aghhh;

	public SignatureValidationStatus Status => phtaq;

	public ValidationStatus CertificateValidationStatus => aghhh;

	public bool Valid => phtaq == (SignatureValidationStatus)0L;

	internal void cwomp(SignatureValidationStatus p0)
	{
		phtaq |= p0;
	}

	internal void leusz(SignatureValidationResult p0)
	{
		phtaq |= p0.phtaq;
		aghhh |= p0.aghhh;
	}

	internal void mpunj(ValidationResult p0)
	{
		aghhh |= p0.Status;
		if (!p0.Valid || 1 == 0)
		{
			phtaq |= SignatureValidationStatus.CertificateNotValid;
		}
	}

	internal SignatureValidationResult()
	{
	}
}
