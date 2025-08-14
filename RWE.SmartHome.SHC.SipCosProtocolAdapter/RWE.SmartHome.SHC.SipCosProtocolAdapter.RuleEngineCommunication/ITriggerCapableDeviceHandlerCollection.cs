using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;

public interface ITriggerCapableDeviceHandlerCollection
{
	void AddTriggerCapableDevice(ITriggerCapableDeviceHandler handler);

	ITriggerCapableDeviceHandler GetDeviceHandler(BuiltinPhysicalDeviceType deviceType);
}
