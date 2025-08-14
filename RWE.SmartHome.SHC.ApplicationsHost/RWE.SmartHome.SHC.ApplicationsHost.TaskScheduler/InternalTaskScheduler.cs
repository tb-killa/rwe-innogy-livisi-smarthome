using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SmartHome.SHC.API.TaskScheduler;

namespace RWE.SmartHome.SHC.ApplicationsHost.TaskScheduler;

internal class InternalTaskScheduler : IDisposable
{
	private const int MaxParallelTaskCount = 3;

	private const string LoggingSource = "InternalTaskScheduler";

	private readonly List<SchedulerTask> schedulerTasks = new List<SchedulerTask>();

	private readonly AutoResetEvent tasksChanged = new AutoResetEvent(initialState: false);

	private readonly List<Guid> runningTasks = new List<Guid>();

	private readonly object runningTasksLock = new object();

	private readonly string appID;

	private Thread thread;

	private volatile bool run = true;

	private int starSchedThreadIndex;

	public event Action<SchedulerTask> TaskCompleted;

	public event Action<SchedulerTask> TaskExecutionFailed;

	public event Action<SchedulerTask> TaskStarting;

	public InternalTaskScheduler(string appID)
	{
		this.appID = appID;
	}

	public void AddTask(SchedulerTask newTask)
	{
		if (newTask.Action == null)
		{
			throw new ArgumentNullException("Action");
		}
		if (newTask.Recurrence == null)
		{
			throw new ArgumentNullException("Recurrence");
		}
		if (newTask.Recurrence.Interval.HasValue && newTask.Recurrence.Interval.Value == TimeSpan.Zero)
		{
			throw new ArgumentException("Invalid recurrence interval.");
		}
		lock (schedulerTasks)
		{
			schedulerTasks.Add(newTask);
			PingRunner();
		}
		Log.DebugFormat(Module.ApplicationsHost, "InternalTaskScheduler", false, "{0} added task {1}", newTask.AppId, newTask.Id);
	}

	public IEnumerable<SchedulerTask> GetTasks(string appId)
	{
		lock (schedulerTasks)
		{
			return schedulerTasks.Where((SchedulerTask st) => st.AppId == appId).ToList();
		}
	}

	public TaskInfo GetTaskInfo(string appId, Guid id)
	{
		TaskInfo result = null;
		lock (schedulerTasks)
		{
			SchedulerTask schedulerTask = schedulerTasks.FirstOrDefault((SchedulerTask x) => x.AppId == appId && x.Id == id);
			if (schedulerTask != null)
			{
				result = schedulerTask.ToTaskInfo();
			}
		}
		return result;
	}

	public bool RemoveTask(string appId, Guid id)
	{
		lock (schedulerTasks)
		{
			return 1 == schedulerTasks.RemoveAll((SchedulerTask t) => t.Id == id && t.AppId == appId);
		}
	}

	public void Dispose()
	{
		run = false;
		tasksChanged.Set();
	}

	internal void SetEnabledState(string appId, Guid id, EnabledState enabledState)
	{
		lock (schedulerTasks)
		{
			SchedulerTask schedulerTask = schedulerTasks.FirstOrDefault((SchedulerTask x) => x.AppId == appId && x.Id == id);
			if (schedulerTask != null)
			{
				schedulerTask.EnabledState = enabledState;
				if (enabledState == EnabledState.Enabled)
				{
					PingRunner();
				}
			}
		}
	}

	private void StartSchedulerThread()
	{
		starSchedThreadIndex++;
		run = true;
		thread = new Thread(Run)
		{
			IsBackground = true,
			Name = $"InternalTaskScheduler({appID}): {starSchedThreadIndex.ToString()} time(s)"
		};
		thread.Start();
	}

