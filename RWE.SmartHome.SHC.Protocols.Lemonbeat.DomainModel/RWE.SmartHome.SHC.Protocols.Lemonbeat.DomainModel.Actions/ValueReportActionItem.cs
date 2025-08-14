namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

public class ValueReportActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public uint? PartnerId { get; set; }

	public TransportModeType? TransportMode { get; set; }

	public ValueReportActionType ActionType { get; set; }

	public override bool Equals(ActionItem other)
	{
		if (other is ValueReportActionItem { PartnerId: var partnerId } valueReportActionItem)
		{
			uint? partnerId2 = PartnerId;
			if (partnerId.GetValueOrDefault() == partnerId2.GetValueOrDefault() && partnerId.HasValue == partnerId2.HasValue && valueReportActionItem.ValueId == ValueId && valueReportActionItem.ActionType == ActionType)
			{
				return TransportMode == valueReportActionItem.TransportMode;
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ValueId.GetHashCode();
	}
}
