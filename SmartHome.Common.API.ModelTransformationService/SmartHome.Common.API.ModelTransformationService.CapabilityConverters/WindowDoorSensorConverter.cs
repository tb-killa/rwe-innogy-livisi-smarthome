using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class WindowDoorSensorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(WindowDoorSensorConverter));

	public WindowDoorSensorConverter()
	{
		List<string> collection = new List<string> { "EventFilterTime" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		WindowDoorSensor windowDoorSensor = logicalDevice as WindowDoorSensor;
		if (windowDoorSensor == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(WindowDoorSensor)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(logicalDevice, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "EventFilterTime",
				Value = windowDoorSensor.EventFilterTime
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		WindowDoorSensor windowDoorSensor = new WindowDoorSensor();
		InitializeCommonSmartHomeDeviceProperties(windowDoorSensor, aCapability);
		if (aCapability.Config != null && aCapability.Config.Any((Property p) => p.Name == "EventFilterTime"))
		{
			windowDoorSensor.EventFilterTime = aCapability.Config.GetPropertyValue<int>("EventFilterTime");
		}
		return windowDoorSensor;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is WindowDoorSensorState windowDoorSensorState))
		{
			logger.Error($"{typeof(WindowDoorSensorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = windowDoorSensorState.IsOpenProperty.Name,
			Value = windowDoorSensorState.IsOpen,
			LastChanged = windowDoorSensorState.IsOpenUpdateTimestamp
		});
		return list;
	}
}
