namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;

public class SetCalculationActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public uint CalculationId { get; set; }

	public SetCalculationActionItem()
	{
	}

	public SetCalculationActionItem(uint valueId, uint calculationId)
	{
		ValueId = valueId;
		CalculationId = calculationId;
	}
}
