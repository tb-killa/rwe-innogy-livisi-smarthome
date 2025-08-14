using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class CompositeDeviceBuilder
{
	private readonly IRepository configRepo;

	private readonly ILogicalDeviceStateRepository stateRepo;

	private readonly StatefulDeviceProvider deviceProvider;

	public CompositeDeviceBuilder(IRepository configRepo, ILogicalDeviceStateRepository stateRepo)
	{
		this.configRepo = configRepo;
		this.stateRepo = stateRepo;
		deviceProvider = new StatefulDeviceProvider(configRepo);
	}

	public IEnumerable<CompositeDevice> BuildDevices(IEnumerable<BaseDevice> unhandledBaseDevices)
	{
		IEnumerable<LogicalDevice> source = unhandledBaseDevices.SelectMany((BaseDevice x) => GetVrccLogicalDevices(x));
		return from x in source
			select BuildCompositeDevice(x) into y
			where y != null
			select y;
	}

	private CompositeDevice BuildCompositeDevice(LogicalDevice logicalDevice)
	{
		CompositeDevice result = null;
		switch (logicalDevice.DeviceType)
		{
		case "RoomTemperature":
		{
			IEnumerable<LogicalDevice> relatedHeatingDevices = deviceProvider.GetRelatedHeatingDevices(logicalDevice, "VRCCTemperature");
			result = new RoomTemperatureSensor(logicalDevice, relatedHeatingDevices);
			break;
		}
		case "RoomHumidity":
		{
			IEnumerable<LogicalDevice> relatedHeatingDevices = deviceProvider.GetRelatedHeatingDevices(logicalDevice, "VRCCHumidity");
			result = new RoomHumiditySensor(logicalDevice, relatedHeatingDevices);
			break;
		}
		case "RoomSetpoint":
		{
			IEnumerable<LogicalDevice> relatedHeatingDevices = deviceProvider.GetRelatedHeatingDevices(logicalDevice, "VRCCSetPoint");
			result = new RoomSetPointActuator(logicalDevice, relatedHeatingDevices);
			break;
		}
		}
		return result;
	}

	private IEnumerable<LogicalDevice> GetVrccLogicalDevices(BaseDevice baseDevice)
	{
		if (baseDevice.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.VRCC)
		{
			return new List<LogicalDevice>();
		}
		return baseDevice.LogicalDeviceIds.Select((Guid x) => configRepo.GetLogicalDevice(x)).ToList();
	}
}
