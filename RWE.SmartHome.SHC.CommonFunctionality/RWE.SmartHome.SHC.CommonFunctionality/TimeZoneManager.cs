using System;
using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class TimeZoneManager
{
	private static ITimeZoneManager Instance = new TimeZoneManagerImplementation();

	public static event EventHandler TimeZoneChanged;

	internal static void OverrideImplementation(ITimeZoneManager implementation)
	{
		Instance = implementation;
	}

	public static string GetShcTimeZoneName()
	{
		return Instance.GetShcTimeZoneName();
	}

	public static TimeSpan GetShcUtcOffset()
	{
		return Instance.GetShcUtcOffset();
	}

	public static bool RefreshTimeZone(string defaultTzName)
	{
		return Instance.RefreshTimeZone(defaultTzName);
	}

	public static bool SetTimeZone(string tzname)
	{
		bool result = Instance.SetTimeZone(tzname);
		if (TimeZoneManager.TimeZoneChanged != null)
		{
			TimeZoneManager.TimeZoneChanged(null, EventArgs.Empty);
		}
		return result;
	}
}
