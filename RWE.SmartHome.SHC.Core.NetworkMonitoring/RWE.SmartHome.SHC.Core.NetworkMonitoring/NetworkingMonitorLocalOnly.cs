using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;

namespace RWE.SmartHome.SHC.Core.NetworkMonitoring;

public class NetworkingMonitorLocalOnly : INetworkingMonitor, IService
{
	public bool InternetAccessAllowed => true;

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}
}
