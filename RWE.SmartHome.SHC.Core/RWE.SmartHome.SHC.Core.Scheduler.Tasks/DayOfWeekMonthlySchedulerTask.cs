using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class DayOfWeekMonthlySchedulerTask : SchedulerTaskBase
{
	private readonly WeekDay daysForExecution;

	private readonly int dayOfWeekOccurrenceInMonth;

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

	public DayOfWeekMonthlySchedulerTask(Guid id, Action taskAction, DateTime startTime, WeekDay days, int dayOfWeekOccurenceInMonth, int month)
		: base(id, taskAction, runOnce: false)
	{
		daysForExecution = days;
		dayOfWeekOccurrenceInMonth = dayOfWeekOccurenceInMonth;
		monthsForExecution = month;
		nextExecutionTime = startTime;
		if (ShcDateTime.Now > nextExecutionTime)
		{
			CalculateNextDate();
		}
	}

	public override bool ShouldExecute(DateTime now)
	{
		if (now > nextExecutionTime && now.TimeOfDay > nextExecutionTime.TimeOfDay && DateTimeHelper.DifferenceInMinutes(now, nextExecutionTime) <= 1.0)
		{
			if (DateTimeHelper.WeekDayMatches(now.DayOfWeek, daysForExecution) && DateTimeHelper.DayOfWeekOccurrenceInMonthMatches(now, dayOfWeekOccurrenceInMonth) && DateTimeHelper.MonthMatches(now, monthsForExecution))
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
