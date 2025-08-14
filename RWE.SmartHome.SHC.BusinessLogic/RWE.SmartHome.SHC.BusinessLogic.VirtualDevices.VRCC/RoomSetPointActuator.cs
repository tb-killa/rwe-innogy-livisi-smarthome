using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class RoomSetPointActuator : CompositeDevice
{
	protected override string StatePropertyName => "PointTemperature";

	public override string RelatedDevicePropertyName => "VRCCSetPoint";

	public RoomSetPointActuator(LogicalDevice logicalDevice, IEnumerable<LogicalDevice> relatedDevices)
		: base(logicalDevice, relatedDevices)
	{
	}

	protected override UnderlyingDevice CreateUnderlyingDevice(LogicalDevice logicalDevice)
	{
		return new ThermostatActuatorDevice(logicalDevice, this);
	}

	public override IEnumerable<ActionDescription> SetInitialState(LogicalDeviceState initialState)
	{
		if (initialState == null)
		{
			return new ActionDescription[1] { GetSetStateAction() };
		}
		return new ActionDescription[0];
	}

	public override decimal? GetStateValue()
	{
		return base.UnderlyingDevices.FirstOrDefault((UnderlyingDevice x) => x.StateValue.HasValue)?.StateValue;
	}

	public override IEnumerable<ActionDescription> HandleStateChange(LogicalDeviceStateChangedEventArgs stateUpdate)
	{
		decimal? newStateValue = stateUpdate.NewLogicalDeviceState.GetStateValue();
		return (from x in base.UnderlyingDevices
			select x.SetStateViaComposite(newStateValue) into a
			where a != null
			select a).ToList();
	}
}
