using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class CalendarTask
{
	public uint TaskId { get; set; }

	public DateTime Start { get; set; }

	public DateTime? End { get; set; }

	public uint ActionId { get; set; }

	public Repeat? Repeat { get; set; }

	public List<DayOfWeek> WeekDays { get; set; }

	public CalendarTask()
	{
	}

	public CalendarTask(uint taskId, DateTime start, DateTime? end, uint actionId, Repeat? repeat, List<DayOfWeek> weekDays)
	{
		TaskId = taskId;
		Start = start;
		End = end;
		ActionId = actionId;
		Repeat = repeat;
		WeekDays = weekDays;
	}
}
