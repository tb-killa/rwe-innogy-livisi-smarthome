using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;

public interface ILogicalDeviceStateRepository
{
	LogicalDeviceState GetLogicalDeviceState(Guid logicalDeviceId);

	List<LogicalDeviceState> GetAllLogicalDeviceStates(params string[] deviceIds);
}
