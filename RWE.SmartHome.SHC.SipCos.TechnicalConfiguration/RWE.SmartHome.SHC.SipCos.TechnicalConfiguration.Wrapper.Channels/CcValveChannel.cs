using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class CcValveChannel : BaseChannel<BaseLink>
{
	public ValveType? ValveType { get; set; }

	public ControlMode? ControlMode { get; set; }

	public CcValveChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount)
	{
	}

	public override ConfigurationChannel SaveConfiguration(IDictionary<byte, ConfigurationChannel> channels)
	{
		if (!ValveType.HasValue)
		{
			return base.SaveConfiguration(channels);
		}
		ConfigurationChannel configurationChannel = channels[base.ChannelIndex];
		ConfigurationLink configurationLink = new ConfigurationLink();
		configurationLink[ConfigKeys.CcValveType] = (byte)ValveType.Value;
		if (ControlMode.HasValue)
		{
			configurationLink[ConfigKeys.CcControlMode] = (byte)ControlMode.Value;
		}
		configurationChannel.Links[LinkPartner.Empty] = configurationLink;
		foreach (KeyValuePair<LinkPartner, BaseLink> link in base.Links)
		{
			ConfigurationLink configurationLink2 = new ConfigurationLink();
			link.Value.SaveConfiguration(configurationLink2);
			configurationChannel.Links.Add(link.Key, configurationLink2);
		}
		if (base.Links.Count > base.MaxLinkCount)
		{
			throw GetChannelLinkCountExceededException();
		}
		return configurationChannel;
	}
}
