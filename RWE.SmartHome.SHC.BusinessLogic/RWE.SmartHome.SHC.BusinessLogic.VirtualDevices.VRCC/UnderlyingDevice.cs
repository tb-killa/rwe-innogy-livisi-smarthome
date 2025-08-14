using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal abstract class UnderlyingDevice : IVrccDevice
{
	public Guid Id { get; private set; }

	public Guid BaseDeviceId { get; private set; }

	public Guid CompositeDeviceId => CompositeDevice.Id;

	public decimal? StateValue { get; set; }

	public string PrimaryPropertyName { get; private set; }

	public CompositeDevice CompositeDevice { get; private set; }

	public IEnumerable<Guid> GetGroupIds()
	{
		return CompositeDevice.GetGroupIds();
	}

	protected UnderlyingDevice(LogicalDevice logicalDevice, CompositeDevice compositeDevice)
	{
		Id = logicalDevice.Id;
		BaseDeviceId = logicalDevice.Id;
		PrimaryPropertyName = logicalDevice.PrimaryPropertyName;
		CompositeDevice = compositeDevice;
	}

	public ActionDescription GetState()
	{
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.ActionType = "SetState";
		actionDescription.Target = new LinkBinding(EntityType.LogicalDevice, Id);
		actionDescription.Data = new List<Parameter>
		{
			new Parameter
			{
				Name = PrimaryPropertyName,
				Value = (StateValue.HasValue ? new ConstantNumericBinding
				{
					Value = StateValue.Value
				} : null)
			}
		};
		return actionDescription;
	}

	public abstract IEnumerable<ActionDescription> HandleStateChange(LogicalDeviceStateChangedEventArgs stateUpdate);

	internal ActionDescription SetStateViaComposite(decimal? newStateValue)
	{
		if (StateValue != newStateValue)
		{
			StateValue = newStateValue;
			return GetState();
		}
		return null;
	}
}
