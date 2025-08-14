using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal interface IVrccDevice
{
	Guid Id { get; }

	Guid BaseDeviceId { get; }

	IEnumerable<ActionDescription> HandleStateChange(LogicalDeviceStateChangedEventArgs logicalDeviceState);

	IEnumerable<Guid> GetGroupIds();
}
