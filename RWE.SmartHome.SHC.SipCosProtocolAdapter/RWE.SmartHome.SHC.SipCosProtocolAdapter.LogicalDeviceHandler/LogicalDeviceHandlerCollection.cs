using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ActuatorHandler.SirenHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class LogicalDeviceHandlerCollection : ILogicalDeviceHandlerCollection
{
	private readonly Dictionary<Type, IActuatorHandlerEntityTypes> actuatorHandlers;

	private readonly Dictionary<Type, ISensorHandlerEntityTypes> sensorHandlers;

	private readonly Dictionary<string, IActuatorHandlerStringTypes> actuatorHandlersString;

	private readonly Dictionary<string, ISensorHandlerStringTypes> sensorHandlersString;

	public LogicalDeviceHandlerCollection(IRepository configurationRepository, IDeviceManager deviceManager, ILogicalDeviceStateRepository logicalDeviceStateRepository, IEventManager eventManager)
	{
		actuatorHandlers = new Dictionary<Type, IActuatorHandlerEntityTypes>();
		sensorHandlers = new Dictionary<Type, ISensorHandlerEntityTypes>();
		actuatorHandlersString = new Dictionary<string, IActuatorHandlerStringTypes>();
		sensorHandlersString = new Dictionary<string, ISensorHandlerStringTypes>();
		AddActuatorHandler(new SwitchActuatorHandler(logicalDeviceStateRepository));
		AddActuatorHandler(new DimmerActuatorHandler(configurationRepository, logicalDeviceStateRepository));
		AddActuatorHandler(new RollerShutterActuatorHandler());
		AddActuatorHandler(new AlarmActuatorHandler(deviceManager));
		AddActuatorHandler(new ValveActuatorHandler());
		AddActuatorHandler(new ThermostatActuatorHandler(configurationRepository, eventManager));
		AddActuatorHandler(new SirenAlarmActuatorHandler(configurationRepository, deviceManager, eventManager));
		AddSensorHandler(new WindowDoorSensorHandler());
		AddSensorHandler(new LuminanceSensorHandler());
		AddSensorHandler(new SmokeDetectorSensorHandler());
		AddSensorHandler(new TemperatureSensorHandler(deviceManager.DeviceList));
		AddSensorHandler(new HumiditySensorHandler(deviceManager.DeviceList));
		AddSensorHandler(new PushButtonSensorHandler());
		AddSensorHandler(new MotionDetectionSensorHandler());
		AddSensorHandler(new SabotageSensorHandler(deviceManager));
	}

	private void AddActuatorHandler(IActuatorHandlerEntityTypes handler)
	{
		foreach (Type supportedActuatorType in handler.SupportedActuatorTypes)
		{
			actuatorHandlers.Add(supportedActuatorType, handler);
		}
	}

	private void AddActuatorHandler(IActuatorHandlerStringTypes handler)
	{
		foreach (string supportedActuatorType in handler.SupportedActuatorTypes)
		{
			actuatorHandlersString.Add(supportedActuatorType, handler);
		}
	}

	private void AddSensorHandler(ISensorHandlerEntityTypes handler)
	{
		foreach (Type supportedSensorType in handler.SupportedSensorTypes)
		{
			sensorHandlers.Add(supportedSensorType, handler);
		}
	}

	private void AddSensorHandler(ISensorHandlerStringTypes handler)
	{
		foreach (string supportedSensorType in handler.SupportedSensorTypes)
		{
			sensorHandlersString.Add(supportedSensorType, handler);
		}
	}

	public ILogicalDeviceHandler GetAlarActuatorHandler()
	{
		return GetDeviceHandler(typeof(AlarmActuator));
	}

	public ILogicalDeviceHandler GetSirenAlarmActuator()
	{
		return GetDeviceHandler("SirenActuator");
	}

	public bool TryGetHandler(LogicalDevice logicalDevice, out ILogicalDeviceHandler logicalDeviceHandler)
	{
		logicalDeviceHandler = GetLogicalDeviceHandler(logicalDevice);
		return logicalDeviceHandler != null;
	}

	public ILogicalDeviceHandler GetLogicalDeviceHandler(LogicalDevice logicalDevice)
	{
		return Promise.GetFirstValue(null, logicalDevice, (ILogicalDeviceHandler m) => m != null, (LogicalDevice l) => GetDeviceHandler(l.DeviceType), (LogicalDevice l) => GetDeviceHandler(l.GetType()));
	}

	private ILogicalDeviceHandler GetDeviceHandler(Type logicalDeviceType)
	{
		return Promise.GetFirstValue(null, logicalDeviceType, (ILogicalDeviceHandler m) => m != null, (Type l) => ((object)l == null || !actuatorHandlers.ContainsKey(l)) ? null : actuatorHandlers[l], (Type l) => ((object)l == null || !sensorHandlers.ContainsKey(l)) ? null : sensorHandlers[l]);
	}

	private ILogicalDeviceHandler GetDeviceHandler(string logicalDeviceType)
	{
		return Promise.GetFirstValue(null, logicalDeviceType, (ILogicalDeviceHandler m) => m != null, (string l) => (l == null || !actuatorHandlersString.ContainsKey(l)) ? null : actuatorHandlersString[l], (string l) => (l == null || !sensorHandlersString.ContainsKey(l)) ? null : sensorHandlersString[l]);
	}
}
