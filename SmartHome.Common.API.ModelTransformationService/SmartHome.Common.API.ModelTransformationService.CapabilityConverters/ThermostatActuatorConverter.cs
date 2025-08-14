using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class ThermostatActuatorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ThermostatActuatorConverter));

	public ThermostatActuatorConverter()
	{
		List<string> collection = new List<string> { "MaxTemperature", "MinTemperature", "ChildLock", "WindowOpenTemperature", "DisplayCurrentTemperature" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		ThermostatActuator thermostatActuator = logicalDevice as ThermostatActuator;
		if (thermostatActuator == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(ThermostatActuator)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(logicalDevice, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "MaxTemperature",
				Value = thermostatActuator.MaxTemperature
			},
			new Property
			{
				Name = "MinTemperature",
				Value = thermostatActuator.MinTemperature
			},
			new Property
			{
				Name = "ChildLock",
				Value = thermostatActuator.ChildLock
			},
			new Property
			{
				Name = "WindowOpenTemperature",
				Value = thermostatActuator.WindowOpenTemperature
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		ThermostatActuator thermostatActuator = new ThermostatActuator();
		InitializeCommonSmartHomeDeviceProperties(thermostatActuator, aCapability);
		if (aCapability.Config != null && aCapability.Config.Count > 0)
		{
			thermostatActuator.MaxTemperature = aCapability.Config.GetPropertyValue<decimal>("MaxTemperature");
			thermostatActuator.MinTemperature = aCapability.Config.GetPropertyValue<decimal>("MinTemperature");
			thermostatActuator.ChildLock = aCapability.Config.GetPropertyValue<bool>("ChildLock");
			thermostatActuator.WindowOpenTemperature = aCapability.Config.GetPropertyValue<decimal>("WindowOpenTemperature");
		}
		return thermostatActuator;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is ThermostatActuatorState thermostatActuatorState))
		{
			logger.Error($"{typeof(ThermostatActuatorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = thermostatActuatorState.PointTemperatureProperty.Name,
			Value = thermostatActuatorState.PointTemperature,
			LastChanged = thermostatActuatorState.PointTemperatureUpdateTimestamp
		});
		list.Add(new Property
		{
			Name = thermostatActuatorState.WindowReductionActiveProperty.Name,
			Value = thermostatActuatorState.WindowReductionActive,
			LastChanged = thermostatActuatorState.WindowReductionActiveUpdateTimestamp
		});
		list.Add(new Property
		{
			Name = thermostatActuatorState.OperationModeProperty.Name,
			Value = thermostatActuatorState.OperationModeString,
			LastChanged = thermostatActuatorState.OperationModeUpdateTimestamp
		});
		return list;
	}
}
