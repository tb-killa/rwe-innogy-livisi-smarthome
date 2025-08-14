using System;

namespace Org.Mentalis.Security;

internal struct CertificateExtension
{
	public IntPtr pszObjId;

	public int fCritical;

	public int cbData;

	public IntPtr pbData;
}
