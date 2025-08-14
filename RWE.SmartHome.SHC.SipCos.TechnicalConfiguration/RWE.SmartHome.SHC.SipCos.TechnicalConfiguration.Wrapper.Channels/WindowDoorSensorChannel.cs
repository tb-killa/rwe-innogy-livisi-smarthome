using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class WindowDoorSensorChannel : SensorChannel<BaseLink>
{
	public int EventFilterTime { get; set; }

	public WindowDoorSensorChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount, needsLinkUpdatePendingFlag: true)
	{
		EventFilterTime = 5;
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		CheckEventFilterTime(EventFilterTime);
		ConfigurationLink configurationLink = new ConfigurationLink();
		configurationLink[ConfigKeys.EventFiltertime] = ConvertFilterTime(EventFilterTime);
		return configurationLink;
	}

	private static byte ConvertFilterTime(int eventFilterTime)
	{
		return (eventFilterTime > 127) ? ((byte)(128.0 + Math.Round((float)eventFilterTime * (1f / 60f)))) : ((byte)eventFilterTime);
	}

	private static void CheckEventFilterTime(int eventFilterTime)
	{
		if (eventFilterTime < 0 || eventFilterTime >= 7650)
		{
			throw ConversionHelpers.CreateParameterException(ValidationErrorCode.TimeSpan, "EventFilterTime", eventFilterTime);
		}
	}
}
