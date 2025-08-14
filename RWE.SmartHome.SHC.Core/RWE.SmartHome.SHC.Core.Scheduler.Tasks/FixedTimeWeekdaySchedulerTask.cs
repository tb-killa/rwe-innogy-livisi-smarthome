using System;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class FixedTimeWeekdaySchedulerTask : FixedTimeSchedulerTask
{
	private readonly WeekDay daysForExecution;

	public FixedTimeWeekdaySchedulerTask(Guid id, Action taskAction, TimeSpan timeOfDay, WeekDay days)
		: base(id, taskAction, timeOfDay, runOnce: false)
	{
		daysForExecution = days;
	}

	public override bool ShouldExecute(DateTime now)
	{
		if (base.ShouldExecute(now))
		{
			if (DateTimeHelper.WeekDayMatches(now.DayOfWeek, daysForExecution))
			{
				return true;
			}
			CalculateNextDate();
		}
		return false;
	}
}
