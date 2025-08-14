using System;
using SmartHome.SHC.API.Control;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface ITargetStateHandler
{
	ControlMessage HandleLogicalState(Guid capabilityId, CapabilityState state);
}
