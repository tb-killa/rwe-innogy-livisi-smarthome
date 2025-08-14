using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Org.Mentalis.Security.Certificates;

public class CertificateStore
{
	public const string CAStore = "CA";

	public const string MyStore = "My";

	public const string RootStore = "Root";

	public const string SoftwarePublisherStore = "SPC";

	public const string TrustStore = "Trust";

	public const string UnTrustedStore = "Disallowed";

	private static Hashtable m_CachedStores = new Hashtable();

	private IntPtr m_Handle;

	public IntPtr Handle => m_Handle;

	public CertificateStore(CertificateStore store)
	{
		if (store == null)
		{
			throw new ArgumentNullException();
		}
		InitStore(store.m_Handle, duplicate: true);
	}

	public CertificateStore(IntPtr handle)
		: this(handle, duplicate: false)
	{
	}

	public CertificateStore(IntPtr handle, bool duplicate)
	{
		InitStore(handle, duplicate);
	}

	public CertificateStore(string store)
		: this(StoreLocation.CurrentUser, store)
	{
	}

	public CertificateStore(StoreLocation location, string store)
	{
		if (store == null)
		{
			throw new ArgumentNullException("The name of the store cannot be a null reference.");
		}
		m_Handle = SspiProvider.CertOpenStore(new IntPtr(9), 0, 0, (int)location, store);
		if (m_Handle == IntPtr.Zero)
		{
			throw new CertificateException("An error occurs while opening the specified store.");
		}
	}

	public CertificateStore()
	{
		m_Handle = SspiProvider.CertOpenStore(new IntPtr(2), 65537, 0, 0, null);
		if (m_Handle == IntPtr.Zero)
		{
			throw new CertificateException("An error occurs while creating the store.");
		}
	}

	public CertificateStore(IEnumerable certs)
		: this()
	{
		IEnumerator enumerator = certs.GetEnumerator();
		while (enumerator.MoveNext())
		{
			AddCertificate((Certificate)enumerator.Current);
		}
	}