	private void Run()
	{
		Log.Debug(Module.ApplicationsHost, $"Start sched thread: {appID} for {Thread.CurrentThread.Name}");
		while (run)
		{
			int num = 0;
			GetNextTask(out var dueTime, out var schedulerTask);
			if (schedulerTask != null)
			{
				Log.DebugFormat(Module.ApplicationsHost, "InternalTaskScheduler", false, "Next task for {0} is {1} at {2}", appID, schedulerTask.Id, dueTime);
			}
			if (dueTime > DateTime.UtcNow)
			{
				num = (int)Math.Min(3600000.0, dueTime.Subtract(DateTime.UtcNow).TotalMilliseconds);
			}
			if (num > 0 || schedulerTask == null)
			{
				tasksChanged.Reset();
				tasksChanged.WaitOne(num, exitContext: false);
				continue;
			}
			lock (runningTasksLock)
			{
				if (runningTasks.Count > 3)
				{
					tasksChanged.WaitOne(100, exitContext: false);
					continue;
				}
				runningTasks.Add(schedulerTask.Id);
			}
			Thread thread = new Thread((ThreadStart)delegate
			{
				RunTaskInBackground(schedulerTask);
			});
			thread.IsBackground = true;
			thread.Name = schedulerTask.Id.ToString();
			Thread thread2 = thread;
			thread2.Start();
		}
		Log.Debug(Module.ApplicationsHost, $"End sched thread: {Thread.CurrentThread.Name} for {appID}");
	}

	private void RunTaskInBackground(object task)
	{
		Log.Debug(Module.ApplicationsHost, $"Starting worker thread: {Thread.CurrentThread.Name} for {appID}");
		if (!(task is SchedulerTask schedulerTask))
		{
			return;
		}
		Action<SchedulerTask> taskStarting = this.TaskStarting;
		try
		{
			schedulerTask.RunningState = RunningState.Running;
			schedulerTask.LastExecutionTime = DateTime.UtcNow;
			if (taskStarting != null)
			{
				try
				{
					taskStarting(schedulerTask);
				}
				catch
				{
				}
			}
			schedulerTask.Action();
			schedulerTask.LastError = null;
			schedulerTask.RunningState = RunningState.Stopped;
			taskStarting = this.TaskCompleted;
			if (taskStarting != null)
			{
				try
				{
					taskStarting(schedulerTask);
				}
				catch
				{
				}
			}
		}
		catch (Exception lastError)
		{
			schedulerTask.LastError = lastError;
			try
			{
				schedulerTask.RunningState = RunningState.Faulted;
				this.TaskExecutionFailed?.Invoke(schedulerTask);
			}
			catch
			{
			}
		}
		finally
		{
			lock (runningTasksLock)
			{
				runningTasks.Remove(schedulerTask.Id);
			}
			if (!schedulerTask.Recurrence.Interval.HasValue)
			{
				lock (schedulerTasks)
				{
					schedulerTasks.Remove(schedulerTask);
				}
			}
			tasksChanged.Set();
		}
		Log.Debug(Module.ApplicationsHost, $"Ending worker thread: {Thread.CurrentThread.Name} for {appID}");
	}

	private void GetNextTask(out DateTime dueTime, out SchedulerTask dueTask)
	{
		dueTime = DateTime.MaxValue;
		dueTask = null;
		List<SchedulerTask> list;
		lock (schedulerTasks)
		{
			list = new List<SchedulerTask>(schedulerTasks);
		}
		foreach (SchedulerTask item in list)
		{
			if (item.EnabledState != EnabledState.Enabled || (item.Recurrence.EndTime.HasValue && item.Recurrence.EndTime.Value < DateTime.UtcNow))
			{
				continue;
			}
			lock (runningTasksLock)
			{
				if (runningTasks.Contains(item.Id))
				{
					continue;
				}
			}
			if (item.Recurrence.StartTime > DateTime.UtcNow)
			{
				if (item.Recurrence.StartTime < dueTime)
				{
					dueTime = item.Recurrence.StartTime;
					dueTask = item;
				}
			}
			else if (!item.LastExecutionTime.HasValue)
			{
				dueTask = item;
				dueTime = DateTime.UtcNow;
			}
			else if (item.Recurrence.Interval.HasValue)
			{
				DateTime dateTime = item.LastExecutionTime.Value.Add(item.Recurrence.Interval.Value);
				if (dateTime < dueTime)
				{
					dueTime = dateTime;
					dueTask = item;
				}
			}
		}
	}

	private void PingRunner()
	{
		tasksChanged.Set();
		if (thread == null)
		{
			StartSchedulerThread();
		}
	}
}
