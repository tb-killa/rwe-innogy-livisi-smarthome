using System;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

public class RollerShutterLink : ActuatorLink
{
	public SwitchAction ShortPressAction { get; set; }

	public SwitchAction LongPressAction { get; set; }

	public SwitchAction AboveAction { get; set; }

	public SwitchAction BelowAction { get; set; }

	public int? ComparisonValuePercent { get; set; }

	public int OnLevel { get; set; }

	public int OffLevel { get; set; }

	public int ShortMovementTime { get; set; }

	public bool IsCalibrating { get; set; }

	public RollerShutterLink()
	{
		ShortPressAction = SwitchAction.Default;
		LongPressAction = SwitchAction.Default;
		AboveAction = SwitchAction.Default;
		BelowAction = SwitchAction.Default;
		OnLevel = 100;
		OffLevel = 0;
	}

	public override void SaveConfiguration(ConfigurationLink link)
	{
		base.SaveConfiguration(link);
		ConversionHelpers.CheckPercentageRange(OnLevel, "OnLevel");
		ConversionHelpers.CheckPercentageRange(OffLevel, "OffLevel");
		byte onLevel = (byte)(2 * OnLevel);
		byte offLevel = (byte)(2 * OffLevel);
		byte? condValue = ConversionHelpers.ConvertPercentToByteRange(ComparisonValuePercent, "ComparisonValue");
		ParameterList list = link[3];
		ConfigureAction(list, onLevel, offLevel, ShortPressAction, condValue, longPress: false);
		ConfigureAction(list, onLevel, offLevel, LongPressAction, condValue, longPress: true);
	}

	private void ConfigureAction(ParameterList list, byte onLevel, byte offLevel, SwitchAction action, byte? condValue, bool longPress)
	{
		byte value = (byte)((!longPress) ? 1u : 37u);
		byte value2 = (byte)(longPress ? 3u : 255u);
		byte value3 = (byte)(longPress ? 137u : 20u);
		byte value4 = (byte)(longPress ? 152u : 82u);
		byte value5 = (byte)(longPress ? 152u : 99u);
		byte value6 = (byte)(longPress ? 152u : 99u);
		byte conditionalOperation = ConversionHelpers.GetConditionalOperation(ref action, BelowAction, AboveAction);
		switch (action)
		{
		case SwitchAction.UpButton:
			value = (byte)((!longPress) ? 1u : 33u);
			value2 = (byte)(longPress ? 3u : 255u);
			value3 = (byte)(longPress ? 136u : 17u);
			value4 = (byte)(longPress ? 136u : 18u);
			value5 = (byte)(longPress ? 136u : 99u);
			value6 = (byte)(longPress ? 136u : 99u);
			break;
		case SwitchAction.DownButton:
			value = (byte)((!longPress) ? 1u : 33u);
			value2 = (byte)(longPress ? 3u : 255u);
			value3 = (byte)(longPress ? 153u : 68u);
			value4 = (byte)(longPress ? 153u : 84u);
			value5 = (byte)(longPress ? 153u : 99u);
			value6 = (byte)(longPress ? 153u : 99u);
			break;
		case SwitchAction.On:
			value = (byte)((!longPress) ? 1u : 33u);
			value2 = byte.MaxValue;
			value3 = 136;
			value4 = 136;
			value5 = 136;
			value6 = 136;
			break;
		case SwitchAction.Off:
			value = (byte)((!longPress) ? 1u : 33u);
			value2 = byte.MaxValue;
			value3 = 153;
			value4 = 153;
			value5 = 153;
			value6 = 153;
			break;
		case SwitchAction.DimUp:
			value = (byte)((!longPress) ? 1u : 33u);
			value2 = (byte)(longPress ? 3u : ((uint)ShortMovementTime));
			value3 = (byte)(longPress ? 136u : 17u);
			value4 = (byte)(longPress ? 136u : 18u);
			value5 = (byte)(longPress ? 136u : 99u);
			value6 = (byte)(longPress ? 136u : 99u);
			break;
		case SwitchAction.DimDown:
			value = (byte)((!longPress) ? 1u : 33u);
			value2 = (byte)(longPress ? 3u : ((uint)ShortMovementTime));
			value3 = (byte)(longPress ? 153u : 68u);
			value4 = (byte)(longPress ? 153u : 84u);
			value5 = (byte)(longPress ? 153u : 99u);
			value6 = (byte)(longPress ? 153u : 99u);
			break;
		case SwitchAction.Toggle:
			value = 6;
			value2 = byte.MaxValue;
			break;
		default:
			throw new ArgumentOutOfRangeException("action", "Unknown SwitchAction for the RollerShutterLink: " + action);
		case SwitchAction.Default:
			break;
		}
		if (IsCalibrating)
		{
			value2 = 3;
		}
		int num = (longPress ? 128 : 0);
		list[(byte)(num + 10)] = value;
		list[(byte)(num + 11)] = value3;
		list[(byte)(num + 12)] = value4;
		list[(byte)(num + 13)] = value5;
		list[(byte)(num + 23)] = value6;
		list[(byte)(num + 15)] = onLevel;
		list[(byte)(num + 7)] = byte.MaxValue;
		list[(byte)(num + 6)] = 0;
		list[(byte)(num + 14)] = offLevel;
		list[(byte)(num + 8)] = 0;
		list[(byte)(num + 9)] = byte.MaxValue;
		list[(byte)(num + 4)] = condValue ?? 50;
		list[(byte)(num + 5)] = condValue ?? 150;
		list[(byte)(num + 3)] = conditionalOperation;
		list[(byte)(num + 2)] = conditionalOperation;
		list[(byte)(num + 1)] = conditionalOperation;
		list[(byte)(num + 21)] = conditionalOperation;
		list[(byte)(num + 24)] = 0;
		list[(byte)(num + 22)] = value2;
	}

	internal RollerShutterLink Clone()
	{
		RollerShutterLink rollerShutterLink = new RollerShutterLink();
		rollerShutterLink.AboveAction = AboveAction;
		rollerShutterLink.BelowAction = BelowAction;
		rollerShutterLink.ComparisonValuePercent = ComparisonValuePercent;
		rollerShutterLink.ConfigurationPending = base.ConfigurationPending;
		rollerShutterLink.IsCalibrating = IsCalibrating;
		rollerShutterLink.ShortPressAction = ShortPressAction;
		rollerShutterLink.LongPressAction = LongPressAction;
		rollerShutterLink.OnLevel = OnLevel;
		rollerShutterLink.OffLevel = OffLevel;
		rollerShutterLink.ShortMovementTime = ShortMovementTime;
		return rollerShutterLink;
	}
}
