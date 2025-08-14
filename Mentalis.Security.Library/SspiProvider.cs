using System;
using System.Runtime.InteropServices;
using System.Text;
using Org.Mentalis.Security;

internal static class SspiProvider
{
	[DllImport("crypt32.dll", SetLastError = true)]
	internal static extern int CertAddCertificateContextToStore(IntPtr hCertStore, IntPtr pCertContext, int dwAddDisposition, IntPtr ppStoreContext);

	[DllImport("crypt32.dll")]
	internal static extern int CertAddStoreToCollection(IntPtr hCollectionStore, IntPtr hSiblingStore, int dwUpdateFlag, int dwPriority);

	[DllImport("crypt32.dll")]
	internal static extern int CertCloseStore(IntPtr hCertStore, int dwFlags);

	[DllImport("crypt32.dll")]
	internal static extern int CertCompareCertificate(int dwCertEncodingType, IntPtr pCertId1, IntPtr pCertId2);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertCreateCertificateContext(int dwCertEncodingType, IntPtr pbCertEncoded, int cbCertEncoded);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertCreateCRLContext(int dwCertEncodingType, byte[] pbCrlEncoded, int cbCrlEncoded);

	[DllImport("crypt32.dll")]
	internal static extern int CertDeleteCertificateFromStore(IntPtr pCertContext);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertDuplicateCertificateContext(IntPtr pCertContext);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertDuplicateStore(IntPtr hCertStore);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertEnumCertificatesInStore(IntPtr hCertStore, int pPrevCertContext);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertEnumCertificatesInStore(IntPtr hCertStore, IntPtr pPrevCertContext);

	[DllImport("crypt32.dll")]
	internal static extern int CertFindCertificateInCRL(IntPtr pCert, IntPtr pCrlContext, int dwFlags, IntPtr pvReserved, ref IntPtr ppCrlEntry);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertFindCertificateInStore(IntPtr hCertStore, int dwCertEncodingType, int dwFindFlags, int dwFindType, IntPtr pvFindPara, IntPtr pPrevCertContext);

	[DllImport("crypt32.dll", EntryPoint = "CertFindCertificateInStore")]
	internal static extern IntPtr CertFindDataBlobCertificateInStore(IntPtr hCertStore, int dwCertEncodingType, int dwFindFlags, int dwFindType, ref DataBlob pvFindPara, IntPtr pPrevCertContext);

	[DllImport("crypt32.dll")]
	private static extern IntPtr CertFindExtension(byte[] pszObjId, int cExtensions, IntPtr rgExtensions);

	internal static IntPtr CertFindExtension(string pszObjId, int cExtensions, IntPtr rgExtensions)
	{
		return CertFindExtension(Encoding.ASCII.GetBytes(pszObjId), cExtensions, rgExtensions);
	}

	[DllImport("crypt32.dll")]
	private static extern IntPtr CertFindRDNAttr(byte[] pszObjId, IntPtr pName);

	internal static IntPtr CertFindRDNAttr(string obj, IntPtr pName)
	{
		return CertFindRDNAttr(Encoding.ASCII.GetBytes(obj), pName);
	}

	[DllImport("crypt32.dll", EntryPoint = "CertFindCertificateInStore")]
	internal static extern IntPtr CertFindStringCertificateInStore(IntPtr hCertStore, int dwCertEncodingType, int dwFindFlags, int dwFindType, [MarshalAs(UnmanagedType.LPWStr)] string pvFindPara, IntPtr pPrevCertContext);

	[DllImport("crypt32.dll", EntryPoint = "CertFindCertificateInStore")]
	internal static extern IntPtr CertFindUsageCertificateInStore(IntPtr hCertStore, int dwCertEncodingType, int dwFindFlags, int dwFindType, ref TrustListUsage pvFindPara, IntPtr pPrevCertContext);

	[DllImport("crypt32.dll")]
	internal static extern void CertFreeCertificateChain(IntPtr pChainContext);

