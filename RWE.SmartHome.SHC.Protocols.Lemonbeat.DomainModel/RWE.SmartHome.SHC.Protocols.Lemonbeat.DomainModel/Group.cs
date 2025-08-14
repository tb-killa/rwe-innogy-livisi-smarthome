using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class Group : ConfigurationItem
{
	public uint[] PartnerIds { get; set; }

	public override bool Equals(ConfigurationItem other)
	{
		if (other is Group obj && base.Id == obj.Id)
		{
			return PartnerIds.Compare(obj.PartnerIds);
		}
		return false;
	}
}
