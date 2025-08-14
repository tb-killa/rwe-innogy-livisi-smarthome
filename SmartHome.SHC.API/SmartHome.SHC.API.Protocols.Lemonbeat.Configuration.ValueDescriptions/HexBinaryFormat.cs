namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;

public class HexBinaryFormat
{
	public uint MaximumLength { get; set; }

	public HexBinaryFormat()
	{
	}

	public HexBinaryFormat(uint maximumLength)
	{
		MaximumLength = maximumLength;
	}
}
