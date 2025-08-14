using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class ThermostatActuatorDevice : UnderlyingDevice
{
	public ThermostatActuatorDevice(LogicalDevice logicalDevice, CompositeDevice compositeDevice)
		: base(logicalDevice, compositeDevice)
	{
	}

	public override IEnumerable<ActionDescription> HandleStateChange(LogicalDeviceStateChangedEventArgs stateUpdate)
	{
		List<ActionDescription> list = new List<ActionDescription>();
		decimal? stateValue = base.CompositeDevice.GetStateValue();
		if (stateUpdate.OldLogicalDeviceState == null && stateUpdate.NewLogicalDeviceState != null && stateValue.HasValue)
		{
			base.StateValue = stateValue;
			list.Add(GetState());
		}
		else
		{
			decimal? stateValue2 = stateUpdate.NewLogicalDeviceState.GetStateValue();
			List<UnderlyingDevice> list2 = base.CompositeDevice.UnderlyingDevices.Where((UnderlyingDevice ud) => ud.Id != base.Id && ud.StateValue != stateValue2).ToList();
			foreach (UnderlyingDevice item in list2)
			{
				item.StateValue = stateValue2;
				list.Add(item.GetState());
			}
			base.StateValue = stateValue2;
			list.Add(base.CompositeDevice.GetSetStateAction());
		}
		return list;
	}
}
