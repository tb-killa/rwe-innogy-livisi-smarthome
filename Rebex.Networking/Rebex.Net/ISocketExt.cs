using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using onrkn;

namespace Rebex.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
[wptwl(false)]
public interface ISocketExt : ISocket
{
	ISocketFactory Factory { get; }

	SocketInformation Information { get; }

	int Available { get; }

	SocketState GetConnectionState();

	IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state);

	IAsyncResult BeginConnect(string serverName, int serverPort, AsyncCallback callback, object state);

	void EndConnect(IAsyncResult asyncResult);

	EndPoint Listen(ISocket controlSocket);

	ISocket Accept();

	IAsyncResult BeginSend(byte[] buffer, int offset, int count, SocketFlags socketFlags, AsyncCallback callback, object state);

	int EndSend(IAsyncResult asyncResult);

	IAsyncResult BeginReceive(byte[] buffer, int offset, int count, SocketFlags socketFlags, AsyncCallback callback, object state);

	int EndReceive(IAsyncResult asyncResult);

	IAsyncResult BeginListen(ISocket controlSocket, AsyncCallback callback, object state);

	EndPoint EndListen(IAsyncResult asyncResult);

	IAsyncResult BeginAccept(AsyncCallback callback, object state);

	ISocket EndAccept(IAsyncResult asyncResult);
}
