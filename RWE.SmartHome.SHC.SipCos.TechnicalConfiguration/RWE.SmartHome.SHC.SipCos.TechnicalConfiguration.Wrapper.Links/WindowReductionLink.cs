using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

public class WindowReductionLink : ActuatorLink
{
	public decimal OpenTemperatureCentigrade { get; set; }

	public int MaxOpenTimeSeconds { get; set; }

	public int? ComparisonValuePercent { get; set; }

	public WindowReductionLink()
	{
		OpenTemperatureCentigrade = 12m;
		MaxOpenTimeSeconds = 9000;
		base.ConfigurationPending = true;
	}

	public override void SaveConfiguration(ConfigurationLink link)
	{
		base.SaveConfiguration(link);
		ConversionHelpers.CheckTemperatureRange(OpenTemperatureCentigrade, "OpenTemperatureCentigrade");
		link[ConfigKeys.CcWindowTemp] = ConversionHelpers.ConvertCentigrade(OpenTemperatureCentigrade);
		byte? b = ConversionHelpers.ConvertPercentToByteRange(ComparisonValuePercent, "ComparisonValue");
		ParameterList parameterList = link[3];
		parameterList[3] = 2;
		parameterList[131] = 2;
		parameterList[4] = b ?? 50;
		parameterList[132] = b ?? 50;
		byte value = (parameterList[7] = ConversionHelpers.TenthSecondsToByte(MaxOpenTimeSeconds, nullIsInfinite: true, "MaxOpenTimeSeconds"));
		parameterList[135] = value;
	}
}
