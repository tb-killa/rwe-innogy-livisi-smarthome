using System;
using System.Threading;
using WebSocketLibrary.Common;
using WebSocketLibrary.Managers.Frames;

namespace WebSocketLibrary.Controllers;

public class PingController
{
	private static readonly TimeSpan SleepTimeIfCrashed = TimeSpan.FromMinutes(3.0);

	private static readonly TimeSpan DefaultPingInterval = TimeSpan.FromMinutes(1.0);

	private readonly ILogger logger;

	private readonly IFramesManager framesManager;

	private readonly TimeSpan pingInterval;

	private Timer timer;

	public PingController(IFramesManager framesManager, ILogger logger, TimeSpan pingInterval)
	{
		this.logger = logger;
		this.framesManager = framesManager;
		this.pingInterval = pingInterval;
	}

	public void Start()
	{
		if (timer != null)
		{
			Stop();
		}
		timer = new Timer(SendPing, null, TimeSpan.Zero, pingInterval);
	}

	public void Stop()
	{
		if (timer != null)
		{
			timer.Dispose();
			timer = null;
		}
	}

	private void SendPing(object state)
	{
		framesManager.SendPing(string.Empty);
	}
}
