using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl;

public class RequestEventArgs
{
	public Certificate Certificate { get; set; }

	public RequestEventArgs()
		: this(null)
	{
	}

	public RequestEventArgs(Certificate cert)
	{
		Certificate = cert;
	}
}
