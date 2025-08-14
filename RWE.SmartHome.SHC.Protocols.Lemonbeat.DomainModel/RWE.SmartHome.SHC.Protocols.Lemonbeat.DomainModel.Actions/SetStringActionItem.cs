namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

public class SetStringActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public string Value { get; set; }

	public uint? PartnerId { get; set; }

	public TransportModeType? TransportMode { get; set; }

	public override bool Equals(ActionItem other)
	{
		if (other is SetStringActionItem setStringActionItem && setStringActionItem.Value == Value && setStringActionItem.ValueId == ValueId)
		{
			return setStringActionItem.PartnerId == PartnerId;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ValueId.GetHashCode();
	}
}
