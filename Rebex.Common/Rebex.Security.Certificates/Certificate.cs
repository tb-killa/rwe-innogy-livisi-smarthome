using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Security.Certificates;

public sealed class Certificate : IDisposable, ltutw, lnabj
{
	private const string ezksm = "2.5.29.19";

	private byte[] nubvi;

	private string zthrn;

	[NonSerialized]
	private pmvop ntltw;

	[NonSerialized]
	private nisgb goiwn;

	[NonSerialized]
	private bool tccrb;

	[NonSerialized]
	private KeyAlgorithm ayhnk;

	[NonSerialized]
	private AsymmetricKeyAlgorithm awxad;

	[NonSerialized]
	private AsymmetricKeyAlgorithm bagvf;

	[NonSerialized]
	private AsymmetricKeyAlgorithm xjcjm;

	[NonSerialized]
	private CertificateStore udmyy;

	[NonSerialized]
	private int? kqvsh;

	[NonSerialized]
	private object vffax;

	[NonSerialized]
	private nnzwd jpfuv;

	internal bool dngwn => GetIssuer().Equals(GetSubject());

	public IntPtr Handle => rvuod().fiocy();

	public CertificateExtensionCollection Extensions => goiwn.fdmty;

	public KeyAlgorithm KeyAlgorithm => ayhnk;

