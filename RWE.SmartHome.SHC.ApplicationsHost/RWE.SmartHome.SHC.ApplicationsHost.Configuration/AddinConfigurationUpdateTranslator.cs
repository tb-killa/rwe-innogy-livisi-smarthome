using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API.Configuration;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

internal class AddinConfigurationUpdateTranslator
{
	private const string LoggingSource = "AddinConfigurationUpdateTranslator";

	private readonly string appId;

	private readonly IRepository configurationRepository;

	public AddinConfigurationUpdateTranslator(string appId, IRepository configurationRepository)
	{
		this.appId = appId;
		this.configurationRepository = configurationRepository;
	}

	public AddinConfigurationUpdate Update(IEnumerable<Device> devicesToSet, IEnumerable<Capability> capabilitiesToSet, IEnumerable<Guid> deviceIdsToDelete, IEnumerable<Guid> capabilitiesIdsToDelete)
	{
		AddinConfigurationUpdate ret = new AddinConfigurationUpdate();
		ret.IsValid = true;
		try
		{
			ret.BaseDevices = (devicesToSet ?? new List<Device>()).Select((Device x) => CreateBaseDeviceUpdate(x)).ToList();
			ret.LogicalDevices = (capabilitiesToSet ?? new List<Capability>()).Select((Capability x) => CreateLogicalDeviceUpdate(x, ret.BaseDevices)).ToList();
			UpdateBaseDevicesForLogicalDevices(ret.BaseDevices, ret.LogicalDevices);
		}
		catch (Exception)
		{
			Log.InformationFormat(Module.ApplicationsHost, "AddinConfigurationUpdateTranslator", true, "ConfigurationUpdate requested by application {0}; Validation failed.", appId);
			ret.IsValid = false;
		}
		ret.DeleteBaseDevicesIds = (deviceIdsToDelete ?? new List<Guid>()).ToList();
		foreach (Guid deleteBaseDevicesId in ret.DeleteBaseDevicesIds)
		{
			if (!IsApplicationsDevice(configurationRepository.GetBaseDevice(deleteBaseDevicesId)))
			{
				Log.InformationFormat(Module.ApplicationsHost, "AddinConfigurationUpdateTranslator", true, "ConfigurationUpdate requested by application {0}; Attempted to delete non app device {1}.", appId, deleteBaseDevicesId);
				ret.IsValid = false;
			}
		}
		ret.DeleteLogicalDevicesIds = (capabilitiesIdsToDelete ?? new List<Guid>()).ToList();
		foreach (Guid deleteLogicalDevicesId in ret.DeleteLogicalDevicesIds)
		{
			if (!IsApplicationLogicalDevice(configurationRepository.GetLogicalDevice(deleteLogicalDevicesId)))
			{
				Log.InformationFormat(Module.ApplicationsHost, "AddinConfigurationUpdateTranslator", true, "ConfigurationUpdate requested by application {0}; Attempted to delete non app capability {1}.", appId, deleteLogicalDevicesId);
				ret.IsValid = false;
			}
		}
		return ret;
	}

	private BaseDevice CreateBaseDeviceUpdate(Device apiDevice)
	{
		BaseDevice baseDevice = null;
		BaseDevice baseDevice2 = configurationRepository.GetBaseDevice(apiDevice.Id);
		if (baseDevice2 != null)
		{
			if (!IsApplicationsDevice(baseDevice2))
			{
				Log.InformationFormat(Module.ApplicationsHost, "AddinConfigurationUpdateTranslator", true, "ConfigurationUpdate requested by application {0}; Attempted to update non app device {1}.", appId, baseDevice2.Id);
				throw new Exception();
			}
			baseDevice = baseDevice2.Clone();
			BaseDevice baseDevice3 = apiDevice.ToCoreBaseDevice();
			baseDevice.Properties = baseDevice3.Properties;
			baseDevice.SerialNumber = baseDevice3.SerialNumber;
			baseDevice.Manufacturer = baseDevice3.Manufacturer;
			baseDevice.DeviceType = baseDevice3.DeviceType;
			baseDevice.DeviceVersion = baseDevice3.DeviceVersion;
			baseDevice.Name = baseDevice3.Name;
		}
		else
		{
			baseDevice = apiDevice.ToCoreBaseDevice();
			baseDevice.AppId = appId;
			baseDevice.ProtocolId = ProtocolIdentifier.Virtual;
			baseDevice.TimeOfAcceptance = ShcDateTime.UtcNow;
		}
		return baseDevice;
	}

