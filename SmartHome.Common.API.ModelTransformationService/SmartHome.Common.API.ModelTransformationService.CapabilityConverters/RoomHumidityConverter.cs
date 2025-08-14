using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class RoomHumidityConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(RoomHumidityConverter));

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		RoomHumidity roomHumidity = logicalDevice as RoomHumidity;
		if (roomHumidity == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(RoomHumidity)} type expected - got {logicalDevice.GetType()}");
		}
		return base.FromSmartHomeLogicalDevice(logicalDevice, context);
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		RoomHumidity roomHumidity = new RoomHumidity();
		InitializeCommonSmartHomeDeviceProperties(roomHumidity, aCapability);
		return roomHumidity;
	}
}
