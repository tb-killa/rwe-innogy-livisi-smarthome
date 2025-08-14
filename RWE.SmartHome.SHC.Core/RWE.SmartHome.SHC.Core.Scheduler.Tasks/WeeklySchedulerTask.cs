using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class WeeklySchedulerTask : SchedulerTaskBase
{
	private readonly WeekDay daysForExecution;

	private readonly int recurrenceInterval;

	private readonly DateTime startDate;

	private DateTime nextExecutionTime;

	public override Action TaskAction
	{
		get
		{
			Action action = base.TaskAction;
			return delegate
			{
				CalculateNextDate();
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Log.Exception(Module.Core, ex, "Error occured in executing Task Action");
				}
			};
		}
	}

	public WeeklySchedulerTask(Guid id, Action taskAction, DateTime startTime, int recurrenceInterval, WeekDay days)
		: base(id, taskAction, recurrenceInterval == 0)
	{
		daysForExecution = days;
		this.recurrenceInterval = recurrenceInterval;
		startDate = startTime.Date;
		nextExecutionTime = startTime;
		if (ShcDateTime.Now.Truncate(TimeSpan.FromSeconds(1.0)).TimeOfDay > nextExecutionTime.TimeOfDay)
		{
			CalculateNextDate();
		}
	}

	public override bool ShouldExecute(DateTime now)
	{
		if (now > nextExecutionTime)
		{
			if (RecurrenceRulesAreSatisfied(now) && DateTimeHelper.DifferenceInMinutes(now, nextExecutionTime) <= 2.0)
			{
				return true;
			}
			CalculateNextDate();
		}
		return false;
	}

	protected void CalculateNextDate()
	{
		DateTime now = ShcDateTime.Now;
		nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, nextExecutionTime.Hour, nextExecutionTime.Minute, nextExecutionTime.Second).AddDays(1.0);
		Log.Information(Module.Core, $"WeeklySchedulerTask will be triggered next {nextExecutionTime}");
	}

	private bool RecurrenceRulesAreSatisfied(DateTime now)
	{
		if (!DateTimeHelper.WeekDayMatches(now.DayOfWeek, daysForExecution))
		{
			return false;
		}
		if (TaskIsNotRecurrent())
		{
			return true;
		}
		if (RecurrenceShouldHappenThisWeek(now))
		{
			return true;
		}
		return false;
	}

	private bool RecurrenceShouldHappenThisWeek(DateTime now)
	{
		int num = NumberOfWeeksBetweenDates(now, startDate) % recurrenceInterval;
		return num == 0;
	}

	private int NumberOfWeeksBetweenDates(DateTime firstDate, DateTime secondDate)
	{
		return ((firstDate - secondDate).Days + (DayOfWeekNumberFor(secondDate) - DayOfWeekNumberFor(firstDate))) / 7;
	}

	private int DayOfWeekNumberFor(DateTime firstDate)
	{
		if (firstDate.DayOfWeek == DayOfWeek.Sunday)
		{
			return 7;
		}
		return (int)firstDate.DayOfWeek;
	}

	private bool TaskIsNotRecurrent()
	{
		return recurrenceInterval == 0;
	}
}
