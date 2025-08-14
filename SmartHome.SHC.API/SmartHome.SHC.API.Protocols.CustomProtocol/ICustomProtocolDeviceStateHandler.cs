using System;
using SmartHome.SHC.API.Control;

namespace SmartHome.SHC.API.Protocols.CustomProtocol;

public interface ICustomProtocolDeviceStateHandler
{
	event EventHandler<DeviceStateChangedEventArgs> PhysicalStateChanged;

	DeviceState GetPhysicalState(Guid id);
}
