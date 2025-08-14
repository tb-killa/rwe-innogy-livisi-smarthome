using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;

public class CalendarTask : ConfigurationItem
{
	public DateTime Start { get; set; }

	public DateTime? End { get; set; }

	public uint ActionId { get; set; }

	public Repeat? Repeat { get; set; }

	public List<DayOfWeek> WeekDays { get; set; }

	public override bool Equals(ConfigurationItem other)
	{
		if (other is CalendarTask calendarTask && base.Id == calendarTask.Id && Start == calendarTask.Start)
		{
			DateTime? end = End;
			DateTime? end2 = calendarTask.End;
			if (end.HasValue == end2.HasValue && (!end.HasValue || end.GetValueOrDefault() == end2.GetValueOrDefault()) && ActionId == calendarTask.ActionId && Repeat == calendarTask.Repeat)
			{
				return WeekDays.ContentEqual(calendarTask.WeekDays);
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = (base.Id.GetHashCode() * 937) ^ (Repeat.HasValue ? Repeat.Value.GetHashCode() : 0);
		num = (937 * num) ^ Start.GetHashCode();
		num = (937 * num) ^ End.GetHashCode();
		return (937 * num) ^ ActionId.GetHashCode();
	}
}
