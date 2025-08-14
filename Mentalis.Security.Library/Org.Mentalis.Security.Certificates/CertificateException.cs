using System;

namespace Org.Mentalis.Security.Certificates;

[Serializable]
public class CertificateException : Exception
{
	public CertificateException()
	{
	}

	public CertificateException(string message)
		: base(message)
	{
	}

	public CertificateException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
