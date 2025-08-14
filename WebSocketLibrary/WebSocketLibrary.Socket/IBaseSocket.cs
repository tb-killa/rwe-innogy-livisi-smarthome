namespace WebSocketLibrary.Socket;

public interface IBaseSocket
{
	void Connect(string url);

	void Disconnect();

	int SendBytes(byte[] data, int offset, int size);

	int ReceiveBytes(byte[] data, int offset, int size);

	bool IsConnected();
}
