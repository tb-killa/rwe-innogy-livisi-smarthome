namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;

public class ValueReportActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public PartnerInformation Partner { get; set; }

	public ValueReportActionType ActionType { get; set; }

	public Transport? TransportMode { get; set; }

	public ValueReportActionItem()
	{
	}

	public ValueReportActionItem(uint valueId, PartnerInformation partner, ValueReportActionType type, Transport? transportMode)
	{
		ValueId = valueId;
		Partner = partner;
		ActionType = type;
		TransportMode = transportMode;
	}
}