	[DllImport("crypt32.dll")]
	internal static extern int CertFreeCertificateContext(IntPtr pCertContext);

	[DllImport("crypt32.dll")]
	internal static extern int CertFreeCRLContext(IntPtr pCrlContext);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetCertificateChain(IntPtr hChainEngine, IntPtr pCertContext, IntPtr pTime, IntPtr hAdditionalStore, ref ChainParameters pChainPara, int dwFlags, IntPtr pvReserved, ref IntPtr ppChainContext);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetCertificateContextProperty(IntPtr pCertContext, int dwPropId, byte[] pvData, ref int pcbData);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetCertificateContextProperty(IntPtr pCertContext, int dwPropId, IntPtr pvData, ref int pcbData);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetEnhancedKeyUsage(IntPtr pCertContext, int dwFlags, IntPtr pUsage, ref int pcbUsage);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetIntendedKeyUsage(int dwCertEncodingType, IntPtr pCertInfo, IntPtr pbKeyUsage, int cbKeyUsage);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertGetIssuerCertificateFromStore(IntPtr hCertStore, IntPtr pSubjectContext, IntPtr pPrevIssuerContext, ref int pdwFlags);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetNameString(IntPtr pCertContext, int dwType, int dwFlags, IntPtr pvTypePara, IntPtr pszNameString, int cchNameString);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetNameString(IntPtr pCertContext, int dwType, int dwFlags, IntPtr pvTypePara, byte[] pszNameString, int cchNameString);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetPublicKeyLength(int dwCertEncodingType, IntPtr pPublicKey);

	[DllImport("crypt32.dll")]
	internal static extern int CertGetValidUsages(int cCerts, IntPtr rghCerts, ref int cNumOIDs, IntPtr rghOIDs, ref int pcbOIDs);

	[DllImport("crypt32.dll", SetLastError = true)]
	private static extern IntPtr CertOpenStore(IntPtr lpszStoreProvider, int dwMsgAndCertEncodingType, int hCryptProv, int dwFlags, byte[] pvPara);

	internal static IntPtr CertOpenStore(IntPtr lpszStoreProvider, int dwMsgAndCertEncodingType, int hCryptProv, int dwFlags, string pvPara)
	{
		return CertOpenStore(lpszStoreProvider, dwMsgAndCertEncodingType, hCryptProv, dwFlags, (pvPara == null) ? null : Encoding.ASCII.GetBytes(pvPara));
	}

	[DllImport("crypt32.dll", EntryPoint = "CertOpenStore")]
	internal static extern IntPtr CertOpenStoreData(IntPtr lpszStoreProvider, int dwMsgAndCertEncodingType, IntPtr hCryptProv, int dwFlags, ref DataBlob pvPara);

	[DllImport("crypt32.dll")]
	internal static extern IntPtr CertOpenSystemStore(int hProv, string szSubsystemProtocol);

	[DllImport("crypt32.dll")]
	internal static extern void CertRemoveStoreFromCollection(IntPtr hCollectionStore, IntPtr hSiblingStore);

	[DllImport("crypt32.dll")]
	internal static extern int CertSaveStore(IntPtr hCertStore, int dwMsgAndCertEncodingType, int dwSaveAs, int dwSaveTo, ref DataBlob pvSaveToPara, int dwFlags);

	[DllImport("crypt32.dll")]
	internal static extern int CertSetCertificateContextProperty(IntPtr pCertContext, int dwPropId, int dwFlags, ref CRYPT_KEY_PROV_INFO pvData);

	[DllImport("crypt32.dll")]
	internal static extern int CertStrToName(int dwCertEncodingType, string pszX500, int dwStrType, IntPtr pvReserved, IntPtr pbEncoded, ref int pcbEncoded, IntPtr ppszError);

	[DllImport("crypt32.dll")]
	internal static extern int CertVerifyCertificateChainPolicy(IntPtr pszPolicyOID, IntPtr pChainContext, ref ChainPolicyParameters pPolicyPara, ref ChainPolicyStatus pPolicyStatus);