	public CertificateStore(byte[] buffer, CertificateStoreType type)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		DataBlob pvPara = default(DataBlob);
		pvPara.cbData = buffer.Length;
		pvPara.pbData = Marshal.AllocHGlobal(pvPara.cbData);
		Marshal.Copy(buffer, 0, pvPara.pbData, buffer.Length);
		m_Handle = SspiProvider.CertOpenStoreData((type != CertificateStoreType.Pkcs7Message) ? new IntPtr(6) : new IntPtr(5), 65537, IntPtr.Zero, 0, ref pvPara);
		Marshal.FreeHGlobal(pvPara.pbData);
		if (m_Handle == IntPtr.Zero)
		{
			throw new CertificateException("An error occurs while opening the store.");
		}
	}

	public static CertificateStore CreateFromPfxFile(string file, string password)
	{
		return CreateFromPfxFile(GetFileContents(file), password);
	}

	public static CertificateStore CreateFromPfxFile(string file, string password, bool exportable)
	{
		return CreateFromPfxFile(GetFileContents(file), password, exportable);
	}

	public static CertificateStore CreateFromPfxFile(string file, string password, bool exportable, KeysetLocation location)
	{
		return CreateFromPfxFile(GetFileContents(file), password, exportable, location);
	}

	public static CertificateStore CreateFromPfxFile(byte[] file, string password)
	{
		return CreateFromPfxFile(file, password, exportable: false);
	}

	public static CertificateStore CreateFromPfxFile(byte[] file, string password, bool exportable)
	{
		return CreateFromPfxFile(file, password, exportable, KeysetLocation.Default);
	}

	public static CertificateStore CreateFromPfxFile(byte[] file, string password, bool exportable, KeysetLocation location)
	{
		if (password == null || file == null)
		{
			throw new ArgumentNullException("The arguments cannot be null references.");
		}
		DataBlob pPFX = new DataBlob
		{
			cbData = file.Length
		};
		IntPtr intPtr = Marshal.AllocHGlobal(file.Length);
		Marshal.Copy(file, 0, intPtr, file.Length);
		pPFX.pbData = intPtr;
		try
		{
			if (SspiProvider.PFXIsPFXBlob(ref pPFX) != 0)
			{
				if (SspiProvider.PFXVerifyPassword(ref pPFX, password, 0) != 0)
				{
					int num = (int)location;
					if (exportable)
					{
						num |= 1;
					}
					IntPtr handle = SspiProvider.PFXImportCertStore(ref pPFX, password, num);
					if (handle.Equals(IntPtr.Zero))
					{
						throw new CertificateException("Unable to import the PFX file! [error code = " + Marshal.GetLastWin32Error() + "]");
					}
					return new CertificateStore(handle);
				}
				throw new ArgumentException("The specified password is invalid.");
			}
			throw new CertificateException("The specified file is not a PFX file.");
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	internal static byte[] GetFileContents(string file)
	{
		if (file == null)
		{
			throw new ArgumentNullException();
		}
		try
		{
			FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			byte[] array = new byte[fileStream.Length];
			for (int i = fileStream.Read(array, 0, array.Length); i < fileStream.Length; i += fileStream.Read(array, i, array.Length - i))
			{
			}
			fileStream.Close();
			return array;
		}
		catch (Exception innerException)
		{
			throw new IOException("An error occurs while reading from the file.", innerException);
		}
	}

	public static CertificateStore CreateFromP7bFile(string file)
	{
		if (file == null)
		{
			throw new ArgumentNullException("The filename cannot be a null reference.");
		}
		return new CertificateStore(GetFileContents(file), CertificateStoreType.Pkcs7Message);
	}

	protected void InitStore(IntPtr handle, bool duplicate)
	{
		if (handle == IntPtr.Zero)
		{
			throw new ArgumentException("Invalid certificate store handle!");
		}
		if (duplicate)
		{
			m_Handle = SspiProvider.CertDuplicateStore(handle);
		}
		else
		{
			m_Handle = handle;
		}
	}

	public Certificate FindCertificate()
	{
		return FindCertificate(null);
	}

	public Certificate FindCertificate(Certificate previous)
	{
		IntPtr handle = SspiProvider.CertFindCertificateInStore(pPrevCertContext: (previous != null) ? SspiProvider.CertDuplicateCertificateContext(previous.Handle) : IntPtr.Zero, hCertStore: Handle, dwCertEncodingType: 1, dwFindFlags: 0, dwFindType: 0, pvFindPara: IntPtr.Zero);
		if (handle.Equals(IntPtr.Zero))
		{
			return null;
		}
		return new Certificate(handle, this);
	}

	public Certificate FindCertificateByUsage(string[] keyUsage)
	{
		return FindCertificateByUsage(keyUsage, null);
	}

	public Certificate FindCertificateByUsage(string[] keyUsage, Certificate previous)
	{
		if (keyUsage == null)
		{
			throw new ArgumentNullException();
		}
		if (keyUsage.Length == 0)
		{
			throw new ArgumentException();
		}
		int num = 0;
		for (int i = 0; i < keyUsage.Length; i++)
		{
			if (keyUsage[i] == null || keyUsage[i].Length == 0)
			{
				throw new ArgumentException();
			}
			num += keyUsage[i].Length + 1;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		IntPtr intPtr2 = Marshal.AllocHGlobal(IntPtr.Size * keyUsage.Length);
		num = 0;
		IntPtr destination = intPtr;
		for (int j = 0; j < keyUsage.Length; j++)
		{
			Marshal.Copy(Encoding.ASCII.GetBytes(keyUsage[j] + "\0"), 0, destination, keyUsage[j].Length + 1);
			Marshal.WriteInt32(intPtr2, j * IntPtr.Size, destination.ToInt32());
			destination = new IntPtr(intPtr.ToInt64() + keyUsage[j].Length + 1);
		}
		TrustListUsage trustListUsage = new TrustListUsage
		{
			cUsageIdentifier = keyUsage.Length,
			rgpszUsageIdentifier = intPtr2
		};
		if (previous == null)
		{
			_ = IntPtr.Zero;
		}
		else
		{
			SspiProvider.CertDuplicateCertificateContext(previous.Handle);
		}
		IntPtr intPtr3 = SspiProvider.CertEnumCertificatesInStore(Handle, 0);
		bool flag = false;
		while (intPtr3 != IntPtr.Zero && !flag)
		{
			int pcbUsage = 0;
			SspiProvider.CertGetEnhancedKeyUsage(intPtr3, 0, IntPtr.Zero, ref pcbUsage);
			IntPtr intPtr4 = Marshal.AllocHGlobal(pcbUsage);
			SspiProvider.CertGetEnhancedKeyUsage(intPtr3, 0, intPtr4, ref pcbUsage);
			trustListUsage = (TrustListUsage)Marshal.PtrToStructure(intPtr4, typeof(TrustListUsage));
			intPtr4 = trustListUsage.rgpszUsageIdentifier;
			for (int k = 0; k < trustListUsage.cUsageIdentifier; k++)
			{
				if (flag)
				{
					break;
				}
				string text = CeMarshal.PtrToStringAnsi(Marshal.ReadIntPtr(intPtr4));
				foreach (string text2 in keyUsage)
				{
					if (text == text2)
					{
						flag = true;
						break;
					}
				}
				intPtr4 = (IntPtr)((int)intPtr4 + 4);
			}
			if (!flag)
			{
				intPtr3 = SspiProvider.CertEnumCertificatesInStore(Handle, intPtr3.ToInt32());
			}
		}
		Marshal.FreeHGlobal(intPtr2);
		Marshal.FreeHGlobal(intPtr);
		if (flag)
		{
			return new Certificate(intPtr3);
		}
		return null;
	}

	public Certificate FindCertificateByHash(byte[] hash)
	{
		return FindCertificateByHash(hash, HashType.SHA1);
	}

	public Certificate FindCertificateByHash(byte[] hash, HashType hashType)
	{
		if (hash == null)
		{
			throw new ArgumentNullException();
		}
		int dwFindType = hashType switch
		{
			HashType.SHA1 => 65536, 
			HashType.MD5 => 262144, 
			_ => 65536, 
		};
		DataBlob pvFindPara = new DataBlob
		{
			cbData = hash.Length,
			pbData = Marshal.AllocHGlobal(hash.Length)
		};
		Marshal.Copy(hash, 0, pvFindPara.pbData, hash.Length);
		IntPtr intPtr = SspiProvider.CertFindDataBlobCertificateInStore(Handle, 65537, 0, dwFindType, ref pvFindPara, IntPtr.Zero);
		Marshal.FreeHGlobal(pvFindPara.pbData);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new Certificate(intPtr);
	}

	public Certificate FindCertificateByKeyIdentifier(byte[] keyID)
	{
		if (keyID == null)
		{
			throw new ArgumentNullException();
		}
		if (keyID.Length == 0)
		{
			throw new ArgumentException();
		}
		DataBlob pvFindPara = new DataBlob
		{
			cbData = keyID.Length,
			pbData = Marshal.AllocHGlobal(keyID.Length)
		};
		Marshal.Copy(keyID, 0, pvFindPara.pbData, keyID.Length);
		IntPtr intPtr = SspiProvider.CertFindDataBlobCertificateInStore(Handle, 65537, 0, 983040, ref pvFindPara, IntPtr.Zero);
		Marshal.FreeHGlobal(pvFindPara.pbData);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new Certificate(intPtr);
	}

	public Certificate FindCertificateBySubjectName(string name)
	{
		return FindCertificateBySubjectName(name, null);
	}

	public Certificate FindCertificateBySubjectName(string name, Certificate previous)
	{
		if (name == null)
		{
			throw new ArgumentNullException();
		}
		if (name.Length == 0)
		{
			throw new ArgumentException();
		}
		IntPtr intPtr = IntPtr.Zero;
		IntPtr pPrevCertContext = ((previous != null) ? SspiProvider.CertDuplicateCertificateContext(previous.Handle) : IntPtr.Zero);
		DataBlob pvFindPara = default(DataBlob);
		if (SspiProvider.CertStrToName(65537, name, 3, IntPtr.Zero, IntPtr.Zero, ref pvFindPara.cbData, IntPtr.Zero) == 0)
		{
			throw new CertificateException("Could not encode the specified string. [is the string a valid X500 string?]");
		}
		pvFindPara.pbData = Marshal.AllocHGlobal(pvFindPara.cbData);
		try
		{
			if (SspiProvider.CertStrToName(65537, name, 3, IntPtr.Zero, pvFindPara.pbData, ref pvFindPara.cbData, IntPtr.Zero) == 0)
			{
				throw new CertificateException("Could not encode the specified string.");
			}
			intPtr = SspiProvider.CertFindDataBlobCertificateInStore(Handle, 65537, 0, 131079, ref pvFindPara, pPrevCertContext);
		}
		finally
		{
			Marshal.FreeHGlobal(pvFindPara.pbData);
		}
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new Certificate(intPtr);
	}

	public Certificate FindCertificateBySubjectString(string subject)
	{
		return FindCertificateBySubjectString(subject, null);
	}

	public Certificate FindCertificateBySubjectString(string subject, Certificate previous)
	{
		if (subject == null)
		{
			throw new ArgumentNullException();
		}
		if (subject.Length == 0)
		{
			throw new ArgumentException();
		}
		IntPtr intPtr = SspiProvider.CertFindStringCertificateInStore(pPrevCertContext: (previous != null) ? SspiProvider.CertDuplicateCertificateContext(previous.Handle) : IntPtr.Zero, hCertStore: Handle, dwCertEncodingType: 65537, dwFindFlags: 0, dwFindType: 524295, pvFindPara: subject);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new Certificate(intPtr);
	}

	public Certificate[] EnumCertificates()
	{
		ArrayList arrayList = new ArrayList();
		for (Certificate certificate = FindCertificate(); certificate != null; certificate = FindCertificate(certificate))
		{
			arrayList.Add(certificate);
		}
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	public Certificate[] EnumCertificates(string[] keyUsage)
	{
		ArrayList arrayList = new ArrayList();
		for (Certificate certificate = FindCertificateByUsage(keyUsage); certificate != null; certificate = FindCertificateByUsage(keyUsage, certificate))
		{
			arrayList.Add(certificate);
		}
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	public void ToPfxFile(string filename, string password, bool exportPrivateKeys)
	{
		SaveToFile(GetPfxBuffer(password, exportPrivateKeys), filename);
	}

	public byte[] ToPfxBuffer(string password, bool exportPrivateKeys)
	{
		return GetPfxBuffer(password, exportPrivateKeys);
	}

	private byte[] GetPfxBuffer(string password, bool exportPrivateKeys)
	{
		if (password == null)
		{
			throw new ArgumentNullException();
		}
		DataBlob pPFX = default(DataBlob);
		try
		{
			pPFX.pbData = IntPtr.Zero;
			pPFX.cbData = 0;
			if (SspiProvider.PFXExportCertStoreEx(Handle, ref pPFX, password, IntPtr.Zero, exportPrivateKeys ? 4 : 0) == 0)
			{
				throw new CertificateException("Could not export the certificate store.");
			}
			pPFX.pbData = Marshal.AllocHGlobal(pPFX.cbData);
			if (SspiProvider.PFXExportCertStoreEx(Handle, ref pPFX, password, IntPtr.Zero, exportPrivateKeys ? 4 : 0) == 0)
			{
				throw new CertificateException("Could not export the certificate store.");
			}
			byte[] array = new byte[pPFX.cbData];
			Marshal.Copy(pPFX.pbData, array, 0, array.Length);
			return array;
		}
		finally
		{
			if (pPFX.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pPFX.pbData);
			}
		}
	}

	public void ToCerFile(string filename, CertificateStoreType type)
	{
		SaveToFile(GetCerBuffer(type), filename);
	}

	public byte[] ToCerBuffer(CertificateStoreType type)
	{
		return GetCerBuffer(type);
	}

	private byte[] GetCerBuffer(CertificateStoreType type)
	{
		DataBlob pvSaveToPara = default(DataBlob);
		try
		{
			pvSaveToPara.cbData = 0;
			pvSaveToPara.pbData = IntPtr.Zero;
			if (SspiProvider.CertSaveStore(Handle, 65537, (int)type, 2, ref pvSaveToPara, 0) == 0)
			{
				throw new CertificateException("Unable to get the certificate data.");
			}
			pvSaveToPara.pbData = Marshal.AllocHGlobal(pvSaveToPara.cbData);
			if (SspiProvider.CertSaveStore(Handle, 65537, (int)type, 2, ref pvSaveToPara, 0) == 0)
			{
				throw new CertificateException("Unable to get the certificate data.");
			}
			byte[] array = new byte[pvSaveToPara.cbData];
			Marshal.Copy(pvSaveToPara.pbData, array, 0, pvSaveToPara.cbData);
			return array;
		}
		finally
		{
			if (pvSaveToPara.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pvSaveToPara.pbData);
			}
		}
	}

	private void SaveToFile(byte[] buffer, string filename)
	{
		if (filename == null || buffer == null)
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

	public void AddCertificate(Certificate cert)
	{
		if (cert == null)
		{
			throw new ArgumentNullException();
		}
		if (SspiProvider.CertAddCertificateContextToStore(Handle, cert.Handle, 1, IntPtr.Zero) == 0 && Marshal.GetLastWin32Error() != -2146885627)
		{
			throw new CertificateException("An error occurs while adding the certificate to the store.");
		}
	}

	public void DeleteCertificate(Certificate cert)
	{
		if (cert == null)
		{
			throw new ArgumentNullException();
		}
		Certificate certificate = FindCertificateByHash(cert.GetCertHash(HashType.SHA1), HashType.SHA1);
		if (certificate == null)
		{
			throw new CertificateException("The certificate could not be found in the store.");
		}
		if (SspiProvider.CertDeleteCertificateFromStore(SspiProvider.CertDuplicateCertificateContext(certificate.Handle)) == 0)
		{
			throw new CertificateException("An error occurs while removing the certificate from the store.");
		}
	}

	~CertificateStore()
	{
		Dispose();
	}

	internal void Dispose()
	{
		if (m_Handle != IntPtr.Zero)
		{
			SspiProvider.CertCloseStore(m_Handle, 0);
			m_Handle = IntPtr.Zero;
		}
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
	}

	internal static CertificateStore GetCachedStore(string name)
	{
		CertificateStore certificateStore = null;
		lock (m_CachedStores)
		{
			certificateStore = m_CachedStores[name] as CertificateStore;
			if (certificateStore == null)
			{
				certificateStore = new CertificateStore(name);
				m_CachedStores.Add(name, certificateStore);
			}
		}
		return certificateStore;
	}
}
