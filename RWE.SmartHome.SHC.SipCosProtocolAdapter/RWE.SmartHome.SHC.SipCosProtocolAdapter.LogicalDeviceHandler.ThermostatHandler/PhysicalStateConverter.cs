using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ThermostatHandler;

public class PhysicalStateConverter
{
	private readonly TemperatureActuatorStateCache stateCache;

	private readonly object syncRoot = new object();

	public PhysicalStateConverter(TemperatureActuatorStateCache stateCache)
	{
		this.stateCache = stateCache;
	}

	public LogicalDeviceState ConvertPhysicalState(ThermostatActuator thermostat, SortedList<byte, ChannelState> channelStates)
	{
		if (thermostat == null)
		{
			throw new ArgumentException("Passed logical device is not a ThermostatActuator");
		}
		lock (syncRoot)
		{
			ThermostatPhysicalState physicalState = stateCache.GetPhysicalState(thermostat.Id);
			physicalState.Update(channelStates, thermostat.WindowOpenTemperature);
			return Convert(thermostat, physicalState);
		}
	}

	private LogicalDeviceState Convert(ThermostatActuator thermostat, ThermostatPhysicalState phState)
	{
		ThermostatActuatorState thermostatActuatorState = new ThermostatActuatorState();
		thermostatActuatorState.LogicalDeviceId = thermostat.Id;
		thermostatActuatorState.OperationMode = phState.OperationMode;
		thermostatActuatorState.OperationModeUpdateTimestamp = ShcDateTime.UtcNow;
		thermostatActuatorState.WindowReductionActive = phState.IsWindowReduction;
		thermostatActuatorState.WindowReductionActiveUpdateTimestamp = ShcDateTime.UtcNow;
		thermostatActuatorState.PointTemperature = phState.SetpointTemperature;
		thermostatActuatorState.PointTemperatureUpdateTimestamp = ShcDateTime.UtcNow;
		return thermostatActuatorState;
	}
}
