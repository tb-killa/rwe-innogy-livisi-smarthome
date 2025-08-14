using WebSocketLibrary.Common;
using WebSocketLibrary.Common.Cookies;
using WebSocketLibrary.Exceptions;
using WebSocketLibrary.Handlers.Handshake;

namespace WebSocketLibrary.Managers;

public class HandshakeManager : IHandshakeManager
{
	private readonly ILogger logger;

	private readonly IHandshakeHandler handshakeHandler;

	public HandshakeManager(IHandshakeHandler handshakeHandler, ILogger logger)
	{
		this.logger = logger;
		this.handshakeHandler = handshakeHandler;
	}

	public ReceivedHandshake NegotiateHandshake(string url, CookiesCollection cookies)
	{
		handshakeHandler.SendHandshake(url, cookies);
		ReceivedHandshake receivedHandshake = handshakeHandler.ReceiveHandshake();
		if (receivedHandshake == null)
		{
			ThrowConnectionFailed("Received handshake is null");
		}
		if (receivedHandshake.StatusCode != 101)
		{
			ThrowConnectionFailed("Received handshake status code {0}", receivedHandshake.StatusCode);
		}
		return receivedHandshake;
	}

	private void ThrowConnectionFailed(string format, params object[] @params)
	{
		string message = string.Format(format, @params);
		throw new ConnectionFailedException(message);
	}
}
