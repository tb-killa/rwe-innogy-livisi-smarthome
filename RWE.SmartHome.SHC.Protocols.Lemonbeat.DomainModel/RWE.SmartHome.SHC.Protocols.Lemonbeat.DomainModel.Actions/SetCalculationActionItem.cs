namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

public class SetCalculationActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public uint CalculationId { get; set; }

	public uint? PartnerId { get; set; }

	public TransportModeType? TransportMode { get; set; }

	public override bool Equals(ActionItem other)
	{
		if (other is SetCalculationActionItem setCalculationActionItem && setCalculationActionItem.CalculationId == CalculationId && setCalculationActionItem.ValueId == ValueId)
		{
			return setCalculationActionItem.PartnerId == PartnerId;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ValueId.GetHashCode();
	}
}
