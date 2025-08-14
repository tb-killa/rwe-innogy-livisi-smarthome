using System;

namespace Org.Mentalis.Security;

internal struct ChainPolicyStatus
{
	public int cbSize;

	public int dwError;

	public int lChainIndex;

	public int lElementIndex;

	public IntPtr pvExtraPolicyStatus;
}
