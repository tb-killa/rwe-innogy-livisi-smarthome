using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal abstract class UnderlyingSensorDevice : UnderlyingDevice
{
	protected UnderlyingSensorDevice(LogicalDevice logicalDevice, CompositeDevice compositeDevice)
		: base(logicalDevice, compositeDevice)
	{
	}

	public override IEnumerable<ActionDescription> HandleStateChange(LogicalDeviceStateChangedEventArgs stateUpdate)
	{
		base.StateValue = stateUpdate.NewLogicalDeviceState.GetStateValue();
		return new ActionDescription[1] { base.CompositeDevice.GetSetStateAction() };
	}
}
