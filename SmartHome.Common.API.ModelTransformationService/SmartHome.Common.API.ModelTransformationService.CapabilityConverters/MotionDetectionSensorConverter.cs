using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class MotionDetectionSensorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(MotionDetectionSensorConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		MotionDetectionSensor motionDetectionSensor = logicalDevice as MotionDetectionSensor;
		if (motionDetectionSensor == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(MotionDetectionSensor)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		MotionDetectionSensor motionDetectionSensor = new MotionDetectionSensor();
		InitializeCommonSmartHomeDeviceProperties(motionDetectionSensor, aCapability);
		return motionDetectionSensor;
	}
}
