using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;

public class CoreCapabilityFactory
{
	private const string SwitchActuatorName = "Switch";

	private const string DimmerActuatorName = "Dimmer Actuator";

	private const string MotionDetectionSensorName = "Motion Detector";

	private const string LuminanceSensorName = "Luminance Sensor";

	private const string PushButtonSensorName = "Push Button Sensor";

	private const string RolleShutterActuatorName = "Roller Shutter";

	private const int defaultTechnicalMinValue = 30;

	private const int defaultTechnicalMaxValue = 100;

	private const int defaultTimeFullUp = 600;

	private const int defaultTimeFullDown = 600;

	private const int defaultHuminityMoldProtection = 1;

	private const int defaultFreezeProtection = 6;

	private const int defaultWindowOpenTemperature = 6;

	private const int initialMinTemperature = 6;

	private const int initialMaxTemperature = 30;

	public List<LogicalDevice> CreateCapabilities(BaseDevice physicalDevice)
	{
		switch (physicalDevice.GetBuiltinDeviceDeviceType())
		{
		case BuiltinPhysicalDeviceType.PSS:
		case BuiltinPhysicalDeviceType.PSSO:
		case BuiltinPhysicalDeviceType.ChargingStation:
			return CreateSwitchLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.WSC2:
			return CreateWsc2LogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.BRC8:
			return CreateBrc8LogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.WMD:
		case BuiltinPhysicalDeviceType.WMDO:
			return CreateWmdLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.WDS:
			return CreateWdsLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.WSD:
		case BuiltinPhysicalDeviceType.WSD2:
			return CreateWsdLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.PSD:
			return CreatePsdLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.ISC2:
			return CreateIsc2LogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.ISS2:
			return CreateIssLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.ISD2:
			return CreateIsdLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.ISR2:
			return CreateIsr2LogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.WRT:
			return CreateWrtLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.RST:
		case BuiltinPhysicalDeviceType.RST2:
			return CreateRstLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.FSC8:
			return CreateFsc8LogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.PSR:
			return CreatePsrLogicalDevices(physicalDevice);
		case BuiltinPhysicalDeviceType.NotificationSender:
			return CreateNotificationSenderCapabilities(physicalDevice);
		case BuiltinPhysicalDeviceType.SHC:
			return CreateSHCCapabilities(physicalDevice);
		case BuiltinPhysicalDeviceType.VRCC:
			return new List<LogicalDevice>();
		case BuiltinPhysicalDeviceType.SIR:
			return CreateSIRCapabilities(physicalDevice);
		default:
			throw new InvalidOperationException(string.Concat(physicalDevice.GetBuiltinDeviceDeviceType(), " is not found in the available values"));
		}
	}

