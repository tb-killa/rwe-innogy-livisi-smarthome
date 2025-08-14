using System;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.Core.Scheduler;

public static class DateTimeHelper
{
	private static readonly int LastDayOfWeekOccurrenceInMonth = 16;

	private static readonly int LastDayOfMonth = int.MinValue;

	public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
	{
		if (timeSpan == TimeSpan.Zero)
		{
			return dateTime;
		}
		return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
	}

	public static bool WeekDayMatches(DayOfWeek dayOfWeek, WeekDay daysForExecution)
	{
		return dayOfWeek switch
		{
			DayOfWeek.Monday => (int)(daysForExecution & WeekDay.Monday) > 0, 
			DayOfWeek.Tuesday => (int)(daysForExecution & WeekDay.Tuesday) > 0, 
			DayOfWeek.Wednesday => (int)(daysForExecution & WeekDay.Wednesday) > 0, 
			DayOfWeek.Thursday => (int)(daysForExecution & WeekDay.Thursday) > 0, 
			DayOfWeek.Friday => (int)(daysForExecution & WeekDay.Friday) > 0, 
			DayOfWeek.Saturday => (int)(daysForExecution & WeekDay.Saturday) > 0, 
			DayOfWeek.Sunday => (int)(daysForExecution & WeekDay.Sunday) > 0, 
			_ => throw new ArgumentOutOfRangeException("dayOfWeek"), 
		};
	}

	public static bool DayOfWeekOccurrenceInMonthMatches(DateTime now, int bitmask)
	{
		if ((bitmask & GetDayOfWeekOccurrenceInMonthHexValue(now)) <= 0)
		{
			if ((bitmask & LastDayOfWeekOccurrenceInMonth) > 0)
			{
				return IsLastDayOfWeekOccurrenceInMonth(now);
			}
			return false;
		}
		return true;
	}

	public static bool MonthMatches(DateTime now, int bitmask)
	{
		return (bitmask & GetMonthHexValue(now)) > 0;
	}

	public static bool DayOfMonthMatches(DateTime now, uint bitmask)
	{
		if ((bitmask & GetDayOfMonthHexValue(now)) <= 0)
		{
			if ((bitmask & LastDayOfMonth) > 0)
			{
				return IsLastDayOfMonth(now);
			}
			return false;
		}
		return true;
	}

	public static WeekDay GetWeekDay(int dayOfWeek)
	{
		WeekDay weekDay = (WeekDay)0;
		if ((long)((ulong)dayOfWeek & 1uL) > 0L)
		{
			weekDay = WeekDay.Monday;
		}
		if ((long)((ulong)dayOfWeek & 2uL) > 0L)
		{
			weekDay |= WeekDay.Tuesday;
		}
		if ((long)((ulong)dayOfWeek & 4uL) > 0L)
		{
			weekDay |= WeekDay.Wednesday;
		}
		if ((long)((ulong)dayOfWeek & 8uL) > 0L)
		{
			weekDay |= WeekDay.Thursday;
		}
		if ((long)((ulong)dayOfWeek & 0x10uL) > 0L)
		{
			weekDay |= WeekDay.Friday;
		}
		if ((long)((ulong)dayOfWeek & 0x20uL) > 0L)
		{
			weekDay |= WeekDay.Saturday;
		}
		if ((long)((ulong)dayOfWeek & 0x40uL) > 0L)
		{
			weekDay |= WeekDay.Sunday;
		}
		return weekDay;
	}

	public static bool IsTheSameDay(DateTime firstDateTime, DateTime secondDateTime)
	{
		if (firstDateTime.Year == secondDateTime.Year)
		{
			return firstDateTime.DayOfYear == secondDateTime.DayOfYear;
		}
		return false;
	}

	public static int GetDayOfWeekOccurrenceInMonthHexValue(DateTime now)
	{
		int num = 0;
		DateTime dateTime = now;
		do
		{
			dateTime = dateTime.AddDays(-7.0);
			num++;
		}
		while (dateTime.Month == now.Month);
		return 1 << num - 1;
	}

	public static int GetMonthHexValue(DateTime now)
	{
		return 1 << now.Month - 1;
	}

	public static int GetDayOfMonthHexValue(DateTime now)
	{
		return 1 << now.Day - 1;
	}

	public static double DifferenceInMinutes(DateTime firstDate, DateTime secondDate)
	{
		return (firstDate.TimeOfDay - secondDate.TimeOfDay).TotalMinutes;
	}

	private static bool IsLastDayOfMonth(DateTime now)
	{
		if (now.AddDays(1.0).Month != now.Month)
		{
			return true;
		}
		return false;
	}

	private static bool IsLastDayOfWeekOccurrenceInMonth(DateTime now)
	{
		if (now.AddDays(7.0).Month != now.Month)
		{
			return true;
		}
		return false;
	}
}
