using System;

namespace Org.Mentalis.Security;

internal struct RdnAttribute
{
	public IntPtr pszObjId;

	public int dwValueType;

	public int cbData;

	public IntPtr pbData;
}
