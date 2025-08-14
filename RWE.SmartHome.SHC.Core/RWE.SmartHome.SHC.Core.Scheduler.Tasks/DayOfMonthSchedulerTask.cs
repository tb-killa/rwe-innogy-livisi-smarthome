using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class DayOfMonthSchedulerTask : SchedulerTaskBase
{
	private readonly uint daysOfMonth;

	private readonly int monthsForExecution;

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

	public DayOfMonthSchedulerTask(Guid id, Action taskAction, DateTime startTime, uint dayOfMonth, int month)
		: base(id, taskAction, runOnce: false)
	{
		daysOfMonth = dayOfMonth;
		monthsForExecution = month;
		nextExecutionTime = startTime;
		if (ShcDateTime.Now > nextExecutionTime)
		{
			CalculateNextDate();
		}
	}

	public override bool ShouldExecute(DateTime now)
	{
		if (now > nextExecutionTime && now.TimeOfDay > nextExecutionTime.TimeOfDay && DateTimeHelper.DifferenceInMinutes(now, nextExecutionTime) <= 2.0)
		{
			if (DateTimeHelper.MonthMatches(now, monthsForExecution) && DateTimeHelper.DayOfMonthMatches(now, daysOfMonth))
			{
				return true;
			}
			CalculateNextDate();
		}
		return false;
	}

	protected void CalculateNextDate()
	{
		nextExecutionTime = nextExecutionTime.AddDays(1.0);
	}
}
