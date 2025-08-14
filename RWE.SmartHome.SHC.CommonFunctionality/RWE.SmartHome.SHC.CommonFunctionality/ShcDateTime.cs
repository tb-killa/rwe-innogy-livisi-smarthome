using System;
using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class ShcDateTime
{
	private static IDateTimeProvider instance = new ShcDateTimeImplementation();

	public static DateTime Now => instance.Now;

	public static DateTime UtcNow => instance.UtcNow;

	public static void OverrideImplementation(IDateTimeProvider implementation)
	{
		instance = implementation;
	}

	public static DateTime LocalToUtc(DateTime localTime)
	{
		return instance.LocalToUtc(localTime);
	}

	public static DateTime UtcToLocal(DateTime utcTime)
	{
		return instance.UtcToLocal(utcTime);
	}
}
