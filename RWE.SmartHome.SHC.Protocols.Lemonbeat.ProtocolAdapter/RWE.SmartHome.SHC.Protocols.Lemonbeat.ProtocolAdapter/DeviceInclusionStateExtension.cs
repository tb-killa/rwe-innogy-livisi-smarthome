using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

public static class DeviceInclusionStateExtension
{
	public static DeviceInclusionState ToProtocolMultiplexerState(this LemonbeatDeviceInclusionState deviceInclusionState)
	{
		switch (deviceInclusionState)
		{
		case LemonbeatDeviceInclusionState.Found:
		case LemonbeatDeviceInclusionState.PublicKeyReceived:
			return DeviceInclusionState.Found;
		case LemonbeatDeviceInclusionState.InclusionPending:
		case LemonbeatDeviceInclusionState.InclusionInProgress:
			return DeviceInclusionState.InclusionPending;
		case LemonbeatDeviceInclusionState.Included:
			return DeviceInclusionState.Included;
		case LemonbeatDeviceInclusionState.ExclusionPending:
			return DeviceInclusionState.ExclusionPending;
		case LemonbeatDeviceInclusionState.FactoryReset:
			return DeviceInclusionState.FactoryReset;
		default:
			return DeviceInclusionState.None;
		}
	}
}
