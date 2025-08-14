using WebSocketLibrary.Common.Cookies;
using WebSocketLibrary.Handlers.Handshake;

namespace WebSocketLibrary.Managers;

public interface IHandshakeManager
{
	ReceivedHandshake NegotiateHandshake(string url, CookiesCollection cookies);
}
