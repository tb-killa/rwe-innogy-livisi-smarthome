namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;

public class SetHexBinaryActionItem : ActionItem
{
	public uint ValueId { get; set; }

	public byte[] Value { get; set; }

	public SetHexBinaryActionItem()
	{
	}

	public SetHexBinaryActionItem(uint valueId, byte[] value)
	{
		ValueId = valueId;
		Value = value;
	}
}
