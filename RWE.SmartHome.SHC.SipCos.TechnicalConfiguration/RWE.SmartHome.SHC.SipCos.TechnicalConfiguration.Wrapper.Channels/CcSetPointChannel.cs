using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class CcSetPointChannel : ActuatorChannel<SetPointLink>
{
	public decimal? OffStateTemperature { get; set; }

	public CcSetPointChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount)
	{
	}

	public bool AddLink(LinkPartner sensor, ActionDescription action, SwitchAction switchAction, SwitchAction aboveAction, SwitchAction belowAction, Guid ruleId, Guid physicalDeviceId)
	{
		Parameter parameter = action.Data.SingleOrDefault((Parameter p) => p.Name == "PointTemperature");
		if (parameter == null || !(parameter.Value is ConstantNumericBinding))
		{
			return false;
		}
		if (switchAction == SwitchAction.Default || aboveAction != SwitchAction.Default || belowAction != SwitchAction.Default)
		{
			throw new IncompatibleLinkTypeException();
		}
		decimal? num = null;
		SwitchAction switchAction2;
		decimal value;
		switch (switchAction)
		{
		case SwitchAction.On:
			switchAction2 = SwitchAction.On;
			value = (parameter.Value as ConstantNumericBinding).Value;
			break;
		case SwitchAction.Off:
			if (!OffStateTemperature.HasValue)
			{
				return false;
			}
			switchAction2 = SwitchAction.On;
			value = OffStateTemperature.Value;
			break;
		case SwitchAction.Toggle:
			if (!OffStateTemperature.HasValue)
			{
				return false;
			}
			switchAction2 = SwitchAction.Toggle;
			value = (parameter.Value as ConstantNumericBinding).Value;
			num = OffStateTemperature.Value;
			break;
		default:
			throw new ArgumentOutOfRangeException("switchAction");
		}
		SetPointLink setPointLink = new SetPointLink();
		setPointLink.ShortPressAction = switchAction2;
		setPointLink.LongPressAction = switchAction2;
		setPointLink.ShortPressOnCentigrade = value;
		setPointLink.LongPressOnCentigrade = value;
		SetPointLink setPointLink2 = setPointLink;
		if (num.HasValue)
		{
			setPointLink2.ShortPressOffCentigrade = num.Value;
			setPointLink2.LongPressOffCentigrade = num.Value;
		}
		AddLink(sensor, setPointLink2);
		return true;
	}
}
