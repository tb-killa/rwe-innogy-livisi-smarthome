using System;
using SmartHome.SHC.API.Control;

namespace SmartHome.SHC.API.Protocols.CustomProtocol;

public interface ICustomProtocolCapabilityStateHandler
{
	event EventHandler<CapabilityStateChangedEventArgs> StateChanged;

	CapabilityState GetState(Guid deviceId);
}
