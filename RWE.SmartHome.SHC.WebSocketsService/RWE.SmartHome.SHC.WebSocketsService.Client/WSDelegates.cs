namespace RWE.SmartHome.SHC.WebSocketsService.Client;

public static class WSDelegates
{
	public delegate void ConnectionChangedEventHandler(WebSocketState websocketState);

	public delegate void TextMessageReceivedEventHandler(string textMessage);

	public delegate void DataMessageReceivedEventHandler(byte[] dataMessage);

	public delegate void ErrorEventHandler(string message, string stackTrace);
}
