using System;
using WebSocketLibrary.Common;

namespace WebSocketLibrary.Managers.Frames;

public interface IFramesManager
{
	void Start();

	void Stop();

	void SendData(ArraySegment<byte> data);

	void SendText(string text);

	void SendCloseConnection(CloseFrameStatusCode code, string text);

	void SendPing(string text);

	ReceivedResult ReceiveData(ArraySegment<byte> data);
}
