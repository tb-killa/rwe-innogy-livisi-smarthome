using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

public class LogicalDeviceStateChangedEventArgs : EventArgs
{
	public Guid LogicalDeviceId { get; private set; }

	public LogicalDeviceState OldLogicalDeviceState { get; private set; }

	public LogicalDeviceState NewLogicalDeviceState { get; private set; }

	public LogicalDeviceStateChangedEventArgs(Guid logicalDeviceId, LogicalDeviceState oldDeviceState, LogicalDeviceState newDeviceState)
	{
		LogicalDeviceId = logicalDeviceId;
		OldLogicalDeviceState = oldDeviceState;
		NewLogicalDeviceState = newDeviceState;
	}
}
