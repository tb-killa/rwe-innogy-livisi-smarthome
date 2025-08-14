using System;
using WebSocketLibrary.Common;

namespace WebSocketLibrary.Managers.Frames;

public class PingPongWatcher
{
	private const int MAX_COUNTER_LIMIT = 1;

	private readonly object sync = new object();

	private readonly Action counterExceededCallback;

	private readonly ILogger logger;

	private int counter;

	public PingPongWatcher(Action counterExceededCallback, ILogger logger)
	{
		counter = 0;
		this.counterExceededCallback = counterExceededCallback;
		this.logger = logger;
	}

	public void SendPing()
	{
		lock (sync)
		{
			counter++;
			CheckCounter();
		}
	}

	public void ReceivedPong()
	{
		lock (sync)
		{
			counter--;
			CheckCounter();
		}
	}

	private void CheckCounter()
	{
		if (counter > 1 && counterExceededCallback != null)
		{
			logger.Error("PingPong counter exceede maximum limit, connection will be closed {0}", counter);
			counterExceededCallback();
		}
	}
}
