using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class FixedTimeSchedulerTask : SchedulerTaskBase
{
	private DateTime nextExecutionTime;

	public override Action TaskAction
	{
		get
		{
			Action action = base.TaskAction;
			return delegate
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Log.Exception(Module.Core, ex, "Error occured in executing Task Action");
				}
				CalculateNextDate();
			};
		}
	}

	public FixedTimeSchedulerTask(Guid id, Action taskAction, TimeSpan timeOfDay)
		: this(id, taskAction, timeOfDay, runOnce: false)
	{
	}

	public FixedTimeSchedulerTask(Guid id, Action taskAction, TimeSpan timeOfDay, bool runOnce)
		: base(id, taskAction, runOnce)
	{
		nextExecutionTime = new DateTime(ShcDateTime.Now.Year, ShcDateTime.Now.Month, ShcDateTime.Now.Day).Add(timeOfDay);
		if (ShcDateTime.Now > nextExecutionTime)
		{
			CalculateNextDate();
		}
	}

	public override bool ShouldExecute(DateTime now)
	{
		return now > nextExecutionTime;
	}

	protected void CalculateNextDate()
	{
		DateTime now = ShcDateTime.Now;
		nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, nextExecutionTime.Hour, nextExecutionTime.Minute, nextExecutionTime.Second).AddDays(1.0);
	}
}
