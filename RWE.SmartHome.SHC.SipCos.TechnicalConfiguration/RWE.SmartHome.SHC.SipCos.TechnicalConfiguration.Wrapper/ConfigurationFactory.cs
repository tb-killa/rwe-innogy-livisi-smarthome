using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper;

public static class ConfigurationFactory
{
	public static TechnicalConfigurationCreator GetConfigurationCreator(LogicalDevice logicalDevice, byte[] deviceAddress)
	{
		TechnicalConfigurationCreator result = null;
		byte[] array = deviceAddress;
		if (array == null)
		{
			array = new byte[3];
		}
		if (logicalDevice is SwitchActuator)
		{
			result = new SwitchConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is DimmerActuator)
		{
			result = new DimmerConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is PushButtonSensor)
		{
			result = new PushButtonSensorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is WindowDoorSensor)
		{
			result = new WindowDoorSensorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is MotionDetectionSensor)
		{
			result = new MotionDetectorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is LuminanceSensor)
		{
			result = new LuminanceSensorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is SmokeDetectorSensor)
		{
			result = new SmokeDetectorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is RollerShutterActuator)
		{
			result = new RollerShutterConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is ThermostatActuator)
		{
			result = new ThermostatActuatorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is TemperatureSensor)
		{
			result = new TemperatureSensorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is HumiditySensor)
		{
			result = new HumiditySensorConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is ValveActuator)
		{
			result = new CcValveConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is AlarmActuator)
		{
			result = new AlarmActuatorConfiguration(logicalDevice);
		}
		else if (logicalDevice is Router)
		{
			result = new RouterConfiguration(logicalDevice, array);
		}
		else if (logicalDevice is RoomSetpoint)
		{
			result = new RoomSetpointConfiguration(logicalDevice, array);
		}
		return result;
	}
}
