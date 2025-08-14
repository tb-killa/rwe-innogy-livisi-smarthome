using System.Net;
using System.Net.Sockets;

namespace Rebex.Net;

public interface ISocket
{
	int Timeout { get; set; }

	bool Connected { get; }

	EndPoint LocalEndPoint { get; }

	EndPoint RemoteEndPoint { get; }

	bool Poll(int microSeconds, SocketSelectMode mode);

	void Connect(EndPoint remoteEP);

	void Connect(string serverName, int serverPort);

	int Send(byte[] buffer, int offset, int count, SocketFlags socketFlags);

	int Receive(byte[] buffer, int offset, int count, SocketFlags socketFlags);

	void Shutdown(SocketShutdown how);

	void Close();
}
