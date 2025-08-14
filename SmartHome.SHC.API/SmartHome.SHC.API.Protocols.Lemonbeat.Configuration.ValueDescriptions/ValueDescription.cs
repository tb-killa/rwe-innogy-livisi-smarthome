namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;

public class ValueDescription
{
	public uint ValueId { get; set; }

	public string Name { get; set; }

	public uint Type { get; set; }

	public bool Readable { get; set; }

	public bool Writeable { get; set; }

	public bool Persistent { get; set; }

	public NumberFormat NumberFormat { get; set; }

	public StringFormat StringFormat { get; set; }

	public HexBinaryFormat HexBinaryFormat { get; set; }

	public uint? MinLogInterval { get; set; }

	public uint? MaxLogValues { get; set; }

	public bool IsVirtual { get; set; }

	public ValueDescription()
	{
	}

	public ValueDescription(byte id, byte type, string name, bool readable, bool writeable, bool persistent)
	{
		ValueId = id;
		Type = type;
		Name = name;
		Readable = readable;
		Writeable = writeable;
		Persistent = persistent;
	}
}
