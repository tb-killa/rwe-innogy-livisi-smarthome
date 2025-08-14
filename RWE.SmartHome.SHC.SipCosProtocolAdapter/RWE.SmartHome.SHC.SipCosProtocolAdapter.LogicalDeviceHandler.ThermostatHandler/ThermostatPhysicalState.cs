using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ThermostatHandler;

public class ThermostatPhysicalState
{
	internal decimal? SetpointTemperature { get; private set; }

	internal bool? IsWindowReduction { get; private set; }

	internal OperationMode? OperationMode { get; private set; }

	public ThermostatPhysicalState()
	{
	}

	public ThermostatPhysicalState(SortedList<byte, ChannelState> channelStates)
	{
		SetpointTemperature = GetSetpoint(channelStates);
		IsWindowReduction = GetWindowReduction(channelStates);
		OperationMode = GetOperationMode(channelStates);
	}

	public void Update(SortedList<byte, ChannelState> channelStates, decimal setpointThreshold)
	{
		ThermostatPhysicalState thermostatPhysicalState = new ThermostatPhysicalState(channelStates);
		if (thermostatPhysicalState.OperationMode.HasValue)
		{
			OperationMode = thermostatPhysicalState.OperationMode;
		}
		if (thermostatPhysicalState.SetpointTemperature.HasValue)
		{
			SetpointTemperature = thermostatPhysicalState.SetpointTemperature;
		}
		if (thermostatPhysicalState.IsWindowReduction.HasValue)
		{
			IsWindowReduction = thermostatPhysicalState.IsWindowReduction;
		}
		bool? isWindowReduction = IsWindowReduction;
		if (isWindowReduction == true && isWindowReduction.HasValue && SetpointTemperature.HasValue)
		{
			SetpointTemperature = NormalizeWRSetpointValue(SetpointTemperature.Value, setpointThreshold);
		}
	}

	private decimal NormalizeWRSetpointValue(decimal currentSetpoint, decimal setpointThreshold)
	{
		decimal result = currentSetpoint;
		if (currentSetpoint > setpointThreshold)
		{
			result = setpointThreshold;
		}
		return result;
	}

	private bool? GetWindowReduction(SortedList<byte, ChannelState> channelStates)
	{
		ChannelState value = null;
		if (channelStates.TryGetValue(5, out value))
		{
			return value.Value != 0;
		}
		return null;
	}

	private decimal? GetSetpoint(SortedList<byte, ChannelState> channelStates)
	{
		ChannelState value = null;
		if (channelStates.TryGetValue(4, out value))
		{
			return (decimal)value.Value * 0.5m;
		}
		return null;
	}

	private OperationMode? GetOperationMode(SortedList<byte, ChannelState> channelStates)
	{
		ChannelState value = null;
		if (channelStates.TryGetValue(2, out value))
		{
			return (OperationMode)value.Value;
		}
		return null;
	}
}
