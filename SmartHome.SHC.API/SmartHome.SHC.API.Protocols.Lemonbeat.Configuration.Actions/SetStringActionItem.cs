namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;

public class SetStringActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public string Value { get; set; }

	public SetStringActionItem()
	{
	}

	public SetStringActionItem(uint valueId, string value)
	{
		ValueId = valueId;
		Value = value;
	}
}
