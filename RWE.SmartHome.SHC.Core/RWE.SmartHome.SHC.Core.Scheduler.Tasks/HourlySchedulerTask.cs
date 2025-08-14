using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class HourlySchedulerTask : SchedulerTaskBase
{
	internal DateTime nextExecutionTime;

	private readonly int recurrenceInterval;

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

	public HourlySchedulerTask(Guid id, Action taskAction, DateTime startTime, int recurrenceInterval)
		: base(id, taskAction, recurrenceInterval == 0)
	{
		this.recurrenceInterval = recurrenceInterval;
		DateTime now = ShcDateTime.Now;
		nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, startTime.TimeOfDay.Hours, startTime.TimeOfDay.Minutes, 0);
		if (ShcDateTime.Now > nextExecutionTime)
		{
			CalculateNextDate();
		}
	}

	public override bool ShouldExecute(DateTime now)
	{
		if (DateTimeHelper.IsTheSameDay(now, nextExecutionTime) && now.TimeOfDay > nextExecutionTime.TimeOfDay)
		{
			return DateTimeHelper.DifferenceInMinutes(now, nextExecutionTime) <= 2.0;
		}
		return false;
	}

	protected void CalculateNextDate()
	{
		nextExecutionTime = nextExecutionTime.AddHours(recurrenceInterval);
	}
}
