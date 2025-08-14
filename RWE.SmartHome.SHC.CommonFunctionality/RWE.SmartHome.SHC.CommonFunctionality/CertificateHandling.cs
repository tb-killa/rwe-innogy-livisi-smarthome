using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class CertificateHandling
{
	[Flags]
	public enum KeyContainerFlags : uint
	{
		CERT_SET_KEY_PROV_HANDLE_PROP_ID = 1u,
		CRYPT_MACHINE_KEYSET = 0x20u,
		CRYPT_SILENT = 0x40u
	}

	public enum KeySpecification : uint
	{
		AT_KEYEXCHANGE = 1u,
		AT_SIGNATURE
	}

	[Flags]
	public enum KeyGenerationFlags : ushort
	{
		CRYPT_EXPORTABLE = 1,
		CRYPT_USER_PROTECTED = 2,
		CRYPT_CREATE_SALT = 4,
		CRYPT_UPDATE_KEY = 8,
		CRYPT_NO_SALT = 0x10,
		CRYPT_PREGEN = 0x40,
		CRYPT_RECIPIENT = 0x10,
		CRYPT_INITIATOR = 0x40,
		CRYPT_ONLINE = 0x80,
		CRYPT_SF = 0x100,
		CRYPT_CREATE_IV = 0x200,
		CRYPT_KEK = 0x400,
		CRYPT_DATA_KEY = 0x800,
		CRYPT_VOLATILE = 0x1000,
		CRYPT_SGCKEY = 0x2000,
		CRYPT_ARCHIVABLE = 0x4000
	}

	public static string CreateCertificateRequest(string containerName, string provName, string subjectName, string certificateTemplate, string principalName, string[] alternateNames, KeyContainerFlags flags, KeySpecification keySpec, KeyGenerationFlags keyFlags, ushort keyLength)
	{
		byte[] array = new byte[4096];
		uint certReqLength = (uint)array.Length;
		if (!CreateCertificateRequest(containerName, provName, subjectName, certificateTemplate, principalName, alternateNames, (uint)alternateNames.Length, flags, keySpec, keyFlags, keyLength, array, ref certReqLength))
		{
			return null;
		}
		return Convert.ToBase64String(array, 0, (int)certReqLength);
	}

	public static bool AddCertificateResponseToStore(string containerName, string provName, KeyContainerFlags flags, KeySpecification keySpec, string certStore, string certResp)
	{
		byte[] array = Convert.FromBase64String(certResp);
		return AddCertificateResponseToStore(containerName, provName, (uint)flags, (uint)keySpec, certStore, array, (uint)array.Length);
	}

	[DllImport("CommonFunctionalityNative.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool CreateCertificateRequest(string containerName, string provName, string subjectName, string certificateTemplate, string principalName, string[] alternateNames, uint alternateNamesLength, KeyContainerFlags flags, KeySpecification keySpec, KeyGenerationFlags keyFlags, ushort keyLength, byte[] certReq, ref uint certReqLength);

	[DllImport("CommonFunctionalityNative.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool AddCertificateResponseToStore(string containerName, string provName, uint flags, uint keySpec, string certStore, byte[] certResp, uint certRespLength);

	public static string ExtractSubjectAlternateNameFromCertificate(byte[] cert)
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		if (!ExtractSubjectAlternateNameFromCertificate(cert, (uint)cert.Length, stringBuilder, (uint)stringBuilder.Capacity))
		{
			return string.Empty;
		}
		return stringBuilder.ToString();
	}

	[DllImport("CommonFunctionalityNative.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool ExtractSubjectAlternateNameFromCertificate(byte[] cert, uint certLength, StringBuilder subjectAlternateName, uint subjectAlternateNameLength);
}
