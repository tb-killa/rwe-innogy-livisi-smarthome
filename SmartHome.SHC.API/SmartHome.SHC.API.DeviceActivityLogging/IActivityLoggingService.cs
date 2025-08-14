using System;

namespace SmartHome.SHC.API.DeviceActivityLogging;

public interface IActivityLoggingService
{
	DeviceActivityLoggingType GetLoggingState(Guid deviceId);
}
