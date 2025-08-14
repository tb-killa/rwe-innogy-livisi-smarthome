using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DomainModel.Constants;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;

internal class WDSEventTriggerHandler : ITriggerCapableDeviceHandler
{
	private const byte wdsChannel = 1;

	private readonly List<BuiltinPhysicalDeviceType> supportedTypes;

	public List<BuiltinPhysicalDeviceType> DeviceTypes => supportedTypes;

	public WDSEventTriggerHandler()
	{
		supportedTypes = new List<BuiltinPhysicalDeviceType> { BuiltinPhysicalDeviceType.WDS };
	}

	public List<Property> GetTriggerProperties(BuiltinPhysicalDeviceType deviceType, SipCosSwitchCommandEventArgs eventArgs)
	{
		List<Property> list = new List<Property>();
		list.Add(new BooleanProperty
		{
			Name = "IsOpen",
			Value = Convert.ToBoolean(Math.Round((double)Convert.ToInt32(eventArgs.DecisionValue) * 100.0 / 255.0))
		});
		list.Add(new StringProperty
		{
			Name = EventConstants.EventTypePropertyName,
			Value = "StateChanged"
		});
		return list;
	}

	public Guid GetEventSourceId(List<LogicalDevice> logicalDevices, SipCosSwitchCommandEventArgs eventArgs)
	{
		Guid result = Guid.Empty;
		if (eventArgs.KeyChannelNumber == 1)
		{
			LogicalDevice logicalDevice = logicalDevices.FirstOrDefault((LogicalDevice s) => s is WindowDoorSensor);
			if (logicalDevice != null)
			{
				result = logicalDevice.Id;
			}
		}
		return result;
	}

	public List<Property> GetUITriggerProperties(BuiltinPhysicalDeviceType deviceType, int buttonIndex)
	{
		return new List<Property>();
	}
}
