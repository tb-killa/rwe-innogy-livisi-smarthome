using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class RoomSetpointConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(RoomSetpointConverter));

	public RoomSetpointConverter()
	{
		List<string> collection = new List<string> { "MaxTemperature", "MinTemperature" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		RoomSetpoint roomSetpoint = logicalDevice as RoomSetpoint;
		if (roomSetpoint == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(RoomSetpoint)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(logicalDevice, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "MaxTemperature",
				Value = roomSetpoint.MaxTemperature
			},
			new Property
			{
				Name = "MinTemperature",
				Value = roomSetpoint.MinTemperature
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		RoomSetpoint roomSetpoint = new RoomSetpoint();
		InitializeCommonSmartHomeDeviceProperties(roomSetpoint, aCapability);
		if (aCapability.Config != null && aCapability.Config.Count > 0)
		{
			roomSetpoint.MaxTemperature = aCapability.Config.GetPropertyValue<int>("MaxTemperature");
			roomSetpoint.MinTemperature = aCapability.Config.GetPropertyValue<int>("MinTemperature");
		}
		return roomSetpoint;
	}
}
