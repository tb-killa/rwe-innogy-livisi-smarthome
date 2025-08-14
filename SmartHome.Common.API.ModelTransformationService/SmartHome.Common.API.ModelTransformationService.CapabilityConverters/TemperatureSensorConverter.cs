using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class TemperatureSensorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(TemperatureSensorConverter));

	public TemperatureSensorConverter()
	{
		List<string> collection = new List<string> { "IsFreezeProtectionActivated", "FreezeProtection" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		TemperatureSensor temperatureSensor = logicalDevice as TemperatureSensor;
		if (temperatureSensor == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(TemperatureSensor)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(logicalDevice, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "IsFreezeProtectionActivated",
				Value = temperatureSensor.IsFreezeProtectionActivated
			},
			new Property
			{
				Name = "FreezeProtection",
				Value = temperatureSensor.FreezeProtection
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		TemperatureSensor temperatureSensor = new TemperatureSensor();
		InitializeCommonSmartHomeDeviceProperties(temperatureSensor, aCapability);
		if (aCapability.Config != null && aCapability.Config.Count > 0)
		{
			temperatureSensor.IsFreezeProtectionActivated = aCapability.Config.GetPropertyValue<bool>("IsFreezeProtectionActivated", isMandatory: false);
			temperatureSensor.FreezeProtection = aCapability.Config.GetPropertyValue<decimal>("FreezeProtection", isMandatory: false);
		}
		return temperatureSensor;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is TemperatureSensorState temperatureSensorState))
		{
			logger.Error($"{typeof(TemperatureSensorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = temperatureSensorState.TemperatureProperty.Name,
			Value = temperatureSensorState.Temperature,
			LastChanged = temperatureSensorState.TemperatureUpdateTimestamp
		});
		list.Add(new Property
		{
			Name = temperatureSensorState.FrostWarningProperty.Name,
			Value = temperatureSensorState.FrostWarning,
			LastChanged = temperatureSensorState.FrostWarningUpdateTimestamp
		});
		return list;
	}
}
