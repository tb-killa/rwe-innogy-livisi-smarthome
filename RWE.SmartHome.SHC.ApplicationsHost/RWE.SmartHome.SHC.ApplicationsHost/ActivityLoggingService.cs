using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SmartHome.SHC.API.DeviceActivityLogging;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class ActivityLoggingService : IActivityLoggingService
{
	private readonly string applicationId;

	private readonly IRepository repository;

	public ActivityLoggingService(string applicationId, IRepository repository)
	{
		this.applicationId = applicationId;
		this.repository = repository;
	}

	public DeviceActivityLoggingType GetLoggingState(Guid deviceId)
	{
		LogicalDevice logicalDevice = repository.GetLogicalDevice(deviceId);
		if (IsDeviceSupportedByApplication(logicalDevice))
		{
			return GetDeviceLoggingState(logicalDevice);
		}
		return DeviceActivityLoggingType.None;
	}

	private DeviceActivityLoggingType GetDeviceLoggingState(LogicalDevice logicalDevice)
	{
		try
		{
			return (repository.GetShcBaseDevice().Properties.FirstOrDefault((Property x) => x.Name.Equals("ActivityLogEnabled")) is BooleanProperty { Value: var value } && value == true && logicalDevice != null && logicalDevice.ActivityLogActive == true) ? DeviceActivityLoggingType.Addin : DeviceActivityLoggingType.None;
		}
		catch (Exception ex)
		{
			Log.Error(Module.ApplicationsHost, "Failed to check DAL state.", ex.ToString());
			return DeviceActivityLoggingType.None;
		}
	}

	private bool IsDeviceSupportedByApplication(LogicalDevice logicalDevice)
	{
		BaseDevice baseDevice = logicalDevice?.BaseDevice;
		if (baseDevice != null)
		{
			return baseDevice.AppId == applicationId;
		}
		return false;
	}
}
