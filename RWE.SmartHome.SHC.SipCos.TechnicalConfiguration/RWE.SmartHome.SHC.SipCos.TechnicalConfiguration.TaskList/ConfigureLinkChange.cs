using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

internal class ConfigureLinkChange : ConfigurationChange
{
	private readonly byte parameterListIndex;

	private readonly ParameterList parameters;

	public ConfigureLinkChange(byte channel, LinkPartner partner, byte parameterListIndex, ParameterList parameters)
		: base(channel, partner)
	{
		this.parameterListIndex = parameterListIndex;
		this.parameters = parameters;
	}

	public override void ApplyTo(DeviceConfiguration config)
	{
		ConfigurationChannel configurationChannel = config.Channels[Channel];
		configurationChannel.Links[Partner].IsUnknownState = false;
		configurationChannel.Links[Partner][parameterListIndex].ApplyDiff(parameters);
	}

	public override string ToString(string addressStr)
	{
		return $"ConfigureLink: target=[{addressStr}:{Channel}] linkpartner=[{Partner.Address.ToReadable()}:{Partner.Channel}] parameters:[{parameterListIndex} | {parameters.ToReadable()}]";
	}
}
