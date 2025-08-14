using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class AlarmActuatorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(AlarmActuatorConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		AlarmActuator alarmActuator = logicalDevice as AlarmActuator;
		if (alarmActuator == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(AlarmActuator)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(alarmActuator, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		AlarmActuator alarmActuator = new AlarmActuator();
		InitializeCommonSmartHomeDeviceProperties(alarmActuator, aCapability);
		return alarmActuator;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is AlarmActuatorState alarmActuatorState))
		{
			logger.Error($"{typeof(AlarmActuatorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = alarmActuatorState.OnStateProperty.Name,
			Value = alarmActuatorState.OnState,
			LastChanged = alarmActuatorState.OnStateUpdateTimestamp
		});
		return list;
	}
}
