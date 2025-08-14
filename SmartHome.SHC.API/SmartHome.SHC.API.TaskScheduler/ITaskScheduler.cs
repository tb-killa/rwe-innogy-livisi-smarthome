using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.TaskScheduler;

public interface ITaskScheduler
{
	event EventHandler<TaskEventArgs> TaskStarting;

	event EventHandler<TaskEventArgs> TaskCompleted;

	event EventHandler<TaskFailureEventArgs> TaskFailed;

	Guid CreateTask(Action action, RecurrencePattern recurrencePattern);

	bool RemoveTask(Guid id);

	TaskInfo GetTaskInfo(Guid id);

	IEnumerable<TaskInfo> GetAll();

	void SetEnabledState(Guid id, EnabledState enabledState);
}