	private List<LogicalDevice> CreateSIRCapabilities(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new LogicalDevice
		{
			BaseDevice = physicalDevice,
			BaseDeviceId = physicalDevice.Id,
			DeviceType = "SirenActuator",
			Id = Guid.NewGuid(),
			Name = "Siren actuator",
			Properties = new List<Property>
			{
				new StringProperty("AlarmSoundId", "0VF"),
				new StringProperty("FeedbackSoundId", "7MF"),
				new StringProperty("NotificationSoundId", "8VS")
			},
			Version = 1,
			PrimaryPropertyName = "ActiveChannel"
		});
		list.Add(new LogicalDevice
		{
			BaseDevice = physicalDevice,
			BaseDeviceId = physicalDevice.Id,
			DeviceType = "SabotageSensor",
			Id = Guid.NewGuid(),
			Name = "Sabotage sensor",
			Properties = new List<Property>(),
			Version = 1,
			PrimaryPropertyName = "IsOn"
		});
		return list;
	}

	private List<LogicalDevice> CreateSHCCapabilities(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new LogicalDevice
		{
			BaseDevice = physicalDevice,
			BaseDeviceId = physicalDevice.Id,
			ActivityLogActive = false,
			DeviceType = "VirtualResident",
			Id = Guid.NewGuid(),
			Name = "Virtual Resident",
			Properties = new List<Property>(),
			Version = 1
		});
		list.Add(new LogicalDevice
		{
			BaseDevice = physicalDevice,
			BaseDeviceId = physicalDevice.Id,
			ActivityLogActive = false,
			DeviceType = "Calendar",
			Id = Guid.NewGuid(),
			Name = "Calendar",
			Properties = new List<Property>(),
			Version = 1
		});
		return list;
	}

	private List<LogicalDevice> CreateNotificationSenderCapabilities(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new LogicalDevice
		{
			BaseDevice = physicalDevice,
			BaseDeviceId = physicalDevice.Id,
			ActivityLogActive = false,
			DeviceType = "PushNotificationActuator",
			Id = Guid.NewGuid(),
			Name = "Push Notification Actuator",
			Properties = new List<Property>(),
			Version = 1
		});
		list.Add(new LogicalDevice
		{
			BaseDevice = physicalDevice,
			BaseDeviceId = physicalDevice.Id,
			ActivityLogActive = false,
			DeviceType = "SmsActuator",
			Id = Guid.NewGuid(),
			Name = "SMS Actuator",
			Properties = new List<Property>(),
			Version = 1
		});
		list.Add(new LogicalDevice
		{
			BaseDevice = physicalDevice,
			BaseDeviceId = physicalDevice.Id,
			ActivityLogActive = false,
			DeviceType = "EmailActuator",
			Id = Guid.NewGuid(),
			Name = "Email Actuator",
			Properties = new List<Property>(),
			Version = 1
		});
		return list;
	}

	private List<LogicalDevice> CreateSwitchLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreateSwitchActuator(physicalDevice));
		return list;
	}

	private static SwitchActuator CreateSwitchActuator(BaseDevice physicalDevice)
	{
		SwitchActuator switchActuator = new SwitchActuator();
		switchActuator.BaseDevice = physicalDevice;
		switchActuator.Name = "Switch";
		switchActuator.DeviceType = "SwitchActuator";
		return switchActuator;
	}

	private List<LogicalDevice> CreateWsc2LogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreatePushButtonSensor(physicalDevice, 2));
		return list;
	}

	private static PushButtonSensor CreatePushButtonSensor(BaseDevice physicalDevice, int buttonCount)
	{
		PushButtonSensor pushButtonSensor = new PushButtonSensor();
		pushButtonSensor.BaseDevice = physicalDevice;
		pushButtonSensor.ButtonCount = buttonCount;
		pushButtonSensor.Name = physicalDevice.DeviceType;
		pushButtonSensor.DeviceType = "PushButtonSensor";
		return pushButtonSensor;
	}

	private List<LogicalDevice> CreateBrc8LogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreatePushButtonSensor(physicalDevice, 8));
		return list;
	}

	private List<LogicalDevice> CreateWmdLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new MotionDetectionSensor
		{
			BaseDevice = physicalDevice,
			Name = "Motion Detector",
			DeviceType = "MotionDetectionSensor"
		});
		list.Add(new LuminanceSensor
		{
			BaseDevice = physicalDevice,
			Name = "Luminance Sensor",
			DeviceType = "LuminanceSensor"
		});
		return list;
	}

	private List<LogicalDevice> CreateWdsLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new WindowDoorSensor
		{
			BaseDevice = physicalDevice,
			Name = physicalDevice.DeviceType,
			DeviceType = "WindowDoorSensor"
		});
		return list;
	}

	private List<LogicalDevice> CreateWsdLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new AlarmActuator
		{
			BaseDevice = physicalDevice,
			Name = physicalDevice.DeviceType,
			DeviceType = "AlarmActuator"
		});
		list.Add(new SmokeDetectorSensor
		{
			BaseDevice = physicalDevice,
			Name = physicalDevice.DeviceType,
			DeviceType = "SmokeDetectorSensor"
		});
		return list;
	}

	private List<LogicalDevice> CreatePsdLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreateDimmerActuator(physicalDevice));
		return list;
	}

	private static DimmerActuator CreateDimmerActuator(BaseDevice physicalDevice)
	{
		DimmerActuator dimmerActuator = new DimmerActuator();
		dimmerActuator.BaseDevice = physicalDevice;
		dimmerActuator.Name = "Dimmer Actuator";
		dimmerActuator.TechnicalMinValue = 30;
		dimmerActuator.TechnicalMaxValue = 100;
		dimmerActuator.DeviceType = "DimmerActuator";
		return dimmerActuator;
	}

	private List<LogicalDevice> CreateIsc2LogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreatePushButtonSensor(physicalDevice, 2));
		return list;
	}

	private List<LogicalDevice> CreateIssLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreateSwitchActuator(physicalDevice));
		list.Add(CreatePushButtonSensor(physicalDevice, 2));
		return list;
	}

	private List<LogicalDevice> CreateIsdLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreateDimmerActuator(physicalDevice));
		list.Add(CreatePushButtonSensor(physicalDevice, 2));
		return list;
	}

	private List<LogicalDevice> CreateIsr2LogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new RollerShutterActuator
		{
			BaseDevice = physicalDevice,
			Name = "Roller Shutter",
			TimeFullDown = 600,
			TimeFullUp = 600,
			IsCalibrating = false,
			DeviceType = "RollerShutterActuator"
		});
		list.Add(CreatePushButtonSensor(physicalDevice, 2));
		return list;
	}

	private List<LogicalDevice> CreateWrtLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreateThermostatActuator(physicalDevice));
		list.Add(CreateTemperatureSensor(physicalDevice));
		list.Add(CreateHumiditySensor(physicalDevice));
		return list;
	}

	private List<LogicalDevice> CreateRstLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(CreateThermostatActuator(physicalDevice));
		list.Add(CreateTemperatureSensor(physicalDevice));
		list.Add(CreateHumiditySensor(physicalDevice));
		return list;
	}

	private LogicalDevice CreateHumiditySensor(BaseDevice bd)
	{
		HumiditySensor humiditySensor = new HumiditySensor();
		humiditySensor.BaseDevice = bd;
		humiditySensor.Name = "Humidity Level";
		humiditySensor.IsMoldProtectionActivated = false;
		humiditySensor.HumidityMoldProtection = 1m;
		humiditySensor.DeviceType = "HumiditySensor";
		HumiditySensor humiditySensor2 = humiditySensor;
		humiditySensor2.Properties.Add(new StringProperty("VRCCHumidity", humiditySensor2.PrimaryPropertyName));
		return humiditySensor2;
	}

	private LogicalDevice CreateTemperatureSensor(BaseDevice bd)
	{
		TemperatureSensor temperatureSensor = new TemperatureSensor();
		temperatureSensor.BaseDevice = bd;
		temperatureSensor.Name = "Actual Temperature";
		temperatureSensor.IsFreezeProtectionActivated = false;
		temperatureSensor.FreezeProtection = 6m;
		temperatureSensor.DeviceType = "TemperatureSensor";
		TemperatureSensor temperatureSensor2 = temperatureSensor;
		temperatureSensor2.Properties.Add(new StringProperty("VRCCTemperature", temperatureSensor2.PrimaryPropertyName));
		return temperatureSensor2;
	}

	private LogicalDevice CreateThermostatActuator(BaseDevice bd)
	{
		ThermostatActuator thermostatActuator = new ThermostatActuator();
		thermostatActuator.BaseDevice = bd;
		thermostatActuator.Name = "Target Temperature";
		thermostatActuator.MaxTemperature = 30m;
		thermostatActuator.MinTemperature = 6m;
		thermostatActuator.ChildLock = false;
		thermostatActuator.WindowOpenTemperature = 6m;
		thermostatActuator.DeviceType = "ThermostatActuator";
		ThermostatActuator thermostatActuator2 = thermostatActuator;
		thermostatActuator2.Properties.Add(new StringProperty("VRCCSetPoint", thermostatActuator2.PrimaryPropertyName));
		return thermostatActuator2;
	}

	private List<LogicalDevice> CreatePsrLogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new Router
		{
			BaseDevice = physicalDevice,
			Name = physicalDevice.DeviceType,
			DeviceType = "Router"
		});
		return list;
	}

	private List<LogicalDevice> CreateFsc8LogicalDevices(BaseDevice physicalDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		for (int i = 1; i <= 8; i++)
		{
			list.Add(new ValveActuator
			{
				BaseDevice = physicalDevice,
				Name = physicalDevice.DeviceType,
				ValveIndex = i,
				ValveType = ValveType.NormalClose,
				DeviceType = "ValveActuator"
			});
		}
		return list;
	}
}
