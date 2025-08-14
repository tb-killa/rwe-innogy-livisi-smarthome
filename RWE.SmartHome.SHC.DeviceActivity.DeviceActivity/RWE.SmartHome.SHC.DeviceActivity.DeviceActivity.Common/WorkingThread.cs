using System;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity.Common;

public class WorkingThread
{
	private readonly Action infiniteCycleCallback;

	private readonly object sync = new object();

	private bool isRunning;

	public WorkingThread(Action infiniteCycleCallback)
	{
		this.infiniteCycleCallback = infiniteCycleCallback;
		StartThread();
	}

	public void StopThread()
	{
		lock (sync)
		{
			isRunning = false;
		}
	}

	private void StartThread()
	{
		lock (sync)
		{
			if (!isRunning)
			{
				isRunning = true;
				ThreadPool.QueueUserWorkItem(delegate
				{
					RunningThreadBackgroud();
				});
			}
		}
	}

	private void RunningThreadBackgroud()
	{
		Log.Debug(Module.DeviceActivity, "Started working thread");
		while (isRunning)
		{
			infiniteCycleCallback();
		}
		Log.Debug(Module.DeviceActivity, "Stopped working thread");
	}
}
