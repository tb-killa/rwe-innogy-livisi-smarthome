using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Org.Mentalis.Security.Certificates;
using RWE.SmartHome.SHC.BusinessLogic.Properties;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces.Exceptions;

namespace RWE.SmartHome.SHC.BusinessLogic.USBLogExporter.Export;

public class LogExporter
{
	private const string NONE = "None";

	private readonly IFileLogger fileLogger;

	private readonly ICertificateManager certificateManager;

	private Certificate signingCertificate;

	private Certificate encryptionCertificate;

	private RijndaelManaged aes128;

	private HashingStream hashingStream;

	private XmlWriter xmlWriter;

	public LogExporter(IFileLogger fileLogger, ICertificateManager certificateManager)
	{
		this.fileLogger = fileLogger;
		this.certificateManager = certificateManager;
	}

	public void ExportLog(string path)
	{
		GetSigningCertificate();
		GetEncryptionCertificate();
		if (encryptionCertificate != null)
		{
			CreateAes128();
		}
		using FileStream wrappedStream = ExceptionWithWorkflowError.WrapException(() => new FileStream(path, FileMode.Create), WorkflowError.UsbStickLogExport_WriteFailed);
		using (hashingStream = new HashingStream(wrappedStream))
		{
			using (xmlWriter = XmlWriter.Create(hashingStream))
			{
				WriteUpload();
				xmlWriter.Close();
			}
		}
	}

	private void WriteUpload()
	{
		xmlWriter.WriteStartElement("Upload");
		WriteLogfile();
		WriteSignature();
		xmlWriter.WriteEndElement();
	}

	private void WriteLogfile()
	{
		xmlWriter.Flush();
		hashingStream.BeginHashing(skipPreviousBracket: true);
		xmlWriter.WriteStartElement("Logfile");
		WriteInfoElement();
		WriteContentElement();
		xmlWriter.WriteEndElement();
		xmlWriter.Flush();
		hashingStream.FinishHashing();
	}

	private void WriteSignature()
	{
		xmlWriter.WriteStartElement("Signature");
		RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)signingCertificate.PrivateKey;
		string str = CryptoConfig.MapNameToOID("SHA1");
		byte[] bytes = rSACryptoServiceProvider.SignHash(hashingStream.Hash, str);
		WriteBinHex(bytes);
		xmlWriter.WriteEndElement();
	}

	private void GetEncryptionCertificate()
	{
		encryptionCertificate = Certificate.CreateFromPemFile(RWE.SmartHome.SHC.BusinessLogic.Properties.Resources.SHCLogFileEncryptionCertificate);
	}

	private void GetSigningCertificate()
	{
		CertificateStore certificateStore = new CertificateStore("My");
		if (certificateManager.PersonalCertificateThumbprint != null)
		{
			signingCertificate = certificateStore.FindCertificateByHash(certificateManager.PersonalCertificateThumbprint.ToByteArray());
		}
		else
		{
			signingCertificate = certificateStore.FindCertificateByHash(certificateManager.DefaultCertificateThumbprint.ToByteArray());
		}
	}

	private void CreateAes128()
	{
		aes128 = new RijndaelManaged
		{
			KeySize = 128
		};
	}

	private void WriteContentElement()
	{
		xmlWriter.WriteStartElement("Content");
		if (aes128 != null)
		{
			WriteEncryptedContent();
		}
		else
		{
			WritePlainContent();
		}
		xmlWriter.WriteEndElement();
	}

	private void WritePlainContent()
	{
		fileLogger.ProcessAllLines(delegate(string line)
		{
			xmlWriter.WriteString(line);
			return true;
		});
	}

	private void WriteInfoElement()
	{
		xmlWriter.WriteStartElement("Info");
		WriteEncryptionElement();
		WriteSigningElement();
		WriteShcSerialNumberElement();
		xmlWriter.WriteEndElement();
	}

	private void WriteShcSerialNumberElement()
	{
		xmlWriter.WriteStartElement("SHCSerialNumber");
		xmlWriter.WriteString(SHCSerialNumber.SerialNumber());
		xmlWriter.WriteEndElement();
	}

	private void WriteSigningElement()
	{
		xmlWriter.WriteStartElement("Signing");
		WriteSigningCertElement();
		WriteSigningCertThumbprintElement();
		WriteSigningCertSubjectElement();
		xmlWriter.WriteEndElement();
	}

	private void WriteSigningCertSubjectElement()
	{
		xmlWriter.WriteStartElement("SigningCertSubject");
		xmlWriter.WriteString((signingCertificate != null) ? signingCertificate.GetName() : "None");
		xmlWriter.WriteEndElement();
	}

	private void WriteSigningCertThumbprintElement()
	{
		xmlWriter.WriteStartElement("SigningCertThumbprint");
		if (signingCertificate != null)
		{
			WriteBinHex(signingCertificate.GetCertHash());
		}
		else
		{
			xmlWriter.WriteString("None");
		}
		xmlWriter.WriteEndElement();
	}

	private void WriteSigningCertElement()
	{
		xmlWriter.WriteStartElement("SigningCertContent");
		xmlWriter.WriteString(Convert.ToBase64String(signingCertificate.GetRawCertData()));
		xmlWriter.WriteEndElement();
	}

	private void WriteEncryptionElement()
	{
		xmlWriter.WriteStartElement("Encryption");
		xmlWriter.WriteStartElement("EncryptionCertThumbprint");
		if (encryptionCertificate != null)
		{
			WriteBinHex(encryptionCertificate.GetCertHash());
		}
		else
		{
			xmlWriter.WriteString("None");
		}
		xmlWriter.WriteEndElement();
		xmlWriter.WriteStartElement("EncryptionCertSubject");
		xmlWriter.WriteString((encryptionCertificate != null) ? encryptionCertificate.GetName() : "None");
		xmlWriter.WriteEndElement();
		WriteEncryptionKey();
		xmlWriter.WriteEndElement();
	}

	private void WriteEncryptionKey()
	{
		xmlWriter.WriteStartElement("EncryptionKey");
		if (encryptionCertificate != null)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)encryptionCertificate.PublicKey;
			byte[] array = new byte[aes128.Key.Length + aes128.IV.Length];
			Array.Copy(aes128.Key, array, aes128.Key.Length);
			Array.Copy(aes128.IV, 0, array, aes128.Key.Length, aes128.IV.Length);
			byte[] bytes = rSACryptoServiceProvider.Encrypt(array, fOAEP: false);
			WriteBinHex(bytes);
		}
		else
		{
			xmlWriter.WriteString("None");
		}
		xmlWriter.WriteEndElement();
	}

	private void WriteEncryptedContent()
	{
		try
		{
			ICryptoTransform transform = aes128.CreateEncryptor(aes128.Key, aes128.IV);
			XmlWriterStream stream = new XmlWriterStream(xmlWriter);
			CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
			try
			{
				fileLogger.ProcessAllLines(delegate(string line)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(line);
					cryptoStream.Write(bytes, 0, bytes.Length);
					return true;
				});
			}
			finally
			{
				if (cryptoStream != null)
				{
					((IDisposable)cryptoStream).Dispose();
				}
			}
		}
		finally
		{
			aes128.Clear();
		}
	}

	private void WriteBinHex(byte[] bytes)
	{
		xmlWriter.WriteBinHex(bytes, 0, bytes.Length);
	}
}
