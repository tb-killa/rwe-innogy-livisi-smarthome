using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class SensorChannel<T> : BaseChannel<T> where T : BaseLink, new()
{
	private readonly bool needsFlag;

	protected SensorChannel(byte channelIndex, byte maxLinkCount, bool needsLinkUpdatePendingFlag)
		: base(channelIndex, maxLinkCount)
	{
		needsFlag = needsLinkUpdatePendingFlag;
	}

	public override ConfigurationChannel SaveConfiguration(IDictionary<byte, ConfigurationChannel> channels)
	{
		ConfigurationChannel configurationChannel = base.SaveConfiguration(channels);
		configurationChannel.ChannelType = (needsFlag ? ChannelType.SensorNeedsFlag : ChannelType.MainsPoweredSensor);
		return configurationChannel;
	}
}
