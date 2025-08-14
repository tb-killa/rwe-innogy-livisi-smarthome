using System;

namespace SmartHome.SHC.API.TaskScheduler;

public class RecurrencePattern
{
	public TimeSpan? Interval { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public static RecurrencePattern CreateOneTimePattern(DateTime time)
	{
		RecurrencePattern recurrencePattern = new RecurrencePattern();
		recurrencePattern.StartTime = time;
		return recurrencePattern;
	}

	public static RecurrencePattern CreatePeriodicPattern(int milliseconds)
	{
		RecurrencePattern recurrencePattern = new RecurrencePattern();
		recurrencePattern.Interval = TimeSpan.FromMilliseconds(milliseconds);
		return recurrencePattern;
	}

	public static RecurrencePattern CreateTemporaryTaskPattern(DateTime fromDate, DateTime toDate, int intervalMilliseconds)
	{
		RecurrencePattern recurrencePattern = new RecurrencePattern();
		recurrencePattern.StartTime = fromDate;
		recurrencePattern.Interval = TimeSpan.FromMilliseconds(intervalMilliseconds);
		recurrencePattern.EndTime = toDate;
		return recurrencePattern;
	}
}
