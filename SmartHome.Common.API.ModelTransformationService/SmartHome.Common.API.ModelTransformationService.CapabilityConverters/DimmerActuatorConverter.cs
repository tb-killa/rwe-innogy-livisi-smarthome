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

internal class DimmerActuatorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(DimmerActuatorConverter));

	public DimmerActuatorConverter()
	{
		List<string> collection = new List<string> { "TechnicalMinValue", "TechnicalMaxValue" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		DimmerActuator dimmerActuator = logicalDevice as DimmerActuator;
		if (dimmerActuator == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(DimmerActuator)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(logicalDevice, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "TechnicalMinValue",
				Value = dimmerActuator.TechnicalMinValue
			},
			new Property
			{
				Name = "TechnicalMaxValue",
				Value = dimmerActuator.TechnicalMaxValue
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		DimmerActuator dimmerActuator = new DimmerActuator();
		InitializeCommonSmartHomeDeviceProperties(dimmerActuator, aCapability);
		if (aCapability.Config != null && aCapability.Config.Count > 0)
		{
			dimmerActuator.TechnicalMinValue = aCapability.Config.GetPropertyValue<int>("TechnicalMinValue");
			dimmerActuator.TechnicalMaxValue = aCapability.Config.GetPropertyValue<int>("TechnicalMaxValue");
		}
		return dimmerActuator;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is DimmerActuatorState dimmerActuatorState))
		{
			logger.Error($"{typeof(DimmerActuatorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = dimmerActuatorState.DimLevelProperty.Name,
			Value = dimmerActuatorState.DimLevel,
			LastChanged = dimmerActuatorState.DimLevelUpdateTimestamp
		});
		return list;
	}
}
