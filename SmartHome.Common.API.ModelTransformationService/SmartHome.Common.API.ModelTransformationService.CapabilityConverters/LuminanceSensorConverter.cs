using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class LuminanceSensorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(LuminanceSensorConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		LuminanceSensor luminanceSensor = logicalDevice as LuminanceSensor;
		if (luminanceSensor == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(LuminanceSensor)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		LuminanceSensor luminanceSensor = new LuminanceSensor();
		InitializeCommonSmartHomeDeviceProperties(luminanceSensor, aCapability);
		return luminanceSensor;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is LuminanceSensorState luminanceSensorState))
		{
			logger.Error($"{typeof(LuminanceSensorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = luminanceSensorState.LuminanceProperty.Name,
			Value = luminanceSensorState.Luminance,
			LastChanged = luminanceSensorState.LuminanceUpdateTimestamp
		});
		return list;
	}
}
