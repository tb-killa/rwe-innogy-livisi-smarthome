using System;

namespace Org.Mentalis.Security;

internal struct ChainPolicyParameters
{
	public int cbSize;

	public int dwFlags;

	public IntPtr pvExtraPolicyPara;
}
