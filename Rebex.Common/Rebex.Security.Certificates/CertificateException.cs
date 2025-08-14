using System;
using System.Security.Cryptography;

namespace Rebex.Security.Certificates;

public class CertificateException : CryptographicException
{
	public CertificateException(string message)
		: base(message)
	{
	}

	public CertificateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
