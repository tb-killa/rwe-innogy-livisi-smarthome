using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.SHC.API.TaskScheduler;

namespace RWE.SmartHome.SHC.ApplicationsHost.TaskScheduler;

internal class TaskScheduler : ITaskScheduler
{
	private readonly string appId;

	private readonly InternalTaskScheduler internalTaskScheduler;

	private EventHandler<TaskEventArgs> startingHandler;

	private EventHandler<TaskEventArgs> terminatingHandler;

	private EventHandler<TaskFailureEventArgs> errorHandler;

	public event EventHandler<TaskEventArgs> TaskCompleted
	{
		add
		{
			internalTaskScheduler.TaskCompleted += OnTaskTerminated;
			terminatingHandler = (EventHandler<TaskEventArgs>)Delegate.Combine(terminatingHandler, value);
		}
		remove
		{
			internalTaskScheduler.TaskCompleted -= OnTaskTerminated;
			if (terminatingHandler != null)
			{
				terminatingHandler = (EventHandler<TaskEventArgs>)Delegate.Remove(terminatingHandler, value);
			}
		}
	}

	public event EventHandler<TaskFailureEventArgs> TaskFailed
	{
		add
		{
			internalTaskScheduler.TaskExecutionFailed += OnTaskError;
			errorHandler = (EventHandler<TaskFailureEventArgs>)Delegate.Combine(errorHandler, value);
		}
		remove
		{
			internalTaskScheduler.TaskExecutionFailed -= OnTaskError;
			if (errorHandler != null)
			{
				errorHandler = (EventHandler<TaskFailureEventArgs>)Delegate.Remove(errorHandler, value);
			}
		}
	}

	public event EventHandler<TaskEventArgs> TaskStarting
	{
		add
		{
			internalTaskScheduler.TaskStarting += OnTaskStarting;
			startingHandler = (EventHandler<TaskEventArgs>)Delegate.Combine(startingHandler, value);
		}
		remove
		{
			internalTaskScheduler.TaskStarting -= OnTaskStarting;
			if (startingHandler != null)
			{
				startingHandler = (EventHandler<TaskEventArgs>)Delegate.Remove(startingHandler, value);
			}
		}
	}

	public Guid CreateTask(Action action, RecurrencePattern recurrencePattern)
	{
		SchedulerTask schedulerTask = new SchedulerTask(appId, action, recurrencePattern);
		internalTaskScheduler.AddTask(schedulerTask);
		return schedulerTask.Id;
	}

	public IEnumerable<TaskInfo> GetAll()
	{
		return (from x in internalTaskScheduler.GetTasks(appId)
			select x.ToTaskInfo()).ToList();
	}

	public TaskInfo GetTaskInfo(Guid id)
	{
		return internalTaskScheduler.GetTaskInfo(appId, id);
	}

	public void SetEnabledState(Guid id, EnabledState enabledState)
	{
		internalTaskScheduler.SetEnabledState(appId, id, enabledState);
	}

	public bool RemoveTask(Guid id)
	{
		return internalTaskScheduler.RemoveTask(appId, id);
	}

	internal TaskScheduler(string appId)
	{
		this.appId = appId;
		internalTaskScheduler = new InternalTaskScheduler(appId);
	}

	private void OnTaskStarting(SchedulerTask schedulerTask)
	{
		if (schedulerTask.AppId == appId)
		{
			startingHandler?.Invoke(this, new TaskEventArgs(schedulerTask.Id));
		}
	}

	private void OnTaskTerminated(SchedulerTask schedulerTask)
	{
		if (schedulerTask.AppId == appId)
		{
			terminatingHandler?.Invoke(this, new TaskEventArgs(schedulerTask.Id));
		}
	}

	private void OnTaskError(SchedulerTask schedulerTask)
	{
		if (schedulerTask.AppId == appId)
		{
			errorHandler?.Invoke(this, new TaskFailureEventArgs(schedulerTask.Id, schedulerTask.LastError));
		}
	}

	public void Uninitialize()
	{
		internalTaskScheduler.Dispose();
	}
}
