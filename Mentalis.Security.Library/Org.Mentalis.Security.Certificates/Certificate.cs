using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.Mentalis.Security.Cryptography;

namespace Org.Mentalis.Security.Certificates;

public class Certificate : ICloneable
{
	internal CertificateInfo m_CertInfo;

	private CertificateChain m_Chain;

	internal CertificateContext m_Context;

	private IntPtr m_Handle;

	private CertificateStore m_Store;

	public IntPtr Handle => m_Handle;

	internal CertificateStore Store
	{
		get
		{
			return m_Store;
		}
		set
		{
			if (m_Store != value)
			{
				m_Chain = null;
			}
			m_Store = value;
		}
	}

	public bool IsCurrent => SspiProvider.CertVerifyTimeValidity(IntPtr.Zero, m_Context.pCertInfo) == 0;

	public bool SupportsDataEncryption
	{
		get
		{
			if (GetIntendedKeyUsage() != 0)
			{
				return (GetIntendedKeyUsage() & 0x10) != 0;
			}
			return true;
		}
	}

	public bool SupportsDigitalSignature
	{
		get
		{
			if (GetIntendedKeyUsage() != 0)
			{
				return (GetIntendedKeyUsage() & 0x80) != 0;
			}
			return true;
		}
	}

	public RSA PrivateKey
	{
		get
		{
			int dwFlags = 0;
			int phCryptProv = 0;
			int pdwKeySpec = 0;
			int pfCallerFreeProv = 0;
			if (SspiProvider.CryptAcquireCertificatePrivateKey(Handle, dwFlags, IntPtr.Zero, ref phCryptProv, ref pdwKeySpec, ref pfCallerFreeProv) == 0)
			{
				throw new CertificateException("Could not acquire private key.");
			}
			if (pfCallerFreeProv != 0)
			{
				SspiProvider.CryptReleaseContext(phCryptProv, 0);
			}
			dwFlags = 64;
			int pcbData = 0;
			if (SspiProvider.CryptFindCertificateKeyProvInfo(Handle, dwFlags, IntPtr.Zero) == 0 || SspiProvider.CertGetCertificateContextProperty(Handle, 2, null, ref pcbData) == 0)
			{
				throw new CertificateException("Could not query the associated private key.");
			}
			IntPtr intPtr = Marshal.AllocHGlobal(pcbData);
			RSA rSA = null;
			try
			{
				SspiProvider.CertGetCertificateContextProperty(Handle, 2, intPtr, ref pcbData);
				CRYPT_KEY_PROV_INFO cRYPT_KEY_PROV_INFO = (CRYPT_KEY_PROV_INFO)Marshal.PtrToStructure(intPtr, typeof(CRYPT_KEY_PROV_INFO));
				CspParameters cspParameters = new CspParameters();
				cspParameters.KeyContainerName = cRYPT_KEY_PROV_INFO.pwszContainerName;
				cspParameters.ProviderName = cRYPT_KEY_PROV_INFO.pwszProvName;
				cspParameters.ProviderType = cRYPT_KEY_PROV_INFO.dwProvType;
				cspParameters.KeyNumber = cRYPT_KEY_PROV_INFO.dwKeySpec;
				if ((cRYPT_KEY_PROV_INFO.dwFlags & 0x20) != 0)
				{
					cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
				}
				return new RSACryptoServiceProvider(cspParameters);
			}
			catch (CertificateException ex)
			{
				throw ex;
			}
			catch (Exception inner)
			{
				throw new CertificateException("An error occurs while accessing the certificate's private key.", inner);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public RSA PublicKey
	{
		get
		{
			IntPtr intPtr = IntPtr.Zero;
			int containerHandle = CAPIProvider.ContainerHandle;
			int phKey = 0;
			RSA rSA = null;
			try
			{
				CERT_PUBLIC_KEY_INFO pInfo = new CERT_PUBLIC_KEY_INFO(m_CertInfo);
				int pdwDataLen = 0;
				if (SspiProvider.CryptImportPublicKeyInfoEx(containerHandle, 65537, ref pInfo, 0, 0, IntPtr.Zero, ref phKey) == 0)
				{
					throw new CertificateException("Could not obtain the handle of the public key.");
				}
				if (SspiProvider.CryptExportKey(phKey, 0, 6, 0, IntPtr.Zero, ref pdwDataLen) == 0)
				{
					throw new CertificateException("Could not get the size of the key.");
				}
				intPtr = Marshal.AllocHGlobal(pdwDataLen);
				if (SspiProvider.CryptExportKey(phKey, 0, 6, 0, intPtr, ref pdwDataLen) == 0)
				{
					throw new CertificateException("Could not export the key.");
				}
				PUBLIC_KEY_BLOB pUBLIC_KEY_BLOB = (PUBLIC_KEY_BLOB)Marshal.PtrToStructure(intPtr, typeof(PUBLIC_KEY_BLOB));
				if (pUBLIC_KEY_BLOB.magic != 826364754)
				{
					throw new CertificateException("This is not an RSA certificate.");
				}
				RSAParameters parameters = new RSAParameters
				{
					Exponent = ConvertIntToByteArray(pUBLIC_KEY_BLOB.pubexp)
				};
				IntPtr source = new IntPtr(intPtr.ToInt64() + Marshal.SizeOf(typeof(PUBLIC_KEY_BLOB)));
				parameters.Modulus = new byte[pUBLIC_KEY_BLOB.bitlen / 8];
				Marshal.Copy(source, parameters.Modulus, 0, parameters.Modulus.Length);
				Array.Reverse(parameters.Modulus);
				CspParameters cspParameters = new CspParameters();
				cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
				rSA = new RSACryptoServiceProvider(cspParameters);
				rSA.ImportParameters(parameters);
				return rSA;
			}
			finally
			{
				if (phKey != 0)
				{
					SspiProvider.CryptDestroyKey(phKey);
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}
	}

	public Certificate(Certificate certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException();
		}
		InitCertificate(certificate.Handle, duplicate: true, null);
	}

	public Certificate(IntPtr handle)
		: this(handle, duplicate: false)
	{
	}

	public Certificate(IntPtr handle, bool duplicate)
	{
		InitCertificate(handle, duplicate, null);
	}

	internal Certificate(IntPtr handle, CertificateStore store)
	{
		InitCertificate(handle, duplicate: false, store);
	}

	public object Clone()
	{
		return new Certificate(SspiProvider.CertDuplicateCertificateContext(Handle));
	}

	public static Certificate CreateFromPfxFile(string file, string password)
	{
		return CertificateStore.CreateFromPfxFile(file, password).FindCertificate();
	}

	public static Certificate CreateFromPfxFile(string file, string password, bool exportable)
	{
		return CertificateStore.CreateFromPfxFile(file, password, exportable).FindCertificate();
	}

	public static Certificate CreateFromPfxFile(string file, string password, bool exportable, KeysetLocation location)
	{
		return CertificateStore.CreateFromPfxFile(file, password, exportable, location).FindCertificate();
	}

	public static Certificate CreateFromPfxFile(byte[] file, string password)
	{
		return CertificateStore.CreateFromPfxFile(file, password).FindCertificate();
	}

	public static Certificate CreateFromPfxFile(byte[] file, string password, bool exportable)
	{
		return CertificateStore.CreateFromPfxFile(file, password, exportable).FindCertificate();
	}

	public static Certificate CreateFromPfxFile(byte[] file, string password, bool exportable, KeysetLocation location)
	{
		return CertificateStore.CreateFromPfxFile(file, password, exportable, location).FindCertificate();
	}

	public static Certificate CreateFromP7bFile(string file)
	{
		return CertificateStore.CreateFromP7bFile(file).FindCertificate();
	}

	public static Certificate CreateFromCerFile(byte[] file)
	{
		if (file == null)
		{
			throw new ArgumentNullException();
		}
		return CreateFromCerFile(file, 0, file.Length);
	}

	public static Certificate CreateFromCerFile(byte[] file, int offset, int size)
	{
		if (file == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset + size > file.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		IntPtr intPtr = Marshal.AllocHGlobal(size);
		Marshal.Copy(file, offset, intPtr, size);
		IntPtr intPtr2 = SspiProvider.CertCreateCertificateContext(65537, intPtr, size);
		Marshal.FreeHGlobal(intPtr);
		if (intPtr2 == IntPtr.Zero)
		{
			throw new CertificateException("Unable to load the specified certificate.");
		}
		return new Certificate(intPtr2);
	}

	public static Certificate CreateFromX509Certificate(X509Certificate certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException();
		}
		return CreateFromCerFile(certificate.GetRawCertData());
	}

	public static Certificate CreateFromPemFile(string filename)
	{
		return CreateFromPemFile(CertificateStore.GetFileContents(filename));
	}

	public static Certificate CreateFromPemFile(byte[] file)
	{
		if (file == null)
		{
			throw new ArgumentNullException();
		}
		string cert = Encoding.ASCII.GetString(file, 0, file.Length);
		string certString = GetCertString(cert, "CERTIFICATE");
		if (certString == null)
		{
			certString = GetCertString(cert, "X509 CERTIFICATE");
			if (certString == null)
			{
				throw new CertificateException("The specified PEM file does not contain a certificate.");
			}
		}
		byte[] file2 = Convert.FromBase64String(certString);
		return CreateFromCerFile(file2);
	}

	private static string GetCertString(string cert, string delimiter)
	{
		int num = cert.IndexOf("-----BEGIN " + delimiter + "-----");
		if (num < 0)
		{
			return null;
		}
		int num2 = cert.IndexOf("-----END " + delimiter + "-----", num);
		if (num2 < 0)
		{
			return null;
		}
		int num3 = delimiter.Length + 16;
		int length = num2 - (num + num3);
		return cert.Substring(num + num3, length);
	}

	private void InitCertificate(IntPtr handle, bool duplicate, CertificateStore store)
	{
		if (handle == IntPtr.Zero)
		{
			throw new ArgumentException("Invalid certificate handle!");
		}
		if (duplicate)
		{
			m_Handle = SspiProvider.CertDuplicateCertificateContext(handle);
		}
		else
		{
			m_Handle = handle;
		}
		m_Context = (CertificateContext)Marshal.PtrToStructure(handle, typeof(CertificateContext));
		m_CertInfo = (CertificateInfo)Marshal.PtrToStructure(m_Context.pCertInfo, typeof(CertificateInfo));
		if (store == null)
		{
			m_Store = null;
		}
		else
		{
			m_Store = store;
		}
	}

	internal CertificateInfo GetCertificateInfo()
	{
		return m_CertInfo;
	}

	~Certificate()
	{
		if (Handle != IntPtr.Zero)
		{
			SspiProvider.CertFreeCertificateContext(Handle);
			m_Handle = IntPtr.Zero;
		}
	}

	public override string ToString()
	{
		return GetType().FullName;
	}

	public string ToString(bool verbose)
	{
		if (verbose)
		{
			return "CERTIFICATE:\r\n        Format:  X509\r\n        Name:  " + GetName() + "\r\n        Issuing CA:  " + GetIssuerName() + "\r\n        Key Algorithm:  " + GetKeyAlgorithm() + "\r\n        Serial Number:  " + GetSerialNumberString() + "\r\n        Key Alogrithm Parameters:  " + GetKeyAlgorithmParametersString() + "\r\n        Public Key:  " + GetPublicKeyString();
		}
		return ToString();
	}

	public byte[] GetCertHash()
	{
		return GetCertHash(HashType.SHA1);
	}

	public byte[] GetCertHash(HashType type)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(256);
		try
		{
			int pcbData = 256;
			if (SspiProvider.CertGetCertificateContextProperty(Handle, (int)type, intPtr, ref pcbData) == 0 || pcbData <= 0 || pcbData > 256)
			{
				throw new CertificateException("An error occurs while retrieving the hash of the certificate.");
			}
			byte[] array = new byte[pcbData];
			Marshal.Copy(intPtr, array, 0, pcbData);
			return array;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public string GetCertHashString()
	{
		return GetCertHashString(HashType.SHA1);
	}

	public string GetCertHashString(HashType type)
	{
		return BytesToString(GetCertHash(type));
	}

	private string BytesToString(byte[] buffer)
	{
		string text = "";
		for (int i = 0; i < buffer.Length; i++)
		{
			text += buffer[i].ToString("X2");
		}
		return text;
	}

	public DateTime GetEffectiveDate()
	{
		return DateTime.FromFileTime(m_CertInfo.NotBefore);
	}

	public DateTime GetExpirationDate()
	{
		return DateTime.FromFileTime(m_CertInfo.NotAfter);
	}

	public string GetIssuerName()
	{
		int num = SspiProvider.CertGetNameString(Handle, 4, 65537, IntPtr.Zero, IntPtr.Zero, 0);
		if (num <= 0)
		{
			throw new CertificateException("An error occurs while requesting the issuer name.");
		}
		num *= 2;
		byte[] array = new byte[num];
		SspiProvider.CertGetNameString(Handle, 4, 65537, IntPtr.Zero, array, num);
		return Encoding.Unicode.GetString(array, 0, num - 2);
	}

	public string GetKeyAlgorithm()
	{
		return CeMarshal.PtrToStringAnsi(m_CertInfo.SignatureAlgorithmpszObjId);
	}

	public byte[] GetKeyAlgorithmParameters()
	{
		byte[] array = new byte[m_CertInfo.SignatureAlgorithmParameterscbData];
		if (array.Length > 0)
		{
			Marshal.Copy(m_CertInfo.SignatureAlgorithmParameterspbData, array, 0, array.Length);
		}
		return array;
	}

	public string GetKeyAlgorithmParametersString()
	{
		return BytesToString(GetKeyAlgorithmParameters());
	}

	public byte[] GetPublicKey()
	{
		byte[] array = new byte[m_CertInfo.SubjectPublicKeyInfoPublicKeycbData];
		Marshal.Copy(m_CertInfo.SubjectPublicKeyInfoPublicKeypbData, array, 0, array.Length);
		return array;
	}

	public string GetPublicKeyString()
	{
		return BytesToString(GetPublicKey());
	}

	public byte[] GetRawCertData()
	{
		byte[] array = new byte[m_Context.cbCertEncoded];
		Marshal.Copy(m_Context.pbCertEncoded, array, 0, array.Length);
		return array;
	}

	public string GetRawCertDataString()
	{
		return BytesToString(GetRawCertData());
	}

	public byte[] GetSerialNumber()
	{
		byte[] array = new byte[m_CertInfo.SerialNumbercbData];
		if (array.Length > 0)
		{
			Marshal.Copy(m_CertInfo.SerialNumberpbData, array, 0, array.Length);
			Array.Reverse(array);
		}
		return array;
	}

	public string GetSerialNumberString()
	{
		return BytesToString(GetSerialNumber());
	}

	public int GetPublicKeyLength()
	{
		return SspiProvider.CertGetPublicKeyLength(65537, new IntPtr(m_Context.pCertInfo.ToInt64() + 56));
	}

	public DistinguishedName GetDistinguishedName()
	{
		return new DistinguishedName(m_CertInfo.SubjectpbData, m_CertInfo.SubjectcbData);
	}

	public Extension[] GetExtensions()
	{
		Extension[] array = new Extension[m_CertInfo.cExtension];
		int num = 8 + IntPtr.Size * 2;
		IntPtr ptr = m_CertInfo.rgExtension;
		Type typeFromHandle = typeof(CERT_EXTENSION);
		for (int i = 0; i < m_CertInfo.cExtension; i++)
		{
			CERT_EXTENSION cERT_EXTENSION = (CERT_EXTENSION)Marshal.PtrToStructure(ptr, typeFromHandle);
			array[i] = new Extension(CeMarshal.PtrToStringAnsi(cERT_EXTENSION.pszObjId), cERT_EXTENSION.fCritical != 0, new byte[cERT_EXTENSION.ValuecbData]);
			Marshal.Copy(cERT_EXTENSION.ValuepbData, array[i].EncodedValue, 0, cERT_EXTENSION.ValuecbData);
			ptr = new IntPtr(ptr.ToInt64() + num);
		}
		return array;
	}

	public Extension FindExtension(string oid)
	{
		if (oid == null)
		{
			throw new ArgumentNullException();
		}
		IntPtr intPtr = SspiProvider.CertFindExtension(oid, m_CertInfo.cExtension, m_CertInfo.rgExtension);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		CERT_EXTENSION cERT_EXTENSION = (CERT_EXTENSION)Marshal.PtrToStructure(intPtr, typeof(CERT_EXTENSION));
		Extension extension = new Extension(CeMarshal.PtrToStringAnsi(cERT_EXTENSION.pszObjId), cERT_EXTENSION.fCritical != 0, new byte[cERT_EXTENSION.ValuecbData]);
		Marshal.Copy(cERT_EXTENSION.ValuepbData, extension.EncodedValue, 0, cERT_EXTENSION.ValuecbData);
		return extension;
	}

	public static object DecodeExtension(Extension extension, int oid, Type returnType)
	{
		return DecodeExtension(extension, new IntPtr(oid), returnType);
	}

	public static object DecodeExtension(Extension extension, string oid, Type returnType)
	{
		if (oid == null)
		{
			throw new ArgumentNullException("oid");
		}
		IntPtr intPtr = CeMarshal.StringToHGlobalAnsi(oid);
		try
		{
			return DecodeExtension(extension, intPtr, returnType);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	protected static object DecodeExtension(Extension extension, IntPtr oid, Type returnType)
	{
		if (extension.EncodedValue == null || (object)returnType == null)
		{
			throw new ArgumentNullException();
		}
		int pcbStructInfo = 0;
		if (SspiProvider.CryptDecodeObject(65537, oid, extension.EncodedValue, extension.EncodedValue.Length, 0, IntPtr.Zero, ref pcbStructInfo) == 0)
		{
			throw new CertificateException("Could not decode the extension.");
		}
		IntPtr intPtr = Marshal.AllocHGlobal(pcbStructInfo);
		try
		{
			if (SspiProvider.CryptDecodeObject(65537, oid, extension.EncodedValue, extension.EncodedValue.Length, 0, intPtr, ref pcbStructInfo) == 0)
			{
				throw new CertificateException("Could not decode the extension.");
			}
			return returnType.GetConstructor(new Type[2]
			{
				intPtr.GetType(),
				pcbStructInfo.GetType()
			}).Invoke(new object[2] { intPtr, pcbStructInfo });
		}
		catch (CertificateException ex)
		{
			throw ex;
		}
		catch (Exception inner)
		{
			throw new CertificateException("Unable to instantiate the specified object type.", inner);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public string GetName()
	{
		int pcbStructInfo = 0;
		SspiProvider.CryptDecodeObject(65537, new IntPtr(20), m_CertInfo.SubjectpbData, m_CertInfo.SubjectcbData, 0, IntPtr.Zero, ref pcbStructInfo);
		if (pcbStructInfo <= 0)
		{
			throw new CertificateException("Unable to decode the name of the certificate.");
		}
		IntPtr intPtr = IntPtr.Zero;
		string text = null;
		try
		{
			intPtr = Marshal.AllocHGlobal(pcbStructInfo);
			if (SspiProvider.CryptDecodeObject(65537, new IntPtr(20), m_CertInfo.SubjectpbData, m_CertInfo.SubjectcbData, 0, intPtr, ref pcbStructInfo) == 0)
			{
				throw new CertificateException("Unable to decode the name of the certificate.");
			}
			IntPtr intPtr2 = SspiProvider.CertFindRDNAttr("2.5.4.3", intPtr);
			if (intPtr2 == IntPtr.Zero)
			{
				intPtr2 = SspiProvider.CertFindRDNAttr("1.2.840.113549.1.9.2", intPtr);
			}
			if (intPtr2 == IntPtr.Zero)
			{
				intPtr2 = SspiProvider.CertFindRDNAttr("2.5.4.10", intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				RdnAttribute rdnAttribute = (RdnAttribute)Marshal.PtrToStructure(intPtr2, typeof(RdnAttribute));
				text = Marshal.PtrToStringUni(rdnAttribute.pbData, rdnAttribute.cbData / 2);
			}
		}
		catch (CertificateException ex)
		{
			throw ex;
		}
		catch (Exception inner)
		{
			throw new CertificateException("Could not get certificate attributes.", inner);
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
		if (text == null)
		{
			throw new CertificateException("Certificate does not have a name attribute.");
		}
		return text;
	}

	public int GetIntendedKeyUsage()
	{
		IntPtr intPtr = Marshal.AllocHGlobal(4);
		SspiProvider.CertGetIntendedKeyUsage(65537, m_Context.pCertInfo, intPtr, 4);
		byte[] array = new byte[4];
		Marshal.Copy(intPtr, array, 0, 4);
		Marshal.FreeHGlobal(intPtr);
		return BitConverter.ToInt32(array, 0);
	}

	public CertificateChain GetCertificateChain()
	{
		if (m_Chain == null)
		{
			m_Chain = new CertificateChain(this, Store);
		}
		return m_Chain;
	}

	public bool HasPrivateKey()
	{
		int phCryptProv = 0;
		int pdwKeySpec = 0;
		int pfCallerFreeProv = 0;
		bool result = false;
		if (SspiProvider.CryptAcquireCertificatePrivateKey(Handle, 68, IntPtr.Zero, ref phCryptProv, ref pdwKeySpec, ref pfCallerFreeProv) != 0)
		{
			result = true;
		}
		if (pfCallerFreeProv != 0)
		{
			SspiProvider.CryptReleaseContext(phCryptProv, 0);
		}
		return result;
	}

	public string GetFormat()
	{
		return "X509";
	}

	public override int GetHashCode()
	{
		byte[] certHash = GetCertHash();
		byte[] array = new byte[4];
		if (certHash.Length < array.Length)
		{
			Array.Copy(certHash, 0, array, 0, certHash.Length);
		}
		else
		{
			Array.Copy(certHash, 0, array, 0, array.Length);
		}
		return BitConverter.ToInt32(array, 0);
	}

	public virtual bool Equals(Certificate other)
	{
		if (other == null)
		{
			return false;
		}
		return SspiProvider.CertCompareCertificate(65537, m_Context.pCertInfo, other.m_Context.pCertInfo) != 0;
	}

	public virtual bool Equals(X509Certificate other)
	{
		if (other == null)
		{
			return false;
		}
		return other.GetCertHashString() == GetCertHashString();
	}

	public override bool Equals(object other)
	{
		try
		{
			return Equals((Certificate)other);
		}
		catch
		{
			try
			{
				return Equals((X509Certificate)other);
			}
			catch
			{
				return false;
			}
		}
	}

	public static string[] GetValidUsages(Certificate[] certificates)
	{
		if (certificates == null)
		{
			throw new ArgumentNullException();
		}
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = Marshal.AllocHGlobal(certificates.Length * IntPtr.Size);
		try
		{
			for (int i = 0; i < certificates.Length; i++)
			{
				if (certificates[i] == null)
				{
					throw new ArgumentException();
				}
				Marshal.WriteInt32(intPtr2, i * IntPtr.Size, certificates[i].Handle.ToInt32());
			}
			int cNumOIDs = 0;
			int pcbOIDs = 0;
			if (SspiProvider.CertGetValidUsages(certificates.Length, intPtr2, ref cNumOIDs, intPtr, ref pcbOIDs) == 0)
			{
				throw new CertificateException("Unable to get the valid usages.");
			}
			if (cNumOIDs == -1)
			{
				return null;
			}
			intPtr = Marshal.AllocHGlobal(pcbOIDs);
			if (SspiProvider.CertGetValidUsages(certificates.Length, intPtr2, ref cNumOIDs, intPtr, ref pcbOIDs) == 0)
			{
				throw new CertificateException("Unable to get the valid usages.");
			}
			string[] array = new string[cNumOIDs];
			for (int j = 0; j < cNumOIDs; j++)
			{
				array[j] = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(intPtr, j * IntPtr.Size));
			}
			return array;
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr2);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public void ToPfxFile(string filename, string password, bool withPrivateKeys, bool withParents)
	{
		CreateCertStore(withParents).ToPfxFile(filename, password, withPrivateKeys);
	}

	public byte[] ToPfxBuffer(string password, bool withPrivateKeys, bool withParents)
	{
		return CreateCertStore(withParents).ToPfxBuffer(password, withPrivateKeys);
	}

	private CertificateStore CreateCertStore(bool withParents)
	{
		CertificateStore certificateStore = new CertificateStore();
		if (withParents)
		{
			Certificate[] certificates = GetCertificateChain().GetCertificates();
			for (int i = 0; i < certificates.Length; i++)
			{
				certificateStore.AddCertificate(certificates[i]);
			}
		}
		else
		{
			certificateStore.AddCertificate(this);
		}
		return certificateStore;
	}

	public void ToCerFile(string filename)
	{
		SaveToFile(GetCertificateBuffer(), filename);
	}

	public byte[] ToCerBuffer()
	{
		return GetCertificateBuffer();
	}

	private byte[] GetCertificateBuffer()
	{
		byte[] array = new byte[m_Context.cbCertEncoded];
		Marshal.Copy(m_Context.pbCertEncoded, array, 0, m_Context.cbCertEncoded);
		return array;
	}

	private void SaveToFile(byte[] buffer, string filename)
	{
		if (filename == null)
		{
			throw new ArgumentNullException();
		}
		try
		{
			FileStream fileStream = File.Open(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None);
			fileStream.Write(buffer, 0, buffer.Length);
			fileStream.Close();
		}
		catch (Exception innerException)
		{
			throw new IOException("Could not write data to file.", innerException);
		}
	}

	public X509Certificate ToX509()
	{
		CertificateContext certificateContext = (CertificateContext)Marshal.PtrToStructure(SspiProvider.CertDuplicateCertificateContext(Handle), typeof(CertificateContext));
		byte[] array = new byte[certificateContext.cbCertEncoded];
		Marshal.Copy(certificateContext.pbCertEncoded, array, 0, certificateContext.cbCertEncoded);
		return new X509Certificate(array);
	}

	internal IntPtr DuplicateHandle()
	{
		return SspiProvider.CertDuplicateCertificateContext(Handle);
	}

	public static Certificate CreateFromBase64String(string rawString)
	{
		if (rawString == null)
		{
			throw new ArgumentNullException("rawString");
		}
		return CreateFromCerFile(Convert.FromBase64String(rawString));
	}

	public string ToBase64String()
	{
		byte[] array = ToCerBuffer();
		return Convert.ToBase64String(array, 0, array.Length);
	}

	public byte[] ToPemBuffer()
	{
		return Encoding.ASCII.GetBytes("-----BEGIN CERTIFICATE-----\r\n" + ToBase64String() + "\r\n-----END CERTIFICATE-----\r\n");
	}

	public byte[] GetKeyIdentifier()
	{
		int pcbData = 0;
		SspiProvider.CertGetCertificateContextProperty(Handle, 20, null, ref pcbData);
		byte[] array = new byte[pcbData];
		SspiProvider.CertGetCertificateContextProperty(Handle, 20, array, ref pcbData);
		return array;
	}

	internal byte[] ConvertIntToByteArray(int dwInput)
	{
		int num = 0;
		byte[] array = new byte[8];
		if (dwInput == 0)
		{
			return new byte[1];
		}
		int num2 = dwInput;
		while (num2 > 0)
		{
			array[num] = (byte)(num2 & 0xFF);
			num2 >>= 8;
			num++;
		}
		byte[] array2 = new byte[num];
		if (BitConverter.IsLittleEndian)
		{
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[num - i - 1];
			}
		}
		else
		{
			for (int j = 0; j < num; j++)
			{
				array2[j] = array[j];
			}
		}
		return array2;
	}

	public void AssociateWithPrivateKey(string pvkFile, string password)
	{
		AssociateWithPrivateKey(pvkFile, password, exportable: false);
	}

	public void AssociateWithPrivateKey(string pvkFile, string password, bool exportable)
	{
		try
		{
			if (!File.Exists(pvkFile))
			{
				throw new FileNotFoundException("The PVK file could not be found.");
			}
		}
		catch (FileNotFoundException ex)
		{
			throw ex;
		}
		catch (Exception ex2)
		{
			throw new FileNotFoundException("The PVK file could not be found. " + ex2.Message);
		}
		byte[] array = new byte[24];
		FileStream fileStream = File.Open(pvkFile, FileMode.Open, FileAccess.Read, FileShare.Read);
		fileStream.Read(array, 0, array.Length);
		if (BitConverter.ToUInt32(array, 0) != 2964713758u)
		{
			throw new CertificateException("The specified file is not a valid PVK file.");
		}
		int dwKeySpec = BitConverter.ToInt32(array, 8);
		int num = BitConverter.ToInt32(array, 12);
		int num2 = BitConverter.ToInt32(array, 16);
		int num3 = BitConverter.ToInt32(array, 20);
		byte[] array2 = new byte[num2];
		byte[] array3 = new byte[num3];
		fileStream.Read(array2, 0, array2.Length);
		fileStream.Read(array3, 0, array3.Length);
		if (num != 0)
		{
			if (password == null)
			{
				throw new ArgumentNullException();
			}
			byte[] bytes = Encoding.ASCII.GetBytes(password);
			byte[] array4 = new byte[array2.Length + password.Length];
			Array.Copy(array2, 0, array4, 0, array2.Length);
			Array.Copy(bytes, 0, array4, array2.Length, bytes.Length);
			byte[] array5 = TryDecrypt(array3, 8, array3.Length - 8, array4, 16);
			if (array5 == null)
			{
				array5 = TryDecrypt(array3, 8, array3.Length - 8, array4, 5);
				if (array5 == null)
				{
					throw new CertificateException("The PVK file could not be decrypted. [wrong password?]");
				}
			}
			Array.Copy(array5, 0, array3, 8, array5.Length);
			Array.Clear(array5, 0, array5.Length);
			Array.Clear(bytes, 0, bytes.Length);
			Array.Clear(array4, 0, array4.Length);
		}
		int phKey = 0;
		int dwFlags = 0;
		if (exportable)
		{
			dwFlags = 1;
		}
		int containerHandle = CAPIProvider.ContainerHandle;
		if (SspiProvider.CryptImportKey(containerHandle, array3, array3.Length, 0, dwFlags, ref phKey) == 0)
		{
			throw new CertificateException("Could not import the private key from the PVK file.");
		}
		CRYPT_KEY_PROV_INFO pvData = new CRYPT_KEY_PROV_INFO
		{
			pwszContainerName = "{48959A69-B181-4cdd-B135-7565701307C5}",
			pwszProvName = null,
			dwProvType = 1,
			dwFlags = 0,
			cProvParam = 0,
			rgProvParam = IntPtr.Zero,
			dwKeySpec = dwKeySpec
		};
		if (SspiProvider.CertSetCertificateContextProperty(Handle, 2, 0, ref pvData) == 0)
		{
			throw new CertificateException("Could not associate the private key with the certificate.");
		}
		SspiProvider.CryptDestroyKey(phKey);
		Array.Clear(array3, 0, array3.Length);
	}

	public void ExportPrivateKey(string pvkFile, string password)
	{
		if (!HasPrivateKey())
		{
			throw new CertificateException("The certificate does not have an associated private key.");
		}
		int num = 0;
		int phCryptProv = 0;
		int pdwKeySpec = 0;
		int pfCallerFreeProv = 0;
		int phUserKey = 0;
		int pdwDataLen = 0;
		num = 64;
		if (SspiProvider.CryptAcquireCertificatePrivateKey(Handle, num, IntPtr.Zero, ref phCryptProv, ref pdwKeySpec, ref pfCallerFreeProv) == 0)
		{
			throw new CertificateException("Could not acquire private key.");
		}
		if (SspiProvider.CryptGetUserKey(phCryptProv, pdwKeySpec, ref phUserKey) == 0)
		{
			throw new CertificateException("Could not retrieve a handle of the private key.");
		}
		if (SspiProvider.CryptExportKey(phUserKey, 0, 7, 0, IntPtr.Zero, ref pdwDataLen) == 0 && Marshal.GetLastWin32Error() != 234)
		{
			throw new CertificateException("Could not export the private key.");
		}
		byte[] array = new byte[pdwDataLen];
		if (SspiProvider.CryptExportKey(phUserKey, 0, 7, 0, array, ref pdwDataLen) == 0)
		{
			throw new CertificateException("Could not export the private key.");
		}
		if (pfCallerFreeProv != 0)
		{
			SspiProvider.CryptReleaseContext(phCryptProv, 0);
		}
		uint value = 2964713758u;
		int value2 = 0;
		int num2 = ((password != null) ? 1 : 0);
		byte[] array2;
		if (num2 == 0)
		{
			array2 = new byte[0];
		}
		else
		{
			array2 = new byte[16];
			new RNGCryptoServiceProvider().GetBytes(array2);
			byte[] bytes = Encoding.ASCII.GetBytes(password);
			SHA1 sHA = SHA1.Create();
			sHA.TransformBlock(array2, 0, array2.Length, array2, 0);
			sHA.TransformFinalBlock(bytes, 0, bytes.Length);
			bytes = new byte[16];
			Array.Copy(sHA.Hash, 0, bytes, 0, bytes.Length);
			ICryptoTransform cryptoTransform = RC4.Create().CreateEncryptor(bytes, null);
			cryptoTransform.TransformBlock(array, 8, array.Length - 8, array, 8);
			cryptoTransform.Dispose();
			sHA.Clear();
		}
		int value3 = array2.Length;
		FileStream fileStream = null;
		try
		{
			fileStream = File.Open(pvkFile, FileMode.Create, FileAccess.Write, FileShare.Read);
			fileStream.Write(BitConverter.GetBytes(value), 0, 4);
			fileStream.Write(BitConverter.GetBytes(value2), 0, 4);
			fileStream.Write(BitConverter.GetBytes(pdwKeySpec), 0, 4);
			fileStream.Write(BitConverter.GetBytes(num2), 0, 4);
			fileStream.Write(BitConverter.GetBytes(value3), 0, 4);
			fileStream.Write(BitConverter.GetBytes(pdwDataLen), 0, 4);
			if (array2.Length > 0)
			{
				fileStream.Write(array2, 0, array2.Length);
			}
			fileStream.Write(array, 0, array.Length);
		}
		catch (IOException ex)
		{
			throw ex;
		}
		catch (Exception innerException)
		{
			throw new IOException("An error occurs while writing the file.", innerException);
		}
		finally
		{
			fileStream?.Close();
		}
	}

	private byte[] TryDecrypt(byte[] buffer, int offset, int length, byte[] password, int keyLen)
	{
		byte[] array = new byte[16];
		Array.Copy(SHA1.Create().ComputeHash(password, 0, password.Length), 0, array, 0, keyLen);
		byte[] array2 = RC4.Create().CreateDecryptor(array, null).TransformFinalBlock(buffer, offset, length);
		if (array2[0] != 82 || array2[1] != 83 || array2[2] != 65 || array2[3] != 50)
		{
			return null;
		}
		return array2;
	}

	public bool VerifyRevocation(byte[] crl)
	{
		if (crl == null)
		{
			throw new ArgumentNullException();
		}
		IntPtr intPtr = SspiProvider.CertCreateCRLContext(65537, crl, crl.Length);
		if (intPtr == IntPtr.Zero)
		{
			throw new ArgumentException("The parameter is invalid.", "crl");
		}
		IntPtr ppCrlEntry = IntPtr.Zero;
		if (SspiProvider.CertFindCertificateInCRL(Handle, intPtr, 0, IntPtr.Zero, ref ppCrlEntry) == 0)
		{
			throw new CertificateException("Unable to search the specified CRL for the certificate.");
		}
		if (ppCrlEntry == IntPtr.Zero)
		{
			return true;
		}
		return false;
	}
}
