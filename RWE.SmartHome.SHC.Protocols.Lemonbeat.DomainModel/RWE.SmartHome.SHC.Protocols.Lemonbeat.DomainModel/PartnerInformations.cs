using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class PartnerInformations
{
	public IList<Partner> Partners { get; set; }

	public IList<Group> Groups { get; set; }

	public PartnerInformations()
	{
		Partners = new List<Partner>();
		Groups = new List<Group>();
	}
}
