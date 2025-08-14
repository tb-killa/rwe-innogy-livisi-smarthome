using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Security.Certificates;

public sealed class CertificateChain : IEnumerable<Certificate>, IEnumerable, IDisposable
{
	private List<Certificate> qaunb = new List<Certificate>();

	private bool xoxyn;

	private static long[] dujke = new long[18]
	{
		1L, 1L, 2L, 4L, 16L, 32L, 32L, 16L, 64L, 3840L,
		512L, 128L, 1024L, 8L, 65536L, 16L, 131072L, 2L
	};

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("CertificateChain.DefaultEngine property has been deprecated. Please use CertificateChainEngine.Auto instead.", false)]
	[wptwl(false)]
	public static CertificateChainEngine DefaultEngine => fqjtz();

	public int Count => qaunb.Count;

	public Certificate this[int i]
	{
		get
		{
			return qaunb[i];
		}
		set
		{
			qaunb[i] = value;
		}
	}

	public Certificate RootCertificate
	{
		get
		{
			if (qaunb.Count == 0 || 1 == 0)
			{
				return null;
			}
			Certificate certificate = qaunb[qaunb.Count - 1];
			if (certificate.GetSubjectName() != certificate.GetIssuerName() && 0 == 0)
			{
				return null;
			}
			return certificate;
		}
	}

	public Certificate LeafCertificate
	{
		get
		{
			if (qaunb.Count == 0 || 1 == 0)
			{
				return null;
			}
			return qaunb[0];
		}
	}

	internal static CertificateChainEngine fqjtz()
	{
		return CertificateChainEngine.CurrentUser;
	}

	private IEnumerator ipxbt()
	{
		return qaunb.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ipxbt
		return this.ipxbt();
	}

	public IEnumerator<Certificate> GetEnumerator()
	{
		return qaunb.GetEnumerator();
	}

	public int Add(Certificate certificate)
	{
		int count = qaunb.Count;
		qaunb.Add(certificate);
		return count;
	}

	public CertificateChain()
	{
	}

	public CertificateChain(params Certificate[] certificates)
	{
		if (certificates == null || 1 == 0)
		{
			throw new ArgumentNullException("certificates");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0029;
		}
		goto IL_0040;
		IL_0040:
		if (num >= certificates.Length)
		{
			return;
		}
		goto IL_0029;
		IL_0029:
		Add(certificates[num]);
		num++;
		goto IL_0040;
	}

	public static CertificateChain LoadPfx(string path, string password)
	{
		return LoadPfx(path, password, KeySetOptions.Exportable | KeySetOptions.UserKeySet);
	}

	public static CertificateChain LoadPfx(byte[] data, string password)
	{
		return LoadPfx(data, password, KeySetOptions.Exportable | KeySetOptions.UserKeySet);
	}

	public static CertificateChain LoadPfx(string path, string password, KeySetOptions options)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		Stream stream = vtdxm.prsfm(path);
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		stream.Close();
		return LoadPfx(array, password, options);
	}

	public static CertificateChain LoadPfx(byte[] data, string password, KeySetOptions options)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		CertificateChainEngine certificateChainEngine;
		if ((options & (KeySetOptions.MachineKeySet | KeySetOptions.UserKeySet)) != KeySetOptions.MachineKeySet)
		{
			certificateChainEngine = CertificateChainEngine.CurrentUser;
			if (certificateChainEngine == CertificateChainEngine.CurrentUser)
			{
				goto IL_0041;
			}
		}
		certificateChainEngine = CertificateChainEngine.LocalMachine;
		goto IL_0041;
		IL_0041:
		Certificate certificate = null;
		if (pothu.aicde)
		{
			CertificateStore certificateStore = CertificateStore.adphl(data, password, options);
			Certificate[] array = certificateStore.FindCertificates(CertificateFindOptions.HasPrivateKey);
			if (array != null && 0 == 0 && array.Length > 0)
			{
				certificate = array[0];
				certificate.nhtyb(certificateStore);
				if ((options & KeySetOptions.PersistKeySet) == 0 || 1 == 0)
				{
					certificate.rwaxf(p0: true);
				}
			}
			if (certificate == null || 1 == 0)
			{
				certificateStore.Dispose();
				throw new CertificateException("No certificate with a private key found in PFX.");
			}
			return BuildFrom(certificate, certificateStore, certificateChainEngine);
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	public static CertificateChain LoadP7b(string path)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		Stream stream = vtdxm.prsfm(path);
		try
		{
			return LoadP7b(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public static CertificateChain LoadP7b(Stream stream)
	{
		if (stream == null || 1 == 0)
		{
			throw new ArgumentNullException("stream");
		}
		byte[] array = CryptoHelper.zxtjl(stream, 1048576);
		if (array[0] != 48)
		{
			string p;
			try
			{
				array = kjhmn.qzqgg(array, array.Length, out p);
			}
			catch (FormatException ex)
			{
				throw new CertificateException(ex.Message, ex);
			}
			if (p != "PKCS7" && 0 == 0)
			{
				throw new CertificateException("Unexpected file type '" + p + "'.");
			}
		}
		SignedData signedData = new SignedData();
		signedData.Decode(array);
		CertificateChain certificateChain = new CertificateChain();
		int num = 0;
		if (num != 0)
		{
			goto IL_008e;
		}
		goto IL_00a9;
		IL_00a9:
		if (num < signedData.Certificates.Count)
		{
			goto IL_008e;
		}
		return certificateChain;
		IL_008e:
		certificateChain.Add(signedData.Certificates[num]);
		num++;
		goto IL_00a9;
	}

	internal bool tlhda(byte[] p0, byte[] p1, SignatureParameters p2)
	{
		if (Count == 0 || 1 == 0)
		{
			throw new CertificateException("There are no certificates in the chain.");
		}
		Certificate certificate = this[0];
		if (certificate.KeyAlgorithm != KeyAlgorithm.DSA)
		{
			return certificate.bvdsv(p0, p1, p2);
		}
		if (p2.ppgdd() == SignatureFormat.Pkcs)
		{
			p1 = lmjia.bhrbh(p1, 40);
		}
		return VerifyHash(p0, SignatureHashAlgorithm.SHA1, p1);
	}

	internal CertificateChain qdige()
	{
		int count = Count;
		if (count == 0 || 1 == 0)
		{
			return new CertificateChain();
		}
		Certificate[] array = new Certificate[count];
		int num = 0;
		if (num != 0)
		{
			goto IL_0025;
		}
		goto IL_003d;
		IL_003d:
		if (num < count)
		{
			goto IL_0025;
		}
		return new CertificateChain(array);
		IL_0025:
		array[num] = qaunb[num].nehgg();
		num++;
		goto IL_003d;
	}

	public void Dispose()
	{
		if (xoxyn)
		{
			return;
		}
		using (List<Certificate>.Enumerator enumerator = qaunb.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Certificate current = enumerator.Current;
				current.Dispose();
			}
		}
		xoxyn = true;
	}

	public bool VerifyHash(byte[] hash, SignatureHashAlgorithm alg, byte[] signature)
	{
		if (hash == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		if (signature == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		if (Count == 0 || 1 == 0)
		{
			throw new CertificateException("There are no certificates in the chain.");
		}
		Certificate certificate = this[0];
		if (certificate.KeyAlgorithm != KeyAlgorithm.DSA)
		{
			return certificate.VerifyHash(hash, alg, signature);
		}
		if (alg != SignatureHashAlgorithm.SHA1)
		{
			throw new CertificateException("Only SHA-1 algorithm is supported by DSA certificates.");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0079;
		}
		goto IL_00cd;
		IL_0079:
		Certificate certificate2 = this[num];
		byte[] keyAlgorithmParameters = certificate2.GetKeyAlgorithmParameters();
		if (keyAlgorithmParameters.Length > 0)
		{
			DSAParameters key = certificate.pujje(keyAlgorithmParameters);
			AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
			try
			{
				asymmetricKeyAlgorithm.ImportKey(key);
				return asymmetricKeyAlgorithm.VerifyHash(hash, alg, signature);
			}
			finally
			{
				if (asymmetricKeyAlgorithm != null && 0 == 0)
				{
					((IDisposable)asymmetricKeyAlgorithm).Dispose();
				}
			}
		}
		num++;
		goto IL_00cd;
		IL_00cd:
		if (num < Count)
		{
			goto IL_0079;
		}
		throw new CertificateException("Certificates has inherited DSS parameters, but the parent certificate was not found.");
	}

	private static IntPtr jyqqu(CertificateChainEngine p0, Certificate p1, CertificateStore p2, bool p3)
	{
		if (p0 != CertificateChainEngine.LocalMachine)
		{
			p0 = CertificateChainEngine.CurrentUser;
		}
		IntPtr ppChainContext = IntPtr.Zero;
		pothu.nhkqk pChainPara = new pothu.nhkqk
		{
			ggflp = new IntPtr(Marshal.SizeOf(typeof(pothu.nhkqk))),
			rjonk = 
			{
				caaeb = 0,
				daeeh = 
				{
					pwqgd = 0,
					vkfqu = IntPtr.Zero
				}
			}
		};
		uint dwFlags = ((p3 ? true : false) ? 1u : 0u);
		if (pothu.CertGetCertificateChain(new IntPtr((int)p0), p1.Handle, IntPtr.Zero, p2?.Handle ?? IntPtr.Zero, ref pChainPara, dwFlags, IntPtr.Zero, out ppChainContext) == 0 || 1 == 0)
		{
			throw new CertificateException(brgjd.edcru("Unable to create certificate chain ({0}).", Marshal.GetLastWin32Error()));
		}
		int num = Marshal.ReadInt32(ppChainContext, 12);
		if (num < 1)
		{
			throw new CertificateException("The created chain is empty.");
		}
		return ppChainContext;
	}

	public static CertificateChain BuildFrom(Certificate cert)
	{
		return BuildFrom(cert, null, CertificateChainEngine.Auto);
	}

	public static CertificateChain BuildFrom(Certificate cert, CertificateChainEngine engine)
	{
		return BuildFrom(cert, null, engine);
	}

	public static CertificateChain BuildFrom(Certificate cert, CertificateStore store)
	{
		return BuildFrom(cert, store, CertificateChainEngine.Auto);
	}

	public static CertificateChain BuildFrom(Certificate cert, CertificateStore store, CertificateChainEngine engine)
	{
		if (cert == null || 1 == 0)
		{
			throw new ArgumentNullException("cert");
		}
		if (engine == CertificateChainEngine.Auto)
		{
			CertificateEngine certificateEngine = CertificateEngine.GetCurrentEngine();
			if (certificateEngine == CertificateEngine.Default)
			{
				certificateEngine = null;
			}
			if (certificateEngine != null && 0 == 0)
			{
				return certificateEngine.wwils(cert, store);
			}
			engine = fqjtz();
		}
		if (cert.Handle == IntPtr.Zero && 0 == 0)
		{
			throw new NotSupportedException("Automated chain building is not supported for this certificate.");
		}
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = jyqqu(engine, cert, store, p3: true);
			return omsaq(cert, intPtr);
		}
		finally
		{
			if (intPtr != IntPtr.Zero && 0 == 0)
			{
				pothu.CertFreeCertificateChain(intPtr);
			}
		}
	}

	private static CertificateChain omsaq(Certificate p0, IntPtr p1)
	{
		IntPtr p2 = samhn.yjjps(p1, 16);
		IntPtr intPtr = samhn.yjjps(p2, 0);
		int num = Marshal.ReadInt32(intPtr, 12);
		IntPtr p3 = samhn.yjjps(intPtr, 16);
		CertificateChain certificateChain = new CertificateChain();
		certificateChain.Add(p0);
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_003e;
		}
		goto IL_0092;
		IL_003e:
		IntPtr p4 = samhn.yjjps(p3, IntPtr.Size * num2);
		IntPtr handle = samhn.yjjps(p4, IntPtr.Size);
		Certificate certificate = new Certificate(handle);
		certificateChain.Add(certificate);
		if (!(certificate.GetSubjectName() == certificate.GetIssuerName()) || 1 == 0)
		{
			num2++;
			goto IL_0092;
		}
		goto IL_00a1;
		IL_00a1:
		return certificateChain;
		IL_0092:
		if (num2 >= num)
		{
			goto IL_00a1;
		}
		goto IL_003e;
	}

	public ValidationResult Validate()
	{
		return Validate(null, ValidationOptions.None, CertificateChainEngine.Auto);
	}

	public ValidationResult Validate(ValidationOptions options)
	{
		return Validate(null, options, CertificateChainEngine.Auto);
	}

	public ValidationResult Validate(string serverName, ValidationOptions options)
	{
		return Validate(serverName, options, CertificateChainEngine.Auto);
	}

	public ValidationResult Validate(string serverName, ValidationOptions options, CertificateChainEngine engine)
	{
		CertificateStore certificateStore = new CertificateStore();
		try
		{
			int num = 1;
			if (num == 0)
			{
				goto IL_000c;
			}
			goto IL_001d;
			IL_000c:
			certificateStore.Add(this[num]);
			num++;
			goto IL_001d;
			IL_001d:
			if (num < Count)
			{
				goto IL_000c;
			}
			return wjdra(engine, this[0], certificateStore, serverName, options);
		}
		finally
		{
			if (certificateStore != null && 0 == 0)
			{
				((IDisposable)certificateStore).Dispose();
			}
		}
	}

	private static bool ounry(string p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			return false;
		}
		if (p0.StartsWith("*.", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			if (p0.Length < 3)
			{
				return false;
			}
			string text = p0.Substring(1);
			if (p1.EndsWith(text, StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				return true;
			}
			p0 = text.Substring(1);
		}
		if (string.Compare(p0, p1, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			return true;
		}
		return false;
	}

	internal static ValidationResult wjdra(CertificateChainEngine p0, Certificate p1, CertificateStore p2, string p3, ValidationOptions p4)
	{
		ValidationResult validationResult;
		if (p0 == CertificateChainEngine.Auto)
		{
			CertificateEngine certificateEngine = CertificateEngine.GetCurrentEngine();
			if (certificateEngine == CertificateEngine.Default)
			{
				certificateEngine = null;
			}
			if (certificateEngine != null && 0 == 0)
			{
				CertificateChain chain = certificateEngine.wwils(p1, p2);
				CertificateValidationParameters parameters = new CertificateValidationParameters(p4, p3);
				validationResult = certificateEngine.Validate(chain, parameters);
				vujlx(p1, p3, p4, validationResult);
				return validationResult;
			}
			p0 = fqjtz();
		}
		validationResult = btnex(p0, p1, p2, p4, p3, p5: false);
		vujlx(p1, p3, p4, validationResult);
		return validationResult;
	}

	private static void vujlx(Certificate p0, string p1, ValidationOptions p2, ValidationResult p3)
	{
		if ((p2 & ValidationOptions.IgnoreCnNotMatch) == 0 && (!tpsoj(p0, p1) || 1 == 0))
		{
			p3.mpqyw(p0: false, 137438953472L);
		}
	}

	private static bool tpsoj(Certificate p0, string p1)
	{
		if (p1 == null || 1 == 0)
		{
			return true;
		}
		if (ounry(p0.GetCommonName(), p1) && 0 == 0)
		{
			return true;
		}
		string[] commonNames = p0.GetCommonNames();
		int num = 0;
		if (num != 0)
		{
			goto IL_0034;
		}
		goto IL_0051;
		IL_0051:
		if (num < commonNames.Length)
		{
			goto IL_0034;
		}
		return false;
		IL_0034:
		string p2 = commonNames[num];
		if (ounry(p2, p1) && 0 == 0)
		{
			return true;
		}
		num++;
		goto IL_0051;
	}

	internal static ValidationResult btnex(CertificateChainEngine p0, Certificate p1, CertificateStore p2, ValidationOptions p3, string p4, bool p5)
	{
		if (p1.Handle == IntPtr.Zero && 0 == 0)
		{
			throw new NotSupportedException("Validation is not supported on this platform.");
		}
		int num = 0;
		CertificateChain certificateChain = null;
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			bool flag = (p3 & ValidationOptions.SkipRevocationCheck) != 0;
			bool flag2 = (p3 & ValidationOptions.UseCacheOnly) != 0;
			bool p6 = (p5 ? true : false) || (flag ? true : false) || flag2;
			intPtr = jyqqu(p0, p1, p2, p6);
			long num2;
			int num3;
			if ((p5 ? true : false) || ((!flag || 1 == 0) && (!flag2 || 1 == 0)))
			{
				num = Marshal.ReadInt32(intPtr, 4);
				num2 = num & 0xF1FFFFF;
				if ((uint)num != num2)
				{
					num2 |= 0x20000000000L;
				}
				num3 = 0;
				if (num3 != 0)
				{
					goto IL_00ce;
				}
				goto IL_00f3;
			}
			goto IL_0174;
			IL_014e:
			bool flag3;
			if (flag3 && 0 == 0)
			{
				return new ValidationResult(num2 == 0, pothu.apvfl(num2), num);
			}
			goto IL_0174;
			IL_00f3:
			if (num3 < dujke.Length)
			{
				goto IL_00ce;
			}
			if (dahxy.kxxtc && 0 == 0)
			{
				flag3 = true;
				if (flag3)
				{
					goto IL_014e;
				}
			}
			bool flag4 = (num2 & 8) == 0;
			flag3 = (p5 ? true : false) || flag4;
			if (!flag3 || 1 == 0)
			{
				certificateChain = omsaq(p1, intPtr);
				flag3 = cbgmj(certificateChain);
			}
			goto IL_014e;
			IL_0174:
			if (certificateChain == null || 1 == 0)
			{
				certificateChain = omsaq(p1, intPtr);
			}
			goto end_IL_0032;
			IL_00ce:
			if (((ulong)p3 & (ulong)dujke[num3 + 1]) != 0)
			{
				num2 &= ~dujke[num3];
			}
			num3 += 2;
			goto IL_00f3;
			end_IL_0032:;
		}
		finally
		{
			if (intPtr != IntPtr.Zero && 0 == 0)
			{
				pothu.CertFreeCertificateChain(intPtr);
			}
		}
		CertificateEngine p7 = CertificateEngine.Default;
		ValidationResult validationResult = ixklj.wcbka(p7, null, null, certificateChain, new CertificateValidationParameters(p3, p4), null);
		return new ValidationResult(validationResult.Valid, (long)validationResult.Status, num);
	}

	private static bool cbgmj(CertificateChain p0)
	{
		IEnumerator<Certificate> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Certificate current = enumerator.Current;
				if (current.KeyAlgorithm != KeyAlgorithm.RSA && 0 == 0 && current.KeyAlgorithm != KeyAlgorithm.DSA)
				{
					return false;
				}
				if (!CryptoHelper.jspwz || 1 == 0)
				{
					SignatureHashAlgorithm signatureHashAlgorithm = current.GetSignatureHashAlgorithm();
					if ((signatureHashAlgorithm >= SignatureHashAlgorithm.SHA256 && signatureHashAlgorithm <= SignatureHashAlgorithm.SHA512) || signatureHashAlgorithm == SignatureHashAlgorithm.SHA224)
					{
						return false;
					}
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return true;
	}

	public void Save(string certificatePath)
	{
		if (certificatePath == null || 1 == 0)
		{
			throw new ArgumentNullException("certificatePath");
		}
		if (certificatePath.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Empty certificate path.", certificatePath);
		}
		Stream stream = vtdxm.bolpl(certificatePath);
		try
		{
			Save(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Save(Stream output)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		SignedData signedData = new SignedData();
		IEnumerator<Certificate> enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Certificate current = enumerator.Current;
				signedData.Certificates.Add(current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		signedData.Save(output);
	}

	public static CertificateChain LoadDer(string path)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		string text = "CERTIFICATE";
		string value = "-----BEGIN CERTIFICATE-----";
		string text2 = "-----END CERTIFICATE-----";
		int p = 1048576;
		Stream stream = vtdxm.prsfm(path);
		byte[] array;
		try
		{
			array = CryptoHelper.zxtjl(stream, p);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
		if (array.Length == 0 || 1 == 0)
		{
			throw new CertificateException("No data in file specified.");
		}
		List<Certificate> list = new List<Certificate>();
		if (array[0] != 48)
		{
			string text3 = EncodingTools.dmppd.GetString(array, 0, array.Length).Replace("\r", "");
			int num = 0;
			while (true)
			{
				int num2 = text3.IndexOf(value, num);
				if (num2 < 0)
				{
					break;
				}
				num = text3.IndexOf(text2, num2);
				if (num < 0)
				{
					throw new CertificateException("Unexpected certificate format.");
				}
				num += text2.Length;
				array = kjhmn.btkpl(text3.Substring(num2, num - num2), out var p2);
				if (p2 != text && 0 == 0)
				{
					throw new CertificateException("Unexpected certificate format.");
				}
				list.Add(new Certificate(array));
			}
			if (list.Count == 0 || 1 == 0)
			{
				throw new CertificateException("No certificates found in the file.");
			}
			return CertificateEngine.Default.BuildChain(list[0], list);
		}
		Certificate certificate = new Certificate(array);
		return CertificateEngine.Default.BuildChain(certificate, null);
	}
}
