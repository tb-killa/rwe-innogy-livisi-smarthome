using System;
using Org.Mentalis.Security.Ssl.Shared;

namespace Org.Mentalis.Security.Ssl.Tls1;

internal class Tls1ServerHandshakeLayer : ServerHandshakeLayer
{
	public Tls1ServerHandshakeLayer(RecordLayer recordLayer, SecurityOptions options)
		: base(recordLayer, options)
	{
	}

	public Tls1ServerHandshakeLayer(HandshakeLayer handshakeLayer)
		: base(handshakeLayer)
	{
	}

	public override SecureProtocol GetProtocol()
	{
		return SecureProtocol.Tls1;
	}

	protected override byte[] GenerateMasterSecret(byte[] premaster, byte[] clientRandom, byte[] serverRandom)
	{
		return Tls1CipherSuites.GenerateMasterSecret(premaster, clientRandom, serverRandom);
	}

	protected override byte[] GetFinishedMessage()
	{
		byte[] array = new byte[36];
		m_LocalMD5Hash.TransformFinalBlock(new byte[0], 0, 0);
		m_LocalSHA1Hash.TransformFinalBlock(new byte[0], 0, 0);
		Array.Copy(m_LocalMD5Hash.Hash, 0, array, 0, 16);
		Array.Copy(m_LocalSHA1Hash.Hash, 0, array, 16, 20);
		PseudoRandomDeriveBytes pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(m_MasterSecret, "server finished", array);
		HandshakeMessage handshakeMessage = new HandshakeMessage(HandshakeType.Finished, pseudoRandomDeriveBytes.GetBytes(12));
		byte[] result = handshakeMessage.ToBytes();
		pseudoRandomDeriveBytes.Dispose();
		return result;
	}

	protected override void VerifyFinishedMessage(byte[] peerFinished)
	{
		if (peerFinished.Length != 12)
		{
			throw new SslException(AlertDescription.IllegalParameter, "The message is invalid.");
		}
		byte[] array = new byte[36];
		m_RemoteMD5Hash.TransformFinalBlock(new byte[0], 0, 0);
		m_RemoteSHA1Hash.TransformFinalBlock(new byte[0], 0, 0);
		Array.Copy(m_RemoteMD5Hash.Hash, 0, array, 0, 16);
		Array.Copy(m_RemoteSHA1Hash.Hash, 0, array, 16, 20);
		PseudoRandomDeriveBytes pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(m_MasterSecret, "client finished", array);
		byte[] bytes = pseudoRandomDeriveBytes.GetBytes(12);
		pseudoRandomDeriveBytes.Dispose();
		for (int i = 0; i < bytes.Length; i++)
		{
			if (bytes[i] != peerFinished[i])
			{
				throw new SslException(AlertDescription.HandshakeFailure, "The computed hash verification does not correspond with the one of the client.");
			}
		}
	}

	public override ProtocolVersion GetVersion()
	{
		return new ProtocolVersion(3, 1);
	}
}
