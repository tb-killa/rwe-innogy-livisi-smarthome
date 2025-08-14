using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class ValveActuatorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ValveActuatorConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		ValveActuator valveActuator = logicalDevice as ValveActuator;
		if (valveActuator == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(ValveActuator)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		ValveActuator valveActuator = new ValveActuator();
		InitializeCommonSmartHomeDeviceProperties(valveActuator, aCapability);
		return valveActuator;
	}
}
