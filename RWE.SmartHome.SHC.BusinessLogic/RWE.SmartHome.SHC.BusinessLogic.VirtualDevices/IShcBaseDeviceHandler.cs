using System;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public interface IShcBaseDeviceHandler
{
	Guid? DeviceId { get; }

	void UpdateDiscoveryActiveProperty(bool value);
}
