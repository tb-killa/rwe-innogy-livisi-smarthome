using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;

public interface ITriggerCapableDeviceHandler
{
	List<BuiltinPhysicalDeviceType> DeviceTypes { get; }

	List<Property> GetTriggerProperties(BuiltinPhysicalDeviceType deviceType, SipCosSwitchCommandEventArgs eventArgs);

	Guid GetEventSourceId(List<LogicalDevice> logicalDevices, SipCosSwitchCommandEventArgs eventArgs);

	List<Property> GetUITriggerProperties(BuiltinPhysicalDeviceType deviceType, int buttonIndex);
}
