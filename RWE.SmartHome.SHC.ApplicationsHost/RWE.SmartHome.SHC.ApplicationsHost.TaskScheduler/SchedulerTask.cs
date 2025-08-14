using System;
using SmartHome.SHC.API.TaskScheduler;

namespace RWE.SmartHome.SHC.ApplicationsHost.TaskScheduler;

internal class SchedulerTask
{
	public Guid Id { get; private set; }

	public EnabledState EnabledState { get; set; }

	public RunningState RunningState { get; set; }

	public Action Action { get; private set; }

	public RecurrencePattern Recurrence { get; private set; }

	public DateTime? LastExecutionTime { get; set; }

	public Exception LastError { get; set; }

	public string AppId { get; private set; }

	public SchedulerTask(string appId, Action action, RecurrencePattern recurrence)
		: this(appId, action, recurrence, EnabledState.Enabled)
	{
	}

	public SchedulerTask(string appId, Action action, RecurrencePattern recurrence, EnabledState enabledState)
	{
		AppId = appId;
		Id = Guid.NewGuid();
		Action = action;
		Recurrence = recurrence;
		RunningState = RunningState.None;
		EnabledState = enabledState;
	}

	public TaskInfo ToTaskInfo()
	{
		return new TaskInfo(Id, EnabledState, RunningState, Action, Recurrence);
	}
}
