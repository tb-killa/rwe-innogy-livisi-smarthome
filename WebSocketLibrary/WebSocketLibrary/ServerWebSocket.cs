using System;
using Rebex.Net;
using WebSocketLibrary.Common;
using WebSocketLibrary.Common.Cookies;
using WebSocketLibrary.Controllers;
using WebSocketLibrary.Handlers.Frames;
using WebSocketLibrary.Handlers.Handshake;
using WebSocketLibrary.Managers;
using WebSocketLibrary.Managers.Frames;
using WebSocketLibrary.Socket;

namespace WebSocketLibrary;

public class ServerWebSocket : IWebSocket
{
	private readonly ILogger logger;

	private readonly ConnectionState connectionState;

	private readonly IFramesManager framesManager;

	private readonly ConnectionController connectionController;

	private readonly BaseSocket socket;

	public ConnectionStatus Status => connectionState.Status;

	public ServerWebSocket(TlsSocket tlsSocket, CookiesCollection cookies, Action<string> informationCallback, Action<string> errorCallback)
	{
		logger = new Logger(informationCallback, errorCallback);
		connectionState = new ConnectionState();
		socket = new BaseSocket(tlsSocket, logger);
		ReceiverSocket receiver = new ReceiverSocket(socket);
		SenderSocket sender = new SenderSocket(socket);
		HandshakeHandler handshakeHandler = new HandshakeHandler(receiver, sender, socket, logger);
		FrameHandler frameHandler = new FrameHandler(receiver, sender);
		HandshakeManager handshakeManager = new HandshakeManager(handshakeHandler, logger);
		framesManager = new FramesManager(frameHandler, connectionState, isServerSide: true, logger);
		connectionController = new ConnectionController(cookies, connectionState, socket, handshakeManager, framesManager, logger);
	}

	public void Connect()
	{
		connectionController.OnConnectionClosed += delegate
		{
			connectionController.Dispose();
		};
		connectionState.ConnectionStarted();
	}

	public void Disconnect()
	{
		connectionState.CloseConnection();
	}

	public void SendText(string message)
	{
		framesManager.SendText(message);
	}

	public void SendData(ArraySegment<byte> data)
	{
		framesManager.SendData(data);
	}

	public ReceivedResult ReceiveData(ArraySegment<byte> data)
	{
		return framesManager.ReceiveData(data);
	}

	public bool IsConnected()
	{
		if (socket != null)
		{
			return socket.IsConnected();
		}
		return false;
	}

	public void Close()
	{
		socket.Disconnect();
	}
}
