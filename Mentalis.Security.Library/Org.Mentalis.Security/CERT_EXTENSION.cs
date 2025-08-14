using System;

namespace Org.Mentalis.Security;

internal struct CERT_EXTENSION
{
	public IntPtr pszObjId;

	public int fCritical;

	public int ValuecbData;

	public IntPtr ValuepbData;
}
