using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class SmokeDetectorSensorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(SmokeDetectorSensorConverter));

	public SmokeDetectorSensorConverter()
	{
		List<string> collection = new List<string> { "SensorType" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		SmokeDetectorSensor smokeDetectorSensor = logicalDevice as SmokeDetectorSensor;
		if (smokeDetectorSensor == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(SmokeDetectorSensor)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(smokeDetectorSensor, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "SensorType",
				Value = SensorType.Event.ToString()
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		SmokeDetectorSensor smokeDetectorSensor = new SmokeDetectorSensor();
		InitializeCommonSmartHomeDeviceProperties(smokeDetectorSensor, aCapability);
		return smokeDetectorSensor;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is SmokeDetectionSensorState smokeDetectionSensorState))
		{
			logger.Error($"{typeof(SmokeDetectionSensorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = smokeDetectionSensorState.IsSmokeAlarmProperty.Name,
			Value = smokeDetectionSensorState.IsSmokeAlarm,
			LastChanged = smokeDetectionSensorState.IsSmokeAlarmUpdateTimestamp
		});
		return list;
	}
}
