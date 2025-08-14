using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class CcSensorChannel : BaseChannel<BaseLink>
{
	public ThermostateDisplayMode DisplayMode { get; set; }

	public bool IsFreezeProtectionActivated { get; set; }

	public decimal AntiFreezeCentigrade { get; set; }

	public bool IsMoldProtectionActivated { get; set; }

	public decimal AntiMoldPercent { get; set; }

	public decimal TemperatureOffsetCentigrade { get; set; }

	public CcSensorChannel(byte channelIndex)
		: base(channelIndex, (byte)0)
	{
		AntiFreezeCentigrade = 6m;
		AntiMoldPercent = 100m;
		TemperatureOffsetCentigrade = -1m;
		DisplayMode = ThermostateDisplayMode.TargetTemperatue;
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		byte antiMold = GetAntiMold();
		byte antiFreeze = GetAntiFreeze();
		ConfigurationLink configurationLink = new ConfigurationLink();
		ParameterList parameterList = configurationLink[1];
		parameterList[16] = antiMold;
		parameterList[15] = antiFreeze;
		parameterList[14] = ConversionHelpers.ConvertCentigrade(TemperatureOffsetCentigrade);
		switch (DisplayMode)
		{
		case ThermostateDisplayMode.TargetTemperatue:
			parameterList[33] = 0;
			break;
		case ThermostateDisplayMode.CurrentTemperature:
			parameterList[33] = 1;
			break;
		}
		return configurationLink;
	}

	private byte GetAntiFreeze()
	{
		if (!IsFreezeProtectionActivated)
		{
			return 12;
		}
		ConversionHelpers.CheckTemperatureRange(AntiFreezeCentigrade, "FreezeProtection");
		return (byte)(AntiFreezeCentigrade * 2m);
	}

	private byte GetAntiMold()
	{
		if (!IsMoldProtectionActivated)
		{
			return 200;
		}
		ConversionHelpers.CheckPercentageRange(AntiMoldPercent, "HumidityMoldProtection");
		return (byte)(AntiMoldPercent * 2m);
	}
}
