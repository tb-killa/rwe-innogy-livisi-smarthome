using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.DeviceConverters;

public class DeviceConverter : IDeviceConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(DeviceConverter));

	private readonly List<string> baseDeviceAttributes = new List<string> { "Name", "TimeOfAcceptance", "TimeOfDiscovery", "ProtocolId" };

	public virtual Device FromSmartHomeBaseDevice(BaseDevice baseDevice, bool includeCapabilities)
	{
		try
		{
			Device device = new Device();
			device.Manufacturer = baseDevice.Manufacturer;
			device.Version = baseDevice.DeviceVersion.ToString(CultureInfo.InvariantCulture);
			device.Product = baseDevice.AppId.Replace("sh://", string.Empty);
			device.SerialNumber = baseDevice.SerialNumber;
			device.Id = baseDevice.Id.ToString("N");
			device.Type = baseDevice.DeviceType;
			device.Config = new List<SmartHome.Common.API.Entities.Entities.Property>
			{
				new SmartHome.Common.API.Entities.Entities.Property
				{
					Name = "Name",
					Value = baseDevice.Name
				},
				new SmartHome.Common.API.Entities.Entities.Property
				{
					Name = "ProtocolId",
					Value = baseDevice.ProtocolId.ToString()
				}
			};
			Device device2 = device;
			if (baseDevice.TimeOfAcceptance.HasValue)
			{
				device2.Config.Add(new SmartHome.Common.API.Entities.Entities.Property
				{
					Name = "TimeOfAcceptance",
					Value = baseDevice.TimeOfAcceptance.Value.ToUniversalTime()
				});
			}
			if (baseDevice.TimeOfDiscovery.HasValue)
			{
				device2.Config.Add(new SmartHome.Common.API.Entities.Entities.Property
				{
					Name = "TimeOfDiscovery",
					Value = baseDevice.TimeOfDiscovery.Value.ToUniversalTime()
				});
			}
			if (baseDevice.Properties != null && baseDevice.Properties.Any())
			{
				foreach (RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property property in baseDevice.Properties)
				{
					device2.Config.Add(PropertyConverter.ToApiProperty(property));
				}
			}
			if (baseDevice.Tags != null && baseDevice.Tags.Any())
			{
				device2.Tags = baseDevice.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
				{
					Name = t.Name,
					Value = t.Value
				});
			}
			if (baseDevice.LocationId.HasValue)
			{
				string arg = baseDevice.LocationId.Value.ToString("N");
				device2.Location = $"/location/{arg}";
			}
			if (includeCapabilities && baseDevice.LogicalDeviceIds != null && baseDevice.LogicalDeviceIds.Any())
			{
				device2.Capabilities = new List<string>();
				foreach (Guid logicalDeviceId in baseDevice.LogicalDeviceIds)
				{
					device2.Capabilities.Add(string.Format("/capability/{0}", logicalDeviceId.ToString("N")));
				}
			}
			return device2;
		}
		catch (Exception exception)
		{
			logger.Error("Failed to convert base device.", exception);
			throw;
		}
	}

	public virtual BaseDevice ToSmartHomeBaseDevice(Device aDevice)
	{
		if (aDevice.Config == null)
		{
			logger.LogAndThrow<ArgumentException>(string.Format("Missing {0} configuration property", "ProtocolId"));
		}
		ProtocolIdentifier propertyValue = aDevice.Config.GetPropertyValue<ProtocolIdentifier>("ProtocolId");
		BaseDevice baseDevice = new BaseDevice();
		baseDevice.Id = aDevice.Id.ToGuid();
		baseDevice.SerialNumber = aDevice.SerialNumber;
		baseDevice.AppId = string.Format("{0}{1}", "sh://", aDevice.Product);
		baseDevice.Properties = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>();
		baseDevice.Manufacturer = aDevice.Manufacturer;
		baseDevice.DeviceVersion = aDevice.Version;
		baseDevice.DeviceType = aDevice.Type;
		baseDevice.ProtocolId = propertyValue;
		BaseDevice baseDevice2 = baseDevice;
		if (aDevice.Config != null)
		{
			try
			{
				foreach (SmartHome.Common.API.Entities.Entities.Property item in aDevice.Config.Where((SmartHome.Common.API.Entities.Entities.Property p) => !baseDeviceAttributes.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase)))
				{
					baseDevice2.Properties.Add(PropertyConverter.ToSmartHomeProperty(item));
				}
			}
			catch (MissingFieldException ex)
			{
				logger.Error(ex.Message);
				throw;
			}
			if (aDevice.Config.Any((SmartHome.Common.API.Entities.Entities.Property p) => p.Name.EqualsIgnoreCase("Name")))
			{
				baseDevice2.Name = aDevice.Config.FirstOrDefault((SmartHome.Common.API.Entities.Entities.Property p) => p.Name.EqualsIgnoreCase("Name")).Value.ToString();
			}
			DateTime? propertyValue2 = aDevice.Config.GetPropertyValue<DateTime?>("TimeOfAcceptance", isMandatory: false);
			if (propertyValue2.HasValue)
			{
				baseDevice2.TimeOfAcceptance = propertyValue2;
			}
			DateTime? propertyValue3 = aDevice.Config.GetPropertyValue<DateTime?>("TimeOfDiscovery", isMandatory: false);
			if (propertyValue3.HasValue)
			{
				baseDevice2.TimeOfDiscovery = propertyValue3;
			}
		}
		if (aDevice.Capabilities != null && aDevice.Capabilities.Any())
		{
			baseDevice2.LogicalDeviceIds = aDevice.Capabilities.ConvertAll((string c) => c.GetGuid());
		}
		if (aDevice.Tags != null && aDevice.Tags.Any())
		{
			baseDevice2.Tags = aDevice.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property t) => new Tag
			{
				Name = t.Name,
				Value = t.Value.ToString()
			});
		}
		if (aDevice.Location != null && aDevice.Location.Any())
		{
			baseDevice2.LocationId = aDevice.Location.GetGuid();
		}
		if (aDevice.Volatile != null && aDevice.Volatile.Any())
		{
			baseDevice2.VolatileProperties = aDevice.Volatile.ConvertAll((SmartHome.Common.API.Entities.Entities.Property p) => PropertyConverter.ToSmartHomeProperty(p));
		}
		return baseDevice2;
	}

	public virtual List<SmartHome.Common.API.Entities.Entities.Property> FromSmartHomeDeviceState(PhysicalDeviceState state)
	{
		List<SmartHome.Common.API.Entities.Entities.Property> list = null;
		if (state.DeviceProperties != null && state.DeviceProperties.Properties != null && state.DeviceProperties.Properties.Any())
		{
			list = new List<SmartHome.Common.API.Entities.Entities.Property>();
			list.AddRange(state.DeviceProperties.Properties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty));
		}
		return list;
	}
}
