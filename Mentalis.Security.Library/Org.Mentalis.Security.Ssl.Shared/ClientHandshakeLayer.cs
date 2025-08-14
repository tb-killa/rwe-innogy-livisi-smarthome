using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl.Shared;

internal abstract class ClientHandshakeLayer : HandshakeLayer
{
	public ClientHandshakeLayer(RecordLayer recordLayer, SecurityOptions options)
		: base(recordLayer, options)
	{
	}

	public ClientHandshakeLayer(HandshakeLayer handshakeLayer)
		: base(handshakeLayer)
	{
	}

	protected override SslHandshakeStatus ProcessMessage(HandshakeMessage message)
	{
		SslHandshakeStatus sslHandshakeStatus = default(SslHandshakeStatus);
		return message.type switch
		{
			HandshakeType.ServerHello => ProcessServerHello(message), 
			HandshakeType.Certificate => ProcessCertificate(message, client: true), 
			HandshakeType.ServerKeyExchange => ProcessServerKeyExchange(message), 
			HandshakeType.CertificateRequest => ProcessCertificateRequest(message), 
			HandshakeType.ServerHelloDone => ProcessServerHelloDone(message), 
			HandshakeType.Finished => ProcessFinished(message), 
			HandshakeType.HelloRequest => ProcessHelloRequest(message), 
			_ => throw new SslException(AlertDescription.UnexpectedMessage, "The received message was not expected from a server."), 
		};
	}

