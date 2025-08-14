using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class SchedulerTaskBuilder
{
	public List<SchedulerTaskBase> CreateTasks(CustomTrigger customTrigger, Action<Guid> callback)
	{
		Action callback2 = delegate
		{
			callback(customTrigger.Id);
		};
		return customTrigger.Type switch
		{
			"BySecondTrigger" => CreateBySecondTrigger(customTrigger, callback2), 
			"HourlyTrigger" => CreateHourlyTrigger(customTrigger, callback2), 
			"DailyTrigger" => CreateDailyTrigger(customTrigger, callback2), 
			"WeeklyTrigger" => CreateWeeklyTrigger(customTrigger, callback2), 
			"DayOfWeekMonthlyTrigger" => CreateDayOfWeekMonthlyTrigger(customTrigger, callback2), 
			"DayOfMonthTrigger" => CreateDayOfMonthTrigger(customTrigger, callback2), 
			_ => new List<SchedulerTaskBase>(), 
		};
	}

	private List<SchedulerTaskBase> CreateBySecondTrigger(CustomTrigger customTrigger, Action callback)
	{
		int num = customTrigger.GetRecurrenceInterval();
		if (num == 0)
		{
			num = 30;
		}
		List<SchedulerTaskBase> list = new List<SchedulerTaskBase>();
		list.Add(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), callback, TimeSpan.FromSeconds(num)));
		return list;
	}

	private List<SchedulerTaskBase> CreateHourlyTrigger(CustomTrigger customTrigger, Action callback)
	{
		List<SchedulerTaskBase> list = new List<SchedulerTaskBase>();
		TimeSpan timeSpan = customTrigger.GetStartTimes().First();
		int recurrenceInterval = customTrigger.GetRecurrenceInterval();
		DateTime dateTime = DateTime.Now.Date + timeSpan;
		if (WillTaskBeExecuted(ShcDateTime.Now, dateTime, recurrenceInterval))
		{
			list.Add(new HourlySchedulerTask(Guid.NewGuid(), callback, dateTime, recurrenceInterval));
		}
		return list;
	}

	private List<SchedulerTaskBase> CreateDailyTrigger(CustomTrigger customTrigger, Action callback)
	{
		DateTime startDate = GetValidStartDateFromCustomTriggerOrDateTimeNow(customTrigger);
		List<DateTime> dates = (from startTime in customTrigger.GetStartTimes()
			select startDate + startTime).ToList();
		return CreateRecurrentTasks(dates, ShcDateTime.Now, customTrigger.GetRecurrenceInterval(), callback);
	}

	private List<SchedulerTaskBase> CreateWeeklyTrigger(CustomTrigger customTrigger, Action callback)
	{
		DateTime startDate = GetValidStartDateFromCustomTriggerOrDateTimeNow(customTrigger);
		List<DateTime> list = (from startTime in customTrigger.GetStartTimes()
			select startDate + startTime).ToList();
		int recurrenceInterval = customTrigger.GetRecurrenceInterval();
		int dayOfWeek = customTrigger.GetDayOfWeek();
		if (recurrenceInterval != 0)
		{
			return ((IEnumerable<DateTime>)list).Select((Func<DateTime, SchedulerTaskBase>)((DateTime time) => new WeeklySchedulerTask(Guid.NewGuid(), callback, time, recurrenceInterval, DateTimeHelper.GetWeekDay(dayOfWeek)))).ToList();
		}
		return ProcessNonRecurrentTasks(list, dayOfWeek, recurrenceInterval, callback);
	}

	private DateTime GetValidStartDateFromCustomTriggerOrDateTimeNow(CustomTrigger customTrigger)
	{
		DateTime? startDate = customTrigger.GetStartDate();
		if (!startDate.HasValue || startDate < ShcDateTime.Now.Date)
		{
			return ShcDateTime.Now.Date;
		}
		return startDate.Value;
	}

	private List<SchedulerTaskBase> CreateDayOfWeekMonthlyTrigger(CustomTrigger customTrigger, Action callback)
	{
		List<DateTime> source = (from startTime in customTrigger.GetStartTimes()
			select DateTime.Now.Date + startTime).ToList();
		int dayOfWeek = customTrigger.GetDayOfWeek();
		int dayOfWeekOccurrenceInMonth = customTrigger.GetDowOccurrenceInMonth();
		int month = customTrigger.GetMonth();
		return ((IEnumerable<DateTime>)source).Select((Func<DateTime, SchedulerTaskBase>)((DateTime time) => new DayOfWeekMonthlySchedulerTask(Guid.NewGuid(), callback, time, DateTimeHelper.GetWeekDay(dayOfWeek), dayOfWeekOccurrenceInMonth, month))).ToList();
	}

	private List<SchedulerTaskBase> CreateDayOfMonthTrigger(CustomTrigger customTrigger, Action callback)
	{
		List<DateTime> source = (from startTime in customTrigger.GetStartTimes()
			select DateTime.Now.Date + startTime).ToList();
		uint dayOfMonth = customTrigger.GetDayOfMonth();
		int month = customTrigger.GetMonth();
		return ((IEnumerable<DateTime>)source).Select((Func<DateTime, SchedulerTaskBase>)((DateTime startTime) => new DayOfMonthSchedulerTask(Guid.NewGuid(), callback, startTime, dayOfMonth, month))).ToList();
	}

	private SchedulerTaskBase GetDailyRecurrentTask(DateTime referenceTime, DateTime startTime, int recurrenceInterval, Action callback)
	{
		if (!WillTaskBeExecuted(referenceTime, startTime, recurrenceInterval))
		{
			return null;
		}
		return new DailySchedulerTask(Guid.NewGuid(), callback, startTime, recurrenceInterval);
	}

	private List<SchedulerTaskBase> ProcessNonRecurrentTasks(List<DateTime> startTimeList, int dayOfWeek, int recurrenceInterval, Action callback)
	{
		DateTime shcTime = ShcDateTime.Now;
		List<DateTime> list = new List<DateTime>();
		for (int i = 0; i <= 6; i++)
		{
			foreach (DateTime startTime in startTimeList)
			{
				list.Add(startTime.AddDays(i));
			}
		}
		list.RemoveAll((DateTime d) => !DateTimeHelper.WeekDayMatches(d.DayOfWeek, DateTimeHelper.GetWeekDay(dayOfWeek)));
		list.RemoveAll((DateTime d) => shcTime > d);
		return CreateRecurrentTasks(list, shcTime, recurrenceInterval, callback);
	}

	private List<SchedulerTaskBase> CreateRecurrentTasks(IEnumerable<DateTime> dates, DateTime refTime, int recurrenceInterval, Action callback)
	{
		return (from date in dates
			select GetDailyRecurrentTask(refTime, date, recurrenceInterval, callback) into task
			where task != null
			select task).ToList();
	}

	private bool WillTaskBeExecuted(DateTime now, DateTime initialTime, int recurrenceInterval)
	{
		if (now > initialTime)
		{
			return recurrenceInterval != 0;
		}
		return true;
	}
}
