using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Org.Mentalis.Security.Certificates;

namespace RWE.SmartHome.SHC.Core.Configuration;

public class ConfigSignature : IDisposable
{
	private const string CertThumbprint = "11aa7f128dd9ff515df59e00173be856771a8b4a";

	private const string RootNodeName = "Settings";

	private const string SectionsNodeName = "Sections";

	private const string SignatureNodeName = "Signature";

	private readonly string fileName;

	private readonly XmlDocument doc;

	private RSACryptoServiceProvider csp;

	public X509Certificate2 SigningCertificate { get; set; }

	public ConfigSignature(string fileName)
	{
		doc = new XmlDocument();
		this.fileName = fileName;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	internal bool CheckSignature()
	{
		try
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
				doc.LoadXml(streamReader.ReadToEnd());
			}
			string nodeXml = GetNodeXml("Sections");
			byte[] signature = StringToByteArray(GetNodeXml("Signature"));
			return IsSignatureValid(Encoding.UTF8.GetBytes(nodeXml), signature);
		}
		catch
		{
			return false;
		}
	}

	private bool IsSignatureValid(byte[] encryptedData, byte[] signature)
	{
		CertificateStore certificateStore = new CertificateStore(Org.Mentalis.Security.Certificates.StoreLocation.LocalMachine, "CodeSign");
		Certificate[] array = certificateStore.EnumCertificates();
		foreach (Certificate certificate in array)
		{
			try
			{
				RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)certificate.PublicKey;
				SHA1Managed sHA1Managed = new SHA1Managed();
				try
				{
					byte[] rgbHash = sHA1Managed.ComputeHash(encryptedData);
					if (rSACryptoServiceProvider.VerifyHash(rgbHash, CryptoConfig.MapNameToOID("SHA1"), signature))
					{
						return true;
					}
				}
				finally
				{
					rSACryptoServiceProvider.Clear();
					sHA1Managed.Clear();
				}
			}
			catch (CertificateException)
			{
			}
		}
		return false;
	}

	private string GetNodeXml(string nodeName)
	{
		XmlNodeList xmlNodeList = doc.SelectNodes(string.Format("/{0}/{1}", "Settings", nodeName));
		if (xmlNodeList.Count != 1)
		{
			throw new InvalidDataException("Invalid configuration XML file.");
		}
		return xmlNodeList[0].InnerXml;
	}

	private static byte[] StringToByteArray(string hex)
	{
		if (hex.Length % 2 != 0)
		{
			return new byte[0];
		}
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}

	private void Dispose(bool disposing)
	{
		if (csp != null)
		{
			csp.Clear();
			csp = null;
		}
	}
}
