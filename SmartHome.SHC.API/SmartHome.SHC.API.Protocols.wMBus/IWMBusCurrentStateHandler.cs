using System;

namespace SmartHome.SHC.API.Protocols.wMBus;

public interface IWMBusCurrentStateHandler
{
	PhysicalStateTransformationResult HandlePhysicalState(Guid physicalDeviceId, WMBusDeviceState state);
}
