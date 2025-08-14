using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal abstract class CompositeSensorDevice : CompositeDevice
{
	protected CompositeSensorDevice(LogicalDevice logicalDevice, IEnumerable<LogicalDevice> relatedDevices)
		: base(logicalDevice, relatedDevices)
	{
	}

	public override IEnumerable<ActionDescription> SetInitialState(LogicalDeviceState initialState)
	{
		decimal? stateValue = GetStateValue();
		decimal? num = stateValue;
		decimal? stateValue2 = initialState.GetStateValue();
		if (!(num.GetValueOrDefault() != stateValue2.GetValueOrDefault()) && num.HasValue == stateValue2.HasValue)
		{
			return new ActionDescription[0];
		}
		return new ActionDescription[1] { GetSetStateAction() };
	}

	public override IEnumerable<ActionDescription> HandleStateChange(LogicalDeviceStateChangedEventArgs stateUpdate)
	{
		return new ActionDescription[0];
	}

	public override decimal? GetStateValue()
	{
		return CalculateAverage();
	}
}
