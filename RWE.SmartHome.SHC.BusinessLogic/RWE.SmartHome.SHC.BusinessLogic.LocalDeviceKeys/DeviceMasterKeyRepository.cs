using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Org.Mentalis.Security.Certificates;
using RWE.SmartHome.SHC.BusinessLogic.USBLogExporter.Export;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;

public class DeviceMasterKeyRepository : IDeviceMasterKeyRepository
{
	private const string MasterExportKeyPath = "\\NandFlash\\local.xml";

	private const string MasterKeyAttribute = "MasterKey";

	private const int AES_IV_LENGTH = 16;

	private readonly ICertificateManager certificateManager;

	private readonly XmlSerializer deserializer = new XmlSerializer(typeof(KeyVault));

	private byte[] backedKeyBytes;

	public DeviceMasterKeyRepository(ICertificateManager certificateManager)
	{
		this.certificateManager = certificateManager;
	}

	public void StoreMasterKey(string exportKey)
	{
		Certificate certificate = GetCertificate();
		RijndaelManaged aes = GetAes128();
		using FileStream wrappedStream = new FileStream("\\NandFlash\\local.xml", FileMode.Create);
		using HashingStream hashingStream = new HashingStream(wrappedStream);
		using XmlWriter xmlWriter = XmlWriter.Create(hashingStream);
		WriteKeyValut(exportKey, xmlWriter, aes, certificate, hashingStream);
		xmlWriter.Close();
	}

	public bool IsMasterExportKeyAlreadyCreated()
	{
		if (!File.Exists("\\NandFlash\\local.xml"))
		{
			return false;
		}
		return true;
	}

	public byte[] GetMasterKeyFromFile()
	{
		Certificate certificate = GetCertificate();
		RijndaelManaged aes = GetAes128();
		KeyVault keyVault = LoadKeyValutFromFile();
		if (keyVault == null)
		{
			return null;
		}
		return GetMasterKey(keyVault, certificate, aes);
	}

	private KeyVault LoadKeyValutFromFile()
	{
		using (FileStream stream = new FileStream("\\NandFlash\\local.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			try
			{
				return (KeyVault)deserializer.Deserialize(stream);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to load key vault file. " + ex.ToString());
				File.Delete("\\NandFlash\\local.xml");
			}
		}
		return null;
	}

	private byte[] GetMasterKey(KeyVault xml, Certificate shcCertificate, RijndaelManaged aes128)
	{
		string signingCertContent = xml.SigningCertContent;
		try
		{
			new X509Certificate2(Convert.FromBase64String(signingCertContent));
		}
		catch (Exception ex)
		{
			Log.Information(Module.BusinessLogic, $"The signing certificate for the device master key is not valid, {ex.Message} {ex.StackTrace}");
		}
		string encryptionKey = xml.EncryptionKey;
		string masterKey = xml.MasterKey;
		if (string.Compare(encryptionKey, "None", ignoreCase: true) == 0)
		{
			Log.Information(Module.BusinessLogic, "Log file is not encrypted.");
			return Encoding.UTF8.GetBytes(masterKey);
		}
		byte[] array = DecryptAesKey(encryptionKey, shcCertificate);
		byte[] array2 = new byte[16];
		byte[] array3 = new byte[array.Length - 16];
		Buffer.BlockCopy(array, array.Length - 16, array2, 0, 16);
		Buffer.BlockCopy(array, 0, array3, 0, array.Length - 16);
		return AesDecrypt(StringToByteArray(masterKey), array3, array2);
	}

	private static byte[] DecryptAesKey(string encryptedData, Certificate encryptingCert)
	{
		byte[] rgb = StringToByteArray(encryptedData);
		_ = (RSACryptoServiceProvider)encryptingCert.PublicKey;
		RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)encryptingCert.PrivateKey;
		if (rSACryptoServiceProvider == null)
		{
			throw new Exception("Private key not found for the given certificate. Cannot decrypt.");
		}
		try
		{
			return rSACryptoServiceProvider.Decrypt(rgb, fOAEP: false);
		}
		finally
		{
			rSACryptoServiceProvider.Clear();
		}
	}

