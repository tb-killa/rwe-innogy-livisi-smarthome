using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

public interface IMulticastController
{
	Guid SendMultiCastUnconditionalSwitch(IEnumerable<Guid> targetDeviceIds, byte[] sourceIp, byte sourceChannel, bool longPress, byte keystrokeCounter);

	Guid SendMultiCastConditionalSwitch(IEnumerable<Guid> targetDeviceIds, byte[] sourceIp, byte sourceChannel, bool longPress, byte keystrokeCounter, byte decisionValue);
}
