using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

public static class VRCCDeviceCreator
{
	public static readonly string RoomSetpointName = "Room Setpoint";

	public static readonly string RoomTemperatureName = "Actual Temperature";

	public static readonly string RoomHumidityName = "Humidity Level";

	public static BaseDevice CreateVRCCDevice(IRepository cfgRepo, Guid locationId)
	{
		BaseDevice baseDevice = new BaseDevice();
		baseDevice.Name = "Raumklima";
		baseDevice.LocationId = locationId;
		baseDevice.ProtocolId = ProtocolIdentifier.Virtual;
		baseDevice.SerialNumber = locationId.ToString("N");
		baseDevice.Manufacturer = "RWE";
		baseDevice.DeviceType = BuiltinPhysicalDeviceType.VRCC.ToString();
		baseDevice.AppId = CoreConstants.CoreAppId;
		baseDevice.DeviceVersion = "1.0";
		baseDevice.TimeOfAcceptance = ShcDateTime.UtcNow;
		baseDevice.TimeOfDiscovery = ShcDateTime.UtcNow;
		baseDevice.Properties = new List<Property>
		{
			new StringProperty("UnderlyingDeviceIds", string.Empty)
		};
		BaseDevice baseDevice2 = baseDevice;
		cfgRepo.SetBaseDevice(baseDevice2);
		List<LogicalDevice> list = new List<LogicalDevice>();
		list.Add(new RoomSetpoint
		{
			BaseDevice = baseDevice2,
			Name = RoomSetpointName,
			LocationId = baseDevice2.LocationId,
			DeviceType = "RoomSetpoint",
			MinTemperature = 6m,
			MaxTemperature = 30m,
			ActivityLogActive = true,
			Properties = new List<Property>
			{
				new StringProperty("UnderlyingCapabilityIds", string.Empty)
			}
		});
		list.Add(new RoomTemperature
		{
			BaseDevice = baseDevice2,
			Name = RoomTemperatureName,
			LocationId = baseDevice2.LocationId,
			DeviceType = "RoomTemperature",
			ActivityLogActive = true,
			Properties = new List<Property>
			{
				new StringProperty("UnderlyingCapabilityIds", string.Empty)
			}
		});
		list.Add(new RoomHumidity
		{
			BaseDevice = baseDevice2,
			Name = RoomHumidityName,
			LocationId = baseDevice2.LocationId,
			DeviceType = "RoomHumidity",
			ActivityLogActive = true,
			Properties = new List<Property>
			{
				new StringProperty("UnderlyingCapabilityIds", string.Empty)
			}
		});
		List<LogicalDevice> list2 = list;
		list2.ForEach(cfgRepo.SetLogicalDevice);
		baseDevice2.LogicalDeviceIds = list2.Select((LogicalDevice ld) => ld.Id).ToList();
		cfgRepo.SetBaseDevice(baseDevice2);
		return baseDevice2;
	}
}
