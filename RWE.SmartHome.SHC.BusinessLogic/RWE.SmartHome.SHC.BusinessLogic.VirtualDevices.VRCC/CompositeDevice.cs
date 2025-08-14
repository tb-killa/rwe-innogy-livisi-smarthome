using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal abstract class CompositeDevice : IVrccDevice
{
	public Guid Id { get; set; }

	public Guid BaseDeviceId { get; private set; }

	public List<UnderlyingDevice> UnderlyingDevices { get; private set; }

	public abstract string RelatedDevicePropertyName { get; }

	protected abstract string StatePropertyName { get; }

	public IEnumerable<Guid> GetGroupIds()
	{
		List<Guid> list = new List<Guid>(UnderlyingDevices.Count + 1);
		list.Add(Id);
		list.AddRange(UnderlyingDevices.Select((UnderlyingDevice x) => x.Id));
		return list;
	}

	protected CompositeDevice(LogicalDevice logicalDevice, IEnumerable<LogicalDevice> relatedeDevices)
	{
		Id = logicalDevice.Id;
		BaseDeviceId = logicalDevice.BaseDeviceId;
		Func<LogicalDevice, UnderlyingDevice> selector = (LogicalDevice x) => CreateUnderlyingDevice(x);
		UnderlyingDevices = relatedeDevices.Select(selector).ToList();
	}

	public abstract IEnumerable<ActionDescription> SetInitialState(LogicalDeviceState initialState);

	public abstract IEnumerable<ActionDescription> HandleStateChange(LogicalDeviceStateChangedEventArgs stateUpdate);

	public ActionDescription GetSetStateAction()
	{
		decimal? stateValue = GetStateValue();
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.ActionType = "SetState";
		actionDescription.Target = new LinkBinding(EntityType.LogicalDevice, Id);
		actionDescription.Data = new List<Parameter>
		{
			new Parameter
			{
				Name = StatePropertyName,
				Value = (stateValue.HasValue ? new ConstantNumericBinding
				{
					Value = stateValue.Value
				} : null)
			}
		};
		return actionDescription;
	}

	public abstract decimal? GetStateValue();

	protected decimal? CalculateAverage()
	{
		if (UnderlyingDevices.All((UnderlyingDevice ud) => !ud.StateValue.HasValue))
		{
			return null;
		}
		return UnderlyingDevices.Where((UnderlyingDevice ud) => ud.StateValue.HasValue).Average((UnderlyingDevice ud) => ud.StateValue);
	}

	protected FunctionBinding GetFunctionBinding()
	{
		FunctionBinding functionBinding = new FunctionBinding();
		functionBinding.Function = FunctionIdentifier.Average;
		FunctionBinding functionBinding2 = functionBinding;
		foreach (UnderlyingDevice underlyingDevice in UnderlyingDevices)
		{
			functionBinding2.Parameters.Add(new Parameter
			{
				Value = new FunctionBinding
				{
					Function = FunctionIdentifier.GetEntityStateProperty,
					Parameters = new List<Parameter>
					{
						new Parameter
						{
							Name = "EntityId",
							Value = new LinkBinding(EntityType.LogicalDevice, underlyingDevice.Id)
						},
						new Parameter
						{
							Name = "TargetPropertyName",
							Value = new ConstantStringBinding
							{
								Value = StatePropertyName
							}
						}
					}
				}
			});
		}
		return functionBinding2;
	}

	protected abstract UnderlyingDevice CreateUnderlyingDevice(LogicalDevice logicalDevice);
}
