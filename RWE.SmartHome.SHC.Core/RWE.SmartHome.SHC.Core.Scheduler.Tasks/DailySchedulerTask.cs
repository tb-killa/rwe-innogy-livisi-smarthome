using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class DailySchedulerTask : SchedulerTaskBase
{
	private DateTime nextExecutionTime;

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

	public DailySchedulerTask(Guid id, Action taskAction, DateTime startTime, int recurrenceInterval)
		: base(id, taskAction, recurrenceInterval == 0)
	{
		this.recurrenceInterval = recurrenceInterval;
		nextExecutionTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, 0);
		CalculateNextDate();
	}

	public override bool ShouldExecute(DateTime now)
	{
		if (DateTimeHelper.IsTheSameDay(now, nextExecutionTime) && now.TimeOfDay > nextExecutionTime.TimeOfDay)
		{
			return DateTimeHelper.DifferenceInMinutes(now, nextExecutionTime) <= 1.0;
		}
		return false;
	}

	protected void CalculateNextDate()
	{
		if (ShouldDoTheCalculation())
		{
			nextExecutionTime = nextExecutionTime.AddDays(NumberOfDaysToIncrementNextExecutionTime());
		}
	}

	private bool ShouldDoTheCalculation()
	{
		if (ShcDateTime.Now > nextExecutionTime)
		{
			return recurrenceInterval > 0;
		}
		return false;
	}

	private int NumberOfDaysToIncrementNextExecutionTime()
	{
		double totalDays = (ShcDateTime.Now - nextExecutionTime).TotalDays;
		return (int)Math.Ceiling(totalDays / (double)recurrenceInterval) * recurrenceInterval;
	}
}
