using System;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

public class SetPointLink : ActuatorLink
{
	public SwitchAction ShortPressAction { get; set; }

	public decimal ShortPressOnCentigrade { get; set; }

	public decimal ShortPressOffCentigrade { get; set; }

	public SwitchAction LongPressAction { get; set; }

	public decimal LongPressOnCentigrade { get; set; }

	public decimal LongPressOffCentigrade { get; set; }

	public SetPointLink()
	{
		ShortPressAction = SwitchAction.Default;
		ShortPressOnCentigrade = 21m;
		ShortPressOffCentigrade = 17m;
		LongPressAction = SwitchAction.Default;
		LongPressOnCentigrade = 21m;
		LongPressOffCentigrade = 17m;
	}

	public override void SaveConfiguration(ConfigurationLink link)
	{
		base.SaveConfiguration(link);
		ConversionHelpers.CheckTemperatureRange(ShortPressOnCentigrade, "PointTemperature");
		ConversionHelpers.CheckTemperatureRange(ShortPressOffCentigrade, "PointTemperature");
		ConversionHelpers.CheckTemperatureRange(LongPressOnCentigrade, "PointTemperature");
		ConversionHelpers.CheckTemperatureRange(LongPressOffCentigrade, "PointTemperature");
		ParameterList parameterList = link[3];
		parameterList[10] = ConvertAction(ShortPressAction);
		parameterList[15] = ConversionHelpers.ConvertCentigrade(ShortPressOnCentigrade);
		parameterList[14] = ConversionHelpers.ConvertCentigrade(ShortPressOffCentigrade);
		parameterList[138] = ConvertAction(LongPressAction);
		parameterList[143] = ConversionHelpers.ConvertCentigrade(LongPressOnCentigrade);
		parameterList[142] = ConversionHelpers.ConvertCentigrade(LongPressOffCentigrade);
	}

	private static byte ConvertAction(SwitchAction action)
	{
		switch (action)
		{
		case SwitchAction.Default:
		case SwitchAction.On:
		case SwitchAction.Off:
			return 1;
		case SwitchAction.Toggle:
			return 2;
		default:
			throw new ArgumentException("action");
		}
	}
}