	private byte[] AesDecrypt(byte[] cipherText, byte[] Key, byte[] IV)
	{
		if (cipherText == null || cipherText.Length <= 0)
		{
			throw new ArgumentNullException("cipherText");
		}
		if (Key == null || Key.Length <= 0)
		{
			throw new ArgumentNullException("Key");
		}
		if (IV == null || IV.Length <= 0)
		{
			throw new ArgumentNullException("IV");
		}
		backedKeyBytes.Compare(cipherText);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		int num = 65536;
		byte[] array = new byte[num];
		try
		{
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.Key = Key;
			rijndaelManaged.IV = IV;
			ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
			using MemoryStream stream = new MemoryStream(cipherText);
			using CryptoStream input = new CryptoStream(stream, transform, CryptoStreamMode.Read);
			BinaryReader binaryReader = new BinaryReader(input);
			using MemoryStream memoryStream = new MemoryStream();
			while (true)
			{
				int num2 = binaryReader.Read(array, 0, array.Length);
				if (num2 <= 0)
				{
					break;
				}
				memoryStream.Write(array, 0, num2);
			}
			binaryReader.Close();
			byte[] array2 = new byte[memoryStream.Length];
			Buffer.BlockCopy(memoryStream.GetBuffer(), 0, array2, 0, (int)memoryStream.Length);
			return array2;
		}
		finally
		{
			rijndaelManaged.Clear();
		}
	}

	private Certificate GetCertificate()
	{
		CertificateStore certificateStore = new CertificateStore("My");
		if (certificateManager.PersonalCertificateThumbprint == null)
		{
			return certificateStore.FindCertificateByHash(certificateManager.DefaultCertificateThumbprint.ToByteArray());
		}
		return certificateStore.FindCertificateByHash(certificateManager.PersonalCertificateThumbprint.ToByteArray());
	}

	private RijndaelManaged GetAes128()
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 128;
		return rijndaelManaged;
	}

	private void WriteKeyValut(string exportKey, XmlWriter xmlWriter, RijndaelManaged aes128, Certificate certificate, HashingStream hashingStream)
	{
		xmlWriter.WriteStartElement("KeyVault");
		xmlWriter.Flush();
		hashingStream.BeginHashing(skipPreviousBracket: true);
		WriteEncryptionCertThumbprintAndEncryptionKey(certificate, xmlWriter, aes128);
		WriteSigningCertificateContentAndSigningCertificateThumbprint(certificate, xmlWriter);
		WriteMasterKey(exportKey, xmlWriter, aes128);
		xmlWriter.Flush();
		hashingStream.FinishHashing();
		xmlWriter.WriteEndElement();
	}

	private void WriteMasterKey(string exportKey, XmlWriter xmlWriter, RijndaelManaged aes128)
	{
		xmlWriter.WriteStartElement("MasterKey");
		WriteEncryptedContent(exportKey, xmlWriter, aes128);
		xmlWriter.WriteEndElement();
	}

	private void WriteEncryptedContent(string exportKey, XmlWriter xmlWriter, RijndaelManaged aes128)
	{
		try
		{
			ICryptoTransform transform = aes128.CreateEncryptor(aes128.Key, aes128.IV);
			XmlWriterStream stream = new XmlWriterStream(xmlWriter);
			using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
			byte[] array = (backedKeyBytes = Encoding.UTF8.GetBytes(exportKey));
			cryptoStream.Write(array, 0, array.Length);
		}
		finally
		{
			aes128.Clear();
		}
	}

	private void WriteEncryptionCertThumbprintAndEncryptionKey(Certificate signingCertificate, XmlWriter xmlWriter, RijndaelManaged aes128)
	{
		WriteEncryptionKey(signingCertificate, xmlWriter, aes128);
	}

	private void WriteEncryptionKey(Certificate signingCertificate, XmlWriter xmlWriter, RijndaelManaged aes128)
	{
		xmlWriter.WriteStartElement("EncryptionKey");
		RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)signingCertificate.PublicKey;
		byte[] array = new byte[aes128.Key.Length + aes128.IV.Length];
		Array.Copy(aes128.Key, array, aes128.Key.Length);
		Array.Copy(aes128.IV, 0, array, aes128.Key.Length, aes128.IV.Length);
		byte[] bytes = rSACryptoServiceProvider.Encrypt(array, fOAEP: false);
		WriteBinHex(bytes, xmlWriter);
		xmlWriter.WriteEndElement();
	}

	private void WriteSigningCertificateContentAndSigningCertificateThumbprint(Certificate signingCertificate, XmlWriter xmlWriter)
	{
		xmlWriter.WriteStartElement("SigningCertContent");
		xmlWriter.WriteString(Convert.ToBase64String(signingCertificate.GetRawCertData()));
		xmlWriter.WriteEndElement();
	}

	private void WriteBinHex(byte[] bytes, XmlWriter xmlWriter)
	{
		xmlWriter.WriteBinHex(bytes, 0, bytes.Length);
	}

	private static byte[] StringToByteArray(string hex)
	{
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}
}
