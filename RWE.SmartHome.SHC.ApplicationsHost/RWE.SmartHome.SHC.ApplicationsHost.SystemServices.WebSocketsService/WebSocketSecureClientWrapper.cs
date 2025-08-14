using RWE.SmartHome.SHC.WebSocketsService.Client;
using SmartHome.SHC.API.SystemServices.WebSocketsService;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.WebSocketsService;

public class WebSocketSecureClientWrapper : IWebSocketSecureClient
{
	private WebSocketSecureClient client;

	public string LoggerID => client.LoggerID;

	public string Origin => client.Origin;

	public string SubProtocol => client.SubProtocol;

	public string Extensions => client.Extensions;

	public string Version => client.Version;

	public global::SmartHome.SHC.API.SystemServices.WebSocketsService.WebSocketState State => client.State.ToAPIWebSocketState();

	public event global::SmartHome.SHC.API.SystemServices.WebSocketsService.WSDelegates.ConnectionChangedEventHandler ConnectionChanged;

	public event global::SmartHome.SHC.API.SystemServices.WebSocketsService.WSDelegates.TextMessageReceivedEventHandler TextMessageReceived;

	public event global::SmartHome.SHC.API.SystemServices.WebSocketsService.WSDelegates.DataMessageReceivedEventHandler DataMessageReceived;

	public event global::SmartHome.SHC.API.SystemServices.WebSocketsService.WSDelegates.ErrorEventHandler Error;

	public WebSocketSecureClientWrapper(string logSourceId, global::SmartHome.SHC.API.SystemServices.WebSocketsService.WSOptions options)
	{
		client = new WebSocketSecureClient(logSourceId, options.ToWSOptions());
		client.ConnectionChanged += OnConnectionChanged;
		client.DataMessageReceived += OnDataMessageReceived;
		client.Error += OnError;
		client.TextMessageReceived += OnTextMessageReceived;
	}

	public void Connect(string serverUrl)
	{
		client.Connect(serverUrl);
	}

	public void Connect(string serverUrl, int? port)
	{
		client.Connect(serverUrl, port);
	}

	public void Disconnect()
	{
		client.Disconnect();
	}

	public void Disconnect(ushort statusCode, string reason)
	{
		client.Disconnect(statusCode, reason);
	}

	public void SendText(string text)
	{
		client.SendText(text);
	}

	public void SendData(byte[] data)
	{
		client.SendData(data);
	}

	private void OnConnectionChanged(RWE.SmartHome.SHC.WebSocketsService.Client.WebSocketState websocketstate)
	{
		if (this.ConnectionChanged != null)
		{
			this.ConnectionChanged(websocketstate.ToAPIWebSocketState());
		}
	}

	private void OnDataMessageReceived(byte[] datamessage)
	{
		if (this.DataMessageReceived != null)
		{
			this.DataMessageReceived(datamessage);
		}
	}

	private void OnError(string message, string stacktrace)
	{
		if (this.Error != null)
		{
			this.Error(message, stacktrace);
		}
	}

	private void OnTextMessageReceived(string textmessage)
	{
		if (this.TextMessageReceived != null)
		{
			this.TextMessageReceived(textmessage);
		}
	}
}
