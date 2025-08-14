using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

public class CosIPCCIntegrityHandler : ICCIntegrityHandler
{
	private readonly IRepository repositoryProxy;

	public CosIPCCIntegrityHandler(IRepository repoProxy)
	{
		repositoryProxy = repoProxy;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		switch (eventType)
		{
		case ConfigEventType.BaseDeviceUpdated:
			HandleDeviceChange(eventType, entityId);
			break;
		case ConfigEventType.DeviceIncluded:
		case ConfigEventType.DeviceUpdated:
			HandleCapabilityChange(eventType, entityId);
			break;
		case ConfigEventType.BaseDeviceDeleted:
			break;
		}
	}

	private void HandleDeviceChange(ConfigEventType eventType, Guid entityId)
	{
		BaseDevice originalBaseDevice = repositoryProxy.GetOriginalBaseDevice(entityId);
		BaseDevice baseDevice = repositoryProxy.GetBaseDevice(entityId);
		if (baseDevice != null && originalBaseDevice != null && eventType == ConfigEventType.BaseDeviceUpdated)
		{
			HandleLocationChange(originalBaseDevice, baseDevice);
		}
	}

	private void HandleCapabilityChange(ConfigEventType eventType, Guid entityId)
	{
		ThermostatActuator thermostatActuator = repositoryProxy.GetLogicalDevice(entityId) as ThermostatActuator;
		ThermostatActuator origThermostat = repositoryProxy.GetOriginalLogicalDevice(entityId) as ThermostatActuator;
		switch (eventType)
		{
		case ConfigEventType.DeviceIncluded:
			UpdateDeviceWOpTemp(thermostatActuator);
			break;
		case ConfigEventType.DeviceUpdated:
			UpdateRoomWOpTemp(origThermostat, thermostatActuator);
			break;
		}
	}

	private void HandleLocationChange(BaseDevice origDevice, BaseDevice modifDevice)
	{
		if (origDevice.LocationId != modifDevice.LocationId)
		{
			ThermostatActuator thermostatActuator = (from ld in repositoryProxy.GetLogicalDevices()
				where ld.BaseDeviceId == modifDevice.Id
				select ld).OfType<ThermostatActuator>().FirstOrDefault();
			if (thermostatActuator != null)
			{
				UpdateDeviceWOpTemp(thermostatActuator);
			}
		}
	}

	private void UpdateRoomWOpTemp(ThermostatActuator origThermostat, ThermostatActuator modifiedThermostat)
	{
		if (origThermostat != null && modifiedThermostat != null && modifiedThermostat.WindowOpenTemperature != origThermostat.WindowOpenTemperature)
		{
			ApplyWOpTempFromDevice(modifiedThermostat);
		}
	}

	private void UpdateDeviceWOpTemp(ThermostatActuator modifThermostat)
	{
		if (modifThermostat != null)
		{
			ThermostatActuator thermostatActuator = GetRoomThermostats(modifThermostat.BaseDevice.LocationId).FirstOrDefault((ThermostatActuator tt) => tt.Id != modifThermostat.Id);
			if (thermostatActuator != null && modifThermostat.WindowOpenTemperature != thermostatActuator.WindowOpenTemperature)
			{
				modifThermostat.WindowOpenTemperature = thermostatActuator.WindowOpenTemperature;
				repositoryProxy.SetLogicalDevice(modifThermostat);
			}
		}
	}

	private void ApplyWOpTempFromDevice(ThermostatActuator modifiedThermostat)
	{
		Guid? locationId = modifiedThermostat.BaseDevice.LocationId;
		if (!locationId.HasValue)
		{
			return;
		}
		decimal targetWOpTemp = modifiedThermostat.WindowOpenTemperature;
		IEnumerable<ThermostatActuator> enumerable = from ta in GetRoomThermostats(locationId)
			where ta.Id != modifiedThermostat.Id && ta.WindowOpenTemperature != targetWOpTemp
			select ta;
		foreach (ThermostatActuator item in enumerable)
		{
			item.WindowOpenTemperature = targetWOpTemp;
			repositoryProxy.SetLogicalDevice(item);
		}
	}

	private IEnumerable<ThermostatActuator> GetRoomThermostats(Guid? locationId)
	{
		IEnumerable<ThermostatActuator> result = new List<ThermostatActuator>();
		if (locationId.HasValue)
		{
			result = repositoryProxy.GetLogicalDevices().Where(delegate(LogicalDevice ld)
			{
				Guid? locationId2 = ld.BaseDevice.LocationId;
				Guid? guid = locationId;
				return locationId2 == guid;
			}).OfType<ThermostatActuator>();
		}
		return result;
	}
}
