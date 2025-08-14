using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Org.Mentalis.Security.Certificates;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Properties;
using SHCWrapper.Crypto;

namespace RWE.SmartHome.SHC.Core.Certificates;

public sealed class CertificateManager : ICertificateManager, IService
{
	internal class RegisteredCert
	{
		public string AppId { get; private set; }

		public string StoreId { get; private set; }

		public RegisteredCert(string appId, string storeId)
		{
			AppId = appId;
			StoreId = storeId;
		}
	}

	private readonly ConfigurationProperties properties;

	private string defaultCertificateThumbprint;

	private string personalCertificateThumbprint;

	private string defaultCertificateSearchString = "local.rwe.shmprod.Smarthome.Client.SHCDefaultCertificates";

	private Dictionary<byte[], List<RegisteredCert>> addinRegisteredThumbprints = new Dictionary<byte[], List<RegisteredCert>>();

	public string DefaultCertificateThumbprint => defaultCertificateThumbprint;

	public string PersonalCertificateThumbprint => personalCertificateThumbprint;

	public CertificateManager(IConfigurationManager configurationManager)
	{
		properties = new ConfigurationProperties(configurationManager);
	}

	public void DeletePersonalCertificate()
	{
		CertificateStore certificateStore = new CertificateStore("My");
		Certificate[] array = certificateStore.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			Extension extension = certificate.FindExtension("2.5.29.17");
			if (extension != null)
			{
				string text = CertificateHandling.ExtractSubjectAlternateNameFromCertificate(extension.EncodedValue);
				string[] array2 = text.Split('@');
				string text2 = SHCSerialNumber.SerialNumber();
				if (array2[0] == text2)
				{
					certificateStore.DeleteCertificate(certificate);
					personalCertificateThumbprint = null;
					BackupRegistry();
					Console.WriteLine("[CertificateManager] Personal certificate deleted!");
					break;
				}
			}
		}
	}

	public void ExtractPersonalCertificateThumbprint()
	{
		Certificate certificate = FindPersonalCertificate();
		if (certificate != null)
		{
			personalCertificateThumbprint = certificate.GetCertHashString();
		}
		Console.WriteLine(string.IsNullOrEmpty(personalCertificateThumbprint) ? "[CertificateManager] No personal certificate found in store" : "[CertificateManager] Found personal certificate in store");
	}

	public X509Certificate2 GetPersonalCertificate()
	{
		Certificate certificate = FindPersonalCertificate();
		if (certificate != null)
		{
			return new X509Certificate2(certificate.ToX509().Handle);
		}
		return null;
	}

	public List<byte[]> AddCertificate(string appId, byte[] cert, string storeId)
	{
		List<byte[]> list = new List<byte[]>();
		CertificateStore certificateStore = new CertificateStore(storeId);
		CertificateStore certificateStore2 = new CertificateStore(cert, CertificateStoreType.Pkcs7Message);
		Certificate[] array = certificateStore2.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			byte[] certHash = certificate.GetCertHash();
			if (certificateStore.FindCertificateByHash(certHash) == null)
			{
				certificateStore.AddCertificate(certificate);
				list.Add(certHash);
			}
			AddToRegisteredThumbprints(appId, certHash, storeId);
		}
		return list;
	}

	public void RemoveCertificate(string appId, byte[] thumbprint, string storeId)
	{
		RemoveFromRegisteredThumbprints(appId, thumbprint);
		if (CanRemoveCertificate(thumbprint))
		{
			CertificateStore certificateStore = new CertificateStore(storeId);
			Certificate certificate = certificateStore.FindCertificateByHash(thumbprint);
			if (certificate != null)
			{
				certificateStore.DeleteCertificate(certificate);
			}
		}
	}

	public void CleanupCertificates(string appId)
	{
		Dictionary<byte[], string> dictionary = new Dictionary<byte[], string>();
		foreach (KeyValuePair<byte[], List<RegisteredCert>> addinRegisteredThumbprint in addinRegisteredThumbprints)
		{
			foreach (RegisteredCert item in addinRegisteredThumbprints[addinRegisteredThumbprint.Key])
			{
				if (item.AppId == appId)
				{
					dictionary.Add(addinRegisteredThumbprint.Key, item.StoreId);
				}
			}
		}
		foreach (KeyValuePair<byte[], string> item2 in dictionary)
		{
			RemoveCertificate(appId, item2.Key, item2.Value);
		}
	}

	public void Initialize()
	{
		ImportIntermediateCAsFromResources(Resources.shc_ca_certs);
		ImportRootCAsFromResources(Resources.shc_root_certs);
		ImportIntermediateCAsFromResources(Resources.SHCSubCA_VerisignNew);
		ImportRootCAsFromResources(Resources.SHCRootCA_VerisignNew);
		ImportRootCAsFromResources(Resources.BaltimoreCyberTrustRoot);
		ImportRootCAsFromResources(Resources.DigiCertGlobalRootG2);
		ImportRootCAsFromResources(Resources.DigiCertGlobalRootCA);
		ImportRootCAsFromResources(Resources.DTRUSTRootClass3CA22009);
		ImportRootCAsFromResources(Resources.MicrosoftEVECCRootCA2017);
		ImportRootCAsFromResources(Resources.MicrosoftRSARootCA2017);
		ImportRootCAsFromFile();
		ImportIntermediateCAsFromFile();
		bool? forceRegistration = properties.ForceRegistration;
		bool flag = forceRegistration.HasValue && forceRegistration.Value;
		if (string.IsNullOrEmpty(properties.DefaultCertificateFile))
		{
			OverrideDefaultCertificateSearchString();
			SearchDefaultCertificateInMyStore();
			if (flag)
			{
				ClearMyStore();
			}
		}
		else
		{
			if (flag)
			{
				defaultCertificateThumbprint = null;
				ClearMyStore();
			}
			ImportDefaultCertificateFromFile();
		}
		ExtractPersonalCertificateThumbprint();
	}

	public void Uninitialize()
	{
	}

	[DllImport("shc_api.dll")]
	private static extern void BackupRegistry();

	private void ClearMyStore()
	{
		CertificateStore certificateStore = new CertificateStore("My");
		int num = 0;
		Certificate[] array = certificateStore.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			if (certificate.GetCertHashString() != defaultCertificateThumbprint)
			{
				certificateStore.DeleteCertificate(certificate);
				num++;
			}
			else
			{
				Console.WriteLine("[CertificateManager] Skipped deletion of Default Certificate since no cert file is specified.");
			}
		}
		if (num > 0)
		{
			BackupRegistry();
		}
		Console.WriteLine("[CertificateManager] Removed certificates from MY store. Count = " + num);
	}

	private static void ImportIntermediateCAsFromResources(byte[] shcCaCerts)
	{
		CertificateStore certificateStore = new CertificateStore("CA");
		CertificateStore certificateStore2 = new CertificateStore(shcCaCerts, CertificateStoreType.Pkcs7Message);
		bool flag = false;
		Certificate[] array = certificateStore2.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			if (certificateStore.FindCertificateByHash(certificate.GetCertHash()) == null)
			{
				certificateStore.AddCertificate(certificate);
				flag = true;
			}
		}
		if (flag)
		{
			BackupRegistry();
		}
	}

	private static void ImportRootCAsFromResources(byte[] shcRootCerts)
	{
		CertificateStore certificateStore = new CertificateStore("Root");
		CertificateStore certificateStore2 = new CertificateStore(shcRootCerts, CertificateStoreType.Pkcs7Message);
		bool flag = false;
		Certificate[] array = certificateStore2.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			if (certificateStore.FindCertificateByHash(certificate.GetCertHash()) == null)
			{
				certificateStore.AddCertificate(certificate);
				flag = true;
			}
		}
		if (flag)
		{
			BackupRegistry();
		}
	}

	private void ImportDefaultCertificateFromFile()
	{
		if (string.IsNullOrEmpty(properties.DefaultCertificateFile) || string.IsNullOrEmpty(properties.DefaultCertificatePassword))
		{
			return;
		}
		Certificate certificate = Certificate.CreateFromPfxFile(properties.DefaultCertificateFile, properties.DefaultCertificatePassword);
		if (certificate == null)
		{
			Console.WriteLine($"[CertificateManager] Default Certificate could not be retrieved from the PFX: {properties.DefaultCertificateFile}");
			return;
		}
		CertificateStore certificateStore = new CertificateStore("My");
		if (certificateStore.FindCertificateByHash(certificate.GetCertHash()) == null)
		{
			if (!SHCWrapper.Crypto.Certificates.ImportCertificate(properties.DefaultCertificateFile, "MY", properties.DefaultCertificatePassword))
			{
				Console.WriteLine("[CertificateManager] Import Default Certificate from file failed (TPM full?)");
				return;
			}
			BackupRegistry();
			Console.WriteLine("[CertificateManager] Import Default Certificate from file succeeded");
		}
		else
		{
			Console.WriteLine("[CertificateManager] Default Certificate from file already found in store");
		}
		defaultCertificateThumbprint = certificate.GetCertHashString();
		CalculateSearchString(certificate);
	}

	private void SearchDefaultCertificateInMyStore()
	{
		CertificateStore certificateStore = new CertificateStore("My");
		Certificate[] array = certificateStore.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			if (CheckDistinguishedName(certificate, defaultCertificateSearchString))
			{
				defaultCertificateThumbprint = certificate.GetCertHashString();
				Console.WriteLine("[CertificateManager] Found Default certificate in store via search string.");
				CalculateSearchString(certificate);
			}
		}
	}

	private static bool CheckDistinguishedName(Certificate certificate, string searchString)
	{
		DistinguishedName distinguishedName = certificate.GetDistinguishedName();
		string[] array = searchString.Split('.');
		if (distinguishedName.Count < array.Length)
		{
			return false;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (string.Compare(distinguishedName[i].Value, array[i], ignoreCase: true) != 0)
			{
				return false;
			}
		}
		return true;
	}

	private static string CalculateSearchString(Certificate certificate)
	{
		DistinguishedName distinguishedName = certificate.GetDistinguishedName();
		string text = "";
		string text2 = "";
		for (int i = 0; i < distinguishedName.Count; i++)
		{
			if (distinguishedName[i].ObjectID == "0.9.2342.19200300.100.1.25" || distinguishedName[i].ObjectID == "2.5.4.11")
			{
				if (text != "")
				{
					text += ".";
				}
				text += distinguishedName[i].Value;
				continue;
			}
			if (distinguishedName[i].ObjectID == "2.5.4.3")
			{
				text2 = distinguishedName[i].Value;
			}
			break;
		}
		Console.WriteLine("[CertificateManager] Default Certificate Distinguished Name = " + text + "." + text2);
		Console.WriteLine("[CertificateManager] Default Certificate corresponding search string = " + text);
		return text;
	}

	private void OverrideDefaultCertificateSearchString()
	{
		if (!string.IsNullOrEmpty(properties.DefaultCertificateSearchString))
		{
			defaultCertificateSearchString = properties.DefaultCertificateSearchString;
			Console.WriteLine("[CertificateManager] Override Default Certificate Search String = " + properties.DefaultCertificateSearchString);
		}
	}

	private void ImportIntermediateCAsFromFile()
	{
		if (string.IsNullOrEmpty(properties.IntermediateCertificatesFile))
		{
			return;
		}
		CertificateStore certificateStore = new CertificateStore("CA");
		CertificateStore certificateStore2 = CertificateStore.CreateFromP7bFile(properties.IntermediateCertificatesFile);
		bool flag = false;
		Certificate[] array = certificateStore2.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			if (certificateStore.FindCertificateByHash(certificate.GetCertHash()) == null)
			{
				certificateStore.AddCertificate(certificate);
				flag = true;
			}
		}
		if (flag)
		{
			BackupRegistry();
		}
	}

	private void ImportRootCAsFromFile()
	{
		if (string.IsNullOrEmpty(properties.RootCertificatesFile))
		{
			return;
		}
		CertificateStore certificateStore = new CertificateStore("Root");
		CertificateStore certificateStore2 = CertificateStore.CreateFromP7bFile(properties.RootCertificatesFile);
		bool flag = false;
		Certificate[] array = certificateStore2.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			if (certificateStore.FindCertificateByHash(certificate.GetCertHash()) == null)
			{
				certificateStore.AddCertificate(certificate);
				flag = true;
			}
		}
		if (flag)
		{
			BackupRegistry();
		}
	}

	private Certificate FindPersonalCertificate()
	{
		CertificateStore certificateStore = new CertificateStore("My");
		personalCertificateThumbprint = null;
		Certificate[] array = certificateStore.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			Extension extension = certificate.FindExtension("2.5.29.17");
			if (extension != null)
			{
				string text = CertificateHandling.ExtractSubjectAlternateNameFromCertificate(extension.EncodedValue);
				string[] array2 = text.Split('@');
				string text2 = SHCSerialNumber.SerialNumber();
				if (array2[0] == text2)
				{
					personalCertificateThumbprint = certificate.GetCertHashString();
					return certificate;
				}
			}
		}
		return null;
	}

	private void AddToRegisteredThumbprints(string appId, byte[] thumbprint, string storeId)
	{
		if (addinRegisteredThumbprints.ContainsKey(thumbprint))
		{
			bool flag = false;
			foreach (RegisteredCert item in addinRegisteredThumbprints[thumbprint])
			{
				if (item.AppId == appId)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				addinRegisteredThumbprints[thumbprint].Add(new RegisteredCert(appId, storeId));
			}
		}
		else
		{
			addinRegisteredThumbprints.Add(thumbprint, new List<RegisteredCert>
			{
				new RegisteredCert(appId, storeId)
			});
		}
	}

	private bool CanRemoveCertificate(byte[] thumbprint)
	{
		if (addinRegisteredThumbprints.ContainsKey(thumbprint))
		{
			return addinRegisteredThumbprints[thumbprint].Count == 0;
		}
		return true;
	}

	private void RemoveFromRegisteredThumbprints(string appId, byte[] thumbprint)
	{
		if (!addinRegisteredThumbprints.ContainsKey(thumbprint))
		{
			return;
		}
		RegisteredCert registeredCert = null;
		foreach (RegisteredCert item in addinRegisteredThumbprints[thumbprint])
		{
			if (item.AppId == appId)
			{
				registeredCert = item;
				break;
			}
		}
		if (registeredCert != null)
		{
			addinRegisteredThumbprints[thumbprint].Remove(registeredCert);
		}
	}
}
