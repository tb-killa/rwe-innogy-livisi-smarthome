using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class FactoryResetOfDeviceDetectedEventArgs
{
	public Guid DeviceId { get; set; }

	public bool IsUpdatingFirmware { get; set; }
}
