using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivityInterfaces;

public interface IDeviceActivityLogger : IService
{
	void DeleteDeviceActivityData();

	void FlushPendingData();
}
