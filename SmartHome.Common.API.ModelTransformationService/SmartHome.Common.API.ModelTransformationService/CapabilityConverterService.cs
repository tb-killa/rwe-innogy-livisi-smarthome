using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.CapabilityConverters;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class CapabilityConverterService : ICapabilityConverterService
{
	private static Dictionary<string, ICapabilityConverter> capabilityConverters;

	private static Dictionary<string, ICapabilityConverter> capabilityStateConverters;

	private readonly IConversionContext context;

	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(CapabilityConverterService));

	static CapabilityConverterService()
	{
		capabilityConverters = new Dictionary<string, ICapabilityConverter>(new InvariantEqualityComparer())
		{
			{
				typeof(LogicalDevice).Name,
				new BaseCapabilityConverter()
			},
			{
				typeof(SwitchActuator).Name,
				new SwitchActuatorConverter()
			},
			{
				typeof(WindowDoorSensor).Name,
				new WindowDoorSensorConverter()
			},
			{
				typeof(PushButtonSensor).Name,
				new PushButtonSensorConverter()
			},
			{
				typeof(MotionDetectionSensor).Name,
				new MotionDetectionSensorConverter()
			},
			{
				typeof(LuminanceSensor).Name,
				new LuminanceSensorConverter()
			},
			{
				typeof(DimmerActuator).Name,
				new DimmerActuatorConverter()
			},
			{
				typeof(AlarmActuator).Name,
				new AlarmActuatorConverter()
			},
			{
				typeof(SmokeDetectorSensor).Name,
				new SmokeDetectorSensorConverter()
			},
			{
				typeof(RollerShutterActuator).Name,
				new RollerShutterActuatorConverter()
			},
			{
				typeof(Router).Name,
				new RouterConverter()
			},
			{
				typeof(ThermostatActuator).Name,
				new ThermostatActuatorConverter()
			},
			{
				typeof(ValveActuator).Name,
				new ValveActuatorConverter()
			},
			{
				typeof(TemperatureSensor).Name,
				new TemperatureSensorConverter()
			},
			{
				typeof(HumiditySensor).Name,
				new HumiditySensorConverter()
			},
			{
				typeof(RoomSetpoint).Name,
				new RoomSetpointConverter()
			},
			{
				typeof(RoomTemperature).Name,
				new RoomTemperatureConverter()
			},
			{
				typeof(RoomHumidity).Name,
				new RoomHumidityConverter()
			}
		};
		capabilityStateConverters = new Dictionary<string, ICapabilityConverter>(new InvariantEqualityComparer())
		{
			{
				typeof(LogicalDeviceState).Name,
				new BaseCapabilityConverter()
			},
			{
				typeof(SwitchActuatorState).Name,
				new SwitchActuatorConverter()
			},
			{
				typeof(WindowDoorSensorState).Name,
				new WindowDoorSensorConverter()
			},
			{
				typeof(LuminanceSensorState).Name,
				new LuminanceSensorConverter()
			},
			{
				typeof(DimmerActuatorState).Name,
				new DimmerActuatorConverter()
			},
			{
				typeof(AlarmActuatorState).Name,
				new AlarmActuatorConverter()
			},
			{
				typeof(SmokeDetectionSensorState).Name,
				new SmokeDetectorSensorConverter()
			},
			{
				typeof(RollerShutterActuatorState).Name,
				new RollerShutterActuatorConverter()
			},
			{
				typeof(ThermostatActuatorState).Name,
				new ThermostatActuatorConverter()
			},
			{
				typeof(TemperatureSensorState).Name,
				new TemperatureSensorConverter()
			},
			{
				typeof(HumiditySensorState).Name,
				new HumiditySensorConverter()
			},
			{
				typeof(GenericDeviceState).Name,
				new BaseCapabilityConverter()
			}
		};
	}

	public CapabilityConverterService(IConversionContext context)
	{
		this.context = context;
	}

	public Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice)
	{
		if (capabilityConverters.ContainsKey(logicalDevice.GetType().Name))
		{
			return capabilityConverters[logicalDevice.GetType().Name].FromSmartHomeLogicalDevice(logicalDevice, context);
		}
		logger.Error($"Unknown SmartHome logical device: {logicalDevice.GetType()}");
		UnknownCapability unknownCapability = new UnknownCapability();
		unknownCapability.Device = string.Format("/device/{0}", logicalDevice.BaseDeviceId.ToString("N"));
		return new UnknownCapability();
	}

	public LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		logger.DebugEnterMethod("ToSmartHomeLogicalDevice");
		if (!aCapability.Device.Any())
		{
			logger.LogAndThrow<ArgumentException>($"Device link not provided for capability {aCapability.Id}");
		}
		string device = aCapability.Device;
		string text = null;
		BaseDevice baseDevice = context.Devices.FirstOrDefault((BaseDevice d) => d.LogicalDeviceIds.Any((Guid id) => id == aCapability.Id.GetGuid()));
		if (baseDevice == null)
		{
			logger.Info($"Device with id {device} not found in SH config for capability with id {aCapability.Id}");
			return null;
		}
		text = baseDevice.AppId;
		if (capabilityConverters.ContainsKey(aCapability.Type) && text == CoreConstants.CoreAppId)
		{
			logger.Debug($"Converting core API Capability with Type: {aCapability.Type}");
			return capabilityConverters[aCapability.Type].ToSmartHomeLogicalDevice(aCapability);
		}
		logger.Debug($"Generic SmartHome logical device: {aCapability.Type}");
		return capabilityConverters[typeof(LogicalDevice).Name].ToSmartHomeLogicalDevice(aCapability);
	}

	public List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state)
	{
		if (state == null)
		{
			return null;
		}
		if (capabilityStateConverters.ContainsKey(state.GetType().Name))
		{
			return capabilityStateConverters[state.GetType().Name].FromSmartHomeLogicalDeviceState(state, context);
		}
		logger.LogAndThrow<ArgumentException>($"Unsupported logical device state of type {state.GetType().Name}");
		return null;
	}
}
