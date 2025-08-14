using System;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

public interface ICosIPFirmwareUpdateController
{
	void SendStartUpdate(Guid physicalDeviceId, byte firmwareVersion, uint firmwareImageSize);

	void SendFirmwareUpdateData(Guid physicalDeviceId, ushort sequenceNumber, byte[] firmwareData);

	void SendEndUpdate(Guid physicalDeviceId);

	void SendDoUpdate(Guid physicalDeviceId);

	void RemoveUpdatePackagesForDevice(Guid physicalDeviceId);

	void EnableDutyCycle(Guid physicalDeviceId);

	bool DeviceSupportsDutyCycle(IDeviceInformation deviceInformation);
}
