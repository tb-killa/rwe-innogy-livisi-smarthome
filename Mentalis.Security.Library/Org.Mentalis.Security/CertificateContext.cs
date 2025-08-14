using System;

namespace Org.Mentalis.Security;

internal struct CertificateContext
{
	public int dwCertEncodingType;

	public IntPtr pbCertEncoded;

	public int cbCertEncoded;

	public IntPtr pCertInfo;

	public IntPtr hCertStore;
}
