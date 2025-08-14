using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class CcRotaryWheelChannel : BaseChannel<BaseLink>
{
	public decimal RotaryMinCentigrade { get; set; }

	public decimal RotaryMaxCentigrade { get; set; }

	public CcRotaryWheelChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount)
	{
		RotaryMinCentigrade = 6m;
		RotaryMaxCentigrade = 30m;
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		ConversionHelpers.CheckTemperatureRange(RotaryMinCentigrade, "MinTemperature");
		ConversionHelpers.CheckTemperatureRange(RotaryMaxCentigrade, "MaxTemperature");
		ConfigurationLink configurationLink = new ConfigurationLink();
		ParameterList parameterList = configurationLink[1];
		parameterList[17] = ConversionHelpers.ConvertCentigrade(RotaryMinCentigrade);
		parameterList[18] = ConversionHelpers.ConvertCentigrade(RotaryMaxCentigrade);
		return configurationLink;
	}
}
