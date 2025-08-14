using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public interface IProtocolSpecificDeviceUpdater
{
	event EventHandler<UpdateFailedEventArgs> UpdateFailed;

	List<DeviceUpdateInfo> GetDeviceInfo();

	DeviceUpdateInfo GetDeviceInfo(Guid deviceId);

	void EnqueueFirmwareTransfer(Guid deviceId, DeviceFirmwareDescriptor firmware);

	void AbortUpdate(Guid deviceId);

	void PerformUpdate(List<Guid> deviceIdList);
}
