using System;

namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

public interface IPerformanceMonitoring
{
	MemoryStatus GetGlobalMemoryStatus();

	float GetCPULoad();

	float GetDiskUsage();

	string GetPerformanceReport(MemoryStatus memoryStatus);

	void PrintMemoryUsage(string message);

	DateTime GetStartTime();

	float GetFreeDiskMemory();
}
