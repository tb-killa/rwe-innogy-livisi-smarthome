using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement.ClimateControl;

internal class VRCCIntegrityHandler : ICCIntegrityHandler
{
	private class LocationChange
	{
		public Guid? oldLocId { get; set; }

		public Guid? newLocId { get; set; }
	}

	private readonly IRepository repositoryProxy;

	public VRCCIntegrityHandler(IRepository repositoryProxy)
	{
		this.repositoryProxy = repositoryProxy;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		LocationChange locationChange = CheckLocationChange(eventType, entityId);
		if (locationChange != null)
		{
			if (locationChange.oldLocId.HasValue)
			{
				UpdateLocationClimateControl(locationChange.oldLocId.Value);
			}
			if (locationChange.newLocId.HasValue)
			{
				UpdateLocationClimateControl(locationChange.newLocId.Value);
			}
		}
	}

	private void RemoveVRCCDevice(Guid locationId)
	{
		BaseDevice locationVRCC = GetLocationVRCC(locationId);
		if (locationVRCC != null)
		{
			Log.Debug(Module.BusinessLogic, "VRCCIntegrityHandler", "Removing VRCC from location: " + locationVRCC.LocationId);
			repositoryProxy.DeleteBaseDevice(locationVRCC.Id);
		}
	}

	private void UpdateLocationClimateControl(Guid locationId)
	{
		if (repositoryProxy.GetLocation(locationId) == null)
		{
			Log.Error(Module.BusinessLogic, "VRCCIntegrityHandler", "The location information associated with the CC device is invalid. Cannot update VRCC without a valid location information!");
			return;
		}
		BaseDevice locationVRCC = GetLocationVRCC(locationId);
		if (locationVRCC == null)
		{
			Log.Debug(Module.BusinessLogic, "VRCCIntegrityHandler", "Creating VRCC for location: " + locationId);
			VRCCDeviceCreator.CreateVRCCDevice(repositoryProxy, locationId);
		}
		UpdateLocationCCCapabilities(locationId);
		UpdateVRCCUnderlyingDevices(GetLocationVRCC(locationId));
		if (!AnyRoomCCDevice(locationId))
		{
			RemoveVRCCDevice(locationId);
		}
	}

	private bool AnyRoomCCDevice(Guid locationId)
	{
		return repositoryProxy.GetLogicalDevices().Any((LogicalDevice ld) => (ld.LocationId == locationId || ld.BaseDevice.LocationId == locationId) && ld.IsVrccCompatibleDevice());
	}

	internal void UpdateLocationCCCapabilities(Guid locationId)
	{
		List<LogicalDevice> roomVRCCCapabilities = GetRoomVRCCCapabilities(locationId);
		foreach (LogicalDevice item in roomVRCCCapabilities)
		{
			UpdateVRCCUnderlyingCapabilities(locationId, item);
		}
	}

	private void UpdateVRCCUnderlyingCapabilities(Guid locationId, LogicalDevice vrccCapability)
	{
		string deviceType = vrccCapability.DeviceType;
		IEnumerable<Guid> underlyingIds = null;
		switch (deviceType)
		{
		case "RoomSetpoint":
			underlyingIds = from cap in GetVRCCCompatibleCapabilities(locationId, "VRCCSetPoint")
				select cap.Id;
			break;
		case "RoomHumidity":
			underlyingIds = from cap in GetVRCCCompatibleCapabilities(locationId, "VRCCHumidity")
				select cap.Id;
			break;
		case "RoomTemperature":
			underlyingIds = from cap in GetVRCCCompatibleCapabilities(locationId, "VRCCTemperature")
				select cap.Id;
			break;
		}
		UpdateUnderlyingIds(vrccCapability.Properties, underlyingIds, "UnderlyingCapabilityIds");
		repositoryProxy.SetLogicalDevice(vrccCapability);
	}

	private List<LogicalDevice> GetRoomVRCCCapabilities(Guid locationId)
	{
		BaseDevice locationVRCC = GetLocationVRCC(locationId);
		if (locationVRCC == null)
		{
			throw new NullReferenceException("No VRCC device found for location: " + locationId);
		}
		List<LogicalDevice> vRCCCapabilities = GetVRCCCapabilities(locationVRCC);
		if (vRCCCapabilities.Count < 3)
		{
			Log.Error(Module.BusinessLogic, "VRCCIntegrityHandler", "Missing VRCC capabilities. Corrupted configuration?");
			throw new ArgumentException();
		}
		return vRCCCapabilities;
	}

