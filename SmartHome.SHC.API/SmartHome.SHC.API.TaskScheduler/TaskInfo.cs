using System;

namespace SmartHome.SHC.API.TaskScheduler;

public class TaskInfo
{
	public Guid Id { get; private set; }

	public EnabledState EnabledState { get; private set; }

	public RunningState RunningState { get; private set; }

	public Action Action { get; private set; }

	public RecurrencePattern Recurrence { get; private set; }

	public TaskInfo(Guid id, EnabledState enabledState, RunningState runningState, Action action, RecurrencePattern recurrencePattern)
	{
		Id = id;
		EnabledState = enabledState;
		RunningState = runningState;
		Action = action;
		Recurrence = recurrencePattern;
	}
}
