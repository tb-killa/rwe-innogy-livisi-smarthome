using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal class HumiditySensorConverter : BaseCapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(HumiditySensorConverter));

	public HumiditySensorConverter()
	{
		List<string> collection = new List<string> { "IsMoldProtectionActivated", "HumidityMoldProtection" };
		BaseCapabilityAttributes.AddRange(collection);
	}

	public override Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		HumiditySensor humiditySensor = logicalDevice as HumiditySensor;
		if (humiditySensor == null)
		{
			logger.LogAndThrow<ArgumentException>($"Invalid parameter: {typeof(HumiditySensor)} type expected - got {logicalDevice.GetType()}");
		}
		Capability capability = base.FromSmartHomeLogicalDevice(logicalDevice, context);
		capability.Config.AddRange(new List<Property>
		{
			new Property
			{
				Name = "IsMoldProtectionActivated",
				Value = humiditySensor.IsMoldProtectionActivated
			},
			new Property
			{
				Name = "HumidityMoldProtection",
				Value = humiditySensor.HumidityMoldProtection
			}
		});
		return capability;
	}

	public override LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		HumiditySensor humiditySensor = new HumiditySensor();
		InitializeCommonSmartHomeDeviceProperties(humiditySensor, aCapability);
		if (aCapability.Config != null && aCapability.Config.Count > 0)
		{
			humiditySensor.IsMoldProtectionActivated = aCapability.Config.GetPropertyValue<bool>("IsMoldProtectionActivated", isMandatory: false);
			humiditySensor.HumidityMoldProtection = aCapability.Config.GetPropertyValue<decimal>("HumidityMoldProtection", isMandatory: false);
		}
		return humiditySensor;
	}

	public override List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is HumiditySensorState humiditySensorState))
		{
			logger.Error($"{typeof(HumiditySensorState)} type expected - got {state.GetType()}");
			return new List<Property>();
		}
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Name = humiditySensorState.HumidityProperty.Name,
			Value = humiditySensorState.Humidity,
			LastChanged = humiditySensorState.HumidityUpdateTimestamp
		});
		list.Add(new Property
		{
			Name = humiditySensorState.MoldWarningProperty.Name,
			Value = humiditySensorState.MoldWarning,
			LastChanged = humiditySensorState.MoldWarningUpdateTimestamp
		});
		return list;
	}
}
