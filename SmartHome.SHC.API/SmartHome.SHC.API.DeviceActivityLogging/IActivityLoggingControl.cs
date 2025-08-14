using System;

namespace SmartHome.SHC.API.DeviceActivityLogging;

public interface IActivityLoggingControl
{
	DeviceActivityLoggingType GetDeviceLoggingType(Guid deviceId);
}
