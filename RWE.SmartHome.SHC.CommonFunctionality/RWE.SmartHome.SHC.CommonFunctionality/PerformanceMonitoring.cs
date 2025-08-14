using System;
using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class PerformanceMonitoring
{
	private static IPerformanceMonitoring Instance = new PerformanceMonitoringImplementation();

	public static MemoryStatus GetGlobalMemoryStatus()
	{
		return Instance.GetGlobalMemoryStatus();
	}

	public static float GetCPULoad()
	{
		return Instance.GetCPULoad();
	}

	public static float GetDiskUsage()
	{
		return Instance.GetDiskUsage();
	}

	public static DateTime StartTime()
	{
		return Instance.GetStartTime();
	}

	public static string GetPerformanceReport()
	{
		return Instance.GetPerformanceReport(GetGlobalMemoryStatus());
	}

	internal static void OverrideImplementation(IPerformanceMonitoring implementation)
	{
		Instance = implementation;
	}

	public static void PrintMemoryUsage(string message)
	{
		Instance.PrintMemoryUsage(message);
	}

	public static float GetFreeDiskMemory()
	{
		return Instance.GetFreeDiskMemory();
	}
}
