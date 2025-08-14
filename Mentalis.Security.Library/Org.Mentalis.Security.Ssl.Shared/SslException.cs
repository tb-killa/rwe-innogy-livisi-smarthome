using System;

namespace Org.Mentalis.Security.Ssl.Shared;

[Serializable]
internal class SslException : Exception
{
	private readonly AlertDescription m_AlertDescription;

	public AlertDescription AlertDescription => m_AlertDescription;

	public SslException(AlertDescription description, string message)
		: this(null, description, message)
	{
	}

	public SslException(Exception e, AlertDescription description, string message)
		: base(message, e)
	{
		m_AlertDescription = description;
	}
}
