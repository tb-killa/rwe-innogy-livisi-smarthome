namespace SmartHome.SHC.API.SystemServices.WebSocketsService;

public interface IWebSocketSecureClient
{
	string LoggerID { get; }

	string Origin { get; }

	string SubProtocol { get; }

	string Extensions { get; }

	string Version { get; }

	WebSocketState State { get; }

	event WSDelegates.ConnectionChangedEventHandler ConnectionChanged;

	event WSDelegates.TextMessageReceivedEventHandler TextMessageReceived;

	event WSDelegates.DataMessageReceivedEventHandler DataMessageReceived;

	event WSDelegates.ErrorEventHandler Error;

	void Connect(string serverUrl);

	void Connect(string serverUrl, int? port);

	void Disconnect();

	void Disconnect(ushort statusCode, string reason);

	void SendText(string text);

	void SendData(byte[] data);
}
