using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IDeviceUpdateClient
{
	DeviceUpdateResultCode CheckForDeviceUpdate(DeviceDescriptor deviceDescriptor, out DeviceUpdateInfo updateInfo);
}
