using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl.Shared;

internal abstract class ServerHandshakeLayer : HandshakeLayer
{
	protected ProtocolVersion m_MaxClientVersion;

	public ServerHandshakeLayer(RecordLayer recordLayer, SecurityOptions options)
		: base(recordLayer, options)
	{
	}

	public ServerHandshakeLayer(HandshakeLayer handshakeLayer)
		: base(handshakeLayer)
	{
		m_MaxClientVersion = ((ServerHandshakeLayer)handshakeLayer).m_MaxClientVersion;
	}

	protected override SslHandshakeStatus ProcessMessage(HandshakeMessage message)
	{
		return message.type switch
		{
			HandshakeType.ClientHello => ProcessClientHello(message), 
			HandshakeType.Certificate => ProcessCertificate(message, client: false), 
			HandshakeType.ClientKeyExchange => ProcessClientKeyExchange(message), 
			HandshakeType.CertificateVerify => ProcessCertificateVerify(message), 
			HandshakeType.Finished => ProcessFinished(message), 
			_ => throw new SslException(AlertDescription.UnexpectedMessage, "The received message was not expected from a client."), 
		};
	}

	protected SslHandshakeStatus ProcessClientHello(HandshakeMessage message)
	{
		if (m_State != HandshakeType.Nothing && m_State != HandshakeType.Finished)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "ClientHello message must be the first message or must be preceded by a Finished message.");
		}
		m_IsNegotiating = true;
		UpdateHashes(message, HashUpdate.All);
		ProtocolVersion pv = (m_MaxClientVersion = new ProtocolVersion(message.fragment[0], message.fragment[1]));
		if (CompatibilityLayer.SupportsProtocol(m_Options.Protocol, pv) && pv.GetVersionInt() != GetVersion().GetVersionInt())
		{
			throw new SslException(AlertDescription.IllegalParameter, "Unknown protocol version of the client.");
		}
		try
		{
			m_ClientTime = new byte[4];
			Array.Copy(message.fragment, 2, m_ClientTime, 0, 4);
			m_ClientRandom = new byte[28];
			Array.Copy(message.fragment, 6, m_ClientRandom, 0, 28);
			int num = message.fragment[34];
			if (num > 32)
			{
				throw new SslException(AlertDescription.IllegalParameter, "The length of the SessionID cannot be more than 32 bytes.");
			}
			m_SessionID = new byte[num];
			Array.Copy(message.fragment, 35, m_SessionID, 0, num);
			num += 35;
			int num2 = message.fragment[num] * 256 + message.fragment[num + 1];
			if (num2 < 2 || num2 % 2 != 0)
			{
				throw new SslException(AlertDescription.IllegalParameter, "The number of ciphers is invalid -or- the cipher length is not even.");
			}
			byte[] array = new byte[num2];
			Array.Copy(message.fragment, num + 2, array, 0, num2);
			m_EncryptionScheme = CipherSuites.GetCipherSuiteAlgorithm(array, m_Options.AllowedAlgorithms);
			num += num2 + 2;
			int num3 = message.fragment[num];
			if (num3 == 0)
			{
				throw new SslException(AlertDescription.IllegalParameter, "No compressor specified.");
			}
			byte[] array2 = new byte[num3];
			Array.Copy(message.fragment, num + 1, array2, 0, num3);
			m_CompressionMethod = CompressionAlgorithm.GetCompressionAlgorithm(array2, m_Options.AllowedAlgorithms);
		}
		catch (Exception e)
		{
			throw new SslException(e, AlertDescription.InternalError, "The message is invalid.");
		}
		return GetClientHelloResult();
	}

	protected SslHandshakeStatus GetClientHelloResult()
	{
		MemoryStream memoryStream = new MemoryStream();
		SslHandshakeStatus result = default(SslHandshakeStatus);
		HandshakeMessage handshakeMessage = new HandshakeMessage(HandshakeType.ServerHello, new byte[38]);
		m_ServerTime = GetUnixTime();
		m_ServerRandom = new byte[28];
		m_RNG.GetBytes(m_ServerRandom);
		handshakeMessage.fragment[0] = GetVersion().major;
		handshakeMessage.fragment[1] = GetVersion().minor;
		Array.Copy(m_ServerTime, 0, handshakeMessage.fragment, 2, 4);
		Array.Copy(m_ServerRandom, 0, handshakeMessage.fragment, 6, 28);
		handshakeMessage.fragment[34] = 0;
		Array.Copy(CipherSuites.GetCipherAlgorithmBytes(m_EncryptionScheme), 0, handshakeMessage.fragment, 35, 2);
		handshakeMessage.fragment[37] = CompressionAlgorithm.GetAlgorithmByte(m_CompressionMethod);
		byte[] array = handshakeMessage.ToBytes();
		memoryStream.Write(array, 0, array.Length);
		byte[] certificateList = GetCertificateList(m_Options.Certificate);
		handshakeMessage.type = HandshakeType.Certificate;
		handshakeMessage.fragment = certificateList;
		array = handshakeMessage.ToBytes();
		memoryStream.Write(array, 0, array.Length);
		if (m_Options.Certificate.GetPublicKeyLength() > 512 && CipherSuites.GetCipherDefinition(m_EncryptionScheme).Exportable)
		{
			MemoryStream memoryStream2 = new MemoryStream();
			MD5SHA1CryptoServiceProvider mD5SHA1CryptoServiceProvider = new MD5SHA1CryptoServiceProvider();
			mD5SHA1CryptoServiceProvider.TransformBlock(m_ClientTime, 0, 4, m_ClientTime, 0);
			mD5SHA1CryptoServiceProvider.TransformBlock(m_ClientRandom, 0, 28, m_ClientRandom, 0);
			mD5SHA1CryptoServiceProvider.TransformBlock(m_ServerTime, 0, 4, m_ServerTime, 0);
			mD5SHA1CryptoServiceProvider.TransformBlock(m_ServerRandom, 0, 28, m_ServerRandom, 0);
			m_KeyCipher = new RSACryptoServiceProvider(512);
			RSAParameters rSAParameters = m_KeyCipher.ExportParameters(includePrivateParameters: false);
			array = new byte[2]
			{
				(byte)(rSAParameters.Modulus.Length / 256),
				(byte)(rSAParameters.Modulus.Length % 256)
			};
			memoryStream2.Write(array, 0, 2);
			memoryStream2.Write(rSAParameters.Modulus, 0, rSAParameters.Modulus.Length);
			mD5SHA1CryptoServiceProvider.TransformBlock(array, 0, 2, array, 0);
			mD5SHA1CryptoServiceProvider.TransformBlock(rSAParameters.Modulus, 0, rSAParameters.Modulus.Length, rSAParameters.Modulus, 0);
			array = new byte[2]
			{
				(byte)(rSAParameters.Exponent.Length / 256),
				(byte)(rSAParameters.Exponent.Length % 256)
			};
			memoryStream2.Write(array, 0, 2);
			memoryStream2.Write(rSAParameters.Exponent, 0, rSAParameters.Exponent.Length);
			mD5SHA1CryptoServiceProvider.TransformBlock(array, 0, 2, array, 0);
			mD5SHA1CryptoServiceProvider.TransformFinalBlock(rSAParameters.Exponent, 0, rSAParameters.Exponent.Length);
			array = mD5SHA1CryptoServiceProvider.CreateSignature(m_Options.Certificate);
			memoryStream2.Write(new byte[2]
			{
				(byte)(array.Length / 256),
				(byte)(array.Length % 256)
			}, 0, 2);
			memoryStream2.Write(array, 0, array.Length);
			handshakeMessage.type = HandshakeType.ServerKeyExchange;
			handshakeMessage.fragment = memoryStream2.ToArray();
			array = handshakeMessage.ToBytes();
			memoryStream.Write(array, 0, array.Length);
			memoryStream2.Close();
		}
		else
		{
			m_KeyCipher = (RSACryptoServiceProvider)m_Options.Certificate.PrivateKey;
		}
		if (m_MutualAuthentication)
		{
			array = GetDistinguishedNames();
			if (array.Length != 0)
			{
				handshakeMessage.type = HandshakeType.CertificateRequest;
				handshakeMessage.fragment = new byte[array.Length + 4];
				handshakeMessage.fragment[0] = 1;
				handshakeMessage.fragment[1] = 1;
				handshakeMessage.fragment[2] = (byte)(array.Length / 256);
				handshakeMessage.fragment[3] = (byte)(array.Length % 256);
				Array.Copy(array, 0, handshakeMessage.fragment, 4, array.Length);
				array = handshakeMessage.ToBytes();
				memoryStream.Write(array, 0, array.Length);
			}
		}
		handshakeMessage.type = HandshakeType.ServerHelloDone;
		handshakeMessage.fragment = new byte[0];
		array = handshakeMessage.ToBytes();
		memoryStream.Write(array, 0, array.Length);
		result.Status = SslStatus.ContinueNeeded;
		result.Message = memoryStream.ToArray();
		memoryStream.Close();
		UpdateHashes(result.Message, HashUpdate.All);
		result.Message = m_RecordLayer.EncryptBytes(result.Message, 0, result.Message.Length, ContentType.Handshake);
		return result;
	}

	protected byte[] GetDistinguishedNames()
	{
		MemoryStream memoryStream = new MemoryStream();
		CertificateStore certificateStore = new CertificateStore("ROOT");
		for (Certificate certificate = certificateStore.FindCertificate(null); certificate != null; certificate = certificateStore.FindCertificate(certificate))
		{
			if ((certificate.GetIntendedKeyUsage() & 4) != 0 && certificate.IsCurrent)
			{
				byte[] distinguishedName = GetDistinguishedName(certificate);
				if (memoryStream.Length + distinguishedName.Length + 2 < 65536)
				{
					memoryStream.Write(new byte[2]
					{
						(byte)(distinguishedName.Length / 256),
						(byte)(distinguishedName.Length % 256)
					}, 0, 2);
					memoryStream.Write(distinguishedName, 0, distinguishedName.Length);
				}
			}
		}
		return memoryStream.ToArray();
	}

	protected byte[] GetDistinguishedName(Certificate c)
	{
		CertificateInfo certificateInfo = c.GetCertificateInfo();
		byte[] array = new byte[certificateInfo.SubjectcbData];
		Marshal.Copy(certificateInfo.SubjectpbData, array, 0, array.Length);
		return array;
	}

	protected byte[] GetCertificateList(Certificate certificate)
	{
		Certificate[] certificates = certificate.GetCertificateChain().GetCertificates();
		byte[][] array = new byte[certificates.Length][];
		int num = 0;
		for (int i = 0; i < certificates.Length; i++)
		{
			array[i] = certificates[i].ToCerBuffer();
			num += array[i].Length + 3;
		}
		MemoryStream memoryStream = new MemoryStream(num + 3 * certificates.Length + 3);
		memoryStream.WriteByte((byte)(num / 65536));
		memoryStream.WriteByte((byte)(num % 65536 / 256));
		memoryStream.WriteByte((byte)(num % 256));
		for (int j = 0; j < array.Length; j++)
		{
			num = array[j].Length;
			memoryStream.WriteByte((byte)(num / 65536));
			memoryStream.WriteByte((byte)(num % 65536 / 256));
			memoryStream.WriteByte((byte)(num % 256));
			memoryStream.Write(array[j], 0, num);
		}
		return memoryStream.ToArray();
	}

	protected SslHandshakeStatus ProcessClientKeyExchange(HandshakeMessage message)
	{
		if (!(m_MutualAuthentication ? (m_State == HandshakeType.Certificate) : (m_State == HandshakeType.ClientHello)))
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "ClientKeyExchange message must be preceded by a ClientHello or Certificate message.");
		}
		UpdateHashes(message, HashUpdate.All);
		byte[] array;
		try
		{
			if (message.fragment.Length % 8 == 2)
			{
				if (message.fragment[0] * 256 + message.fragment[1] != message.fragment.Length - 2)
				{
					throw new SslException(AlertDescription.DecodeError, "Invalid ClientKeyExchange message.");
				}
				array = new byte[message.fragment.Length - 2];
				Array.Copy(message.fragment, 2, array, 0, array.Length);
				message.fragment = array;
			}
			RSAKeyTransform rSAKeyTransform = new RSAKeyTransform(m_KeyCipher);
			array = rSAKeyTransform.DecryptKeyExchange(message.fragment);
			if (array.Length != 48)
			{
				throw new SslException(AlertDescription.IllegalParameter, "Invalid message.");
			}
			if ((m_Options.Flags & SecurityFlags.IgnoreMaxProtocol) == 0)
			{
				if (array[0] != m_MaxClientVersion.major || array[1] != m_MaxClientVersion.minor)
				{
					throw new SslException(AlertDescription.IllegalParameter, "Version rollback detected.");
				}
			}
			else if (array[0] != 3 || (array[1] != 0 && array[1] != 1))
			{
				throw new SslException(AlertDescription.IllegalParameter, "Invalid protocol version detected.");
			}
			m_KeyCipher.Clear();
			m_KeyCipher = null;
		}
		catch (Exception)
		{
			array = new byte[48];
			m_RNG.GetBytes(array);
		}
		GenerateCiphers(array);
		return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
	}

	protected SslHandshakeStatus ProcessCertificateVerify(HandshakeMessage message)
	{
		if (m_State != HandshakeType.ClientKeyExchange)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "CertificateVerify message must be preceded by a ClientKeyExchange message.");
		}
		UpdateHashes(message, HashUpdate.LocalRemote);
		byte[] array;
		if (message.fragment.Length % 8 == 2)
		{
			if (message.fragment[0] * 256 + message.fragment[1] != message.fragment.Length - 2)
			{
				throw new SslException(AlertDescription.DecodeError, "Invalid CertificateVerify message.");
			}
			array = new byte[message.fragment.Length - 2];
			Array.Copy(message.fragment, 2, array, 0, array.Length);
		}
		else
		{
			array = message.fragment;
		}
		m_CertSignHash.MasterKey = m_MasterSecret;
		m_CertSignHash.TransformFinalBlock(array, 0, 0);
		if (!m_CertSignHash.VerifySignature(m_RemoteCertificate, array))
		{
			throw new SslException(AlertDescription.CertificateUnknown, "The signature in the CertificateVerify message is invalid.");
		}
		return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
	}

	protected SslHandshakeStatus ProcessFinished(HandshakeMessage message)
	{
		if (m_State != HandshakeType.ChangeCipherSpec)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "Finished message must be preceded by a ChangeCipherSpec message.");
		}
		VerifyFinishedMessage(message.fragment);
		UpdateHashes(message, HashUpdate.Local);
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = m_RecordLayer.EncryptBytes(new byte[1] { 1 }, 0, 1, ContentType.ChangeCipherSpec);
		memoryStream.Write(array, 0, array.Length);
		m_RecordLayer.ChangeLocalState(null, m_CipherSuite.Encryptor, m_CipherSuite.LocalHasher);
		array = GetFinishedMessage();
		array = m_RecordLayer.EncryptBytes(array, 0, array.Length, ContentType.Handshake);
		memoryStream.Write(array, 0, array.Length);
		m_State = HandshakeType.Nothing;
		if (m_CipherSuite.Encryptor.OutputBlockSize != 1 && (m_Options.Flags & SecurityFlags.DontSendEmptyRecord) == 0)
		{
			byte[] array2 = m_RecordLayer.EncryptBytes(new byte[0], 0, 0, ContentType.ApplicationData);
			memoryStream.Write(array2, 0, array2.Length);
		}
		byte[] message2 = memoryStream.ToArray();
		memoryStream.Close();
		m_IsNegotiating = false;
		ClearHandshakeStructures();
		return new SslHandshakeStatus(SslStatus.OK, message2);
	}

	protected override SslHandshakeStatus ProcessChangeCipherSpec(RecordMessage message)
	{
		if (message.length != 1 || message.fragment[0] != 1)
		{
			throw new SslException(AlertDescription.IllegalParameter, "The ChangeCipherSpec message was invalid.");
		}
		if (m_State == HandshakeType.ClientKeyExchange || m_State == HandshakeType.CertificateVerify)
		{
			m_RecordLayer.ChangeRemoteState(null, m_CipherSuite.Decryptor, m_CipherSuite.RemoteHasher);
			return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
		}
		throw new SslException(AlertDescription.UnexpectedMessage, "ChangeCipherSpec message must be preceded by a ClientKeyExchange or CertificateVerify message.");
	}

	protected override byte[] GetRenegotiateBytes()
	{
		if (IsNegotiating())
		{
			return null;
		}
		HandshakeMessage handshakeMessage = new HandshakeMessage(HandshakeType.HelloRequest, new byte[0]);
		return m_RecordLayer.EncryptBytes(handshakeMessage.ToBytes(), 0, 4, ContentType.Handshake);
	}

	protected override byte[] GetClientHello()
	{
		throw new SslException(AlertDescription.InternalError, "This is a server socket; it cannot send client hello messages");
	}

	public override SslHandshakeStatus ProcessSsl2Hello(byte[] hello)
	{
		if (m_State != HandshakeType.Nothing)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "SSL2 ClientHello message must be the first message.");
		}
		m_IsNegotiating = true;
		m_State = HandshakeType.ClientHello;
		UpdateHashes(hello, HashUpdate.All);
		ProtocolVersion pv = (m_MaxClientVersion = new ProtocolVersion(hello[1], hello[2]));
		if (CompatibilityLayer.SupportsProtocol(m_Options.Protocol, pv) && pv.GetVersionInt() != GetVersion().GetVersionInt())
		{
			throw new SslException(AlertDescription.IllegalParameter, "Unknown protocol version of the client.");
		}
		int num = hello[3] * 256 + hello[4];
		int num2 = hello[5] * 256 + hello[6];
		int num3 = hello[7] * 256 + hello[8];
		byte[] array = new byte[num / 3 * 2];
		int num4 = 10;
		for (int i = 0; i < array.Length; i += 2)
		{
			Array.Copy(hello, num4, array, i, 2);
			num4 += 3;
		}
		m_EncryptionScheme = CipherSuites.GetCipherSuiteAlgorithm(array, m_Options.AllowedAlgorithms);
		m_SessionID = new byte[num2];
		Array.Copy(hello, 9 + num, m_SessionID, 0, num2);
		m_ClientTime = new byte[4];
		m_ClientRandom = new byte[28];
		if (num3 <= 28)
		{
			Array.Copy(hello, 9 + num + num2, m_ClientRandom, m_ClientRandom.Length - num3, num3);
		}
		else
		{
			Array.Copy(hello, 9 + num + num2 + (num3 - 28), m_ClientRandom, 0, 28);
			Array.Copy(hello, 9 + num + num2, m_ClientTime, 4 - (num3 - 28), num3 - 28);
		}
		m_CompressionMethod = SslAlgorithms.NULL_COMPRESSION;
		return GetClientHelloResult();
	}
}
