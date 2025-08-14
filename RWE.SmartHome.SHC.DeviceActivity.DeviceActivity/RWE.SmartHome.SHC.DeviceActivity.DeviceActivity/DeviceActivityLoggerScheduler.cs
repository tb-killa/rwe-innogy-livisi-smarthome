using System;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceActivity.DeviceActivity.Common;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity;

public class DeviceActivityLoggerScheduler
{
	private static readonly TimeSpan DelaySuccess = TimeSpan.FromMinutes(3.0);

	private static readonly TimeSpan DelayMinFail = TimeSpan.FromMinutes(4.0);

	private static readonly TimeSpan DelayMaxFail = TimeSpan.FromMinutes(10.0);

	private readonly Random random;

	private readonly ManualResetEvent semaphore;

	private readonly Func<bool> flushDalCallback;

	private readonly object sync = new object();

	private WorkingThread thread;

	public DeviceActivityLoggerScheduler(Func<bool> flushDalCallback)
	{
		if (flushDalCallback == null)
		{
			throw new ArgumentException("Flush DAL callback is null");
		}
		this.flushDalCallback = flushDalCallback;
		random = new Random();
		semaphore = new ManualResetEvent(initialState: false);
	}

	public void StartScheduler()
	{
		semaphore.Set();
		lock (sync)
		{
			if (thread == null)
			{
				Log.Information(Module.DeviceActivity, "Started DAL flushing thread");
				thread = new WorkingThread(RunThreadBackgroundCycle);
			}
		}
	}

	public void StopScheduler()
	{
		lock (sync)
		{
			if (thread != null)
			{
				thread.StopThread();
				thread = null;
				Log.Information(Module.DeviceActivity, "Stopped DAL flushing thread");
			}
		}
		semaphore.Set();
	}

	public void ForceFlush()
	{
		semaphore.Set();
		Log.Information(Module.DeviceActivity, "Send DAL data now");
	}

	private void RunThreadBackgroundCycle()
	{
		bool flag = false;
		semaphore.Reset();
		try
		{
			flag = flushDalCallback();
		}
		catch
		{
			flag = false;
		}
		TimeSpan timeSpan = (flag ? GetSuccessDelay() : GetFailDelay());
		if (!flag)
		{
			Log.Debug(Module.DeviceActivity, $"DAL was not flushed. Next flush will be after: {timeSpan}");
		}
		semaphore.WaitOne((int)timeSpan.TotalMilliseconds, exitContext: false);
	}

	private TimeSpan GetSuccessDelay()
	{
		return DelaySuccess;
	}

	private TimeSpan GetFailDelay()
	{
		int minValue = (int)DelayMinFail.TotalSeconds;
		int num = (int)DelayMaxFail.TotalSeconds;
		int num2 = random.Next(minValue, num + 1);
		return TimeSpan.FromSeconds(num2);
	}
}
