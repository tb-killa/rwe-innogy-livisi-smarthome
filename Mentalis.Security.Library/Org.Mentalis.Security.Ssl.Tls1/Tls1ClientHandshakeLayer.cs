using System;
using Org.Mentalis.Security.Ssl.Shared;

namespace Org.Mentalis.Security.Ssl.Tls1;

internal class Tls1ClientHandshakeLayer : ClientHandshakeLayer
{
	public Tls1ClientHandshakeLayer(RecordLayer recordLayer, SecurityOptions options)
		: base(recordLayer, options)
	{
	}

	public Tls1ClientHandshakeLayer(HandshakeLayer handshakeLayer)
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
		HandshakeMessage handshakeMessage = new HandshakeMessage(HandshakeType.Finished, null);
		byte[] array = new byte[36];
		m_LocalMD5Hash.TransformFinalBlock(new byte[0], 0, 0);
		m_LocalSHA1Hash.TransformFinalBlock(new byte[0], 0, 0);
		Array.Copy(m_LocalMD5Hash.Hash, 0, array, 0, 16);
		Array.Copy(m_LocalSHA1Hash.Hash, 0, array, 16, 20);
		PseudoRandomDeriveBytes pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(m_MasterSecret, "client finished", array);
		handshakeMessage.fragment = pseudoRandomDeriveBytes.GetBytes(12);
		pseudoRandomDeriveBytes.Dispose();
		return handshakeMessage.ToBytes();
	}

	protected override void VerifyFinishedMessage(byte[] peerFinished)
	{
		if (peerFinished.Length != 12)
		{
			throw new SslException(AlertDescription.IllegalParameter, "The ServerHelloDone message is invalid.");
		}
		byte[] array = new byte[36];
		m_RemoteMD5Hash.TransformFinalBlock(new byte[0], 0, 0);
		m_RemoteSHA1Hash.TransformFinalBlock(new byte[0], 0, 0);
		Array.Copy(m_RemoteMD5Hash.Hash, 0, array, 0, 16);
		Array.Copy(m_RemoteSHA1Hash.Hash, 0, array, 16, 20);
		PseudoRandomDeriveBytes pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(m_MasterSecret, "server finished", array);
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
