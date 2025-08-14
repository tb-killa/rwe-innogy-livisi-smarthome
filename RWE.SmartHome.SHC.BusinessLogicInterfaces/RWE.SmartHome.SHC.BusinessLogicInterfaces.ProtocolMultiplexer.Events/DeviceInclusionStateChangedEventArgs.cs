using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceInclusionStateChangedEventArgs : EventArgs
{
	public Guid DeviceId { get; private set; }

	public DeviceInclusionState DeviceInclusionState { get; private set; }

	public string ProtocolType { get; private set; }

	public DeviceInclusionStateChangedEventArgs(Guid deviceId, DeviceInclusionState deviceInclusionState, string protocolType)
	{
		DeviceId = deviceId;
		DeviceInclusionState = deviceInclusionState;
		ProtocolType = protocolType;
	}
}
