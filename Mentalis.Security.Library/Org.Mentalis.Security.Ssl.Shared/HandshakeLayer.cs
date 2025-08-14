using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl.Shared;

internal abstract class HandshakeLayer : IDisposable
{
	protected MD5SHA1CryptoServiceProvider m_CertSignHash;

	protected CipherSuite m_CipherSuite;

	protected byte[] m_ClientRandom;

	protected byte[] m_ClientTime;

	protected SslAlgorithms m_CompressionMethod;

	protected bool m_Disposed;

	protected SslAlgorithms m_EncryptionScheme;

	protected byte[] m_IncompleteMessage;

	protected bool m_IsNegotiating;

	protected RSACryptoServiceProvider m_KeyCipher;

	protected MD5 m_LocalMD5Hash;

	protected SHA1 m_LocalSHA1Hash;

	protected byte[] m_MasterSecret;

	protected bool m_MutualAuthentication;

	protected SecurityOptions m_Options;

	protected RecordLayer m_RecordLayer;

	protected Certificate m_RemoteCertificate;

	protected MD5 m_RemoteMD5Hash;

	protected SHA1 m_RemoteSHA1Hash;

	protected RNGCryptoServiceProvider m_RNG;

	protected byte[] m_ServerRandom;

	protected byte[] m_ServerTime;

	protected byte[] m_SessionID;

	protected HandshakeType m_State;

	public SecureSocket Parent => m_RecordLayer.Parent;

	public RNGCryptoServiceProvider RNG => m_RNG;

	public SslAlgorithms ActiveEncryption => m_EncryptionScheme;

	public Certificate RemoteCertificate => m_RemoteCertificate;

	internal RecordLayer RecordLayer
	{
		get
		{
			return m_RecordLayer;
		}
		set
		{
			m_RecordLayer = value;
		}
	}

	public HandshakeLayer(RecordLayer recordLayer, SecurityOptions options)
	{
		m_Disposed = false;
		m_Options = options;
		m_IsNegotiating = true;
		m_RNG = new RNGCryptoServiceProvider();
		m_RecordLayer = recordLayer;
		m_State = HandshakeType.Nothing;
		m_IncompleteMessage = new byte[0];
		m_LocalMD5Hash = new MD5CryptoServiceProvider();
		m_LocalSHA1Hash = new SHA1CryptoServiceProvider();
		m_RemoteMD5Hash = new MD5CryptoServiceProvider();
		m_RemoteSHA1Hash = new SHA1CryptoServiceProvider();
		m_CertSignHash = new MD5SHA1CryptoServiceProvider();
		m_CertSignHash.Protocol = GetProtocol();
		if (options.Entity == ConnectionEnd.Server && (options.Flags & SecurityFlags.MutualAuthentication) != SecurityFlags.Default)
		{
			m_MutualAuthentication = true;
		}
		else
		{
			m_MutualAuthentication = false;
		}
	}

	public HandshakeLayer(HandshakeLayer handshakeLayer)
	{
		m_Disposed = false;
		m_RecordLayer = handshakeLayer.m_RecordLayer;
		m_Options = handshakeLayer.m_Options;
		m_IsNegotiating = handshakeLayer.m_IsNegotiating;
		m_RNG = handshakeLayer.m_RNG;
		m_State = handshakeLayer.m_State;
		m_IncompleteMessage = handshakeLayer.m_IncompleteMessage;
		m_LocalMD5Hash = handshakeLayer.m_LocalMD5Hash;
		m_LocalSHA1Hash = handshakeLayer.m_LocalSHA1Hash;
		m_RemoteMD5Hash = handshakeLayer.m_RemoteMD5Hash;
		m_RemoteSHA1Hash = handshakeLayer.m_RemoteSHA1Hash;
		m_CertSignHash = handshakeLayer.m_CertSignHash;
		m_CertSignHash.Protocol = GetProtocol();
		m_MutualAuthentication = handshakeLayer.m_MutualAuthentication;
		m_ClientTime = handshakeLayer.m_ClientTime;
		m_ClientRandom = handshakeLayer.m_ClientRandom;
		handshakeLayer.Dispose(clear: false);
	}

	public void Dispose()
	{
		Dispose(clear: true);
	}

