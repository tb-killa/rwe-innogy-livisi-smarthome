using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Certificates;

public sealed class CertificateStore : IEnumerable<Certificate>, IEnumerable, IDisposable
{
	private IntPtr yzymv;

	private Dictionary<AsymmetricKeyAlgorithm, object> ywguv;

	private bool ladbs;

	public IntPtr Handle => yzymv;

	internal bool totse
	{
		get
		{
			if (yzymv != IntPtr.Zero && 0 == 0)
			{
				return false;
			}
			return true;
		}
	}

	private static string miwfh(CertificateStoreName p0)
	{
		return p0 switch
		{
			CertificateStoreName.AddressBook => "AddressBook", 
			CertificateStoreName.AuthRoot => "AuthRoot", 
			CertificateStoreName.CertificateAuthority => "CA", 
			CertificateStoreName.Disallowed => "Disallowed", 
			CertificateStoreName.My => "My", 
			CertificateStoreName.Root => "Root", 
			CertificateStoreName.TrustedPeople => "TrustedPeople", 
			CertificateStoreName.TrustedPublisher => "TrustedPublisher", 
			_ => throw new ArgumentException("Invalid store name.", "storeName"), 
		};
	}

	private static string yovcw(string p0)
	{
		return p0;
	}

	public CertificateStore(string storeName, CertificateStoreLocation location)
	{
		if (storeName == null || 1 == 0)
		{
			throw new ArgumentNullException("storeName");
		}
		if (storeName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Empty store name.", "storeName");
		}
		if (location == CertificateStoreLocation.None || 1 == 0)
		{
			throw new ArgumentException("Invalid store location.", "location");
		}
		storeName = yovcw(storeName);
		if (pothu.aicde && 0 == 0)
		{
			jeoei(storeName, location);
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	public CertificateStore(CertificateStoreName storeName, CertificateStoreLocation location)
		: this(miwfh(storeName), location)
	{
	}

	public CertificateStore(string storeName)
		: this(storeName, CertificateStoreLocation.CurrentUser)
	{
	}

	public CertificateStore(CertificateStoreName storeName)
		: this(miwfh(storeName), CertificateStoreLocation.CurrentUser)
	{
	}

	internal CertificateStore(IEnumerable<Certificate> certificates)
		: this((IEnumerable)certificates)
	{
	}

	public CertificateStore(ICollection certificates)
		: this((IEnumerable)certificates)
	{
	}

	private CertificateStore(IEnumerable certificates)
		: this()
	{
		if (certificates == null || 1 == 0)
		{
			throw new ArgumentNullException("certificates");
		}
		bool flag = false;
		try
		{
			nbjwv(certificates);
			flag = true;
		}
		finally
		{
			if (!flag || 1 == 0)
			{
				Dispose();
			}
		}
	}

	internal CertificateStore(IntPtr store, bool memoryBased)
	{
		yzymv = store;
		if (memoryBased && 0 == 0)
		{
			ywguv = new Dictionary<AsymmetricKeyAlgorithm, object>();
		}
	}

	internal CertificateStore()
	{
		if (pothu.aicde && 0 == 0)
		{
			qjzrm();
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	public static bool Exists(string storeName, CertificateStoreLocation location)
	{
		if (storeName == null || 1 == 0)
		{
			throw new ArgumentNullException("storeName");
		}
		if (storeName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Empty store name.", "storeName");
		}
		if (pothu.aicde && 0 == 0)
		{
			return dqwgw(storeName, location);
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	public static bool Exists(CertificateStoreName storeName, CertificateStoreLocation location)
	{
		return Exists(miwfh(storeName), location);
	}

	public static bool Exists(string storeName)
	{
		return Exists(storeName, CertificateStoreLocation.CurrentUser);
	}

	public static bool Exists(CertificateStoreName storeName)
	{
		return Exists(miwfh(storeName), CertificateStoreLocation.CurrentUser);
	}

	[Obsolete("This method has been deprecated. Please use Add method instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public void AddCertificate(Certificate cert)
	{
		httnf(cert, p1: false);
	}

	internal void nbjwv(IEnumerable p0)
	{
		IEnumerator enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				object current = enumerator.Current;
				if (current == null || 1 == 0)
				{
					throw new CertificateException("The collection contains null reference.");
				}
				if (!(current is Certificate certificate) || 1 == 0)
				{
					throw new CertificateException("The collection contains objects that are not certificates.");
				}
				Add(certificate);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}

	public void Add(Certificate certificate)
	{
		httnf(certificate, p1: true);
	}

	private void httnf(Certificate p0, bool p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("cert");
		}
		nyxca();
		if (pothu.aicde && 0 == 0)
		{
			mgrmq(p0, p1);
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	public void Remove(Certificate cert)
	{
		if (cert == null || 1 == 0)
		{
			throw new ArgumentNullException("cert");
		}
		nyxca();
		if (pothu.aicde && 0 == 0)
		{
			rtcmm(cert);
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	private static bool ftgld(Certificate p0, CertificateFindOptions p1)
	{
		if ((p1 & CertificateFindOptions.IsTimeValid) != 0 && (!p0.IsTimeValid() || 1 == 0))
		{
			return false;
		}
		if ((p1 & CertificateFindOptions.HasPrivateKey) != 0 && (!p0.HasPrivateKey() || 1 == 0))
		{
			return false;
		}
		string[] enhancedUsage = p0.GetEnhancedUsage();
		if (enhancedUsage == null || 1 == 0)
		{
			return true;
		}
		p1 &= CertificateFindOptions.ClientAuthentication | CertificateFindOptions.ServerAuthentication;
		CertificateFindOptions certificateFindOptions = CertificateFindOptions.None;
		int num = 0;
		if (num != 0)
		{
			goto IL_005f;
		}
		goto IL_00d6;
		IL_00d6:
		if (num >= enhancedUsage.Length)
		{
			if ((p1 & certificateFindOptions) == p1)
			{
				return true;
			}
			return false;
		}
		goto IL_005f;
		IL_005f:
		string text;
		if ((text = enhancedUsage[num]) != null && 0 == 0)
		{
			if (!(text == "1.3.6.1.5.5.7.3.1") || 1 == 0)
			{
				if (!(text == "1.3.6.1.5.5.7.3.2") || 1 == 0)
				{
					if (text == "2.5.29.37.0")
					{
						certificateFindOptions |= CertificateFindOptions.ServerAuthentication;
						certificateFindOptions |= CertificateFindOptions.ClientAuthentication;
					}
				}
				else
				{
					certificateFindOptions |= CertificateFindOptions.ClientAuthentication;
				}
			}
			else
			{
				certificateFindOptions |= CertificateFindOptions.ServerAuthentication;
			}
		}
		if ((p1 & certificateFindOptions) == p1)
		{
			return true;
		}
		num++;
		goto IL_00d6;
	}

	private void pylmu(ArrayList p0, string p1, CertificateFindOptions p2)
	{
		nyxca();
		if (pothu.aicde && 0 == 0)
		{
			mttpv(p0, p1, p2);
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	private void xybug(ArrayList p0, Certificate p1, CertificateFindOptions p2)
	{
		if (pothu.aicde && 0 == 0)
		{
			bkqsk(p0, p1, p2);
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	private void igids(ArrayList p0, DistinguishedName p1, byte[] p2, CertificateFindOptions p3)
	{
		if (pothu.aicde && 0 == 0)
		{
			qqdgl(p0, p1, p2, p3);
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	private void dzskr(CertificateFindType p0, byte[] p1, ArrayList p2, CertificateFindOptions p3)
	{
		if (pothu.aicde && 0 == 0)
		{
			svdyk(p0, p1, p2, p3);
			return;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	public Certificate[] FindCertificates(CertificateFindType findType, byte[] data, CertificateFindOptions options)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		ArrayList arrayList = new ArrayList();
		dzskr(findType, data, arrayList, options);
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	public Certificate[] FindCertificates(DistinguishedName issuer, CertificateFindOptions options)
	{
		if (issuer == null || 1 == 0)
		{
			throw new ArgumentNullException("issuer");
		}
		return nomif(new DistinguishedName[1] { issuer }, null, options);
	}

	public Certificate[] FindCertificates(DistinguishedName issuer, byte[] serialNumber, CertificateFindOptions options)
	{
		if (issuer == null || 1 == 0)
		{
			throw new ArgumentNullException("issuer");
		}
		if (serialNumber == null || 1 == 0)
		{
			throw new ArgumentNullException("serialNumber");
		}
		return nomif(new DistinguishedName[1] { issuer }, serialNumber, options);
	}

	public Certificate[] FindCertificates(DistinguishedName[] issuers, CertificateFindOptions options)
	{
		if (issuers == null || 1 == 0)
		{
			throw new ArgumentNullException("issuers");
		}
		return nomif(issuers, null, options);
	}

	public Certificate[] FindCertificates(DistinguishedName[] issuers, byte[] serialNumber, CertificateFindOptions options)
	{
		if (issuers == null || 1 == 0)
		{
			throw new ArgumentNullException("issuers");
		}
		if (serialNumber == null || 1 == 0)
		{
			throw new ArgumentNullException("serialNumber");
		}
		return nomif(issuers, serialNumber, options);
	}

	private Certificate[] nomif(DistinguishedName[] p0, byte[] p1, CertificateFindOptions p2)
	{
		if (p0 == null || false || p0.Length == 0 || 1 == 0)
		{
			return new Certificate[0];
		}
		if ((p2 & CertificateFindOptions.IncludeSubordinateAuthorities) != 0)
		{
			p0 = eigvd(p0);
		}
		ArrayList arrayList = new ArrayList();
		int num = 0;
		if (num != 0)
		{
			goto IL_0040;
		}
		goto IL_005d;
		IL_0040:
		if (p0[num] != null && 0 == 0)
		{
			igids(arrayList, p0[num], p1, p2);
		}
		num++;
		goto IL_005d;
		IL_005d:
		if (num < p0.Length)
		{
			goto IL_0040;
		}
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	public Certificate[] FindCertificates(Certificate certificate, CertificateFindOptions options)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		ArrayList arrayList = new ArrayList();
		xybug(arrayList, certificate, options);
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	public Certificate[] FindCertificates(CertificateFindOptions options)
	{
		ArrayList arrayList = new ArrayList();
		pylmu(arrayList, null, options);
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	public Certificate[] FindCertificatesForMailAddress(string address)
	{
		return FindCertificatesForMailAddress(address, CertificateFindOptions.None);
	}

	public Certificate[] FindCertificatesForMailAddress(string address, CertificateFindOptions options)
	{
		if (address == null || 1 == 0)
		{
			throw new ArgumentNullException("address");
		}
		ArrayList arrayList = new ArrayList();
		pylmu(arrayList, address, options);
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	private static DistinguishedName[] eigvd(DistinguishedName[] p0)
	{
		CertificateStore certificateStore = new CertificateStore(CertificateStoreName.CertificateAuthority);
		ArrayList arrayList = new ArrayList(p0);
		ArrayList arrayList2 = new ArrayList(p0);
		ArrayList arrayList3 = new ArrayList();
		int num = 0;
		if (num != 0)
		{
			goto IL_0028;
		}
		goto IL_00c0;
		IL_0028:
		arrayList3.Clear();
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0035;
		}
		goto IL_0052;
		IL_0035:
		certificateStore.igids(arrayList3, (DistinguishedName)arrayList2[num2], null, CertificateFindOptions.IsTimeValid);
		num2++;
		goto IL_0052;
		IL_0052:
		if (num2 < arrayList2.Count)
		{
			goto IL_0035;
		}
		arrayList2.Clear();
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0069;
		}
		goto IL_00a0;
		IL_00a0:
		if (num3 < arrayList3.Count)
		{
			goto IL_0069;
		}
		if (arrayList2.Count != 0 && 0 == 0)
		{
			num++;
			goto IL_00c0;
		}
		goto IL_00d4;
		IL_0069:
		DistinguishedName subject = ((Certificate)arrayList3[num3]).GetSubject();
		if (arrayList.IndexOf(subject) < 0)
		{
			arrayList2.Add(subject);
			arrayList.Add(subject);
		}
		num3++;
		goto IL_00a0;
		IL_00d4:
		return (DistinguishedName[])arrayList.ToArray(typeof(DistinguishedName));
		IL_00c0:
		if (num >= 16)
		{
			goto IL_00d4;
		}
		goto IL_0028;
	}

	internal byte[] liifj(string p0)
	{
		if (Environment.OSVersion.Version.Major < 5)
		{
			throw new CryptographicException("PFX/P12 saving is only supported on Windows CE 5.0 and higher.");
		}
		if (yzymv == IntPtr.Zero && 0 == 0)
		{
			throw new CryptographicException("Unable to save PFX/P12.");
		}
		samhn samhn = new samhn(2 * IntPtr.Size);
		try
		{
			int dwFlags = 7;
			samhn.fpzdi(0, 0);
			samhn.qurik(IntPtr.Size, IntPtr.Zero);
			if (pothu.PFXExportCertStoreEx(yzymv, samhn.inyna(), p0, IntPtr.Zero, (uint)dwFlags) == 0 || 1 == 0)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				switch (lastWin32Error)
				{
				case -2146885628:
					throw new CertificateException("The certificate does not have a permanently associated private key or the key is not exportable.");
				case -2146893811:
					throw new CertificateException("The certificate does not have a permanently associated private key.");
				case -2146893821:
				case -2146893813:
					throw new CertificateException("The private key is not exportable.");
				default:
					throw new CertificateException(brgjd.edcru("Unable to export PFX (0x{0:X8}).", lastWin32Error));
				}
			}
			int num = samhn.sqmvb(0);
			if (num < 0)
			{
				throw new CertificateException("Invalid PFX blob length.");
			}
			if (num > 262144)
			{
				throw new CertificateException("PFX blob is too long.");
			}
			samhn samhn2 = new samhn(num + 2 * IntPtr.Size);
			try
			{
				samhn2.fpzdi(0, num);
				samhn2.qurik(IntPtr.Size, new IntPtr(samhn2.inyna().ToInt64() + 2 * IntPtr.Size));
				if (pothu.PFXExportCertStoreEx(Handle, samhn2.inyna(), p0, IntPtr.Zero, (uint)dwFlags) == 0 || 1 == 0)
				{
					throw new CertificateException(brgjd.edcru("Unable to export PFX (0x{0:X8}).", Marshal.GetLastWin32Error()));
				}
				num = samhn.sqmvb(0);
				byte[] array = new byte[num];
				samhn2.wdcfj(2 * IntPtr.Size, num, array, 0);
				return array;
			}
			finally
			{
				if (samhn2 != null && 0 == 0)
				{
					((IDisposable)samhn2).Dispose();
				}
			}
		}
		finally
		{
			if (samhn != null && 0 == 0)
			{
				((IDisposable)samhn).Dispose();
			}
		}
	}

	internal static CertificateStore adphl(byte[] p0, string p1, KeySetOptions p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		if (Environment.OSVersion.Version.Major < 5)
		{
			throw new CryptographicException("PFX/P12 loading is only supported on Windows CE 5.0 and higher.");
		}
		if ((p2 & KeySetOptions.AlwaysCng) != 0 && 0 == 0 && (!CryptoHelper.wyrkl || 1 == 0))
		{
			throw new CryptographicException("CNG is only available on Windows Embedded Compact 2013 or higher.");
		}
		if ((p2 & KeySetOptions.PreferCng) != 0 && 0 == 0 && (!dahxy.uttbp || 1 == 0))
		{
			p2 &= ~KeySetOptions.PreferCng;
		}
		p2 &= ~KeySetOptions.PersistKeySet;
		IntPtr intPtr = IntPtr.Zero;
		samhn samhn = null;
		try
		{
			samhn = new samhn(p0.Length + 2 * IntPtr.Size);
			samhn.fpzdi(0, p0.Length);
			samhn.qurik(IntPtr.Size, new IntPtr(samhn.inyna().ToInt64() + 2 * IntPtr.Size));
			samhn.zqmse(p0, 0, p0.Length, 2 * IntPtr.Size);
			if (pothu.PFXIsPFXBlob(samhn.inyna()) == 0 || 1 == 0)
			{
				throw new CertificateException("Not a valid PFX data.");
			}
			if (pothu.PFXVerifyPassword(samhn.inyna(), p1, 0) == 0 || 1 == 0)
			{
				if ((p1.Length == 0 || 1 == 0) && pothu.PFXVerifyPassword(samhn.inyna(), null, 0) != 0 && 0 == 0)
				{
					p1 = null;
				}
				if (p1 != null && 0 == 0)
				{
					throw new CertificateException("PFX password is not valid.");
				}
			}
			intPtr = pothu.PFXImportCertStore(samhn.inyna(), p1, (uint)p2);
			if (intPtr == IntPtr.Zero && 0 == 0)
			{
				throw new CertificateException(brgjd.edcru("Unable to load PFX (0x{0:X8}).", Marshal.GetLastWin32Error()));
			}
		}
		finally
		{
			if (samhn != null && 0 == 0)
			{
				samhn.Dispose();
			}
		}
		return new CertificateStore(intPtr, memoryBased: true);
	}

	private void nyxca()
	{
		if (ladbs && 0 == 0)
		{
			throw new ObjectDisposedException("CertificateStore");
		}
	}

	public void Dispose()
	{
		wukyz(p0: true);
		GC.SuppressFinalize(this);
	}

	private void wukyz(bool p0)
	{
		ladbs = true;
		try
		{
			if (yzymv != IntPtr.Zero && 0 == 0)
			{
				pothu.CertCloseStore(yzymv, 0);
			}
		}
		finally
		{
			yzymv = IntPtr.Zero;
		}
	}

	~CertificateStore()
	{
		wukyz(p0: false);
	}

	private void jeoei(string p0, CertificateStoreLocation p1)
	{
		yzymv = pothu.CertOpenStore(new IntPtr(10), 65537, IntPtr.Zero, (int)p1, p0);
		if (yzymv == IntPtr.Zero && 0 == 0)
		{
			throw new CertificateException(brgjd.edcru("Unable to open the certificate store (0x{0:X8}).", Marshal.GetLastWin32Error()));
		}
	}

	private void qjzrm()
	{
		yzymv = pothu.CertOpenStore(new IntPtr(2), 65537, IntPtr.Zero, 0, null);
		if (yzymv == IntPtr.Zero && 0 == 0)
		{
			throw new CertificateException(brgjd.edcru("Unable to create memory store (0x{0:X8}).", Marshal.GetLastWin32Error()));
		}
		ywguv = new Dictionary<AsymmetricKeyAlgorithm, object>();
	}

	private static bool dqwgw(string p0, CertificateStoreLocation p1)
	{
		IntPtr intPtr = pothu.CertOpenStore(new IntPtr(10), 65537, IntPtr.Zero, (int)((CertificateStoreLocation)16384 | p1), p0);
		if (intPtr == IntPtr.Zero && 0 == 0)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 2 || lastWin32Error == 3)
			{
				return false;
			}
			throw new CertificateException(brgjd.edcru("Unable to open the certificate store (0x{0:X8}).", lastWin32Error));
		}
		pothu.CertCloseStore(intPtr, 0);
		return true;
	}

	private void mgrmq(Certificate p0, bool p1)
	{
		int dwAddDisposition = ((!p1 || 1 == 0) ? 1 : 5);
		if (pothu.CertAddCertificateContextToStore(yzymv, p0.Handle, dwAddDisposition, IntPtr.Zero) == 0 || 1 == 0)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			throw new CertificateException(brgjd.edcru("Unable to add certificate to store (0x{0:X8}).", lastWin32Error));
		}
		if (ywguv == null || 1 == 0)
		{
			p0.rwaxf(p0: false);
			return;
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = p0.woudq();
		if (asymmetricKeyAlgorithm != null && 0 == 0 && (!ywguv.ContainsKey(asymmetricKeyAlgorithm) || 1 == 0))
		{
			ywguv.Add(asymmetricKeyAlgorithm, null);
		}
	}

	private void rtcmm(Certificate p0)
	{
		Certificate[] array = FindCertificates(p0, CertificateFindOptions.None);
		if (array.Length == 0 || 1 == 0)
		{
			return;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0024;
		}
		goto IL_00ea;
		IL_0024:
		try
		{
			IntPtr intPtr = pothu.CertDuplicateCertificateContext(array[num].Handle);
			if (intPtr == IntPtr.Zero && 0 == 0)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new CertificateException(brgjd.edcru("Unable to duplicate certificate context (0x{0:X8}).", lastWin32Error));
			}
			if (pothu.CertDeleteCertificateFromStore(intPtr) == 0 || 1 == 0)
			{
				int lastWin32Error2 = Marshal.GetLastWin32Error();
				if (lastWin32Error2 == -2147024891)
				{
					throw new CertificateException(brgjd.edcru("Unable to remove certificate from store because the store is read-only (0x{0:X8}).", lastWin32Error2));
				}
				throw new CertificateException(brgjd.edcru("Unable to remove certificate from store (0x{0:X8}).", lastWin32Error2));
			}
		}
		finally
		{
			array[num].Dispose();
		}
		num++;
		goto IL_00ea;
		IL_00ea:
		if (num >= array.Length)
		{
			if (ywguv != null && 0 == 0)
			{
				AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = p0.woudq();
				if (asymmetricKeyAlgorithm != null && 0 == 0)
				{
					ywguv.Remove(asymmetricKeyAlgorithm);
				}
			}
			return;
		}
		goto IL_0024;
	}

	private void mttpv(ArrayList p0, string p1, CertificateFindOptions p2)
	{
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			while (true)
			{
				intPtr = pothu.CertEnumCertificatesInStore(yzymv, intPtr);
				if ((intPtr == IntPtr.Zero) ? true : false)
				{
					break;
				}
				Certificate certificate = new Certificate(intPtr);
				if (p1 == null || false || certificate.gjmyp(p1))
				{
					if (ftgld(certificate, p2) && 0 == 0)
					{
						p0.Add(certificate);
					}
					else
					{
						certificate.Dispose();
					}
				}
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero && 0 == 0)
			{
				pothu.CertFreeCertificateContext(intPtr);
			}
		}
	}

	private static IntPtr zhfea(Certificate p0)
	{
		IntPtr handle = p0.Handle;
		IntPtr intPtr = samhn.yjjps(handle, 3 * IntPtr.Size);
		if (intPtr == IntPtr.Zero && 0 == 0)
		{
			throw new CryptographicException("Unable to get certificate info.");
		}
		return intPtr;
	}

	private void bkqsk(ArrayList p0, Certificate p1, CertificateFindOptions p2)
	{
		IntPtr intPtr = IntPtr.Zero;
		IntPtr pvFindPara = zhfea(p1);
		try
		{
			while (true)
			{
				intPtr = pothu.CertFindCertificateInStore(yzymv, 65537, 0, 720896, pvFindPara, intPtr);
				if (!(intPtr == IntPtr.Zero) || 1 == 0)
				{
					Certificate certificate = new Certificate(intPtr);
					if (ftgld(certificate, p2) && 0 == 0)
					{
						p0.Add(certificate);
					}
					else
					{
						certificate.Dispose();
					}
					continue;
				}
				break;
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero && 0 == 0)
			{
				pothu.CertFreeCertificateContext(intPtr);
			}
		}
	}

	private void qqdgl(ArrayList p0, DistinguishedName p1, byte[] p2, CertificateFindOptions p3)
	{
		IntPtr intPtr = IntPtr.Zero;
		samhn samhn;
		if (p2 != null && 0 == 0)
		{
			byte[] array = p1.ToArray();
			byte[] array2 = new byte[p2.Length];
			p2.CopyTo(array2, 0);
			Array.Reverse(array2, 0, array2.Length);
			samhn = new samhn(IntPtr.Size * 5 + array.Length + p2.Length);
			samhn.fpzdi(0, 1);
			samhn.fpzdi(IntPtr.Size, array.Length);
			samhn.qurik(IntPtr.Size * 2, new IntPtr(samhn.inyna().ToInt64() + IntPtr.Size * 5));
			samhn.fpzdi(IntPtr.Size * 3, p2.Length);
			samhn.qurik(IntPtr.Size * 4, new IntPtr(samhn.inyna().ToInt64() + IntPtr.Size * 5 + array.Length));
			samhn.zqmse(array, 0, array.Length, IntPtr.Size * 5);
			samhn.zqmse(array2, 0, array2.Length, IntPtr.Size * 5 + array.Length);
		}
		else
		{
			samhn = p1.btuxq();
		}
		try
		{
			while (true)
			{
				intPtr = ((p2 != null) ? pothu.CertFindCertificateInStore(yzymv, 65537, 0, 1048576, samhn.inyna(), intPtr) : pothu.CertFindCertificateInStore(yzymv, 65537, 0, 131076, samhn.inyna(), intPtr));
				if (!(intPtr == IntPtr.Zero) || 1 == 0)
				{
					Certificate certificate = new Certificate(intPtr);
					if (ftgld(certificate, p3) && 0 == 0)
					{
						p0.Add(certificate);
					}
					else
					{
						certificate.Dispose();
					}
					continue;
				}
				break;
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero && 0 == 0)
			{
				pothu.CertFreeCertificateContext(intPtr);
			}
			if (samhn != null && 0 == 0)
			{
				samhn.Dispose();
			}
		}
	}

	private void svdyk(CertificateFindType p0, byte[] p1, ArrayList p2, CertificateFindOptions p3)
	{
		IntPtr intPtr = IntPtr.Zero;
		samhn samhn = new samhn(p1.Length + 2 * IntPtr.Size);
		samhn.fpzdi(0, p1.Length);
		samhn.qurik(IntPtr.Size, new IntPtr(samhn.inyna().ToInt64() + 2 * IntPtr.Size));
		samhn.zqmse(p1, 0, p1.Length, 2 * IntPtr.Size);
		try
		{
			while (true)
			{
				intPtr = pothu.CertFindCertificateInStore(yzymv, 65537, 0, 983040, samhn.inyna(), intPtr);
				if (!(intPtr == IntPtr.Zero) || 1 == 0)
				{
					Certificate certificate = new Certificate(intPtr);
					if (ftgld(certificate, p3) && 0 == 0)
					{
						p2.Add(certificate);
					}
					else
					{
						certificate.Dispose();
					}
					continue;
				}
				break;
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero && 0 == 0)
			{
				pothu.CertFreeCertificateContext(intPtr);
			}
			if (samhn != null && 0 == 0)
			{
				samhn.Dispose();
			}
		}
	}

	private IEnumerator<Certificate> byfby()
	{
		Certificate[] array = FindCertificates(CertificateFindOptions.None);
		return ((IEnumerable<Certificate>)array).GetEnumerator();
	}

	IEnumerator<Certificate> IEnumerable<Certificate>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in byfby
		return this.byfby();
	}

	private IEnumerator dmpbh()
	{
		return ((IEnumerable<Certificate>)this).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in dmpbh
		return this.dmpbh();
	}
}
