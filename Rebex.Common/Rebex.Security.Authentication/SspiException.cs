using System.Runtime.InteropServices;

namespace Rebex.Security.Authentication;

public class SspiException : ExternalException
{
	internal SspiException(string message, int errorCode)
		: base(message)
	{
	}
}
