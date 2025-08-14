using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

public interface IDeviceFirmwareManager : IService
{
	void PerformUpdate(List<Guid> deviceList);

	void RegisterUpdater(IProtocolSpecificDeviceUpdater updater);
}
