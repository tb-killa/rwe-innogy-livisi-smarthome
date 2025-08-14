using System;

namespace Org.Mentalis.Security;

internal struct CERT_PUBLIC_KEY_INFO
{
	public IntPtr pszObjId;

	public int agcbData;

	public IntPtr agpbData;

	public int pkcbData;

	public IntPtr pkpbData;

	public int pkcUnusedBits;

	public CERT_PUBLIC_KEY_INFO(CertificateInfo info)
	{
		pszObjId = info.SubjectPublicKeyInfoAlgorithmpszObjId;
		agcbData = info.SubjectPublicKeyInfoAlgorithmParameterscbData;
		agpbData = info.SubjectPublicKeyInfoAlgorithmParameterspbData;
		pkcbData = info.SubjectPublicKeyInfoPublicKeycbData;
		pkpbData = info.SubjectPublicKeyInfoPublicKeypbData;
		pkcUnusedBits = info.SubjectPublicKeyInfoPublicKeycUnusedBits;
	}
}
