using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

public class RawLogicalDeviceStateChangedEventArgs : EventArgs
{
	public Guid DeviceId { get; private set; }

	public LogicalDeviceState LogicalDeviceState { get; private set; }

	public RawLogicalDeviceStateChangedEventArgs(Guid deviceId, LogicalDeviceState deviceState)
	{
		LogicalDeviceState = deviceState;
		DeviceId = deviceId;
		UpdateTimestamps();
	}

	private void UpdateTimestamps()
	{
		if (LogicalDeviceState == null)
		{
			return;
		}
		foreach (Property property in LogicalDeviceState.GetProperties())
		{
			if (!property.UpdateTimestamp.HasValue)
			{
				property.UpdateTimestamp = ShcDateTime.UtcNow;
			}
		}
	}
}
