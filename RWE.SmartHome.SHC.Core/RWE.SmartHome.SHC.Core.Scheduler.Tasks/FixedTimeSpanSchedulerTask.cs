using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class FixedTimeSpanSchedulerTask : SchedulerTaskBase
{
	private readonly TimeSpan timeSpan;

	private DateTime nextExecutionTime;

	public override Action TaskAction => delegate
	{
		try
		{
			nextExecutionTime = ShcDateTime.Now;
			ExecuteBaseAction();
		}
		catch (Exception ex)
		{
			Log.Exception(Module.Core, ex, "Error occured in executing Task Action");
		}
		CalculateNextDate();
	};

	public FixedTimeSpanSchedulerTask(Guid id, Action taskAction, TimeSpan timeSpan)
		: this(id, taskAction, timeSpan, runOnce: false)
	{
	}

	public FixedTimeSpanSchedulerTask(Guid id, Action taskAction, TimeSpan timeSpan, bool runOnce)
		: base(id, taskAction, runOnce)
	{
		this.timeSpan = timeSpan;
		nextExecutionTime = ShcDateTime.Now;
		CalculateNextDate();
	}

	public override bool ShouldExecute(DateTime now)
	{
		return now > nextExecutionTime;
	}

	private void ExecuteBaseAction()
	{
		base.TaskAction();
	}

	private void CalculateNextDate()
	{
		nextExecutionTime = new DateTime(nextExecutionTime.Year, nextExecutionTime.Month, nextExecutionTime.Day, nextExecutionTime.Hour, nextExecutionTime.Minute, nextExecutionTime.Second).Add(timeSpan);
	}
}
