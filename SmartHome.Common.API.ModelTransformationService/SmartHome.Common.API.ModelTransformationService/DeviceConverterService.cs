using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.DeviceConverters;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class DeviceConverterService : IDeviceConverterService
{
	private const string GenericDeviceType = "Generic";

	private readonly IDeviceConverter deviceConverter;

	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(DeviceConverterService));

	public DeviceConverterService()
	{
		deviceConverter = new DeviceConverter();
	}

	public Device FromSmartHomeBaseDevice(BaseDevice baseDevice, bool includeCapabilities)
	{
		string arg = (baseDevice.IsCoreDevice() ? baseDevice.DeviceType : "Generic");
		try
		{
			return deviceConverter.FromSmartHomeBaseDevice(baseDevice, includeCapabilities);
		}
		catch (Exception exception)
		{
			logger.Error($"Unknown SmartHome device type: {arg}.", exception);
		}
		return new UnknownDevice();
	}

	public BaseDevice ToSmartHomeBaseDevice(Device aDevice)
	{
		logger.DebugEnterMethod(aDevice.Type);
		string text = (aDevice.IsCoreDevice() ? aDevice.Type : "Generic");
		try
		{
			logger.Debug($"Converting API Devices with Product: {aDevice.Product}, Type: {text}");
			return deviceConverter.ToSmartHomeBaseDevice(aDevice);
		}
		catch (Exception)
		{
			logger.LogAndThrow<ArgumentException>($"Cannot convert API device to SmartHome: {text}");
			return null;
		}
	}

	public List<Property> FromSmartHomeDeviceState(PhysicalDeviceState state)
	{
		if (state == null)
		{
			return null;
		}
		try
		{
			logger.Debug($"Converting SH PhysicalDeviceState with Type: {state.GetType()}");
			return deviceConverter.FromSmartHomeDeviceState(state);
		}
		catch (Exception)
		{
			logger.LogAndThrow<NotImplementedException>($"Unknown SmartHome device type, cannot convert SmartHome state: {state.GetType()}");
			return null;
		}
	}
}
