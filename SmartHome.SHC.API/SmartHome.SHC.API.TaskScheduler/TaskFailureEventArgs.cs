using System;

namespace SmartHome.SHC.API.TaskScheduler;

public class TaskFailureEventArgs : TaskEventArgs
{
	public Exception Exception { get; private set; }

	public TaskFailureEventArgs(Guid taskId, Exception exception)
		: base(taskId)
	{
		Exception = exception;
	}
}
