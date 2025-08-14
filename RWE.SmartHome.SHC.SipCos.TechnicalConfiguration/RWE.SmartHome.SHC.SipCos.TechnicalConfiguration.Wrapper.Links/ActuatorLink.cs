using System;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

public class ActuatorLink : BaseLink
{
	public bool ConfigurationPending { get; set; }

	public static ConfigurationLink SetLinkConfigUpdatePendingConfiguration { get; private set; }

	public static ConfigurationLink ClearLinkConfigUpdatePendingConfiguration { get; private set; }

	public ToggleBehavior ActuatorToggleBehavior { get; set; }

	static ActuatorLink()
	{
		SetLinkConfigUpdatePendingConfiguration = ConfigureLinkUpdatePending(new ConfigurationLink(), doUpdate: true);
		ClearLinkConfigUpdatePendingConfiguration = ConfigureLinkUpdatePending(new ConfigurationLink(), doUpdate: false);
	}

	public override void SaveConfiguration(ConfigurationLink link)
	{
		ConfigureLinkUpdatePending(link, ConfigurationPending);
	}

	private static ConfigurationLink ConfigureLinkUpdatePending(ConfigurationLink link, bool doUpdate)
	{
		link[ConfigKeys.LinkConfigUpdatePending] = Convert.ToByte(doUpdate);
		return link;
	}
}
