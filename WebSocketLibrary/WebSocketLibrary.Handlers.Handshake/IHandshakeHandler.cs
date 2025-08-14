using WebSocketLibrary.Common.Cookies;

namespace WebSocketLibrary.Handlers.Handshake;

public interface IHandshakeHandler
{
	void SendHandshake(string url, CookiesCollection cookies);

	ReceivedHandshake ReceiveHandshake();
}