	private void UpdateVRCCUnderlyingDevices(BaseDevice vrccDevice)
	{
		if (vrccDevice == null)
		{
			throw new ArgumentNullException("VRCC device is null. Unable to update underlying device list!");
		}
		List<Guid> underlyingIds = (from dev in repositoryProxy.GetLogicalDevices()
			where dev.BaseDevice.LocationId == vrccDevice.LocationId && dev.IsVrccCompatibleDevice()
			select dev.BaseDeviceId).Distinct().ToList();
		UpdateUnderlyingIds(vrccDevice.Properties, underlyingIds, "UnderlyingDeviceIds");
		repositoryProxy.SetBaseDevice(vrccDevice);
	}

	private List<LogicalDevice> GetVRCCCapabilities(BaseDevice vrccDevice)
	{
		return (from ld in repositoryProxy.GetLogicalDevices()
			where ld.BaseDeviceId == vrccDevice.Id
			select ld).ToList();
	}

	private void UpdateUnderlyingIds(List<Property> properties, IEnumerable<Guid> underlyingIds, string propertyName)
	{
		if (properties.FirstOrDefault((Property prop) => prop.Name == propertyName) is StringProperty stringProperty && underlyingIds != null)
		{
			stringProperty.Value = string.Join(",", underlyingIds.Select((Guid devId) => devId.ToString("N")).ToArray());
		}
	}

	internal List<LogicalDevice> GetVRCCCompatibleCapabilities(Guid locationId, string vrccCapability)
	{
		return (from dev in repositoryProxy.GetLogicalDevices()
			where (dev.LocationId == locationId || dev.BaseDevice.LocationId == locationId) && dev.Properties.Any((Property p) => p.Name == vrccCapability)
			select dev).ToList();
	}

	private BaseDevice GetLocationVRCC(Guid locationId)
	{
		return repositoryProxy.GetBaseDevices().FirstOrDefault((BaseDevice bd) => bd.DeviceType == BuiltinPhysicalDeviceType.VRCC.ToString() && bd.LocationId == locationId);
	}

	private LocationChange CheckLocationChange(ConfigEventType eventType, Guid entityId)
	{
		LocationChange result = null;
		switch (eventType)
		{
		case ConfigEventType.BaseDeviceUpdated:
		{
			BaseDevice originalBaseDevice2 = repositoryProxy.GetOriginalBaseDevice(entityId);
			BaseDevice baseDevice = repositoryProxy.GetBaseDevice(entityId);
			if (originalBaseDevice2 != null && baseDevice != null && originalBaseDevice2.LocationId != baseDevice.LocationId)
			{
				LocationChange locationChange4 = new LocationChange();
				locationChange4.oldLocId = originalBaseDevice2.LocationId;
				locationChange4.newLocId = baseDevice.LocationId;
				result = locationChange4;
			}
			break;
		}
		case ConfigEventType.DeviceUpdated:
		{
			LogicalDevice originalLogicalDevice = repositoryProxy.GetOriginalLogicalDevice(entityId);
			LogicalDevice logicalDevice2 = repositoryProxy.GetLogicalDevice(entityId);
			if (originalLogicalDevice != null && logicalDevice2 != null && originalLogicalDevice.LocationId != logicalDevice2.LocationId)
			{
				LocationChange locationChange3 = new LocationChange();
				locationChange3.oldLocId = originalLogicalDevice.LocationId;
				locationChange3.newLocId = logicalDevice2.LocationId;
				result = locationChange3;
			}
			break;
		}
		case ConfigEventType.DeviceIncluded:
		{
			LogicalDevice logicalDevice = repositoryProxy.GetLogicalDevice(entityId);
			if (logicalDevice != null && (logicalDevice.LocationId.HasValue || logicalDevice.BaseDevice.LocationId.HasValue))
			{
				LocationChange locationChange2 = new LocationChange();
				locationChange2.newLocId = logicalDevice.LocationId ?? logicalDevice.BaseDevice.LocationId;
				result = locationChange2;
			}
			break;
		}
		case ConfigEventType.BaseDeviceDeleted:
		{
			BaseDevice originalBaseDevice = repositoryProxy.GetOriginalBaseDevice(entityId);
			LocationChange locationChange = new LocationChange();
			locationChange.oldLocId = originalBaseDevice.LocationId;
			result = locationChange;
			break;
		}
		}
		return result;
	}
}
