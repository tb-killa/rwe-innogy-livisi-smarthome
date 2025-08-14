using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ThermostatHandler;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class TemperatureActuatorStateCache
{
	private object lockContext = new object();

	private readonly Dictionary<Guid, ThermostatPhysicalState> deviceStates = new Dictionary<Guid, ThermostatPhysicalState>();

	public ThermostatPhysicalState GetPhysicalState(Guid deviceId)
	{
		lock (lockContext)
		{
			ThermostatPhysicalState value = null;
			if (!deviceStates.TryGetValue(deviceId, out value))
			{
				value = new ThermostatPhysicalState();
				deviceStates.Add(deviceId, value);
			}
			return value;
		}
	}

	public void RemoveDeviceState(Guid deviceId)
	{
		lock (lockContext)
		{
			deviceStates.Remove(deviceId);
		}
	}
}
