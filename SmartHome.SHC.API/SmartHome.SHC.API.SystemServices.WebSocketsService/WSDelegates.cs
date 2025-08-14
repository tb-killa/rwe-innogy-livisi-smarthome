namespace SmartHome.SHC.API.SystemServices.WebSocketsService;

public static class WSDelegates
{
	public delegate void ConnectionChangedEventHandler(WebSocketState websocketState);

	public delegate void TextMessageReceivedEventHandler(string textMessage);

	public delegate void DataMessageReceivedEventHandler(byte[] dataMessage);

	public delegate void ErrorEventHandler(string message, string stackTrace);
}