	public SslHandshakeStatus ProcessMessages(RecordMessage message)
	{
		if (message == null)
		{
			throw new ArgumentNullException();
		}
		SslHandshakeStatus result;
		if (message.contentType == ContentType.ChangeCipherSpec)
		{
			result = ProcessChangeCipherSpec(message);
			m_State = HandshakeType.ChangeCipherSpec;
		}
		else if (message.contentType == ContentType.Handshake)
		{
			result = default(SslHandshakeStatus);
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[m_IncompleteMessage.Length + message.length];
			Array.Copy(m_IncompleteMessage, 0, array, 0, m_IncompleteMessage.Length);
			Array.Copy(message.fragment, 0, array, m_IncompleteMessage.Length, message.length);
			int num = 0;
			for (HandshakeMessage handshakeMessage = GetHandshakeMessage(array, num); handshakeMessage != null; handshakeMessage = GetHandshakeMessage(array, num))
			{
				num += handshakeMessage.fragment.Length + 4;
				SslHandshakeStatus sslHandshakeStatus = ProcessMessage(handshakeMessage);
				if (sslHandshakeStatus.Message != null)
				{
					memoryStream.Write(sslHandshakeStatus.Message, 0, sslHandshakeStatus.Message.Length);
				}
				result.Status = sslHandshakeStatus.Status;
				m_State = handshakeMessage.type;
			}
			if (num > 0)
			{
				m_IncompleteMessage = new byte[array.Length - num];
				Array.Copy(array, num, m_IncompleteMessage, 0, m_IncompleteMessage.Length);
			}
			else
			{
				m_IncompleteMessage = array;
			}
			if (memoryStream.Length > 0)
			{
				result.Message = memoryStream.ToArray();
			}
			memoryStream.Close();
		}
		else
		{
			result = ProcessAlert(message);
		}
		return result;
	}

