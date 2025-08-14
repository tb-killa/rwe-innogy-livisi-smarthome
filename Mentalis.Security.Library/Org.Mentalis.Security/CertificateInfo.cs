using System;

namespace Org.Mentalis.Security;

internal struct CertificateInfo
{
	public int dwVersion;

	public int SerialNumbercbData;

	public IntPtr SerialNumberpbData;

	public IntPtr SignatureAlgorithmpszObjId;

	public int SignatureAlgorithmParameterscbData;

	public IntPtr SignatureAlgorithmParameterspbData;

	public int IssuercbData;

	public IntPtr IssuerpbData;

	public long NotBefore;

	public long NotAfter;

	public int SubjectcbData;

	public IntPtr SubjectpbData;

	public IntPtr SubjectPublicKeyInfoAlgorithmpszObjId;

	public int SubjectPublicKeyInfoAlgorithmParameterscbData;

	public IntPtr SubjectPublicKeyInfoAlgorithmParameterspbData;

	public int SubjectPublicKeyInfoPublicKeycbData;

	public IntPtr SubjectPublicKeyInfoPublicKeypbData;

	public int SubjectPublicKeyInfoPublicKeycUnusedBits;

	public int IssuerUniqueIdcbData;

	public IntPtr IssuerUniqueIdpbData;

	public int IssuerUniqueIdcUnusedBits;

	public int SubjectUniqueIdcbData;

	public IntPtr SubjectUniqueIdpbData;

	public int SubjectUniqueIdcUnusedBits;

	public int cExtension;

	public IntPtr rgExtension;
}
