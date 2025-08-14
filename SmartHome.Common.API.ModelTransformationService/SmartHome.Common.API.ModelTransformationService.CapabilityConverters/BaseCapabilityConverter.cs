using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

public class BaseCapabilityConverter : ICapabilityConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(BaseCapabilityConverter));

	protected List<string> BaseCapabilityAttributes = new List<string> { "Name", "ActivityLogActive" };

	public virtual Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context)
	{
		Capability capability = new Capability();
		capability.Id = logicalDevice.Id.ToString("N");
		capability.Type = logicalDevice.DeviceType;
		capability.Config = new List<SmartHome.Common.API.Entities.Entities.Property>
		{
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "Name",
				Value = logicalDevice.Name
			}
		};
		Capability capability2 = capability;
		if (logicalDevice.ActivityLogActive.HasValue)
		{
			capability2.Config.Add(new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "ActivityLogActive",
				Value = logicalDevice.ActivityLogActive.Value
			});
		}
		string arg = logicalDevice.BaseDeviceId.ToString("N");
		capability2.Device = $"/device/{arg}";
		if (logicalDevice.LocationId.HasValue)
		{
			string arg2 = logicalDevice.LocationId.Value.ToString("N");
			capability2.Location = $"/location/{arg2}";
		}
		if (logicalDevice.Properties != null)
		{
			foreach (RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property property in logicalDevice.Properties)
			{
				capability2.Config.Add(PropertyConverter.ToApiProperty(property));
			}
		}
		if (logicalDevice.Tags != null && logicalDevice.Tags.Any())
		{
			capability2.Tags = logicalDevice.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		return capability2;
	}

	public virtual LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability)
	{
		LogicalDevice logicalDevice = new LogicalDevice();
		InitializeCommonSmartHomeDeviceProperties(logicalDevice, aCapability);
		return logicalDevice;
	}

	protected void InitializeCommonSmartHomeDeviceProperties(LogicalDevice shDevice, Capability aCapability)
	{
		shDevice.Id = aCapability.Id.ToGuid();
		shDevice.DeviceType = aCapability.Type;
		shDevice.Properties = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>();
		if (aCapability.Device != null && aCapability.Device.Any())
		{
			shDevice.BaseDeviceId = aCapability.Device.GetGuid();
		}
		if (aCapability.Config != null)
		{
			foreach (SmartHome.Common.API.Entities.Entities.Property item in aCapability.Config.Where((SmartHome.Common.API.Entities.Entities.Property p) => !BaseCapabilityAttributes.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase)))
			{
				shDevice.Properties.Add(PropertyConverter.ToSmartHomeProperty(item));
			}
			if (aCapability.Config.Any((SmartHome.Common.API.Entities.Entities.Property p) => p.Name.EqualsIgnoreCase("Name")))
			{
				shDevice.Name = aCapability.Config.GetPropertyValue<string>("Name");
			}
			if (aCapability.Config.Any((SmartHome.Common.API.Entities.Entities.Property p) => p.Name.EqualsIgnoreCase("ActivityLogActive")))
			{
				shDevice.ActivityLogActive = aCapability.Config.GetPropertyValue<bool>("ActivityLogActive");
			}
		}
		if (aCapability.Tags != null && aCapability.Tags.Any())
		{
			shDevice.Tags = aCapability.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property t) => new Tag
			{
				Name = t.Name,
				Value = t.Value.ToString()
			});
		}
		if (aCapability.Location != null && aCapability.Location.Any())
		{
			shDevice.LocationId = aCapability.Location.GetGuid();
		}
	}

	public virtual List<SmartHome.Common.API.Entities.Entities.Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context)
	{
		if (!(state is GenericDeviceState))
		{
			logger.Error($"{typeof(GenericDeviceState)} type expected - got {state.GetType()}");
			return new List<SmartHome.Common.API.Entities.Entities.Property>();
		}
		List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> properties = state.GetProperties();
		return (properties != null && properties.Any()) ? properties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty) : new List<SmartHome.Common.API.Entities.Entities.Property>();
	}
}
