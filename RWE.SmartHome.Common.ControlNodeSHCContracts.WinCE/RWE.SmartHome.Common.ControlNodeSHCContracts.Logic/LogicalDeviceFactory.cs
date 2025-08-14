using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Logic;

public class LogicalDeviceFactory
{
	public List<LogicalDevice> CreateLogicalDevices(BaseDevice physicalDevice)
	{
		if (physicalDevice.AppId == CoreConstants.CoreAppId)
		{
			switch (physicalDevice.GetBuiltinDeviceDeviceType())
			{
			case BuiltinPhysicalDeviceType.PSS:
			case BuiltinPhysicalDeviceType.PSSO:
			case BuiltinPhysicalDeviceType.ChargingStation:
			{
				List<LogicalDevice> list15 = new List<LogicalDevice>();
				list15.Add(new SwitchActuator
				{
					BaseDevice = physicalDevice
				});
				return list15;
			}
			case BuiltinPhysicalDeviceType.WSC2:
			{
				List<LogicalDevice> list14 = new List<LogicalDevice>();
				list14.Add(new PushButtonSensor
				{
					BaseDevice = physicalDevice,
					ButtonCount = 2
				});
				return list14;
			}
			case BuiltinPhysicalDeviceType.BRC8:
			{
				List<LogicalDevice> list13 = new List<LogicalDevice>();
				list13.Add(new PushButtonSensor
				{
					BaseDevice = physicalDevice,
					ButtonCount = 8
				});
				return list13;
			}
			case BuiltinPhysicalDeviceType.WMD:
			case BuiltinPhysicalDeviceType.WMDO:
			{
				List<LogicalDevice> list12 = new List<LogicalDevice>();
				list12.Add(new MotionDetectionSensor
				{
					BaseDevice = physicalDevice
				});
				list12.Add(new LuminanceSensor
				{
					BaseDevice = physicalDevice
				});
				return list12;
			}
			case BuiltinPhysicalDeviceType.WDS:
			{
				List<LogicalDevice> list11 = new List<LogicalDevice>();
				list11.Add(new WindowDoorSensor
				{
					BaseDevice = physicalDevice
				});
				return list11;
			}
			case BuiltinPhysicalDeviceType.WSD:
			{
				List<LogicalDevice> list10 = new List<LogicalDevice>();
				list10.Add(new AlarmActuator
				{
					BaseDevice = physicalDevice
				});
				list10.Add(new SmokeDetectorSensor
				{
					BaseDevice = physicalDevice
				});
				return list10;
			}
			case BuiltinPhysicalDeviceType.PSD:
			{
				List<LogicalDevice> list9 = new List<LogicalDevice>();
				list9.Add(new DimmerActuator
				{
					BaseDevice = physicalDevice
				});
				return list9;
			}
			case BuiltinPhysicalDeviceType.ISC2:
			{
				List<LogicalDevice> list8 = new List<LogicalDevice>();
				list8.Add(new PushButtonSensor
				{
					BaseDevice = physicalDevice,
					ButtonCount = 2
				});
				return list8;
			}
			case BuiltinPhysicalDeviceType.ISS2:
			{
				List<LogicalDevice> list7 = new List<LogicalDevice>();
				list7.Add(new SwitchActuator
				{
					BaseDevice = physicalDevice
				});
				list7.Add(new PushButtonSensor
				{
					BaseDevice = physicalDevice,
					ButtonCount = 2
				});
				return list7;
			}
			case BuiltinPhysicalDeviceType.ISD2:
			{
				List<LogicalDevice> list6 = new List<LogicalDevice>();
				list6.Add(new DimmerActuator
				{
					BaseDevice = physicalDevice
				});
				list6.Add(new PushButtonSensor
				{
					BaseDevice = physicalDevice,
					ButtonCount = 2
				});
				return list6;
			}
			case BuiltinPhysicalDeviceType.ISR2:
			{
				List<LogicalDevice> list5 = new List<LogicalDevice>();
				list5.Add(new RollerShutterActuator
				{
					BaseDevice = physicalDevice
				});
				list5.Add(new PushButtonSensor
				{
					BaseDevice = physicalDevice,
					ButtonCount = 2
				});
				return list5;
			}
			case BuiltinPhysicalDeviceType.WRT:
			{
				List<LogicalDevice> list4 = new List<LogicalDevice>();
				list4.Add(new ThermostatActuator
				{
					BaseDevice = physicalDevice
				});
				list4.Add(new TemperatureSensor
				{
					BaseDevice = physicalDevice
				});
				list4.Add(new HumiditySensor
				{
					BaseDevice = physicalDevice
				});
				return list4;
			}
			case BuiltinPhysicalDeviceType.RST:
			{
				List<LogicalDevice> list3 = new List<LogicalDevice>();
				list3.Add(new ThermostatActuator
				{
					BaseDevice = physicalDevice
				});
				list3.Add(new TemperatureSensor
				{
					BaseDevice = physicalDevice
				});
				list3.Add(new HumiditySensor
				{
					BaseDevice = physicalDevice
				});
				return list3;
			}
			case BuiltinPhysicalDeviceType.FSC8:
			{
				List<LogicalDevice> list2 = new List<LogicalDevice>();
				list2.Add(new ValveActuator
				{
					BaseDevice = physicalDevice
				});
				return list2;
			}
			case BuiltinPhysicalDeviceType.PSR:
			{
				List<LogicalDevice> list = new List<LogicalDevice>();
				list.Add(new Router
				{
					BaseDevice = physicalDevice
				});
				return list;
			}
			default:
				throw new InvalidOperationException();
			}
		}
		return null;
	}
}
