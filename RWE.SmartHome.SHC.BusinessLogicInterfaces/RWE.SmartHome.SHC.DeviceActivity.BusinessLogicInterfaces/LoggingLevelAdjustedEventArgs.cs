using System;

namespace RWE.SmartHome.SHC.DeviceActivity.BusinessLogicInterfaces;

public class LoggingLevelAdjustedEventArgs
{
	public TimeSpan ElevationExpiration { get; set; }

	public string Requester { get; set; }

	public string Reason { get; set; }

	public LoggingLevelAdjustedEventArgs(TimeSpan elevationExpiration, string requester, string reason)
	{
		ElevationExpiration = elevationExpiration;
		Requester = requester;
		Reason = reason;
	}
}
