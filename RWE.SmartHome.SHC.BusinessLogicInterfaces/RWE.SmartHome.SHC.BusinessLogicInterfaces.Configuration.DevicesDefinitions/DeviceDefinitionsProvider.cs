using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class DeviceDefinitionsProvider : IDeviceDefinitionsProvider
{
	private readonly List<BaseDeviceDefinition> deviceDefinitions = new List<BaseDeviceDefinition>();

	public DeviceDefinitionsProvider()
	{
		deviceDefinitions.AddRange(new BaseDeviceDefinition[9]
		{
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.FSC8.ToString(),
				MinVersion = null,
				MaxVersion = new SipcosFirmwareVersion(18),
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.0"),
					new StringPropertyDefinition("Desc", "/desc/device/FSC8.RWE/1.0")
				},
				ConfigurationProperties = new List<PropertyDefinition>(),
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "ValveActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("ValveIndex", null),
							new StringPropertyDefinition("ValveType", "NormalClose")
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.FSC8.ToString(),
				MinVersion = new SipcosFirmwareVersion(18),
				MaxVersion = null,
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.1"),
					new StringPropertyDefinition("Desc", "/desc/device/FSC8.RWE/1.1")
				},
				ConfigurationProperties = new List<PropertyDefinition>(),
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "ValveActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("ValveIndex", null),
							new StringPropertyDefinition("ValveType", "NormalClose"),
							new StringPropertyDefinition("ControlMode", "Heating")
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.ISS2.ToString(),
				MinVersion = null,
				MaxVersion = new SipcosFirmwareVersion(17),
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.0"),
					new StringPropertyDefinition("Desc", "/desc/device/ISS2.RWE/1.0")
				},
				ConfigurationProperties = new List<PropertyDefinition>(),
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "SwitchActuator",
						ConfigurationProperties = new List<PropertyDefinition>()
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "PushButtonSensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("PushButtons", 2m)
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.ISS2.ToString(),
				MinVersion = new SipcosFirmwareVersion(17),
				MaxVersion = null,
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.1"),
					new StringPropertyDefinition("Desc", "/desc/device/ISS2.RWE/1.1")
				},
				ConfigurationProperties = new List<PropertyDefinition>(),
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "SwitchActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new StringPropertyDefinition("SensingBehavior", "Enabled")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "PushButtonSensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("PushButtons", 2m)
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.RST.ToString(),
				MinVersion = null,
				MaxVersion = new SipcosFirmwareVersion(25),
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.0"),
					new StringPropertyDefinition("Desc", "/desc/device/RST.RWE/1.0")
				},
				ConfigurationProperties = new List<PropertyDefinition>(),
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "ThermostatActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("MaxTemperature", 30m),
							new NumericPropertyDefinition("MinTemperature", 6m),
							new BooleanPropertyDefinition("ChildLock", false),
							new NumericPropertyDefinition("WindowOpenTemperature", 6m),
							new StringPropertyDefinition("VRCCSetPoint", "PointTemperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "TemperatureSensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsFreezeProtectionActivated", false),
							new NumericPropertyDefinition("FreezeProtection", 6m),
							new StringPropertyDefinition("VRCCTemperature", "Temperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "HumiditySensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsMoldProtectionActivated", false),
							new NumericPropertyDefinition("HumidityMoldProtection", 1m),
							new StringPropertyDefinition("VRCCHumidity", "Humidity")
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.RST.ToString(),
				MinVersion = new SipcosFirmwareVersion(25),
				MaxVersion = null,
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.1"),
					new StringPropertyDefinition("Desc", "/desc/device/RST.RWE/1.1")
				},
				ConfigurationProperties = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("DisplayCurrentTemperature", "TargetTemperature")
				},
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "ThermostatActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("MaxTemperature", 30m),
							new NumericPropertyDefinition("MinTemperature", 6m),
							new BooleanPropertyDefinition("ChildLock", false),
							new NumericPropertyDefinition("WindowOpenTemperature", 6m),
							new StringPropertyDefinition("VRCCSetPoint", "PointTemperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "TemperatureSensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsFreezeProtectionActivated", false),
							new NumericPropertyDefinition("FreezeProtection", 6m),
							new StringPropertyDefinition("VRCCTemperature", "Temperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "HumiditySensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsMoldProtectionActivated", false),
							new NumericPropertyDefinition("HumidityMoldProtection", 1m),
							new StringPropertyDefinition("VRCCHumidity", "Humidity")
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.RST2.ToString(),
				MinVersion = new SipcosFirmwareVersion(25),
				MaxVersion = null,
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.0"),
					new StringPropertyDefinition("Desc", "/desc/device/RST2.RWE/1.0")
				},
				ConfigurationProperties = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("DisplayCurrentTemperature", "TargetTemperature")
				},
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "ThermostatActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("MaxTemperature", 30m),
							new NumericPropertyDefinition("MinTemperature", 6m),
							new BooleanPropertyDefinition("ChildLock", false),
							new NumericPropertyDefinition("WindowOpenTemperature", 6m),
							new StringPropertyDefinition("VRCCSetPoint", "PointTemperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "TemperatureSensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsFreezeProtectionActivated", false),
							new NumericPropertyDefinition("FreezeProtection", 6m),
							new StringPropertyDefinition("VRCCTemperature", "Temperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "HumiditySensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsMoldProtectionActivated", false),
							new NumericPropertyDefinition("HumidityMoldProtection", 1m),
							new StringPropertyDefinition("VRCCHumidity", "Humidity")
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.WRT.ToString(),
				MinVersion = null,
				MaxVersion = new SipcosFirmwareVersion(25),
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.0"),
					new StringPropertyDefinition("Desc", "/desc/device/WRT.RWE/1.0")
				},
				ConfigurationProperties = new List<PropertyDefinition>(),
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "ThermostatActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("MaxTemperature", 30m),
							new NumericPropertyDefinition("MinTemperature", 6m),
							new BooleanPropertyDefinition("ChildLock", false),
							new NumericPropertyDefinition("WindowOpenTemperature", 6m),
							new StringPropertyDefinition("VRCCSetPoint", "PointTemperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "TemperatureSensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsFreezeProtectionActivated", false),
							new NumericPropertyDefinition("FreezeProtection", 6m),
							new StringPropertyDefinition("VRCCTemperature", "Temperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "HumiditySensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsMoldProtectionActivated", false),
							new NumericPropertyDefinition("HumidityMoldProtection", 1m),
							new StringPropertyDefinition("VRCCHumidity", "Humidity")
						}
					}
				}
			},
			new BaseDeviceDefinition
			{
				DeviceType = BuiltinPhysicalDeviceType.WRT.ToString(),
				MinVersion = new SipcosFirmwareVersion(25),
				MaxVersion = null,
				Attributes = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("Version", "1.1"),
					new StringPropertyDefinition("Desc", "/desc/device/WRT.RWE/1.1")
				},
				ConfigurationProperties = new List<PropertyDefinition>
				{
					new StringPropertyDefinition("DisplayCurrentTemperature", "TargetTemperature")
				},
				LogicalDevices = new List<LogicalDeviceDefinition>
				{
					new LogicalDeviceDefinition
					{
						DeviceType = "ThermostatActuator",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new NumericPropertyDefinition("MaxTemperature", 30m),
							new NumericPropertyDefinition("MinTemperature", 6m),
							new BooleanPropertyDefinition("ChildLock", false),
							new NumericPropertyDefinition("WindowOpenTemperature", 6m),
							new StringPropertyDefinition("VRCCSetPoint", "PointTemperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "TemperatureSensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsFreezeProtectionActivated", false),
							new NumericPropertyDefinition("FreezeProtection", 6m),
							new StringPropertyDefinition("VRCCTemperature", "Temperature")
						}
					},
					new LogicalDeviceDefinition
					{
						DeviceType = "HumiditySensor",
						ConfigurationProperties = new List<PropertyDefinition>
						{
							new BooleanPropertyDefinition("IsMoldProtectionActivated", false),
							new NumericPropertyDefinition("HumidityMoldProtection", 1m),
							new StringPropertyDefinition("VRCCHumidity", "Humidity")
						}
					}
				}
			}
		});
	}

	public BaseDeviceDefinition GetDeviceDefinition(string appId, string deviceType, FirmwareVersion firmwareVersion)
	{
		return deviceDefinitions.FirstOrDefault((BaseDeviceDefinition def) => def.DeviceType == deviceType && def.MatchFirmware(firmwareVersion));
	}

	public IEnumerable<LogicalDeviceDefinition> GetLogicalDeviceDefinition(BaseDevice physicalDevice)
	{
		return deviceDefinitions.FirstOrDefault((BaseDeviceDefinition def) => def.DeviceType == physicalDevice.DeviceType && def.Version == physicalDevice.DeviceVersion)?.LogicalDevices;
	}
}
