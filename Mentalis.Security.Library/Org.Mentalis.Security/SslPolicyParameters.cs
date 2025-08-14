using System;

namespace Org.Mentalis.Security;

internal struct SslPolicyParameters
{
	public int cbSize;

	public int dwAuthType;

	public int fdwChecks;

	public IntPtr pwszServerName;
}
