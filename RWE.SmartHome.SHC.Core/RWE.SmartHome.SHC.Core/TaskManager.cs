using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace RWE.SmartHome.SHC.Core;

public class TaskManager : ITaskManager, IService
{
	private readonly IEventManager eventManager;

	private readonly IDictionary<int, ITask> tasks;

	private readonly ManualResetEvent shutdownSignal;

	private SubscriptionToken subscriptionToken;

	public TaskManager(IEventManager eventManager)
	{
		this.eventManager = eventManager;
		tasks = new Dictionary<int, ITask>();
		shutdownSignal = new ManualResetEvent(initialState: false);
	}

	public void Initialize()
	{
		if (subscriptionToken == null)
		{
			ShutdownEvent shutdownEvent = eventManager.GetEvent<ShutdownEvent>();
			subscriptionToken = shutdownEvent.Subscribe(Shutdown, null, ThreadOption.PublisherThread, null);
		}
	}

	public void Uninitialize()
	{
		if (subscriptionToken != null)
		{
			ShutdownEvent shutdownEvent = eventManager.GetEvent<ShutdownEvent>();
			shutdownEvent.Unsubscribe(subscriptionToken);
			subscriptionToken = null;
		}
	}

	public void Register(ITask task)
	{
		if (!tasks.ContainsKey(task.ManagedThreadId))
		{
			tasks.Add(task.ManagedThreadId, task);
		}
	}

	public void Startup()
	{
		try
		{
			foreach (ITask value in tasks.Values)
			{
				value.Start();
			}
			shutdownSignal.WaitOne();
		}
		finally
		{
			Uninitialize();
			shutdownSignal.Close();
		}
	}

	private void Shutdown(ShutdownEventArgs args)
	{
		foreach (ITask item in tasks.Values.Reverse())
		{
			item.Stop();
		}
		WaitHandle[] waitHandles = tasks.Values.Select((ITask c) => c.WaitHandle).ToArray();
		WaitAll(waitHandles, args.TimeoutMilliseconds);
		shutdownSignal.Set();
	}

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	private static extern int WaitForMultipleObjects(int count, IntPtr[] handle, bool waitAll, int milliseconds);

	private static int WaitAll(IList<WaitHandle> waitHandles, int milliseconds)
	{
		if (waitHandles == null || waitHandles.Count == 0)
		{
			return -2;
		}
		IntPtr[] array = new IntPtr[waitHandles.Count];
		for (int i = 0; i < waitHandles.Count; i++)
		{
			ref IntPtr reference = ref array[i];
			reference = waitHandles[i].Handle;
		}
		return WaitForMultipleObjects(array.Length, array, waitAll: true, milliseconds);
	}
}
