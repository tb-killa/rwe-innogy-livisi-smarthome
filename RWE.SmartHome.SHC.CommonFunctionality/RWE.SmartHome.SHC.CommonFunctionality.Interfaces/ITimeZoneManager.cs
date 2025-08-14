using System;

namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

public interface ITimeZoneManager
{
	string GetShcTimeZoneName();

	bool RefreshTimeZone(string defaultTzName);

	bool SetTimeZone(string tzname);

	TimeSpan GetShcUtcOffset();
}
