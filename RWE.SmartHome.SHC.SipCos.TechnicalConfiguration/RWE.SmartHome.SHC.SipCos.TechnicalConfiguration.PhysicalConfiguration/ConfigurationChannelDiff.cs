using System.Collections.Generic;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

public class ConfigurationChannelDiff
{
	private readonly Dictionary<LinkPartner, ConfigurationLink> toCreate = new Dictionary<LinkPartner, ConfigurationLink>();

	private readonly Dictionary<LinkPartner, ConfigurationLink> toChange = new Dictionary<LinkPartner, ConfigurationLink>();

	private readonly List<LinkPartner> toDelete = new List<LinkPartner>();

	public IDictionary<LinkPartner, ConfigurationLink> ToCreate => toCreate;

	public IDictionary<LinkPartner, ConfigurationLink> ToChange => toChange;

	public IList<LinkPartner> ToDelete => toDelete;
}
