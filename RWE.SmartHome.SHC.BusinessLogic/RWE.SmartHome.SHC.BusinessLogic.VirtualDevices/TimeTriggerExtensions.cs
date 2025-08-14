using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public static class TimeTriggerExtensions
{
	private const int DayOfWeekDefaultValue = 127;

	private const int DayOfWeekOccurrenceInMonthDefaultValue = 31;

	private const int MonthDefaultValue = 4095;

	private const uint DayOfMonthDefaultValue = uint.MaxValue;

	private const int RecurrenceIntervalDefaultValue = 0;

	public static List<TimeSpan> GetStartTimes(this CustomTrigger ct)
	{
		return (from prop in ct.Properties.OfType<StringProperty>()
			where prop.Name == "StartTime" && prop.Value.Length > 0
			select TimeSpan.Parse(prop.Value)).ToList();
	}

	public static int GetDayOfWeek(this CustomTrigger ct)
	{
		return Convert.ToInt32(ct.Properties.GetDecimalValue("DayOfWeek") ?? 127m);
	}

	public static int GetDowOccurrenceInMonth(this CustomTrigger ct)
	{
		return Convert.ToInt32(ct.Properties.GetDecimalValue("DayOfWeekOccurrenceInMonth") ?? 31m);
	}

	public static int GetMonth(this CustomTrigger ct)
	{
		return Convert.ToInt32(ct.Properties.GetDecimalValue("Month") ?? 4095m);
	}

	public static uint GetDayOfMonth(this CustomTrigger ct)
	{
		return Convert.ToUInt32(ct.Properties.GetDecimalValue("DayOfMonth") ?? 4294967295m);
	}

	public static int GetRecurrenceInterval(this CustomTrigger ct)
	{
		return Convert.ToInt32(ct.Properties.GetDecimalValue("RecurrenceInterval") ?? 0m);
	}

	public static DateTime? GetStartDate(this CustomTrigger ct)
	{
		StringProperty stringProperty = ct.Properties.OfType<StringProperty>().FirstOrDefault((StringProperty pp) => pp.Name == "StartDate" && pp.Value.Length > 0);
		string value = ((stringProperty != null) ? stringProperty.Value : string.Empty);
		return GetDateFromString(value);
	}

	private static DateTime? GetDateFromString(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			Match match = Regex.Match(value, "(?<year>\\d{4})-(?<month>\\d{1,2})-(?<day>\\d{1,2})");
			if (match != null)
			{
				int year = int.Parse(match.Groups["year"].Value);
				int month = int.Parse(match.Groups["month"].Value);
				int day = int.Parse(match.Groups["day"].Value);
				return new DateTime(year, month, day);
			}
		}
		return null;
	}
}
