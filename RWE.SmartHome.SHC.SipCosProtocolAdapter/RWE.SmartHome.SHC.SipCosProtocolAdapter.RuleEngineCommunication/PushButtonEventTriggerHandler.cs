using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DomainModel.Constants;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;

internal class PushButtonEventTriggerHandler : ITriggerCapableDeviceHandler
{
	private readonly List<BuiltinPhysicalDeviceType> supportedTypes;

	public List<BuiltinPhysicalDeviceType> DeviceTypes => supportedTypes;

	public PushButtonEventTriggerHandler()
	{
		supportedTypes = new List<BuiltinPhysicalDeviceType>
		{
			BuiltinPhysicalDeviceType.BRC8,
			BuiltinPhysicalDeviceType.WSC2,
			BuiltinPhysicalDeviceType.ISC2,
			BuiltinPhysicalDeviceType.ISS2,
			BuiltinPhysicalDeviceType.ISD2,
			BuiltinPhysicalDeviceType.ISR2
		};
	}

	public List<Property> GetTriggerProperties(BuiltinPhysicalDeviceType deviceType, SipCosSwitchCommandEventArgs eventArgs)
	{
		int pushButtonIndex = PushButtonIndexProvider.GetPushButtonIndex(deviceType, eventArgs.KeyChannelNumber);
		NumericProperty numericProperty = new NumericProperty();
		numericProperty.Name = "index";
		numericProperty.Value = pushButtonIndex;
		Property item = numericProperty;
		NumericProperty numericProperty2 = new NumericProperty();
		numericProperty2.Name = "LastPressedButtonIndex";
		numericProperty2.Value = pushButtonIndex;
		Property item2 = numericProperty2;
		StringProperty stringProperty = new StringProperty();
		stringProperty.Name = "type";
		stringProperty.Value = (eventArgs.IsLongPress ? "LongPress" : "ShortPress");
		Property item3 = stringProperty;
		NumericProperty numericProperty3 = new NumericProperty();
		numericProperty3.Name = "keyPressCounter";
		numericProperty3.Value = eventArgs.KeyStrokeCounter;
		Property item4 = numericProperty3;
		StringProperty stringProperty2 = new StringProperty();
		stringProperty2.Name = EventConstants.EventTypePropertyName;
		stringProperty2.Value = "ButtonPressed";
		Property item5 = stringProperty2;
		List<Property> list = new List<Property>();
		list.Add(item);
		list.Add(item2);
		list.Add(item3);
		list.Add(item4);
		list.Add(item5);
		return list;
	}

	public Guid GetEventSourceId(List<LogicalDevice> logicalDevices, SipCosSwitchCommandEventArgs eventArgs)
	{
		Guid result = Guid.Empty;
		if (logicalDevices.FirstOrDefault((LogicalDevice s) => s is PushButtonSensor) is PushButtonSensor pushButtonSensor)
		{
			result = pushButtonSensor.Id;
			BaseDevice baseDevice = pushButtonSensor.BaseDevice;
			if (baseDevice == null)
			{
				result = Guid.Empty;
			}
			else
			{
				int pushButtonIndex = PushButtonIndexProvider.GetPushButtonIndex(baseDevice.GetBuiltinDeviceDeviceType(), eventArgs.KeyChannelNumber);
				if (pushButtonIndex < 0 || pushButtonIndex >= pushButtonSensor.ButtonCount)
				{
					Log.Debug(Module.SipCosProtocolAdapter, "RuleEngineEvaluator", $"Invalid device/push button channel combination: {baseDevice.GetBuiltinDeviceDeviceType()}-{eventArgs.KeyChannelNumber} resulting buttonIndex: {pushButtonIndex}");
					result = Guid.Empty;
				}
			}
		}
		return result;
	}

	public List<Property> GetUITriggerProperties(BuiltinPhysicalDeviceType deviceType, int buttonIndex)
	{
		NumericProperty numericProperty = new NumericProperty();
		numericProperty.Name = "LastPressedButtonIndex";
		numericProperty.Value = buttonIndex;
		NumericProperty item = numericProperty;
		List<Property> list = new List<Property>();
		list.Add(item);
		return list;
	}
}
