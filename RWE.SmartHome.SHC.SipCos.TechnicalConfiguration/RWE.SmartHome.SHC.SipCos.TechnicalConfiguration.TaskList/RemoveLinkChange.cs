using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

internal class RemoveLinkChange : ConfigurationChange
{
	public RemoveLinkChange(byte channel, LinkPartner partner)
		: base(channel, partner)
	{
	}

	public override void ApplyTo(DeviceConfiguration config)
	{
		ConfigurationChannel configurationChannel = config.Channels[Channel];
		configurationChannel.Links.Remove(Partner);
	}

	public override string ToString(string addressStr)
	{
		return $"DeleteLink: target=[{addressStr}:{Channel}] linkpartner=[{Partner.Address.ToReadable()}:{Partner.Channel}]";
	}
}