	protected SslHandshakeStatus ProcessCertificate(HandshakeMessage message, bool client)
	{
		if (client)
		{
			if (m_State != HandshakeType.ServerHello)
			{
				throw new SslException(AlertDescription.UnexpectedMessage, "Certificate message must be preceded by a ServerHello message.");
			}
		}
		else if (m_State != HandshakeType.ClientHello)
		{
			throw new SslException(AlertDescription.UnexpectedMessage, "Certificate message must be preceded by a ClientHello message.");
		}
		UpdateHashes(message, HashUpdate.All);
		Certificate[] array = null;
		try
		{
			array = ParseCertificateList(message.fragment);
			if (array.Length == 0 && !m_MutualAuthentication)
			{
				return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
			}
		}
		catch (SslException ex)
		{
			throw ex;
		}
		catch (Exception e)
		{
			throw new SslException(e, AlertDescription.InternalError, "The Certificate message is invalid.");
		}
		CertificateChain chain = null;
		m_RemoteCertificate = null;
		if (array.Length != 0)
		{
			m_RemoteCertificate = array[0];
			if (m_RemoteCertificate.GetPublicKeyLength() < 512)
			{
				throw new SslException(AlertDescription.HandshakeFailure, "The pulic key should be at least 512 bits.");
			}
			CertificateStore certificateStore = new CertificateStore(array);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Store = certificateStore;
			}
			chain = new CertificateChain(m_RemoteCertificate, certificateStore);
		}
		VerifyChain(chain, client);
		return new SslHandshakeStatus(SslStatus.MessageIncomplete, null);
	}

	protected void VerifyChain(CertificateChain chain, bool client)
	{
		VerifyEventArgs e = new VerifyEventArgs();
		switch (m_Options.VerificationType)
		{
		case CredentialVerification.Manual:
			try
			{
				m_Options.Verifier(Parent, m_RemoteCertificate, chain, e);
			}
			catch (Exception e2)
			{
				throw new SslException(e2, AlertDescription.InternalError, "The code inside the CertVerifyEventHandler delegate threw an exception.");
			}
			break;
		case CredentialVerification.Auto:
			if (chain != null)
			{
				e.Valid = chain.VerifyChain(m_Options.CommonName, client ? AuthType.Client : AuthType.Server) == CertificateStatus.ValidCertificate;
			}
			else
			{
				e.Valid = false;
			}
			break;
		case CredentialVerification.AutoWithoutCName:
			if (chain != null)
			{
				e.Valid = chain.VerifyChain(m_Options.CommonName, client ? AuthType.Client : AuthType.Server, VerificationFlags.IgnoreInvalidName) == CertificateStatus.ValidCertificate;
			}
			else
			{
				e.Valid = false;
			}
			break;
		default:
			e.Valid = true;
			break;
		}
		if (!e.Valid)
		{
			throw new SslException(AlertDescription.CertificateUnknown, "The certificate could not be verified.");
		}
	}

	protected Certificate[] ParseCertificateList(byte[] list)
	{
		Queue queue = new Queue();
		int num;
		int i;
		for (i = 3; i < list.Length; i += num + 3)
		{
			num = list[i] * 65536 + list[i + 1] * 256 + list[i + 2];
			queue.Enqueue(Certificate.CreateFromCerFile(list, i + 3, num));
		}
		Certificate[] array = new Certificate[queue.Count];
		i = 0;
		while (queue.Count > 0)
		{
			array[i] = (Certificate)queue.Dequeue();
			i++;
		}
		return array;
	}

	protected SslHandshakeStatus ProcessAlert(RecordMessage message)
	{
		if (message.length != 2 || message.fragment.Length != 2)
		{
			throw new SslException(AlertDescription.RecordOverflow, "The alert message is invalid.");
		}
		try
		{
			AlertLevel alertLevel = (AlertLevel)message.fragment[0];
			AlertDescription alertDescription = (AlertDescription)message.fragment[1];
			if (alertLevel == AlertLevel.Fatal)
			{
				throw new SslException(alertDescription, "The other side has sent a failure alert.");
			}
			return (alertDescription != AlertDescription.CloseNotify) ? new SslHandshakeStatus(SslStatus.OK, null) : ((m_State != HandshakeType.ShuttingDown) ? new SslHandshakeStatus(SslStatus.Close, GetControlBytes(ControlType.Shutdown)) : new SslHandshakeStatus(SslStatus.Close, null));
		}
		catch (SslException ex)
		{
			throw ex;
		}
		catch (Exception e)
		{
			throw new SslException(e, AlertDescription.InternalError, "There was an internal error.");
		}
	}

	protected HandshakeMessage GetHandshakeMessage(byte[] buffer, int offset)
	{
		if (buffer.Length < offset + 4)
		{
			return null;
		}
		int num = buffer[offset + 1] * 65536 + buffer[offset + 2] * 256 + buffer[offset + 3];
		if (buffer.Length < offset + 4 + num)
		{
			return null;
		}
		byte[] array = new byte[num];
		Array.Copy(buffer, offset + 4, array, 0, num);
		return new HandshakeMessage((HandshakeType)buffer[offset], array);
	}

	protected byte[] GetUnixTime()
	{
		byte[] bytes = BitConverter.GetBytes((uint)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	public bool IsNegotiating()
	{
		return m_IsNegotiating;
	}

	protected void GenerateCiphers(byte[] premaster)
	{
		byte[] array = new byte[32];
		byte[] array2 = new byte[32];
		Array.Copy(m_ClientTime, 0, array, 0, 4);
		Array.Copy(m_ClientRandom, 0, array, 4, 28);
		Array.Copy(m_ServerTime, 0, array2, 0, 4);
		Array.Copy(m_ServerRandom, 0, array2, 4, 28);
		m_MasterSecret = GenerateMasterSecret(premaster, array, array2);
		m_CipherSuite = CipherSuites.GetCipherSuite(GetProtocol(), m_MasterSecret, array, array2, m_EncryptionScheme, m_Options.Entity);
		Array.Clear(premaster, 0, premaster.Length);
	}

	protected void UpdateHashes(HandshakeMessage message, HashUpdate update)
	{
		UpdateHashes(new byte[4]
		{
			(byte)message.type,
			(byte)(message.fragment.Length / 65536),
			(byte)(message.fragment.Length % 65536 / 256),
			(byte)(message.fragment.Length % 256)
		}, update);
		UpdateHashes(message.fragment, update);
	}

	protected void UpdateHashes(byte[] buffer, HashUpdate update)
	{
		if (update == HashUpdate.All || update == HashUpdate.Local || update == HashUpdate.LocalRemote)
		{
			m_LocalMD5Hash.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
			m_LocalSHA1Hash.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
		}
		if (update == HashUpdate.All || update == HashUpdate.Remote || update == HashUpdate.LocalRemote)
		{
			m_RemoteMD5Hash.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
			m_RemoteSHA1Hash.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
		}
		if (update == HashUpdate.All)
		{
			m_CertSignHash.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
		}
	}

	public byte[] GetControlBytes(ControlType type)
	{
		switch (type)
		{
		case ControlType.Shutdown:
			m_IsNegotiating = true;
			m_State = HandshakeType.ShuttingDown;
			return m_RecordLayer.EncryptBytes(new byte[2] { 1, 0 }, 0, 2, ContentType.Alert);
		case ControlType.Renegotiate:
			return GetRenegotiateBytes();
		case ControlType.ClientHello:
			return GetClientHello();
		default:
			throw new NotSupportedException("The selected ControlType field is not supported.");
		}
	}

	protected void ClearHandshakeStructures()
	{
		try
		{
			m_LocalMD5Hash.Initialize();
			m_LocalSHA1Hash.Initialize();
			m_RemoteMD5Hash.Initialize();
			m_RemoteSHA1Hash.Initialize();
			m_CertSignHash.Initialize();
			if (m_ClientTime != null)
			{
				Array.Clear(m_ClientTime, 0, m_ClientTime.Length);
			}
			if (m_ClientRandom != null)
			{
				Array.Clear(m_ClientRandom, 0, m_ClientRandom.Length);
			}
			if (m_ServerTime != null)
			{
				Array.Clear(m_ServerTime, 0, m_ServerTime.Length);
			}
			if (m_ServerRandom != null)
			{
				Array.Clear(m_ServerRandom, 0, m_ServerRandom.Length);
			}
			if (m_SessionID != null)
			{
				Array.Clear(m_SessionID, 0, m_SessionID.Length);
			}
			if (m_MasterSecret != null)
			{
				Array.Clear(m_MasterSecret, 0, m_MasterSecret.Length);
			}
			if (m_KeyCipher != null)
			{
				m_KeyCipher.Clear();
			}
		}
		catch
		{
		}
	}

	public void Dispose(bool clear)
	{
		if (!m_Disposed)
		{
			return;
		}
		m_Disposed = true;
		if (!clear)
		{
			return;
		}
		ClearHandshakeStructures();
		m_LocalMD5Hash.Clear();
		m_LocalSHA1Hash.Clear();
		m_RemoteMD5Hash.Clear();
		m_RemoteSHA1Hash.Clear();
		m_CertSignHash.Clear();
		if (m_IncompleteMessage != null)
		{
			Array.Clear(m_IncompleteMessage, 0, m_IncompleteMessage.Length);
		}
		if (m_CipherSuite != null)
		{
			if (m_CipherSuite.Decryptor != null)
			{
				m_CipherSuite.Decryptor.Dispose();
			}
			if (m_CipherSuite.Encryptor != null)
			{
				m_CipherSuite.Encryptor.Dispose();
			}
			if (m_CipherSuite.LocalHasher != null)
			{
				m_CipherSuite.LocalHasher.Clear();
			}
			if (m_CipherSuite.RemoteHasher != null)
			{
				m_CipherSuite.RemoteHasher.Clear();
			}
		}
	}

	~HandshakeLayer()
	{
		Dispose();
	}

	public abstract SecureProtocol GetProtocol();

	public abstract ProtocolVersion GetVersion();

	public abstract SslHandshakeStatus ProcessSsl2Hello(byte[] hello);

	protected abstract SslHandshakeStatus ProcessChangeCipherSpec(RecordMessage message);

	protected abstract SslHandshakeStatus ProcessMessage(HandshakeMessage message);

	protected abstract byte[] GetClientHello();

	protected abstract byte[] GetRenegotiateBytes();

	protected abstract byte[] GenerateMasterSecret(byte[] premaster, byte[] clientRandom, byte[] serverRandom);

	protected abstract byte[] GetFinishedMessage();

	protected abstract void VerifyFinishedMessage(byte[] peerFinished);
}