	public string Thumbprint
	{
		get
		{
			byte[] array = HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, nubvi);
			return BitConverter.ToString(array, 0, array.Length).Replace("-", "").emrfc();
		}
	}

	public object Tag
	{
		get
		{
			return vffax;
		}
		set
		{
			vffax = value;
		}
	}

	internal pmvop asabh()
	{
		return ntltw;
	}

	internal pmvop rvuod()
	{
		if (ntltw == null || 1 == 0)
		{
			ntltw = new pmvop(nubvi);
			tccrb = true;
		}
		return ntltw;
	}

	public Certificate(byte[] data)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (data.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("No data.", "data");
		}
		data = ((data[0] == 48) ? data.aqhfc() : dcmqo(data));
		nisgb p = new nisgb(data);
		bbdbq(data, p, null);
	}

	private Certificate(byte[] data, nisgb parsed, string friendlyName)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		bbdbq(data, parsed, friendlyName);
	}

	public static Certificate LoadDer(string path)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		Stream stream = vtdxm.prsfm(path);
		try
		{
			return LoadDer(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public static Certificate LoadDer(Stream stream)
	{
		if (stream == null || 1 == 0)
		{
			throw new ArgumentNullException("stream");
		}
		byte[] data = CryptoHelper.zxtjl(stream, 32767);
		return new Certificate(data);
	}

	public static Certificate LoadDerWithKey(string certificatePath, string privateKeyPath, string privateKeyPassword)
	{
		if (certificatePath == null || 1 == 0)
		{
			throw new ArgumentNullException("certificatePath");
		}
		if (privateKeyPath == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKeyPath");
		}
		Stream stream = vtdxm.prsfm(certificatePath);
		try
		{
			Stream stream2 = vtdxm.prsfm(privateKeyPath);
			try
			{
				return LoadDerWithKey(stream, stream2, privateKeyPassword);
			}
			finally
			{
				if (stream2 != null && 0 == 0)
				{
					((IDisposable)stream2).Dispose();
				}
			}
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public static Certificate LoadDerWithKey(Stream certificateStream, Stream privateKeyStream, string privateKeyPassword)
	{
		if (privateKeyStream == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKeyStream");
		}
		if (certificateStream == null || 1 == 0)
		{
			throw new ArgumentNullException("certificateStream");
		}
		Certificate certificate = LoadDer(certificateStream);
		PrivateKeyInfo privateKeyInfo = new PrivateKeyInfo();
		privateKeyInfo.Load(privateKeyStream, privateKeyPassword);
		certificate.Associate(privateKeyInfo);
		return certificate;
	}

	private void pxbbb(RSAParameters p0)
	{
		RSAParameters rSAParameters = GetRSAParameters();
		bool flag = false;
		if (zjcch.wduyr(rSAParameters.Exponent, p0.Exponent) && 0 == 0 && zjcch.wduyr(rSAParameters.Modulus, p0.Modulus) && 0 == 0)
		{
			flag = true;
		}
		if (!flag || 1 == 0)
		{
			throw new CryptographicException("The key used by the CSP does not correspond to the certificate's public key.");
		}
	}

	private void hkrrs(DSAParameters p0)
	{
		DSAParameters dSAParameters = GetDSAParameters();
		bool flag = false;
		if (zjcch.wduyr(dSAParameters.P, p0.P) && 0 == 0 && zjcch.wduyr(dSAParameters.Q, p0.Q) && 0 == 0 && zjcch.wduyr(dSAParameters.G, p0.G) && 0 == 0 && (dSAParameters.J == null || false || p0.J == null || false || zjcch.wduyr(dSAParameters.J, p0.J)) && zjcch.wduyr(dSAParameters.Y, p0.Y) && 0 == 0)
		{
			flag = true;
		}
		if (!flag || 1 == 0)
		{
			throw new CryptographicException("The key used by the CSP does not correspond to the certificate's public key.");
		}
	}

	private void gudos(AsymmetricKeyAlgorithm p0)
	{
		if (p0.PublicOnly && 0 == 0)
		{
			throw new CryptographicException("The supplied CSP can only perform public key operations.");
		}
		switch (p0.Algorithm)
		{
		case AsymmetricKeyAlgorithmId.RSA:
		{
			if (ayhnk != KeyAlgorithm.RSA && 0 == 0)
			{
				throw new CryptographicException("Certificate is not using RSA keys.");
			}
			RSAParameters rSAParameters = p0.GetPublicKey().GetRSAParameters();
			pxbbb(rSAParameters);
			break;
		}
		case AsymmetricKeyAlgorithmId.DSA:
		{
			if (ayhnk != KeyAlgorithm.DSA)
			{
				throw new CryptographicException("Certificate is not using DSA keys.");
			}
			DSAParameters dSAParameters = p0.GetPublicKey().GetDSAParameters();
			hkrrs(dSAParameters);
			break;
		}
		default:
			dgodq(p0);
			break;
		}
	}

	private void dgodq(AsymmetricKeyAlgorithm p0)
	{
		AsymmetricKeyAlgorithmId algorithm = p0.Algorithm;
		AsymmetricKeyAlgorithmId asymmetricKeyAlgorithmId = bpkgq.zjdcx(KeyAlgorithm);
		if (algorithm != asymmetricKeyAlgorithmId)
		{
			throw new CryptographicException(brgjd.edcru("Certificate is not using {0} keys.", bpkgq.bjusl(asymmetricKeyAlgorithmId)));
		}
		if (p0.PublicOnly && 0 == 0)
		{
			throw new CryptographicException("The supplied CSP can only perform public key operations.");
		}
		if (!jlfbq.oreja(GetPublicKey(), p0.zimkk()) || 1 == 0)
		{
			throw new CryptographicException("The key used by the CSP does not correspond to the certificate's public key.");
		}
	}

	public void Associate(AsymmetricAlgorithm privateKey)
	{
		Associate(privateKey, permanentBind: false);
	}

	public void Associate(PrivateKeyInfo privateKey)
	{
		if (privateKey == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKey");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		asymmetricKeyAlgorithm.ImportKey(privateKey);
		Associate(asymmetricKeyAlgorithm, permanentBind: false);
	}

	public void Associate(AsymmetricKeyAlgorithm privateKey)
	{
		Associate(privateKey, permanentBind: false);
	}

	public void Associate(AsymmetricAlgorithm privateKey, bool permanentBind)
	{
		if (privateKey == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKey");
		}
		AsymmetricKeyAlgorithm privateKey2 = AsymmetricKeyAlgorithm.CreateFrom(privateKey, ownsAlgorithm: false);
		Associate(privateKey2, permanentBind);
	}

	public void Associate(PrivateKeyInfo privateKey, bool permanentBind)
	{
		if (privateKey == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKey");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		asymmetricKeyAlgorithm.ImportKey(privateKey);
		Associate(asymmetricKeyAlgorithm, permanentBind);
	}

	public void Associate(AsymmetricKeyAlgorithm privateKey, bool permanentBind)
	{
		if (privateKey == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKey");
		}
		gudos(privateKey);
		if (!permanentBind || 1 == 0)
		{
			xjcjm = privateKey;
			return;
		}
		if (pothu.aicde)
		{
			CspParameters cspParameters = privateKey.GetCspParameters();
			if (cspParameters != null && 0 == 0)
			{
				hflqg.jqcpo(this, cspParameters);
			}
			else
			{
				PrivateKeyInfo privateKey2 = privateKey.GetPrivateKey();
				wldhb.zafys(this, privateKey2, CryptoHelper.mdumc);
			}
			xjcjm = privateKey;
			return;
		}
		throw new CryptographicException("Permanent bind is not supported on this platform.");
	}

	internal string pjvyh()
	{
		return Guid.NewGuid().ToString();
	}

	public static Certificate LoadPfx(string path, string password)
	{
		return LoadPfx(path, password, KeySetOptions.Exportable | KeySetOptions.UserKeySet);
	}

	public static Certificate LoadPfx(byte[] data, string password)
	{
		return LoadPfx(data, password, KeySetOptions.Exportable | KeySetOptions.UserKeySet);
	}

	public static Certificate LoadPfx(string path, string password, KeySetOptions options)
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

	public static Certificate LoadPfx(byte[] data, string password, KeySetOptions options)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		if (pothu.aicde && 0 == 0)
		{
			CertificateStore certificateStore = CertificateStore.adphl(data, password, options);
			Certificate[] array = certificateStore.FindCertificates(CertificateFindOptions.HasPrivateKey);
			if (array == null || false || array.Length == 0 || 1 == 0)
			{
				certificateStore.Dispose();
				throw new CertificateException("No certificate with a private key found in PFX.");
			}
			Certificate certificate = array[0];
			certificate.nhtyb(certificateStore);
			if ((options & KeySetOptions.PersistKeySet) == 0 || 1 == 0)
			{
				certificate.rwaxf(p0: true);
			}
			return certificate;
		}
		throw new CryptographicException("Access to CryptoAPI is not available.");
	}

	public void Save(string fileName)
	{
		Save(fileName, CertificateFormat.Der, null);
	}

	public void Save(Stream stream)
	{
		Save(stream, CertificateFormat.Der, null);
	}

	public void Save(string fileName, CertificateFormat format)
	{
		Save(fileName, format, null);
	}

	public void Save(Stream stream, CertificateFormat format)
	{
		Save(stream, format, null);
	}

	public void Save(string fileName, CertificateFormat format, string password)
	{
		Stream stream = vtdxm.bolpl(fileName);
		try
		{
			Save(stream, format, password);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Save(Stream stream, CertificateFormat format, string password)
	{
		byte[] array;
		switch (format)
		{
		case CertificateFormat.Der:
		case CertificateFormat.Base64Der:
			array = GetRawCertData();
			break;
		case CertificateFormat.Pfx:
			if (ayhnk == KeyAlgorithm.ED25519)
			{
				throw new NotSupportedException(string.Concat("PFX/P12 saving is not supported for ", ayhnk, " key algorithm."));
			}
			if (ayhnk == KeyAlgorithm.ECDsa && dahxy.ucaou && 0 == 0 && (!dahxy.uttbp || 1 == 0))
			{
				throw new NotSupportedException(string.Concat("PFX/P12 saving is not supported for ", ayhnk, " key algorithm on this platform."));
			}
			if (pothu.aicde)
			{
				AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(p0: true);
				CspParameters cspParameters = asymmetricKeyAlgorithm.GetCspParameters();
				if (cspParameters != null && 0 == 0)
				{
					array = wldhb.fxwel(this, cspParameters, password);
					break;
				}
				PrivateKeyInfo privateKey = asymmetricKeyAlgorithm.GetPrivateKey();
				array = wldhb.jzwyy(this, privateKey, password);
				break;
			}
			throw new NotSupportedException("PFX/P12 saving is not supported on this platform.");
		default:
			throw hifyx.nztrs("format", format, "Unsupported format.");
		}
		if (format == CertificateFormat.Base64Der)
		{
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				StreamWriter streamWriter = new StreamWriter(memoryStream, EncodingTools.ASCII);
				try
				{
					streamWriter.WriteLine("-----BEGIN CERTIFICATE-----");
					kjhmn.jsvbw(streamWriter, array);
					streamWriter.WriteLine("-----END CERTIFICATE-----");
				}
				finally
				{
					if (streamWriter != null && 0 == 0)
					{
						((IDisposable)streamWriter).Dispose();
					}
				}
				array = memoryStream.ToArray();
			}
			finally
			{
				if (memoryStream != null && 0 == 0)
				{
					((IDisposable)memoryStream).Dispose();
				}
			}
		}
		stream.Write(array, 0, array.Length);
	}

	public void SavePrivateKey(string fileName, string password, PrivateKeyFormat format, bool silent)
	{
		Stream stream = vtdxm.bolpl(fileName);
		try
		{
			SavePrivateKey(stream, password, format, silent);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void SavePrivateKey(Stream stream, string password, PrivateKeyFormat format, bool silent)
	{
		PrivateKeyInfo privateKeyInfo = GetPrivateKeyInfo(silent);
		privateKeyInfo.Save(stream, password, format);
	}

	public PrivateKeyInfo GetPrivateKeyInfo(bool silent)
	{
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(silent);
		return asymmetricKeyAlgorithm.GetPrivateKey();
	}

	public Certificate(IntPtr handle)
	{
		if (handle == IntPtr.Zero && 0 == 0)
		{
			throw new ArgumentException("Handle is zero.", "handle");
		}
		vxuqr(new pmvop(handle), p1: true);
	}

	public Certificate(X509Certificate certificate)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		if (dahxy.kfygb() && 0 == 0)
		{
			vxuqr(new pmvop(certificate.Handle), p1: true);
		}
		else
		{
			vxuqr(new pmvop(certificate), p1: true);
		}
	}

	public static implicit operator Certificate(X509Certificate certificate)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		return new Certificate(certificate);
	}

	public static implicit operator X509Certificate(Certificate certificate)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		return new X509Certificate(certificate.GetRawCertData());
	}

	internal Certificate nehgg()
	{
		return new Certificate(nubvi, goiwn, zthrn);
	}

	private void vxuqr(pmvop p0, bool p1)
	{
		string p2 = null;
		bbdbq(p0.hsodj(), null, p2);
		ntltw = p0;
		tccrb = p1;
	}

	private void bbdbq(byte[] p0, nisgb p1, string p2)
	{
		nubvi = p0;
		zthrn = p2;
		nisgb nisgb = p1;
		if (nisgb == null || 1 == 0)
		{
			nisgb = new nisgb(p0);
		}
		goiwn = nisgb;
		ayhnk = goiwn.jmggg.qlesd();
	}

	private static byte[] dcmqo(byte[] p0)
	{
		string text = "-----BEGIN CERTIFICATE-----";
		string text2 = "-----END CERTIFICATE-----";
		string text3 = EncodingTools.dmppd.GetString(p0, 0, p0.Length);
		int num = text3.IndexOf(text, 0);
		if (num < 0)
		{
			throw new CertificateException("Unexpected certificate format.");
		}
		int num2 = text3.IndexOf(text2, num + text.Length);
		if (num2 < 0)
		{
			throw new CertificateException("Unexpected certificate format.");
		}
		text3 = text3.Substring(num, num2 + text2.Length - num);
		try
		{
			return kjhmn.bhchj(text3);
		}
		catch (FormatException innerException)
		{
			throw new CertificateException("Invalid certificate encoding.", innerException);
		}
	}

	public SignatureHashAlgorithm GetSignatureHashAlgorithm()
	{
		return goiwn.qwiov.ldvqi();
	}

	internal AlgorithmIdentifier dyagy()
	{
		return goiwn.qwiov;
	}

	public CrlDistributionPointCollection GetCrlDistributionPoints()
	{
		CertificateExtension certificateExtension = prkbw("2.5.29.31");
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		CrlDistributionPointCollection crlDistributionPointCollection = new CrlDistributionPointCollection();
		hfnnn.qnzgo(crlDistributionPointCollection, certificateExtension.Value);
		crlDistributionPointCollection.hksnh();
		return crlDistributionPointCollection;
	}

	internal vcnjn vgnxi()
	{
		CertificateExtension certificateExtension = prkbw("1.3.6.1.5.5.7.1.1");
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		vcnjn vcnjn = new vcnjn();
		hfnnn.qnzgo(vcnjn, certificateExtension.Value);
		vcnjn.hksnh();
		return vcnjn;
	}

	public DistinguishedName GetIssuer()
	{
		return goiwn.zofhh;
	}

	public string GetIssuerName()
	{
		return GetIssuer().ToString();
	}

	public DistinguishedName GetSubject()
	{
		return goiwn.phamo;
	}

	public string GetSubjectName()
	{
		return GetSubject().ToString();
	}

	public byte[] GetPublicKey()
	{
		return goiwn.lwwfi();
	}

	public byte[] GetKeyAlgorithmParameters()
	{
		return goiwn.jmggg.Parameters;
	}

	internal AlgorithmIdentifier lukcl()
	{
		return goiwn.jmggg;
	}

	internal string awyrh()
	{
		return bpkgq.cafoz(goiwn.jmggg);
	}

	public PublicKeyInfo GetPublicKeyInfo()
	{
		switch (ayhnk)
		{
		case KeyAlgorithm.DSA:
		{
			DSAParameters dSAParameters = GetDSAParameters();
			return new PublicKeyInfo(dSAParameters);
		}
		case KeyAlgorithm.RSA:
		{
			RSAParameters rSAParameters = GetRSAParameters();
			return new PublicKeyInfo(rSAParameters);
		}
		case KeyAlgorithm.ECDsa:
		{
			AlgorithmIdentifier algorithm = lukcl();
			byte[] publicKey = GetPublicKey();
			return new PublicKeyInfo(algorithm, publicKey);
		}
		case KeyAlgorithm.ED25519:
		{
			AlgorithmIdentifier algorithm = AlgorithmIdentifier.xhnfa("1.3.101.112", null);
			byte[] publicKey = GetPublicKey();
			return new PublicKeyInfo(algorithm, publicKey);
		}
		default:
			throw new NotSupportedException("The key algorithm of this certificate is not supported.");
		}
	}

	public byte[] GetSubjectKeyIdentifier()
	{
		CertificateExtension certificateExtension = prkbw("2.5.29.14");
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		return rwolq.tvjgt(certificateExtension.Value).rtrhq;
	}

	public byte[] GetAuthorityKeyIdentifier()
	{
		CertificateExtension certificateExtension = prkbw("2.5.29.35");
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		ohkxj ohkxj = new ohkxj();
		hfnnn.qnzgo(ohkxj, certificateExtension.Value);
		return ohkxj.gvwnt;
	}

	public string GetCommonName()
	{
		DistinguishedName subject = GetSubject();
		return subject.GetCommonName();
	}

	public string[] GetCommonNames()
	{
		CertificateExtension certificateExtension = prkbw("2.5.29.17");
		azfwz azfwz;
		List<string> list;
		string[] array;
		int num;
		if (certificateExtension != null && 0 == 0)
		{
			azfwz = new azfwz();
			hfnnn.qnzgo(azfwz, certificateExtension.Value);
			list = new List<string>();
			array = azfwz.hhoay(ukmqt.zhlgm);
			num = 0;
			if (num != 0)
			{
				goto IL_0046;
			}
			goto IL_0059;
		}
		goto IL_00b9;
		IL_0070:
		IPAddress[] array2;
		int num2;
		IPAddress iPAddress = array2[num2];
		list.Add(iPAddress.ToString());
		num2++;
		goto IL_008a;
		IL_008a:
		if (num2 < array2.Length)
		{
			goto IL_0070;
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		if (certificateExtension.Critical && 0 == 0)
		{
			return new string[0];
		}
		goto IL_00b9;
		IL_0046:
		string item = array[num];
		list.Add(item);
		num++;
		goto IL_0059;
		IL_0059:
		if (num < array.Length)
		{
			goto IL_0046;
		}
		array2 = azfwz.daile();
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_0070;
		}
		goto IL_008a;
		IL_00b9:
		string commonName = GetCommonName();
		if (commonName != null && 0 == 0)
		{
			return new string[1] { commonName };
		}
		return new string[0];
	}

	public string[] GetMailAddresses()
	{
		CertificateExtension certificateExtension = prkbw("2.5.29.17");
		List<string> list;
		string[] array;
		int num;
		if (certificateExtension != null && 0 == 0)
		{
			azfwz azfwz = new azfwz();
			hfnnn.qnzgo(azfwz, certificateExtension.Value);
			list = new List<string>();
			array = azfwz.hhoay(ukmqt.yzwxj);
			num = 0;
			if (num != 0)
			{
				goto IL_0043;
			}
			goto IL_0056;
		}
		goto IL_0085;
		IL_0056:
		if (num < array.Length)
		{
			goto IL_0043;
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		if (certificateExtension.Critical && 0 == 0)
		{
			return new string[0];
		}
		goto IL_0085;
		IL_0085:
		DistinguishedName subject = GetSubject();
		return subject.GetMailAddresses();
		IL_0043:
		string item = array[num];
		list.Add(item);
		num++;
		goto IL_0056;
	}

	private string[] slrwi(ukmqt p0)
	{
		CertificateExtension certificateExtension = prkbw("2.5.29.17");
		List<string> list;
		string[] array;
		int num;
		if (certificateExtension != null && 0 == 0)
		{
			azfwz azfwz = new azfwz();
			hfnnn.qnzgo(azfwz, certificateExtension.Value);
			list = new List<string>();
			array = azfwz.hhoay(p0);
			num = 0;
			if (num != 0)
			{
				goto IL_0046;
			}
			goto IL_0059;
		}
		goto IL_008f;
		IL_0059:
		if (num < array.Length)
		{
			goto IL_0046;
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		if (certificateExtension.Critical && 0 == 0)
		{
			return new string[0];
		}
		goto IL_008f;
		IL_008f:
		return null;
		IL_0046:
		string item = array[num];
		list.Add(item);
		num++;
		goto IL_0059;
	}

	internal bool gjmyp(string p0)
	{
		CertificateExtension certificateExtension = prkbw("2.5.29.17");
		string[] array;
		int num;
		if (certificateExtension != null && 0 == 0)
		{
			azfwz azfwz = new azfwz();
			hfnnn.qnzgo(azfwz, certificateExtension.Value);
			array = azfwz.hhoay(ukmqt.yzwxj);
			num = 0;
			if (num != 0)
			{
				goto IL_0040;
			}
			goto IL_0063;
		}
		goto IL_007d;
		IL_0063:
		if (num < array.Length)
		{
			goto IL_0040;
		}
		if (certificateExtension.Critical && 0 == 0)
		{
			return false;
		}
		goto IL_007d;
		IL_007d:
		DistinguishedName subject = GetSubject();
		return subject.gcuoq(p0);
		IL_0040:
		string strA = array[num];
		if (string.Compare(strA, p0, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			return true;
		}
		num++;
		goto IL_0063;
	}

	internal CertificateExtension prkbw(string p0)
	{
		CertificateExtension certificateExtension = Extensions[p0];
		if (certificateExtension == null || false || certificateExtension.Value == null)
		{
			return null;
		}
		return certificateExtension;
	}

	internal xxerq ortjx()
	{
		CertificateExtension certificateExtension = Extensions["2.5.29.19"];
		xxerq xxerq = new xxerq();
		if (certificateExtension != null && 0 == 0)
		{
			hfnnn.qnzgo(xxerq, certificateExtension.Value);
		}
		return xxerq;
	}

	public byte[] GetSerialNumber()
	{
		return goiwn.dfjyj();
	}

	public DateTime GetExpirationDate()
	{
		return goiwn.ocdia;
	}

	public DateTime GetEffectiveDate()
	{
		return goiwn.rnidf;
	}

	public byte[] GetCertHash()
	{
		return HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, nubvi);
	}

	public byte[] GetRawCertData()
	{
		return nubvi.aqhfc();
	}

	public bool IsTimeValid()
	{
		DateTime now = DateTime.Now;
		DateTime effectiveDate = GetEffectiveDate();
		DateTime expirationDate = GetExpirationDate();
		if (effectiveDate <= now && 0 == 0)
		{
			return now < expirationDate;
		}
		return false;
	}

	public KeyUses GetIntendedUsage()
	{
		htykq htykq = new htykq();
		CertificateExtension certificateExtension = prkbw("2.5.29.15");
		if (certificateExtension == null || false || certificateExtension.Value == null)
		{
			return KeyUses.DigitalSignature | KeyUses.NonRepudiation | KeyUses.KeyEncipherment | KeyUses.DataEncipherment | KeyUses.KeyAgreement | KeyUses.KeyCertSign | KeyUses.CrlSign | KeyUses.EncipherOnly | KeyUses.DecipherOnly;
		}
		hfnnn.qnzgo(htykq, certificateExtension.Value);
		return (KeyUses)htykq.xmojg();
	}

	[Obsolete("The GetUsage method has been deprecated. Please use GetEnhancedUsage method instead.", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string[] GetUsage()
	{
		return GetEnhancedUsage();
	}

	public string[] GetEnhancedUsage()
	{
		motgc motgc = new motgc();
		CertificateExtension certificateExtension = prkbw("2.5.29.37");
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		hfnnn.qnzgo(motgc, certificateExtension.Value);
		string[] array = new string[motgc.Count];
		int num = 0;
		if (num != 0)
		{
			goto IL_003d;
		}
		goto IL_0050;
		IL_0050:
		if (num < array.Length)
		{
			goto IL_003d;
		}
		return array;
		IL_003d:
		array[num] = motgc[num].Value;
		num++;
		goto IL_0050;
	}

	public DSAParameters GetDSAParameters()
	{
		if (ayhnk != KeyAlgorithm.DSA)
		{
			throw new CertificateException("Certificate does not contain DSA parameters.");
		}
		byte[] keyAlgorithmParameters = GetKeyAlgorithmParameters();
		if (keyAlgorithmParameters.Length == 0 || 1 == 0)
		{
			throw new CertificateException("Certificates with inherited DSA parameters are not supported.");
		}
		return pujje(keyAlgorithmParameters);
	}

	internal DSAParameters pujje(byte[] p0)
	{
		DSAParameters p1 = ocawh.rgahm(p0);
		byte[] publicKey = GetPublicKey();
		if (publicKey == null || false || publicKey.Length == 0 || 1 == 0)
		{
			throw new CertificateException("Certificate does not contain DSA parameters.");
		}
		p1.Y = zjcch.yowmh(publicKey).rtrhq;
		return DSAManaged.busby(p1);
	}

	public RSAParameters GetRSAParameters()
	{
		if (ayhnk != KeyAlgorithm.RSA && 0 == 0)
		{
			throw new CertificateException("Certificate does not contain RSA parameters.");
		}
		byte[] publicKey = GetPublicKey();
		if (publicKey == null || false || publicKey.Length == 0 || 1 == 0)
		{
			throw new CertificateException("Certificate does not contain RSA parameters.");
		}
		rcqtb rcqtb = new rcqtb();
		hfnnn.qnzgo(rcqtb, publicKey);
		return rcqtb.hqyyf();
	}

	public RSAParameters GetRSAParameters(bool exportPrivateKeys, bool silent)
	{
		if (ayhnk != KeyAlgorithm.RSA && 0 == 0)
		{
			throw new CertificateException("Certificate does not contain RSA parameters.");
		}
		if (!exportPrivateKeys || 1 == 0)
		{
			return GetRSAParameters();
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(silent);
		PrivateKeyInfo privateKey = asymmetricKeyAlgorithm.GetPrivateKey();
		return privateKey.GetRSAParameters();
	}

	public DSAParameters GetDSAParameters(bool exportPrivateKeys, bool silent)
	{
		if (ayhnk != KeyAlgorithm.DSA)
		{
			throw new CertificateException("Certificate does not contain DSA parameters.");
		}
		if (!exportPrivateKeys || 1 == 0)
		{
			return GetDSAParameters();
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(silent);
		PrivateKeyInfo privateKey = asymmetricKeyAlgorithm.GetPrivateKey();
		return privateKey.GetDSAParameters();
	}

	public bool HasPrivateKey()
	{
		return kivnt(p0: true);
	}

	internal bool kivnt(bool p0)
	{
		if (xjcjm != null && 0 == 0)
		{
			return !xjcjm.PublicOnly;
		}
		if (awxad == null || 1 == 0)
		{
			awxad = wldhb.uktlc(this, p0, p2: false, out var p1);
			if (p1 && 0 == 0)
			{
				return true;
			}
		}
		return awxad != null;
	}

	internal PrivateKeyInfo ihwyt()
	{
		if (awxad == null || 1 == 0)
		{
			awxad = wldhb.beraz(this, p1: true, p2: false);
		}
		if (awxad == null || 1 == 0)
		{
			return null;
		}
		return awxad.GetPrivateKey();
	}

	internal AsymmetricKeyAlgorithm woudq()
	{
		return awxad;
	}

	private AsymmetricKeyAlgorithm ddysr(bool p0)
	{
		if (xjcjm != null && 0 == 0)
		{
			if (xjcjm.PublicOnly && 0 == 0)
			{
				throw new CryptographicException("Associated key is not private.");
			}
			return xjcjm;
		}
		if (awxad == null || 1 == 0)
		{
			awxad = wldhb.beraz(this, p0, p2: true);
		}
		return awxad;
	}

	private AsymmetricKeyAlgorithm tonfs()
	{
		if (xjcjm != null && 0 == 0)
		{
			return xjcjm;
		}
		if (bagvf == null || 1 == 0)
		{
			bagvf = wldhb.hidpx(this);
		}
		return bagvf;
	}

	public int GetKeySize()
	{
		if (!kqvsh.HasValue || 1 == 0)
		{
			switch (ayhnk)
			{
			case KeyAlgorithm.DSA:
				kqvsh = GetDSAParameters().P.Length * 8;
				break;
			case KeyAlgorithm.RSA:
				kqvsh = GetRSAParameters().Modulus.Length * 8;
				break;
			case KeyAlgorithm.ED25519:
				kqvsh = 256;
				break;
			case KeyAlgorithm.ECDsa:
			{
				AlgorithmIdentifier p = lukcl();
				string p2 = bpkgq.cafoz(p);
				int num = bpkgq.kgsco(p2);
				if (num != 0 && 0 == 0)
				{
					kqvsh = num;
					break;
				}
				goto default;
			}
			default:
				throw new NotSupportedException("The key algorithm of this certificate is not supported.");
			}
		}
		return kqvsh.Value;
	}

	internal int wwnxt()
	{
		return (GetKeySize() + 7) / 8;
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
		xsnsm(hash, alg);
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = tonfs();
		return asymmetricKeyAlgorithm.VerifyHash(hash, alg, signature);
	}

	internal bool bvdsv(byte[] p0, byte[] p1, SignatureParameters p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = tonfs();
		return asymmetricKeyAlgorithm.hefzy(p0, p1, p2);
	}

	public bool VerifyMessage(byte[] message, byte[] signature)
	{
		return VerifyMessage(message, signature, null);
	}

	public bool VerifyMessage(byte[] message, byte[] signature, SignatureParameters parameters)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		if (signature == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = tonfs();
		return asymmetricKeyAlgorithm.VerifyMessage(message, signature, parameters);
	}

	public byte[] SignHash(byte[] hash, SignatureHashAlgorithm alg, bool silent)
	{
		if (hash == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		switch (ayhnk)
		{
		case KeyAlgorithm.DSA:
			if (alg != SignatureHashAlgorithm.SHA1)
			{
				throw new CertificateException("Only SHA-1 hash algorithm is supported by DSA certificates.");
			}
			break;
		case KeyAlgorithm.ED25519:
			throw new NotSupportedException("The SignHash method is not supported for this key algorithm. Use SignMessage method instead.");
		default:
			throw new NotSupportedException("The key algorithm of this certificate is not supported.");
		case KeyAlgorithm.RSA:
		case KeyAlgorithm.ECDsa:
			break;
		}
		xsnsm(hash, alg);
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(silent);
		return asymmetricKeyAlgorithm.SignHash(hash, alg);
	}

	internal byte[] vvtcj(byte[] p0, SignatureParameters p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		bool p2 = p1 == null || 1 == 0 || p1.Silent;
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(p2);
		return asymmetricKeyAlgorithm.sizlw(p0, p1);
	}

	public byte[] SignMessage(byte[] message)
	{
		return SignMessage(message, null);
	}

	public byte[] SignMessage(byte[] message, SignatureParameters parameters)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		bool p = parameters == null || 1 == 0 || parameters.Silent;
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(p);
		return asymmetricKeyAlgorithm.SignMessage(message, parameters);
	}

	private static void xsnsm(byte[] p0, SignatureHashAlgorithm p1)
	{
		switch (p1)
		{
		case SignatureHashAlgorithm.MD5:
			if (p0.Length != 16)
			{
				throw new CertificateException("Invalid MD5 hash.");
			}
			break;
		case SignatureHashAlgorithm.SHA1:
			if (p0.Length != 20)
			{
				throw new CertificateException("Invalid SHA-1 hash.");
			}
			break;
		case SignatureHashAlgorithm.SHA224:
			if (p0.Length != 28)
			{
				throw new CertificateException("Invalid SHA-224 hash.");
			}
			break;
		case SignatureHashAlgorithm.SHA256:
			if (p0.Length != 32)
			{
				throw new CertificateException("Invalid SHA-256 hash.");
			}
			break;
		case SignatureHashAlgorithm.SHA384:
			if (p0.Length != 48)
			{
				throw new CertificateException("Invalid SHA-384 hash.");
			}
			break;
		case SignatureHashAlgorithm.SHA512:
			if (p0.Length != 64)
			{
				throw new CertificateException("Invalid SHA-512 hash.");
			}
			break;
		case SignatureHashAlgorithm.MD5SHA1:
			if (p0.Length == 36)
			{
				break;
			}
			throw new CertificateException("Invalid MD5SHA1 hash.");
		default:
			throw new CertificateException("Unsupported signature hash algorithm.");
		}
	}

	public byte[] Encrypt(byte[] rgb)
	{
		return hoier(rgb, null);
	}

	internal byte[] hoier(byte[] p0, EncryptionParameters p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgb");
		}
		switch (ayhnk)
		{
		case KeyAlgorithm.DSA:
		case KeyAlgorithm.ECDsa:
			throw new NotSupportedException("This certificate does not support encryption.");
		default:
			throw new NotSupportedException("The key algorithm of this certificate is not supported.");
		case KeyAlgorithm.RSA:
		{
			AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = tonfs();
			return asymmetricKeyAlgorithm.Encrypt(p0, p1);
		}
		}
	}

	public byte[] Decrypt(byte[] rgb, bool silent)
	{
		EncryptionParameters encryptionParameters = new EncryptionParameters();
		encryptionParameters.Silent = silent;
		return nivtp(rgb, encryptionParameters);
	}

	internal byte[] nivtp(byte[] p0, EncryptionParameters p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgb");
		}
		switch (ayhnk)
		{
		case KeyAlgorithm.DSA:
		case KeyAlgorithm.ECDsa:
			throw new NotSupportedException("This certificate does not support encryption.");
		default:
			throw new NotSupportedException("The key algorithm of this certificate is not supported.");
		case KeyAlgorithm.RSA:
		{
			bool p2 = p1 == null || 1 == 0 || p1.Silent;
			AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = ddysr(p2);
			return asymmetricKeyAlgorithm.Decrypt(p0, p1);
		}
		}
	}

	public ValidationResult Validate()
	{
		return CertificateChain.wjdra(CertificateChainEngine.Auto, this, null, null, ValidationOptions.None);
	}

	public ValidationResult Validate(ValidationOptions options)
	{
		return CertificateChain.wjdra(CertificateChainEngine.Auto, this, null, null, options);
	}

	public ValidationResult Validate(string serverName, ValidationOptions options)
	{
		return CertificateChain.wjdra(CertificateChainEngine.Auto, this, null, serverName, options);
	}

	public ValidationResult Validate(string serverName, ValidationOptions options, CertificateChainEngine engine)
	{
		return CertificateChain.wjdra(engine, this, null, serverName, options);
	}

	public CertificateRevocationListStatus ValidateRevocationList(CertificateRevocationList crl, DateTime? validationTime)
	{
		if (crl == null || 1 == 0)
		{
			throw new ArgumentNullException("crl");
		}
		return crl.vlpfp(this, validationTime, p2: true, null);
	}

	internal bool mprva(Certificate p0)
	{
		return goiwn.qpdba(p0);
	}

	public void Dispose()
	{
		avemo();
	}

	private void avemo()
	{
		if (awxad != null && 0 == 0)
		{
			awxad.Dispose();
		}
		if (bagvf != null && 0 == 0)
		{
			bagvf.Dispose();
		}
		if (udmyy != null && 0 == 0)
		{
			udmyy.Dispose();
		}
		if (tccrb && 0 == 0 && ntltw != null && 0 == 0)
		{
			ntltw.wbhry();
		}
	}

	internal void nhtyb(CertificateStore p0)
	{
		udmyy = p0;
	}

	internal void rwaxf(bool p0)
	{
		if (p0 && 0 == 0)
		{
			HasPrivateKey();
		}
		if (awxad != null && 0 == 0)
		{
			awxad.zceob(p0);
		}
	}

	internal Certificate()
	{
	}

	private void dgsgn(rmkkr p0, bool p1, int p2)
	{
		jpfuv = new nnzwd();
		jpfuv.zkxnk(p0, p1, p2);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dgsgn
		this.dgsgn(p0, p1, p2);
	}

	private lnabj dfpup(rmkkr p0, bool p1, int p2)
	{
		return jpfuv.qaqes(p0, p1, p2);
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dfpup
		return this.dfpup(p0, p1, p2);
	}

	private void vfxav(byte[] p0, int p1, int p2)
	{
		jpfuv.lnxah(p0, p1, p2);
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vfxav
		this.vfxav(p0, p1, p2);
	}

	private void wprwm()
	{
		jpfuv.somzq();
		byte[] lktyp = jpfuv.lktyp;
		if (lktyp.Length > 0 && ((lktyp[0] & 0x80) == 0 || 1 == 0))
		{
			bbdbq(lktyp, null, null);
			jpfuv = null;
			return;
		}
		throw new CryptographicException("Invalid certificate data in ASN.1 blob.");
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in wprwm
		this.wprwm();
	}

	private void sxvsa(fxakl p0)
	{
		nnzwd p1 = new nnzwd(GetRawCertData());
		p0.kfyej(p1);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sxvsa
		this.sxvsa(p0);
	}
}
