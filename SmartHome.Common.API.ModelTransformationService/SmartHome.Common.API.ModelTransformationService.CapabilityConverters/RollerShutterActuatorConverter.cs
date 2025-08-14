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

internal class RollerShutterActuatorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(RollerShutterActuatorConverter));

	public RollerShutterActuatorConverter()
	{
		List<string> collection = new List<string> { "TimeFullUp", "TimeFullDown", "IsCalibrating" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		RollerShutterActuator rollerShutterActuator = logicalDevice as RollerShutterActuator;
		if (rollerShutterActuator == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(RollerShutterActuator)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(logicalDevice, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "TimeFullUp",
				Value = rollerShutterActuator.TimeFullUp
			},
			new Property
			{
				Name = "TimeFullDown",
				Value = rollerShutterActuator.TimeFullDown
			},
			new Property
			{
				Name = "IsCalibrating",
				Value = rollerShutterActuator.IsCalibrating
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		RollerShutterActuator rollerShutterActuator = new RollerShutterActuator();
		InitializeCommonSmartHomeDeviceProperties(rollerShutterActuator, aCapability);
		if (aCapability.Config != null && aCapability.Config.Count > 0)
		{
			rollerShutterActuator.TimeFullUp = aCapability.Config.GetPropertyValue<int>("TimeFullUp");
			rollerShutterActuator.TimeFullDown = aCapability.Config.GetPropertyValue<int>("TimeFullDown");
			rollerShutterActuator.IsCalibrating = aCapability.Config.GetPropertyValue<bool>("IsCalibrating");
		}
		return rollerShutterActuator;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is RollerShutterActuatorState rollerShutterActuatorState))
		{
			logger.Error($"{typeof(RollerShutterActuatorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = rollerShutterActuatorState.ShutterLevelProperty.Name,
			Value = rollerShutterActuatorState.ShutterLevel,
			LastChanged = rollerShutterActuatorState.ShutterLevelUpdateTimestamp
		});
		return list;
	}
}
