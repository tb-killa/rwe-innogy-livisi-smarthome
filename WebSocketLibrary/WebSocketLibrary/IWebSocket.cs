using System;
using WebSocketLibrary.Common;
using WebSocketLibrary.Managers.Frames;

namespace WebSocketLibrary;

public interface IWebSocket
{
	ConnectionStatus Status { get; }

	void Connect();

	void Disconnect();

	void SendText(string message);

	void SendData(ArraySegment<byte> data);

	ReceivedResult ReceiveData(ArraySegment<byte> data);

	bool IsConnected();
}
