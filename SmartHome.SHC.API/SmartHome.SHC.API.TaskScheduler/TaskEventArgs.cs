using System;

namespace SmartHome.SHC.API.TaskScheduler;

public class TaskEventArgs : EventArgs
{
	public Guid TaskId { get; private set; }

	public TaskEventArgs(Guid taskId)
	{
		TaskId = taskId;
	}
}
