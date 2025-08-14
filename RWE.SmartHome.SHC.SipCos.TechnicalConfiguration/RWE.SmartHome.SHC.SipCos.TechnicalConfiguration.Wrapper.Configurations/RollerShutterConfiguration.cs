using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class RollerShutterConfiguration : ActuatorConfiguration
{
	public RollerShutterActuatorChannel RollerShutterActuator { get; private set; }

	public RollerShutterConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		RollerShutterActuator = new RollerShutterActuatorChannel(1, 26);
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
		if (!(base.LogicalDeviceContract is RollerShutterActuator rollerShutterActuator))
		{
			throw new ArgumentException("LogicalDevice type doesn't match RollerShutterConfiguration: " + base.LogicalDeviceContract.GetType().Name);
		}
		RollerShutterActuator.RunningTimeTopBottom = rollerShutterActuator.TimeFullDown;
		RollerShutterActuator.RunningTimeBottomTop = rollerShutterActuator.TimeFullUp;
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		RollerShutterActuator.SaveConfiguration(configuration.Channels);
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		return new int[1] { RollerShutterActuator.ChannelIndex };
	}

	internal override IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		if (!(base.LogicalDeviceContract is RollerShutterActuator rollerShutterActuator))
		{
			throw new ArgumentException("LogicalDevice type doesn't match RollerShutterConfiguration: " + base.LogicalDeviceContract.GetType().Name);
		}
		bool isCalibrating = rollerShutterActuator.IsCalibrating && sensor.DeviceId == base.PhysicalDeviceId;
		RollerShutterLink rollerShutterLink = new RollerShutterLink();
		rollerShutterLink.OnLevel = GetActionParameterValue(action, "ShutterLevel").GetValueOrDefault();
		rollerShutterLink.OffLevel = 0;
		rollerShutterLink.ShortMovementTime = GetActionParameterValue(action, "StepDriveTime").GetValueOrDefault();
		rollerShutterLink.ComparisonValuePercent = comparisonValuePercent;
		rollerShutterLink.IsCalibrating = isCalibrating;
		RollerShutterLink rollerShutterLink2 = rollerShutterLink;
		ShutterControlBehaviorType controlBehavior = GetControlBehavior(action.Data);
		if (IsBuiltinButton(sensor))
		{
			switchEvent = GetSwitchEventFromTargetValue(action, rollerShutterLink2.OffLevel);
		}
		switch (switchEvent)
		{
		case ProfileAction.On:
			switch (controlBehavior)
			{
			case ShutterControlBehaviorType.Normal:
				rollerShutterLink2.ShortPressAction = SwitchAction.On;
				rollerShutterLink2.LongPressAction = SwitchAction.DimUp;
				break;
			case ShutterControlBehaviorType.Inverted:
				rollerShutterLink2.ShortPressAction = SwitchAction.DimUp;
				rollerShutterLink2.LongPressAction = SwitchAction.On;
				break;
			default:
				throw new ArgumentOutOfRangeException("switchEvent", "Unsupported ShutterControlBehavior");
			}
			break;
		case ProfileAction.Off:
			switch (controlBehavior)
			{
			case ShutterControlBehaviorType.Normal:
				rollerShutterLink2.ShortPressAction = SwitchAction.Off;
				rollerShutterLink2.LongPressAction = SwitchAction.DimDown;
				break;
			case ShutterControlBehaviorType.Inverted:
				rollerShutterLink2.ShortPressAction = SwitchAction.DimDown;
				rollerShutterLink2.LongPressAction = SwitchAction.Off;
				break;
			default:
				throw new ArgumentOutOfRangeException("switchEvent", "Unsupported ShutterControlBehavior");
			}
			break;
		case ProfileAction.Toggle:
			throw new IncompatibleButtonActionException();
		case ProfileAction.NoAction:
			rollerShutterLink2.AboveAction = ConversionHelpers.GetConditionalSwitchAction(aboveEvent);
			rollerShutterLink2.BelowAction = ConversionHelpers.GetConditionalSwitchAction(belowEvent);
			break;
		default:
			throw new ArgumentOutOfRangeException("switchEvent");
		}
		RollerShutterActuator.AddLink(sensor, rollerShutterLink2);
		RollerShutterLink rollerShutterLink3 = rollerShutterLink2.Clone();
		rollerShutterLink3.ShortPressAction = rollerShutterLink2.ShortPressAction;
		return new LinkPartner[1]
		{
			new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, RollerShutterActuator.ChannelIndex)
		};
	}

	private bool IsBuiltinButton(LinkPartner sensor)
	{
		if (sensor.Address[0] == base.PhysicalAddress[0] && sensor.Address[1] == base.PhysicalAddress[1])
		{
			return sensor.Address[2] == base.PhysicalAddress[2];
		}
		return false;
	}

	private ShutterControlBehaviorType GetControlBehavior(List<Parameter> actionData)
	{
		return actionData.GetStringValue("ShutterControlBehavior") switch
		{
			"Normal" => ShutterControlBehaviorType.Normal, 
			"Inverted" => ShutterControlBehaviorType.Inverted, 
			_ => throw new ArgumentException("Unknown or not specified shutter control behavior type"), 
		};
	}

	internal override IEnumerable<LinkPartner> CreateInternalLinks(LinkPartner sensor, InternalLinkType linkType)
	{
		SwitchAction switchAction = ConversionHelpers.FromInternalLinkType(linkType);
		RollerShutterLink rollerShutterLink = new RollerShutterLink();
		rollerShutterLink.ShortPressAction = switchAction;
		rollerShutterLink.LongPressAction = switchAction;
		RollerShutterLink config = rollerShutterLink;
		RollerShutterActuator.AddLink(sensor, config);
		return new LinkPartner[1]
		{
			new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, RollerShutterActuator.ChannelIndex)
		};
	}

	private ProfileAction GetSwitchEventFromTargetValue(ActionDescription action, int offLevel)
	{
		ProfileAction result = ProfileAction.On;
		switch (action.ActionType)
		{
		case "SetStateWithBehavior":
		case "SetState":
			if (GetActionParameterValue(action, "ShutterLevel") == offLevel)
			{
				result = ProfileAction.Off;
			}
			break;
		}
		return result;
	}
}
