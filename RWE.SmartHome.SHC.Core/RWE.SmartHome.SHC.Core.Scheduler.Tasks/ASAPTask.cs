using System;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class ASAPTask : SchedulerTaskBase
{
	public ASAPTask(Guid id, Action taskAction)
		: base(id, taskAction, runOnce: true)
	{
	}

	public override bool ShouldExecute(DateTime now)
	{
		return true;
	}
}
