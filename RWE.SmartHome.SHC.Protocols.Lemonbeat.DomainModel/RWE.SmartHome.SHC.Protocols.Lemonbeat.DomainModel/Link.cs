namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class Link : ConfigurationItem
{
	public uint PartnerId { get; set; }

	public uint MyValueId { get; set; }

	public override bool Equals(ConfigurationItem other)
	{
		if (other is Link link && base.Id == link.Id && PartnerId == link.PartnerId)
		{
			return MyValueId == link.MyValueId;
		}
		return false;
	}
}
