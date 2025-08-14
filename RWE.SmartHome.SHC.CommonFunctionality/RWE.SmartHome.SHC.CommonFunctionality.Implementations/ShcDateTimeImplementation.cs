using System;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

public class ShcDateTimeImplementation : IDateTimeProvider
{
	public DateTime Now => DateTime.Now;

	public DateTime UtcNow => DateTime.UtcNow;

	public DateTime LocalToUtc(DateTime localTime)
	{
		return localTime.ToUniversalTime();
	}

	public DateTime UtcToLocal(DateTime utcTime)
	{
		return utcTime.ToLocalTime();
	}
}
