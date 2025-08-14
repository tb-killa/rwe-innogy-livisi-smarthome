namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

public class SetHexBinaryActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public byte[] Value { get; set; }

	public uint? PartnerId { get; set; }

	public TransportModeType? TransportMode { get; set; }

	public override bool Equals(ActionItem other)
	{
		if (other is SetHexBinaryActionItem setHexBinaryActionItem && setHexBinaryActionItem.Value == Value && setHexBinaryActionItem.ValueId == ValueId)
		{
			return setHexBinaryActionItem.PartnerId == PartnerId;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ValueId.GetHashCode();
	}
}
