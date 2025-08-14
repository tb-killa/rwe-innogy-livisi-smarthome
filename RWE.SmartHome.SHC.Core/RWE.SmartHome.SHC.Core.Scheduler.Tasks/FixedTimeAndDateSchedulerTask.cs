using System;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class FixedTimeAndDateSchedulerTask : FixedTimeSchedulerTask
{
	private readonly DateTime executionDay;

	public FixedTimeAndDateSchedulerTask(Guid id, Action taskAction, DateTime fixedDate)
		: base(id, taskAction, fixedDate.TimeOfDay, runOnce: true)
	{
		executionDay = fixedDate;
	}

	public override bool ShouldExecute(DateTime now)
	{
		if (base.ShouldExecute(now))
		{
			return executionDay.Date == now.Date;
		}
		return false;
	}
}
