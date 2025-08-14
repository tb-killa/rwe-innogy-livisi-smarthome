using System;
using Org.Mentalis.Security.Ssl.Shared;

namespace Org.Mentalis.Security.Ssl.Ssl3;

internal class Ssl3ServerHandshakeLayer : ServerHandshakeLayer
{
	public Ssl3ServerHandshakeLayer(RecordLayer recordLayer, SecurityOptions options)
		: base(recordLayer, options)
	{
	}

	public Ssl3ServerHandshakeLayer(HandshakeLayer handshakeLayer)
		: base(handshakeLayer)
	{
	}

	public override SecureProtocol GetProtocol()
	{
		return SecureProtocol.Ssl3;
	}

	protected override byte[] GenerateMasterSecret(byte[] premaster, byte[] clientRandom, byte[] serverRandom)
	{
		return Ssl3CipherSuites.GenerateMasterSecret(premaster, clientRandom, serverRandom);
	}

	protected override byte[] GetFinishedMessage()
	{
		HandshakeMessage handshakeMessage = new HandshakeMessage(HandshakeType.Finished, new byte[36]);
		Ssl3HandshakeMac ssl3HandshakeMac = new Ssl3HandshakeMac(HashType.MD5, m_LocalMD5Hash, m_MasterSecret);
		Ssl3HandshakeMac ssl3HandshakeMac2 = new Ssl3HandshakeMac(HashType.SHA1, m_LocalSHA1Hash, m_MasterSecret);
		ssl3HandshakeMac.TransformFinalBlock(new byte[4] { 83, 82, 86, 82 }, 0, 4);
		ssl3HandshakeMac2.TransformFinalBlock(new byte[4] { 83, 82, 86, 82 }, 0, 4);
		Array.Copy(ssl3HandshakeMac.Hash, 0, handshakeMessage.fragment, 0, 16);
		Array.Copy(ssl3HandshakeMac2.Hash, 0, handshakeMessage.fragment, 16, 20);
		ssl3HandshakeMac.Clear();
		ssl3HandshakeMac2.Clear();
		return handshakeMessage.ToBytes();
	}

	protected override void VerifyFinishedMessage(byte[] peerFinished)
	{
		if (peerFinished.Length != 36)
		{
			throw new SslException(AlertDescription.IllegalParameter, "The message is invalid.");
		}
		byte[] array = new byte[36];
		Ssl3HandshakeMac ssl3HandshakeMac = new Ssl3HandshakeMac(HashType.MD5, m_RemoteMD5Hash, m_MasterSecret);
		Ssl3HandshakeMac ssl3HandshakeMac2 = new Ssl3HandshakeMac(HashType.SHA1, m_RemoteSHA1Hash, m_MasterSecret);
		ssl3HandshakeMac.TransformFinalBlock(new byte[4] { 67, 76, 78, 84 }, 0, 4);
		ssl3HandshakeMac2.TransformFinalBlock(new byte[4] { 67, 76, 78, 84 }, 0, 4);
		Array.Copy(ssl3HandshakeMac.Hash, 0, array, 0, 16);
		Array.Copy(ssl3HandshakeMac2.Hash, 0, array, 16, 20);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != peerFinished[i])
			{
				throw new SslException(AlertDescription.HandshakeFailure, "The computed hash verification does not correspond with the one of the client.");
			}
		}
		ssl3HandshakeMac.Clear();
		ssl3HandshakeMac2.Clear();
	}

	public override ProtocolVersion GetVersion()
	{
		return new ProtocolVersion(3, 0);
	}
}
