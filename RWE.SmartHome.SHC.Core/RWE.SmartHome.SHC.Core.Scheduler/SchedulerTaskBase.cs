using System;

namespace RWE.SmartHome.SHC.Core.Scheduler;

public abstract class SchedulerTaskBase : ISchedulerTask
{
	public Guid TaskId { get; private set; }

	public bool RunOnce { get; private set; }

	public virtual Action TaskAction { get; private set; }

	protected SchedulerTaskBase(Guid id, Action taskAction, bool runOnce)
	{
		TaskId = id;
		TaskAction = taskAction;
		RunOnce = runOnce;
	}

	public abstract bool ShouldExecute(DateTime now);
}
