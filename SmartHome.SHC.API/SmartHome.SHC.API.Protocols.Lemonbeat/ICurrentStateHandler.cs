using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface ICurrentStateHandler
{
	PhysicalStateTransformationResult HandlePhysicalState(Guid physicalDeviceId, PhysicalDeviceState state);
}
