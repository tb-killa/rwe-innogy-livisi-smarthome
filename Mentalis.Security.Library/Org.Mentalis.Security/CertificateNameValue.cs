using System;

namespace Org.Mentalis.Security;

internal struct CertificateNameValue
{
	public int dwValueType;

	public int cbData;

	public IntPtr pbData;
}
