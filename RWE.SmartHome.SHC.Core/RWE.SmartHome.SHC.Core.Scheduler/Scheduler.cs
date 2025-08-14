using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler;

public class Scheduler : Task, IScheduler, IEnumerable<ISchedulerTask>, IEnumerable
{
	private readonly object syncRoot = new object();

	private readonly Dictionary<Guid, ISchedulerTask> schedulerTasks = new Dictionary<Guid, ISchedulerTask>();

	private readonly TimeSpan executionInterval;

	private readonly ManualResetEvent signal = new ManualResetEvent(initialState: false);

	private volatile bool isRunning;

	public bool IsRunning
	{
		get
		{
			return isRunning;
		}
		private set
		{
			isRunning = value;
		}
	}

	public int Count => schedulerTasks.Count;

	public Scheduler(TimeSpan executionInterval)
	{
		base.Name = "Core.Scheduler";
		this.executionInterval = executionInterval;
	}

	public override void Start()
	{
		if (!IsRunning)
		{
			base.Start();
		}
	}

	public override void Stop()
	{
		if (IsRunning)
		{
			IsRunning = false;
			signal.Set();
		}
	}

	protected override void Run()
	{
		try
		{
			IsRunning = true;
			do
			{
				Dictionary<Guid, ISchedulerTask> source;
				lock (syncRoot)
				{
					source = new Dictionary<Guid, ISchedulerTask>(schedulerTasks);
				}
				DateTime dateTime = ShcDateTime.Now;
				foreach (KeyValuePair<Guid, ISchedulerTask> item in source.Where((KeyValuePair<Guid, ISchedulerTask> schedulerTask) => schedulerTask.Value.ShouldExecute(dateTime)))
				{
					try
					{
						item.Value.TaskAction();
					}
					catch (Exception ex)
					{
						Log.Exception(Module.Core, ex, $"[Core.Scheduler] Scheduled task failed with exception: {ex}");
					}
					if (item.Value.RunOnce)
					{
						RemoveSchedulerTask(item.Key);
					}
				}
				signal.WaitOne((int)executionInterval.TotalMilliseconds, exitContext: false);
			}
			while (IsRunning);
		}
		finally
		{
			signal.Close();
		}
	}

	public void AddSchedulerTask(ISchedulerTask schedulerTask)
	{
		lock (syncRoot)
		{
			schedulerTasks.Add(schedulerTask.TaskId, schedulerTask);
		}
	}

	public void RemoveSchedulerTask(Guid taskId)
	{
		lock (syncRoot)
		{
			schedulerTasks.Remove(taskId);
		}
	}

	public IEnumerator<ISchedulerTask> GetEnumerator()
	{
		Dictionary<Guid, ISchedulerTask>.Enumerator enumerator = schedulerTasks.GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current.Value;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
