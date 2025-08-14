using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class DeviceUpdateClientLocalOnly : IDeviceUpdateClient
{
	public DeviceUpdateResultCode CheckForDeviceUpdate(DeviceDescriptor deviceDescriptor, out DeviceUpdateInfo updateInfo)
	{
		updateInfo = new DeviceUpdateInfo();
		return DeviceUpdateResultCode.AlreadyLatestVersion;
	}
}
