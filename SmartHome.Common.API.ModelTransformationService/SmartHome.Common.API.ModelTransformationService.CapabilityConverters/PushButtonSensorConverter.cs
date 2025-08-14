using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class PushButtonSensorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(PushButtonSensorConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		PushButtonSensor pushButtonSensor = logicalDevice as PushButtonSensor;
		if (pushButtonSensor == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(PushButtonSensor)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		PushButtonSensor pushButtonSensor = new PushButtonSensor();
		InitializeCommonSmartHomeDeviceProperties(pushButtonSensor, aCapability);
		return pushButtonSensor;
	}
}
