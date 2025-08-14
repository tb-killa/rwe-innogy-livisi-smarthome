using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.Configuration;

public class TimeInteractionSetPoint
{
	public List<DayOfWeek> Weekdays { get; set; }

	public DateTime Time { get; set; }

	public ActionDescription ActionDescriptions { get; set; }
}
