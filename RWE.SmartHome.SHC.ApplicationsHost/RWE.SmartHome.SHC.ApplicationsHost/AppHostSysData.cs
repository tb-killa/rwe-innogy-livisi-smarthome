using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class AppHostSysData : IAppHostSysData
{
	public int MemoryLoad => PerformanceMonitoring.GetGlobalMemoryStatus().LoadPercentage;

	public int MemoryLimit { get; set; }

	public int MemoryLimitStartup { get; set; }

	public AppHostSysData()
	{
		MemoryLimit = 100;
		MemoryLimitStartup = 100;
	}
}
