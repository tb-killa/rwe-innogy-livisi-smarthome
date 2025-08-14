using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class DimmerConfiguration : ActuatorConfiguration
{
	private int MinPercent;

	private int MaxPercent;

	private PushButtonChannel InternalButton { get; set; }

	public ActuatorChannel<DimmerLink> Dimmer { get; private set; }

	public DimmerConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		Dimmer = new ActuatorChannel<DimmerLink>(1, 25);
		if (logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.PSD)
		{
			InternalButton = new PushButtonChannel(2, 8, needsLinkUpdatePendingFlag: false);
			InternalButton.Links[new LinkPartner(base.PhysicalDeviceId, address, Dimmer.ChannelIndex)] = new BaseLink();
			Dimmer.Links[new LinkPartner(base.PhysicalDeviceId, address, InternalButton.ChannelIndex)] = new DimmerLink();
		}
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
		if (!(base.LogicalDeviceContract is DimmerActuator dimmerActuator))
		{
			throw new ArgumentException("LogicalDevice type doesn't match DimmerConfiguration: " + base.LogicalDeviceContract.GetType().Name);
		}
		MinPercent = dimmerActuator.TechnicalMinValue;
		MaxPercent = dimmerActuator.TechnicalMaxValue;
		if (InternalButton != null)
		{
			DimmerLink dimmerLink = Dimmer.Links[new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, InternalButton.ChannelIndex)];
			dimmerLink.Minimum = MinPercent;
			dimmerLink.Maximum = MaxPercent;
			dimmerLink.LongPressAction = SwitchAction.DimToggle;
		}
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		Dimmer.SaveConfiguration(configuration.Channels);
		if (InternalButton != null)
		{
			InternalButton.SaveConfiguration(configuration.Channels);
		}
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		return new int[1] { Dimmer.ChannelIndex };
	}

	internal override IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		ToggleBehavior actuatorToggleBehavior = ToggleBehavior.ToggleToCounter;
		if (rule != null && rule.Actions.Count() == 1 && rule.Actions.Any((ActionDescription act) => act.ActionType.Equals("Toggle")))
		{
			actuatorToggleBehavior = ToggleBehavior.ToggleToState;
		}
		int? actionParameterValue = GetActionParameterValue(action, "DimLevel");
		int? actionParameterValue2 = GetActionParameterValue(action, "SwitchOffDelayTime");
		int? actionParameterValue3 = GetActionParameterValue(action, "RampOnTime");
		int? actionParameterValue4 = GetActionParameterValue(action, "RampOffTime");
		DimmerLink dimmerLink = new DimmerLink();
		dimmerLink.Minimum = MinPercent;
		dimmerLink.Maximum = MaxPercent;
		dimmerLink.OnLevel = actionParameterValue.GetValueOrDefault();
		dimmerLink.OffTimer = actionParameterValue2.GetValueOrDefault();
		dimmerLink.RampOnTime = actionParameterValue3.GetValueOrDefault();
		dimmerLink.RampOffTime = actionParameterValue4.GetValueOrDefault();
		dimmerLink.ComparisonValuePercent = comparisonValuePercent;
		dimmerLink.ActuatorToggleBehavior = actuatorToggleBehavior;
		DimmerLink dimmerLink2 = dimmerLink;
		if (switchEvent != ProfileAction.NoAction)
		{
			switchEvent = GetSwitchEventFromAction(action);
		}
		switch (switchEvent)
		{
		case ProfileAction.On:
			if (!actionParameterValue.HasValue)
			{
				return new LinkPartner[0];
			}
			dimmerLink2.ShortPressAction = SwitchAction.On;
			dimmerLink2.LongPressAction = SwitchAction.DimUp;
			break;
		case ProfileAction.Off:
			dimmerLink2.ShortPressAction = SwitchAction.Off;
			dimmerLink2.LongPressAction = SwitchAction.DimDown;
			break;
		case ProfileAction.Toggle:
			if (!actionParameterValue.HasValue)
			{
				return new LinkPartner[0];
			}
			dimmerLink2.ShortPressAction = SwitchAction.Toggle;
			dimmerLink2.LongPressAction = SwitchAction.DimToggle;
			break;
		case ProfileAction.NoAction:
			dimmerLink2.AboveAction = ConversionHelpers.GetConditionalSwitchAction(aboveEvent);
			dimmerLink2.BelowAction = ConversionHelpers.GetConditionalSwitchAction(belowEvent);
			break;
		default:
			throw new ArgumentOutOfRangeException("switchEvent");
		}
		Dimmer.AddLink(sensor, dimmerLink2);
		return new LinkPartner[1]
		{
			new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, Dimmer.ChannelIndex)
		};
	}

	private ProfileAction GetSwitchEventFromAction(ActionDescription action)
	{
		ProfileAction result = ProfileAction.On;
		switch (action.ActionType)
		{
		case "Toggle":
			result = ProfileAction.Toggle;
			break;
		case "SoftSwitchWithOffTimer":
		case "SetState":
		{
			int? actionParameterValue = GetActionParameterValue(action, "DimLevel");
			int? num = actionParameterValue;
			if (num.GetValueOrDefault() == 0 && num.HasValue)
			{
				result = ProfileAction.Off;
			}
			break;
		}
		}
		return result;
	}

	internal override IEnumerable<LinkPartner> CreateInternalLinks(LinkPartner sensor, InternalLinkType linkType)
	{
		SwitchAction switchAction = ConversionHelpers.FromInternalLinkType(linkType);
		DimmerLink dimmerLink = new DimmerLink();
		dimmerLink.ShortPressAction = switchAction;
		dimmerLink.LongPressAction = switchAction;
		DimmerLink config = dimmerLink;
		Dimmer.AddLink(sensor, config);
		return new LinkPartner[1]
		{
			new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, Dimmer.ChannelIndex)
		};
	}
}
