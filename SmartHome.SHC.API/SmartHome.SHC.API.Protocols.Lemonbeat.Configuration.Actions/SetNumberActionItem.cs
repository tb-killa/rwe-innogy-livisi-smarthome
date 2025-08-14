namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;

public class SetNumberActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public decimal Value { get; set; }

	public SetNumberActionItem()
	{
	}

	public SetNumberActionItem(uint valueId, decimal value)
	{
		ValueId = valueId;
		Value = value;
	}
}
