using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class RouterConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(RouterConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		Router router = logicalDevice as Router;
		if (router == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(Router)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		Router router = new Router();
		InitializeCommonSmartHomeDeviceProperties(router, aCapability);
		return router;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		return null;
	}
}
