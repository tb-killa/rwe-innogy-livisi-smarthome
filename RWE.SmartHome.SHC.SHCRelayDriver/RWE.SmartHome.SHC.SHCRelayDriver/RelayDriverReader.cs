using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SmartHome.Common.Generic.Contracts.BackendShc;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class RelayDriverReader
{
	private readonly WebSocketManager webSocketManager;

	private static readonly XmlSerializer msgsSerializer = new XmlSerializer(typeof(List<MessageReceivedInfo>));

	private readonly Queue<List<MessageReceivedInfo>> allMessages = new Queue<List<MessageReceivedInfo>>();

	private readonly ManualResetEvent receivedMessaegesEvent = new ManualResetEvent(initialState: false);

	private readonly ManualResetEvent stopEvent = new ManualResetEvent(initialState: false);

	private readonly DelayRelayDriverManager delayManager = new DelayRelayDriverManager();

	private readonly object sync = new object();

	private bool isRunning = true;

	private Exception receivedException;

	public RelayDriverReader(WebSocketManager webSocketManager)
	{
		this.webSocketManager = webSocketManager;
	}

	public List<MessageReceivedInfo> GetAllMessages(TimeSpan timeout)
	{
		receivedMessaegesEvent.Reset();
		lock (sync)
		{
			ThrowExceptionIfExists();
			if (allMessages.Count > 0)
			{
				return allMessages.Dequeue();
			}
		}
		receivedMessaegesEvent.WaitOne((int)timeout.TotalMilliseconds, exitContext: false);
		lock (sync)
		{
			ThrowExceptionIfExists();
			if (allMessages.Count > 0)
			{
				return allMessages.Dequeue();
			}
		}
		return new List<MessageReceivedInfo>();
	}

	public void StartReceiving()
	{
		isRunning = true;
		stopEvent.Reset();
		new Thread(ReadAllMessagesThread).Start();
	}

	public void StopReceivingMessages()
	{
		isRunning = false;
		stopEvent.Set();
	}

	private void ThrowExceptionIfExists()
	{
		if (receivedException != null)
		{
			Exception ex = receivedException;
			receivedException = null;
			throw ex;
		}
	}

	private void ReadAllMessagesThread()
	{
		while (isRunning)
		{
			bool isSuccess = ReadAllMessages();
			delayManager.MarkConnectionSuccess(isSuccess);
			int delaySeconds = delayManager.GetDelaySeconds();
			if (delaySeconds > 0)
			{
				Log.Information(Module.RelayDriver, $"RelayDriver will pause for {delaySeconds} seconds");
				stopEvent.WaitOne(delaySeconds * 1000, exitContext: false);
			}
		}
	}

	private bool ReadAllMessages()
	{
		try
		{
			using SocketReadStream stream = new SocketReadStream(webSocketManager);
			if (msgsSerializer.Deserialize(stream) is List<MessageReceivedInfo> item)
			{
				lock (sync)
				{
					allMessages.Enqueue(item);
					receivedMessaegesEvent.Set();
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(Module.RelayDriver, "Failed to retrieve messages", ex.ToString());
			lock (sync)
			{
				receivedException = ex;
				receivedMessaegesEvent.Set();
				webSocketManager.CloseSocket();
			}
			return false;
		}
	}
}
