using System;
using System.Threading;
using WebSocketLibrary.Common;
using WebSocketLibrary.Controllers;
using WebSocketLibrary.Exceptions;
using WebSocketLibrary.Frames;
using WebSocketLibrary.Handlers.Frames;
using WebSocketLibrary.Helpers;

namespace WebSocketLibrary.Managers.Frames;

public class ReceiverFramesManager
{
	private static readonly TimeSpan SleepFailedReceiving = TimeSpan.FromSeconds(5.0);

	private readonly ILogger logger;

	private readonly IFrameHandler frameHandler;

	private readonly ConnectionState connectionState;

	private readonly SenderFramesManager senderFramesManager;

	private readonly ReceiverBuffersManager buffersManager;

	private readonly PingPongWatcher pingPongWatcher;

	private readonly byte[] controlFrameBuffer = new byte[125];

	private readonly object sync = new object();

	private readonly ManualResetEvent semaphore = new ManualResetEvent(initialState: false);

	private bool isRunning = true;

	public ReceiverFramesManager(IFrameHandler frameHandler, SenderFramesManager senderFramesManager, ConnectionState connectionState, PingPongWatcher pingPongWatcher, ILogger logger)
	{
		this.logger = logger;
		this.frameHandler = frameHandler;
		this.pingPongWatcher = pingPongWatcher;
		this.connectionState = connectionState;
		this.senderFramesManager = senderFramesManager;
		buffersManager = new ReceiverBuffersManager();
	}

	public void StartReceiving()
	{
		Thread thread = new Thread(ReceiveFramesThread);
		thread.Start();
	}

	public void StopReceiving()
	{
		isRunning = false;
		buffersManager.StopWaiting();
		semaphore.Set();
	}

	public ReceivedResult ReceiveData(ArraySegment<byte> data)
	{
		lock (sync)
		{
			buffersManager.AddWaitingBuffer(new ReceiverBuffer
			{
				Data = data
			});
			ReceiverBuffer readyBuffer = buffersManager.GetReadyBuffer();
			if (readyBuffer == null)
			{
				throw new ConnectionFailedException("Connection was already closed");
			}
			ReceivedResult receivedResult = new ReceivedResult();
			receivedResult.IsFinalData = readyBuffer.IsAllBytesRead && readyBuffer.IsFinalFrame;
			receivedResult.Type = readyBuffer.Type;
			receivedResult.ReadBytes = readyBuffer.ReadBytes;
			return receivedResult;
		}
	}

	private void ReceiveFramesThread()
	{
		while (isRunning)
		{
			try
			{
				FrameIdentifier frameIdentifier = frameHandler.ReceiveNextFrameIdentifier();
				ProccessFrameIdentifier(frameIdentifier);
			}
			catch (ConnectionFailedException ex)
			{
				logger.Error("Connection failed: {0}", ex.Message);
				connectionState.CloseConnection();
			}
			catch (Exception ex2)
			{
				logger.Error("Failed receiving messages, sleep for {0}, error: {1}", SleepFailedReceiving, ex2.Message);
				semaphore.WaitOne((int)SleepFailedReceiving.TotalMilliseconds, exitContext: false);
			}
		}
	}

	private void ProccessFrameIdentifier(FrameIdentifier frameIdentifier)
	{
		logger.Info("Received message type: {0}", frameIdentifier.Type);
		switch (frameIdentifier.Type)
		{
		case FrameType.Text:
		case FrameType.Binary:
			ProcessDataFramePayload(frameIdentifier);
			break;
		case FrameType.Close:
		{
			int count2 = ReceiveControlFramePayload(frameIdentifier, controlFrameBuffer);
			ArraySegment<byte> data2 = new ArraySegment<byte>(controlFrameBuffer, 0, count2);
			CloseFrameHelper.GetCodeFromPayload(data2);
			CloseFrameHelper.GetTextFromPayload(data2);
			connectionState.ServerClosedConnection();
			break;
		}
		case FrameType.Ping:
		{
			int count = ReceiveControlFramePayload(frameIdentifier, controlFrameBuffer);
			ArraySegment<byte> data = new ArraySegment<byte>(controlFrameBuffer, 0, count);
			senderFramesManager.SendPong(data);
			break;
		}
		case FrameType.Pong:
			ReceiveControlFramePayload(frameIdentifier, controlFrameBuffer);
			pingPongWatcher.ReceivedPong();
			break;
		}
	}

	private void ProcessDataFramePayload(FrameIdentifier frameIdentifier)
	{
		while (frameIdentifier.RemainedBytesToProcess != 0 && connectionState.IsConnectionOpened())
		{
			ReceiverBuffer waitingBuffer = buffersManager.GetWaitingBuffer();
			if (waitingBuffer != null)
			{
				int readBytes = frameHandler.ReceivePayload(frameIdentifier, waitingBuffer.Data);
				waitingBuffer.IsAllBytesRead = frameIdentifier.RemainedBytesToProcess == 0;
				waitingBuffer.IsFinalFrame = frameIdentifier.Header.IsFinal;
				waitingBuffer.Type = GetResultDataType(frameIdentifier.Type);
				waitingBuffer.ReadBytes = readBytes;
				buffersManager.AddReadyBuffer(waitingBuffer);
			}
		}
	}

	private ResultDataType GetResultDataType(FrameType frameType)
	{
		return frameType switch
		{
			FrameType.Binary => ResultDataType.Binary, 
			FrameType.Text => ResultDataType.Text, 
			_ => throw new ArgumentException($"Cannot convert FrameType to ResultDataType : {frameType}"), 
		};
	}

	private int ReceiveControlFramePayload(FrameIdentifier frameIdentifier, byte[] buffer)
	{
		ArraySegment<byte> data = new ArraySegment<byte>(buffer, 0, buffer.Length);
		int result = frameHandler.ReceivePayload(frameIdentifier, data);
		if (frameIdentifier.RemainedBytesToProcess != 0)
		{
			throw new FrameSizeException($"Received {frameIdentifier.Type} frame with bigger payload than allowed, size: {frameIdentifier.PayloadLength}");
		}
		return result;
	}
}
