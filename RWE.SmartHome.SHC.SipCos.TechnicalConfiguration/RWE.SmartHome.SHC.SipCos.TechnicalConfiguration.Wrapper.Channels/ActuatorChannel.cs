using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class ActuatorChannel<T> : BaseChannel<T> where T : ActuatorLink, new()
{
	public ActuatorChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount)
	{
	}

	public override ConfigurationChannel SaveConfiguration(IDictionary<byte, ConfigurationChannel> channels)
	{
		ConfigurationChannel configurationChannel = base.SaveConfiguration(channels);
		configurationChannel.ChannelType = ChannelType.ActuatorHasFlag;
		configurationChannel.SetLinkConfigUpdatePendingConfiguration = ActuatorLink.SetLinkConfigUpdatePendingConfiguration;
		configurationChannel.ClearLinkConfigUpdatePendingConfiguration = ActuatorLink.ClearLinkConfigUpdatePendingConfiguration;
		return configurationChannel;
	}

	public override void AddLink(LinkPartner linkPartner, T config)
	{
		base.AddLink(linkPartner, config);
	}

	public void ReplaceLink(LinkPartner linkPartner, T config)
	{
		if (base.Links.ContainsKey(linkPartner))
		{
			base.Links.Remove(linkPartner);
		}
		AddLink(linkPartner, config);
	}

	private static ProfileAction TriggerActionFromSwitchAction(SwitchAction switchAction)
	{
		return switchAction switch
		{
			SwitchAction.On => ProfileAction.On, 
			SwitchAction.Off => ProfileAction.Off, 
			SwitchAction.Toggle => ProfileAction.Toggle, 
			_ => ProfileAction.NoAction, 
		};
	}
}
