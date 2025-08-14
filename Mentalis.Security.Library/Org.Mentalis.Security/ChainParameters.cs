using System;

namespace Org.Mentalis.Security;

internal struct ChainParameters
{
	public int cbSize;

	public int RequestedUsagedwType;

	public int RequestedUsagecUsageIdentifier;

	public IntPtr RequestedUsagergpszUsageIdentifier;
}
