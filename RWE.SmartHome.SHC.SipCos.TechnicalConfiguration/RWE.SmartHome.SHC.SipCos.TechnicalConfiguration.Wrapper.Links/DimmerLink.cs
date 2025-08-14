using System;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

public class DimmerLink : ActuatorLink
{
	public SwitchAction ShortPressAction { get; set; }

	public SwitchAction LongPressAction { get; set; }

	public SwitchAction AboveAction { get; set; }

	public SwitchAction BelowAction { get; set; }

	public int? ComparisonValuePercent { get; set; }

	public int OffTimer { get; set; }

	public int OnLevel { get; set; }

	public int Minimum { get; set; }

	public int Maximum { get; set; }

	public int RampOnTime { get; set; }

	public int RampOffTime { get; set; }

	public DimmerLink()
	{
		ShortPressAction = SwitchAction.Default;
		LongPressAction = SwitchAction.Default;
		AboveAction = SwitchAction.Default;
		BelowAction = SwitchAction.Default;
		OffTimer = 0;
		OnLevel = 100;
		Minimum = 10;
		Maximum = 100;
		RampOnTime = 0;
		RampOffTime = 0;
	}

	public override void SaveConfiguration(ConfigurationLink link)
	{
		base.SaveConfiguration(link);
		ConversionHelpers.CheckPercentageRange(OnLevel, "OnLevel");
		ConversionHelpers.CheckPercentageRange(Minimum, "Minimum");
		ConversionHelpers.CheckPercentageRange(Maximum, "Maximum");
		byte offDelay = ConversionHelpers.TenthSecondsToByte(OffTimer, nullIsInfinite: true, "OffTimer");
		byte rampOnTime = ConversionHelpers.TenthSecondsToByte(RampOnTime, nullIsInfinite: false, "RampOnTime");
		byte rampOffTime = ConversionHelpers.TenthSecondsToByte(RampOffTime, nullIsInfinite: false, "RampOffTime");
		byte onLevel = ConversionHelpers.ConvertDimLevel(Minimum, Maximum, OnLevel);
		byte minimum = (byte)(Minimum * 2);
		byte maximum = (byte)(Maximum * 2);
		byte? condValue = ConversionHelpers.ConvertPercentToByteRange(ComparisonValuePercent, "ComparisonValue");
		ParameterList list = link[3];
		ConfigureAction(list, offDelay, rampOnTime, rampOffTime, onLevel, minimum, maximum, ShortPressAction, condValue, longPress: false);
		ConfigureAction(list, offDelay, rampOnTime, rampOffTime, onLevel, minimum, maximum, LongPressAction, condValue, longPress: true);
	}

	private void ConfigureAction(ParameterList list, byte offDelay, byte rampOnTime, byte rampOffTime, byte onLevel, byte minimum, byte maximum, SwitchAction action, byte? condValue, bool longPress)
	{
		byte conditionalOperation = ConversionHelpers.GetConditionalOperation(ref action, BelowAction, AboveAction);
		byte value = 20;
		byte value2 = 82;
		byte value3 = 99;
		bool flag = false;
		byte value4;
		switch (action)
		{
		case SwitchAction.Default:
			value4 = (byte)(longPress ? 53u : 17u);
			break;
		case SwitchAction.UpButton:
			value4 = (byte)(longPress ? 51u : 17u);
			value = 18;
			value2 = 34;
			value3 = 35;
			break;
		case SwitchAction.DownButton:
			value4 = (byte)(longPress ? 52u : 17u);
			value = 100;
			value2 = 86;
			value3 = 100;
			break;
		case SwitchAction.On:
			value4 = 17;
			value = 18;
			value2 = 32;
			value3 = 32;
			break;
		case SwitchAction.Off:
			value4 = 17;
			value = 4;
			value2 = 6;
			value3 = 4;
			break;
		case SwitchAction.Toggle:
			if (AboveAction != SwitchAction.Default || BelowAction != SwitchAction.Default)
			{
				value4 = 17;
				value2 = 38;
				value3 = 37;
			}
			else
			{
				value4 = (byte)((base.ActuatorToggleBehavior == ToggleBehavior.ToggleToState) ? 1 : 18);
			}
			break;
		case SwitchAction.DimUp:
			value4 = (byte)(longPress ? 51u : 19u);
			flag = true;
			break;
		case SwitchAction.DimDown:
			value4 = (byte)(longPress ? 52u : 20u);
			flag = true;
			break;
		case SwitchAction.DimToggle:
			value4 = (byte)(longPress ? 54u : 22u);
			flag = true;
			break;
		default:
			throw new ArgumentOutOfRangeException("action", "Unknown SwitchAction for the DimmerLink" + action);
		}
		int num = (longPress ? 128 : 0);
		list[(byte)(num + 10)] = value4;
		list[(byte)(num + 11)] = value;
		list[(byte)(num + 12)] = value2;
		list[(byte)(num + 13)] = value3;
		list[(byte)(num + 16)] = GetMinLevel(action, minimum);
		list[(byte)(num + 19)] = minimum;
		list[(byte)(num + 20)] = maximum;
		list[(byte)(num + 15)] = (byte)(flag ? 200 : onLevel);
		list[(byte)(num + 7)] = (flag ? byte.MaxValue : offDelay);
		list[(byte)(num + 17)] = rampOnTime;
		list[(byte)(num + 18)] = rampOffTime;
		list[(byte)(num + 6)] = 0;
		list[(byte)(num + 8)] = 0;
		list[(byte)(num + 9)] = byte.MaxValue;
		list[(byte)(num + 4)] = condValue ?? 50;
		list[(byte)(num + 5)] = condValue ?? 150;
		list[(byte)(num + 3)] = conditionalOperation;
		list[(byte)(num + 2)] = conditionalOperation;
		list[(byte)(num + 1)] = conditionalOperation;
	}

	private byte GetMinLevel(SwitchAction action, byte minimum)
	{
		if (action != SwitchAction.Default || BelowAction != SwitchAction.Default || AboveAction != SwitchAction.Default)
		{
			return minimum;
		}
		return 20;
	}
}
