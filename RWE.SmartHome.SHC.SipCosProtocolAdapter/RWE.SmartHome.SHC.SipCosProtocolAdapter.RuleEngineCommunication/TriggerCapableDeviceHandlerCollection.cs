using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;

public class TriggerCapableDeviceHandlerCollection : ITriggerCapableDeviceHandlerCollection
{
	private readonly Dictionary<BuiltinPhysicalDeviceType, ITriggerCapableDeviceHandler> triggerCapableDevices;

	public TriggerCapableDeviceHandlerCollection()
	{
		triggerCapableDevices = new Dictionary<BuiltinPhysicalDeviceType, ITriggerCapableDeviceHandler>();
		AddTriggerCapableDevice(new MotionDetectorEventTriggerHandler());
		AddTriggerCapableDevice(new PushButtonEventTriggerHandler());
	}

	public void AddTriggerCapableDevice(ITriggerCapableDeviceHandler handler)
	{
		handler.DeviceTypes.ForEach(delegate(BuiltinPhysicalDeviceType s)
		{
			triggerCapableDevices.Add(s, handler);
		});
	}

	public ITriggerCapableDeviceHandler GetDeviceHandler(BuiltinPhysicalDeviceType deviceType)
	{
		if (triggerCapableDevices.ContainsKey(deviceType))
		{
			return triggerCapableDevices[deviceType];
		}
		return null;
	}
}
