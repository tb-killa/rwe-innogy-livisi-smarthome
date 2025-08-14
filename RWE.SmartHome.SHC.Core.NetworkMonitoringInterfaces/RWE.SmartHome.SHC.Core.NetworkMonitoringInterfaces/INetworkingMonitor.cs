namespace RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;

public interface INetworkingMonitor : IService
{
	bool InternetAccessAllowed { get; }
}
