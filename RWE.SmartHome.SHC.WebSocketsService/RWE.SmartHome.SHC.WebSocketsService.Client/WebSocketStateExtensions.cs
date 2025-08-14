namespace RWE.SmartHome.SHC.WebSocketsService.Client;

public static class WebSocketStateExtensions
{
	public static string Name(this WebSocketState webSocketState)
	{
		string result = "";
		switch (webSocketState)
		{
		case WebSocketState.Initialized:
			result = "Initialized";
			break;
		case WebSocketState.Connecting:
			result = "Connecting";
			break;
		case WebSocketState.Connected:
			result = "Connected";
			break;
		case WebSocketState.Disconnecting:
			result = "Disconnecting";
			break;
		case WebSocketState.Disconnected:
			result = "Disconnected";
			break;
		}
		return result;
	}
}
