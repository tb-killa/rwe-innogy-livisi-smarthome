using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

public class SwitchLink : ActuatorLink
{
	public SwitchAction ShortPressAction { get; set; }

	public SwitchAction LongPressAction { get; set; }

	public SwitchAction AboveAction { get; set; }

	public SwitchAction BelowAction { get; set; }

	public int? ComparisonValuePercent { get; set; }

	public int OffTimer { get; set; }

	public SensingBehaviorType? CurrentSensingBehavior { get; set; }

	public bool ForceLongPressAsShortPress { get; set; }

	public override void SaveConfiguration(ConfigurationLink link)
	{
		base.SaveConfiguration(link);
		byte b = ConversionHelpers.TenthSecondsToByte(OffTimer, nullIsInfinite: true, "OffTimer");
		bool withTimer = b != byte.MaxValue;
		byte? condValue = ConversionHelpers.ConvertPercentToByteRange(ComparisonValuePercent, "ComparisonValue");
		ParameterList parameterList = link[1];
		if (CurrentSensingBehavior.HasValue)
		{
			parameterList[31] = (byte)CurrentSensingBehavior.Value;
		}
		parameterList = link[3];
		ConfigureAction(ShortPressAction, b, parameterList, withTimer, condValue, longPress: false);
		ConfigureAction(LongPressAction, b, parameterList, withTimer, condValue, longPress: true);
	}

	private void ConfigureAction(SwitchAction action, byte offDelay, ParameterList list, bool withTimer, byte? condValue, bool longPress)
	{
		byte value = (byte)((action != SwitchAction.Toggle || base.ActuatorToggleBehavior != ToggleBehavior.ToggleToCounter || AboveAction != SwitchAction.Default || BelowAction != SwitchAction.Default) ? 1u : 2u);
		byte conditionalOperation = ConversionHelpers.GetConditionalOperation(ref action, BelowAction, AboveAction);
		byte value2;
		byte value3;
		if (longPress && ((action == SwitchAction.On && !withTimer) || action == SwitchAction.Off) && !ForceLongPressAsShortPress)
		{
			value2 = 0;
			value3 = 0;
		}
		else
		{
			switch (action)
			{
			case SwitchAction.Default:
			case SwitchAction.Toggle:
				value2 = 20;
				value3 = 99;
				break;
			case SwitchAction.On:
			case SwitchAction.UpButton:
				value2 = 19;
				value3 = 51;
				break;
			case SwitchAction.Off:
			case SwitchAction.DownButton:
				value2 = 100;
				value3 = 102;
				break;
			default:
				throw new ArgumentOutOfRangeException("action");
			}
		}
		int num = (longPress ? 128 : 0);
		list[(byte)(num + 10)] = value;
		list[(byte)(num + 11)] = value2;
		list[(byte)(num + 12)] = value3;
		list[(byte)(num + 7)] = offDelay;
		list[(byte)(num + 6)] = 0;
		list[(byte)(num + 8)] = 0;
		list[(byte)(num + 9)] = byte.MaxValue;
		list[(byte)(num + 4)] = condValue ?? 50;
		list[(byte)(num + 5)] = condValue ?? 150;
		list[(byte)(num + 3)] = conditionalOperation;
		list[(byte)(num + 2)] = conditionalOperation;
	}
}
