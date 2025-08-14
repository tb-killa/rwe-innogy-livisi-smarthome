namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public interface IAppHostSysData
{
	int MemoryLoad { get; }

	int MemoryLimit { get; }

	int MemoryLimitStartup { get; }
}
