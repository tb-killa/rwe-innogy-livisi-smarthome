using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class RoomSetpointConfiguration : ActuatorConfiguration
{
	private IList<CcActuatorConfiguration> underlyingActuatorConfigurations { get; set; }

	private CcActuatorConfiguration MasterClimateController { get; set; }

	public RoomSetpointConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensorConfigurations, IDictionary<Guid, ActuatorConfiguration> actuatorConfigurations)
	{
		base.InitializeConfiguration(shcAddresses, sensorConfigurations, actuatorConfigurations);
		if (!(base.LogicalDeviceContract is RoomSetpoint))
		{
			throw new ArgumentException("Expected RoomSetpoint as LogicalDevice");
		}
		underlyingActuatorConfigurations = CollectUnderlyingActuatorConfigurations(actuatorConfigurations);
		MasterClimateController = GetMasterClimateController(underlyingActuatorConfigurations);
		IEnumerable<CcActuatorConfiguration> allWrtsNoMaster = GetAllWrtsNoMaster(underlyingActuatorConfigurations);
		UpdateUnderlyingDeviceConfigurations(underlyingActuatorConfigurations);
		foreach (CcActuatorConfiguration underlyingActuatorConfiguration in underlyingActuatorConfigurations)
		{
			if (MasterClimateController != null)
			{
				underlyingActuatorConfiguration.LinkWithMasterDevice(MasterClimateController);
				underlyingActuatorConfiguration.LinkWithOtherDevices(allWrtsNoMaster);
			}
			else
			{
				underlyingActuatorConfiguration.LinkWithOtherDevices(underlyingActuatorConfigurations);
			}
		}
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
	}

	internal override IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		if (MasterClimateController != null)
		{
			return LinkDevices(MasterClimateController, sensor, action, switchEvent, aboveEvent, belowEvent, comparisonValuePercent, rule);
		}
		return underlyingActuatorConfigurations.SelectMany((CcActuatorConfiguration actuator) => LinkDevices(actuator, sensor, action, switchEvent, aboveEvent, belowEvent, comparisonValuePercent, rule)).ToList();
	}

	private IEnumerable<LinkPartner> LinkDevices(CcActuatorConfiguration actuator, LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		return actuator.CreateLinks(sensor, action, switchEvent, aboveEvent, belowEvent, comparisonValuePercent, rule);
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		return new List<int>();
	}

	public override bool AddDeviceSetpoint(TimeSpan timeOfDay, byte weekdays, ActionDescription action)
	{
		RoomSetpoint roomSetpoint = base.LogicalDeviceContract as RoomSetpoint;
		bool flag = true;
		if (roomSetpoint != null)
		{
			if (MasterClimateController != null)
			{
				flag = MasterClimateController.AddDeviceSetpoint(timeOfDay, weekdays, action);
			}
			else
			{
				foreach (ThermostatActuatorConfiguration item in underlyingActuatorConfigurations.OfType<ThermostatActuatorConfiguration>())
				{
					flag |= item.AddDeviceSetpoint(timeOfDay, weekdays, action);
				}
			}
		}
		return flag;
	}

	private List<CcActuatorConfiguration> CollectUnderlyingActuatorConfigurations(IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		return (from actCfg in actuators.Values.OfType<CcActuatorConfiguration>()
			where IsInSameRoom(base.LogicalDeviceContract, actCfg)
			select actCfg).ToList();
	}

	private CcActuatorConfiguration GetMasterClimateController(IEnumerable<CcActuatorConfiguration> underlyingDeviceConfigurations)
	{
		return (from m in underlyingDeviceConfigurations.OfType<ThermostatActuatorConfiguration>()
			where m.IsWrt
			orderby (!m.PhysicalDeviceTimeOfAcceptance.HasValue) ? DateTime.MaxValue : m.PhysicalDeviceTimeOfAcceptance.Value
			select m).FirstOrDefault();
	}

	private IEnumerable<CcActuatorConfiguration> GetAllWrtsNoMaster(IEnumerable<CcActuatorConfiguration> underlyingDeviceConfigurations)
	{
		return underlyingDeviceConfigurations.Where(delegate(CcActuatorConfiguration m)
		{
			ThermostatActuatorConfiguration thermostatActuatorConfiguration = m as ThermostatActuatorConfiguration;
			return m != MasterClimateController && thermostatActuatorConfiguration != null && thermostatActuatorConfiguration.IsWrt;
		});
	}

	private void UpdateUnderlyingDeviceConfigurations(IEnumerable<CcActuatorConfiguration> underlyingDeviceConfigurations)
	{
		foreach (ThermostatActuatorConfiguration item in underlyingDeviceConfigurations.OfType<ThermostatActuatorConfiguration>())
		{
			item.HasMaster = MasterClimateController != null;
		}
	}

	private bool IsInSameRoom(LogicalDevice vrcc, ActuatorConfiguration actCfg)
	{
		if (vrcc.BaseDevice != null && vrcc.BaseDevice.LocationId.HasValue)
		{
			if (!(actCfg.LogicalDeviceContract.BaseDevice.LocationId == vrcc.BaseDevice.LocationId))
			{
				Guid? locationId = actCfg.LogicalDeviceContract.LocationId;
				Guid? locationId2 = vrcc.BaseDevice.LocationId;
				if (locationId.HasValue == locationId2.HasValue)
				{
					if (locationId.HasValue)
					{
						return locationId.GetValueOrDefault() == locationId2.GetValueOrDefault();
					}
					return true;
				}
				return false;
			}
			return true;
		}
		return false;
	}
}
