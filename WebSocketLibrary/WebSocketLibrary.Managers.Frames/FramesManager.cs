using System;
using WebSocketLibrary.Common;
using WebSocketLibrary.Controllers;
using WebSocketLibrary.Handlers.Frames;

namespace WebSocketLibrary.Managers.Frames;

public class FramesManager : IFramesManager
{
	private readonly ILogger logger;

	private readonly IFrameHandler frameHandler;

	private readonly ConnectionState connectionState;

	private readonly SenderFramesManager senderFrames;

	private readonly ReceiverFramesManager receiverFrames;

	private readonly PingPongWatcher pingPongWatcher;

	private readonly bool isServerSide;

	public FramesManager(IFrameHandler frameHandler, ConnectionState connectionState, bool isServerSide, ILogger logger)
	{
		this.isServerSide = isServerSide;
		this.logger = logger;
		this.frameHandler = frameHandler;
		this.connectionState = connectionState;
		pingPongWatcher = new PingPongWatcher(connectionState.ClosingConnection, logger);
		senderFrames = new SenderFramesManager(frameHandler, pingPongWatcher, isServerSide);
		receiverFrames = new ReceiverFramesManager(frameHandler, senderFrames, connectionState, pingPongWatcher, logger);
	}

	public void Start()
	{
		receiverFrames.StartReceiving();
	}

	public void Stop()
	{
		receiverFrames.StopReceiving();
	}

	public void SendData(ArraySegment<byte> data)
	{
		if (connectionState.IsConnectionOpened())
		{
			senderFrames.SendData(data);
		}
		else
		{
			logger.Info("FrameManager.SendData -> Connection is not opened");
		}
	}

	public void SendText(string text)
	{
		if (connectionState.IsConnectionOpened())
		{
			senderFrames.SendText(text);
		}
		else
		{
			logger.Info("FrameManager.SendText -> Connection is not opened");
		}
	}

	public void SendCloseConnection(CloseFrameStatusCode code, string text)
	{
		if (connectionState.IsConnectionOpened())
		{
			senderFrames.SendCloseConnection(code, text);
		}
		else
		{
			logger.Info("FrameManager.SendCloseConnection -> Connection is not opened");
		}
	}

	public void SendPing(string text)
	{
		if (connectionState.IsConnectionOpened())
		{
			senderFrames.SendPing(text);
		}
		else
		{
			logger.Info("FrameManager.SendPing -> Connection is not opened");
		}
	}

	public ReceivedResult ReceiveData(ArraySegment<byte> data)
	{
		return receiverFrames.ReceiveData(data);
	}
}
