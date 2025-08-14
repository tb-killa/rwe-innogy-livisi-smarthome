using System;
using System.Runtime.InteropServices;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

public class PerformanceMonitoringImplementation : IPerformanceMonitoring
{
	private const int oneMbSquared = 1048576;

	private static bool first = true;

	private static uint lastTick;

	private static uint lastIdle;

	private readonly DateTime startTime = DateTime.UtcNow;

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	private static extern uint GetIdleTime();

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	private static extern uint GetTickCount();

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	private static extern void GlobalMemoryStatus(ref MEMORYSTATUS lpBuffer);

	[DllImport("coredll.dll")]
	private static extern bool GetDiskFreeSpaceEx(string direcotryName, out long freeBytes, out long totalBytes, out long totalFreeBytes);

	public MemoryStatus GetGlobalMemoryStatus()
	{
		MEMORYSTATUS lpBuffer = default(MEMORYSTATUS);
		lpBuffer.dwLength = Marshal.SizeOf((object)lpBuffer);
		if (Environment.OSVersion.Platform == PlatformID.WinCE)
		{
			GlobalMemoryStatus(ref lpBuffer);
		}
		return new MemoryStatus(lpBuffer.dwMemoryLoad, lpBuffer.dwTotalPhys, lpBuffer.dwAvailPhys);
	}

	public float GetCPULoad()
	{
		if (Environment.OSVersion.Platform == PlatformID.WinCE)
		{
			if (first)
			{
				first = false;
				lastTick = GetTickCount();
				lastIdle = GetIdleTime();
			}
			uint tickCount = GetTickCount();
			uint idleTime = GetIdleTime();
			uint num = Math.Max(tickCount - lastTick, 1u);
			float result = 100f - 100f * (float)(idleTime - lastIdle) / (float)num;
			lastTick = tickCount;
			lastIdle = idleTime;
			return result;
		}
		return 0f;
	}

	public float GetDiskUsage()
	{
		long freeBytes = 0L;
		long totalBytes = 0L;
		long totalFreeBytes = 0L;
		GetDiskFreeSpaceEx("\\Nandflash", out freeBytes, out totalBytes, out totalFreeBytes);
		return CalculateDiskUsage(totalBytes, freeBytes);
	}

	public float GetFreeDiskMemory()
	{
		long freeBytes = 0L;
		long totalBytes = 0L;
		long totalFreeBytes = 0L;
		GetDiskFreeSpaceEx("\\Nandflash", out freeBytes, out totalBytes, out totalFreeBytes);
		return freeBytes / 1048576;
	}

	private float CalculateDiskUsage(long totalBytes, long freeBytes)
	{
		long num = totalBytes - freeBytes;
		return num * 100 / totalBytes;
	}

	public string GetPerformanceReport(MemoryStatus memoryStatus)
	{
		float cPULoad = GetCPULoad();
		double num = (double)GC.GetTotalMemory(forceFullCollection: false) / 1048576.0;
		double num2 = (double)memoryStatus.AvailablePhysical / 1048576.0;
		double num3 = (double)memoryStatus.TotalPhysical / 1048576.0;
		long freeBytes = 0L;
		long totalBytes = 0L;
		long totalFreeBytes = 0L;
		GetDiskFreeSpaceEx("\\Nandflash", out freeBytes, out totalBytes, out totalFreeBytes);
		float num4 = CalculateDiskUsage(totalBytes, freeBytes);
		float num5 = totalBytes / 1048576;
		float num6 = freeBytes / 1048576;
		return $"CPU Load {cPULoad}%, Memory Load {memoryStatus.LoadPercentage}%, Used {num3 - num2} MB, Usage managed: {num} MB, Disk Usage {num4}%, {num6} MB available out of {num5} MB";
	}

	public DateTime GetStartTime()
	{
		return startTime;
	}

	public void PrintMemoryUsage(string message)
	{
	}
}
