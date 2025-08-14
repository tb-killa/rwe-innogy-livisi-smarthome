using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class RoomTemperatureConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(RoomTemperatureConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		RoomTemperature roomTemperature = logicalDevice as RoomTemperature;
		if (roomTemperature == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(RoomTemperature)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		RoomTemperature roomTemperature = new RoomTemperature();
		InitializeCommonSmartHomeDeviceProperties(roomTemperature, aCapability);
		return roomTemperature;
	}
}
