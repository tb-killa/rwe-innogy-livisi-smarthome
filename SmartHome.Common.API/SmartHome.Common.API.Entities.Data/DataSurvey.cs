using System;
using System.Collections.Generic;

namespace SmartHome.Common.API.Entities.Data;

public class DataSurvey
{
	public int Count { get; set; }

	public DateTime MaxDate { get; set; }

	public DateTime MinDate { get; set; }

	public List<DateTime> Records { get; set; }
}
