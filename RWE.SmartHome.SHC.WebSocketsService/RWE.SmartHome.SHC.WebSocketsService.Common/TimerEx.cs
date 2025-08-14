using System;
using System.Threading;

namespace RWE.SmartHome.SHC.WebSocketsService.Common;

public class TimerEx : IDisposable
{
	private object syncObject;

	private bool timedOut;

	private int timeoutTime;

	private object state;

	private Timer timer;

	public object State
	{
		get
		{
			object obj = null;
			lock (syncObject)
			{
				return state;
			}
		}
	}

	public bool HasTimedOut
	{
		get
		{
			bool flag = false;
			lock (syncObject)
			{
				return timedOut;
			}
		}
	}

	public TimerEx()
	{
		syncObject = new object();
		timedOut = false;
		timeoutTime = -1;
		state = null;
		timer = null;
	}

	public void Dispose()
	{
	}

	public void Start(int timeoutTime, object state)
	{
		lock (syncObject)
		{
			if (timer == null)
			{
				timedOut = false;
				this.timeoutTime = timeoutTime * 1000;
				this.state = state;
				timer = new Timer(timerCallback, this.state, this.timeoutTime, -1);
			}
		}
	}

	public void Restart(object state)
	{
		lock (syncObject)
		{
			if (timer != null)
			{
				timer.Dispose();
				timer = null;
			}
			timedOut = false;
			this.state = state;
			timer = new Timer(timerCallback, this.state, timeoutTime, -1);
		}
	}

	public void Stop()
	{
		lock (syncObject)
		{
			if (timer != null)
			{
				timer.Dispose();
				timer = null;
			}
			timedOut = false;
			timeoutTime = -1;
			state = null;
		}
	}

	private void timerCallback(object data)
	{
		lock (syncObject)
		{
			if (timer != null)
			{
				timer.Dispose();
				timer = null;
			}
			timedOut = true;
		}
	}
}