	protected override byte[] GetClientHello()
	{
		if (m_State != HandshakeType.Nothing && m_State != HandshakeType.Finished)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "ClientHello message must be the first message or must be preceded by a Finished message.");
		}
		m_IsNegotiating = true;
		m_State = HandshakeType.ClientHello;
		byte[] cipherAlgorithmBytes = CipherSuites.GetCipherAlgorithmBytes(m_Options.AllowedAlgorithms);
		byte[] compressionAlgorithmBytes = CompressionAlgorithm.GetCompressionAlgorithmBytes(m_Options.AllowedAlgorithms);
		HandshakeMessage handshakeMessage = new HandshakeMessage(HandshakeType.ClientHello, new byte[38 + cipherAlgorithmBytes.Length + compressionAlgorithmBytes.Length]);
		m_ClientTime = GetUnixTime();
		m_ClientRandom = new byte[28];
		m_RNG.GetBytes(m_ClientRandom);
		ProtocolVersion maxProtocol = CompatibilityLayer.GetMaxProtocol(m_Options.Protocol);
		handshakeMessage.fragment[0] = maxProtocol.major;
		handshakeMessage.fragment[1] = maxProtocol.minor;
		Array.Copy(m_ClientTime, 0, handshakeMessage.fragment, 2, 4);
		Array.Copy(m_ClientRandom, 0, handshakeMessage.fragment, 6, 28);
		handshakeMessage.fragment[34] = 0;
		handshakeMessage.fragment[35] = (byte)(cipherAlgorithmBytes.Length / 256);
		handshakeMessage.fragment[36] = (byte)(cipherAlgorithmBytes.Length % 256);
		Array.Copy(cipherAlgorithmBytes, 0, handshakeMessage.fragment, 37, cipherAlgorithmBytes.Length);
		handshakeMessage.fragment[37 + cipherAlgorithmBytes.Length] = (byte)compressionAlgorithmBytes.Length;
		Array.Copy(compressionAlgorithmBytes, 0, handshakeMessage.fragment, 38 + cipherAlgorithmBytes.Length, compressionAlgorithmBytes.Length);
		byte[] array = handshakeMessage.ToBytes();
		UpdateHashes(array, HashUpdate.All);
		return m_RecordLayer.EncryptBytes(array, 0, array.Length, ContentType.Handshake);
	}

	protected SslHandshakeStatus ProcessServerHello(HandshakeMessage message)
	{
		if (m_State != HandshakeType.ClientHello && m_State != HandshakeType.HelloRequest)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "ServerHello message must be preceded by a ClientHello message.");
		}
		UpdateHashes(message, HashUpdate.All);
		if (message.fragment.Length < 2 || message.fragment[0] != GetVersion().major || message.fragment[1] != GetVersion().minor)
		{
			throw new SslException(AlertDescription.IllegalParameter, "Unknown protocol version of the client.");
		}
		try
		{
			m_ServerTime = new byte[4];
			Array.Copy(message.fragment, 2, m_ServerTime, 0, 4);
			m_ServerRandom = new byte[28];
			Array.Copy(message.fragment, 6, m_ServerRandom, 0, 28);
			int num = message.fragment[34];
			if (num > 32)
			{
				throw new SslException(AlertDescription.IllegalParameter, "The length of the SessionID cannot be more than 32 bytes.");
			}
			m_SessionID = new byte[num];
			Array.Copy(message.fragment, 35, m_SessionID, 0, num);
			m_EncryptionScheme = CipherSuites.GetCipherAlgorithmType(message.fragment, 35 + num);
			m_CompressionMethod = CompressionAlgorithm.GetCompressionAlgorithmType(message.fragment, 37 + num);
		}
		catch (Exception e)
		{
			throw new SslException(e, AlertDescription.InternalError, "The message is invalid.");
		}
		return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
	}

	protected SslHandshakeStatus ProcessServerKeyExchange(HandshakeMessage message)
	{
		if (m_State != HandshakeType.Certificate)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "ServerKeyExchange message must be preceded by a Certificate message.");
		}
		if (!CipherSuites.GetCipherDefinition(m_EncryptionScheme).Exportable)
		{
			throw new SslException(AlertDescription.HandshakeFailure, "The ServerKeyExchange message should not be sent for non-exportable ciphers.");
		}
		if (m_RemoteCertificate.GetPublicKeyLength() <= 512)
		{
			throw new SslException(AlertDescription.HandshakeFailure, "The ServerKeyExchange message should not be sent because the server certificate public key is of exportable length.");
		}
		UpdateHashes(message, HashUpdate.All);
		RSAParameters parameters = default(RSAParameters);
		int num = message.fragment[0] * 256 + message.fragment[1];
		parameters.Modulus = new byte[num];
		Array.Copy(message.fragment, 2, parameters.Modulus, 0, num);
		int num2 = num + 2;
		num = message.fragment[num2] * 256 + message.fragment[num2 + 1];
		parameters.Exponent = new byte[num];
		Array.Copy(message.fragment, num2 + 2, parameters.Exponent, 0, num);
		num2 += num + 2;
		parameters.Modulus = RemoveLeadingZeros(parameters.Modulus);
		parameters.Exponent = RemoveLeadingZeros(parameters.Exponent);
		m_KeyCipher = new RSACryptoServiceProvider();
		m_KeyCipher.ImportParameters(parameters);
		MD5SHA1CryptoServiceProvider mD5SHA1CryptoServiceProvider = new MD5SHA1CryptoServiceProvider();
		mD5SHA1CryptoServiceProvider.TransformBlock(m_ClientTime, 0, m_ClientTime.Length, m_ClientTime, 0);
		mD5SHA1CryptoServiceProvider.TransformBlock(m_ClientRandom, 0, m_ClientRandom.Length, m_ClientRandom, 0);
		mD5SHA1CryptoServiceProvider.TransformBlock(m_ServerTime, 0, m_ServerTime.Length, m_ServerTime, 0);
		mD5SHA1CryptoServiceProvider.TransformBlock(m_ServerRandom, 0, m_ServerRandom.Length, m_ServerRandom, 0);
		mD5SHA1CryptoServiceProvider.TransformFinalBlock(message.fragment, 0, num2);
		num = message.fragment[num2] * 256 + message.fragment[num2 + 1];
		byte[] array = new byte[num];
		Array.Copy(message.fragment, num2 + 2, array, 0, num);
		if (!mD5SHA1CryptoServiceProvider.VerifySignature(m_RemoteCertificate, array))
		{
			throw new SslException(AlertDescription.HandshakeFailure, "The data was not signed by the server certificate.");
		}
		mD5SHA1CryptoServiceProvider.Clear();
		return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
	}

	private byte[] RemoveLeadingZeros(byte[] input)
	{
		int num = 0;
		for (int i = 0; i < input.Length && input[i] == 0; i++)
		{
			num++;
		}
		if (num == 0)
		{
			return input;
		}
		byte[] array = new byte[input.Length - num];
		Array.Copy(input, num, array, 0, array.Length);
		return array;
	}

	protected SslHandshakeStatus ProcessCertificateRequest(HandshakeMessage message)
	{
		if (m_State == HandshakeType.ServerKeyExchange)
		{
			CipherDefinition cipherDefinition = CipherSuites.GetCipherDefinition(m_EncryptionScheme);
			if (m_RemoteCertificate.GetPublicKeyLength() <= 512 || !cipherDefinition.Exportable)
			{
				throw new SslException(AlertDescription.HandshakeFailure, "Invalid message.");
			}
		}
		else if (m_State != HandshakeType.Certificate)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "CertificateRequest message must be preceded by a Certificate or ServerKeyExchange message.");
		}
		UpdateHashes(message, HashUpdate.All);
		bool flag = false;
		byte[] array = new byte[message.fragment[0]];
		Array.Copy(message.fragment, 1, array, 0, array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == 1)
			{
				flag = true;
				break;
			}
		}
		if (m_Options.RequestHandler != null && flag)
		{
			Queue queue = new Queue();
			DistinguishedNameList distinguishedNameList = new DistinguishedNameList();
			int num;
			for (int j = message.fragment[0] + 3; j < message.fragment.Length; j += num + 2)
			{
				num = message.fragment[j] * 256 + message.fragment[j + 1];
				byte[] array2 = new byte[num];
				Array.Copy(message.fragment, j + 2, array2, 0, num);
				queue.Enqueue(array2);
			}
			while (queue.Count > 0)
			{
				distinguishedNameList.Add(ProcessName((byte[])queue.Dequeue()));
			}
			RequestEventArgs e = new RequestEventArgs();
			try
			{
				m_Options.RequestHandler(base.Parent, distinguishedNameList, e);
				if (e.Certificate != null)
				{
					m_Options.Certificate = e.Certificate;
				}
			}
			catch (Exception e2)
			{
				throw new SslException(e2, AlertDescription.InternalError, "The code in the CertRequestEventHandler delegate threw an error.");
			}
		}
		if (!flag)
		{
			m_Options.Certificate = null;
		}
		m_MutualAuthentication = true;
		return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
	}

	private DistinguishedName ProcessName(byte[] buffer)
	{
		GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		try
		{
			return new DistinguishedName(gCHandle.AddrOfPinnedObject(), buffer.Length);
		}
		finally
		{
			gCHandle.Free();
		}
	}

	protected SslHandshakeStatus ProcessServerHelloDone(HandshakeMessage message)
	{
		if (m_State != HandshakeType.Certificate && m_State != HandshakeType.ServerKeyExchange && m_State != HandshakeType.CertificateRequest)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "ServerHello message must be preceded by a ClientHello message.");
		}
		if (message.fragment.Length != 0)
		{
			throw new SslException(AlertDescription.IllegalParameter, "The ServerHelloDone message is invalid.");
		}
		UpdateHashes(message, HashUpdate.All);
		MemoryStream memoryStream = new MemoryStream();
		HandshakeMessage handshakeMessage = new HandshakeMessage(HandshakeType.ClientKeyExchange, null);
		byte[] array;
		if (m_MutualAuthentication)
		{
			handshakeMessage.type = HandshakeType.Certificate;
			handshakeMessage.fragment = GetCertificateBytes(m_Options.Certificate);
			array = m_RecordLayer.EncryptBytes(handshakeMessage.ToBytes(), 0, handshakeMessage.fragment.Length + 4, ContentType.Handshake);
			memoryStream.Write(array, 0, array.Length);
			UpdateHashes(handshakeMessage, HashUpdate.All);
		}
		if (m_KeyCipher == null)
		{
			m_KeyCipher = (RSACryptoServiceProvider)m_RemoteCertificate.PublicKey;
		}
		RSAKeyTransform rSAKeyTransform = new RSAKeyTransform(m_KeyCipher);
		byte[] array2 = new byte[48];
		m_RNG.GetBytes(array2);
		ProtocolVersion maxProtocol = CompatibilityLayer.GetMaxProtocol(m_Options.Protocol);
		array2[0] = maxProtocol.major;
		array2[1] = maxProtocol.minor;
		array = rSAKeyTransform.CreateKeyExchange(array2);
		handshakeMessage.type = HandshakeType.ClientKeyExchange;
		if (GetProtocol() == SecureProtocol.Ssl3)
		{
			handshakeMessage.fragment = array;
		}
		else
		{
			handshakeMessage.fragment = new byte[array.Length + 2];
			Array.Copy(array, 0, handshakeMessage.fragment, 2, array.Length);
			handshakeMessage.fragment[0] = (byte)(array.Length / 256);
			handshakeMessage.fragment[1] = (byte)(array.Length % 256);
		}
		GenerateCiphers(array2);
		array = m_RecordLayer.EncryptBytes(handshakeMessage.ToBytes(), 0, handshakeMessage.fragment.Length + 4, ContentType.Handshake);
		memoryStream.Write(array, 0, array.Length);
		UpdateHashes(handshakeMessage, HashUpdate.All);
		m_KeyCipher.Clear();
		m_KeyCipher = null;
		if (m_MutualAuthentication && m_Options.Certificate != null)
		{
			m_CertSignHash.MasterKey = m_MasterSecret;
			m_CertSignHash.TransformFinalBlock(array, 0, 0);
			array = m_CertSignHash.CreateSignature(m_Options.Certificate);
			handshakeMessage.type = HandshakeType.CertificateVerify;
			handshakeMessage.fragment = new byte[array.Length + 2];
			handshakeMessage.fragment[0] = (byte)(array.Length / 256);
			handshakeMessage.fragment[1] = (byte)(array.Length % 256);
			Array.Copy(array, 0, handshakeMessage.fragment, 2, array.Length);
			array = m_RecordLayer.EncryptBytes(handshakeMessage.ToBytes(), 0, handshakeMessage.fragment.Length + 4, ContentType.Handshake);
			memoryStream.Write(array, 0, array.Length);
			UpdateHashes(handshakeMessage, HashUpdate.LocalRemote);
		}
		array = m_RecordLayer.EncryptBytes(new byte[1] { 1 }, 0, 1, ContentType.ChangeCipherSpec);
		memoryStream.Write(array, 0, array.Length);
		m_RecordLayer.ChangeLocalState(null, m_CipherSuite.Encryptor, m_CipherSuite.LocalHasher);
		array = GetFinishedMessage();
		UpdateHashes(array, HashUpdate.Remote);
		array = m_RecordLayer.EncryptBytes(array, 0, array.Length, ContentType.Handshake);
		memoryStream.Write(array, 0, array.Length);
		if (m_CipherSuite.Encryptor.OutputBlockSize != 1 && (m_Options.Flags & SecurityFlags.DontSendEmptyRecord) == 0)
		{
			byte[] array3 = m_RecordLayer.EncryptBytes(new byte[0], 0, 0, ContentType.ApplicationData);
			memoryStream.Write(array3, 0, array3.Length);
		}
		array = memoryStream.ToArray();
		memoryStream.Close();
		return new SslHandshakeStatus(SslStatus.ContinueNeeded, array);
	}

	protected byte[] GetCertificateBytes(Certificate certificate)
	{
		if (certificate == null)
		{
			return new byte[3];
		}
		byte[] array = certificate.ToCerBuffer();
		byte[] array2 = new byte[6 + array.Length];
		int num = array.Length + 3;
		array2[0] = (byte)(num / 65536);
		array2[1] = (byte)(num % 65536 / 256);
		array2[2] = (byte)(num % 256);
		array2[3] = (byte)(array.Length / 65536);
		array2[4] = (byte)(array.Length % 65536 / 256);
		array2[5] = (byte)(array.Length % 256);
		Array.Copy(array, 0, array2, 6, array.Length);
		return array2;
	}

	protected SslHandshakeStatus ProcessFinished(HandshakeMessage message)
	{
		if (m_State != HandshakeType.ChangeCipherSpec)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "Finished message must be preceded by a ChangeCipherSpec message.");
		}
		VerifyFinishedMessage(message.fragment);
		m_IsNegotiating = false;
		ClearHandshakeStructures();
		return new SslHandshakeStatus(SslStatus.OK, null);
	}

	protected SslHandshakeStatus ProcessHelloRequest(HandshakeMessage message)
	{
		if (IsNegotiating())
		{
			return new SslHandshakeStatus(SslStatus.OK, null);
		}
		return new SslHandshakeStatus(SslStatus.ContinueNeeded, GetClientHello());
	}

	protected override SslHandshakeStatus ProcessChangeCipherSpec(RecordMessage message)
	{
		if (message.length != 1 || message.fragment[0] != 1)
		{
			throw new SslException(AlertDescription.IllegalParameter, "The ChangeCipherSpec message was invalid.");
		}
		if (m_State == HandshakeType.ServerHelloDone)
		{
			m_RecordLayer.ChangeRemoteState(null, m_CipherSuite.Decryptor, m_CipherSuite.RemoteHasher);
			return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
		}
		throw new SslException(AlertDescription.UnexpectedMessage, "ChangeCipherSpec message must be preceded by a ServerHelloDone message.");
	}

	protected override byte[] GetRenegotiateBytes()
	{
		if (IsNegotiating())
		{
			return null;
		}
		return GetClientHello();
	}

	public override SslHandshakeStatus ProcessSsl2Hello(byte[] hello)
	{
		throw new SslException(AlertDescription.InternalError, "This is a client socket; it cannot accept a client hello messages");
	}
}
