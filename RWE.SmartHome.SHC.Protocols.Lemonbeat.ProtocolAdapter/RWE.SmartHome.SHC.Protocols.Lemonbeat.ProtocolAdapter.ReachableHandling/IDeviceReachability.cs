using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.ReachableHandling;

public interface IDeviceReachability
{
	void UpdateReachability(DeviceIdentifier remoteEndPoint, bool reachable);

	void ForceReachabilityCheck(DeviceIdentifier remoteEndPoint);
}
