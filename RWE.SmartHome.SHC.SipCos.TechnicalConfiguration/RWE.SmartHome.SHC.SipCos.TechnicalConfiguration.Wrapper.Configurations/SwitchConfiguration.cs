using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class SwitchConfiguration : ActuatorConfiguration
{
	private enum LongPressBehavior
	{
		Normal,
		SameAsShortPress
	}

	public ActuatorChannel<SwitchLink> Switch { get; private set; }

	public SwitchConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		Switch = new ActuatorChannel<SwitchLink>(1, 24);
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		Switch.SaveConfiguration(configuration.Channels);
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		return new int[1] { Switch.ChannelIndex };
	}

	private bool IsToggleAction(ActionDescription action)
	{
		return action.ActionType == "Toggle";
	}

	private SwitchLink GetUnconditionalSwitchLink(ActionDescription action, ProfileAction switchEvent)
	{
		bool? targetState = GetTargetState(action);
		if (IsOnEvent(switchEvent) && !targetState.HasValue)
		{
			return null;
		}
		new SwitchLink();
		SwitchAction switchAction = GetSwitchAction(action, switchEvent, targetState);
		SwitchLink switchLink = new SwitchLink();
		switchLink.ShortPressAction = switchAction;
		switchLink.LongPressAction = switchAction;
		return switchLink;
	}

	private bool IsOnEvent(ProfileAction switchEvent)
	{
		return switchEvent == ProfileAction.On;
	}

	private SwitchLink GetConditionalSwitchLink(ActionDescription action, ProfileAction aboveEvent, ProfileAction belowEvent)
	{
		bool? targetState = GetTargetState(action);
		if ((IsOnEvent(aboveEvent) || IsOnEvent(belowEvent)) && !targetState.HasValue)
		{
			return null;
		}
		SwitchAction switchAction = GetSwitchAction(action, aboveEvent, targetState);
		SwitchAction switchAction2 = GetSwitchAction(action, belowEvent, targetState);
		SwitchLink switchLink = new SwitchLink();
		switchLink.AboveAction = switchAction;
		switchLink.BelowAction = switchAction2;
		return switchLink;
	}

	private SwitchAction GetSwitchAction(ActionDescription action, ProfileAction switchEvent, bool? targetState)
	{
		if (IsToggleAction(action))
		{
			return SwitchAction.Toggle;
		}
		switch (switchEvent)
		{
		case ProfileAction.On:
			if (!targetState.Value)
			{
				return SwitchAction.Off;
			}
			return SwitchAction.On;
		case ProfileAction.Off:
			return SwitchAction.Off;
		case ProfileAction.Toggle:
			return SwitchAction.Toggle;
		case ProfileAction.NoAction:
			return SwitchAction.Default;
		default:
			throw new ArgumentOutOfRangeException("switchEvent");
		}
	}

	internal override IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		SwitchLink switchLink = null;
		switchLink = ((switchEvent == ProfileAction.NoAction) ? GetConditionalSwitchLink(action, aboveEvent, belowEvent) : GetUnconditionalSwitchLink(action, switchEvent));
		if (switchLink == null)
		{
			return new LinkPartner[0];
		}
		SwitchLink value = null;
		if (Switch.Links.TryGetValue(sensor, out value))
		{
			switchLink.AboveAction = ((switchLink.AboveAction == SwitchAction.Default) ? value.AboveAction : switchLink.AboveAction);
			switchLink.BelowAction = ((switchLink.BelowAction == SwitchAction.Default) ? value.BelowAction : switchLink.BelowAction);
			switchLink.OffTimer = ((value.OffTimer > 0) ? value.OffTimer : GetOffTimer(action).GetValueOrDefault());
			switchLink.ForceLongPressAsShortPress = GetLongPressBeahvior(rule) == LongPressBehavior.SameAsShortPress;
			switchLink.ActuatorToggleBehavior = GetToggleBehavior(rule);
			Switch.Links.Remove(sensor);
		}
		else
		{
			switchLink.OffTimer = GetOffTimer(action).GetValueOrDefault();
			switchLink.ComparisonValuePercent = comparisonValuePercent;
			switchLink.ForceLongPressAsShortPress = GetLongPressBeahvior(rule) == LongPressBehavior.SameAsShortPress;
			switchLink.ActuatorToggleBehavior = GetToggleBehavior(rule);
		}
		LogicalDevice logicalDevice = base.ConfigurationRepository.GetLogicalDevice(new Guid(action.Target.EntityId));
		if (logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.ISS2)
		{
			switchLink.CurrentSensingBehavior = ((SwitchActuator)logicalDevice).SensingBehavior;
		}
		Switch.AddLink(sensor, switchLink);
		return new LinkPartner[1]
		{
			new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, Switch.ChannelIndex)
		};
	}

	internal override IEnumerable<LinkPartner> CreateInternalLinks(LinkPartner sensor, InternalLinkType linkType)
	{
		SwitchAction switchAction = ConversionHelpers.FromInternalLinkType(linkType);
		SwitchLink switchLink = new SwitchLink();
		switchLink.ShortPressAction = switchAction;
		switchLink.LongPressAction = switchAction;
		SwitchLink switchLink2 = switchLink;
		if (base.LogicalDeviceContract.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.ISS2)
		{
			switchLink2.CurrentSensingBehavior = SensingBehaviorType.Enabled;
		}
		Switch.AddLink(sensor, switchLink2);
		return new LinkPartner[1]
		{
			new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, Switch.ChannelIndex)
		};
	}

	private bool? GetTargetState(ActionDescription action)
	{
		if (action == null)
		{
			return null;
		}
		if (action.ActionType == "Toggle" || action.ActionType == "SwitchOnWithOffTimer")
		{
			return true;
		}
		Parameter parameter = action.Data.FirstOrDefault((Parameter p) => p.Name == "OnState");
		if (parameter == null || !(parameter.Value is ConstantBooleanBinding))
		{
			return null;
		}
		return (parameter.Value as ConstantBooleanBinding).Value;
	}

	private int? GetOffTimer(ActionDescription action)
	{
		return (int?)action.Data.GetNumericValue("SwitchOffDelayTime");
	}

	private ToggleBehavior GetToggleBehavior(Rule rule)
	{
		ToggleBehavior result = ToggleBehavior.ToggleToCounter;
		if (rule != null && rule.Actions != null && rule.Actions.Count() == 1 && rule.Actions.Any(IsToggleAction))
		{
			result = ToggleBehavior.ToggleToState;
		}
		return result;
	}

	private LongPressBehavior GetLongPressBeahvior(Rule rule)
	{
		LongPressBehavior result = LongPressBehavior.Normal;
		if (rule != null)
		{
			foreach (ActionDescription action in rule.Actions)
			{
				LogicalDevice logicalDevice = base.ConfigurationRepository.GetLogicalDevice(action.Target.EntityIdAsGuid());
				BuiltinPhysicalDeviceType builtinPhysicalDeviceType = ((logicalDevice.BaseDevice != null) ? logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() : BuiltinPhysicalDeviceType.Unknown);
				if (builtinPhysicalDeviceType == BuiltinPhysicalDeviceType.ISR2 || builtinPhysicalDeviceType == BuiltinPhysicalDeviceType.PSD || builtinPhysicalDeviceType == BuiltinPhysicalDeviceType.ISD2)
				{
					result = LongPressBehavior.SameAsShortPress;
					break;
				}
			}
		}
		return result;
	}
}
