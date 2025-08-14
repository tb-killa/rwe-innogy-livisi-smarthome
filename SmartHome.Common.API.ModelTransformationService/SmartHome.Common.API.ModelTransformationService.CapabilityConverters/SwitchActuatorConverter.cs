using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class SwitchActuatorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(SwitchActuatorConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		SwitchActuator switchActuator = logicalDevice as SwitchActuator;
		if (switchActuator == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(SwitchActuator)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		SwitchActuator switchActuator = new SwitchActuator();
		InitializeCommonSmartHomeDeviceProperties(switchActuator, aCapability);
		return switchActuator;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is SwitchActuatorState switchActuatorState))
		{
			logger.Error($"{typeof(SwitchActuatorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = switchActuatorState.OnStateProperty.Name,
			Value = switchActuatorState.OnState,
			LastChanged = switchActuatorState.OnStateUpdateTimestamp
		});
		return list;
	}
}
