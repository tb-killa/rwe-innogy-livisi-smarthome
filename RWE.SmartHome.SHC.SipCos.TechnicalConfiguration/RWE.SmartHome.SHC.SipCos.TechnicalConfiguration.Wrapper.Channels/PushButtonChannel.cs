using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class PushButtonChannel : SensorChannel<BaseLink>
{
	public float LongPressTime { get; set; }

	public PushButtonChannel(byte channelIndex, byte maxLinkCount, bool needsLinkUpdatePendingFlag)
		: base(channelIndex, maxLinkCount, needsLinkUpdatePendingFlag)
	{
		LongPressTime = 0.4f;
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		CheckLongPressTime(LongPressTime);
		ConfigurationLink configurationLink = new ConfigurationLink();
		double num = Math.Round(((double)LongPressTime - 0.3) * 10.0);
		configurationLink[ConfigKeys.LongPressTime] = (byte)num;
		return configurationLink;
	}

	private void CheckLongPressTime(float longPressTime)
	{
		if (longPressTime < 0.3f || longPressTime > 1.8f)
		{
			throw ConversionHelpers.CreateParameterException(ValidationErrorCode.LongPressTime, "LongPressTime", LongPressTime);
		}
	}
}
