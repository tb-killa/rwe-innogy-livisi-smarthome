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

public class WebSocket : IWebSocket
{
	private readonly ILogger logger;

	private readonly ConnectionState connectionState;

	private readonly PingController pingController;

	private readonly DataController dataController;

	private readonly ConnectionController connectionController;

	private readonly BaseSocket socket;

	public ConnectionStatus Status => connectionState.Status;

	public WebSocket(string url, ICertificateRequestHandler certificateRequestHandler, CookiesCollection cookies, Action<string> informationCallback, Action<string> errorCallback)
	{
		logger = new Logger(informationCallback, errorCallback);
		connectionState = new ConnectionState();
		socket = new BaseSocket(certificateRequestHandler, logger);
		ReceiverSocket receiver = new ReceiverSocket(socket);
		SenderSocket sender = new SenderSocket(socket);
		HandshakeHandler handshakeHandler = new HandshakeHandler(receiver, sender, socket, logger);
		FrameHandler frameHandler = new FrameHandler(receiver, sender);
		HandshakeManager handshakeManager = new HandshakeManager(handshakeHandler, logger);
		FramesManager framesManager = new FramesManager(frameHandler, connectionState, isServerSide: false, logger);
		connectionController = new ConnectionController(url, cookies, connectionState, socket, handshakeManager, framesManager, logger);
		dataController = new DataController(framesManager);
		pingController = new PingController(framesManager, logger, TimeSpan.FromMinutes(1.0));
	}

	public void Connect()
	{
		connectionController.OnConnectionClosed += StopAllControllers;
		connectionState.ConnectionStarted();
		if (pingController != null)
		{
			pingController.Start();
		}
	}

	public void CloseConnection()
	{
		connectionState.ClosingConnection();
	}

	public void Disconnect()
	{
		connectionState.CloseConnection();
		connectionController.OnConnectionClosed -= StopAllControllers;
		StopAllControllers();
	}

	public void SendText(string message)
	{
		dataController.SendText(message);
	}

	public void SendData(ArraySegment<byte> data)
	{
		dataController.SendData(data);
	}

	public ReceivedResult ReceiveData(ArraySegment<byte> data)
	{
		return dataController.ReceiveData(data);
	}

	private void StopAllControllers()
	{
		connectionController.Dispose();
		if (pingController != null)
		{
			pingController.Stop();
		}
	}

	public bool IsConnected()
	{
		if (socket != null)
		{
			return socket.IsConnected();
		}
		return false;
	}
}
