namespace WebSocketLibrary.Socket;

public interface ISenderSocket
{
	void Send(byte[] data);

	void Send(byte[] data, uint mask);

	void Send(byte[] data, int offset, int size);

	void Send(byte[] data, int offset, int size, uint mask);
}
