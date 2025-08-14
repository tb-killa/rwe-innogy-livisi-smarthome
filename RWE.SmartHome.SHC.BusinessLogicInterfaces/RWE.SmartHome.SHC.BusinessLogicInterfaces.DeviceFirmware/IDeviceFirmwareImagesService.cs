using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

public interface IDeviceFirmwareImagesService : IDeviceFirmwareRepository, IDeviceUpdateClient
{
	void CheckDeviceUpdate(DeviceDescriptor deviceDescriptor);

	void RemoveUnneededImages(List<DeviceDescriptor> neededFirmwareImages);
}