	private LogicalDevice CreateLogicalDeviceUpdate(Capability apiCapability, List<BaseDevice> updateBaseDevices)
	{
		LogicalDevice logicalDevice = null;
		LogicalDevice logicalDevice2 = configurationRepository.GetLogicalDevice(apiCapability.Id);
		if (logicalDevice2 != null)
		{
			if (!IsApplicationLogicalDevice(logicalDevice2))
			{
				Log.InformationFormat(Module.ApplicationsHost, "AddinConfigurationUpdateTranslator", true, "ConfigurationUpdate requested by application {0}; Attempted to update non app capability {1}.", appId, logicalDevice2.Id);
				throw new Exception();
			}
			logicalDevice = logicalDevice2.Clone();
			LogicalDevice logicalDevice3 = apiCapability.ToCoreLogicalDevice();
			logicalDevice.DeviceType = logicalDevice3.DeviceType;
			logicalDevice.Name = logicalDevice3.Name;
			logicalDevice.BaseDeviceId = logicalDevice3.BaseDeviceId;
			logicalDevice.Properties = logicalDevice3.Properties;
			logicalDevice.PrimaryPropertyName = logicalDevice3.PrimaryPropertyName;
			logicalDevice.ActivityLogActive = logicalDevice3.ActivityLogActive;
		}
		else
		{
			logicalDevice = apiCapability.ToCoreLogicalDevice();
			logicalDevice.ActivityLogActive = true;
		}
		return logicalDevice;
	}

	private void UpdateBaseDevicesForLogicalDevices(List<BaseDevice> baseDevices, List<LogicalDevice> logicalDevices)
	{
		foreach (LogicalDevice logicalDevice3 in logicalDevices)
		{
			LogicalDevice logicalDevice = logicalDevice3;
			LogicalDevice logicalDevice2 = configurationRepository.GetLogicalDevice(logicalDevice.Id);
			if (logicalDevice2 != null)
			{
				if (logicalDevice.BaseDeviceId == logicalDevice2.Id)
				{
					continue;
				}
				BaseDevice baseDevice = EnsureDeviceIsInUpdateList(baseDevices, logicalDevice2.BaseDeviceId);
				baseDevice.LogicalDeviceIds.Remove(logicalDevice2.Id);
			}
			BaseDevice baseDevice2 = EnsureDeviceIsInUpdateList(baseDevices, logicalDevice.BaseDeviceId);
			if (baseDevice2 == null)
			{
				Log.InformationFormat(Module.ApplicationsHost, "AddinConfigurationUpdateTranslator", true, "ConfigurationUpdate requested by application {0}; Could not find target device with id {1} for capability {2}.", logicalDevice.BaseDeviceId, logicalDevice.Id);
				throw new Exception();
			}
			baseDevice2.LogicalDeviceIds.Add(logicalDevice.Id);
		}
	}

	private bool IsApplicationsDevice(BaseDevice baseDevice)
	{
		return baseDevice.AppId == appId;
	}

	private bool IsApplicationLogicalDevice(LogicalDevice logicalDevice)
	{
		return IsApplicationsDevice(logicalDevice.BaseDevice);
	}

	private BaseDevice EnsureDeviceIsInUpdateList(List<BaseDevice> baseDevicesUpdate, Guid deviceId)
	{
		BaseDevice baseDevice = baseDevicesUpdate.FirstOrDefault((BaseDevice x) => x.Id == deviceId);
		if (baseDevice == null)
		{
			baseDevice = configurationRepository.GetBaseDevice(deviceId);
			if (baseDevice.AppId == appId)
			{
				baseDevicesUpdate.Add(baseDevice);
			}
			else
			{
				baseDevice = null;
			}
		}
		return baseDevice;
	}
}
