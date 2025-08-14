using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

internal class CreateLinkChange : ConfigurationChange
{
	public CreateLinkChange(byte channel, LinkPartner partner)
		: base(channel, partner)
	{
	}

	public override void ApplyTo(DeviceConfiguration config)
	{
		ConfigurationChannel configurationChannel = config.Channels[Channel];
		configurationChannel.Links[Partner].IsUnknownState = false;
	}

	public override string ToString(string addressStr)
	{
		return $"CreateLink: target=[{addressStr}:{Channel}] linkpartner=[{Partner.Address.ToReadable()}:{Partner.Channel}]";
	}
}