	[DllImport("crypt32.dll")]
	internal static extern int CertVerifyCRLRevocation(int dwCertEncodingType, IntPtr pCertId, int cCrlInfo, ref IntPtr rgpCrlInfo);

	[DllImport("crypt32.dll")]
	internal static extern int CertVerifyTimeValidity(IntPtr pTimeToVerify, IntPtr pCertInfo);

	[DllImport("crypt32.dll")]
	internal static extern int CryptAcquireCertificatePrivateKey(IntPtr pCert, int dwFlags, IntPtr pvReserved, ref int phCryptProv, ref int pdwKeySpec, ref int pfCallerFreeProv);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptAcquireContext(ref int phProv, IntPtr pszContainer, string pszProvider, int dwProvType, int dwFlags);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptAcquireContext(ref int phProv, string pszContainer, string pszProvider, int dwProvType, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptCreateHash(int hProv, int Algid, int hKey, int dwFlags, out int phHash);

	internal static int CryptDecodeObject(int dwCertEncodingType, IntPtr lpszStructType, IntPtr pbEncoded, int cbEncoded, int dwFlags, IntPtr pvStructInfo, ref int pcbStructInfo)
	{
		return CryptDecodeObjectEx(dwCertEncodingType, lpszStructType, pbEncoded, cbEncoded, dwFlags, 0, pvStructInfo, ref pcbStructInfo);
	}

	internal static int CryptDecodeObject(int dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, int cbEncoded, int dwFlags, IntPtr pvStructInfo, ref int pcbStructInfo)
	{
		return CryptDecodeObjectEx(dwCertEncodingType, lpszStructType, pbEncoded, cbEncoded, dwFlags, 0, pvStructInfo, ref pcbStructInfo);
	}

	[DllImport("crypt32.dll")]
	internal static extern int CryptDecodeObjectEx(int dwCertEncodingType, IntPtr lpszStructType, IntPtr pbEncoded, int cbEncoded, int dwFlags, int pDecodePara, IntPtr pvStructInfo, ref int pcbStructInfo);

	[DllImport("crypt32.dll")]
	internal static extern int CryptDecodeObjectEx(int dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, int cbEncoded, int dwFlags, int pvPara, IntPtr pvStructInfo, ref int pcbStructInfo);

	[DllImport("coredll.dll")]
	internal static extern int CryptDecrypt(int hKey, int hHash, int Final, int dwFlags, byte[] pbData, ref int pdwDataLen);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptDecrypt(IntPtr hKey, int hHash, int Final, int dwFlags, byte[] pbData, ref int pdwDataLen);

	[DllImport("coredll.dll")]
	internal static extern int CryptDestroyHash(int hHash);

	[DllImport("coredll.dll")]
	internal static extern int CryptDestroyKey(int hKey);

	[DllImport("coredll.dll")]
	internal static extern int CryptDuplicateHash(int hHash, IntPtr pdwReserved, int dwFlags, out int phHash);

	[DllImport("coredll.dll")]
	internal static extern int CryptEncrypt(int hKey, int hHash, int Final, int dwFlags, IntPtr pbData, ref int pdwDataLen, int dwBufLen);

	[DllImport("coredll.dll")]
	internal static extern int CryptEncrypt(int hKey, int hHash, int Final, int dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

	[DllImport("coredll.dll")]
	internal static extern int CryptEncrypt(IntPtr hKey, int hHash, int Final, int dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptExportKey(int hKey, int hExpKey, int dwBlobType, int dwFlags, byte[] pbData, ref int pdwDataLen);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptExportKey(int hKey, int hExpKey, int dwBlobType, int dwFlags, IntPtr pbData, ref int pdwDataLen);

	[DllImport("crypt32.dll")]
	internal static extern int CryptFindCertificateKeyProvInfo(IntPtr pCert, int dwFlags, IntPtr pvReserved);

	[DllImport("coredll.dll")]
	internal static extern int CryptGenKey(int hProv, IntPtr Algid, int dwFlags, ref int phKey);

	[DllImport("coredll.dll")]
	internal static extern int CryptGenRandom(int hProv, int dwLen, IntPtr pbBuffer);

	[DllImport("coredll.dll")]
	internal static extern int CryptGetHashParam(int hHash, int dwParam, byte[] pbData, ref int pdwDataLen, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptGetKeyParam(int hKey, int dwParam, byte[] pbData, ref int pdwDataLen, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptGetKeyParam(int hKey, int dwParam, ref int pbData, ref int pdwDataLen, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptGetKeyParam(int hKey, int dwParam, ref IntPtr pbData, ref int pdwDataLen, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptGetProvParam(int hProv, int dwParam, IntPtr pbData, ref int pdwDataLen, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptGetUserKey(int hProv, int dwKeySpec, ref int phUserKey);

	[DllImport("coredll.dll")]
	internal static extern int CryptHashData(int hHash, byte[] pbData, int dwDataLen, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptHashData(int hHash, IntPtr pbData, int dwDataLen, int dwFlags);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptImportKey(int hProv, byte[] pbData, int dwDataLen, int hPubKey, int dwFlags, ref int phKey);

	[DllImport("coredll.dll")]
	internal static extern int CryptImportKey(int hProv, IntPtr pbData, int dwDataLen, int hPubKey, int dwFlags, ref int phKey);

	[DllImport("crypt32.dll")]
	internal static extern int CryptImportPublicKeyInfo(int hCryptProv, int dwCertEncodingType, ref CERT_PUBLIC_KEY_INFO pInfo, out int phKey);

	[DllImport("crypt32.dll")]
	internal static extern int CryptImportPublicKeyInfoEx(int hCryptProv, int dwCertEncodingType, ref CERT_PUBLIC_KEY_INFO pInfo, int aiKeyAlg, int dwFlags, IntPtr pvAuxInfo, ref int phKey);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	internal static extern int CryptProtectData(ref DataBlob pDataIn, string szDataDescr, ref DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DataBlob pDataOut);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptReleaseContext(int hProv, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptSetHashParam(int hHash, int dwParam, byte[] pbData, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptSetKeyParam(int hKey, int dwParam, byte[] pbData, int dwFlags);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern int CryptSetKeyParam(int hKey, int dwParam, ref DataBlob pbData, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptSetKeyParam(int hKey, int dwParam, ref int pbData, int dwFlags);

	[DllImport("coredll.dll")]
	internal static extern int CryptSignHash(int hHash, int dwKeySpec, IntPtr sDescription, int dwFlags, byte[] pbSignature, ref int pdwSigLen);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	internal static extern int CryptUnprotectData(ref DataBlob pDataIn, IntPtr ppszDataDescr, ref DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DataBlob pDataOut);

	[DllImport("coredll.dll")]
	internal static extern int CryptVerifySignature(int hHash, byte[] pbSignature, int dwSigLen, int hPubKey, IntPtr sDescription, int dwFlags);

	[DllImport("crypt32.dll", CharSet = CharSet.Unicode)]
	internal static extern int PFXExportCertStoreEx(IntPtr hStore, ref DataBlob pPFX, string szPassword, IntPtr pvReserved, int dwFlags);

	[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern IntPtr PFXImportCertStore(ref DataBlob pPFX, string szPassword, int dwFlags);

	[DllImport("crypt32.dll")]
	internal static extern int PFXIsPFXBlob(ref DataBlob pPFX);

	[DllImport("crypt32.dll", CharSet = CharSet.Unicode)]
	internal static extern int PFXVerifyPassword(ref DataBlob pPFX, string szPassword, int dwFlags);

	[DllImport("secur32.dll")]
	internal static extern int QueryContextAttributes(ref IntPtr phContext, int ulAttribute, byte[] pBuffer);

	[DllImport("secur32.dll")]
	internal static extern int QueryContextAttributes(ref IntPtr phContext, int ulAttribute, ref CertificateContext ctx);
}
