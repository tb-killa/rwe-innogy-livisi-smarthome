namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

public class SetNumberActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public double Value { get; set; }

	public uint? PartnerId { get; set; }

	public TransportModeType? TransportMode { get; set; }

	public override bool Equals(ActionItem other)
	{
		if (other is SetNumberActionItem setNumberActionItem && setNumberActionItem.Value == Value && setNumberActionItem.ValueId == ValueId)
		{
			return setNumberActionItem.PartnerId == PartnerId;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ValueId.GetHashCode();
	}
}
